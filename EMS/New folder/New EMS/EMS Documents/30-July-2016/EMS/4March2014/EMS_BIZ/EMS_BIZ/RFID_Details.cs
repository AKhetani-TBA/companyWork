#region Includes
	using System;
	using System.Collections.Generic;
	using System.Text;
using System.Data;
#endregion

namespace VCM.EMS.Biz
{

[Serializable]
public class RFID_Details
{
	#region Private Declarations
	private VCM.EMS.Dal.RFID_Details obj_Details;
	#endregion

	#region Constructor
	public RFID_Details()
	{
		obj_Details = new VCM.EMS.Dal.RFID_Details();
	}
	#endregion

	#region Public Methods
	public int Save_Details(VCM.EMS.Base.RFID_Details obj_DetailsEntity)
	{
		return obj_Details.Save_Details(obj_DetailsEntity);
	}

    public int RfidDelete_Details(System.Int64 rFIDId, System.String rFIDNo)
    {
        return obj_Details.RfidDelete_Details(rFIDId, rFIDNo);
	}
    public int checkUsage(System.Int64 rFIDId, System.String rFIDNo)
    {
        return obj_Details.checkUsage(rFIDId,rFIDNo);
    }
    public VCM.EMS.Base.RFID_Details GetCard_DetailsByID(System.Int64 RFIDId)
    {
        return obj_Details.GetCard_DetailsByID(RFIDId);
    }
	public int ActivateInactivate_Details(string strIDs, int modifiedBy, bool isActive)
	{
		return obj_Details.ActivateInactivate_Details(strIDs, modifiedBy, isActive);
	} 
    public DataSet GetCardByDept(System.Int64 deptId, System.String type, System.String status,string fieldName,string order)
    {
        return obj_Details.GetCardByDept(deptId, type, status, fieldName, order);
    }
    public DataSet GetAllCardDetails(System.String type, System.String status,string fieldName,string order)
    {
        return obj_Details.GetAllCardDetails(type, status, fieldName, order);
    }
    public DataSet GetMasterCardDetail(string fieldName, string order)
    {
        return obj_Details.GetMasterCardDetail(fieldName, order);
    }
         public DataSet GetAllEmpCardDetailsByEmp(System.Int64 deptId, System.Int64 empId, System.String type, System.String status,string fieldName,string order)
    {
        return obj_Details.GetAllEmpCardDetailsByEmp(deptId, empId, type, status,fieldName,order);
    }
     public DataSet GetUsedFreeAll(int cardstatus, int cardype)
     {
         return obj_Details.GetUsedFreeAll(cardstatus, cardype);
     }

    
	#endregion
}
}