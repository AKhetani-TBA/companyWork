#region Includes
	using System;
	using System.Text;
#endregion

namespace VCM.EMS.Base
{

[Serializable]
public class Leave_Type
{

	#region Constructor
    public Leave_Type()
	{
		this._leaveTypeId = -1;
		this._leaveTypeName = string.Empty;
		this._lastAction = string.Empty;
		this._createdBy = string.Empty;
		this._modifyBy = string.Empty;
		this._modifyDate = DateTime.Now;
		this._cratedDate = DateTime.Now;
	}
	#endregion

	#region Private Variables 
	private System.Int64 _leaveTypeId;
	private System.String _leaveTypeName;
	private System.String _lastAction;
	private System.String _createdBy;
	private System.String _modifyBy;
	private System.DateTime _modifyDate;
	private System.DateTime _cratedDate;
	#endregion

	#region Public Properties
	public System.Int64 LeaveTypeId
	{
		get { return  _leaveTypeId; }
		set { _leaveTypeId = value; }
	}

	public System.String LeaveTypeName
	{
		get { return  _leaveTypeName; }
		set { _leaveTypeName = value; }
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
