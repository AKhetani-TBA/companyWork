#region Includes
	using System;
	using System.Text;
#endregion

namespace VCM.EMS.Base
{

[Serializable]
public class MstUser
{

	#region Constructor
    public MstUser()
	{
		this._userId = string.Empty;
		this._empId = -1;
		this._userType = -1;
	}
	#endregion

	#region Private Variables 
	private System.String _userId;
	private System.Int64 _empId;
	private System.Int32 _userType;
	#endregion

	#region Public Properties
	public System.String UserId
	{
		get { return  _userId; }
		set { _userId = value; }
	}

	public System.Int64 EmpId
	{
		get { return  _empId; }
		set { _empId = value; }
	}

	public System.Int32 UserType
	{
		get { return  _userType; }
		set { _userType = value; }
	}

	#endregion

}

}
