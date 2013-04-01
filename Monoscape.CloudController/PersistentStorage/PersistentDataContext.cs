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

// 
//  ____  _     __  __      _        _ 
// |  _ \| |__ |  \/  | ___| |_ __ _| |
// | | | | '_ \| |\/| |/ _ \ __/ _` | |
// | |_| | |_) | |  | |  __/ || (_| | |
// |____/|_.__/|_|  |_|\___|\__\__,_|_|
//
// Auto-generated from main on 2011-12-22 12:11:43Z.
// Please visit http://code.google.com/p/dblinq2007/ for more information.
//
namespace Monoscape.CloudController.PersistenceStorage
{
	using System;
	using System.ComponentModel;
	using System.Data;
#if MONO_STRICT
	using System.Data.Linq;
#else   // MONO_STRICT
	using DbLinq.Data.Linq;
	using DbLinq.Vendor;
#endif  // MONO_STRICT
	using System.Data.Linq.Mapping;
	using System.Diagnostics;
	
	
	public partial class PersistentDataContext : DataContext
	{
		
		#region Extensibility Method Declarations
		partial void OnCreated();
		#endregion
		
		
		public PersistentDataContext(string connectionString) : 
				base(connectionString)
		{
			this.OnCreated();
		}
		
		public PersistentDataContext(string connection, MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			this.OnCreated();
		}

        public PersistentDataContext(IDbConnection connection, MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			this.OnCreated();
		}
		
		public Table<Subscription> Subscription
		{
			get
			{
				return this.GetTable<Subscription>();
			}
		}
		
		public Table<SubscriptionItem> SubscriptionItem
		{
			get
			{
				return this.GetTable<SubscriptionItem>();
			}
		}
	}
	
	#region Start MONO_STRICT
#if MONO_STRICT

	public partial class Main
	{
		
		public Main(IDbConnection connection) : 
				base(connection)
		{
			this.OnCreated();
		}
	}
	#region End MONO_STRICT
	#endregion
#else     // MONO_STRICT
	
	public partial class PersistentDataContext
	{
		
		public PersistentDataContext(IDbConnection connection) : 
				base(connection, new DbLinq.Sqlite.SqliteVendor())
		{
			this.OnCreated();
		}
		
		public PersistentDataContext(IDbConnection connection, IVendor sqlDialect) : 
				base(connection, sqlDialect)
		{
			this.OnCreated();
		}

        public PersistentDataContext(IDbConnection connection, MappingSource mappingSource, IVendor sqlDialect) : 
				base(connection, mappingSource, sqlDialect)
		{
			this.OnCreated();
		}
	}
	#region End Not MONO_STRICT
	#endregion
#endif     // MONO_STRICT
	#endregion
	
	[Table(Name="Subscription")]
	public partial class Subscription : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	{
		
		private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");
		
		private string _accessKey;
		
		private System.DateTime _createdDate;
		
		private int _id;
		
		private string _secretKey;
		
		private string _state;
		
		private string _type;
		
		private EntitySet<SubscriptionItem> _subscriptionItem;
		
		#region Extensibility Method Declarations
		partial void OnCreated();
		
		partial void OnAccessKeyChanged();
		
		partial void OnAccessKeyChanging(string value);
		
		partial void OnCreatedDateChanged();
		
		partial void OnCreatedDateChanging(System.DateTime value);
		
		partial void OnIDChanged();
		
		partial void OnIDChanging(int value);
		
		partial void OnSecretKeyChanged();
		
		partial void OnSecretKeyChanging(string value);
		
		partial void OnStateChanged();
		
		partial void OnStateChanging(string value);
		
		partial void OnTypeChanged();
		
		partial void OnTypeChanging(string value);
		#endregion
		
		
		public Subscription()
		{
			_subscriptionItem = new EntitySet<SubscriptionItem>(new Action<SubscriptionItem>(this.SubscriptionItem_Attach), new Action<SubscriptionItem>(this.SubscriptionItem_Detach));
			this.OnCreated();
		}
		
