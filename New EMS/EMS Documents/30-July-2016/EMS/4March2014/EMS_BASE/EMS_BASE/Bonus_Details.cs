#region Includes
	using System;
	using System.Text;
#endregion

namespace VCM.EMS.Base
{

[Serializable]
public class Bonus_Details
{

	#region Constructor
    public Bonus_Details()
	{
		this._packageId = -1;
        this._empId = -1;
		this._bonusId = -1;
		this._bonusAmount = -1;
		this._wef = DateTime.Now;
		this._empBonusId = -1;
        this._criteria = string.Empty;
		this._payableOn = DateTime.Now;
        this._paidBonusAmount = -1;
		this._lastAction = string.Empty;
		this._createdBy = string.Empty;
		this._modifyBy = string.Empty;
		this._modifyDate = DateTime.Now;
		this._createdDate = DateTime.Now;
	}
	#endregion

	#region Private Variables 
	private System.Int64 _packageId;
    private System.Int64 _empId;
	private System.Int64 _bonusId;
	private System.Decimal _bonusAmount;
	private System.DateTime _wef;
	private System.Int64 _empBonusId;
    private System.String _criteria;
	private System.DateTime _payableOn;
    private System.Decimal _paidBonusAmount;
	private System.String _lastAction;
	private System.String _createdBy;
	private System.String _modifyBy;
	private System.DateTime _modifyDate;
	private System.DateTime _createdDate;
	#endregion

	#region Public Properties

    public System.Int64 EmpId
    {
        get { return _empId; }
        set { _empId = value; }
    }

	public System.Int64 PackageId
	{
		get { return  _packageId; }
		set { _packageId = value; }
	}

	public System.Int64 BonusId
	{
		get { return  _bonusId; }
		set { _bonusId = value; }
	}

	public System.Decimal BonusAmount
	{
		get { return  _bonusAmount; }
		set { _bonusAmount = value; }
	}

	public System.DateTime Wef
	{
		get { return  _wef; }
		set { _wef = value; }
	}

	public System.Int64 EmpBonusId
	{
		get { return  _empBonusId; }
		set { _empBonusId = value; }
	}
    public System.String Criteria
    {
        get { return _criteria; }
        set { _criteria = value; }
    }

	public System.DateTime PayableOn
	{
		get { return  _payableOn; }
		set { _payableOn = value; }
	}
    public System.Decimal PaidBonusAmount
    {
        get { return _paidBonusAmount; }
        set { _paidBonusAmount = value; }
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
