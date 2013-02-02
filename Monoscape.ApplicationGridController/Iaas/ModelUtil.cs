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
using AWSModel = Amazon.EC2.Model;

namespace Monoscape.ApplicationGridController.Iaas
{
    internal class ModelUtil
    {
        public static Instance CreateInstance(AWSModel.RunningInstance runningInstance)
        {
            Instance instance = new Instance();
            instance.ImageId = runningInstance.ImageId;
            instance.InstanceId = runningInstance.InstanceId;
            instance.PrivateDnsName = runningInstance.PrivateDnsName;

            // Openstack sends Public IP address in PublicDnsName
            if (!string.IsNullOrEmpty(runningInstance.PublicDnsName))
                instance.IpAddress = runningInstance.PublicDnsName;
            else
                instance.IpAddress = runningInstance.IpAddress;

            if (runningInstance.InstanceState != null)
                instance.State = runningInstance.InstanceState.Name;
            instance.Type = runningInstance.InstanceType;
            return instance;
        }

        public static Address CreateInstance(AWSModel.Address address)
        {
            Address address_ = new Address();
            address_.InstanceId = address.InstanceId;
            address_.PublicIp = address.PublicIp;
            return address_;
        }

        public static ConsoleOutput CreateInstance(AWSModel.ConsoleOutput consoleOutput)
        {
            ConsoleOutput output = new ConsoleOutput();
            output.InstanceId = consoleOutput.InstanceId;
            output.Output = consoleOutput.Output;
            output.Timestamp = consoleOutput.Timestamp;
            return output;
        }

        public static KeyPair CreateInstance(AWSModel.KeyPair awsKeyPair)
        {
            KeyPair keyPair = new KeyPair();
            keyPair.KeyFingerprint = awsKeyPair.KeyFingerprint;
            keyPair.KeyMaterial = awsKeyPair.KeyMaterial;
            keyPair.KeyName = awsKeyPair.KeyName;
            return keyPair;
        }
    }
}
