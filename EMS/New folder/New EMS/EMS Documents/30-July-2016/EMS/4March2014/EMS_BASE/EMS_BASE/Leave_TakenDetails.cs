#region Includes
	using System;
	using System.Text;
#endregion

namespace VCM.EMS.Base
{

[Serializable]
public class Leave_TakenDetails
{

	#region Constructor
    public Leave_TakenDetails()
	{
		this._leaveId = -1;
		this._empId = -1;
		this._leaveTypeId = -1;
		this._leaveReason = string.Empty;
		this._fromDate = string.Empty;
		this._toDate = string.Empty;
		this._lastAction = string.Empty;
		this._createdBy = string.Empty;
		this._modifyBy = string.Empty;
		this._modifyDate = DateTime.Now;
		this._cratedDate = DateTime.Now;
        this._leaveType = string.Empty;

        this._leaveAbb = string.Empty;
        this._leaveName = string.Empty;
        this._criteria = string.Empty;
        this._cF = string.Empty;
        this._days = 0;
        this._cF_Max_No = 0;
        this._LeaveID = -1;
        this._cL = 0;
        this._sL = 0;
        this._pL = 0;
        this._voL = 0;
        this._vPL = 0;
        this._total = 0;

        this._EmpID = 0;
        this._cId = 0;
        this._comments = string.Empty;
        this._approved = string.Empty;
        this._cOffDate = System.DateTime.Now;        
        this._DayType  = string.Empty;
	}
	#endregion

	#region Private Variables 
	private System.Int64 _leaveId;
	private System.Int64 _empId;
	private System.Int64 _leaveTypeId;
	private System.String _leaveReason;
	private System.String _fromDate;
	private System.String _toDate;
	private System.String _lastAction;
	private System.String _createdBy;
	private System.String _modifyBy;
	private System.DateTime _modifyDate;
	private System.DateTime _cratedDate;
    private System.String _leaveType;

    private System.String _leaveAbb;
    private System.String _leaveName;
    private System.String _criteria;
    private System.Decimal _days;
    private System.String _cF;
    private System.Decimal _cF_Max_No;
    private System.Int32 _LeaveID;
    private System.Decimal _cL;
    private System.Decimal _sL;
    private System.Decimal _pL;
    private System.Decimal _vPL;
    private System.Decimal _voL;
    private System.Decimal _total;

    private System.Int32 _cId;
    private System.Int32 _EmpID;
    private System.DateTime _cOffDate;
    private System.String _comments;
    private System.String _approved;
    private System.String _DayType;

	#endregion

	#region Public Properties
	public System.Int64 LeaveId
	{
		get { return  _leaveId; }
		set { _leaveId = value; }
	}

	public System.Int64 EmpId
	{
		get { return  _empId; }
		set { _empId = value; }
	}
    public System.String  LeaveType
	{
		get { return  _leaveType; }
		set { _leaveType = value; }
	}
	public System.Int64 LeaveTypeId
	{
		get { return  _leaveTypeId; }
		set { _leaveTypeId = value; }
	}

	public System.String LeaveReason
	{
		get { return  _leaveReason; }
		set { _leaveReason = value; }
	}

	public System.String FromDate
	{
		get { return  _fromDate; }
		set { _fromDate = value; }
	}

	public System.String ToDate
	{
		get { return  _toDate; }
		set { _toDate = value; }
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

	public System.DateTime CratedDate
	{
		get { return  _cratedDate; }
		set { _cratedDate = value; }
	}

    public System.String LeaveAbb
    {
        get { return _leaveAbb; }
        set { _leaveAbb = value; }
    }
    public System.String LeaveName
    {
        get { return _leaveName; }
        set { _leaveName = value; }
    }
    public System.String Criteria
    {
        get { return _criteria; }
        set { _criteria = value; }
    }
    public System.String CF
    {
        get { return _cF; }
        set { _cF = value; }
    }
    public System.Decimal Days
    {
        get { return _days; }
        set { _days = value; }
    }
    public System.Decimal CF_Max_No
    {
        get { return _cF_Max_No; }
        set { _cF_Max_No = value; }
    }
    public System.Int32 LeaveID
    {
        get { return _LeaveID; }
        set { _LeaveID = value; }
    }
    public System.Decimal CL
    {
        get { return _cL; }
        set { _cL = value; }
    }
    public System.Decimal SL
    {
        get { return _sL; }
        set { _sL = value; }
    }
    public System.Decimal PL
    {
        get { return _pL; }
        set { _pL = value; }
    }
    public System.Decimal VPL
    {
        get { return _vPL; }
        set { _vPL = value; }
    }
    public System.Decimal VOL
    {
        get { return _voL; }
        set { _voL = value; }
    }
    public System.Decimal Total
    {
        get { return _total; }
        set { _total = value; }
    }


    public System.Int32 CId
    {
        get { return _cId; }
        set { _cId = value; }
    }

    public System.Int32 EmpID
    {
        get{ return _EmpID;}
        set{ _EmpID= value;}
    }

    public System.String Comments
    {
        get { return _comments; }
        set { _comments = value; }
    }

    public System.String Approved
    {
        get { return _approved; }
        set { _approved = value; }
    }

    public System.DateTime CoffDate
    {
        get { return _cOffDate; }
        set { _cOffDate = value; }
    }
    public System.String DayType
    {
        get { return _DayType; }
        set { _DayType = value; }
    }
	#endregion

}

}
