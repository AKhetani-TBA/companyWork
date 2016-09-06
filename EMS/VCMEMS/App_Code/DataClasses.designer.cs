﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3634
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;



[System.Data.Linq.Mapping.DatabaseAttribute(Name="Demo_EMS")]
public partial class DataClassesDataContext : System.Data.Linq.DataContext
{
	
	private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
	
  #region Extensibility Method Definitions
  partial void OnCreated();
  partial void InsertEmp_ShiftDetail(Emp_ShiftDetail instance);
  partial void UpdateEmp_ShiftDetail(Emp_ShiftDetail instance);
  partial void DeleteEmp_ShiftDetail(Emp_ShiftDetail instance);
  #endregion
	
	public DataClassesDataContext() : 
			base(global::System.Configuration.ConfigurationManager.ConnectionStrings["Demo_EMSConnectionString"].ConnectionString, mappingSource)
	{
		OnCreated();
	}
	
	public DataClassesDataContext(string connection) : 
			base(connection, mappingSource)
	{
		OnCreated();
	}
	
	public DataClassesDataContext(System.Data.IDbConnection connection) : 
			base(connection, mappingSource)
	{
		OnCreated();
	}
	
	public DataClassesDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
			base(connection, mappingSource)
	{
		OnCreated();
	}
	
	public DataClassesDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
			base(connection, mappingSource)
	{
		OnCreated();
	}
	
	public System.Data.Linq.Table<Emp_ShiftDetail> Emp_ShiftDetails
	{
		get
		{
			return this.GetTable<Emp_ShiftDetail>();
		}
	}
}

[Table(Name="dbo.Emp_ShiftDetails")]
public partial class Emp_ShiftDetail : INotifyPropertyChanging, INotifyPropertyChanged
{
	
	private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
	
	private decimal _ShiftId;
	
	private System.Nullable<decimal> _EmployeeId;
	
	private System.Nullable<decimal> _DeptId;
	
	private System.Nullable<int> _ShiftDetail;
	
	private System.Nullable<System.DateTime> _FromDate;
	
	private System.Nullable<System.DateTime> _ToDate;
	
	private string _CreateBy;
	
	private System.Nullable<System.DateTime> _CreatedDate;
	
	private string _ModifyBy;
	
	private System.Nullable<System.DateTime> _ModifyDate;
	
	private string _LastAction;
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnShiftIdChanging(decimal value);
    partial void OnShiftIdChanged();
    partial void OnEmployeeIdChanging(System.Nullable<decimal> value);
    partial void OnEmployeeIdChanged();
    partial void OnDeptIdChanging(System.Nullable<decimal> value);
    partial void OnDeptIdChanged();
    partial void OnShiftDetailChanging(System.Nullable<int> value);
    partial void OnShiftDetailChanged();
    partial void OnFromDateChanging(System.Nullable<System.DateTime> value);
    partial void OnFromDateChanged();
    partial void OnToDateChanging(System.Nullable<System.DateTime> value);
    partial void OnToDateChanged();
    partial void OnCreateByChanging(string value);
    partial void OnCreateByChanged();
    partial void OnCreatedDateChanging(System.Nullable<System.DateTime> value);
    partial void OnCreatedDateChanged();
    partial void OnModifyByChanging(string value);
    partial void OnModifyByChanged();
    partial void OnModifyDateChanging(System.Nullable<System.DateTime> value);
    partial void OnModifyDateChanged();
    partial void OnLastActionChanging(string value);
    partial void OnLastActionChanged();
    #endregion
	
	public Emp_ShiftDetail()
	{
		OnCreated();
	}
	
