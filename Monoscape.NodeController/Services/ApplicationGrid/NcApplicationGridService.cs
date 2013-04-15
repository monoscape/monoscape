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
using System.Diagnostics;
using System.IO;
using System.Linq;
using Monoscape.Common;
using Monoscape.Common.Exceptions;
using Monoscape.Common.Model;
using Monoscape.NodeController.Api.Services.ApplicationGrid;
using Monoscape.NodeController.Api.Services.ApplicationGrid.Model;
using Monoscape.NodeController.Runtime;

namespace Monoscape.NodeController.Services.ApplicationGrid
{
    public class NcApplicationGridService : MonoscapeService, INcApplicationGridService
    {
        #region Protected Properties
        protected override MonoscapeCredentials Credentials
        {
            get
            {
                return Settings.Credentials;
            }
        }
        #endregion

		public NcAddApplicationResponse AddApplication(NcAddApplicationRequest request)
        {
            Log.Info(this, "AddApplication()");

            try
            {
                Authenticate(request);
                var handler = NodeCartridgeFactory.GetHandler(request.ApplicationType);
                return handler.AddApplication(request);
            }
            catch(Exception e)
			{
				Log.Error(this, e);				
                throw e;
			}
        }

        public NcDeployApplicationResponse DeployApplication(NcDeployApplicationRequest request)
        {
            Log.Info(this, "DeployApplication()");

            try
            {
                Authenticate(request);
				var handler = NodeCartridgeFactory.GetHandler(request.ApplicationType);
                return handler.DeployApplication(request);
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        public NcStartApplicationInstancesResponse StartApplicationInstances(NcStartApplicationInstancesRequest request)
        {
            Log.Info(this, "StartApplicationInstances()");

            try
            {
                Authenticate(request);
				var handler = NodeCartridgeFactory.GetHandler(request.ApplicationType);
                return handler.StartApplicationInstances(request);
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        public NcStopApplicationResponse StopApplicationInstance(NcStopApplicationRequest request)
        {
            Log.Info(this, "StopApplicationInstance()");

            try
            {
                Authenticate(request);
                var handler = NodeCartridgeFactory.GetHandler(request.ApplicationType);
                return handler.StopApplicationInstance(request);
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        public NcDescribeApplicationsResponse DescribeApplications(NcDescribeApplicationsRequest request)
        {
            Log.Info(this, "DescribeApplications()");

            try
            {
                Authenticate(request);
                var handler = NodeCartridgeFactory.GetHandler(request.ApplicationType);
                return handler.DescribeApplications(request);
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        public NcApplicationExistsResponse ApplicationExists(NcApplicationExistsRequest request)
        {
            Log.Info(this, "ApplicationExists()");

            try
            {
                Authenticate(request);
                var handler = NodeCartridgeFactory.GetHandler(request.ApplicationType);
                return handler.ApplicationExists(request);
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }
    }
}
