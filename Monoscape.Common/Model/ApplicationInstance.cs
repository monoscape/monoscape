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
    public class ApplicationInstance : Entity, IComparable
    {
		[DataMember]
        public int ApplicationId { get; set; }

        [DataMember]
        public string ApplicationName { get; set; }
		
		[DataMember]
        public int NodeId { get; set; }
		
		[DataMember]
        public int ProcessId { get; set; }

        [DataMember]
        public string IpAddress { get; set; }

        [DataMember]
        public int Port { get; set; }

        [DataMember]
        public string Url { get; set; }

        [DataMember]
        public Tenant Tenant { get; set; }

        [DataMember]
        public DateTime CreatedTime { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public int RequestCount { get; set; }

        public ApplicationInstance Clone()
        {
            ApplicationInstance clone = new ApplicationInstance();
            clone.Id = Id;
            clone.ApplicationId = ApplicationId;
            clone.ApplicationName = ApplicationName;
            clone.NodeId = NodeId;
            clone.ProcessId = ProcessId;
            clone.IpAddress = IpAddress;
            clone.Port = Port;
            clone.Url = Url;
            clone.Tenant = Tenant;
            clone.RowState = RowState;
            clone.RowVersion = RowVersion;
            clone.CreatedTime = CreatedTime;
            clone.State = State;
            clone.RequestCount = RequestCount;
            return clone;
        }

        public bool Equals(ApplicationInstance instance)
        {
            return ((NodeId == instance.NodeId) && (ApplicationId == instance.ApplicationId) && (Id == instance.Id));
        }

        public override string ToString()
        {
            return String.Format("Node ID: {0} ApplicationID: {1} InstanceID: {2} Url: {3}", NodeId, ApplicationId, Id, Url);
        }

        public int CompareTo(object obj)
        {
            if(obj is ApplicationInstance)
            {
                ApplicationInstance instance = (ApplicationInstance)obj;
                return (RequestCount.CompareTo(instance.RequestCount));
            }
            throw new ArgumentException("Object is not an Application Instance");
        }
    }
}
