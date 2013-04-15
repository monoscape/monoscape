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
using System.Web.Mvc;
using Monoscape.Dashboard.Runtime;
using Monoscape.Common.Model;
using Monoscape.LoadBalancerController.Api.Services.Dashboard.Model;

namespace Monoscape.Dashboard.Controllers
{
    public class LoadBalancerController : AbstractController
    {
        //
        // GET: /LoadBalancerController/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RequestQueue()
        {
            try
            {
                LbGetRequestQueueRequest request = new LbGetRequestQueueRequest(Settings.Credentials);
                request.RequestType = RequestType.RequestQueue;
                LbGetRequestQueueResponse response = EndPoints.LbDashboardService.GetRequestQueue(request);
                var list = new List<ApplicationHttpRequest>();
                if (response.RequestQueue != null)
                    list.AddRange(response.RequestQueue);
                return View(list);
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }

        public ActionResult RequestHistory()
        {
            try
            {
                LbGetRequestQueueRequest request = new LbGetRequestQueueRequest(Settings.Credentials);
                request.RequestType = RequestType.AllRequests;
                LbGetRequestQueueResponse response = EndPoints.LbDashboardService.GetRequestQueue(request);
                var list = new List<ApplicationHttpRequest>();
                if (response.RequestQueue != null)
                    list.AddRange(response.RequestQueue);
                return View(list);
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }

        public ActionResult RoutingMeshHistory()
        {
            try
            {
                LbGetRoutingMeshHistoryRequest request = new LbGetRoutingMeshHistoryRequest(Settings.Credentials);                
                LbGetRoutingMeshHistoryResponse response = EndPoints.LbDashboardService.GetRoutingMeshHistory(request);
                List<ApplicationInstance> list = new List<ApplicationInstance>();
                if (response.RoutingMeshHistory != null)
                    list.AddRange(response.RoutingMeshHistory);
                return View(list);
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }
    }
}
