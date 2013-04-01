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
using System.Diagnostics;
using System.IO;
using System.Linq;
using Monoscape.Common;
using Monoscape.Common.Exceptions;
using Monoscape.Common.Model;
using Monoscape.NodeController.Api.Services.ApplicationGrid;
using Monoscape.NodeController.Api.Services.ApplicationGrid.Model;
using Monoscape.NodeController.Runtime;

namespace Monoscape.NodeController.Services.ApplicationGrid
{
    public class NcApplicationGridService : MonoscapeService, INcApplicationGridService
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
        private ApplicationInstance StartWebServer(Application application, Tenant tenant, int port)
        {
            try
            {
                string xsp = Settings.WindowsXSPWebServerPath;
                if (MonoscapeUtil.IsRunningOnMono() && (Environment.OSVersion.Platform == PlatformID.Unix))
                    xsp = Settings.UnixXSPWebServerPath;

                string arguments = "--port " + port + " --applications /" + tenant.Name.Replace(" ","").ToLower() + "/" + MonoscapeUtil.PrepareApplicationVirtualFolderName(application.Name) + ":.";
                string workingDirectory = PrepareApplicationDeployPath(application, tenant.Name, port);
                var p = new Process()
                {
                    StartInfo = new ProcessStartInfo(xsp, arguments)
                    {
                        RedirectStandardOutput = false,
                        RedirectStandardError = false,
                        UseShellExecute = false,
                        CreateNoWindow = true,                        
                        WorkingDirectory = workingDirectory
                    }
                };
                if (p.Start())
                {
                    Database.LastWebServerPort = port;
                    ApplicationInstance instance = new ApplicationInstance();
                    instance.Id = FindNextInstanceId(application);
                    instance.ProcessId = p.Id;
                    instance.ApplicationId = application.Id;
                    instance.ApplicationName = application.Name;
                    instance.NodeId = Database.Node.Id;
                    instance.IpAddress = Database.Node.IpAddress;
                    instance.Port = port;
                    instance.Tenant = tenant;
                    instance.CreatedTime = DateTime.Now;
                    instance.State = "Started";
                    instance.Url = "http://" + instance.IpAddress + ":" + instance.Port + "/" + tenant.Name.Replace(" ","").ToLower() + "/" + MonoscapeUtil.PrepareApplicationVirtualFolderName(application.Name);

                    Database.ChildProcesses.Add(p);
                    Log.Info(typeof(NcApplicationGridService), "Started web server: " + instance.Url);
                    return instance;
                }
                else
                {
                    Database.ChildProcesses.Add(p);
                    Log.Error(typeof(NcApplicationGridService), "Could not start web server for application " + application.Name + " " + application.Version);
                    return null;
                }
            }
            catch (Exception e)
            {
                Log.Error(this, "Could not start web server");
                throw e;
            }
        }

        private int FindNextInstanceId(Application application)
        {
            int nextId = 1;
            if ((application.ApplicationInstances != null) && (application.ApplicationInstances.Count > 0))
            {
                ApplicationInstance lastInstance = application.ApplicationInstances.OrderByDescending(x => x.Id).FirstOrDefault();
                return lastInstance.Id + 1;
            }
            return nextId;
        }

        private int ExtractApplicationPackage(Application app, string tenantName)
        {
            try
            {
                string filePath = Path.Combine(Settings.ApplicationStorePath, app.FileName);
                if (!MonoscapeUtil.WebConfigExistsInRoot(filePath))                
                    throw new MonoscapeException("Application package is not valid. Re-package the application without any folders and try again.");

                int port = (Database.LastWebServerPort + 1);
                string extractPath = PrepareApplicationDeployPath(app, tenantName, port);

                if (Directory.Exists(extractPath))
                {
                    // Remove previously extracted application content
                    Directory.Delete(extractPath, true);
                    Directory.CreateDirectory(extractPath);
                }
                else if (!Directory.Exists(Settings.ApplicationDeployPath))
                {
                    // Create application deployment path
                    Directory.CreateDirectory(Settings.ApplicationDeployPath);
                }

                SharpZip.Extract(extractPath, filePath);
                return port;
            }
            catch (Exception e)
            {
                throw new MonoscapeException("Application package extraction failed", e);
            }
        }

        private static string PrepareApplicationDeployPath(Application app, string tenantName, int port)
        {
            return Settings.ApplicationDeployPath + Path.DirectorySeparatorChar + app.Name + " - " + app.Version + " - " + tenantName + " - " + port;
        }
        #endregion

