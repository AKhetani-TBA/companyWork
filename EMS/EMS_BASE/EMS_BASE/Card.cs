#region Includes
	using System;
	using System.Text;
#endregion

namespace VCM.EMS.Base
{

[Serializable]
public class Card
{

	#region Constructor
	public Card()
	{
		this._serialId = -1;
		this._empId = -1;
		this._cardType = string.Empty;
		this._rFIDNo = -1;
		this._status = string.Empty;
		this._reason = string.Empty;
		this._lastAction = string.Empty;
		this._createdBy = string.Empty;
		this._createdDate = DateTime.Now;
		this._modifyBy = string.Empty;
		this._modifyDate = DateTime.Now;
		this._issuedDate = DateTime.Now;
		this._revokedDate = DateTime.Now;
		this._fromTo = -1;
	}
	#endregion

	#region Private Variables 
	private System.Int64 _serialId;
	private System.Int64 _empId;
	private System.String _cardType;
	private System.Int32 _rFIDNo;
	private System.String _status;
	private System.String _reason;
	private System.String _lastAction;
	private System.String _createdBy;
	private System.DateTime _createdDate;
	private System.String _modifyBy;
	private System.DateTime _modifyDate;
	private System.DateTime _issuedDate;
	private System.DateTime _revokedDate;
	private System.Int32 _fromTo;
	#endregion

	#region Public Properties
	public System.Int64 SerialId
	{
		get { return  _serialId; }
		set { _serialId = value; }
	}

	public System.Int64 EmpId
	{
		get { return  _empId; }
		set { _empId = value; }
	}

	public System.String CardType
	{
		get { return  _cardType; }
		set { _cardType = value; }
	}

	public System.Int32 RFIDNo
	{
		get { return  _rFIDNo; }
		set { _rFIDNo = value; }
	}

	public System.String Status
	{
		get { return  _status; }
		set { _status = value; }
	}

	public System.String Reason
	{
		get { return  _reason; }
		set { _reason = value; }
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

	public System.DateTime CreatedDate
	{
		get { return  _createdDate; }
		set { _createdDate = value; }
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

	public System.DateTime IssuedDate
	{
		get { return  _issuedDate; }
		set { _issuedDate = value; }
	}

	public System.DateTime RevokedDate
	{
		get { return  _revokedDate; }
		set { _revokedDate = value; }
	}

	public System.Int32 FromTo
	{
		get { return  _fromTo; }
		set { _fromTo = value; }
	}

	#endregion

}

}
