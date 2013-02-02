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
using System.ServiceModel;
using Monoscape.NodeController.Api.Services.ApplicationGrid.Model;
using Monoscape.Common.Model;
using Monoscape.Common;

namespace Monoscape.NodeController.Api.Services.ApplicationGrid
{
    [ServiceContract]
    public interface INcApplicationGridService
    {
        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        EchoResponse Echo(EchoRequest request);

		[OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
		NcDeployApplicationResponse DeployApplication(NcDeployApplicationRequest request);
		
        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        NcStartApplicationInstancesResponse StartApplicationInstances(NcStartApplicationInstancesRequest request);

        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        NcStopApplicationResponse StopApplicationInstance(NcStopApplicationRequest request);

        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        NcDescribeApplicationsResponse DescribeApplications(NcDescribeApplicationsRequest request);

        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        NcApplicationExistsResponse ApplicationExists(NcApplicationExistsRequest request);

        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        NcAddApplicationResponse AddApplication(NcAddApplicationRequest request);
    }
}
