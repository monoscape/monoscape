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
 *  2013/04/15 Imesh Gunaratne <imesh@monoscape.org> Created.
 */

using System;
using Monoscape.NodeController.Api;
using Monoscape.Common.Model;
using Monoscape.Common;
using Monoscape.Common.Exceptions;

namespace Monoscape.NodeController
{
    public static class NodeCartridgeFactory
    {
        public static INodeCartridge GetHandler(ApplicationType applicationType)
        {
            switch (applicationType)
            {
                case ApplicationType.Mono:
                    return new MonoCartridge();
                default:
                    throw new MonoscapeException("Unknown application type found");
            }
        }
    }
}

