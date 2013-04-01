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
using System.IO;
using log4net.Config;
using log4net;
using System.Reflection;

namespace Monoscape.Common
{
    public static class Log
    {
        private static bool enabled = true;
        private static bool info = enabled && false;
        private static bool debug = enabled && false;
        private static bool error = enabled && true;
        private static bool configured = false;

        private static ILog GetLogger(Type type)
        {
            if (!configured)
            {
                XmlConfigurator.Configure();
                configured = true;
            }
            return LogManager.GetLogger(type);
        }

        public static void Info(Type type, string message)
        {
            if (info)
                GetLogger(type).Info(message);
        }

        public static void Info(Object caller, string message)
        {
            if (info)
                GetLogger(caller.GetType()).Info(message);
        }

        public static void Debug(Type type, string message)
        {
            if (debug)
                GetLogger(type).Debug(message);
        }

        public static void Debug(Object caller, string message)
        {
            if (debug)
                GetLogger(caller.GetType()).Debug(message);
        }

        public static void Debug(Type type, string message, Exception e)
        {
            if (debug)
                GetLogger(type).Debug(message, e);
        }

        public static void Debug(Object caller, string message, Exception e)
        {
            if (debug)
                GetLogger(caller.GetType()).Debug(message, e);
        }

        public static void Error(Type type, string message)
        {
            if (error)
                GetLogger(type).Error(message);
        }

        public static void Error(Object caller, string message)
        {
            if (error)
                GetLogger(caller.GetType()).Error(message);
        }

        public static void Error(Type type, Exception e)
        {
            if (error)
                GetLogger(type).Error(e);
        }

        public static void Error(Object caller, Exception e)
        {
            if (error)
                GetLogger(caller.GetType()).Error(e);
        }

        public static void Error(Type type, string message, Exception e)
        {
            if (error)
                GetLogger(type).Error(message, e);
        }

        public static void Error(Object caller, string message, Exception e)
        {
            if (error)
                GetLogger(caller.GetType()).Error(message, e);
        }
    }
}
