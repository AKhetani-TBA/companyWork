#region Includes
	using System;
	using System.Data;
	using System.Text;
#endregion

namespace VCM.EMS.Biz
{

[Serializable]
public class Documents
{
	#region Private Declarations
	private VCM.EMS.Dal.Documents objDocuments;
	#endregion

	#region Constructor
	public Documents()
	{
		objDocuments = new VCM.EMS.Dal.Documents();
	}
	#endregion

	#region Public Methods
	public int SaveDocuments(VCM.EMS.Base.Documents objDocumentsEntity)
	{
		return objDocuments.SaveDocuments(objDocumentsEntity);
	}

	public int DeleteDocuments(System.Int64 docId)
	{
		return objDocuments.DeleteDocuments(docId);
	} 

	public int ActivateInactivateDocuments(string strIDs, int modifiedBy, bool isActive)
	{
		return objDocuments.ActivateInactivateDocuments(strIDs, modifiedBy, isActive);
	} 

	public VCM.EMS.Base.Documents GetDocumentsByID(System.Int64 lempId)
	{
		return objDocuments.GetDocumentsByID(lempId);
	}
    public DataSet GetAllDocuments(System.Int64 empId, System.Int64 deptId, System.Int32 docTitle, string fieldName, string order)
    {
        return objDocuments.GetAllDocuments(empId, deptId,docTitle, fieldName, order);
    }
    public DataSet GetPhotos(System.Int64 empId)
	{
        return objDocuments.GetPhotos(empId);
	}
	#endregion

}

}