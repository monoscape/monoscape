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
using Monoscape.LoadBalancerController.Web.Runtime;
using Monoscape.Common;
using Monoscape.LoadBalancerController.Api.Services.LoadBalancerWeb.Model;

namespace Monoscape.LoadBalancerController.Web
{
    internal class LoadBalancerControllerWebUtil
    {
        public static ApplicationInstance GetApplicationInstance(string tenantName, string applicationName)
        {
            LbGetRoutingMeshRequest request = new LbGetRoutingMeshRequest(Settings.Credentials);
            request.ApplicationName = Decode(applicationName);
            request.TenantName = Decode(tenantName);
            LbGetRoutingMeshResponse response = EndPoints.LoadBalancerWebService.GetRoutingMesh(request);

            if ((response != null) && (response.ApplicationsInstances != null) && (response.ApplicationsInstances.Count > 0))
            {
                return FindNextAvailableInstance(response.ApplicationsInstances);
            }
            return null;
        }

        private static string Decode(string applicationName)
        {
            return applicationName.Replace("-", " ");
        }

        private static ApplicationInstance FindNextAvailableInstance(List<ApplicationInstance> list)
        {
            Log.Info(typeof(LoadBalancerControllerWebUtil), "FindNextAvailableInstance()");
            Log.Info(typeof(LoadBalancerControllerWebUtil), "Instances: " + list.Count);

            ApplicationInstance instance = list.Min();
            Log.Debug(typeof(LoadBalancerControllerWebUtil), "Selected: " + instance.ToString());
            return instance;            
        }
    }
}
