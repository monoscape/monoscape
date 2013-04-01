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
using Monoscape.Common;
using System.IO;
using System.Configuration;

namespace Monoscape.NodeController.Runtime
{
    public class Initializer
    {
        public static void Initialize()
        {
            Log.Info(typeof(Initializer), "Initializing Node Controller...");

            AppSettingsReader reader = new AppSettingsReader();

            NodeControllerSettings settings = new NodeControllerSettings();
            settings.MonoscapeAccessKey = (string)reader.GetValue("MonoscapeAccessKey", typeof(string));
            settings.MonoscapeSecretKey = (string)reader.GetValue("MonoscapeSecretKey", typeof(string));

            string hostIp = MonoscapeUtil.FindHostIpAddress().ToString();
            settings.ApplicationGridServiceURL = ((string)reader.GetValue("ApplicationGridServiceURL", typeof(string))).Replace("node-ipaddress", hostIp);
            settings.FileServerServiceURL = ((string)reader.GetValue("FileServerServiceURL", typeof(string))).Replace("node-ipaddress", hostIp);
            settings.FileServerServiceNetTcpURL = ((string)reader.GetValue("FileServerServiceNetTcpURL", typeof(string))).Replace("node-ipaddress", hostIp);
            settings.FileServerServiceNetPipeURL = ((string)reader.GetValue("FileServerServiceNetPipeURL", typeof(string))).Replace("node-ipaddress", hostIp);

            settings.ApplicationGridEndPointURL = ((string)reader.GetValue("ApplicationGridEndPointURL", typeof(string)));

            settings.ApplicationStoreFolder = (string)reader.GetValue("ApplicationStoreFolder", typeof(string));
            settings.ApplicationStorePath = Path.GetFullPath(settings.ApplicationStoreFolder);
            settings.ApplicationDeployFolder = (string)reader.GetValue("ApplicationDeployFolder", typeof(string));
            settings.ApplicationDeployPath = Path.GetFullPath(settings.ApplicationDeployFolder);
            settings.SQLiteConnectionString = (string)reader.GetValue("SQLiteConnectionString", typeof(string));
            settings.InitialWebServerPort = (int)reader.GetValue("InitialWebServerPort", typeof(int));
            settings.WindowsXSPWebServerPath = (string)reader.GetValue("WindowsXSPWebServerPath", typeof(string));
            settings.UnixXSPWebServerPath = (string)reader.GetValue("UnixXSPWebServerPath", typeof(string));
            settings.NcApFileReceiveSocketPort = (int)reader.GetValue("NcApFileReceiveSocketPort", typeof(int));

            Settings.Initialize(settings);
            
            Database.LastWebServerPort = Settings.InitialWebServerPort - 1;
            ClearApplicationStore();
            ClearWebRoot();
            
            Log.Info(typeof(Initializer), "Node Controller initialized");
        }

        private static void ClearApplicationStore()
        {
            try
            {
                if (Directory.Exists(Settings.ApplicationStorePath))
                    Directory.Delete(Settings.ApplicationStorePath, true);
                Directory.CreateDirectory(Settings.ApplicationStorePath);
            }
            catch 
            {
                Log.Error(typeof(Initializer), "Could not clear Application Store");
            }
        }

        private static void ClearWebRoot()
        {
            try
            {
                if (Directory.Exists(Settings.ApplicationDeployPath))
                    Directory.Delete(Settings.ApplicationDeployPath, true);
                Directory.CreateDirectory(Settings.ApplicationDeployPath);
            }
            catch
            {
                Log.Error(typeof(Initializer), "Could not clear Web Root");
            }
        }
    }
}
