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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Monoscape.Common;
using System.ServiceModel;
using Monoscape.LoadBalancerController.Api.Services.LoadBalancerWeb;

namespace Monoscape.LoadBalancerController.Web.Runtime
{
    internal static class EndPoints
    {
        //private static Object threadLock = new Object();

        public static ILbLoadBalancerWebService LoadBalancerWebService
        {
            get
            {
                //Log.Debug(typeof(EndPoints), "Waiting for thread lock...");
                //lock (threadLock)
                //{
                    //Log.Debug(typeof(EndPoints), "Lock acquired");
                    var binding = MonoscapeServiceHost.GetBinding();
                    var address = new EndpointAddress(Settings.LoadBalancerEndPointURL);
                    ChannelFactory<ILbLoadBalancerWebService> factory = new ChannelFactory<ILbLoadBalancerWebService>(binding, address);
                    return factory.CreateChannel();
                //}
            }
        }
    }
}
