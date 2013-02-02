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
using Monoscape.Common.Model;
using Monoscape.Common;
using System.IO;
using System.Diagnostics;

namespace Monoscape.NodeController.Runtime
{
    internal static class Database
    {
        private static object threadLock = new object();

        public static int lastWebServerPort_;
        public static int LastWebServerPort 
        { 
            get
            {
                lock(threadLock)
                {
                    return lastWebServerPort_;
                }
            }
            set
            {
                lock(threadLock)
                {
                    lastWebServerPort_ = value;
                }
            }
        }

        private static Node node_;
        public static Node Node 
        {
            get
            {
                lock (threadLock)
                {
                    return node_;
                }
            }
            set
            {
                lock (threadLock)
                {
                    node_ = value;
                }
            } 
        }

        public static List<Application> Applications
        {
            get
            {
                lock (threadLock)
                {
                    if (Node == null)
                        return null;
                    if (Node.Applications == null)
                        Node.Applications = new List<Application>();
                    return Node.Applications;
                }
            }
            private set
            {
                Node.Applications = value;
            }
        }

        private static List<Process> childProcesses_;
        public static List<Process> ChildProcesses
        {
            get
            {
                lock (threadLock)
                {
                    if (childProcesses_ == null)
                        childProcesses_ = new List<Process>();
                    return childProcesses_;
                }
            }
            private set
            {
                childProcesses_ = value;                
            } 
        }
    }
}
