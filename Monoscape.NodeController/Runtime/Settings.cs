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
using System.Linq;
using System.Text;
using Monoscape.Common.Model;

namespace Monoscape.NodeController.Runtime
{
    public static class Settings
    {
        public static string MonoscapeAccessKey { get; private set; }
        public static string MonoscapeSecretKey { get; private set; }
        private static MonoscapeCredentials credentials_;
        public static MonoscapeCredentials Credentials
        {
            get
            {
                if (credentials_ == null)
                    credentials_ = new MonoscapeCredentials(MonoscapeAccessKey, MonoscapeSecretKey);
                return credentials_;
            }
        }
        public static string ApplicationGridServiceUrl { get; private set; }        
        public static string FileServerServiceUrl { get; set; }
        public static string FileServerServiceNetTcpURL { get; set; }
        public static string FileServerServiceNetPipeURL { get; set; }

        public static string ApplicationGridEndPointURL { get; private set; }
        
        public static string ApplicationStoreFolder { get; set; }
        public static string ApplicationStorePath { get; set; }
        public static string ApplicationDeployFolder { get; set; }
        public static string ApplicationDeployPath { get; set; }
        public static string SQLiteConnectionString { get; set; }
        public static int InitialWebServerPort { get; set; }
        public static string WindowsXSPWebServerPath { get; set; }
        public static string UnixXSPWebServerPath { get; set; }

		public static int NcApFileReceiveSocketPort { get; set; }

        public static void Initialize(NodeControllerSettings settings)
        {
            MonoscapeAccessKey = settings.MonoscapeAccessKey;
            MonoscapeSecretKey = settings.MonoscapeSecretKey;
            ApplicationGridServiceUrl = settings.ApplicationGridServiceURL;
            ApplicationGridEndPointURL = settings.ApplicationGridEndPointURL;
            FileServerServiceUrl = settings.FileServerServiceURL;
            FileServerServiceNetTcpURL = settings.FileServerServiceNetTcpURL;
            FileServerServiceNetPipeURL = settings.FileServerServiceNetPipeURL;
            ApplicationStoreFolder = settings.ApplicationStoreFolder;
            ApplicationStorePath = settings.ApplicationStorePath;
            ApplicationDeployFolder = settings.ApplicationDeployFolder;
            ApplicationDeployPath = settings.ApplicationDeployPath;
            SQLiteConnectionString = settings.SQLiteConnectionString;
            InitialWebServerPort = settings.InitialWebServerPort;
            WindowsXSPWebServerPath = settings.WindowsXSPWebServerPath;
            UnixXSPWebServerPath = settings.UnixXSPWebServerPath;			
			NcApFileReceiveSocketPort = settings.NcApFileReceiveSocketPort;
        }
    }
}
