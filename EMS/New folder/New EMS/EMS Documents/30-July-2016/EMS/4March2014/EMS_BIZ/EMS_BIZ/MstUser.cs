#region Includes
	using System;
	using System.Collections.Generic;
	using System.Text;
    using System.Data.SqlClient;
    using System.Data;
#endregion

namespace VCM.EMS.Biz
{

[Serializable]
public class MstUser
{
	#region Private Declarations
	private VCM.EMS.Dal.MstUser objMstUser;
	#endregion

	#region Constructor
	public MstUser()
	{
		objMstUser = new VCM.EMS.Dal.MstUser();
	}
	#endregion

	#region Public Methods
    public int UpdateMstUser(VCM.EMS.Base.MstUser objMstUserEntity)
	{
        return objMstUser.UpdateMstUser(objMstUserEntity);
	}

    public int AddMstUser(VCM.EMS.Base.MstUser objMstUserEntity)
    {
        return objMstUser.AddMstUser(objMstUserEntity);
    }

	public int DeleteMstUser(System.String userId)
	{
		return objMstUser.DeleteMstUser(userId);
	} 

	public int ActivateInactivateMstUser(string strIDs, int modifiedBy, bool isActive)
	{
		return objMstUser.ActivateInactivateMstUser(strIDs, modifiedBy, isActive);
	}

    public VCM.EMS.Base.MstUser GetMstUserByID(System.Int64 empId)
	{
        return objMstUser.GetMstUserByID(empId);
	}

	public List<VCM.EMS.Base.MstUser> GetAll(Boolean isActive)
	{
		return objMstUser.GetAll(isActive);
	}

    public DataSet GetAllMasterUsers(System.Int64 empId, System.Int64 deptId, string fieldName, string order,int status)
    {
        return objMstUser.GetAllMasterUsers(empId, deptId, fieldName, order,status );
    }
    public string GetUserType(System.String userId)
    {
        return objMstUser.GetUserType(userId);
    }
    public string GetUserId(System.String userId)
    {
        return objMstUser.GetUserId(userId);
    }
	#endregion

}

}