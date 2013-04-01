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
using System.ServiceModel;
using Monoscape.ApplicationGrid;
using System.ServiceModel.Channels;
using Monoscape.ApplicationGrid.EndPoints.CloudController.Model;
using Monoscape.ApplicationGrid.EndPoints.CloudController;

namespace Monoscape.Dashboard.Runtime.ServiceClients
{
    public class ApplicationGridServiceClient : ClientBase<ICloudControllerService>, ICloudControllerService
    {
        public ApplicationGridServiceClient(Binding binding, EndpointAddress address)
            : base(binding, address)
        {
        }

        public GetConfigurationSettingsResponse GetConfigurationSettings(GetConfigurationSettingsRequest request)
        {
            return Channel.GetConfigurationSettings(request);
        }

        public AllocateAddressResponse AllocateAddress(AllocateAddressRequest request)
        {
            return Channel.AllocateAddress(request);
        }

        public void AssociateAddress(AssociateAddressRequest request)
        {
            Channel.AssociateAddress(request);
        }

        public AuthorizeResponse Authorize(AuthorizeRequest request)
        {
            return Channel.Authorize(request);
        }

        public DescribeAddressesResponse DescribeAddresses(DescribeAddressesRequest request)
        {
            return Channel.DescribeAddresses(request);
        }

        public DescribeImagesResponse DescribeImages(DescribeImagesRequest request)
        {
            return Channel.DescribeImages(request);
        }

        public DescribeInstancesResponse DescribeInstances(DescribeInstancesRequest request)
        {
            return Channel.DescribeInstances(request);
        }

        public RunInstancesResponse RunInstances(RunInstancesRequest request)
        {
            return Channel.RunInstances(request);
        }

        public void TerminateInstance(TerminateInstanceRequest request)
        {
            Channel.TerminateInstance(request);
        }

        public void DeregisterImage(DeregisterImageRequest request)
        {
            Channel.DeregisterImage(request);
        }

        public void RebootInstance(RebootInstanceRequest request)
        {
            Channel.RebootInstance(request);
        }

        public GetConsoleOutputResponse GetConsoleOutput(GetConsoleOutputRequest request)
        {
            return Channel.GetConsoleOutput(request);
        }


        public DescribeNodesResponse DescribeNodes(DescribeNodesRequest request)
        {
            return Channel.DescribeNodes(request);
        }
    }
}