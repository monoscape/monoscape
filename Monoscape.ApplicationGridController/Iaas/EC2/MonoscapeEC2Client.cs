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
using Monoscape.ApplicationGridController.Iaas;
using Amazon.EC2;
using Monoscape.ApplicationGridController.Model;
using AWSModel = Amazon.EC2.Model;
using Monoscape.ApplicationGridController;
using System.Net;
using System.Xml;
using Monoscape.Common;
using Monoscape.ApplicationGridController.Runtime;

namespace Monoscape.ApplicationGridController.Iaas.EC2
{
    /// <summary>
    /// EC2 client that implements Monoscape.Cloud IIaasClient interface using Amazon EC2 SDK.
    /// </summary>
    internal class MonoscapeEC2Client : IMonoscapeIaasClient
    {
        private AmazonEC2Client ec2;

        public MonoscapeEC2Client(MonoscapeIaasConfig config)
        {
            Initialize(config.AccessKey, config.SecretKey, config.ServiceURL);
        }

        private void Initialize(string ec2AccessKey, string ec2SecretKey, string serviceURL)
        {
            ec2 = new AmazonEC2Client(ec2AccessKey, ec2SecretKey, new AmazonEC2Config().WithServiceURL(serviceURL));
        }

        public bool Authorize()
        {
            DescribeImages();
            return true;          
        }

        public string AllocateAddress()
        {
            try
            {
                Amazon.EC2.Model.AllocateAddressRequest request = new Amazon.EC2.Model.AllocateAddressRequest();
                Amazon.EC2.Model.AllocateAddressResponse response = ec2.AllocateAddress(request);
                return response.AllocateAddressResult.PublicIp;
            }
            catch (WebException e)
            {                
                throw new MonoscapeEC2Exception(e.Message, e);
            }
            catch (AmazonEC2Exception e)
            {               
                throw new MonoscapeEC2Exception(e.Message, e);
            }
        }

        public void AssociateAddress(string vmInstanceId, string ipAddress)
        {
            try
            {
                AWSModel.AssociateAddressRequest request = new AWSModel.AssociateAddressRequest();
                request.InstanceId = vmInstanceId;
                request.PublicIp = ipAddress;
                ec2.AssociateAddress(request);
			}
            catch (WebException e)
            {                
                throw new MonoscapeEC2Exception(e.Message, e);
            }
            catch (AmazonEC2Exception e)
            {                
                throw new MonoscapeEC2Exception(e.Message, e);
            }
            catch (XmlException e)
            {
                // There is an issue in Amazon EC2 API in processing the XML returned from euca_associate_address,
				// still the remote call has executed properly. Ignore the xml exception as an workaround
				Log.Debug(this, "AssociateAddress() XmlException raised", e);
            }
        }

        public List<Address> DescribeAddresses()
        {
            try
            {
                AWSModel.DescribeAddressesRequest request = new AWSModel.DescribeAddressesRequest();
                AWSModel.DescribeAddressesResponse response = ec2.DescribeAddresses(request);
                List<Address> addresses = new List<Address>();
                if (response != null)
                {
                    foreach (AWSModel.Address address in response.DescribeAddressesResult.Address)
                    {
                        Address address_ = ModelUtil.CreateInstance(address);
                        addresses.Add(address_);
                    }
                }
                return addresses;
            }
            catch (WebException e)
            {                
                throw new MonoscapeEC2Exception(e.Message, e);
            }
            catch (AmazonEC2Exception e)
            {                
                throw new MonoscapeEC2Exception(e.Message, e);
            }
        }

        public List<Image> DescribeImages()
        {
            try
            {
                AWSModel.DescribeImagesRequest request = new AWSModel.DescribeImagesRequest();
                AWSModel.DescribeImagesResponse response = ec2.DescribeImages(request);
                List<Image> images = new List<Image>();
                if (response != null)
                {
                    foreach (AWSModel.Image image in response.DescribeImagesResult.Image)
                    {
                        if(image.ImageType.Equals("machine"))
                        {
                            Image vmImage = new Image();
                            vmImage.ImageId = image.ImageId;
                            vmImage.Name = image.ImageLocation;
                            vmImage.State = image.ImageState;
                            images.Add(vmImage);
                        }
                    }
                }
                return images;
            }
            catch (WebException e)
            {                
                throw new MonoscapeEC2Exception(e.Message, e);
            }
            catch (AmazonEC2Exception e)
            {                
                throw new MonoscapeEC2Exception(e.Message, e);
            }
        }

