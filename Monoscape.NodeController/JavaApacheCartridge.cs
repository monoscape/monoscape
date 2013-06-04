using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monoscape.NodeController.Api;
using Monoscape.NodeController.Api.Services.ApplicationGrid.Model;

namespace Monoscape.NodeController
{
    class JavaApacheCartridge : INodeCartridge
    {
        public NcDeployApplicationResponse DeployApplication(NcDeployApplicationRequest request)
        {
            throw new NotImplementedException();
        }

        public NcStartApplicationInstancesResponse StartApplicationInstances(NcStartApplicationInstancesRequest request)
        {
            throw new NotImplementedException();
        }

        public NcStopApplicationResponse StopApplicationInstance(NcStopApplicationRequest request)
        {
            throw new NotImplementedException();
        }

        public NcDescribeApplicationsResponse DescribeApplications(NcDescribeApplicationsRequest request)
        {
            throw new NotImplementedException();
        }

        public NcApplicationExistsResponse ApplicationExists(NcApplicationExistsRequest request)
        {
            throw new NotImplementedException();
        }

        public NcAddApplicationResponse AddApplication(NcAddApplicationRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
