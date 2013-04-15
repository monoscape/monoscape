/*
 *  Copyright 2013 Monoscape
 *
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 *
 *  History: 
 *  2011/11/10 Imesh Gunaratne <imesh@monoscape.org> Created.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using Monoscape.ApplicationGridController.Api.Services.Dashboard;
using Monoscape.ApplicationGridController.Api.Services.Dashboard.Model;
using Monoscape.ApplicationGridController.Runtime;
using Monoscape.Common;
using Monoscape.Common.Exceptions;
using Monoscape.Common.Model;
using Monoscape.LoadBalancerController.Api.Services.ApplicationGrid.Model;

namespace Monoscape.ApplicationGridController.Scaling
{
    internal class ScalingManager
    {
        private int monitoringInterval = 5000; // Monitoring interval 5 sec

        public void MonitorRequestQueue()
        {
            Log.Info(this, "Scaling Manager started");            
            while (Thread.CurrentThread.IsAlive)
            {
                LbGetRequestQueueResponse response = null;
                try
                {
                    Log.Info(this, "Checking request queue status...");
                    LbGetRequestQueueRequest request = new LbGetRequestQueueRequest(Settings.Credentials);
                    response = EndPoints.GetLbApplicationGridService().GetRequestQueue(request);
                }
                catch (Exception e)
                {
                    Log.Error(this, e);
                }

                try
                {
                    if (response != null)
                        ScaleApplications(response.RequestQueue);
                    Thread.Sleep(monitoringInterval);
                }
                catch (Exception e)
                {
                    Log.Error(this, e);
                }
            }
            Log.Info(this, "Scaling Manager stopped");
        }

        private void ScaleApplications(List<ApplicationHttpRequest> requestQueue)
        {
            if ((requestQueue == null) || (requestQueue.Count == 0))
            {
                // No requests found in the queue
                // Stop all extra application instances
                foreach (Application application in Database.GetInstance().Applications)
                {
                    foreach (Tenant tenant in application.Tenants)
                    {
                        int reqScale = 1;
                        int currentScale = FindCurrentScale(application.Id, tenant.Id);                        
                        int diff = currentScale - reqScale;
                        if (diff == -1)
                        {
                            // Scale Up: Start initial instance of each application tenant
                            if (ScaleUp(application.Id, tenant.Name, 1))
                                AddScalingHistory(application.Id, tenant.Id, 0, reqScale);
                        }
                        else if (diff > 0)
                        {
                            // Scale Down: Stop extra application instances
                            if(ScaleDown(application.Id, tenant.Name, diff))
                                AddScalingHistory(application.Id, tenant.Id, 0, reqScale);
                        }
                    }
                }
            }
            else
            {
                foreach (ApplicationHttpRequest request in requestQueue)
                {
                    Application app = Database.GetInstance().Applications.Find(x => x.Id == request.ApplicationId);
                    if (app != null)
                    {
                        Tenant tenant = app.Tenants.Find(y => y.Id.Equals(request.TenantId));
                        if (tenant != null)
                        {
                            // Check tenant upper scale limit
                            int upperScaleLimit = tenant.UpperScaleLimit;
                            // Check scaling factor for the application tenant
                            // Scaling Factor is the number of requests served by an application tenant instance
                            int scalingFactor = tenant.ScalingFactor;
                            // Check request count for the application tenant
                            int requestCount = requestQueue.Count(x => x.ApplicationId == request.ApplicationId);

                            if (requestCount > scalingFactor)
                            {
                                int reqScale = (int)Math.Ceiling((double)requestCount / scalingFactor);
                                int currentScale = FindCurrentScale(request.ApplicationId, request.TenantId);

                                if (reqScale > currentScale)
                                {
                                    if (reqScale > upperScaleLimit)
                                        reqScale = upperScaleLimit;
                                    
                                    // Scale Up: Start new application instances
                                    int diff = reqScale - currentScale;
                                    if (diff > 0)
                                    {
                                        if (ScaleUp(request.ApplicationId, tenant.Name, diff))
                                            AddScalingHistory(request, requestCount, reqScale);
                                    }
                                }
                                else
                                {
                                    // Scale Down: Stop extra application instances
                                    int diff = currentScale - reqScale;
                                    if (diff > 0)
                                    {
                                        if (ScaleDown(request.ApplicationId, tenant.Name, diff))
                                            AddScalingHistory(request, requestCount, reqScale);
                                    }
                                }
                            }
                            else
                            {
                                int reqScale = 1;
                                int currentScale = FindCurrentScale(request.ApplicationId, request.TenantId);
                                // Scale Down: Stop extra application instances
                                int diff = currentScale - reqScale;
                                if (diff > 0)
                                {
                                    if (ScaleDown(request.ApplicationId, tenant.Name, diff))
                                        AddScalingHistory(request, requestCount, reqScale);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void AddScalingHistory(ApplicationHttpRequest request, int requestCount, int reqScale)
        {
            AddScalingHistory(request.ApplicationId, request.TenantId, requestCount, reqScale);
        }

        private static void AddScalingHistory(int applicationId, int tenantId, int requestCount, int reqScale)
        {
            ScalingHistoryItem item = new ScalingHistoryItem();
            item.Time = DateTime.Now;
            item.ApplicationId = applicationId;
            item.TenantId = tenantId;
            item.RequestCount = requestCount;
            item.Scale = reqScale;
            Database.GetInstance().ScalingHistory.Add(item);
        }

        private bool ScaleUp(int applicationId, string tenantName, int scale)
        {
            Log.Info(this, "Scaling up application: " + applicationId);
            ApStartApplicationRequest request = new ApStartApplicationRequest(Settings.Credentials);
            request.ApplicationId = applicationId;
            request.TenantName = tenantName;
            request.NumberOfInstances = scale;
            GetApDashboardService().StartApplication(request);
            return true;
        }

        private bool ScaleDown(int applicationId, string tenantName, int scale)
        {
            Log.Info(this, "Scaling down application: " + applicationId);
            LbGetApplicationInstancesRequest request_ = new LbGetApplicationInstancesRequest(Settings.Credentials);
            request_.NodeId = -1;
            request_.ApplicationId = applicationId;
            LbGetApplicationInstancesResponse response_ = EndPoints.GetLbApplicationGridService().GetApplicationInstances(request_);
            List<ApplicationInstance> list = response_.ApplicationInstances.OrderBy(x => x.Id).ToList();

            if ((list != null) && (list.Count >= scale))
            {
                // Stop idling application instances
                List<ApplicationInstance> idlingInstances = list.FindAll(x => x.RequestCount == 0);                
                // Keep one instance alive
                if (idlingInstances.Count == list.Count)
                    idlingInstances.Remove(idlingInstances.Last());

                if (idlingInstances.Count > 0)
                {
                    foreach (ApplicationInstance instance in idlingInstances)
                    {
                        ApStopApplicationInstanceRequest request = new ApStopApplicationInstanceRequest(Settings.Credentials);
                        request.NodeId = instance.NodeId;
                        request.ApplicationId = instance.ApplicationId;
                        request.InstanceId = instance.Id;
                        GetApDashboardService().StopApplicationInstance(request);
                    }
                    return true;
                }
            }
            return false;
        }

        private int FindCurrentScale(int applicationId, int tenantId)
        {
            LbGetApplicationScaleRequest request = new LbGetApplicationScaleRequest(Settings.Credentials);
            request.ApplicationId = applicationId;
            request.TenantId = tenantId;
            LbGetApplicationScaleResponse response = EndPoints.GetLbApplicationGridService().GetApplicationScale(request);
            return response.Scale;
        }

        public IApDashboardService GetApDashboardService()
        {
            try
            {
                string serviceUrl = Settings.DashboardServiceURL;
                Log.Debug(typeof(EndPoints), "Creating IApDashboardService channel: " + serviceUrl);
                var binding = MonoscapeServiceHost.GetBinding();
                var address = new EndpointAddress(serviceUrl);
                ChannelFactory<IApDashboardService> factory = new ChannelFactory<IApDashboardService>(binding, address);
                IApDashboardService channel = factory.CreateChannel();
                return channel;
            }
            catch (Exception e)
            {
                MonoscapeException me = new MonoscapeException("Could not connect to the Load Balancer", e);
                Log.Error(typeof(EndPoints), me);
                throw me;
            }
        }
    }
}
