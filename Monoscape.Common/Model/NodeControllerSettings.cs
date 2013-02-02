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
using System.Runtime.Serialization;

namespace Monoscape.Common.Model
{
	[DataContract]
    public class NodeControllerSettings
    {
		[DataMember]
        public string MonoscapeAccessKey { get; set; }
		[DataMember]
        public string MonoscapeSecretKey { get; set; }
		[DataMember]
        public string ApplicationStoreFolder { get; set; }
		[DataMember]
        public string ApplicationStorePath { get; set; }
		[DataMember]
        public string ApplicationDeployFolder { get; set; }
		[DataMember]
        public string ApplicationDeployPath { get; set; }
		[DataMember]
        public string SQLiteConnectionString { get; set; }
        
        // Service URLs
		[DataMember]
        public string ApplicationGridServiceURL { get; set; }
		[DataMember]
        public string FileServerServiceURL { get; set; }
		[DataMember]
        public string FileServerServiceNetTcpURL { get; set; }
		[DataMember]
        public string FileServerServiceNetPipeURL { get; set; }	

        // End Point URLs
		[DataMember]
        public string ApplicationGridEndPointURL { get; set; }

        // Node Web Server Settings
        [DataMember]
        public int InitialWebServerPort { get; set; }
        [DataMember]
        public string WindowsXSPWebServerPath { get; set; }
        [DataMember]
        public string UnixXSPWebServerPath { get; set; }		
		[DataMember]
		public int NcApFileReceiveSocketPort { get; set; }
    }
}
