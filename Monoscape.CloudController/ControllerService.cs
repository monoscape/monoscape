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
using System.Configuration;
using System.ServiceModel;
using Monoscape.CloudController.Api.Services.Dashboard;
using Monoscape.CloudController.Runtime;
using Monoscape.CloudController.Services.Dashboard;
using Monoscape.Common;
using Monoscape.Common.Exceptions;
using Monoscape.CloudController.Services.Application;
using Monoscape.CloudController.External.Api.Services.Application;
using Monoscape.CloudController.Services.ExternalSystem;
using Monoscape.CloudController.External.Api.Services.ExternalSystem;

namespace Monoscape.CloudController
{
    internal class ControllerService : IDisposable
    {
        #region Private Attributes
        private MonoscapeServiceHost dashboardHost;
        private MonoscapeServiceHost applicationHost;
        private MonoscapeServiceHost externalSysHost;
        #endregion

        public void Run()
        {
            try
            {
                Console.WriteLine("Monoscape Cloud Controller");
                Console.WriteLine("Version: 1.0.0.0");
                String hostName = MonoscapeUtil.FindHostName();
                String hostIpAddress = MonoscapeUtil.FindHostIpAddress().ToString();
                Console.WriteLine("Host Name: " + hostName);
                Console.WriteLine("Host IP Address: " + hostIpAddress);
                if (MonoscapeUtil.IsRunningOnMono())
                    Console.WriteLine("Mono: " + MonoscapeUtil.GetMonoRuntime());
                Console.WriteLine(".NET Runtime Version: " + MonoscapeUtil.GetDotNetRuntime());
                Console.WriteLine("Operating System: " + MonoscapeUtil.GetOperatingSystem() + Environment.NewLine);

                Initialize();
                StartDashboardService();
                StartApplicationService();
                StartExternalSystemService();

                Console.WriteLine("Press Enter to stop...");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                Console.ReadLine();
            }
            finally
            {
                Dispose();
            }
        }

        private void Initialize()
        {
            AppSettingsReader reader = new AppSettingsReader();
            Settings.SQLiteConnectionString = (string)reader.GetValue("SQLiteConnectionString", typeof(string));
            Settings.DashboardServiceURL = (string)reader.GetValue("DashboardServiceURL", typeof(string));
            Settings.ApplicationServiceURL = (string)reader.GetValue("ApplicationServiceURL", typeof(string));
            Settings.ExternalSystemServiceURL = (string)reader.GetValue("ExternalSystemServiceURL", typeof(string));
        }

        private void StartDashboardService()
        {
            try
            {
                dashboardHost = MonoscapeServiceHost.CreateHost(typeof(CcDashboardService));
                var binding = MonoscapeServiceHost.GetBinding();
                dashboardHost.AddServiceEndpoint(typeof(ICcDashboardService), binding, Settings.DashboardServiceURL);
                dashboardHost.Open();
                Console.WriteLine("Dashboard service started at: " + Settings.DashboardServiceURL);
            }
            catch (CommunicationException ex)
            {
                if (dashboardHost != null)
                    dashboardHost.Abort();
                throw new MonoscapeException("Could not start service host: CcDashboardService", ex);
            }
            catch (TimeoutException ex)
            {
                if (dashboardHost != null)
                    dashboardHost.Abort();
                throw new MonoscapeException("Could not start service host: CcDashboardService", ex);
            }
            catch (Exception)
            {
                if (dashboardHost != null)
                    dashboardHost.Abort();
                throw;
            }
        }

        private void StartApplicationService()
        {
            try
            {
                applicationHost = MonoscapeServiceHost.CreateHost(typeof(ApplicationService));
                var binding = MonoscapeServiceHost.GetBinding();
                applicationHost.AddServiceEndpoint(typeof(IApplicationService), binding, Settings.ApplicationServiceURL);
                applicationHost.Open();
                Console.WriteLine("Application service started at: " + Settings.ApplicationServiceURL);
            }
            catch (CommunicationException ex)
            {
                if (applicationHost != null)
                    applicationHost.Abort();
                throw new MonoscapeException("Could not start service host: ApplicationService", ex);
            }
            catch (TimeoutException ex)
            {
                if (applicationHost != null)
                    applicationHost.Abort();
                throw new MonoscapeException("Could not start service host: ApplicationService", ex);
            }
            catch (Exception)
            {
                if (applicationHost != null)
                    applicationHost.Abort();
                throw;
            }
        }

        private void StartExternalSystemService()
        {
            try
            {
                externalSysHost = MonoscapeServiceHost.CreateHost(typeof(ExternalSystemService));
                var binding = MonoscapeServiceHost.GetBinding();
                externalSysHost.AddServiceEndpoint(typeof(IExternalSystemService), binding, Settings.ExternalSystemServiceURL);
                externalSysHost.Open();
                Console.WriteLine("External system service started at: " + Settings.ExternalSystemServiceURL);
            }
            catch (CommunicationException ex)
            {
                if (externalSysHost != null)
                    externalSysHost.Abort();
                throw new MonoscapeException("Could not start service host: ExternalSystemService", ex);
            }
            catch (TimeoutException ex)
            {
                if (externalSysHost != null)
                    externalSysHost.Abort();
                throw new MonoscapeException("Could not start service host: ExternalSystemService", ex);
            }
            catch (Exception)
            {
                if (externalSysHost != null)
                    externalSysHost.Abort();
                throw;
            }
        }

        public void Dispose()
        {
            Console.WriteLine("Stopping Cloud Controller services...");

            if (dashboardHost != null)
                dashboardHost.Close(TimeSpan.FromSeconds(60));

            Console.WriteLine("Cloud Controller stopped.");
        }
    }
}
