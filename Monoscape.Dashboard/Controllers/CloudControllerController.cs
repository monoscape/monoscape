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
using System.Web;
using System.Web.Mvc;
using Monoscape.Common.Model;
using Monoscape.Dashboard.Runtime;
using Monoscape.CloudController.Api.Services.Dashboard.Model;
using Monoscape.ApplicationGridController.Api.Services.Dashboard.Model;

namespace Monoscape.Dashboard.Controllers
{
    public class CloudControllerController : AbstractController
    {
        //
        // GET: /CloudController/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Subscriptions()
        {
            try
            {
                CcQuerySubscriptionsRequest request = new CcQuerySubscriptionsRequest(Settings.Credentials);
                CcQuerySubscriptionsResponse response = EndPoints.CcDashboardService.QuerySubscriptions(request);
                return View(response.Subscriptions);
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }

        public ActionResult Subscription(int subscriptionId)
        {
            try
            {
                CcGetSubscriptionRequest request = new CcGetSubscriptionRequest(Settings.Credentials);
                request.SubscriptionId = subscriptionId;
                CcGetSubscriptionResponse response = EndPoints.CcDashboardService.GetSubscription(request);
                UpdateApplications(response.Subscription);
                return View(response.Subscription);
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }

        private void UpdateApplications(Subscription subscription)
        {
            if((subscription != null) && (subscription.Items != null))
            {
                foreach (SubscriptionItem item in subscription.Items)
                {
                    ApGetApplicationRequest request = new ApGetApplicationRequest(Settings.Credentials);
                    request.ApplicationId = item.ApplicationId;
                    ApGetApplicationResponse response = EndPoints.ApDashboardService.GetApplication(request);
                    item.Application = response.Application;
                }
            }
        }

        public ActionResult AddApplicationSubscription()
        {
            try
            {
                Subscription record = new Subscription();                
                ViewData["Applications"] = DescribeApplications();
                return View(record);
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }

        public ActionResult AddExSystemSubscription()
        {
            try
            {
                Subscription record = new Subscription();                
                return View(record);
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }

        [HttpPost]
        public ActionResult AddExSystemSubscription(Subscription record)
        {
            try
            {
                record.Type = "External System";
                record.CreatedDate = DateTime.Now;
                record.State = "Active";

                CcAddSubscriptionRequest request = new CcAddSubscriptionRequest(Settings.Credentials);
                request.Subscription = record;
                EndPoints.CcDashboardService.AddSubscription(request);
                return RedirectToAction("Subscriptions");
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }

        public ActionResult RemoveSubscription(int subscriptionId)
        {
            try
            {
                CcRemoveSubscriptionRequest request = new CcRemoveSubscriptionRequest(Settings.Credentials);
                request.SubscriptionId = subscriptionId;
                EndPoints.CcDashboardService.RemoveSubscription(request);
                return RedirectToAction("Subscriptions");
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }

        private List<Application> DescribeApplications()
        {            
            ApDescribeApplicationsRequest request = new ApDescribeApplicationsRequest(Settings.Credentials);
            ApDescribeApplicationsResponse response = EndPoints.ApDashboardService.DescribeApplications(request);
            return response.Applications;
        }

        [HttpPost]
        public ActionResult AddApplicationSubscription(Subscription record, FormCollection form)
        {
            try
            {
                record.Type = "Application";
                record.CreatedDate = DateTime.Now;
                record.State = "Active";

                var application_ = form.GetValue("application_");
                if ((application_ != null) && (application_.AttemptedValue != null))
                {
                    int applicationId = Int32.Parse(application_.AttemptedValue);
                    SubscriptionItem item = new SubscriptionItem();
                    item.ApplicationId = applicationId;
                    record.Items = new List<SubscriptionItem>();
                    record.Items.Add(item);
                }

                CcAddSubscriptionRequest request = new CcAddSubscriptionRequest(Settings.Credentials);
                request.Subscription = record;
                EndPoints.CcDashboardService.AddSubscription(request);
                return RedirectToAction("Subscriptions");
            }
            catch (Exception e)
            {
                return ShowError(e);
            }
        }
    }
}
