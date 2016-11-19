#region Includes
	using System;
	using System.Text;
#endregion

namespace VCM.EMS.Base
{

[Serializable]
public class Qualification
{

	#region Constructor
	public Qualification()
	{
		this._qualifId = -1;
		this._empId = -1;
		this._qualifName = string.Empty;
		this._qualifBoard = string.Empty;
		this._qualifYear = string.Empty;
		this._qualifPerc = string.Empty;
		this._lastAction = string.Empty;
		this._createdBy = string.Empty;
		this._createdDate = DateTime.Now;
		this._modifyBy = string.Empty;
		this._modifyDate = DateTime.Now;
	}
	#endregion

	#region Private Variables 
	private System.Int64 _qualifId;
	private System.Int64 _empId;
	private System.String _qualifName;
	private System.String _qualifBoard;
	private System.String _qualifYear;
	private System.String _qualifPerc;
	private System.String _lastAction;
	private System.String _createdBy;
	private System.DateTime _createdDate;
	private System.String _modifyBy;
	private System.DateTime _modifyDate;
	#endregion

	#region Public Properties
	public System.Int64 QualifId
	{
		get { return  _qualifId; }
		set { _qualifId = value; }
	}

	public System.Int64 EmpId
	{
		get { return  _empId; }
		set { _empId = value; }
	}

	public System.String QualifName
	{
		get { return  _qualifName; }
		set { _qualifName = value; }
	}

	public System.String QualifBoard
	{
		get { return  _qualifBoard; }
		set { _qualifBoard = value; }
	}

	public System.String QualifYear
	{
		get { return  _qualifYear; }
		set { _qualifYear = value; }
	}

	public System.String QualifPerc
	{
		get { return  _qualifPerc; }
		set { _qualifPerc = value; }
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

	public System.DateTime CreatedDate
	{
		get { return  _createdDate; }
		set { _createdDate = value; }
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

	#endregion

}

}
