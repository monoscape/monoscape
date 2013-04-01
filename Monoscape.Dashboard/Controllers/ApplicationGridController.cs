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
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using Monoscape.ApplicationGridController.Model;
using Monoscape.ApplicationGridController.Api.Services.Dashboard;
using Monoscape.ApplicationGridController.Api.Services.Dashboard.Model;
using Monoscape.Common;
using Monoscape.Common.Exceptions;
using Monoscape.Common.Model;
using Monoscape.Dashboard.Runtime;
using Monoscape.Common.Services.FileServer;
using Monoscape.Common.Services.FileServer.Model;
using Monoscape.Common.WCFExtensions;
using Monoscape.Dashboard.Models;
using System.Net;
using Monoscape.ApplicationGridController.Sockets;

namespace Monoscape.Dashboard.Controllers
{
    public class ApplicationGridController : AbstractController
    {
        #region Private Properties        
        private MonoscapeCredentials Credentials
        {
            get
            {
                MonoscapeCredentials credentials = new MonoscapeCredentials(Settings.MonoscapeAccessKey, Settings.MonoscapeSecretKey);
                return credentials;
            }
        }
        #endregion

        #region Private Methods
		private void UploadFile(Stream fileStream, Application application)
        {
			ApGetConfigurationSettingsRequest request1 = new ApGetConfigurationSettingsRequest(Credentials);
            ApGetConfigurationSettingsResponse response1 = EndPoints.ApDashboardService.GetConfigurationSettings(request1);
                        
            IPAddress appGridIpAddress = response1.ConfigurationSettings.IpAddress;
            ApFileTransferSocket socket = new ApFileTransferSocket(appGridIpAddress, Settings.ApFileTransferSocketPort);
            socket.SendFile(fileStream, application.FileName);

            ApAddApplicationRequest request = new ApAddApplicationRequest(Credentials);
            request.Application = application;
            EndPoints.ApDashboardService.AddApplication(request);
		}
		
		private Application GetApplication(int applicationId)
        {
            ApGetApplicationRequest request = new ApGetApplicationRequest(Credentials);
            request.ApplicationId = applicationId;
            ApGetApplicationResponse response = EndPoints.ApDashboardService.GetApplication(request);
            return response.Application;
        }

        private object FindImageName(List<Image> images, string imageId)
        {
            if ((images != null) && (imageId != null))
            {
                foreach (Image image in images)
                {
                    if (image.ImageId.Equals(imageId))
                        return image.Name;
                }
            }
            return string.Empty;
        }
		
        private List<Node> DescribeNodes()
        {
            ApDescribeNodesRequest request = new ApDescribeNodesRequest(Credentials);
            ApDescribeNodesResponse response = EndPoints.ApDashboardService.DescribeNodes(request);
            return response.Nodes;
        }

        private List<Image> DescribeImages()
        {
            ApDescribeImagesRequest request = new ApDescribeImagesRequest(Credentials);
            ApDescribeImagesResponse response = EndPoints.ApDashboardService.DescribeImages(request);
            return response.Images;
        }

        private List<Instance> DescribeInstances()
        {
            ApDescribeInstancesRequest request = new ApDescribeInstancesRequest(Credentials);
            ApDescribeInstancesResponse response = EndPoints.ApDashboardService.DescribeInstances(request);
            return response.Instances;
        }

        private List<Application> DescribeApplications()
        {
            ApDescribeApplicationsRequest request = new ApDescribeApplicationsRequest(Credentials);
            ApDescribeApplicationsResponse response = EndPoints.ApDashboardService.DescribeApplications(request);
            return response.Applications;
        }

        private List<ScalingHistoryItem> DescribeScalingHistory()
        {
            ApDescribeScalingHistoryRequest request = new ApDescribeScalingHistoryRequest(Credentials);
            ApDescribeScalingHistoryResponse response = EndPoints.ApDashboardService.DescribeScalingHistory(request);
            return response.ScalingHistory;
        }
        #endregion

