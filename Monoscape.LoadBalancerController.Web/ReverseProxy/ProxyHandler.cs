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
using System.IO;
using System.Net;
using System.Web;
using Monoscape.Common;
using Monoscape.LoadBalancerController.Api;
using Monoscape.LoadBalancerController.Api.Services.LoadBalancerWeb.Model;
using Monoscape.LoadBalancerController.Web.Runtime;
using Monoscape.Common.Model;

namespace Monoscape.LoadBalancerController.Web.ReverseProxy
{
    /// <summary>
    /// Monoscape Reverse Proxy Handler
    /// </summary>
    public class ProxyHandler : IHttpAsyncHandler
    {
        #region Private Attributes
        private IAsyncResult addReqResult;
        #endregion

        #region Private Methods
        private string findTenantName(string pathAndQuery)
        {
            string[] split = pathAndQuery.Split('/');
            if (split.Length > 1)
                return split[1];
            else
                return null;
        }

        private string findApplicationName(string pathAndQuery)
        {
            string[] split = pathAndQuery.Split('/');
            if (split.Length > 2)
                return split[2];
            else
                return null;
        }

        public delegate int ASyncAddRequestToQueue(HttpContext context, int nodeId, int applicationId, int instanceId, int tenantId);

        private int AddRequestToQueue(HttpContext context, int nodeId, int applicationId, int instanceId, int tenantId)
        {
            try
            {
                LbAddRequestToQueueRequest request = new LbAddRequestToQueueRequest(Settings.Credentials);
                request.ApplicationRequest = LoadBalancerControllerUtil.ConvertRequest(context.Request, nodeId, applicationId, instanceId, tenantId);
                LbAddRequestToQueueResponse response = EndPoints.LoadBalancerWebService.AddRequestToQueue(request);
                return response.RequestId;
            }
            catch (Exception e)
            {
                Log.Error(this, "AddRequestToQueue() failed: ", e);
                return -1;
            }
        }

        public delegate void ASyncRemoveRequestFromQueue(int requestId);

        private void RemoveRequestFromQueue(int requestId)
        {
            try
            {
                LbRemoveRequestFromQueueRequest request = new LbRemoveRequestFromQueueRequest(Settings.Credentials);
                request.RequestId = requestId;
                EndPoints.LoadBalancerWebService.RemoveRequestFromQueue(request);
            }
            catch (Exception e)
            {
                Log.Error(this, "RemoveRequestFromQueue() failed: ", e);
            }
        }
        #endregion

        #region IHttpHandler Members
        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.</returns>
        public bool IsReusable
        {
            get { return false; }
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            Log.Debug(typeof(ProxyHandler), "ProcessRequest()");
            //ReverseProxyRequest(context, null);
        }
        #endregion

        #region IHttpAsyncHandler Members

        private AsyncProcessorDelegate delegate_;
        protected delegate void AsyncProcessorDelegate(HttpContext context, ReverseProxyContext rpContext);

        /// <summary>
        /// Initiates an asynchronous call to the HTTP handler.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"></see> object that provides references to intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        /// <param name="cb">The <see cref="T:System.AsyncCallback"></see> to call when the asynchronous method call is complete. If cb is null, the delegate is not called.</param>
        /// <param name="extraData">Any extra data needed to process the request.</param>
        /// <returns>
        /// An <see cref="T:System.IAsyncResult"></see> that contains information about the status of the process.
        /// </returns>
        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            Log.Debug(typeof(ProxyHandler), "BeginProcessRequest()");

            // Request URL Format: http://<load-balancer>:<port>/<tenant>/<application>
            Uri requestedUrl = context.Request.Url;
            Log.Debug(typeof(ProxyHandler), "Request: " + requestedUrl);

            ReverseProxyContext rpContext = new ReverseProxyContext();
            rpContext.RequestedHostName = requestedUrl.Host;
            rpContext.RequestedPort = requestedUrl.Port;
            rpContext.RequestedPathAndQuery = requestedUrl.PathAndQuery;
            rpContext.TenantName = findTenantName(rpContext.RequestedPathAndQuery);
            rpContext.ApplicationName = findApplicationName(rpContext.RequestedPathAndQuery);

