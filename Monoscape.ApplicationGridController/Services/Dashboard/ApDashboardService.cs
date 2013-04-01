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
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Monoscape.ApplicationGridController.Iaas;
using Monoscape.ApplicationGridController.Model;
using Monoscape.ApplicationGridController.Runtime;
using Monoscape.ApplicationGridController.Api.Services.Dashboard;
using Monoscape.ApplicationGridController.Api.Services.Dashboard.Model;
using Monoscape.ApplicationGridController.Sockets;
using Monoscape.Common;
using Monoscape.Common.Exceptions;
using Monoscape.Common.Model;
using Monoscape.Common.Services.FileServer;
using Monoscape.Common.Services.FileServer.Model;
using Monoscape.Common.Sockets.FileServer;
using Monoscape.Common.WCFExtensions;
using Monoscape.NodeController.Api.Services.ApplicationGrid;
using Monoscape.NodeController.Api.Services.ApplicationGrid.Model;
using Monoscape.NodeController.Sockets;
using AppGridModel = Monoscape.ApplicationGridController.Api.Services.Dashboard.Model;
using Monoscape.LoadBalancerController.Api.Services.ApplicationGrid.Model;

namespace Monoscape.ApplicationGridController.Services.Dashboard
{
    public class ApDashboardService : MonoscapeService, IApDashboardService
    {
        #region Protected Properties
        protected override MonoscapeCredentials Credentials
        {
            get 
            { 
                return Settings.Credentials; 
            }
        }

        private IMonoscapeIaasClient IaasClient
        {
            get
            {
                MonoscapeIaasConfig config = new MonoscapeIaasConfig(Settings.IaasAccessKey, Settings.IaasSecretKey, Settings.IaasServiceURL);
                return MonoscapeIaasClientFactory.GetInstance(config);
            }
        }
        #endregion

        #region Private Methods
		private void UpdateApplications(List<Node> nodes)
        {
            foreach (Node node in nodes)
            {
				try
				{
	                NcDescribeApplicationsRequest request = new NcDescribeApplicationsRequest(Settings.Credentials);
	                var channel = EndPoints.GetNcApplicationGridService(node);
	                NcDescribeApplicationsResponse response = channel.DescribeApplications(request);
	                node.Applications = response.Applications;
                    foreach(Application application in node.Applications)
                    {
                        LbGetApplicationInstancesRequest request1 = new LbGetApplicationInstancesRequest(Settings.Credentials);
                        request1.NodeId = node.Id;
                        request1.ApplicationId = application.Id;                        
                        LbGetApplicationInstancesResponse response1 = EndPoints.GetLbApplicationGridService().GetApplicationInstances(request1);
                        application.ApplicationInstances = response1.ApplicationInstances;
                    }
	                Log.Debug(this, "Updated applications of node " + node.ToString());
				}
				catch(Exception e)
				{
					Log.Error(this, "Could not update applications of node " + node.ToString());
					throw e;
				}				
            }
        }
		
        private bool ApplicationExistsInNode(int applicationId, Node node)
        {
            NcApplicationExistsRequest request = new NcApplicationExistsRequest(Credentials);
            request.ApplicationId = applicationId;
            NcApplicationExistsResponse response = EndPoints.GetNcApplicationGridService(node).ApplicationExists(request);
            return response.Exists;
        }

        private Application FindApplication(int id)
        {
            foreach (Application app in Database.GetInstance().Applications)
            {
                if (app.Id == id)
                    return app;
            }
            return null;
        }

        private void UploadApplicationToNode(int applicationId, Node node)
        {
			Log.Debug(this, "Uploading application " + applicationId + " to node " + node.ToString());			           

            Application app = FindApplication(applicationId);
            if (app != null)
            {
                string filePath = Path.Combine(Settings.ApplicationStorePath, app.FileName);
                if (File.Exists(filePath))
                {
                	Log.Debug(this, "Transferring application package...");
                    NcFileTransferSocket socket = new NcFileTransferSocket(node.IpAddress_, Settings.NcFileTransferSocketPort);
                    socket.SendFile(filePath);

                    Log.Debug(this, "Transferring application data...");
                    NcAddApplicationRequest request = new NcAddApplicationRequest(Credentials);
                    request.Application = app;
                    EndPoints.GetNcApplicationGridService(node).AddApplication(request);
                }
                else
                {
                    throw new MonoscapeException("File not found: " + app.FileName);
                }
            }
            else
            {
                throw new MonoscapeException("Application not found: " + app.Name);
            }
        }

        private List<Node> FindAvailableNodes()
        {
            // TODO: Write logic to find nodes with free resources

            UpdateNodes();
            return Database.GetInstance().Nodes;
        }

