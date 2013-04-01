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
// Auto-generated from main on 2011-12-25 19:25:56Z.
// Please visit http://code.google.com/p/dblinq2007/ for more information.
//
namespace Monoscape.ApplicationGridController.PersistentStorage
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
		
		public Table<Application> Application
		{
			get
			{
				return this.GetTable<Application>();
			}
		}
		
		public Table<Tenant> Tenant
		{
			get
			{
				return this.GetTable<Tenant>();
			}
		}
	}
	
	#region Start MONO_STRICT
#if MONO_STRICT

	public partial class PersistentDataContext
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
	
	[Table(Name="Application")]
	public partial class Application : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	{
		
		private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");
		
		private string _filePath;
		
		private int _id;
		
		private string _name;
		
		private string _state;
		
		private string _version;
		
		private EntitySet<Tenant> _tenant;
		
		#region Extensibility Method Declarations
		partial void OnCreated();
		
		partial void OnFilePathChanged();
		
		partial void OnFilePathChanging(string value);
		
		partial void OnIDChanged();
		
		partial void OnIDChanging(int value);
		
		partial void OnNameChanged();
		
		partial void OnNameChanging(string value);
		
		partial void OnStateChanged();
		
		partial void OnStateChanging(string value);
		
		partial void OnVersionChanged();
		
		partial void OnVersionChanging(string value);
		#endregion
		
		
		public Application()
		{
			_tenant = new EntitySet<Tenant>(new Action<Tenant>(this.Tenant_Attach), new Action<Tenant>(this.Tenant_Detach));
			this.OnCreated();
		}
		
		[Column(Storage="_filePath", Name="FilePath", DbType="VARCHAR(2000)", AutoSync=AutoSync.Never)]
		[DebuggerNonUserCode()]
		public string FilePath
		{
			get
			{
				return this._filePath;
			}
			set
			{
				if (((_filePath == value) 
							== false))
				{
					this.OnFilePathChanging(value);
					this.SendPropertyChanging();
					this._filePath = value;
					this.SendPropertyChanged("FilePath");
					this.OnFilePathChanged();
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
		
		[Column(Storage="_name", Name="Name", DbType="NVARCHAR(1000)", AutoSync=AutoSync.Never)]
		[DebuggerNonUserCode()]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (((_name == value) 
							== false))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[Column(Storage="_state", Name="State", DbType="NVARCHAR(100)", AutoSync=AutoSync.Never)]
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
		
		[Column(Storage="_version", Name="Version", DbType="NVARCHAR(200)", AutoSync=AutoSync.Never)]
		[DebuggerNonUserCode()]
		public string Version
		{
			get
			{
				return this._version;
			}
			set
			{
				if (((_version == value) 
							== false))
				{
					this.OnVersionChanging(value);
					this.SendPropertyChanging();
					this._version = value;
					this.SendPropertyChanged("Version");
					this.OnVersionChanged();
				}
			}
		}
		
		#region Children
		[Association(Storage="_tenant", OtherKey="ApplicationID", ThisKey="ID", Name="fk_Tenant_0")]
		[DebuggerNonUserCode()]
		public EntitySet<Tenant> Tenant
		{
			get
			{
				return this._tenant;
			}
			set
			{
				this._tenant = value;
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
		private void Tenant_Attach(Tenant entity)
		{
			this.SendPropertyChanging();
			entity.Application = this;
		}
		
		private void Tenant_Detach(Tenant entity)
		{
			this.SendPropertyChanging();
			entity.Application = null;
		}
		#endregion
	}
	
	[Table(Name="Tenant")]
	public partial class Tenant : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	{
		
		private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");
		
		private int _applicationID;
		
		private int _id;
		
		private string _name;
		
		private System.Nullable<int> _scalingFactor;
		
		private System.Nullable<int> _upperScaleLimit;
		
		private EntityRef<Application> _application = new EntityRef<Application>();
		
		#region Extensibility Method Declarations
		partial void OnCreated();
		
		partial void OnApplicationIDChanged();
		
		partial void OnApplicationIDChanging(int value);
		
		partial void OnIDChanged();
		
		partial void OnIDChanging(int value);
		
		partial void OnNameChanged();
		
		partial void OnNameChanging(string value);
		
		partial void OnScalingFactorChanged();
		
		partial void OnScalingFactorChanging(System.Nullable<int> value);
		
		partial void OnUpperScaleLimitChanged();
		
		partial void OnUpperScaleLimitChanging(System.Nullable<int> value);
		#endregion
		
		
		public Tenant()
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
		
		[Column(Storage="_name", Name="Name", DbType="NVARCHAR(200)", AutoSync=AutoSync.Never)]
		[DebuggerNonUserCode()]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (((_name == value) 
							== false))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[Column(Storage="_scalingFactor", Name="ScalingFactor", DbType="INTEGER", AutoSync=AutoSync.Never)]
		[DebuggerNonUserCode()]
		public System.Nullable<int> ScalingFactor
		{
			get
			{
				return this._scalingFactor;
			}
			set
			{
				if ((_scalingFactor != value))
				{
					this.OnScalingFactorChanging(value);
					this.SendPropertyChanging();
					this._scalingFactor = value;
					this.SendPropertyChanged("ScalingFactor");
					this.OnScalingFactorChanged();
				}
			}
		}
		
		[Column(Storage="_upperScaleLimit", Name="UpperScaleLimit", DbType="INTEGER", AutoSync=AutoSync.Never)]
		[DebuggerNonUserCode()]
		public System.Nullable<int> UpperScaleLimit
		{
			get
			{
				return this._upperScaleLimit;
			}
			set
			{
				if ((_upperScaleLimit != value))
				{
					this.OnUpperScaleLimitChanging(value);
					this.SendPropertyChanging();
					this._upperScaleLimit = value;
					this.SendPropertyChanged("UpperScaleLimit");
					this.OnUpperScaleLimitChanged();
				}
			}
		}
		
		#region Parents
		[Association(Storage="_application", OtherKey="ID", ThisKey="ApplicationID", Name="fk_Tenant_0", IsForeignKey=true)]
		[DebuggerNonUserCode()]
		public Application Application
		{
			get
			{
				return this._application.Entity;
			}
			set
			{
				if (((this._application.Entity == value) 
							== false))
				{
					if ((this._application.Entity != null))
					{
						Application previousApplication = this._application.Entity;
						this._application.Entity = null;
						previousApplication.Tenant.Remove(this);
					}
					this._application.Entity = value;
					if ((value != null))
					{
						value.Tenant.Add(this);
						_applicationID = value.ID;
					}
					else
					{
						_applicationID = default(int);
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
