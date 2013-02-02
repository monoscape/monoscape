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
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace Monoscape.Common
{
	public class MonoscapeServiceHost : ServiceHost
	{		
		private MonoscapeServiceHost ()
		{
		}
		
		private MonoscapeServiceHost(Type serviceType) : base(serviceType)
		{
		}
		
		private MonoscapeServiceHost(Type serviceType, Uri[] uriArray) : base(serviceType, uriArray)
		{
		}


        private static void SetTimeOuts(Binding binding)
        {
            binding.SendTimeout = TimeSpan.FromHours(1);
            binding.ReceiveTimeout = TimeSpan.FromHours(1);
        }

        public static CustomBinding GetBinding()
		{
			// Same binding should be used in both service and service client.
            //var binding = new BasicHttpBinding();            
            //binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            //SetTimeOuts(binding);
            //return binding;            

            // Instantiate message encoding element and configure
            TextMessageEncodingBindingElement text = new TextMessageEncodingBindingElement();
            text.MessageVersion = MessageVersion.Soap11;
            text.ReaderQuotas.MaxStringContentLength = int.MaxValue;

            // Instantiate transport element and configure
            HttpTransportBindingElement http = new HttpTransportBindingElement();
            http.TransferMode = TransferMode.Buffered;
            http.UseDefaultWebProxy = true;

            CustomBinding binding = new CustomBinding();
            binding.Name = "MonoscapeHttpBinding";
            binding.Elements.Add(text);
            binding.Elements.Add(http);
            SetTimeOuts(binding);

            return binding;
		}
		
		public static NetTcpBinding GetNetTcpBinding()
		{
			var binding = new NetTcpBinding();
            SetTimeOuts(binding);
            return binding;
		}	
		
		public static NetNamedPipeBinding GetNetNamedPipeBinding()
		{
			var binding = new NetNamedPipeBinding();
            SetTimeOuts(binding);
            return binding;
		}
		
		static void EnableDebugging (MonoscapeServiceHost host)
		{
			// Enable IncludeExceptionDetailInFaults
			ServiceDebugBehavior debug = host.Description.Behaviors.Find<ServiceDebugBehavior>();
			if (debug == null)
			    host.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });
			else
			{
			    if (!debug.IncludeExceptionDetailInFaults)
			        debug.IncludeExceptionDetailInFaults = true;
			}
		}
		
		public static MonoscapeServiceHost CreateHost(Type serviceType)
        {
            var host = new MonoscapeServiceHost(serviceType);            
			EnableDebugging (host);
            return host;
        }
		
		public static MonoscapeServiceHost CreateFileServerHost (Type serviceType, string httpAddress, string tcpAddress, string pipeAddress)
		{
			Uri httpUri = new Uri(httpAddress);
			Uri tcpUri = new Uri(tcpAddress);
			Uri pipeUri = new Uri(pipeAddress);
			Uri[] uriArray = new Uri[] { httpUri, tcpUri, pipeUri };
			
			var serviceHost = new MonoscapeServiceHost(serviceType, uriArray);
			EnableDebugging (serviceHost);			
			return serviceHost;
		}
	}
}

