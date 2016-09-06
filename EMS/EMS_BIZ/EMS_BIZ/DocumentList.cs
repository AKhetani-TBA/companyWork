#region Includes
	using System;
    using System.Data;
    using System.Data.SqlClient;
	using System.Collections.Generic;
	using System.Text;
#endregion

namespace VCM.EMS.Biz
{

[Serializable]
public class DocumentList
{
	#region Private Declarations
	private VCM.EMS.Dal.DocumentList objDocumentList;
	#endregion

	#region Constructor
	public DocumentList()
	{
		objDocumentList = new VCM.EMS.Dal.DocumentList();
	}
	#endregion

	#region Public Methods
	public int SaveDocumentList(VCM.EMS.Base.DocumentList objDocumentListEntity)
	{
		return objDocumentList.SaveDocumentList(objDocumentListEntity);
	}

	public int DeleteDocumentList(System.Int32 documentId)
	{
		return objDocumentList.DeleteDocumentList(documentId);
	} 

	public int ActivateInactivateDocumentList(string strIDs, int modifiedBy, bool isActive)
	{
		return objDocumentList.ActivateInactivateDocumentList(strIDs, modifiedBy, isActive);
	} 

	public VCM.EMS.Base.DocumentList GetDocumentListByID(System.Int32 documentId)
	{
		return objDocumentList.GetDocumentListByID(documentId);
	}

	public DataSet GetAll()
	{
		return objDocumentList.GetAll();
	}

	#endregion

}

}