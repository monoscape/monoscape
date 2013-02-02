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
using System.Diagnostics;
using Monoscape.Common;
using System.Management;

namespace Monoscape.NodeController
{
    internal static class ProcessManager
    {
        public static bool KillProcessAndChildren(int pid)
        {
            bool killed = false;
            try
            {
                Process[] array = Process.GetProcesses();
                for (int i = 0; i < array.Length; i++)
                {
                    if (GetParentProcess(array[i].Id) == pid)
                    {
                        // Recursively kill all child processes                        
                        if (KillProcessAndChildren(array[i].Id) == true)
                            killed = true;
                    }
                }
                try
                {
                    Process process = Process.GetProcessById(pid);
                    if (process != null)
                    {
                        process.Kill();
                        return true;
                    }
                    return false;
                }
                catch { }
            }
            catch (ManagementException) { }
            catch (Exception e)
            {
                Log.Error(typeof(ProcessManager), e);
            }
            return killed;
        }

        private static int GetParentProcess(int Id)
        {
            int parentPid = 0;
            using (ManagementObject mo = new ManagementObject("win32_process.handle='" + Id.ToString() + "'"))
            {
                mo.Get();
                parentPid = Convert.ToInt32(mo["ParentProcessId"]);
            }
            return parentPid;
        }
    }
}
