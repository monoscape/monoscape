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

#define MONO

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monoscape.Common.Model;
using Monoscape.Common;
using PersistenceStorage=Monoscape.ApplicationGridController.PersistentStorage;
using System.IO;
using Monoscape.Common.Exceptions;
using System.Data;

using Mono.Data.Sqlite;

namespace Monoscape.ApplicationGridController.Runtime
{
    class Database
    {
		private static Object threadLock = new Object();
		private static Database instance;
		private List<Application> applicationsField;
        private List<Node> nodesField;
		
		private Database()
		{
		}
		
		public static Database GetInstance()
		{
			lock(threadLock)
			{
				if(instance == null)
					instance = new Database();
				return instance;
			}
		}
		
		public List<Application> Applications
		{
			get 
			{
				lock(threadLock)
				{
	                if (applicationsField == null)
	                    applicationsField = ReadApplications();
					return applicationsField;
				}
			}
			private set
			{
				lock(threadLock)
				{
					applicationsField = value;
				}
			}
		}
		
        public List<Node> Nodes 
        {
            get
            {
				lock(threadLock)
				{
	                if (nodesField == null)
	                    nodesField = new List<Node>();
	                return nodesField;
				}
            }
            private set
            {
				lock(threadLock)
				{
                	nodesField = value;
				}
            }
        }

        private List<ScalingHistoryItem> scalingHistoryField;
        public List<ScalingHistoryItem> ScalingHistory
        {
            get
            {
                lock (threadLock)
                {
                    if (scalingHistoryField == null)
                        scalingHistoryField = new List<ScalingHistoryItem>();
                    return scalingHistoryField;
                }
            }

            private set
            {
                scalingHistoryField = value;
            }
        }

        public void Commit()
        {
			lock(threadLock)
			{
	            Log.Info(typeof(Database), "Committing database...");
	            WriteApplications();
	            ResetRecordState();
	            Log.Info(typeof(Database), "Database committed");
			}
        }

        private void ResetRecordState()
        {
            // Applications
            foreach (Application app in Applications)            
                app.RowState = EntityState.Queried;            
        }

        private List<Application> ReadApplications()
        {
            Log.Info(typeof(Database), "Reading application data...");			
			
			try
			{
	            using(var connection = new SqliteConnection(Settings.SQLiteConnectionString))
				{
		            PersistenceStorage.PersistentDataContext persistenceDb = new PersistenceStorage.PersistentDataContext(connection);
					List<Application> applications = new List<Application>();
		            
		            foreach (PersistenceStorage.Application persistenceApp in persistenceDb.Application.ToList())
		            {
		                Monoscape.Common.Model.Application app = new Monoscape.Common.Model.Application();
		                app.Id = persistenceApp.ID;
		                app.Name = persistenceApp.Name;
		                app.Version = persistenceApp.Version;
		                app.State = ConvertApplicationState(persistenceApp.State.ToString());
		                app.FileName = persistenceApp.FilePath;
	                    app.Tenants = ConvertTenants(persistenceApp.Tenant);
		                app.RowState = EntityState.Queried;
		                applications.Add(app);
		            }
					Log.Info(typeof(Database), applications.Count + " applications found");
					return applications;
				}	            
			}
			catch(Exception e)
			{				
				Log.Error(typeof(Database), "Could not connect to the database", e);
				throw e;
			}            
        }

        private List<Tenant> ConvertTenants(DbLinq.Data.Linq.EntitySet<PersistenceStorage.Tenant> entitySet)
        {
            List<Tenant> tenants = new List<Tenant>();
            foreach (PersistenceStorage.Tenant t in entitySet)
            {
                Tenant tenant = new Tenant();
                tenant.Id = t.ID;
                tenant.Name = t.Name;
                if(t.UpperScaleLimit.HasValue)
                    tenant.UpperScaleLimit = (int)t.UpperScaleLimit;
                if (t.ScalingFactor.HasValue)
                    tenant.ScalingFactor = (int)t.ScalingFactor;
                tenants.Add(tenant);
            }
            return tenants;
        }

        private ApplicationState ConvertApplicationState(string value)
        {
            if (value.Equals(ApplicationState.Deploying.ToString()))
                return ApplicationState.Deploying;
            if (value.Equals(ApplicationState.Running.ToString()))
                return ApplicationState.Running;
            if (value.Equals(ApplicationState.Starting.ToString()))
                return ApplicationState.Starting;
            if (value.Equals(ApplicationState.Stopped.ToString()))
                return ApplicationState.Stopped;
            if (value.Equals(ApplicationState.Stopping.ToString()))
                return ApplicationState.Stopping;
            if (value.Equals(ApplicationState.Uploaded.ToString()))
                return ApplicationState.Uploaded;
            else
                return ApplicationState.Null;
        }

