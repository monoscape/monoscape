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
using Monoscape.Common.Model;

namespace Monoscape.LoadBalancerController.Runtime
{
    public class Settings
    {
        public static string MonoscapeAccessKey { get; set; }
        public static string MonoscapeSecretKey { get; set; }
        public static string LbApplicationGridServiceURL { get; set; }
        public static string LbDashboardServiceURL { get; set; }
        public static string LbLoadBalancerWebServiceURL { get; set; }

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
    }
}