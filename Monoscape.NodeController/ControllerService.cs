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
using System.Net;
using System.ServiceModel;
using Monoscape.ApplicationGridController.Api.Services.NodeController;
using Monoscape.ApplicationGridController.Api.Services.NodeController.Model;
using Monoscape.Common;
using Monoscape.Common.Exceptions;
using Monoscape.Common.Model;
using Monoscape.NodeController.Api.Services.ApplicationGrid;
using Monoscape.NodeController.Runtime;
using Monoscape.NodeController.Services.ApplicationGrid;
using Monoscape.NodeController.Sockets;
using System.Diagnostics;

namespace Monoscape.NodeController
{
    internal class ControllerService : IDisposable
    {
        private MonoscapeServiceHost gridHost;        
        private NcApFileReceiveSocket ncApFileReceiveSocket;

        public void Run()
        {
            try
            {
                Console.WriteLine("Monoscape Node Controller");
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
                SubscribeToApplicationGrid();
                StartApplicationGridService();
                StartNcApFileReceiveSocket();

                Console.WriteLine("Press Enter to stop...");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Log.Error(this, e.Message);                
            }
            finally
            {
                Dispose();
            }
        }

        private void StartNcApFileReceiveSocket()
        {
            ncApFileReceiveSocket = new NcApFileReceiveSocket(Settings.ApplicationStorePath, MonoscapeUtil.FindHostIpAddress(), Settings.NcApFileReceiveSocketPort);
            ncApFileReceiveSocket.Open();
        }

        private void SubscribeToApplicationGrid()
        {
            try
            {
                Console.WriteLine("Subscribing to Application Grid at: " + Settings.ApplicationGridEndPointURL);

                MonoscapeCredentials credentials = new MonoscapeCredentials(Settings.MonoscapeAccessKey, Settings.MonoscapeSecretKey);
                ApSubscribeNodeRequest request = new ApSubscribeNodeRequest(credentials);
                IPAddress hostIp = MonoscapeUtil.FindHostIpAddress();
                request.IpAddress = hostIp.ToString();
                request.IpAddress_ = hostIp;
                request.ApplicationGridServiceUrl = Settings.ApplicationGridServiceUrl;
                request.FileTransferServiceUrl = Settings.FileServerServiceUrl + "/wsHttp";
                IApNodeControllerService channel = EndPoints.GetApNodeControllerService();
                ApSubscribeNodeResponse response = channel.SubscribeNode(request);
                Database.Node = response.Node;

                Console.WriteLine("Subscribed successfully");
            }
            catch (EndpointNotFoundException e)
            {
                throw new MonoscapeException("Could not connect to the Application Grid", e);
            }
        }

        private void Initialize()
        {           
            Initializer.Initialize();
        }

        private void StartApplicationGridService()
        {            
            try
            {
                gridHost = MonoscapeServiceHost.CreateHost(typeof(NcApplicationGridService));
                var binding = MonoscapeServiceHost.GetBinding();
                gridHost.AddServiceEndpoint(typeof(INcApplicationGridService), binding, Settings.ApplicationGridServiceUrl);
                gridHost.Open();
                Console.WriteLine("Application Grid service started at: " + Settings.ApplicationGridServiceUrl);                
            }
            catch (CommunicationException ex)
            {
                if (gridHost != null)
                    gridHost.Abort();
                throw new MonoscapeException("Could not start service host: NcApplicationGridService", ex);
            }
            catch (TimeoutException ex)
            {
                if (gridHost != null)
                    gridHost.Abort();
                throw new MonoscapeException("Could not start service host: NcApplicationGridService", ex);
            }
            catch (Exception e)
            {
                if (gridHost != null)
                    gridHost.Abort();
                throw e;
            }
        }

        public void Dispose()
        {
            Console.WriteLine("Un-Subscribing from Application Grid...");
            UnSubscribeFromApplicationGrid();

            Console.WriteLine("Stopping Node Controller services...");
            if (gridHost != null)
                gridHost.Close();
            if (ncApFileReceiveSocket != null)
                ncApFileReceiveSocket.Close();

            Console.WriteLine("Stopping web servers...");
            StopWebServerProcesses();

            Console.WriteLine("Node Controller stopped.");
        }

        private void UnSubscribeFromApplicationGrid()
        {
            try
            {
                ApUnSubscribeNodeRequest request = new ApUnSubscribeNodeRequest(Settings.Credentials);
                request.IpAddress = Database.Node.IpAddress;
                EndPoints.GetApNodeControllerService().UnSubscribeNode(request);
            }
            catch (Exception e)
            {
                Log.Error(this, e);
            }
        }

        private void StopWebServerProcesses()
        {
            if (Database.Applications != null)
            {
                foreach (Application app in Database.Applications)
                {
                    if (app.ApplicationInstances != null)
                    {
                        foreach (ApplicationInstance instance in app.ApplicationInstances)
                        {
                            ProcessManager.KillProcessAndChildren(instance.ProcessId);
                        }
                    }
                }
            }
        }
    }
}
