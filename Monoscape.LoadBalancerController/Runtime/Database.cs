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
using Monoscape.Common.Model;
using Monoscape.Common.Models;

namespace Monoscape.LoadBalancerController.Runtime
{
    internal class Database
    {
        private static Object threadLock = new object();
        private static Database singleton;

        private RoutingMesh routingMesh_;
        private RoutingMesh routingMeshHistory_;
        
        private RequestQueue requestQueue_;
        private RequestQueue requestQueueHistory_;

        public RoutingMesh RoutingMesh
        {
            get
            {
                lock (threadLock)
                {
                    if (routingMesh_ == null)
                        routingMesh_ = new RoutingMesh();
                    return routingMesh_;
                }
            }
        }

        public RoutingMesh RoutingMeshHistory
        {
            get
            {
                lock (threadLock)
                {
                    if (routingMeshHistory_ == null)
                        routingMeshHistory_ = new RoutingMesh();
                    return routingMeshHistory_;
                }
            }
        }

        public RequestQueue RequestQueue
        {
            get
            {
                lock (threadLock)
                {
                    if (requestQueue_ == null)
                        requestQueue_ = new RequestQueue();
                    return requestQueue_;
                }
            }
        }

        public RequestQueue RequestQueueHistory
        {
            get
            {
                lock (threadLock)
                {
                    if (requestQueueHistory_ == null)
                        requestQueueHistory_ = new RequestQueue();
                    return requestQueueHistory_;
                }
            }
        }

        private Database()
        {
        }

        public static Database GetInstance()
        {
            lock (threadLock)
            {
                if (singleton == null)
                    singleton = new Database();
                return singleton;
            }
        }
    }
}