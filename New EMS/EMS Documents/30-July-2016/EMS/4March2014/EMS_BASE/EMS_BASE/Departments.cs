#region Includes
	using System;
	using System.Text;
#endregion

namespace VCM.EMS.Base
{

[Serializable]
public class Departments
{

	#region Constructor
	public Departments()
	{
		this._deptId = -1;
		this._deptName = string.Empty;
	}
	#endregion

	#region Private Variables 
	private System.Int32 _deptId;
	private System.String _deptName;
	#endregion

	#region Public Properties
	public System.Int32 DeptId
	{
		get { return  _deptId; }
		set { _deptId = value; }
	}

	public System.String DeptName
	{
		get { return  _deptName; }
		set { _deptName = value; }
	}

	#endregion

}

}
