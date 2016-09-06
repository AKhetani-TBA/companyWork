#region Includes
	using System;
	using System.Text;
#endregion

namespace VCM.EMS.Base
{

[Serializable]
public class Documents
{

	#region Constructor
	public Documents()
	{
		this._empId = -1;
		this._docId = -1;
		this._documentId = string.Empty;
		this._docURL = string.Empty;
        this._docDate = System.DateTime.Now;
	}
	#endregion

	#region Private Variables 
	private System.Int64 _empId;
	private System.Int64 _docId;
    private System.String _documentId;
	private System.String _docURL;
    private System.DateTime _docDate;
	#endregion

	#region Public Properties
	public System.Int64 EmpId
	{
		get { return  _empId; }
		set { _empId = value; }
	}

	public System.Int64 DocId
	{
		get { return  _docId; }
		set { _docId = value; }
	}

	public System.String DocumentId
	{
        get { return _documentId; }
        set { _documentId = value; }
	}

	public System.String DocURL
	{
		get { return  _docURL; }
		set { _docURL = value; }
	}
    public System.DateTime DocDate
    {
        get { return _docDate; }
        set { _docDate = value; }
    }

	#endregion

}

}
