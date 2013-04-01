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
using System.Text;
using Monoscape.Common.Model;
using System.Web;

namespace Monoscape.LoadBalancerController.Api
{
    public static class LoadBalancerControllerUtil
    {
        public static ApplicationRequest ConvertRequest(HttpRequest httpRequest, int nodeId, int applicationId, int instanceId, int tenantId)
        {
            ApplicationRequest request = new ApplicationRequest();
            request.Time = DateTime.Now;
            request.RawUrl = httpRequest.RawUrl;
            request.RequestType = httpRequest.RequestType;
            request.Url = httpRequest.Url;
            request.UserAgent = httpRequest.UserAgent;
            request.UserHostAddress = httpRequest.UserHostAddress;
            request.UserHostName = httpRequest.UserHostName;
            request.NodeId = nodeId;
            request.ApplicationId = applicationId;
            request.InstanceId = instanceId;
            request.TenantId = tenantId;
            return request;
        }
    }
}
