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
using Monoscape.Common;
using System.ServiceModel;
using Monoscape.Common.Model;
using Monoscape.LoadBalancerController.Api.Services.LoadBalancerWeb.Model;

namespace Monoscape.LoadBalancerController.Api.Services.LoadBalancerWeb
{
    [ServiceContract]
    public interface ILbLoadBalancerWebService
    {
        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        LbGetRoutingMeshResponse GetRoutingMesh(LbGetRoutingMeshRequest request);

        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        LbAddRequestToQueueResponse AddRequestToQueue(LbAddRequestToQueueRequest request);

        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        LbRemoveRequestFromQueueResponse RemoveRequestFromQueue(LbRemoveRequestFromQueueRequest request);

        //[OperationContract]
        //[FaultContract(typeof(MonoscapeFault))]
        //LbIncrementRequestCountResponse IncrementRequestCount(LbIncrementRequestCountRequest request);

        //[OperationContract]
        //[FaultContract(typeof(MonoscapeFault))]
        //LbDecrementRequestCountResponse DecrementRequestCount(LbDecrementRequestCountRequest request);
    }
}
