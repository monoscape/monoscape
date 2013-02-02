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
using System.Text;

namespace Monoscape.ApplicationGridController.Iaas
{
    public class MonoscapeIaasConfig
    {
        private string accessKeyField;
        private string secretKeyField;
        private string serviceURLField;

        public string AccessKey
        {
            get { return accessKeyField; }
            set { accessKeyField = value; }
        }

        public string SecretKey
        {
            get { return secretKeyField; }
            set { secretKeyField = value; }
        }

        public string ServiceURL
        {
            get { return serviceURLField; }
            set { serviceURLField = value; }
        }

        public MonoscapeIaasConfig(string accessKey, string secretKey, string serviceURL)
        {
            this.AccessKey = accessKey;
            this.SecretKey = secretKey;
            this.ServiceURL = serviceURL;
        }
    }
}
