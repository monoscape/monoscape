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
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.ServiceModel;
using Monoscape.Common.Model;
using System.ServiceModel.Channels;

namespace Monoscape.Common.WCFExtensions
{
    public class ExceptionMarshallingBehavior : IErrorHandler, IServiceBehavior
    {
        #region IErrorHandler Members
        public bool HandleError(Exception error)
        {
            return true;
        }

        public void ProvideFault(Exception ex, MessageVersion version, ref Message message)
        {            
            // Log exception raised
            Log.Error(this, ex);

            try
            {
                FaultException<MonoscapeFault> fe = new FaultException<MonoscapeFault>(new MonoscapeFault(ex.Message), new FaultReason(ex.Message));
                MessageFault fault = fe.CreateMessageFault();
                message = Message.CreateMessage(version, fault, "http://monoscape.common.wcfextensions/exceptionmarshallingbehavior");
                Log.Debug(this, "Exception " + ex.GetType() + " marshalled");
            }
            catch (Exception e)
            {
                Log.Error(this, "Could not marshall exception " + ex.GetType());
                Log.Error(this, e);
            }
        }
        #endregion

        // This behavior modifies no binding parameters.
        #region IServiceBehavior Members
        public void AddBindingParameters(ServiceDescription description, ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection parameters)
        {
            return;
        }

        // This behavior is an IErrorHandler implementation and 
        // must be applied to each ChannelDispatcher.
        public void ApplyDispatchBehavior(ServiceDescription description, ServiceHostBase serviceHostBase)
        {            
            foreach (ChannelDispatcher chanDisp in serviceHostBase.ChannelDispatchers)
            {
                chanDisp.ErrorHandlers.Add(this);
            }
            Log.Debug(typeof(ExceptionMarshallingBehavior), "ExceptionMarshallingBehavior was applied.");
        }

        // This behavior requires that the contract have a SOAP fault with a detail type of GreetingFault.
        public void Validate(ServiceDescription description, ServiceHostBase serviceHostBase)
        {
            Log.Debug(typeof(ExceptionMarshallingBehavior), "Validating service " + description.Name);
            foreach (ServiceEndpoint endpoint in description.Endpoints)
            {
                // Must not examine any metadata endpoint.
                if (endpoint.Contract.Name.Equals("IMetadataExchange") && endpoint.Contract.Namespace.Equals("http://schemas.microsoft.com/2006/04/mex"))
                    continue;

                foreach (OperationDescription opDesc in endpoint.Contract.Operations)
                {
                    if (opDesc.Faults.Count == 0)
                        throw new InvalidOperationException(String.Format(
                          "FaultBehavior requires a [FaultContract(typeof(MonoscapeFault))] in each operation contract. The \"{0}\" operation contains no FaultContractAttribute.",
                          opDesc.Name)
                        );
                    bool gfExists = false;
                    foreach (FaultDescription fault in opDesc.Faults)
                    {
                        if (fault.DetailType.Equals(typeof(MonoscapeFault)))
                        {
                            gfExists = true;
                            continue;
                        }
                    }
                    if (gfExists == false)
                    {
                        throw new InvalidOperationException("FaultBehavior requires a FaultContractAttribute(typeof(MonoscapeFault)) in an operation contract.");
                    }
                }
            }
        }
        #endregion
    }
}