        public NcDeployApplicationResponse DeployApplication(NcDeployApplicationRequest request)
        {
            Log.Info(this, "DeployApplication()");

            try
            {
                Authenticate(request);
                Application app = Database.Applications.Where(x => x.Id == request.ApplicationId).FirstOrDefault();
                if (app != null)
                {                    
                    NcDeployApplicationResponse response = new NcDeployApplicationResponse();
                    if ((app.Tenants != null) && (app.Tenants.Count > 0))
                    {
                        // Start application instance per tenant
                        foreach (Tenant tenant in app.Tenants)
                        {
                            int port = ExtractApplicationPackage(app, tenant.Name);
                            ApplicationInstance instance = StartWebServer(app, tenant, port);
                            if (instance != null)
                            {
                                response.Deployed = true;
                                response.Url = instance.Url;
                            }
                        }
                    }
                    else
                    {
                        // No tenants found, start default
                        Tenant tenant = new Tenant();
                        tenant.Name = "Default";
                        int port = ExtractApplicationPackage(app, tenant.Name);
                        ApplicationInstance instance = StartWebServer(app, tenant, port);
                        if (instance != null)
                        {
                            response.Deployed = true;
                            response.Url = instance.Url;
                        }
                    }
                    return response;
                }
                else
                {
                    throw new MonoscapeException("Application not found");
                }
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        public NcStartApplicationInstancesResponse StartApplicationInstances(NcStartApplicationInstancesRequest request)
        {
            Log.Info(this, "StartApplicationInstances()");

            try
            {
                Authenticate(request);
                Application app = Database.Applications.Where(x => x.Id == request.ApplicationId).FirstOrDefault();
                if (app != null)
                {
                    Tenant tenant = null;
                    if (string.IsNullOrEmpty(request.TenantName))
                    {
                        throw new MonoscapeException("Tenant name should be specified");
                    }
                    else
                    {
                        tenant = app.Tenants.Where(x => x.Name.Equals(request.TenantName)).FirstOrDefault();
                        if (tenant == null)
                            throw new MonoscapeException("Tenant " + request.TenantName + " not found");
                    }

                    List<ApplicationInstance> instancesStarted = new List<ApplicationInstance>();
                    NcStartApplicationInstancesResponse response = new NcStartApplicationInstancesResponse();
                    for (int i = 0; i < request.NumberOfInstances; i++)
                    {
                        // Extract application to a new folder
                        int port = ExtractApplicationPackage(app, tenant.Name);
                        // Start a web server instance
                        ApplicationInstance instance = StartWebServer(app, tenant, port);
                        if (instance != null)
                        {
                            // Add to database
                            if (app.ApplicationInstances == null)
                                app.ApplicationInstances = new List<ApplicationInstance>();
                            app.ApplicationInstances.Add(instance);
                            // Track instances started
                            instancesStarted.Add(instance.Clone());
                            // Add server url to the response
                            if (response.Urls == null)
                                response.Urls = new List<string>();
                            response.Urls.Add(instance.Url);
                        }
                    }

                    response.ApplicationInstances = instancesStarted;
                    return response;
                }
                else
                {
                    throw new MonoscapeException("Application not found");
                }
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        public NcStopApplicationResponse StopApplicationInstance(NcStopApplicationRequest request)
        {
            Log.Info(this, "StopApplicationInstance()");

            try
            {
                Authenticate(request);
                Application application = Database.Applications.Where(x => x.Id == request.ApplicationId).FirstOrDefault();
                if (application != null)
                {
                    ApplicationInstance instance = application.ApplicationInstances.Where(y => y.Id == request.InstanceId).FirstOrDefault();
                    if (instance != null)
                    {
                        ProcessManager.KillProcessAndChildren(instance.ProcessId);
                        application.ApplicationInstances.Remove(instance);
                        NcStopApplicationResponse response = new NcStopApplicationResponse();
                        return response;
                    }
                    else
                    {
                        throw new MonoscapeException("Application instance not found");
                    }
                }
                else
                {
                    throw new MonoscapeException("Application not found");
                }
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        public NcDescribeApplicationsResponse DescribeApplications(NcDescribeApplicationsRequest request)
        {
            Log.Info(this, "DescribeApplications()");

            try
            {
                Authenticate(request);
                NcDescribeApplicationsResponse response = new NcDescribeApplicationsResponse();
                response.Applications = Database.Applications;
                return response;
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        public NcApplicationExistsResponse ApplicationExists(NcApplicationExistsRequest request)
        {
            Log.Info(this, "ApplicationExists()");

            try
            {
                Authenticate(request);
                NcApplicationExistsResponse response = new NcApplicationExistsResponse();
                if (Database.Applications != null)
                {
                    Application app = Database.Applications.Where(x => x.Id == request.ApplicationId).FirstOrDefault();
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

        public NcAddApplicationResponse AddApplication(NcAddApplicationRequest request)
        {
            Log.Info(this, "AddApplication()");

            try
            {
                Log.Info(this, "Application " + request.Application.Name + " received");
                Authenticate(request);

                if (!ControllerUtil.ApplicationExists(request.Application))
                {
                    try
                    {                        
                        Database.Applications.Add(request.Application);
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
                return new NcAddApplicationResponse();
            }
            catch(Exception e)
			{
				Log.Error(this, e);				
                throw e;
			}
        }
    }
}
