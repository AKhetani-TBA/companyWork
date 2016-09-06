#region Includes
	using System;
	using System.Text;
#endregion

namespace VCM.EMS.Base
{

[Serializable]
public class Leave_TypeDetails
{

	#region Constructor
    public Leave_TypeDetails()
	{
		this._leaveTypeDetailsId = -1;
		this._leaveTypeId = -1;
		this._empId = -1;
		this._forTheYear = string.Empty;
		this._january = string.Empty;
		this._february = string.Empty;
		this._march = string.Empty;
		this._april = string.Empty;
		this._may = string.Empty;
		this._june = string.Empty;
		this._july = string.Empty;
		this._august = string.Empty;
		this._september = string.Empty;
		this._october = string.Empty;
		this._november = string.Empty;
		this._december = string.Empty;
		this._lastAction = string.Empty;
		this._createdBy = string.Empty;
		this._modifyBy = string.Empty;
		this._modifyDate = DateTime.Now;
		this._cratedDate = DateTime.Now;
	}
	#endregion

	#region Private Variables 
	private System.Int64 _leaveTypeDetailsId;
	private System.Int64 _leaveTypeId;
	private System.Int64 _empId;
	private System.String _forTheYear;
	private System.String _january;
	private System.String _february;
	private System.String _march;
	private System.String _april;
	private System.String _may;
	private System.String _june;
	private System.String _july;
	private System.String _august;
	private System.String _september;
	private System.String _october;
	private System.String _november;
	private System.String _december;
	private System.String _lastAction;
	private System.String _createdBy;
	private System.String _modifyBy;
	private System.DateTime _modifyDate;
	private System.DateTime _cratedDate;
	#endregion

	#region Public Properties
	public System.Int64 LeaveTypeDetailsId
	{
		get { return  _leaveTypeDetailsId; }
		set { _leaveTypeDetailsId = value; }
	}

	public System.Int64 LeaveTypeId
	{
		get { return  _leaveTypeId; }
		set { _leaveTypeId = value; }
	}

	public System.Int64 EmpId
	{
		get { return  _empId; }
		set { _empId = value; }
	}

	public System.String ForTheYear
	{
		get { return  _forTheYear; }
		set { _forTheYear = value; }
	}

	public System.String January
	{
		get { return  _january; }
		set { _january = value; }
	}

	public System.String February
	{
		get { return  _february; }
		set { _february = value; }
	}

	public System.String March
	{
		get { return  _march; }
		set { _march = value; }
	}

	public System.String April
	{
		get { return  _april; }
		set { _april = value; }
	}

	public System.String May
	{
		get { return  _may; }
		set { _may = value; }
	}

	public System.String June
	{
		get { return  _june; }
		set { _june = value; }
	}

	public System.String July
	{
		get { return  _july; }
		set { _july = value; }
	}

	public System.String August
	{
		get { return  _august; }
		set { _august = value; }
	}

	public System.String September
	{
		get { return  _september; }
		set { _september = value; }
	}

	public System.String October
	{
		get { return  _october; }
		set { _october = value; }
	}

	public System.String November
	{
		get { return  _november; }
		set { _november = value; }
	}

	public System.String December
	{
		get { return  _december; }
		set { _december = value; }
	}

	public System.String LastAction
	{
		get { return  _lastAction; }
		set { _lastAction = value; }
	}

	public System.String CreatedBy
	{
		get { return  _createdBy; }
		set { _createdBy = value; }
	}

	public System.String ModifyBy
	{
		get { return  _modifyBy; }
		set { _modifyBy = value; }
	}

	public System.DateTime ModifyDate
	{
		get { return  _modifyDate; }
		set { _modifyDate = value; }
	}

	public System.DateTime CratedDate
	{
		get { return  _cratedDate; }
		set { _cratedDate = value; }
	}

	#endregion

}

}
