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
using Monoscape.Common.Services.FileServer;
using Monoscape.Common;
using Monoscape.Common.Model;
using Monoscape.Common.Services.FileServer.Model;
using Monoscape.NodeController.Runtime;

namespace Monoscape.NodeController.Services.FileServer
{
    //public class FileTransferService : AbstractFileTransferService
    //{
    //    #region Private Methods
    //    private bool ApplicationExists(string name, string version)
    //    {
    //        foreach (Application application in Database.Applications)
    //        {
    //            if (application.Name.Equals(name) && (application.Version.Equals(version)))
    //                return true;
    //        }
    //        return false;
    //    }
    //    #endregion

    //    #region Protected Methods
    //    protected override string GetApplicationStorePath()
    //    {
    //        return Settings.ApplicationStorePath;
    //    }        

    //    protected override void ApplicationUploaded(Application application)
    //    {
    //        Log.Info(typeof(FileTransferService), "Application " + application.Name + " received");
    //        if (!NodeControllerUtil.ApplicationExists(application))
    //        {
    //            Database.Applications.Add(application);
    //        }
    //    }
    //    #endregion

    //    public override ApplicationExistsResponse ApplicationExists(ApplicationExistsRequest request)
    //    {
    //        Log.Info(this, "ApplicationExists()");

    //        try
    //        {
    //            ApplicationExistsResponse response = new ApplicationExistsResponse();
    //            response.Exists = ApplicationExists(request.ApplicationName, request.ApplicationVersion);
    //            return response;
    //        }
    //        catch (Exception e)
    //        {
    //            Log.Error(this, e);
    //            throw e;
    //        }
    //    }
    //}
}