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
 *  2013/06/04 Imesh Gunaratne <imesh@monoscape.org> Created.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monoscape.NodeController.Api;
using Monoscape.NodeController.Api.Services.ApplicationGrid.Model;

namespace Monoscape.NodeController
{
    class JavaApacheCartridge : INodeCartridge
    {
        public NcDeployApplicationResponse DeployApplication(NcDeployApplicationRequest request)
        {
            throw new NotImplementedException();
        }

        public NcStartApplicationInstancesResponse StartApplicationInstances(NcStartApplicationInstancesRequest request)
        {
            throw new NotImplementedException();
        }

        public NcStopApplicationResponse StopApplicationInstance(NcStopApplicationRequest request)
        {
            throw new NotImplementedException();
        }

        public NcDescribeApplicationsResponse DescribeApplications(NcDescribeApplicationsRequest request)
        {
            throw new NotImplementedException();
        }

        public NcApplicationExistsResponse ApplicationExists(NcApplicationExistsRequest request)
        {
            throw new NotImplementedException();
        }

        public NcAddApplicationResponse AddApplication(NcAddApplicationRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
