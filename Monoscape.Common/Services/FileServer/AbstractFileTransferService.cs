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
using System.IO;
using System.Linq;
using System.ServiceModel;
using Monoscape.Common.Exceptions;
using Monoscape.Common.Model;
using Monoscape.Common.Services.FileServer.Model;
using Monoscape.Common.WCFExtensions;
using ICSharpCode.SharpZipLib.Zip;

namespace Monoscape.Common.Services.FileServer
{
    public abstract class AbstractFileTransferService : ExceptionMarshallingBehavior, IFileTransferService
    {
        #region Abstract Protected Methods
        protected abstract string GetApplicationStorePath();

        protected abstract void ApplicationUploaded(Application application);
        #endregion

        public abstract ApplicationExistsResponse ApplicationExists(ApplicationExistsRequest request);

        public DownloadApplicationResponse DownloadApplication(DownloadApplicationRequest request)
        {
            Log.Info(this, "DownloadApplication()");

            try
            {
                string localFileName = request.FileMetaData.LocalFileName;
                string basePath = GetApplicationStorePath();
                string serverFileName = Path.Combine(basePath, request.FileMetaData.RemoteFileName);
                Stream fileStream = new FileStream(serverFileName, FileMode.Open);
                return new DownloadApplicationResponse(new ApplicationMetadata(localFileName, serverFileName), fileStream);
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        public UploadApplicationResponse UploadApplication(UploadApplicationRequest request)
        {
            Log.Info(this, "UploadApplication()");

            try
            {
                UploadApplicationResponse response = new UploadApplicationResponse();

                string basePath = GetApplicationStorePath();
                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                string serverFileName = Path.Combine(basePath, request.Metadata.RemoteFileName);
                using (FileStream outfile = new FileStream(serverFileName, FileMode.Create))
                {
                    const int bufferSize = 65536; // 64K

                    Byte[] buffer = new Byte[bufferSize];
                    int bytesRead = request.FileByteStream.Read(buffer, 0, bufferSize);

                    while (bytesRead > 0)
                    {
                        outfile.Write(buffer, 0, bytesRead);
                        bytesRead = request.FileByteStream.Read(buffer, 0, bufferSize);
                    }
                    outfile.Close();
                }
                
                //using (ZipFile zip = ZipFile.Read(serverFileName, options))
                {
                    if (!MonoscapeUtil.WebConfigExistsInRoot(serverFileName))
                    {                        
                        if (File.Exists(serverFileName))
                            File.Delete(serverFileName);
                        throw new MonoscapeException("Application package is not valid. Re-package the application excluding any project folders and try again.");
                    }
                }

                Application application = new Application();
                application.Id = request.Metadata.ApplicationId;
                application.Name = request.Metadata.ApplicationName;
                application.Version = request.Metadata.ApplicationVersion;
                application.FileName = Path.GetFileName(serverFileName);
                application.State = ApplicationState.Uploaded;

                ApplicationUploaded(application);
                return response;
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }
        }

        public ApRemoveApplicationResponse RemoveApplication(ApRemoveApplicationRequest request)
        {
            Log.Info(this, "RemoveApplication()");
            throw new NotImplementedException();
        }
    }
}