	[Column(Storage="_ShiftId", AutoSync=AutoSync.OnInsert, DbType="Decimal(18,0) NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
	public decimal ShiftId
	{
		get
		{
			return this._ShiftId;
		}
		set
		{
			if ((this._ShiftId != value))
			{
				this.OnShiftIdChanging(value);
				this.SendPropertyChanging();
				this._ShiftId = value;
				this.SendPropertyChanged("ShiftId");
				this.OnShiftIdChanged();
			}
		}
	}
	
	[Column(Storage="_EmployeeId", DbType="Decimal(18,0)")]
	public System.Nullable<decimal> EmployeeId
	{
		get
		{
			return this._EmployeeId;
		}
		set
		{
			if ((this._EmployeeId != value))
			{
				this.OnEmployeeIdChanging(value);
				this.SendPropertyChanging();
				this._EmployeeId = value;
				this.SendPropertyChanged("EmployeeId");
				this.OnEmployeeIdChanged();
			}
		}
	}
	
	[Column(Storage="_DeptId", DbType="Decimal(18,0)")]
	public System.Nullable<decimal> DeptId
	{
		get
		{
			return this._DeptId;
		}
		set
		{
			if ((this._DeptId != value))
			{
				this.OnDeptIdChanging(value);
				this.SendPropertyChanging();
				this._DeptId = value;
				this.SendPropertyChanged("DeptId");
				this.OnDeptIdChanged();
			}
		}
	}
	
	[Column(Storage="_ShiftDetail", DbType="Int")]
	public System.Nullable<int> ShiftDetail
	{
		get
		{
			return this._ShiftDetail;
		}
		set
		{
			if ((this._ShiftDetail != value))
			{
				this.OnShiftDetailChanging(value);
				this.SendPropertyChanging();
				this._ShiftDetail = value;
				this.SendPropertyChanged("ShiftDetail");
				this.OnShiftDetailChanged();
			}
		}
	}
	
	[Column(Storage="_FromDate", DbType="DateTime")]
	public System.Nullable<System.DateTime> FromDate
	{
		get
		{
			return this._FromDate;
		}
		set
		{
			if ((this._FromDate != value))
			{
				this.OnFromDateChanging(value);
				this.SendPropertyChanging();
				this._FromDate = value;
				this.SendPropertyChanged("FromDate");
				this.OnFromDateChanged();
			}
		}
	}
	
	[Column(Storage="_ToDate", DbType="DateTime")]
	public System.Nullable<System.DateTime> ToDate
	{
		get
		{
			return this._ToDate;
		}
		set
		{
			if ((this._ToDate != value))
			{
				this.OnToDateChanging(value);
				this.SendPropertyChanging();
				this._ToDate = value;
				this.SendPropertyChanged("ToDate");
				this.OnToDateChanged();
			}
		}
	}
	
	[Column(Storage="_CreateBy", DbType="VarChar(100)")]
	public string CreateBy
	{
		get
		{
			return this._CreateBy;
		}
		set
		{
			if ((this._CreateBy != value))
			{
				this.OnCreateByChanging(value);
				this.SendPropertyChanging();
				this._CreateBy = value;
				this.SendPropertyChanged("CreateBy");
				this.OnCreateByChanged();
			}
		}
	}
	
	[Column(Storage="_CreatedDate", DbType="DateTime")]
	public System.Nullable<System.DateTime> CreatedDate
	{
		get
		{
			return this._CreatedDate;
		}
		set
		{
			if ((this._CreatedDate != value))
			{
				this.OnCreatedDateChanging(value);
				this.SendPropertyChanging();
				this._CreatedDate = value;
				this.SendPropertyChanged("CreatedDate");
				this.OnCreatedDateChanged();
			}
		}
	}
	
	[Column(Storage="_ModifyBy", DbType="VarChar(100)")]
	public string ModifyBy
	{
		get
		{
			return this._ModifyBy;
		}
		set
		{
			if ((this._ModifyBy != value))
			{
				this.OnModifyByChanging(value);
				this.SendPropertyChanging();
				this._ModifyBy = value;
				this.SendPropertyChanged("ModifyBy");
				this.OnModifyByChanged();
			}
		}
	}
	
	[Column(Storage="_ModifyDate", DbType="DateTime")]
	public System.Nullable<System.DateTime> ModifyDate
	{
		get
		{
			return this._ModifyDate;
		}
		set
		{
			if ((this._ModifyDate != value))
			{
				this.OnModifyDateChanging(value);
				this.SendPropertyChanging();
				this._ModifyDate = value;
				this.SendPropertyChanged("ModifyDate");
				this.OnModifyDateChanged();
			}
		}
	}
	
	[Column(Storage="_LastAction", DbType="VarChar(2)")]
	public string LastAction
	{
		get
		{
			return this._LastAction;
		}
		set
		{
			if ((this._LastAction != value))
			{
				this.OnLastActionChanging(value);
				this.SendPropertyChanging();
				this._LastAction = value;
				this.SendPropertyChanged("LastAction");
				this.OnLastActionChanged();
			}
		}
	}
	
	public event PropertyChangingEventHandler PropertyChanging;
	
	public event PropertyChangedEventHandler PropertyChanged;
	
	protected virtual void SendPropertyChanging()
	{
		if ((this.PropertyChanging != null))
		{
			this.PropertyChanging(this, emptyChangingEventArgs);
		}
	}
	
	protected virtual void SendPropertyChanged(String propertyName)
	{
		if ((this.PropertyChanged != null))
		{
			this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
#pragma warning restore 1591
