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
using System.Web;
using System.Configuration;
using Monoscape.Common.Model;

namespace Monoscape.LoadBalancerController.Runtime
{
    internal static class Initializer
    {
        public static void Initialize()
        {
            AppSettingsReader reader = new AppSettingsReader();

            LoadBalancerSettings settings = new LoadBalancerSettings();
            settings.MonoscapeAccessKey = (string)reader.GetValue("MonoscapeAccessKey", typeof(string));
            settings.MonoscapeSecretKey = (string)reader.GetValue("MonoscapeSecretKey", typeof(string));
            settings.ApplicationGridServiceURL = (string)reader.GetValue("ApplicationGridServiceURL", typeof(string));
            settings.DashboardServiceURL = (string)reader.GetValue("DashboardServiceURL", typeof(string));
            settings.LoadBalancerWebServiceURL = (string)reader.GetValue("LoadBalancerWebServiceURL", typeof(string));

            Settings.MonoscapeAccessKey = settings.MonoscapeAccessKey;
            Settings.MonoscapeSecretKey = settings.MonoscapeSecretKey;
            Settings.LbApplicationGridServiceURL = settings.ApplicationGridServiceURL;
            Settings.LbDashboardServiceURL = settings.DashboardServiceURL;
            Settings.LbLoadBalancerWebServiceURL = settings.LoadBalancerWebServiceURL;        
        }
    }
}