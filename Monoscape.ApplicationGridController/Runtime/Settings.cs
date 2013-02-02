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
using Monoscape.Common.Model;

namespace Monoscape.ApplicationGridController.Runtime
{
	public class Settings
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
		
        // Service Urls
		public static string DashboardServiceURL { get; private set; }
        public static string NodeControllerServiceURL { get; private set; }
		public static string FileServerServiceURL { get; private set; }
		public static string FileServerServiceNetTcpURL { get; private set; }
		public static string FileServerServiceNetPipeURL { get; private set; }
		
		public static string ApplicationStorePath { get; private set; }
		public static string ApplicationStoreFolder { get; private set; }
        public static string SQLiteConnectionString { get; private set; }
        
        // Sockets
        public static int ApCcFileReceiveSocketPort { get; set; }
        public static int ApFileReceiveSocket { get; set; }
        public static int NcFileTransferSocketPort { get; set; }

        // End Point Urls
        public static string LbApplicationGridEndPointUrl { get; set; }
        public static string NodeFileServerEndPointURL { get; private set; }
        public static string NodeEndPointURL { get; private set; }

        // Iaas Settings
        public static string IaasName { get; private set; }
        public static string IaasAccessKey { get; private set; }
        public static string IaasSecretKey { get; private set; }
        public static string IaasServiceURL { get; private set; }
        public static string IaasKeyName { get; private set; }
        public static IaasClientType IaasClientType = IaasClientType.EC2;
		
		public static void Initialize(ApplicationGridSettings settings)
		{
			MonoscapeAccessKey = settings.MonoscapeAccessKey;
			MonoscapeSecretKey = settings.MonoscapeSecretKey;
			
            DashboardServiceURL = settings.DashboardServiceURL;
			NodeControllerServiceURL = settings.NodeControllerServiceURL;
			FileServerServiceURL = settings.FileServerServiceURL;
			FileServerServiceNetTcpURL = settings.FileServerServiceNetTcpURL;
			FileServerServiceNetPipeURL = settings.FileServerServiceNetPipeURL;
			
            ApplicationStorePath = settings.ApplicationStorePath;
			ApplicationStoreFolder = settings.ApplicationStoreFolder;
			SQLiteConnectionString = settings.SQLiteConnectionString;

            LbApplicationGridEndPointUrl = settings.LbApplicationGridEndPointUrl;
            NodeFileServerEndPointURL = settings.NodeFileServerEndPointURL;
            NodeEndPointURL = settings.NodeEndPointURL;
            
            ApCcFileReceiveSocketPort = settings.ApFileReceiveSocketPort;            
            NcFileTransferSocketPort = settings.NcFileTransferSocketPort;

            IaasName = settings.IaasName;
            IaasAccessKey = settings.IaasAccessKey;
            IaasSecretKey = settings.IaasSecretKey;
            IaasServiceURL = settings.IaasServiceURL;
            IaasKeyName = settings.IaasKeyName;
            IaasClientType = settings.IaasClientType;
		}        
    }
}

