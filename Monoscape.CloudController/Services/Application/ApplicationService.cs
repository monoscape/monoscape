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
using Monoscape.CloudController.External.Api.Services.Application;
using Monoscape.CloudController.External.Api.Model;
using Monoscape.CloudController.External.Api.Services.Application.Model;

namespace Monoscape.CloudController.Services.Application
{
    internal class ApplicationService : IApplicationService 
    {
        protected void Authenticate(AbstractApplicationRequest request)
        {
        }

        public GetTenantUpperScaleLimitResponse GetTenantUpperScaleLimit(GetTenantUpperScaleLimitRequest request)
        {
            GetTenantUpperScaleLimitResponse response = new GetTenantUpperScaleLimitResponse();
            return response;
        }

        public GetTenantCurrentScaleResponse GetTenantCurrentScale(GetTenantCurrentScaleRequest request)
        {
            GetTenantCurrentScaleResponse response = new GetTenantCurrentScaleResponse();
            return response;
        }
    }
}
