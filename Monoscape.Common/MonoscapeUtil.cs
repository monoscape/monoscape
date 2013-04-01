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
using System.Net;
using System.Diagnostics;

namespace Monoscape.Common
{
    public class MonoscapeUtil
    {
        public static bool IsRunningOnWindows()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsRunningOnMono()
        {
            return (Type.GetType("Mono.Runtime") != null);
        }

		public static bool WebConfigExistsInRoot(string zipFileName)
        {
            return SharpZip.FileExistsInRoot(zipFileName, "web.config", true);
        }
		
        public static IPAddress FindHostIpAddress()
        {
            string hostName = Dns.GetHostName();
            IPHostEntry entry = Dns.GetHostEntry(hostName);
            foreach (IPAddress address in entry.AddressList)
            {
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    // First inter network address is considered as the host ip address
                    return address;
                }
            }
            return null;
        }

        public static string FindHostName()
        {
            return Dns.GetHostName();
        }

		public static string GetOperatingSystem ()
		{
			return Environment.OSVersion.VersionString;
		}

        public static string GetDotNetRuntime()
        {
			return Environment.Version.ToString();
        }
		
		public static string GetMonoRuntime()
		{
			try
			{
				var p = new Process
                {
                    StartInfo = new ProcessStartInfo("mono")
                    {
                        RedirectStandardOutput = true,
                        RedirectStandardError = false,
                        UseShellExecute = false,
                        CreateNoWindow = true,
						Arguments = "--version"
                    }
                };
                if (p.Start())
				{
					string output = p.StandardOutput.ReadLine();
					return output;
				}
			}
			catch {	}
			return "Unknown";
		}

        public static string PrepareApplicationVirtualFolderName(string applicationName)
        {
            return applicationName.ToLower().Replace(" ", "-");
        }
    }
}
