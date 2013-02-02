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
using System.ServiceModel;
using Monoscape.Common;
using Monoscape.Common.Exceptions;
using Monoscape.LoadBalancerController.Api.Services.ApplicationGrid;
using Monoscape.LoadBalancerController.Api.Services.Dashboard;
using Monoscape.LoadBalancerController.Api.Services.LoadBalancerWeb;
using Monoscape.LoadBalancerController.Runtime;
using Monoscape.LoadBalancerController.Services.ApplicationGrid;
using Monoscape.LoadBalancerController.Services.Dashboard;
using Monoscape.LoadBalancerController.Services.LoadBalancerWeb;

namespace Monoscape.LoadBalancerController
{
    internal class ControllerService : IDisposable
    {
        private MonoscapeServiceHost gridServiceHost;
        private MonoscapeServiceHost dashboardServiceHost;
        private MonoscapeServiceHost loadBalancerWebServiceHost;

        public void Run()
        {
            try
            {
                Console.WriteLine("Monoscape Load Balancer Controller");
                Console.WriteLine("Version: 1.0.0.0");
                String hostName = MonoscapeUtil.FindHostName();
                String hostIpAddress = MonoscapeUtil.FindHostIpAddress().ToString();
                Console.WriteLine("Host Name: " + hostName);
                Console.WriteLine("Host IP Address: " + hostIpAddress);
                if (MonoscapeUtil.IsRunningOnMono())
                    Console.WriteLine("Mono: " + MonoscapeUtil.GetMonoRuntime());
                Console.WriteLine(".NET Runtime Version: " + MonoscapeUtil.GetDotNetRuntime());
                Console.WriteLine("Operating System: " + Environment.OSVersion.VersionString + Environment.NewLine);

                Initialize();
                StartApplicationGridService();
                StartDashboardService();
                StartLoadBalancerWebService();

                Console.WriteLine("Press Enter to stop...");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Log.Error(this, e.Message);
                Console.ReadLine();
            }
            finally
            {
                Dispose();
            }
        }

        private void StartApplicationGridService()
        {
            try
            {
                if (gridServiceHost == null)
                {
                    gridServiceHost = MonoscapeServiceHost.CreateHost(typeof(LbApplicationGridService));
                    var binding = MonoscapeServiceHost.GetBinding();
                    gridServiceHost.AddServiceEndpoint(typeof(ILbApplicationGridService), binding, Settings.LbApplicationGridServiceURL);
                    gridServiceHost.Open();
                    Console.WriteLine("Application Grid service started at: " + Settings.LbApplicationGridServiceURL);
                }
            }
            catch (AddressAlreadyInUseException)
            {
            }
            catch (CommunicationException ex)
            {
                if (gridServiceHost != null)
                    gridServiceHost.Abort();
                throw new MonoscapeException("Could not start service host: LbApplicationGridService", ex);
            }
            catch (TimeoutException ex)
            {
                if (gridServiceHost != null)
                    gridServiceHost.Abort();
                throw new MonoscapeException("Could not start service host: LbApplicationGridService", ex);
            }
            catch (Exception)
            {
                if (gridServiceHost != null)
                    gridServiceHost.Abort();
                throw;
            }
        }

        private void StartDashboardService()
        {
            try
            {
                if (dashboardServiceHost == null)
                {
                    dashboardServiceHost = MonoscapeServiceHost.CreateHost(typeof(LbDashboardService));
                    var binding = MonoscapeServiceHost.GetBinding();
                    dashboardServiceHost.AddServiceEndpoint(typeof(ILbDashboardService), binding, Settings.LbDashboardServiceURL);
                    dashboardServiceHost.Open();
                    Console.WriteLine("Dashboard service started at: " + Settings.LbDashboardServiceURL);
                }
            }
            catch (AddressAlreadyInUseException)
            {
            }
            catch (CommunicationException ex)
            {
                if (dashboardServiceHost != null)
                    dashboardServiceHost.Abort();
                throw new MonoscapeException("Could not start service host: LbDashboardService", ex);
            }
            catch (TimeoutException ex)
            {
                if (dashboardServiceHost != null)
                    dashboardServiceHost.Abort();
                throw new MonoscapeException("Could not start service host: LbDashboardService", ex);
            }
            catch (Exception)
            {
                if (dashboardServiceHost != null)
                    dashboardServiceHost.Abort();
                throw;
            }
        }

        private void StartLoadBalancerWebService()
        {
            try
            {
                if (loadBalancerWebServiceHost == null)
                {
                    loadBalancerWebServiceHost = MonoscapeServiceHost.CreateHost(typeof(LbLoadBalancerWebService));
                    var binding = MonoscapeServiceHost.GetBinding();
                    loadBalancerWebServiceHost.AddServiceEndpoint(typeof(ILbLoadBalancerWebService), binding, Settings.LbLoadBalancerWebServiceURL);
                    loadBalancerWebServiceHost.Open();
                    Console.WriteLine("Load Balancer Web service started at: " + Settings.LbLoadBalancerWebServiceURL);
                }
            }
            catch (AddressAlreadyInUseException)
            {
            }
            catch (CommunicationException ex)
            {
                if (loadBalancerWebServiceHost != null)
                    loadBalancerWebServiceHost.Abort();
                throw new MonoscapeException("Could not start service host: LbApplicationGridService", ex);
            }
            catch (TimeoutException ex)
            {
                if (loadBalancerWebServiceHost != null)
                    loadBalancerWebServiceHost.Abort();
                throw new MonoscapeException("Could not start service host: LbApplicationGridService", ex);
            }
            catch (Exception)
            {
                if (loadBalancerWebServiceHost != null)
                    loadBalancerWebServiceHost.Abort();
                throw;
            }
        }

        private void Initialize()
        {
            Initializer.Initialize();
        }

        public void Dispose()
        {
            Console.WriteLine("Stopping Load Balancer Controller services...");
            if (gridServiceHost != null)
                gridServiceHost.Close();
            if (dashboardServiceHost != null)
                dashboardServiceHost.Close();
            if (loadBalancerWebServiceHost != null)
                loadBalancerWebServiceHost.Close();
            Console.WriteLine("Node Controller stopped.");
        }
    }
}
