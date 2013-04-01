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
using Monoscape.LoadBalancerController.Api.Services.ApplicationGrid;
using Monoscape.Common;
using Monoscape.Common.Model;
using Monoscape.LoadBalancerController.Runtime;
using Monoscape.LoadBalancerController.Api.Services.ApplicationGrid.Model;

namespace Monoscape.LoadBalancerController.Services.ApplicationGrid
{
    public class LbApplicationGridService : MonoscapeService, ILbApplicationGridService
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

        public LbAddApplicationInstancesResponse AddApplicationInstances(LbAddApplicationInstancesRequest request)
        {
            try
            {
                Log.Debug(this, "AddApplicationInstances()");

                Authenticate(request);
                foreach (ApplicationInstance instance in request.AppInstances)
                {
                    if (!Database.GetInstance().RoutingMesh.Exists(x => (x.NodeId == instance.NodeId) && (x.ApplicationId == instance.ApplicationId) && (x.Id == instance.Id)))
                    {
                        Database.GetInstance().RoutingMesh.Add(instance);
                        Database.GetInstance().RoutingMeshHistory.Add(instance.Clone());
                        Log.Debug(this, "Added application instance_: " + instance);
                    }
                }
                LbAddApplicationInstancesResponse response = new LbAddApplicationInstancesResponse();
                return response;
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        public LbRemoveApplicationInstanceResponse RemoveApplicationInstances(LbRemoveApplicationInstanceRequest request)
        {
            try
            {
                Log.Debug(this, "RemoveApplicationInstances()");

                Authenticate(request);
                if((request.NodeId != -1) && (request.ApplicationId != -1) && (request.InstanceId != -1))
                {
                    ApplicationInstance instance = Database.GetInstance().RoutingMesh.Find(x => (x.NodeId == request.NodeId) && (x.ApplicationId == request.ApplicationId) && (x.Id == request.InstanceId));
                    if (instance != null)                    
                        RemoveApplicationInstance_(instance);                    
                }
                else if ((request.NodeId != -1) && (request.ApplicationId == -1) && (request.InstanceId == -1))
                {
                    Log.Debug(this, "Removing all application instances of node: " + request.NodeId);
                    List<ApplicationInstance> toRemove = Database.GetInstance().RoutingMesh.FindAll(x => x.NodeId == request.NodeId);
                    foreach (ApplicationInstance instance in toRemove)                    
                        RemoveApplicationInstance_(instance);                    
                }
                LbRemoveApplicationInstanceResponse response = new LbRemoveApplicationInstanceResponse();
                return response;
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        private void RemoveApplicationInstance_(ApplicationInstance instance_)
        {
            ApplicationInstance clone = instance_.Clone();
            Database.GetInstance().RoutingMesh.Remove(instance_);
            clone.CreatedTime = DateTime.Now;
            clone.State = "Stopped";
            Database.GetInstance().RoutingMeshHistory.Add(clone);
        }

        public LbGetApplicationInstancesResponse GetApplicationInstances(LbGetApplicationInstancesRequest request)
        {
            try
            {
                Log.Debug(this, "GetApplicationInstances()");

                Authenticate(request);
                LbGetApplicationInstancesResponse response = new LbGetApplicationInstancesResponse();
                List<ApplicationInstance> instances = Database.GetInstance().RoutingMesh.FindAll(x => x.ApplicationId == request.ApplicationId);
                if (instances != null)
                {
                    if (request.NodeId == -1)
                        response.ApplicationInstances = instances;
                    else
                        response.ApplicationInstances = instances.FindAll(x => x.NodeId == request.NodeId);
                }
                else
                    response.ApplicationInstances = new List<ApplicationInstance>();
                return response;
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        public LbGetApplicationScaleResponse GetApplicationScale(LbGetApplicationScaleRequest request)
        {
            try
            {
                Log.Debug(this, "GetApplicationScale()");

                Authenticate(request);
                LbGetApplicationScaleResponse response = new LbGetApplicationScaleResponse();
                List<ApplicationInstance> instances = Database.GetInstance().RoutingMesh.FindAll(x => (x.ApplicationId == request.ApplicationId) && (x.Tenant.Id == request.TenantId));
                if (instances != null)
                    response.Scale = instances.Count;
                else
                    response.Scale = -1;
                return response;
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        public LbGetRequestQueueResponse GetRequestQueue(LbGetRequestQueueRequest request)
        {
            try
            {
                Log.Debug(this, "GetRequestQueue()");

                Authenticate(request);
                LbGetRequestQueueResponse response = new LbGetRequestQueueResponse();
                if (request.RequestType == RequestType.RequestQueue)
                    response.RequestQueue = Database.GetInstance().RequestQueue;
                else if (request.RequestType == RequestType.AllRequests)
                    response.RequestQueue = Database.GetInstance().RequestQueueHistory;
                return response;
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }
    }
}