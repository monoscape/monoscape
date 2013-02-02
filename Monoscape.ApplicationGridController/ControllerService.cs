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
using System.Threading;
using Monoscape.ApplicationGridController.Api.Services.Dashboard;
using Monoscape.ApplicationGridController.Api.Services.NodeController;
using Monoscape.ApplicationGridController.Runtime;
using Monoscape.ApplicationGridController.Scaling;
using Monoscape.ApplicationGridController.Services.Dashboard;
using Monoscape.ApplicationGridController.Services.NodeController;
using Monoscape.ApplicationGridController.Sockets;
using Monoscape.Common;
using Monoscape.Common.Exceptions;

namespace Monoscape.ApplicationGridController
{
    internal class ControllerService : IDisposable
    {
        #region Private Attributes
        // Service Hosts
        private MonoscapeServiceHost cloudControllerHost;
        private MonoscapeServiceHost nodeControllerHost;
        // Sockets
        private ApFileReceiveSocket ccFileReceiveSocket;
        // Threads
        private Thread scalingManagerThread;
        #endregion

        public void Run()
        {
            try
            {
                Console.WriteLine("Monoscape Application Grid Controller");
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
                StartNodeControllerService();
                StartCcFileReceiveSocket();
                StartScalingManagerThread();

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

        private void StartScalingManagerThread()
        {
            Console.WriteLine("Starting Scaling Manager thread...");
            ScalingManager mgr = new ScalingManager();
            scalingManagerThread = new Thread(new ThreadStart(mgr.MonitorRequestQueue));
            scalingManagerThread.Start();

            // Spin for a while waiting for the started thread to become alive
            while (!scalingManagerThread.IsAlive) ;

            Console.WriteLine("Scaling Manager thread started");
        }

        private void StartCcFileReceiveSocket()
        {
            ccFileReceiveSocket = new ApFileReceiveSocket(Settings.ApplicationStorePath, MonoscapeUtil.FindHostIpAddress(), Settings.ApCcFileReceiveSocketPort);
            ccFileReceiveSocket.Open();
        }

        private void Initialize()
        {
            Initializer.Initialize();
        }

        private void StartDashboardService()
        {
            try
            {
                cloudControllerHost = MonoscapeServiceHost.CreateHost(typeof(ApDashboardService));
                var binding = MonoscapeServiceHost.GetBinding();
                cloudControllerHost.AddServiceEndpoint(typeof(IApDashboardService), binding, Settings.DashboardServiceURL);
                cloudControllerHost.Open();
                Console.WriteLine("Dashboard service started at: " + Settings.DashboardServiceURL);
            }
            catch (CommunicationException ex)
            {
                if (cloudControllerHost != null)
                    cloudControllerHost.Abort();
                throw new MonoscapeException("Could not start service host: ApDashboardService", ex);
            }
            catch (TimeoutException ex)
            {
                if (cloudControllerHost != null)
                    cloudControllerHost.Abort();
                throw new MonoscapeException("Could not start service host: ApDashboardService", ex);
            }
            catch (Exception)
            {
                if (cloudControllerHost != null)
                    cloudControllerHost.Abort();
                throw;
            }
        }

        private void StartNodeControllerService()
        {
            try
            {
                nodeControllerHost = MonoscapeServiceHost.CreateHost(typeof(ApNodeControllerService));
                var binding = MonoscapeServiceHost.GetBinding();
                nodeControllerHost.AddServiceEndpoint(typeof(IApNodeControllerService), binding, Settings.NodeControllerServiceURL);
                nodeControllerHost.Open();
                Console.WriteLine("Node Controller service started at: " + Settings.NodeControllerServiceURL);
            }
            catch (CommunicationException ex)
            {
                if (nodeControllerHost != null)
                    nodeControllerHost.Abort();
                throw new MonoscapeException("Could not start service host: ApNodeControllerService", ex);
            }
            catch (TimeoutException ex)
            {
                if (nodeControllerHost != null)
                    nodeControllerHost.Abort();
                throw new MonoscapeException("Could not start service host: ApNodeControllerService", ex);
            }
            catch (Exception)
            {
                if (nodeControllerHost != null)
                    nodeControllerHost.Abort();
                throw;
            }
        }

        public void Dispose()
        {
            Console.WriteLine("Stopping Application Grid services...");
            if (cloudControllerHost != null)
                cloudControllerHost.Close(TimeSpan.FromSeconds(60));
            if (nodeControllerHost != null)
                nodeControllerHost.Close(TimeSpan.FromSeconds(60));
            if (ccFileReceiveSocket != null)
                ccFileReceiveSocket.Close();
            if (scalingManagerThread != null)
                scalingManagerThread.Abort();
            Console.WriteLine("Application Grid stopped.");
        }
    }
}
