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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Monoscape.Dashboard.Models;
using System.IO;
using Monoscape.Common.Model;

namespace Monoscape.Dashboard.Runtime
{
    class Initializer
    {
        internal static void Initialize(MvcApplication mvcApplication)
        {
            AppSettingsReader reader = new AppSettingsReader();
            DashboardSettings settings = new DashboardSettings();
            settings.SiteTitle = (string)reader.GetValue("SiteTitle", typeof(string));
            settings.MonoscapeAccessKey = (string)reader.GetValue("MonoscapeAccessKey", typeof(string));
            settings.MonoscapeSecretKey = (string)reader.GetValue("MonoscapeSecretKey", typeof(string));
            settings.ApplicationGridEndPointURL = (string)reader.GetValue("ApplicationGridEndPointURL", typeof(string));
            settings.FileServerEndPointURL = (string)reader.GetValue("FileServerEndPointURL", typeof(string));
            settings.LoadBalancerEndPointURL = (string)reader.GetValue("LoadBalancerEndPointURL", typeof(string));
            settings.CloudControllerEndPointURL = (string)reader.GetValue("CloudControllerEndPointURL", typeof(string));
            settings.ApFileTransferSocketPort = (int)reader.GetValue("ApFileTransferSocketPort", typeof(int));
            
			Settings.Initialize(settings);
        }
    }
}