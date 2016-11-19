#region Includes
	using System;
	using System.Text;
#endregion

namespace VCM.EMS.Base
{

[Serializable]
public class RFID_Details
{

	#region Constructor
	public RFID_Details()
	{
		this._rFIDId = -1;
        this._rFIDNo = string.Empty;
		this._lastAction = string.Empty;
		this._createdBy = string.Empty;
		this._modifyBy = string.Empty;
		this._modifyDate = DateTime.Now;
		this._createdDate = DateTime.Now;
	}
	#endregion

	#region Private Variables 
	private System.Int64 _rFIDId;
	private System.String _rFIDNo;
    private System.Int32 _isTemp;
	private System.String _lastAction;
	private System.String _createdBy;
	private System.String _modifyBy;
	private System.DateTime _modifyDate;
	private System.DateTime _createdDate;
	#endregion

	#region Public Properties
	public System.Int64 RFIDId
	{
		get { return  _rFIDId; }
		set { _rFIDId = value; }
	}

	public System.String RFIDNo
	{
		get { return  _rFIDNo; }
		set { _rFIDNo = value; }
	}
    public System.Int32 ISTEMP
    {
        get { return _isTemp; }
        set { _isTemp = value; }
    }
	public System.String LastAction
	{
		get { return  _lastAction; }
		set { _lastAction = value; }
	}

	public System.String CreatedBy
	{
		get { return  _createdBy; }
		set { _createdBy = value; }
	}

	public System.String ModifyBy
	{
		get { return  _modifyBy; }
		set { _modifyBy = value; }
	}

	public System.DateTime ModifyDate
	{
		get { return  _modifyDate; }
		set { _modifyDate = value; }
	}

	public System.DateTime CreatedDate
	{
		get { return  _createdDate; }
		set { _createdDate = value; }
	}

	#endregion

}

}
