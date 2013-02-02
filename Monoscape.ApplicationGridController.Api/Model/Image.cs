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

namespace Monoscape.ApplicationGridController.Model
{
	[DataContract]
    public class Image
    {
		[DataMember]
        public string ImageId { get; set; }

		[DataMember]
        public string Name { get; set; }

		[DataMember]
        public string State { get; set; }
		
		public override string ToString ()
		{
			return string.Format ("[Image: ImageId={0}, Name={1}, State={2}]", ImageId, Name, State);
		}
    }
}
