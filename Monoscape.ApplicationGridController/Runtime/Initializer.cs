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
using Monoscape.Common;
using Monoscape.ApplicationGridController.Services.Dashboard;
using System.Configuration;
using System.IO;

namespace Monoscape.ApplicationGridController.Runtime
{
    public static class Initializer
    {
        public static void Initialize()
        {
			Log.Info(typeof(Initializer), "Initializing Application Grid Controller...");

            Settings.Initialize(ReadConfigSettings());
		    AuthenticateIaas();

            Log.Info(typeof(Initializer), "Application Grid Controller initialized");
        }

        private static ApplicationGridSettings ReadConfigSettings()
        {
            AppSettingsReader reader = new AppSettingsReader();

            ApplicationGridSettings settings = new ApplicationGridSettings();
            settings.MonoscapeAccessKey = (string)reader.GetValue("MonoscapeAccessKey", typeof(string));
            settings.MonoscapeSecretKey = (string)reader.GetValue("MonoscapeSecretKey", typeof(string));
            settings.DashboardServiceURL = (string)reader.GetValue("DashboardServiceURL", typeof(string));
            settings.NodeControllerServiceURL = (string)reader.GetValue("NodeControllerServiceURL", typeof(string));
            settings.FileServerServiceURL = (string)reader.GetValue("FileServerServiceURL", typeof(string));
            settings.FileServerServiceNetTcpURL = (string)reader.GetValue("FileServerServiceNetTcpURL", typeof(string));
            settings.FileServerServiceNetPipeURL = (string)reader.GetValue("FileServerServiceNetPipeURL", typeof(string));

            settings.ApplicationStoreFolder = (string)reader.GetValue("ApplicationStoreFolder", typeof(string));
            settings.ApplicationStorePath = Path.GetFullPath(settings.ApplicationStoreFolder);
            settings.SQLiteConnectionString = (string)reader.GetValue("SQLiteConnectionString", typeof(string));

            settings.LbApplicationGridEndPointUrl = (string)reader.GetValue("LbApplicationGridEndPointUrl", typeof(string));
            settings.NodeFileServerEndPointURL = (string)reader.GetValue("NodeFileServerEndPointURL", typeof(string));
            settings.NodeEndPointURL = (string)reader.GetValue("NodeEndPointURL", typeof(string));

            settings.ApFileReceiveSocketPort = (int)reader.GetValue("ApFileReceiveSocketPort", typeof(int));
            settings.NcFileTransferSocketPort = (int)reader.GetValue("NcFileTransferSocketPort", typeof(int));

            settings.IaasName = (string)reader.GetValue("IaasName", typeof(string));
            settings.IaasAccessKey = (string)reader.GetValue("IaasAccessKey", typeof(string));
            settings.IaasSecretKey = (string)reader.GetValue("IaasSecretKey", typeof(string));
            settings.IaasServiceURL = (string)reader.GetValue("IaasServiceURL", typeof(string));
            settings.IaasKeyName = (string)reader.GetValue("IaasKeyName", typeof(string));
            return settings;
        }             
		
		private static void AuthenticateIaas()
		{
			try
			{
				Log.Info(typeof(Initializer), "Authenticating " + Runtime.Settings.IaasName + "...");
                Log.Info(typeof(Initializer), "Service URL: " + Runtime.Settings.IaasServiceURL);
				ApDashboardService service = new ApDashboardService();				
				service.AuthorizeIaasClient();
                Log.Info(typeof(Initializer), "Authenticated " + Runtime.Settings.IaasName + " successfully");		
			}
			catch(Exception e)
			{
				Log.Error(typeof(Initializer), "Could not connect to " + Runtime.Settings.IaasServiceURL, e);
                // The exception is not thrown as it will terminate the start up process
			}
		}
    }
}