        private void WriteApplications()
        {
            Log.Info(typeof(Database), "Writing application data...");
			
			try
			{
	            var connection = new SqliteConnection(Settings.SQLiteConnectionString);
	            PersistenceStorage.PersistentDataContext persistenceDb = new PersistenceStorage.PersistentDataContext(connection);
	            
	            List<Application> removedApps = new List<Application>();
	            foreach (Application app in Applications)
	            {              
	                if (app.RowState == EntityState.New)
	                {
	                    PersistenceStorage.Application persistenceApp = new PersistenceStorage.Application();
	                    persistenceApp.Name = app.Name;
	                    persistenceApp.Version = app.Version;
	                    persistenceApp.State = app.State.ToString();
	                    persistenceApp.FilePath = app.FileName;
	
	                    persistenceDb.Application.InsertOnSubmit(persistenceApp);
	                    Log.Info(typeof(Database), "New application " + app.Name + " added");

                        WriteTenants(persistenceDb, app);
	                }
	                else if (app.RowState == EntityState.Modified)
	                {
	                    PersistenceStorage.Application persistenceApp = persistenceDb.Application.FirstOrDefault(a => a.ID == app.Id);
	                    persistenceApp.Name = app.Name;
	                    persistenceApp.Version = app.Version;
	                    persistenceApp.State = app.State.ToString();
	                    persistenceApp.FilePath = app.FileName;                   
	
	                    Log.Info(typeof(Database), "Application " + app.Name + " updated");

                        WriteTenants(persistenceDb, app);
	                }
	                else if (app.RowState == EntityState.Removed)
	                {
	                    string fileName = Path.GetFileName(app.FileName);
	                    string filePath = Path.Combine(Path.GetFullPath(Settings.ApplicationStoreFolder), fileName);
	
	                    PersistenceStorage.Application persistenceApp = persistenceDb.Application.FirstOrDefault(a => a.ID == app.Id);
	                    persistenceDb.Application.DeleteOnSubmit(persistenceApp);
	                    removedApps.Add(app);
	
	                    // Remove uploaded file
	                    if(File.Exists(filePath))
	                        File.Delete(filePath);

                        // Remove tenants
                        foreach (Tenant t in app.Tenants)
                            t.RowState = EntityState.Removed;
                        WriteTenants(persistenceDb, app);
	                }
	            }
	
	            foreach (Application removed in removedApps)
	            {
	                Database.GetInstance().Applications.Remove(removed);
	                Log.Info(typeof(Database), "Application " + removed.Name + " removed");
	            }
	
	            // Reload applications from the database on next call
	            applicationsField = null;
	
	            // Commit persistence storage
	            persistenceDb.SubmitChanges();
			}
			catch(Exception e)
			{				
				Log.Error(typeof(Database), "Could not write application data", e);
				throw e;
			}
        }

        private void WriteTenants(PersistenceStorage.PersistentDataContext persistenceDb, Application app)
        {
            if ((app.Tenants != null) && (app.Tenants.Count > 0))
            {
                List<Tenant> removedTenants = new List<Tenant>();

                foreach (Tenant tenant in app.Tenants)
                {
                    if (tenant.RowState == EntityState.New)
                    {
                        PersistenceStorage.Tenant pTenant = new PersistenceStorage.Tenant();
                        pTenant.Name = tenant.Name;
                        pTenant.ApplicationID = tenant.ApplicationId;
                        pTenant.UpperScaleLimit = tenant.UpperScaleLimit;
                        pTenant.ScalingFactor = tenant.ScalingFactor;

                        persistenceDb.Tenant.InsertOnSubmit(pTenant);
                        Log.Info(typeof(Database), "New application " + app.Name + " tenant " + tenant.Name + " added");
                    }
                    else if (tenant.RowState == EntityState.Modified)
                    {
                        PersistenceStorage.Tenant pTenant = persistenceDb.Tenant.FirstOrDefault(x => x.ID == tenant.Id);
                        pTenant.Name = tenant.Name;
                        pTenant.UpperScaleLimit = tenant.UpperScaleLimit;
                        pTenant.ScalingFactor = tenant.ScalingFactor;

                        Log.Info(typeof(Database), "Application " + app.Name + " tenant " + tenant.Name + " updated");
                    }
                    else if (tenant.RowState == EntityState.Removed)
                    {
                        PersistenceStorage.Tenant pTenant = persistenceDb.Tenant.FirstOrDefault(x => x.ID == tenant.Id);
                        persistenceDb.Tenant.DeleteOnSubmit(pTenant);

                        removedTenants.Add(tenant);
                    }
                }

                foreach (Tenant removed in removedTenants)
                {
                    app.Tenants.Remove(removed);
                    Log.Info(typeof(Database), "Application " + app.Name + " tenant " + removed.Name + " removed");
                }
            }
        }
    }
}
