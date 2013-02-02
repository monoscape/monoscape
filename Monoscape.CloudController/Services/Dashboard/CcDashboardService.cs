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
using Monoscape.CloudController.Api.Services.Dashboard;
using Monoscape.CloudController.Api.Services.Dashboard.Model;
using Monoscape.CloudController.Runtime;
using Monoscape.Common.Model;
using Mono.Data.Sqlite;

namespace Monoscape.CloudController.Services.Dashboard
{
    internal class CcDashboardService : ICcDashboardService
    {
        #region Private Methods
        private static Subscription TransformSubscription(PersistenceStorage.Subscription subscription)
        {
            Subscription subscription_ = new Subscription();
            subscription_.Id = subscription.ID;
            subscription_.Type = subscription.Type;
            subscription_.AccessKey = subscription.AccessKey;
            subscription_.SecretKey = subscription.SecretKey;
            subscription_.State = subscription.State;
            subscription_.CreatedDate = subscription.CreatedDate;
            if ((subscription.SubscriptionItem != null) && (subscription.SubscriptionItem.Count > 0))            
                subscription_.Items = TransformSubscriptionItems(subscription.SubscriptionItem);           

            return subscription_;
        }

        private static List<SubscriptionItem> TransformSubscriptionItems(DbLinq.Data.Linq.EntitySet<PersistenceStorage.SubscriptionItem> entitySet)
        {
            List<SubscriptionItem> list = new List<SubscriptionItem>();
            foreach (var item in entitySet)
            {
                list.Add(TransformSubscriptionItem(item));
            }
            return list;
        }

        private static SubscriptionItem TransformSubscriptionItem(PersistenceStorage.SubscriptionItem item)
        {
            SubscriptionItem item_ = new SubscriptionItem();
            item_.Id = item.ID;
            item_.SubscriptionId = item.SubscriptionID;
            item_.ApplicationId = item.ApplicationID;            
            return item_;
        }
        #endregion

        public CcAddSubscriptionResponse AddSubscription(CcAddSubscriptionRequest request)
        {
            var connection = new SqliteConnection(Settings.SQLiteConnectionString);
            PersistenceStorage.PersistentDataContext context = new PersistenceStorage.PersistentDataContext(connection);

            PersistenceStorage.Subscription subscription = new PersistenceStorage.Subscription();
            subscription.Type = request.Subscription.Type;
            subscription.AccessKey = request.Subscription.AccessKey;
            subscription.SecretKey = request.Subscription.SecretKey;
            subscription.State = request.Subscription.State;
            subscription.CreatedDate = request.Subscription.CreatedDate;

            List<PersistenceStorage.SubscriptionItem> items = TransformSubscriptionItems(request.Subscription.Items);
            subscription.SubscriptionItem.AddRange(items);

            context.Subscription.InsertOnSubmit(subscription);
            context.SubmitChanges();

            CcAddSubscriptionResponse response = new CcAddSubscriptionResponse();
            return response;
        }

        private List<PersistenceStorage.SubscriptionItem> TransformSubscriptionItems(List<SubscriptionItem> list)
        {
            List<PersistenceStorage.SubscriptionItem> list_ = new List<PersistenceStorage.SubscriptionItem>();
            if (list != null)
            {
                foreach (SubscriptionItem item in list)
                {
                    list_.Add(TransformSubscriptionItem(item));
                }
            }
            return list_;
        }

        private PersistenceStorage.SubscriptionItem TransformSubscriptionItem(SubscriptionItem item)
        {
            PersistenceStorage.SubscriptionItem item_ = new PersistenceStorage.SubscriptionItem();
            item_.ID = item.Id;
            item_.SubscriptionID = item.SubscriptionId;
            item_.ApplicationID = item.ApplicationId;
            return item_;
        }

        public CcRemoveSubscriptionResponse RemoveSubscription(CcRemoveSubscriptionRequest request)
        {
            var connection = new SqliteConnection(Settings.SQLiteConnectionString);
            PersistenceStorage.PersistentDataContext context = new PersistenceStorage.PersistentDataContext(connection);

            PersistenceStorage.Subscription subscription = context.Subscription.Where(x => x.ID == request.SubscriptionId).FirstOrDefault();
            if (subscription != null)
            {
                context.Subscription.DeleteOnSubmit(subscription);
                context.SubmitChanges();
            }

            CcRemoveSubscriptionResponse response = new CcRemoveSubscriptionResponse();
            return response;
        }

        public CcUpdateSubscriptionResponse UpdateSubscription(CcUpdateSubscriptionRequest request)
        {
            var connection = new SqliteConnection(Settings.SQLiteConnectionString);
            PersistenceStorage.PersistentDataContext context = new PersistenceStorage.PersistentDataContext(connection);

            PersistenceStorage.Subscription subscription = context.Subscription.Where(x => x.ID == request.Subscription.Id).FirstOrDefault();
            if (subscription != null)
            {
                subscription.Type = request.Subscription.Type;
                subscription.AccessKey = request.Subscription.AccessKey;
                subscription.SecretKey = request.Subscription.SecretKey;
                subscription.State = request.Subscription.State;
                subscription.CreatedDate = request.Subscription.CreatedDate;
                                
                context.SubmitChanges();
            }

            CcUpdateSubscriptionResponse response = new CcUpdateSubscriptionResponse();
            return response;
        }

        public CcGetSubscriptionResponse GetSubscription(CcGetSubscriptionRequest request)
        {
            var connection = new SqliteConnection(Settings.SQLiteConnectionString);
            PersistenceStorage.PersistentDataContext context = new PersistenceStorage.PersistentDataContext(connection);

            CcGetSubscriptionResponse response = new CcGetSubscriptionResponse();
            PersistenceStorage.Subscription subscription = context.Subscription.Where(x => x.ID == request.SubscriptionId).FirstOrDefault();
            if (subscription != null)
            {
                Subscription subscription_ = TransformSubscription(subscription);
                response.Subscription = subscription_;
            }
            return response;
        }

        public CcQuerySubscriptionsResponse QuerySubscriptions(CcQuerySubscriptionsRequest request)
        {
            var connection = new SqliteConnection(Settings.SQLiteConnectionString);
            PersistenceStorage.PersistentDataContext context = new PersistenceStorage.PersistentDataContext(connection);
            
            CcQuerySubscriptionsResponse response = new CcQuerySubscriptionsResponse();
            List<Subscription> list = new List<Subscription>();
            
            foreach (PersistenceStorage.Subscription item in context.Subscription)
            {
                list.Add(TransformSubscription(item));
            }
            
            response.Subscriptions = list;
            return response;
        }
    }
}