            if ((!string.IsNullOrEmpty(rpContext.TenantName)) && (!string.IsNullOrEmpty(rpContext.ApplicationName)))
            {
                rpContext.ApplicationInstance = LoadBalancerControllerWebUtil.GetApplicationInstance(rpContext.TenantName, rpContext.ApplicationName);
                if (rpContext.ApplicationInstance != null)
                {
                    // Asynchronously add request to the load balancer request queue
                    rpContext.AddReqCaller = new ASyncAddRequestToQueue(AddRequestToQueue);
                    addReqResult = rpContext.AddReqCaller.BeginInvoke(context, rpContext.ApplicationInstance.NodeId,
                                                                               rpContext.ApplicationInstance.ApplicationId,
                                                                               rpContext.ApplicationInstance.Id, 
                                                                               rpContext.ApplicationInstance.Tenant.Id, null, null);                   
                }
            }

            delegate_ = new AsyncProcessorDelegate(ReverseProxyRequest);
            return delegate_.BeginInvoke(context, rpContext, cb, extraData);
        }

        /// <summary>
        /// Provides an asynchronous process End method when the process ends.
        /// </summary>
        /// <param name="result">An <see cref="T:System.IAsyncResult"></see> that contains information about the status of the process.</param>
        public void EndProcessRequest(IAsyncResult result)
        {
            Log.Debug(typeof(ProxyHandler), "EndProcessRequest()");
            delegate_.EndInvoke(result);
        }
        #endregion

        #region Reverse Proxy Logic
        private void ReverseProxyRequest(HttpContext context, ReverseProxyContext rpContext)
        {
            Log.Debug(typeof(ProxyHandler), "ReverseProxyRequest()");

            // Request URL Format: http://<load-balancer>:<port>/<tenant>/<application>
            Uri requestedUrl = context.Request.Url;
            Log.Debug(typeof(ProxyHandler), "Request: " + requestedUrl);

            if ((!string.IsNullOrEmpty(rpContext.TenantName)) && (!string.IsNullOrEmpty(rpContext.ApplicationName)))
            {
                var appInstance = rpContext.ApplicationInstance;
                if (appInstance == null)
                {
                    context.Response.Write("Application or the tenant not found");
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    context.Response.End();
                    return;
                }
                else
                {

                    Uri proxyRequestUrl = null;
                    Uri proxyResponseUrl = null;
                    WebResponse response = null;
                    bool incremented = false;

                    try
                    {
                        // Use same path and query for the reverse proxy
                        string pathAndQuery = rpContext.RequestedPathAndQuery;
                        // Set Proxy Host Name/Port
                        string proxyHostName = appInstance.IpAddress;
                        int proxyPort = appInstance.Port;

                        UriBuilder requestUrlBuilder = new UriBuilder(requestedUrl.Scheme, proxyHostName, proxyPort, pathAndQuery);
                        proxyRequestUrl = requestUrlBuilder.Uri;
                        Log.Debug(typeof(ProxyHandler), "Proxy Request: " + proxyRequestUrl);

                        UriBuilder responseUrlBuilder = new UriBuilder(requestedUrl.Scheme, rpContext.RequestedHostName, rpContext.RequestedPort, rpContext.RequestedPathAndQuery);
                        proxyResponseUrl = responseUrlBuilder.Uri;

                        // Send the request to the selected application instance
                        response = SendRequestToTarget(context, proxyRequestUrl, rpContext, out incremented);
                    }
                    catch (HttpException e)
                    {
                        Log.Error(this, "Http Error: " + e.GetHttpCode(), e);
                        context.Response.Write(e.Message);
                        context.Response.StatusCode = e.GetHttpCode();
                        context.Response.End();
                        return;
                    }
                    catch (Exception e)
                    {
                        Log.Error(this, e);
                        context.Response.Write("An internal error occurred");
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.End();
                        return;
                    }
                    finally
                    {                        
                        //if (incremented)
                        //{
                        //    // Decrement application instance request count
                        //    DecrementRequestCount(appInstance);
                        //}
                        if (response != null)
                        {
                            // Send the response to the client
                            SendResponseToClient(context, response, proxyRequestUrl, proxyResponseUrl);
                        }
                    }                    
                }
            }
            else
            {
                context.Response.Write(String.Format("Invalid request URL, expected: http://{0}:{1}/tenant/application", rpContext.RequestedHostName, rpContext.RequestedPort));
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.End();
                return;
            }
        }

