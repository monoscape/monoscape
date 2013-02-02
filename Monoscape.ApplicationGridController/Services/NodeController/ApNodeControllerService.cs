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
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using Monoscape.ApplicationGridController.Model;
using Monoscape.ApplicationGridController.Runtime;
using Monoscape.ApplicationGridController.Api.Services.Dashboard;
using Monoscape.ApplicationGridController.Api.Services.Dashboard.Model;
using Monoscape.ApplicationGridController.Api.Services.NodeController;
using Monoscape.ApplicationGridController.Api.Services.NodeController.Model;
using Monoscape.Common;
using Monoscape.Common.Exceptions;
using Monoscape.Common.Model;
using Monoscape.Common.WCFExtensions;
using System.Net;
using Monoscape.ApplicationGridController.Services.Dashboard;
using Monoscape.LoadBalancerController.Api.Services.ApplicationGrid.Model;

namespace Monoscape.ApplicationGridController.Services.NodeController
{
    public class ApNodeControllerService : MonoscapeService, IApNodeControllerService
    {
        #region Protected Properties
        protected override MonoscapeCredentials Credentials
        {
            get
            {
                return Settings.Credentials;
            }
        }
        #endregion

        #region Private Methods
        private int GetNextNodeId()
        {
            if (Database.GetInstance().Nodes.Count > 0)
            {
                Node lastNode = Database.GetInstance().Nodes.OrderBy(i => i.Id).Last();
                return lastNode.Id + 1;
            }
            return 1;
        }
		
		private string FindInstanceId(string ipAddress)
        {
            try
            {
                ApDashboardService service = new ApDashboardService();
                MonoscapeCredentials credentials = new MonoscapeCredentials(Settings.MonoscapeAccessKey, Settings.MonoscapeSecretKey);
                ApDescribeInstancesRequest request = new ApDescribeInstancesRequest(credentials);
                ApDescribeInstancesResponse response = service.DescribeInstances(request);

                foreach (Instance instance in response.Instances)
                {
                    if (instance.IpAddress.Equals(ipAddress))
                        return instance.InstanceId;
                }
            }
            catch (Exception)
            {
                Log.Error(this, "Could not find Instance ID of " + ipAddress);
            }
            return null;
        }
		#endregion

        public ApSubscribeNodeResponse SubscribeNode(ApSubscribeNodeRequest request)
        {
            Log.Debug(typeof(ApNodeControllerService), "SubscribeNode()");

            try
            {
                // Remove existing node instance if already subscribed
                Node existing = Database.GetInstance().Nodes.Find(x => x.IpAddress.Equals(request.IpAddress));
                if (existing != null)
                    Database.GetInstance().Nodes.Remove(existing);

                Node node = new Node();
                // Initialize Node Id
                // Multiple nodes may exist in the same host
                node.Id = GetNextNodeId();

				// Initialize node IP Address
                node.IpAddress = request.IpAddress;
                node.IpAddress_ = request.IpAddress_;                

                // Find node's virtual machine instance id from IaaS
                node.InstanceId = FindInstanceId(request.IpAddress);

                node.ApplicationGridServiceUrl = request.ApplicationGridServiceUrl;
                node.FileTransferServiceUrl = request.FileTransferServiceUrl;
                node.SubscribedOn = DateTime.Now;

                // Update database
                Database.GetInstance().Nodes.Add(node);

                ApSubscribeNodeResponse response = new ApSubscribeNodeResponse();
                // Send node information back
                response.Node = node;

                Log.Debug(typeof(ApNodeControllerService), "Node " + node.ToString() + " subscribed successfully");
                return response;
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }        

        public void UnSubscribeNode(ApUnSubscribeNodeRequest request)
        {
            Log.Debug(typeof(ApNodeControllerService), "UnSubscribeNode()");

            try
            {
                Node node = Database.GetInstance().Nodes.Find(x => x.IpAddress.Equals(request.IpAddress));
                if (node != null)
                {
                    Database.GetInstance().Nodes.Remove(node);

                    // Update routing mesh in the load balancer                    
                    LbRemoveApplicationInstanceRequest request_ = new LbRemoveApplicationInstanceRequest(Credentials);
                    request_.NodeId = node.Id;
                    request_.ApplicationId = -1;
                    request_.InstanceId = -1;
                    EndPoints.GetLbApplicationGridService().RemoveApplicationInstances(request_);
                }
                Log.Debug(typeof(ApNodeControllerService), "Node " + node.IpAddress + " removed successfully");
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }
    }
}
