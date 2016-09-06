#region Includes
	using System;
	using System.Text;
#endregion

namespace VCM.EMS.Base
{

[Serializable]
public class Designations
{

	#region Constructor
	public Designations()
	{
        this._designationId = -1;
		this._designationName = string.Empty;
		this._lastAction = string.Empty;
		this._createdBy = string.Empty;
		this._createdDate = DateTime.Now;
		this._modifyBy = string.Empty;
		this._modifyDate = DateTime.Now;
	}
	#endregion

	#region Private Variables 
	private System.Int32 _designationId;
	private System.String _designationName;
	private System.String _lastAction;
	private System.String _createdBy;
	private System.DateTime _createdDate;
	private System.String _modifyBy;
	private System.DateTime _modifyDate;
	#endregion

	#region Public Properties
	public System.Int32 DesignationId
	{
		get { return  _designationId; }
		set { _designationId = value; }
	}

	public System.String DesignationName
	{
		get { return  _designationName; }
		set { _designationName = value; }
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