        private void UpdateNodes()
        {
            // Update nodes
            Log.Debug(this, "Updating nodes...");
            List<Node> nodesNotAvailable = new List<Node>();
            foreach (Node node in Database.GetInstance().Nodes)
            {
                if (!EndPoints.IsNodeAvailable(node))
                    nodesNotAvailable.Add(node);
            }
            foreach (Node node in nodesNotAvailable)
            {
                // Remove nodes not available
                Log.Debug(this, "Removing node " + node);
                Database.GetInstance().Nodes.Remove(node);
            }
        }
		#endregion

        #region Other Methods
        public ApGetConfigurationSettingsResponse GetConfigurationSettings(ApGetConfigurationSettingsRequest request)
        {
            Log.Debug(this, "GetConfigurationSettings()");
			
			try
			{
	            Authenticate(request);
	            ApGetConfigurationSettingsResponse response = new ApGetConfigurationSettingsResponse();
	            ApConfigurationSettings settings = new ApConfigurationSettings();
                settings.IpAddress = MonoscapeUtil.FindHostIpAddress();
                settings.RunningOnMono = MonoscapeUtil.IsRunningOnMono();
                settings.MonoRuntime = MonoscapeUtil.GetMonoRuntime();
                settings.DotNetRuntime = MonoscapeUtil.GetDotNetRuntime();
                settings.OperatingSystem = MonoscapeUtil.GetOperatingSystem();

                settings.IaasName = Settings.IaasName;
	            settings.IaasAccessKey = Settings.IaasAccessKey;
	            settings.IaasSecretKey = Settings.IaasSecretKey;
	            settings.IaasServiceURL = Settings.IaasServiceURL;
	            settings.IaasKeyName = Settings.IaasKeyName;
				
	            response.ConfigurationSettings = settings;
	            return response;
			}
			catch(Exception e)
			{
				Log.Error(this, e);
				throw e;
			}
        }
		
		public ApDescribeNodesResponse DescribeNodes(ApDescribeNodesRequest request)
        {
            Log.Debug(this, "DescribeNodes()");
			
			try
			{
	            Authenticate(request);
	            
                // Update nodes
                UpdateNodes();
                // Update application instances in nodes
	            UpdateApplications(Database.GetInstance().Nodes);

                ApDescribeNodesResponse response = new ApDescribeNodesResponse();
	            response.Nodes = Database.GetInstance().Nodes;            
	            return response;
			}
			catch(Exception e)
			{
				Log.Error(this, e);
				throw e;
			}
        }

        public ApDescribeApplicationsResponse DescribeApplications(ApDescribeApplicationsRequest request)
        {
            Log.Debug(this, "DescribeApplications()");
			
			try
			{
	            Authenticate(request);
	            ApDescribeApplicationsResponse response = new ApDescribeApplicationsResponse();
	            response.Applications = Database.GetInstance().Applications;
	            return response;
			}
			catch(Exception e)
			{
				Log.Error(this, e);
				throw e;
			}
        }

