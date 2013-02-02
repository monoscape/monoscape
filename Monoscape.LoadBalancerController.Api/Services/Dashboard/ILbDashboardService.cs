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

using System.ServiceModel;
using Monoscape.Common.Model;
using Monoscape.LoadBalancerController.Api.Services.Dashboard.Model;

namespace Monoscape.LoadBalancerController.Api.Services.Dashboard
{
    [ServiceContract]
    public interface ILbDashboardService
    {
        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        LbGetRequestQueueResponse GetRequestQueue(LbGetRequestQueueRequest request);

        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        LbGetRoutingMeshHistoryResponse GetRoutingMeshHistory(LbGetRoutingMeshHistoryRequest request);
    }
}
