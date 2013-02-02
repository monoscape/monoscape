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
    public class Application : Entity
    {        
		[DataMember]
        public string Name { get; set; }
		
		[DataMember]
        public string Version { get; set; }
		
		[DataMember]
        public string FileName { get; set; }
		
		[DataMember]
        public ApplicationState State { get; set; }

        [DataMember]
        public List<Tenant> Tenants { get; set; }

		[DataMember]
        public List<ApplicationInstance> ApplicationInstances { get; set; }

        public override string ToString()
        {
            return String.Format("Name: {0} Version: {1}", Name, Version);
        }

        public Application Clone()
        {
            Application clone = new Application();
            clone.Id = Id;
            clone.Name = Name;
            clone.Version = Version;
            clone.FileName = FileName;
            clone.State = State;
                        
            clone.ApplicationInstances = new List<ApplicationInstance>();
            foreach (ApplicationInstance instance in ApplicationInstances)
                clone.ApplicationInstances.Add(instance.Clone());

            clone.RowState = RowState;
            clone.RowVersion = RowVersion;
            return clone;
        }
    }
}
