#region Includes
	using System;
	using System.Collections.Generic;
	using System.Text;
using System.Data;
#endregion

namespace VCM.EMS.Biz
{

[Serializable]
public class SkillDetails
{
	#region Private Declarations
	private VCM.EMS.Dal.SkillDetails objLeave_Type;
	#endregion

	#region Constructor
    public SkillDetails()
	{
        objLeave_Type = new VCM.EMS.Dal.SkillDetails();
	}
	#endregion

	#region Public Methods

    public int Save_Skill_Type(VCM.EMS.Base.SkillDetails objLeave_TypeEntity)
	{
        return objLeave_Type.Save_Skill_Type(objLeave_TypeEntity);
	}

    public void Delete_Skill_Type(System.Int32 SkillId)
	{
         objLeave_Type.Delete_Skill_Type(SkillId);
	}
    public DataSet GetAllLSkillTypes(int SkillId, int EmpId)
    {
        return objLeave_Type.GetAllLSkillTypes(SkillId, EmpId);
    }

	#endregion

}

}