using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monoscape.Common;
using Monoscape.NodeController.Api;
using Monoscape.Common.Exceptions;

namespace Monoscape.NodeController
{
    class JavaApacheCartridgeFactory
    {
        public static INodeCartridge GetHandler(ApplicationType applicationType)
        {
            switch (applicationType)
            {
                case ApplicationType.Mono:
                    return new JavaApacheCartridge();
                default:
                    throw new MonoscapeException("Unknown application type found");
            }
        }
    }
}
