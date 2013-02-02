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

using Monoscape.ApplicationGridController.Api.Services.Dashboard;
using Monoscape.Common;
using System.ServiceModel;
using Monoscape.LoadBalancerController.Api.Services.Dashboard;
using Monoscape.CloudController.Api.Services.Dashboard;

namespace Monoscape.Dashboard.Runtime
{
    internal static class EndPoints
    {
        public static IApDashboardService ApDashboardService
        {
            get
            {
                var binding = MonoscapeServiceHost.GetBinding();
                var address = new EndpointAddress(Settings.ApplicationGridEndPointURL);
                ChannelFactory<IApDashboardService> factory = new ChannelFactory<IApDashboardService>(binding, address);
                return factory.CreateChannel();
            }
        }

        public static ILbDashboardService LbDashboardService
        {
            get
            {
                var binding = MonoscapeServiceHost.GetBinding();
                var address = new EndpointAddress(Settings.LoadBalancerEndPointURL);
                ChannelFactory<ILbDashboardService> factory = new ChannelFactory<ILbDashboardService>(binding, address);
                return factory.CreateChannel();
            }
        }

        public static ICcDashboardService CcDashboardService
        {
            get
            {
                var binding = MonoscapeServiceHost.GetBinding();
                var address = new EndpointAddress(Settings.CloudControllerEndPointURL);
                ChannelFactory<ICcDashboardService> factory = new ChannelFactory<ICcDashboardService>(binding, address);
                return factory.CreateChannel();
            }
        }
    }
}