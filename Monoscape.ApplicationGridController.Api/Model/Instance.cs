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

namespace Monoscape.ApplicationGridController.Model
{
	[DataContract]
    public class Instance
    {
		[DataMember]
        public string InstanceId { get; set; }
		
		[DataMember]
        public string ImageId { get; set; }
		
		[DataMember]
        public string PrivateDnsName { get; set; }
		
		[DataMember]
        public string IpAddress { get; set; }
		
		[DataMember]
        public string Type { get; set; }
		
		[DataMember]
        public string State { get; set; }
		
		public override string ToString ()
		{
			return string.Format ("[Instance: InstanceId={0}, ImageId={1}, PrivateDnsName={2}, IpAddress={3}, Type={4}, State={5}]", InstanceId, ImageId, PrivateDnsName, IpAddress, Type, State);
		}
    }
}
