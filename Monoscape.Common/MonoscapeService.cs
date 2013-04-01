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
using Monoscape.Common.WCFExtensions;
using Monoscape.Common.Model;
using Monoscape.Common.Exceptions;
using System.ServiceModel;

namespace Monoscape.Common
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public abstract class MonoscapeService : ExceptionMarshallingBehavior
    {
        protected abstract MonoscapeCredentials Credentials { get; }

        public EchoResponse Echo(EchoRequest request)
        {
            return new EchoResponse();
        }

        protected void Authenticate(AbstractRequest request)
        {
            MonoscapeCredentials requestCredentials = request.Credentials;
            if ((requestCredentials == null) || (!requestCredentials.AccessKey.Equals(Credentials.AccessKey)) || (!requestCredentials.SecretKey.Equals(Credentials.SecretKey)))
            {
                Log.Error(this, "Monoscape request authentication failed!");
                throw new MonoscapeSecurityException("Invalid Monoscape credentials");
            }
            Log.Debug(this, "Monoscape request authenticated");
        }
    }
}