        public ApDescribeScalingHistoryResponse DescribeScalingHistory(ApDescribeScalingHistoryRequest request)
        {
            Log.Debug(this, "DescribeScalingHistory()");

            try
            {
                Authenticate(request);
                ApDescribeScalingHistoryResponse response = new ApDescribeScalingHistoryResponse();
                response.ScalingHistory = Database.GetInstance().ScalingHistory;
                return response;
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        public ApStartApplicationResponse StartApplication(ApStartApplicationRequest request)
        {
            Log.Info(this, "StartApplication()");
			
            try 
			{
            	Authenticate(request);
	            ApStartApplicationResponse response = new ApStartApplicationResponse();
	            List<Node> nodes = FindAvailableNodes();
	            foreach (Node node in nodes)
	            {
	                if (!ApplicationExistsInNode(request.ApplicationId, node))
	                {
	                    // Upload application to node
	                    UploadApplicationToNode(request.ApplicationId, node);
	                }
	            
	                NcStartApplicationInstancesRequest ncRequest = new NcStartApplicationInstancesRequest(Credentials);
	                ncRequest.ApplicationId = request.ApplicationId;
                    ncRequest.TenantName = request.TenantName;
	                ncRequest.NumberOfInstances = request.NumberOfInstances;
	                NcStartApplicationInstancesResponse ncResponse = EndPoints.GetNcApplicationGridService(node).StartApplicationInstances(ncRequest);
	                response.Urls = ncResponse.Urls;

                    // Update Routing Mesh in Load Balancer
                    LbAddApplicationInstancesRequest request2 = new LbAddApplicationInstancesRequest(Credentials);                    
                    request2.AppInstances = ncResponse.ApplicationInstances;
                    EndPoints.GetLbApplicationGridService().AddApplicationInstances(request2);
	            }
	            return response;
            } 
			catch(Exception e)
			{
				Log.Error(this, e);				
                throw e;
			}
        }

        public ApApplicationExistsResponse ApplicationExists(ApApplicationExistsRequest request)
        {
            Log.Info(this, "ApplicationExists()");

            try
            {
                Authenticate(request);
                ApApplicationExistsResponse response = new ApApplicationExistsResponse();
                if (Database.GetInstance().Applications != null)
                {
                    Application app = Database.GetInstance().Applications.Where(x => x.Id == request.ApplicationId).FirstOrDefault();
                    response.Exists = (app != null);
                }
                return response;
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }        

        public ApStopApplicationResponse StopApplication(AppGridModel.ApStopApplicationRequest request)
        {
            Log.Debug(this, "StopApplication()");
            throw new NotImplementedException();
        }

        public ApAddApplicationResponse AddApplication(ApAddApplicationRequest request)
        {
            Log.Debug(this, "AddApplication()");

            try
            {
                Authenticate(request);
                Log.Info(this, "Application " + request.Application.Name + " received");                

                if (!ControllerUtil.ApplicationExists(request.Application))
                {
                    string filePath = Settings.ApplicationStorePath + Path.DirectorySeparatorChar + request.Application.FileName;
                    if (!MonoscapeUtil.WebConfigExistsInRoot(filePath))
                    {
                        if (File.Exists(filePath))
                            File.Delete(filePath);
                        throw new MonoscapeException("Application package is not valid. Re-package the application excluding any project folders and try again.");
                    }

                    try
                    {                        
                        Database.GetInstance().Applications.Add(request.Application);
                        Database.GetInstance().Commit();
                        Log.Debug(this, "Application added to the database");
                    }
                    catch (Exception)
                    {
                        Log.Debug(this, "Application package not found");
                    }
                }
                else
                {
                    Log.Debug(this, "Application already exists in the database");
                }
                return new ApAddApplicationResponse();
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        public ApAddApplicationTenantsResponse AddApplicationTenants(ApAddApplicationTenantsRequest request)
        {
            Log.Debug(this, "AddApplicationTenants()");
            try
            {
                Authenticate(request);
                Application application = FindApplication(request.ApplicationId);
                if (application != null)
                {
                    if (application.Tenants == null)
                        application.Tenants = request.Tenants;
                    else
                        application.Tenants.AddRange(request.Tenants);

                    application.RowState = EntityState.Modified;
                    Database.GetInstance().Commit();

                    foreach (Tenant tenant in request.Tenants)
                        Log.Debug(this, "Tenant added: " + application + " " + tenant);
                }

                ApAddApplicationTenantsResponse response = new ApAddApplicationTenantsResponse();
                return response;
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        public ApRemoveApplicationResponse RemoveApplication(ApRemoveApplicationRequest request)
        {
            Log.Debug(this, "RemoveApplication()");

            try
            {
                Authenticate(request);
                ApRemoveApplicationResponse response = new ApRemoveApplicationResponse();
                Application application = FindApplication(request.ApplicationId);
                if (application != null)
                {
					string appNameVersion = application.ToString();
                    string filePath = Settings.ApplicationStorePath + Path.DirectorySeparatorChar + application.FileName;
					if(File.Exists(filePath))
						File.Delete(filePath);
                    application.RowState = EntityState.Removed;
                    Database.GetInstance().Commit();
                    response.Removed = true;
					Log.Debug(this, "Application " + appNameVersion + " removed");
                }
                else
                {
                    throw new MonoscapeException("Application not found");
                }
                return response;
            }                        
			catch(Exception e)
			{
				Log.Error(this, e);
				throw e;
			}
        }

        public ApGetApplicationResponse GetApplication(ApGetApplicationRequest request)
        {
            Log.Debug(this, "GetApplication()");

            try
            {
                Authenticate(request);
                ApGetApplicationResponse response = new ApGetApplicationResponse();
                response.Application = FindApplication(request.ApplicationId);
                return response;
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        public ApStopApplicationInstanceResponse StopApplicationInstance(ApStopApplicationInstanceRequest request)
        {
            Log.Info(this, "StopApplicationInstance()");

            try
            {
                Authenticate(request);
                ApStopApplicationInstanceResponse response = new ApStopApplicationInstanceResponse();
                Node node = Database.GetInstance().Nodes.Where(x => x.Id == request.NodeId).FirstOrDefault();
                if (node != null)
                {
                    // Stop application instance in the node
                    NcStopApplicationRequest ncRequest = new NcStopApplicationRequest(Credentials);
                    ncRequest.ApplicationId = request.ApplicationId;
                    ncRequest.InstanceId = request.InstanceId;
                    EndPoints.GetNcApplicationGridService(node).StopApplicationInstance(ncRequest);

                    // Update routing mesh in the load balancer                    
                    LbRemoveApplicationInstanceRequest request_ = new LbRemoveApplicationInstanceRequest(Credentials);
                    request_.NodeId = request.NodeId;
                    request_.ApplicationId = request.ApplicationId;
                    request_.InstanceId = request.InstanceId;                    
                    EndPoints.GetLbApplicationGridService().RemoveApplicationInstances(request_);
                }
                else
                {
                    throw new MonoscapeException("Node not found");
                }
                return response;
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }
        #endregion
			
		#region Iaas Interface Methods
        public ApAllocateAddressResponse AllocateAddress(ApAllocateAddressRequest request)
        {
            Log.Debug(this, "AllocateAddress()");

            try
            {
                Authenticate(request);
                ApAllocateAddressResponse response = new ApAllocateAddressResponse();
                response.Address = IaasClient.AllocateAddress();
                return response;
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        public void AssociateAddress(ApAssociateAddressRequest request)
        {
            Log.Debug(this, "AssociateAddress()");

            try
            {
                Authenticate(request);
                IaasClient.AssociateAddress(request.InstanceId, request.IpAddress);
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        public ApAuthorizeResponse Authorize(ApAuthorizeRequest request)
        {
            Log.Debug(this, "Authorize()");

            try
            {
                Authenticate(request);
                ApAuthorizeResponse response = new ApAuthorizeResponse();
                response.Authorized = IaasClient.Authorize();
                return response;
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        internal bool AuthorizeIaasClient()
        {
            IaasClient.Authorize();
            return false;
        }
                
        public ApDescribeAddressesResponse DescribeAddresses(ApDescribeAddressesRequest request)
        {
            Log.Debug(this, "DescribeAddresses()");

            try
            {
                Authenticate(request);
                ApDescribeAddressesResponse response = new ApDescribeAddressesResponse();
                response.Addresses = IaasClient.DescribeAddresses();
                return response;
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        public ApCreateKeyPairResponse CreateKeyPair(ApCreateKeyPairRequest request)
        {
            Log.Debug(this, "CreateKeyPair()");

            try
            {
                Authenticate(request);
                ApCreateKeyPairResponse response = new ApCreateKeyPairResponse();
                response.KeyPair = IaasClient.CreateKeyPair(request.KeyName);
                return response;
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        public ApDescribeImagesResponse DescribeImages(ApDescribeImagesRequest request)
        {
            Log.Debug(this, "DescribeImages()");

            try
            {
                Authenticate(request);
                ApDescribeImagesResponse response = new ApDescribeImagesResponse();
                response.Images = IaasClient.DescribeImages();
                return response;
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        public ApDescribeInstancesResponse DescribeInstances(ApDescribeInstancesRequest request)
        {
            Log.Debug(this, "DescribeInstances()");

            try
            {
                Authenticate(request);
                ApDescribeInstancesResponse response = new ApDescribeInstancesResponse();
                response.Instances = IaasClient.DescribeInstances();
                return response;
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        public ApRunInstancesResponse RunInstances(ApRunInstancesRequest request)
        {
            Log.Debug(this, "RunInstances()");

            try
            {
                Authenticate(request);
                ApRunInstancesResponse response = new ApRunInstancesResponse();
                response.Reservation = IaasClient.RunInstances(request.ImageId, request.InstanceType, request.KeyName, request.NoOfInstances);
                return response;
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        public void TerminateInstance(ApTerminateInstanceRequest request)
        {
            Log.Debug(this, "TerminateInstance()");

            try
            {
                Authenticate(request);
                IaasClient.TerminateInstance(request.InstanceId);
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        public void DeregisterImage(ApDeregisterImageRequest request)
        {
            Log.Debug(this, "DeregisterImage()");

            try
            {
                Authenticate(request);
                IaasClient.DeregisterImage(request.ImageId);
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        public void RebootInstance(ApRebootInstanceRequest request)
        {
            Log.Debug(this, "RebootInstance()");

            try
            {
                Authenticate(request);
                IaasClient.RebootInstance(request.InstanceId);
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        public ApGetConsoleOutputResponse GetConsoleOutput(ApGetConsoleOutputRequest request)
        {
            Log.Debug(this, "GetConsoleOutput()");

            try
            {
                Authenticate(request);
                ApGetConsoleOutputResponse response = new ApGetConsoleOutputResponse();
                response.ConsoleOutput = IaasClient.GetConsoleOutput(request.InstanceId);
                return response;
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }
		#endregion
    }
}