        public List<Instance> DescribeInstances()
        {
            try
            {
                AWSModel.DescribeInstancesRequest request = new AWSModel.DescribeInstancesRequest();
                AWSModel.DescribeInstancesResponse response = ec2.DescribeInstances(request);
                List<Instance> instances = new List<Instance>();
                if (response != null)
                {
                    foreach (AWSModel.Reservation reservation in response.DescribeInstancesResult.Reservation)
                    {
                        foreach (AWSModel.RunningInstance runningInstance in reservation.RunningInstance)
                        {
                            Instance instance = ModelUtil.CreateInstance(runningInstance);
                            instances.Add(instance);
                        }
                    }
                }
                return instances;
            }
            catch (WebException e)
            {                
                throw new MonoscapeEC2Exception(e.Message, e);
            }
            catch (AmazonEC2Exception e)
            {                
                throw new MonoscapeEC2Exception(e.Message, e);
            }
        }

        public KeyPair CreateKeyPair(string keyName)
        {
            AWSModel.CreateKeyPairRequest request = new AWSModel.CreateKeyPairRequest();
            request.KeyName = keyName;
            
            AWSModel.CreateKeyPairResponse response = ec2.CreateKeyPair(request);
            KeyPair keyPair = ModelUtil.CreateInstance(response.CreateKeyPairResult.KeyPair);
            return keyPair;
        }

        public Reservation RunInstances(string vmImageId, string instanceType, string keyName, int numberOfInstances)
        {
            try
            {
                AWSModel.RunInstancesRequest request = new AWSModel.RunInstancesRequest();
                request.ImageId = vmImageId;
                if (!string.IsNullOrEmpty(instanceType))
                    request.InstanceType = instanceType;
                if (!string.IsNullOrEmpty(keyName))
                    request.KeyName = keyName;
                else
                    request.KeyName = Settings.IaasKeyName;
                request.MaxCount = numberOfInstances;
                AWSModel.RunInstancesResponse response = ec2.RunInstances(request);
                if (response != null)
                {
                    Reservation reservation = new Reservation();
                    reservation.ReservationId = response.RunInstancesResult.Reservation.ReservationId;
                    foreach (AWSModel.RunningInstance runningIns in response.RunInstancesResult.Reservation.RunningInstance)
                    {
                        if (reservation.Instances == null)
                            reservation.Instances = new List<Instance>();

                        Instance instance = ModelUtil.CreateInstance(runningIns);
                        reservation.Instances.Add(instance);
                    }
                    return reservation;
                }
                return null;
            }
            catch (WebException e)
            {                
                throw new MonoscapeEC2Exception(e.Message, e);
            }
            catch (AmazonEC2Exception e)
            {                
                throw new MonoscapeEC2Exception(e.Message, e);
            }
        }

        public void TerminateInstance(string instanceId)
        {
            try
            {
                AWSModel.TerminateInstancesRequest request = new AWSModel.TerminateInstancesRequest();
                List<string> terminateList = new List<string>();
                terminateList.Add(instanceId);
                request.InstanceId = terminateList;
                ec2.TerminateInstances(request);
            }
            catch (WebException e)
            {               
                throw new MonoscapeEC2Exception(e.Message, e);
            }
            catch (AmazonEC2Exception e)
            {                
                throw new MonoscapeEC2Exception(e.Message, e);
            }
        }

        public void DeregisterImage(string imageId)
        {
            try
            {
                AWSModel.DeregisterImageRequest request = new AWSModel.DeregisterImageRequest();
                request.ImageId = imageId;
                ec2.DeregisterImage(request);
            }
            catch (WebException e)
            {                
                throw new MonoscapeEC2Exception(e.Message, e);
            }
            catch (AmazonEC2Exception e)
            {                
                throw new MonoscapeEC2Exception(e.Message, e);
            }
            catch (Exception e)
            {
                // There seems to be an issue in Openstack EC2 api, workaround
                if (!((e.Message.Equals("There is an error in XML document (0, 0).")) && (e.InnerException != null) && (e.InnerException.Message.Equals("Root element is missing."))))
                    throw e;
            }
        }

        public void RebootInstance(string instanceId)
        {
            try
            {
                AWSModel.RebootInstancesRequest request = new AWSModel.RebootInstancesRequest();
                List<string> list = new List<string>();
                list.Add(instanceId);
                request.InstanceId = list;
                ec2.RebootInstances(request);
            }
            catch (WebException e)
            {                
                throw new MonoscapeEC2Exception(e.Message, e);
            }
            catch (AmazonEC2Exception e)
            {                
                throw new MonoscapeEC2Exception(e.Message, e);
            }
        }

        public ConsoleOutput GetConsoleOutput(string instanceId)
        {
            try
            {
                AWSModel.GetConsoleOutputRequest request = new AWSModel.GetConsoleOutputRequest();
                request.InstanceId = instanceId;
                AWSModel.GetConsoleOutputResponse response = ec2.GetConsoleOutput(request);
                if (response != null)
                    return ModelUtil.CreateInstance(response.GetConsoleOutputResult.ConsoleOutput);
                else
                    return null;
            }
            catch (WebException e)
            {                
                throw new MonoscapeEC2Exception(e.Message, e);
            }
            catch (AmazonEC2Exception e)
            {                
                throw new MonoscapeEC2Exception(e.Message, e);
            }
        }
    }
}
