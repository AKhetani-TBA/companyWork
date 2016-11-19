#region Includes
	using System;
	using System.Collections.Generic;
	using System.Text;
#endregion

namespace VCM.EMS.Biz
{

[Serializable]
public class Earnings
{
	#region Private Declarations
	private VCM.EMS.Dal.Earnings objEarnings;
	#endregion

	#region Constructor
	public Earnings()
	{
		objEarnings = new VCM.EMS.Dal.Earnings();
	}
	#endregion

	#region Public Methods
	public int SaveEarnings(VCM.EMS.Base.Earnings objEarningsEntity)
	{
		return objEarnings.SaveEarnings(objEarningsEntity);
	}

    public int DeleteEarnings(System.Int64 earningId)
	{
		return objEarnings.DeleteEarnings(earningId);
	} 

	
    public System.Data.DataSet GetAllDS(System.Int64 packageId, System.Int64 empId,string order,string fieldName)
    {
        return objEarnings.GetAllDS(packageId, empId,order,fieldName);
    }
    public VCM.EMS.Base.Earnings GetEarningsByID(System.Int32 packageId, System.Int32 empId, System.Int32 earningId)
	{
        return objEarnings.GetEarningsByID(packageId, empId, earningId);
	}

    public List<VCM.EMS.Base.Earnings> GetAll(System.Int32 packageId, System.Int32 empId)
	{
        return objEarnings.GetAll(packageId, empId);
	}

	#endregion

}

}