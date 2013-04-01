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
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Monoscape.ApplicationGridController.Model;
using Monoscape.ApplicationGridController.Api.Services.Dashboard.Model;
using Monoscape.Common.Services.FileServer.Model;
using Monoscape.Common.Model;
using Monoscape.Common;

namespace Monoscape.ApplicationGridController.Api.Services.Dashboard
{    
    [ServiceContract]
    public interface IApDashboardService
    {
        #region Monoscape Specific API Methods
        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        ApGetConfigurationSettingsResponse GetConfigurationSettings(ApGetConfigurationSettingsRequest request);

        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        ApDescribeNodesResponse DescribeNodes(ApDescribeNodesRequest request);

        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        ApDescribeApplicationsResponse DescribeApplications(ApDescribeApplicationsRequest request);

        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        ApDescribeScalingHistoryResponse DescribeScalingHistory(ApDescribeScalingHistoryRequest request);

        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        ApStartApplicationResponse StartApplication(ApStartApplicationRequest request);

        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        ApStopApplicationResponse StopApplication(ApStopApplicationRequest request);

        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        ApRemoveApplicationResponse RemoveApplication(ApRemoveApplicationRequest request);

        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        ApAddApplicationResponse AddApplication(ApAddApplicationRequest request);

        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        ApAddApplicationTenantsResponse AddApplicationTenants(ApAddApplicationTenantsRequest request);

        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        ApGetApplicationResponse GetApplication(ApGetApplicationRequest request);

        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        ApApplicationExistsResponse ApplicationExists(ApApplicationExistsRequest request);

        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        ApStopApplicationInstanceResponse StopApplicationInstance(ApStopApplicationInstanceRequest request);
        #endregion

        #region IaaS Interface Methods
        /// <summary>
        /// Acquire an elastic IP address from the IaaS.
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        ApAllocateAddressResponse AllocateAddress(ApAllocateAddressRequest request);

        /// <summary>
        /// Associate an elastic IP address to a virtual machine instance.
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        void AssociateAddress(ApAssociateAddressRequest request);

        /// <summary>
        /// Authorize client with the IaaS using access key, secret key and service URL.
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        ApAuthorizeResponse Authorize(ApAuthorizeRequest request);

        /// <summary>
        /// Create a new RSA key pair to be used when starting a new VM instance.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        ApCreateKeyPairResponse CreateKeyPair(ApCreateKeyPairRequest request);

        /// <summary>
        /// Describe IP addresses used in the IaaS.
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        ApDescribeAddressesResponse DescribeAddresses(ApDescribeAddressesRequest request);

        /// <summary>
        /// Describe images available in the IaaS.
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        ApDescribeImagesResponse DescribeImages(ApDescribeImagesRequest request);

        /// <summary>
        /// Describe the instances available in the IaaS.
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        ApDescribeInstancesResponse DescribeInstances(ApDescribeInstancesRequest request);

        /// <summary>
        /// Run image instances.
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        ApRunInstancesResponse RunInstances(ApRunInstancesRequest request);

        /// <summary>
        /// Terminate an instance.
        /// </summary>        
        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        void TerminateInstance(ApTerminateInstanceRequest request);

        /// <summary>
        /// De-Register an image.
        /// </summary>        
        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        void DeregisterImage(ApDeregisterImageRequest request);


        /// <summary>
        /// Reboot an instance.
        /// </summary>        
        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        void RebootInstance(ApRebootInstanceRequest request);

        /// <summary>
        /// Get the console output of an instance.
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(MonoscapeFault))]
        ApGetConsoleOutputResponse GetConsoleOutput(ApGetConsoleOutputRequest request);                
		#endregion
    }
}
