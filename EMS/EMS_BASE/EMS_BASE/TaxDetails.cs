#region Includes
	using System;
	using System.Text;
#endregion

namespace VCM.EMS.Base
{

[Serializable]
public class TaxDetails
{

	#region Constructor
	public TaxDetails()
	{
		this._taxId = -1;
        this._taxId = -1;
		this._applyOn = string.Empty;
		this._startRange = -1;
		this._endRange = -1;
		this._taxPercentage = -1;
		this._wef = DateTime.Now;
		this._till = DateTime.Now;
	}
	#endregion

	#region Private Variables 
	private System.Int64 _taxId;
    private System.Int64 _taxMasterId;
	private System.String _applyOn;
	private System.Decimal _startRange;
	private System.Decimal _endRange;
	private System.Decimal _taxPercentage;
	private System.DateTime _wef;
	private System.DateTime _till;
    private System.String _lastAction;
    
	#endregion

	#region Public Properties
	public System.Int64 TaxId
	{
		get { return  _taxId; }
		set { _taxId = value; }
	}
    public System.String LastAction
    {
        get { return _lastAction; }
        set { _lastAction = value; }
    }

	public System.String ApplyOn
	{
		get { return  _applyOn; }
		set { _applyOn = value; }
	}
    
        public System.Int64 TaxMasterId
	{
        get { return _taxMasterId; }
        set { _taxMasterId = value; }
	}
	public System.Decimal StartRange
	{
		get { return  _startRange; }
		set { _startRange = value; }
	}

	public System.Decimal EndRange
	{
		get { return  _endRange; }
		set { _endRange = value; }
	}

	public System.Decimal TaxPercentage
	{
		get { return  _taxPercentage; }
		set { _taxPercentage = value; }
	}

	public System.DateTime Wef
	{
		get { return  _wef; }
		set { _wef = value; }
	}

	public System.DateTime Till
	{
		get { return  _till; }
		set { _till = value; }
	}

	#endregion

}

}
