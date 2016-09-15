#region Includes
	using System;
	using System.Collections.Generic;
	using System.Text;
#endregion
using System.Data;
namespace VCM.EMS.Biz
{

[Serializable]
public class Leave_DaysOff
{
	#region Private Declarations
	private VCM.EMS.Dal.Leave_DaysOff objLeave_DaysOff;
	#endregion

	#region Constructor
    public Leave_DaysOff()
	{
        objLeave_DaysOff = new VCM.EMS.Dal.Leave_DaysOff();
	}
	#endregion

	#region Public Methods
	public int Save_DaysOff(VCM.EMS.Base.Leave_DaysOff objLeave_DaysOffEntity)
	{
		return objLeave_DaysOff.Savee_DaysOff (objLeave_DaysOffEntity);
	}

	public int Delete_DaysOff(System.Int64 holidayId)
	{
		return objLeave_DaysOff.Delete_DaysOff(holidayId);
	}
    public DataSet GetAllHolidays(System.Int64 HolidayId, string fieldName, string order, int Year)
    {
        return objLeave_DaysOff.GetAllHolidays(HolidayId, fieldName, order, Year);
    }
	public int ActivateInactivatee_DaysOff(string strIDs, int modifiedBy, bool isActive)
	{
		return objLeave_DaysOff.ActivateInactivatee_DaysOff(strIDs, modifiedBy, isActive);
	} 

	public VCM.EMS.Base.Leave_DaysOff Gete_DaysOffByID(System.Int64 holidayId)
	{
		return objLeave_DaysOff.Gete_DaysOffByID(holidayId);
	}

    public List<VCM.EMS.Base.Leave_DaysOff> GetAll(Boolean isActive)
	{
		return objLeave_DaysOff.GetAll(isActive);
	}

	#endregion

}

}