        //
        // GET: /applicationgrid/
        public ActionResult Index()
        {
            try
            {
                ViewData["MonoscapeAccessKey"] = Credentials.AccessKey;
                ViewData["MonoscapeSecretKey"] = Credentials.SecretKey;
                ViewData["ApplicationGridEndPointURL"] = Settings.ApplicationGridEndPointURL;
                ViewData["ApplicationGridStatus"] = "Offline";

                try
                {
                    ApGetConfigurationSettingsRequest request = new ApGetConfigurationSettingsRequest(Credentials);
                    ApGetConfigurationSettingsResponse response = EndPoints.ApDashboardService.GetConfigurationSettings(request);
                    if (response != null)
                    {
                        ViewData["IaasName"] = response.ConfigurationSettings.IaasName;
                        ViewData["IaasAccessKey"] = response.ConfigurationSettings.IaasAccessKey;
                        ViewData["IaasSecretKey"] = response.ConfigurationSettings.IaasSecretKey;
                        ViewData["IaasServiceURL"] = response.ConfigurationSettings.IaasServiceURL;
                        ViewData["IaasKeyName"] = response.ConfigurationSettings.IaasKeyName;
						ViewData["RunningOnMono"] = response.ConfigurationSettings.RunningOnMono.ToString().ToUpper();
						ViewData["MonoRuntime"] = response.ConfigurationSettings.MonoRuntime;
						ViewData["DotNetRuntime"] = response.ConfigurationSettings.DotNetRuntime;
						ViewData["OperatingSystem"] = response.ConfigurationSettings.OperatingSystem;
                        ViewData["ApplicationGridStatus"] = "Authorized";

                        try
                        {
                            ApAuthorizeRequest authRequest = new ApAuthorizeRequest(Credentials);
                            ApAuthorizeResponse authResponse = EndPoints.ApDashboardService.Authorize(authRequest);
                            if (authResponse.Authorized)
                                ViewData["IaasStatus"] = "Authorized";
                            else
                                ViewData["IaasStatus"] = "Authentication failed";
                        }
                        catch (Exception e)
                        {
                            ViewData["IaasStatus"] = "Authentication failed";
                            ViewData["IaasError"] = e.Message;
                        }
                    }
                }
                catch (Exception e)
                {
                    ViewData["ApplicationGridError"] = e.Message;
                }                
                return View();
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }

        //
        // GET: /applicationgrid/nodes
        public ActionResult Nodes()
        {
            try
            {
                return View(DescribeNodes());
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }

        //
        // GET: /applicationgrid/images
        public ActionResult Images()
        {
            try
            {                
                return View(DescribeImages());
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }        

        //
        // GET: /applicationgrid/instances
        public ActionResult Instances()
        {
            try
            {                
                return View(DescribeInstances());
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }

        public ActionResult ScalingHistory()
        {
            try
            {
                return View(DescribeScalingHistory());
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }

        public ActionResult Applications()
        {
            try
            {
                return View(DescribeApplications());
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }

        public ActionResult ApplicationInfo(int applicationId)
        {
            try
            {
                Application app = GetApplication(applicationId);
                if (app != null)
                {
                    return View(app);
                }
                else
                {
                    throw new MonoscapeException("Application not found");
                }
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }

        public ActionResult ApplicationInstances()
        {
            try
            {
                return View(DescribeNodes());
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }

        //
        // GET: /applicationgrid/startapplication
        public ActionResult StartApplication(int applicationId)
        {
            try
            {
                StartApplicationModel model = new StartApplicationModel();
                Application app = GetApplication(applicationId);
                if(app != null)
                {
                    model.ApplicationId = applicationId;
                    model.Name = app.Name;
                    model.NumberOfInstances = 1;
                    ViewData["Tenants"] = PrepareTenants(app.Tenants);
                    return View("StartApplication", model);
                }
                else
                {
                    throw new MonoscapeException("Application not found");
                }
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }

        private object PrepareTenants(List<Tenant> list)
        {
            if ((list != null) && (list.Count > 0))
            {
                string text = string.Empty;
                foreach (Tenant tenant in list)
                {
                    text += tenant.Name;
                    if (list.IndexOf(tenant) < (list.Count - 1))
                        text += ", ";
                }
                return text;
            }
            return "No tenants defined";
        }

        //
        // POST: /applicationgrid/startapplication
        [HttpPost]
        public ActionResult StartApplication(StartApplicationModel model)
        {
            try
            {
                ApStartApplicationRequest request = new ApStartApplicationRequest(Credentials);
                request.ApplicationId = model.ApplicationId;
                request.TenantName = model.TenantName;
                request.NumberOfInstances = model.NumberOfInstances;
                EndPoints.ApDashboardService.StartApplication(request);
                return RedirectToAction("ApplicationInstances");
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }

        //
        // GET: /applicationgrid/addapplicationtenant
        public ActionResult AddApplicationTenant(int applicationId)
        {
            try
            {                
                Application app = GetApplication(applicationId);
                if (app != null)
                {
                    Tenant tenant = new Tenant();
                    tenant.ApplicationId = app.Id;
                    tenant.UpperScaleLimit = 1;
                    ViewData["ApplicationName"] = app.Name;
                    return View("AddApplicationTenant", tenant);
                }
                else
                {
                    throw new MonoscapeException("Application not found");
                }
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }

        //
        // POST: /applicationgrid/addapplicationtenant
        [HttpPost]
        public ActionResult AddApplicationTenant(Tenant tenant)
        {
            try
            {
                ApAddApplicationTenantsRequest request = new ApAddApplicationTenantsRequest(Credentials);
                request.ApplicationId = tenant.ApplicationId;
                List<Tenant> tenants = new List<Tenant>();
                tenants.Add(tenant);
                request.Tenants = tenants;
                EndPoints.ApDashboardService.AddApplicationTenants(request);
                return RedirectToAction("ApplicationInfo", new { applicationId = tenant.ApplicationId });
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }

        //
        // GET: /applicationgrid/stopapplicationinstance
        public ActionResult StopApplicationInstance(int nodeId, int applicationId, int instanceId)
        {
            try
            {
                ApStopApplicationInstanceRequest request = new ApStopApplicationInstanceRequest(Credentials);
                request.NodeId = nodeId;
                request.ApplicationId = applicationId;
                request.InstanceId = instanceId;
                EndPoints.ApDashboardService.StopApplicationInstance(request);
                return RedirectToAction("ApplicationInstances");
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }    
		
		//
        // GET: /applicationgrid/runinstance/{imageId}
        public ActionResult RunInstance(string imageId)
        {
            try
            {
                Instance instance = new Instance();
                instance.ImageId = imageId;                
                ViewData["ImageName"] = FindImageName(DescribeImages(), instance.ImageId);
                return View(instance);
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }

        //
        // POST: /applicationgrid/runinstance
        [HttpPost]
        public ActionResult RunInstance(Instance instance)
        {
            try
            {
                ApRunInstancesRequest request = new ApRunInstancesRequest(Credentials);
                request.ImageId = instance.ImageId;
                request.InstanceType = instance.Type;                
                request.NoOfInstances = 1;
                ApRunInstancesResponse response = EndPoints.ApDashboardService.RunInstances(request);
                Reservation reservation = response.Reservation;
                
                if ((reservation != null) && ((reservation.Instances == null) || (reservation.Instances.Count < 1)))
                    return View("Reservation", reservation);
                else
                    return RedirectToAction("Instances");
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }

        //
        // GET: /applicationgrid/deregisterimage/{imageId}
        public ActionResult DeregisterImage(string imageId)
        {
            try
            {
                ApDeregisterImageRequest request = new ApDeregisterImageRequest(Credentials);
                request.ImageId = imageId;
                EndPoints.ApDashboardService.DeregisterImage(request);                
                return RedirectToAction("Images");
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }

        //
        // GET: /applicationgrid/assignpublicip/{instanceId}
        public ActionResult AssignPublicIp(string instanceId)
        {
            try
            {
                ApAllocateAddressRequest allocateRequest = new ApAllocateAddressRequest(Credentials);
                ApAllocateAddressResponse allocateResponse = EndPoints.ApDashboardService.AllocateAddress(allocateRequest);

                ApAssociateAddressRequest associateRequest = new ApAssociateAddressRequest(Credentials);
                associateRequest.InstanceId = instanceId;
                associateRequest.IpAddress = allocateResponse.Address;
                EndPoints.ApDashboardService.AssociateAddress(associateRequest);

                return RedirectToAction("Instances");
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }

        //
        // GET: /applicationgrid/terminateinstance/{instanceId}
        public ActionResult TerminateInstance(string instanceId)
        {
            try
            {
                ApTerminateInstanceRequest request = new ApTerminateInstanceRequest(Credentials);
                request.InstanceId = instanceId;
                EndPoints.ApDashboardService.TerminateInstance(request);
                return RedirectToAction("Instances");
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }

        //
        // GET: /applicationgrid/rebootinstance/{instanceId}
        public ActionResult RebootInstance(string instanceId)
        {
            try
            {
                ApRebootInstanceRequest request = new ApRebootInstanceRequest(Credentials);
                request.InstanceId = instanceId;
                EndPoints.ApDashboardService.RebootInstance(request);
                return RedirectToAction("Instances");
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }

        //
        // GET: /applicationgrid/consoleoutput/{instanceId}
        public ActionResult ConsoleOutput(string instanceId)
        {
            try
            {
                ApGetConsoleOutputRequest request = new ApGetConsoleOutputRequest(Credentials);
                request.InstanceId = instanceId;
                ApGetConsoleOutputResponse response = EndPoints.ApDashboardService.GetConsoleOutput(request);
                ConsoleOutput output = new ConsoleOutput();
                output.InstanceId = response.ConsoleOutput.InstanceId;
                output.Output = ApCloudControllerUtil.DecodeConsoleOutput(response.ConsoleOutput.Output);
                output.Timestamp = response.ConsoleOutput.Timestamp;
                return View(output);
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }
		
		//
        // GET: /applicationgrid/uploadapplication/
        public ActionResult UploadApplication()
        {
            try
            {                
                return View(new Application());
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }

        //
        // POST: /applicationgrid/uploadapplication/
        [HttpPost]
        public ActionResult UploadApplication(Application application)
        {
            try
            {
                if (!IsApplicationValid(application))
                {
                    return View(application);
                }

                // Check application exists in Application Grid
                ApApplicationExistsRequest request = new ApApplicationExistsRequest(Credentials);
                ApApplicationExistsResponse response = EndPoints.ApDashboardService.ApplicationExists(request);

                if (response.Exists)
                {
                    ModelState.AddModelError("Application", "Application version already exists.");
                    return View(application);
                }
                else
                {
                    HttpPostedFileBase file = Request.Files["applicationPackage"];
                    application.FileName = file.FileName;
                    UploadFile(file.InputStream, application);                    
                    return RedirectToAction("Applications");
                }
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }

        private bool IsApplicationValid(Application application)
        {
            bool valid = true;
            if (string.IsNullOrEmpty(application.Name))
            {
                ModelState.AddModelError("Name", "Application name is required");
                valid = false;
            }
            if (string.IsNullOrEmpty(application.Version))
            {
                ModelState.AddModelError("Version", "Application version is required");
                valid = false;
            }
            if (Request.Files["applicationPackage"].ContentLength == 0)
            {
                ModelState.AddModelError("applicationPackage", "Application package is required");
                valid = false;
            }
            return valid;
        }
		
		        //
        // GET: /applicationgrid/removeapplication/{applicationId}
        public ActionResult RemoveApplication(int applicationId)
        {
            try
            {
                ApRemoveApplicationRequest request = new ApRemoveApplicationRequest(Credentials);
                request.ApplicationId = applicationId;
                EndPoints.ApDashboardService.RemoveApplication(request);
                return RedirectToAction("Applications");
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }        
    }
}
