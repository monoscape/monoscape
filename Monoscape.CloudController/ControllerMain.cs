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
using Monoscape.Common;

namespace Monoscape.CloudController
{
    class ControllerMain
    {
        private static ControllerService service;

        public static void Main(string[] args)
        {
            if (!MonoscapeUtil.IsRunningOnMono())
            {
                // Subscribe to Win32 process exit event
                Win32.HandlerRoutine hr = new Win32.HandlerRoutine(ProcessExit_Event);
                Win32.SetConsoleCtrlHandler(hr, true);
            }

            service = new ControllerService();
            service.Run();
        }

        static Boolean ProcessExit_Event(Win32.CtrlTypes CtrlType)
        {
            Log.Info(typeof(ControllerMain), "Closing ServiceMain process");
            service.Dispose();
            return true;
        }
    }
}
