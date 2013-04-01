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
using Monoscape.ApplicationGridController.Api.Services.NodeController;
using Monoscape.Common;
using System.ServiceModel;
using Monoscape.NodeController.Runtime;

namespace Monoscape.NodeController
{
    internal static class EndPoints
    {
        public static IApNodeControllerService GetApNodeControllerService()
        {
            var binding = MonoscapeServiceHost.GetBinding();
            var address = new EndpointAddress(Settings.ApplicationGridEndPointURL);
            ChannelFactory<IApNodeControllerService> factory = new ChannelFactory<IApNodeControllerService>(binding, address);
            IApNodeControllerService channel = factory.CreateChannel();
            return channel;
        }
    }
}
