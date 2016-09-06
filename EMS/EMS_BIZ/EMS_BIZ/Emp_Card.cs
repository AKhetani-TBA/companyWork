#region Includes
	using System;
using System.Data ;
	using System.Collections.Generic;
	using System.Text;
#endregion

namespace VCM.EMS.Biz
{

[Serializable]
public class Emp_Card
{
	#region Private Declarations
	private VCM.EMS.Dal.Emp_Card objCard;
	#endregion

	#region Constructor
	public Emp_Card()
	{
		objCard = new VCM.EMS.Dal.Emp_Card();
	}
	#endregion

	#region Public Methods
	public int SaveCard(VCM.EMS.Base.Emp_Card objCardEntity)
	{       
		return objCard.SaveCard(objCardEntity);
	}
    public int updateCard(VCM.EMS.Base.Emp_Card objCardEntity)
    {
        return objCard.updateCard(objCardEntity);
    }
	public int DeleteCard(System.Int64 serialId)
	{
		return objCard.DeleteCard(serialId);
	} 
	public int ActivateInactivateCard(string strIDs, int modifiedBy, bool isActive)
	{
		return objCard.ActivateInactivateCard(strIDs, modifiedBy, isActive);
	} 
	public VCM.EMS.Base.Emp_Card GetCardByID(System.Int64 serialId)
	{
		return objCard.GetCardByID(serialId);
	}
	public List<VCM.EMS.Base.Emp_Card> GetAll(Boolean isActive)
	{
		return objCard.GetAll(isActive);
	}
    public DataSet GetAllNotAssigned()
    {
        return objCard.GetAllNotAssigned();
    }
    public DataSet GetBySerialId(System.Int64 serId)
    {
        return objCard.GetBySerialId(serId);
    }
    public VCM.EMS.Base.Emp_Card GetAllCardDetail(System.Int64 serId)
    {
        return objCard.GetAllCardDetail(serId);
    }
    public DataSet GetAllAssigned()
    {
        return objCard.GetAllAssigned();
    }
    public DataSet GetAllFreeCards(System.Int64 isTemp)
    {
        return objCard.GetAllFreeCards(isTemp);
    }
    public VCM.EMS.Base.Emp_Card GetLastSerId(System.Int64 empId)
    {
        return objCard.GetLastSerId(empId);
    }
    public DataSet GetAllToBeReissued()
    {
        return objCard.GetAllToBeReissued();
    }
  
    public DataSet GetAllShiftEmployeeDetails(int DeptId, int EmpId)
    {
        return objCard.GetAllShiftEmployeeDetails(DeptId, EmpId);
    }
    public DataSet GetEmployeeShiftDetail(int shiftId)
    {
        return objCard.GetEmployeeShiftDetail(shiftId);
    }
    public void DeleteShiftDetails(int shiftId)
    {
        objCard.DeleteShiftDetails(shiftId);
    }
    public DataSet GetAllForgotLogDetails(DateTime StartDate, DateTime EndDate)
    {
        return objCard.GetAllForgotLogDetails(StartDate,EndDate);
    }
    public void SaveForgotLogDetails(VCM.EMS.Base.Emp_Card objCardEntity)
	{
        objCard.SaveForgotLogDetails(objCardEntity);
	}
	#endregion
}
}