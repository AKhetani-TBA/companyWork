#region Includes
	using System;
	using System.Text;
#endregion

namespace VCM.EMS.Base
{

[Serializable]
public class Earnings
{

	#region Constructor
	public Earnings()
	{
		this._slabDetailId = -1;
		this._packageId = -1;
		this._earningId = -1;
        this._slabId = -1;
        this._applicableOn = string.Empty;
        this._isConditionAmount = string.Empty;
        this._startRange = string.Empty;
        this._endRange = string.Empty;
        this._contribution = string.Empty;
        this._isFixed = string.Empty;
        this._contributionFrom = string.Empty;
        this._forTheMonth = -1;
		this._lastAction = string.Empty;
		this._createdBy = string.Empty;
		this._modifyBy = string.Empty;
		this._modifyDate = DateTime.Now;
		this._createdDate = DateTime.Now;
	}
	#endregion

	#region Private Variables 
	private System.Int64 _slabDetailId;
	private System.Int64 _packageId;
	private System.Int64 _earningId;
    private System.Int64 _slabId;
    private System.String _applicableOn;
    private System.String _isConditionAmount;
    private System.String _startRange;
    private System.String _endRange;
    private System.String _contribution;
    private System.String _isFixed;
    private System.String _contributionFrom;
    private System.Int32 _forTheMonth;
	private System.String _lastAction;
	private System.String _createdBy;
	private System.String _modifyBy;
	private System.DateTime _modifyDate;
	private System.DateTime _createdDate;
	#endregion

	#region Public Properties
	public System.Int64 SlabDetailId
	{
		get { return  _slabDetailId; }
		set { _slabDetailId = value; }
	}

	public System.Int64 PackageId
	{
		get { return  _packageId; }
		set { _packageId = value; }
	}

	public System.Int64 EarningId
	{
		get { return  _earningId; }
		set { _earningId = value; }
	}
    public System.Int64 SlabId
    {
        get { return _slabId; }
        set { _slabId = value; }
    }
    public System.String ApplicableOn
    {
        get { return _applicableOn; }
        set { _applicableOn = value; }
    }
    public System.String IsConditionAmount
    {
        get { return _isConditionAmount; }
        set { _isConditionAmount = value; }
    }

    public System.String StartRange
    {
        get { return _startRange; }
        set { _startRange = value; }
    }

    public System.String EndRange
    {
        get { return _endRange; }
        set { _endRange = value; }
    }

    public System.String Contribution
    {
        get { return _contribution; }
        set { _contribution = value; }
    }

    public System.String IsFixed
    {
        get { return _isFixed; }
        set { _isFixed = value; }
    }

    public System.String ContributionFrom
    {
        get { return _contributionFrom; }
        set { _contributionFrom = value; }
    }

    public System.Int32 ForTheMonth
    {
        get { return _forTheMonth; }
        set { _forTheMonth = value; }
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
