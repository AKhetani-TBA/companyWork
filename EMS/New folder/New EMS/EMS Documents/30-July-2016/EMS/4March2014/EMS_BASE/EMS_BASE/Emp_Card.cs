#region Includes
	using System;
	using System.Text;
#endregion

namespace VCM.EMS.Base
{

[Serializable]
public class Emp_Card
{
	#region Constructor
	public Emp_Card()
	{
		this._serialId = -1;
		this._empId = -1;
		this._cardType = string.Empty;
        this._rFIDNo = string.Empty;
		this._status = string.Empty;
		this._reason = string.Empty;
		this._lastAction = string.Empty;
		this._createdBy = string.Empty;
		this._createdDate = DateTime.Now;
		this._modifyBy = string.Empty;
        this._modifyDate = null;
        this._issuedDate = null;
        this._revokedDate = null;
        this._fromTo = null;

        this._machinecode = -1;
        this._flag = -1;
        this._timeStamp = string.Empty;
	}
	#endregion

	#region Private Variables 
	private System.Int64 _serialId;
	private System.Int64 _empId;
	private System.String _cardType;
	private System.String _rFIDNo;
	private System.String _status;
	private System.String _reason;
	private System.String _lastAction;
	private System.String _createdBy;
	private System.DateTime _createdDate;
	private System.String _modifyBy;
    private System.String _modifyDate;
    private System.String _issuedDate;
    private System.String _revokedDate;
    private System.String _fromTo;

    private System.Int32 _shiftId = -1;
    private System.Int32 _deptId = -1;
    private System.Int32 _shiftDetail = -1;
    private System.DateTime _fromDate;
    private System.DateTime _toDate;

    private System.Int32 _flag;
    private System.Int32 _machinecode;
    private System.String _timeStamp;

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

	public System.String RFIDNo
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

    public System.String ModifyDate
	{
		get { return  _modifyDate; }
		set { _modifyDate = value; }
	}

    public System.String IssuedDate
	{
		get { return  _issuedDate; }
		set { _issuedDate = value; }
	}

    public System.String RevokedDate
	{
		get { return  _revokedDate; }
		set { _revokedDate = value; }
	}

    public String FromTo
	{
		get { return  _fromTo; }
		set { _fromTo = value; }
	}
    public System.Int32 ShiftId
    {
        get { return _shiftId; }
        set { _shiftId = value; }
    }
    public System.Int32 DeptId
    {
        get { return _deptId; }
        set { _deptId = value; }
    }
    public System.Int32 ShiftDetail
    {
        get { return _shiftDetail; }
        set { _shiftDetail = value; }
    }
    public System.DateTime FromDate
    {
        get { return _fromDate; }
        set { _fromDate = value; }
    }
    public System.DateTime ToDate
    {
        get { return _toDate; }
        set { _toDate = value; }
    }

    public System.String TimeStamp
    {
        get { return _timeStamp; }
        set { _timeStamp = value; }
    }
    public System.Int32 Machinecode
    {
        get { return _machinecode; }
        set { _machinecode = value; }
    }
    public System.Int32 Flag
    {
        get { return _flag; }
        set { _flag = value; }
    }

	#endregion
}
}
