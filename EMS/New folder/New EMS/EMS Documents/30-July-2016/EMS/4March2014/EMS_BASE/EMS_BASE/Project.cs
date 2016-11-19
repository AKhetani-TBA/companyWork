#region Includes
	using System;
	using System.Text;
#endregion

namespace VCM.EMS.Base
{

[Serializable]
public class Project
{

	#region Constructor
	public Project()
	{
		this._projectId = -1;
		this._empId = -1;
		this._projectName = string.Empty;
		this._fromDate = DateTime.Now;
		this._toDate = DateTime.Now;
		this._description = string.Empty;
        this._roleName = string.Empty;
	}
	#endregion

	#region Private Variables 
	private System.Int64 _projectId;
	private System.Int64 _empId;
	private System.String _projectName;
	private System.DateTime _fromDate;
	private System.DateTime _toDate;
	private System.String _description;
    private System.String _roleName;
	#endregion

	#region Public Properties
	public System.Int64 ProjectId
	{
		get { return  _projectId; }
		set { _projectId = value; }
	}

	public System.Int64 EmpId
	{
		get { return  _empId; }
		set { _empId = value; }
	}

	public System.String ProjectName
	{
		get { return  _projectName; }
		set { _projectName = value; }
	}

	public System.DateTime FromDate
	{
		get { return  _fromDate; }
		set { _fromDate = value; }
	}

	public System.DateTime ToDate
	{
		get { return  _toDate; }
		set { _toDate = value; }
	}

	public System.String Description
	{
		get { return  _description; }
		set { _description = value; }
	}
    public System.String RoleName
    {
        get { return _roleName; }
        set { _roleName = value; }
    }

	#endregion

}

}
