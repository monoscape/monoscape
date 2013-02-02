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
 *  2011/11/10 Created.
 */

using System;
using Monoscape.LoadBalancerController.Api.Services.Dashboard;
using Monoscape.Common;
using Monoscape.Common.Model;
using Monoscape.LoadBalancerController.Runtime;
using Monoscape.LoadBalancerController.Api.Services.Dashboard.Model;

namespace Monoscape.LoadBalancerController.Services.Dashboard
{
    public class LbDashboardService : MonoscapeService, ILbDashboardService
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

        public LbGetRoutingMeshHistoryResponse GetRoutingMeshHistory(LbGetRoutingMeshHistoryRequest request)
        {
            try
            {
                Log.Debug(this, "GetRoutingMeshHistory()");

                Authenticate(request);
                LbGetRoutingMeshHistoryResponse response = new LbGetRoutingMeshHistoryResponse();
                response.RoutingMeshHistory = Database.GetInstance().RoutingMeshHistory;
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