		[Column(Storage="_accessKey", Name="AccessKey", DbType="NVARCHAR(20)", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public string AccessKey
		{
			get
			{
				return this._accessKey;
			}
			set
			{
				if (((_accessKey == value) 
							== false))
				{
					this.OnAccessKeyChanging(value);
					this.SendPropertyChanging();
					this._accessKey = value;
					this.SendPropertyChanged("AccessKey");
					this.OnAccessKeyChanged();
				}
			}
		}
		
		[Column(Storage="_createdDate", Name="CreatedDate", DbType="DATE", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public System.DateTime CreatedDate
		{
			get
			{
				return this._createdDate;
			}
			set
			{
				if ((_createdDate != value))
				{
					this.OnCreatedDateChanging(value);
					this.SendPropertyChanging();
					this._createdDate = value;
					this.SendPropertyChanged("CreatedDate");
					this.OnCreatedDateChanged();
				}
			}
		}
		
		[Column(Storage="_id", Name="Id", DbType="INTEGER", IsPrimaryKey=true, IsDbGenerated=true, AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public int ID
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((_id != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._id = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[Column(Storage="_secretKey", Name="SecretKey", DbType="NVARCHAR(20)", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public string SecretKey
		{
			get
			{
				return this._secretKey;
			}
			set
			{
				if (((_secretKey == value) 
							== false))
				{
					this.OnSecretKeyChanging(value);
					this.SendPropertyChanging();
					this._secretKey = value;
					this.SendPropertyChanged("SecretKey");
					this.OnSecretKeyChanged();
				}
			}
		}
		
		[Column(Storage="_state", Name="State", DbType="NVARCHAR(100)", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public string State
		{
			get
			{
				return this._state;
			}
			set
			{
				if (((_state == value) 
							== false))
				{
					this.OnStateChanging(value);
					this.SendPropertyChanging();
					this._state = value;
					this.SendPropertyChanged("State");
					this.OnStateChanged();
				}
			}
		}
		
		[Column(Storage="_type", Name="Type", DbType="NVARCHAR(200)", AutoSync=AutoSync.Never)]
		[DebuggerNonUserCode()]
		public string Type
		{
			get
			{
				return this._type;
			}
			set
			{
				if (((_type == value) 
							== false))
				{
					this.OnTypeChanging(value);
					this.SendPropertyChanging();
					this._type = value;
					this.SendPropertyChanged("Type");
					this.OnTypeChanged();
				}
			}
		}
		
		#region Children
		[Association(Storage="_subscriptionItem", OtherKey="SubscriptionID", ThisKey="ID", Name="fk_SubscriptionItem_0")]
		[DebuggerNonUserCode()]
		public EntitySet<SubscriptionItem> SubscriptionItem
		{
			get
			{
				return this._subscriptionItem;
			}
			set
			{
				this._subscriptionItem = value;
			}
		}
		#endregion
		
		public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
		
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
			if ((h != null))
			{
				h(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(string propertyName)
		{
			System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
			if ((h != null))
			{
				h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
		
		#region Attachment handlers
		private void SubscriptionItem_Attach(SubscriptionItem entity)
		{
			this.SendPropertyChanging();
			entity.Subscription = this;
		}
		
		private void SubscriptionItem_Detach(SubscriptionItem entity)
		{
			this.SendPropertyChanging();
			entity.Subscription = null;
		}
		#endregion
	}
	
	[Table(Name="SubscriptionItem")]
	public partial class SubscriptionItem : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	{
		
		private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");
		
		private int _applicationID;
		
		private int _id;
		
		private int _subscriptionID;
		
		private EntityRef<Subscription> _subscription = new EntityRef<Subscription>();
		
		#region Extensibility Method Declarations
		partial void OnCreated();
		
		partial void OnApplicationIDChanged();
		
		partial void OnApplicationIDChanging(int value);
		
		partial void OnIDChanged();
		
		partial void OnIDChanging(int value);
		
		partial void OnSubscriptionIDChanged();
		
		partial void OnSubscriptionIDChanging(int value);
		#endregion
		
		
		public SubscriptionItem()
		{
			this.OnCreated();
		}
		
		[Column(Storage="_applicationID", Name="ApplicationId", DbType="INTEGER", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public int ApplicationID
		{
			get
			{
				return this._applicationID;
			}
			set
			{
				if ((_applicationID != value))
				{
					this.OnApplicationIDChanging(value);
					this.SendPropertyChanging();
					this._applicationID = value;
					this.SendPropertyChanged("ApplicationID");
					this.OnApplicationIDChanged();
				}
			}
		}
		
		[Column(Storage="_id", Name="Id", DbType="INTEGER", IsPrimaryKey=true, IsDbGenerated=true, AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public int ID
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((_id != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._id = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[Column(Storage="_subscriptionID", Name="SubscriptionId", DbType="INTEGER", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public int SubscriptionID
		{
			get
			{
				return this._subscriptionID;
			}
			set
			{
				if ((_subscriptionID != value))
				{
					this.OnSubscriptionIDChanging(value);
					this.SendPropertyChanging();
					this._subscriptionID = value;
					this.SendPropertyChanged("SubscriptionID");
					this.OnSubscriptionIDChanged();
				}
			}
		}
		
		#region Parents
		[Association(Storage="_subscription", OtherKey="ID", ThisKey="SubscriptionID", Name="fk_SubscriptionItem_0", IsForeignKey=true)]
		[DebuggerNonUserCode()]
		public Subscription Subscription
		{
			get
			{
				return this._subscription.Entity;
			}
			set
			{
				if (((this._subscription.Entity == value) 
							== false))
				{
					if ((this._subscription.Entity != null))
					{
						Subscription previousSubscription = this._subscription.Entity;
						this._subscription.Entity = null;
						previousSubscription.SubscriptionItem.Remove(this);
					}
					this._subscription.Entity = value;
					if ((value != null))
					{
						value.SubscriptionItem.Add(this);
						_subscriptionID = value.ID;
					}
					else
					{
						_subscriptionID = default(int);
					}
				}
			}
		}
		#endregion
		
		public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
		
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
			if ((h != null))
			{
				h(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(string propertyName)
		{
			System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
			if ((h != null))
			{
				h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
