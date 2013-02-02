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
using Monoscape.ApplicationGridController.Model;

namespace Monoscape.ApplicationGridController.Iaas
{
    public interface IMonoscapeIaasClient
    {
        /// <summary>
        /// Acquire an elastic IP address from the IaaS.
        /// </summary>
        /// <returns></returns>
        string AllocateAddress();

        /// <summary>
        /// Associate an elastic IP address to a virtual machine output.
        /// </summary>
        /// <param name="vmInstanceId"></param>
        /// <param name="ipAddress"></param>
        void AssociateAddress(string vmInstanceId, string ipAddress);

        /// <summary>
        /// Authorize client with the IaaS using access key, secret key and service URL.
        /// </summary>
        /// <returns></returns>
        bool Authorize();

        /// <summary>
        /// Describe IP addresses used in the IaaS.
        /// </summary>
        /// <returns></returns>
        List<Address> DescribeAddresses();

        /// <summary>
        /// Describe images available in the IaaS.
        /// </summary>
        /// <returns></returns>
        List<Image> DescribeImages();

        /// <summary>
        /// Describe the instances available in the IaaS.
        /// </summary>
        /// <returns></returns>
        List<Instance> DescribeInstances();

        /// <summary>
        /// Create a new key RSA key pair to be used when launching a new VM instance.
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        KeyPair CreateKeyPair(string keyName);
        
        /// <summary>
        /// Run image instances.
        /// </summary>
        /// <param name="vmImageId"></param>
        /// <param name="instanceType"></param>
        /// <param name="numberOfInstances"></param>
        /// <returns></returns>
        Reservation RunInstances(string vmImageId, string instanceType, string keyName, int numberOfInstances);

        /// <summary>
        /// Terminate an output.
        /// </summary>
        /// <param name="instanceId"></param>
        void TerminateInstance(string instanceId);

        /// <summary>
        /// De-Register an image.
        /// </summary>
        /// <param name="imageId"></param>
        void DeregisterImage(string imageId);


        /// <summary>
        /// Reboot an output.
        /// </summary>
        /// <param name="instanceId"></param>
        void RebootInstance(string instanceId);

        /// <summary>
        /// Get the console output of an output.
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        ConsoleOutput GetConsoleOutput(string instanceId);
    }
}
