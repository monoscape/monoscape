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
using System.Runtime.Serialization;

namespace Monoscape.Common.Services.FileServer.Model
{
    [DataContract(Namespace = "http://schemas.acme.it/2009/04")]
    public class ApplicationMetadata
    {
        public ApplicationMetadata(string localFileName, string remoteFileName)
        {
            this.LocalFileName = localFileName;
            this.RemoteFileName = remoteFileName;            
        }

        public ApplicationMetadata(string localFileName, string remoteFileName, int applicationId, string applicationName, string applicationVersion)
        {
            this.LocalFileName = localFileName;
            this.RemoteFileName = remoteFileName;
            this.ApplicationId = applicationId;
            this.ApplicationName = applicationName;
            this.ApplicationVersion = applicationVersion;
        }

        [DataMember(Name = "localFilename", Order = 1, IsRequired = false)]
        public string LocalFileName;

        [DataMember(Name = "remoteFilename", Order = 2, IsRequired = false)]
        public string RemoteFileName;

        [DataMember(Name = "applicationId", Order = 3, IsRequired = false)]
        public int ApplicationId;

        [DataMember(Name = "applicationName", Order = 4, IsRequired = false)]
        public string ApplicationName;

        [DataMember(Name = "applicationVersion", Order = 5, IsRequired = false)]
        public string ApplicationVersion;
    }
}
