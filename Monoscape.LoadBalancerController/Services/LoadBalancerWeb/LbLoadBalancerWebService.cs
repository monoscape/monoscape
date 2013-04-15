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
using System.Web;
using Monoscape.LoadBalancerController.Api.Services.ApplicationGrid;
using Monoscape.Common;
using Monoscape.Common.Model;
using Monoscape.LoadBalancerController.Runtime;
using Monoscape.LoadBalancerController.Api.Services.LoadBalancerWeb;
using Monoscape.LoadBalancerController.Api.Services.LoadBalancerWeb.Model;
using Monoscape.Common.Models;

namespace Monoscape.LoadBalancerController.Services.LoadBalancerWeb
{
    public class LbLoadBalancerWebService : MonoscapeService, ILbLoadBalancerWebService
    {
        #region Protected Properties
        protected override MonoscapeCredentials Credentials
        {
            get
            {
                return Settings.Credentials;
            }
        }
        #endregion

        public LbGetRoutingMeshResponse GetRoutingMesh(LbGetRoutingMeshRequest request)
        {
            try
            {
                Log.Debug(this, "GetRoutingMesh()");

                Authenticate(request);
                LbGetRoutingMeshResponse response = new LbGetRoutingMeshResponse();
                if ((!string.IsNullOrEmpty(request.ApplicationName)) && (!string.IsNullOrEmpty(request.TenantName)))
                {
                    response.ApplicationsInstances = Database.GetInstance().RoutingMesh.FindAll(x => (x.ApplicationName.ToLower().Equals(request.ApplicationName.ToLower())) &&
                                                                                                     (x.Tenant.Name.ToLower().Equals(request.TenantName.ToLower())));
                }
                return response;
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        public LbAddRequestToQueueResponse AddRequestToQueue(LbAddRequestToQueueRequest request)
        {
            try
            {
                Log.Debug(this, "AddRequestToQueue()");

                Authenticate(request);
                LbAddRequestToQueueResponse response = new LbAddRequestToQueueResponse();
                if (request.ApplicationRequest != null)
                {
                    request.ApplicationRequest.Id = FindNextRequestId();
                    Database.GetInstance().RequestQueue.Add(request.ApplicationRequest);
                    Database.GetInstance().RequestQueueHistory.Add(request.ApplicationRequest);
                    response.RequestId = request.ApplicationRequest.Id;
                    response.Added = true;

                    IncrementRequestCount(request.ApplicationRequest.NodeId, request.ApplicationRequest.ApplicationId, request.ApplicationRequest.InstanceId);
                    Log.Debug(this, "Added request to queue: " + request.ApplicationRequest.Url);
                }
                return response;
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        private int FindNextRequestId()
        {
            if (Database.GetInstance().RequestQueue.Count > 0)
                return Database.GetInstance().RequestQueue.Max(x => x.Id) + 1;
            else
                return 1;
        }

        public LbRemoveRequestFromQueueResponse RemoveRequestFromQueue(LbRemoveRequestFromQueueRequest request)
        {
            try
            {
                Log.Debug(this, "RemoveRequestFromQueue()");

                Authenticate(request);
                LbRemoveRequestFromQueueResponse response = new LbRemoveRequestFromQueueResponse();
                var req = Database.GetInstance().RequestQueue.Find(x => x.Id == request.RequestId);
                if (req != null)
                {
                    Database.GetInstance().RequestQueue.Remove(req);
                    response.Removed = true;

                    DecrementRequestCount(req.NodeId, req.ApplicationId, req.InstanceId);
                    Log.Debug(this, "Removed request from queue: " + request.RequestId + " - " + req.Url);
                }
                else
                {
                    Log.Debug(this, "Could not find request in queue: " + request.RequestId);
                }
                return response;
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        private void IncrementRequestCount(int nodeId, int applicationId, int instanceId)
        {
            try
            {
                Log.Debug(this, "IncrementRequestCount()");
                ApplicationInstance instance = Database.GetInstance().RoutingMesh.Find(x => (x.NodeId == nodeId) && (x.ApplicationId == applicationId) && (x.Id == instanceId));
                if (instance != null)
                {
                    instance.RequestCount = instance.RequestCount + 1;
                    Log.Info(this, "Application ID: " + instance.ApplicationId + " Instance ID: " + instance.Id + " Request Count: " + instance.RequestCount);
                }
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        public void DecrementRequestCount(int nodeId, int applicationId, int instanceId)
        {
            try
            {
                Log.Debug(this, "DecrementRequestCount()");
                ApplicationInstance instance = Database.GetInstance().RoutingMesh.Find(x => (x.NodeId == nodeId) && (x.ApplicationId == applicationId) && (x.Id == instanceId));
                if (instance != null)
                {
                    if (instance.RequestCount > 0)
                        instance.RequestCount = instance.RequestCount - 1;
                    Log.Info(this, "Application ID: " + instance.ApplicationId + " Instance ID: " + instance.Id + " Request Count: " + instance.RequestCount);
                }
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }
    }
}