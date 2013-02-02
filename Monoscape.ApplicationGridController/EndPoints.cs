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
using System.Text;
using Monoscape.LoadBalancerController.Api.Services.ApplicationGrid;
using Monoscape.NodeController.Api.Services.ApplicationGrid;
using Monoscape.Common;
using System.ServiceModel;
using Monoscape.ApplicationGridController.Runtime;
using Monoscape.Common.Model;
using Monoscape.Common.Services.FileServer;
using Monoscape.LoadBalancerController;
using Monoscape.Common.Exceptions;

namespace Monoscape.ApplicationGridController
{
    internal static class EndPoints
    {
        public static INcApplicationGridService GetNcApplicationGridService(Node node)
        {
            try
            {
                string serviceUrl = node.ApplicationGridServiceUrl;
                string hostIpAddress = MonoscapeUtil.FindHostIpAddress().ToString();
                if (node.IpAddress.Equals(hostIpAddress) && (serviceUrl.Contains(node.IpAddress)))
                {
                    Log.Debug(typeof(EndPoints), "Node " + node.ToString() + " is running on the same host as the Application Grid");
                    serviceUrl.Replace(node.IpAddress, "localhost");
                }

                Log.Debug(typeof(EndPoints), "Creating INcApplicationGridService channel to node: " + serviceUrl);
                var binding = MonoscapeServiceHost.GetBinding();
                var address = new EndpointAddress(serviceUrl);
                ChannelFactory<INcApplicationGridService> factory = new ChannelFactory<INcApplicationGridService>(binding, address);
                INcApplicationGridService channel = factory.CreateChannel();
                return channel;
            }
            catch (Exception e)
            {
                MonoscapeException me = new MonoscapeException("Could not connect to the node", e);
                Log.Error(typeof(EndPoints), me);
                throw me;
            }
        }

        public static ILbApplicationGridService GetLbApplicationGridService()
        {
            try
            {
                string serviceUrl = Settings.LbApplicationGridEndPointUrl;
                Log.Debug(typeof(EndPoints), "Creating ILbApplicationGridService channel to node: " + serviceUrl);
                var binding = MonoscapeServiceHost.GetBinding();
                var address = new EndpointAddress(serviceUrl);
                ChannelFactory<ILbApplicationGridService> factory = new ChannelFactory<ILbApplicationGridService>(binding, address);
                ILbApplicationGridService channel = factory.CreateChannel();
                return channel;
            }
            catch (Exception e)
            {
                MonoscapeException me = new MonoscapeException("Could not connect to the Load Balancer", e);
                Log.Error(typeof(EndPoints), me);
                throw me;
            }
        }

        internal static bool IsNodeAvailable(Node node)
        {
            try
            {
                // Echo node to check availability
                Log.Debug(typeof(EndPoints), "Checking node " + node + " availability...");
                GetNcApplicationGridService(node).Echo(new EchoRequest(Settings.Credentials));
                Log.Debug(typeof(EndPoints), "Node " + node + " is online");
                return true;
            }
            catch (EndpointNotFoundException)
            {
                Log.Debug(typeof(EndPoints), "Node " + node + " is offline");
                return false;
            }
        }
    }
}
