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
    public class ApplicationGridSettings
    {
        #region Monoscape Credentials
        [DataMember]
		public string MonoscapeAccessKey { get; set; }
		[DataMember]
        public string MonoscapeSecretKey { get; set; }
        #endregion

        #region Service Urls
        [DataMember]
        public string DashboardServiceURL { get; set; }
		[DataMember]
        public string NodeControllerServiceURL { get; set; }
		[DataMember]
		public string FileServerServiceURL { get; set; }
		[DataMember]
		public string FileServerServiceNetTcpURL { get; set; }
		[DataMember]
		public string FileServerServiceNetPipeURL { get; set; }
        #endregion

        [DataMember]
		public string ApplicationStorePath { get; set; }
		[DataMember]
		public string ApplicationStoreFolder { get; set; }
		[DataMember]
        public string SQLiteConnectionString { get; set; }
		
        #region End Points
        [DataMember]
        public string LbApplicationGridEndPointUrl { get; set; }
        [DataMember]
        public string NodeFileServerEndPointURL { get; set; }
		[DataMember]
        public string NodeEndPointURL { get; set; }
        #endregion

        #region Sockets
        [DataMember]
        public int ApFileReceiveSocketPort { get; set; }
        [DataMember]
        public int NcFileTransferSocketPort { get; set; }
        #endregion

        #region IaaS Settings
        [DataMember]
        public string IaasName { get; set; }
		[DataMember]
        public string IaasAccessKey { get; set; }
		[DataMember]
        public string IaasSecretKey { get; set; }
		[DataMember]
        public string IaasServiceURL { get; set; }
		[DataMember]
        public string IaasKeyName { get; set; }
		[DataMember]
        public IaasClientType IaasClientType = IaasClientType.EC2;
        #endregion
    }
}