        //private bool IncrementRequestCount(ApplicationInstance instance)
        //{
        //    LbIncrementRequestCountRequest request = new LbIncrementRequestCountRequest(Settings.Credentials);
        //    request.ApplicationId = instance.ApplicationId;
        //    request.InstanceId = instance.Id;
        //    EndPoints.LoadBalancerWebService.IncrementRequestCount(request);
        //    return true;
        //}

        //private void DecrementRequestCount(ApplicationInstance instance)
        //{
        //    LbDecrementRequestCountRequest request = new LbDecrementRequestCountRequest(Settings.Credentials);
        //    request.ApplicationId = instance.ApplicationId;
        //    request.InstanceId = instance.Id;
        //    EndPoints.LoadBalancerWebService.DecrementRequestCount(request);
        //}

        /// <summary>
        /// Sends the request to server
        /// Implemented using Managed Fusion URL Rewriter
        /// Reference: http://www.managedfusion.com/products/url-rewriter/
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        private WebResponse SendRequestToTarget(HttpContext context, Uri requestUrl, ReverseProxyContext rpContext, out bool incremented)
        {
            incremented = false;
            // get the request
            WebRequest request = WebRequest.CreateDefault(requestUrl);

            if (request == null)
                throw new HttpException((int)HttpStatusCode.BadRequest, "The requested url, <" + requestUrl + ">, could not be found.");

            // keep the same HTTP request method
            request.Method = context.Request.HttpMethod;
            var knownVerb = KnownHttpVerb.Parse(request.Method);

            // depending on the type of this request specific values for an HTTP request
            if (request is HttpWebRequest)
            {
                var httpRequest = request as HttpWebRequest;
                httpRequest.AllowAutoRedirect = false;
                httpRequest.ServicePoint.Expect100Continue = false;

                // add all the headers from the other proxied session to this request
                foreach (string name in context.Request.Headers.AllKeys)
                {
                    // add the headers that are restricted in their supported manor
                    switch (name)
                    {
                        case "User-Agent":
                            httpRequest.UserAgent = context.Request.UserAgent;
                            break;

                        case "Connection":
                            string connection = context.Request.Headers[name];
                            if (connection.IndexOf("Keep-Alive", StringComparison.OrdinalIgnoreCase) > 0)
                                httpRequest.KeepAlive = true;

                            List<string> list = new List<string>();
                            foreach (string conn in connection.Split(','))
                            {
                                string c = conn.Trim();
                                if (!c.Equals("Keep-Alive", StringComparison.OrdinalIgnoreCase) && !c.Equals("Close", StringComparison.OrdinalIgnoreCase))
                                    list.Add(c);
                            }

                            if (list.Count > 0)
                                httpRequest.Connection = String.Join(", ", list.ToArray());
                            break;

                        case "Transfer-Encoding":
                            httpRequest.SendChunked = true;
                            httpRequest.TransferEncoding = context.Request.Headers[name];
                            break;

                        case "Expect":
                            httpRequest.ServicePoint.Expect100Continue = true;
                            break;

                        case "If-Modified-Since":
                            DateTime ifModifiedSince;
                            if (DateTime.TryParse(context.Request.Headers[name], out ifModifiedSince))
                                httpRequest.IfModifiedSince = ifModifiedSince;
                            break;

                        case "Content-Length":
                            httpRequest.ContentLength = context.Request.ContentLength;
                            break;

                        case "Content-Type":
                            httpRequest.ContentType = context.Request.ContentType;
                            break;

                        case "Accept":
                            httpRequest.Accept = String.Join(", ", context.Request.AcceptTypes);
                            break;

                        case "Referer":
                            httpRequest.Referer = context.Request.UrlReferrer.OriginalString;
                            break;
                    }

                    // add to header if not restricted
                    if (!WebHeaderCollection.IsRestricted(name, false))
                    {
                        // it is nessisary to get the values for headers that are allowed to specifiy
                        // multiple values in an instance (i.e. Cookie)
                        string[] values = context.Request.Headers.GetValues(name);
                        foreach (string value in values)
                            request.Headers.Add(name, value);
                    }
                }
            }

            // Add Proxy Standard Protocol Headers
            // http://en.wikipedia.org/wiki/X-Forwarded-For
            request.Headers.Add("X-Forwarded-For", context.Request.UserHostAddress);
            // Add Server Variables
            // http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.45
            string currentServerName = context.Request.ServerVariables["SERVER_NAME"];
            string currentServerPort = context.Request.ServerVariables["SERVER_PORT"];
            string currentServerProtocol = context.Request.ServerVariables["SERVER_PROTOCOL"];

            if (currentServerProtocol.IndexOf("/") >= 0)
                currentServerProtocol = currentServerProtocol.Substring(currentServerProtocol.IndexOf("/") + 1);

            string currentVia = String.Format("{0} {1}:{2} ({3})", currentServerProtocol, currentServerName, currentServerPort, "Monoscape.LoadBalancerController");
            request.Headers.Add("Via", currentVia);

            // 
            // ContentLength is set to -1 if their is no data to send
            if ((request.ContentLength >= 0) && (!knownVerb.ContentBodyNotAllowed))
            {
                int bufferSize = 64 * 1024;
                using (Stream requestStream = request.GetRequestStream(), bufferStream = new BufferedStream(context.Request.InputStream, bufferSize))
                {
                    byte[] buffer = new byte[bufferSize];

                    try
                    {
                        while (true)
                        {
                            // make sure that the stream can be read from
                            if (!bufferStream.CanRead)
                                break;
                            int bytesReturned = bufferStream.Read(buffer, 0, bufferSize);
                            // if not bytes were returned the end of the stream has been reached
                            // and the loop should exit
                            if (bytesReturned == 0)
                                break;
                            // write bytes to the response
                            requestStream.Write(buffer, 0, bytesReturned);
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Error(typeof(ProxyHandler), "", e);
                    }
                }
            }
           
            WebResponse response;
            try
            {
                // Increment application instance request count
                //incremented = IncrementRequestCount(rpContext.ApplicationInstance);
                if (addReqResult != null)
                {
                    int requestId = rpContext.AddReqCaller.EndInvoke(addReqResult);
                    // Remove request from load balancer request queue
                    ASyncRemoveRequestFromQueue caller = new ASyncRemoveRequestFromQueue(RemoveRequestFromQueue);
                    caller.BeginInvoke(requestId, null, null);
                }
                // Send request to the proxy target
                response = request.GetResponse();
            }
            catch (WebException e)
            {
                Log.Error(typeof(ProxyHandler), "Error received from " + request.RequestUri + ": " + e.Message, e);
                response = e.Response;
            }

            if (response == null)
            {
                Log.Error(typeof(ProxyHandler), "The requested url " + requestUrl + " could not be found.");
                throw new HttpException((int)HttpStatusCode.NotFound, "The requested url could not be found.");
            }

            Log.Info(typeof(ProxyHandler), response.GetType().ToString());
            if (response is HttpWebResponse)
            {
                HttpWebResponse httpResponse = response as HttpWebResponse;
                Log.Info(typeof(ProxyHandler), "Received '" + ((int)httpResponse.StatusCode) + " " + httpResponse.StatusDescription + "'");
            }
            return response;
        }

        /// <summary>
        /// Sends the response to client
        /// Implemented using Managed Fusion URL Rewriter
        /// Reference: http://www.managedfusion.com/products/url-rewriter/
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="response">The response.</param>
        private void SendResponseToClient(HttpContext context, WebResponse response, Uri RequestUrl, Uri responseUrl)
        {
            context.Response.ClearHeaders();
            context.Response.ClearContent();

            // Add headers from the proxied response to the response
            for (int i = 0; i < response.Headers.Count; i++)
            {
                string name = response.Headers.GetKey(i);

                // Not required for the HttpContext				
                if ((name == "Server") || (name == "X-Powered-By") || (name == "Date") || (name == "Host"))
                    continue;

                string[] values = response.Headers.GetValues(i);
                if (values.Length == 0)
                    continue;

                if (name == "Location")
                {
                    try
                    {
                        string location = values[0];
                        if (!String.IsNullOrEmpty(location))
                        {
                            // If location is an absolute URL, the Uri instance is created using only location.
                            Uri requestLocationUrl = new Uri(RequestUrl, location);
                            UriBuilder responseLocationUrl = new UriBuilder(requestLocationUrl);

                            // if the requested location for the host and port is the same as the requested URL we need to update them to the response
                            if (Uri.Compare(requestLocationUrl, RequestUrl, UriComponents.SchemeAndServer, UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                responseLocationUrl.Port = responseUrl.Port;
                                responseLocationUrl.Host = responseUrl.Host;
                                responseLocationUrl.Scheme = responseUrl.Scheme;

                                string path = responseLocationUrl.Path;
                                int pathIndex = path.IndexOf(RequestUrl.AbsolutePath, StringComparison.OrdinalIgnoreCase);

                                // since this is not redirecting to a different server try to replace the path
                                if (pathIndex > -1)
                                {
                                    path = path.Remove(pathIndex, RequestUrl.AbsolutePath.Length);
                                    path = path.Insert(pathIndex, responseUrl.AbsolutePath);
                                    responseLocationUrl.Path = path;
                                }
                            }
                            context.Response.AppendHeader(name, responseLocationUrl.Uri.OriginalString);
                        }
                    }
                    catch { /* do nothing on purpose */ }

                    // Continue processing from next header
                    continue;
                }

                // if this is a chuncked response then we should send it correctly
                if (name == "Transfer-Encoding")
                {
                    /* http://www.go-mono.com/docs/index.aspx?link=P%3aSystem.Web.HttpResponse.Buffer
                     * 
                     * This controls whether HttpResponse should buffer the output before it is delivered to a 
                     * client. The default is true.
                     * 
                     * The buffering can be changed during the execution back and forth if needed. Notice that 
                     * changing the buffering state will not flush the current contents held in the output buffer, 
                     * the contents will only be flushed out on the next write operation or by manually calling 
                     * System.Web.HttpResponse.Flush
                     */
                    context.Response.BufferOutput = false;
                    continue;
                }

                if (name == "Content-Type")
                {
                    /* http://www.w3.org/Protocols/rfc1341/7_2_Multipart.html
                     * http://en.wikipedia.org/wiki/Motion_JPEG#M-JPEG_over_HTTP
                     * 
                     * The multipart/x-mixed-replace content-type should be treated as a streaming content and shouldn't be buffered.
                     */
                    if (values[0].StartsWith("multipart/x-mixed-replace"))
                        context.Response.BufferOutput = false;
                }

                // it is nessisary to get the values for headers that are allowed to specifiy
                // multiple values in an instance (i.e. Set-Cookie)
                foreach (string value in values)
                    context.Response.AppendHeader(name, value);
            }
            Log.Debug(typeof(ProxyHandler), "Response is " + (context.Response.BufferOutput ? "" : "not ") + "being buffered");

            // Set Http Response Code/Description
            if (response is HttpWebResponse)
            {
                HttpWebResponse httpResponse = response as HttpWebResponse;
                context.Response.StatusCode = (int)httpResponse.StatusCode;
                context.Response.StatusDescription = httpResponse.StatusDescription;
                Log.Debug(typeof(ProxyHandler), "Http Response Status: " + ((int)httpResponse.StatusCode) + " " + httpResponse.StatusDescription);
            }

            // Write response to the response stream
            int bufferSize = 64 * 1024;
            using (Stream responseStream = response.GetResponseStream())
            using (Stream bufferStream = new BufferedStream(responseStream, bufferSize))//Manager.Configuration.Rewriter.Proxy.BufferSize))
            {
                byte[] buffer = new byte[bufferSize];

                try
                {
                    while (true)
                    {
                        if (!bufferStream.CanRead)
                            break;
                        int bytesReturned = bufferStream.Read(buffer, 0, bufferSize);
                        if (bytesReturned == 0)
                            break;
                        context.Response.OutputStream.Write(buffer, 0, bytesReturned);
                    }
                }
                catch (Exception e)
                {
                    Log.Error(typeof(ProxyHandler), "Could not write response to the response stream", e);
                }
            }
        }
        #endregion
    }
}