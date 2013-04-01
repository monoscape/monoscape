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
using Monoscape.CloudController.External.Api.Services.ExternalSystem;
using Monoscape.CloudController.External.Api.Services.ExternalSystem.Model;

namespace Monoscape.CloudController.Services.ExternalSystem
{
    internal class ExternalSystemService : IExternalSystemService
    {
        public AddApplicationResponse AddApplication(AddApplicationRequest request)
        {
            throw new NotImplementedException();
        }

        public StartApplicationResponse StartApplication(StartApplicationRequest request)
        {
            throw new NotImplementedException();
        }

        public StopApplicationResponse StopApplication(StopApplicationRequest request)
        {
            throw new NotImplementedException();
        }

        public ApplicationExistsResponse ApplicationExists(ApplicationExistsRequest request)
        {
            throw new NotImplementedException();
        }

        public UpdateTenantUpperScaleLimitResponse UpdateTenantUpperScaleLimit(UpdateTenantUpperScaleLimitRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
