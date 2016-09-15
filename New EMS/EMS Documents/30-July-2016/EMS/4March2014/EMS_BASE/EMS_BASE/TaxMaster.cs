#region Includes
	using System;
	using System.Text;
#endregion

namespace VCM.EMS.Base
{

[Serializable]
public class TaxMaster
{

	#region Constructor
	public TaxMaster()
	{
		this._taxMasterId = -1;
		this._taxMasterName = string.Empty;
		this._wef = DateTime.Now;
		this._lastAction = string.Empty;
		this._modifyBy = string.Empty;
		this._modifyDate = DateTime.Now;
		this._createdDate = DateTime.Now;
        _ageLimit = -1;
        _sexType = -1;
	}
	#endregion

	#region Private Variables 
	private System.Int32 _taxMasterId;

    private System.Int32 _ageLimit;
    private System.Int32 _sexType;
	private System.String _taxMasterName;
	private System.DateTime _wef;
	private System.String _lastAction;
	private System.String _modifyBy;
	private System.DateTime _modifyDate;
	private System.DateTime _createdDate;
	#endregion

	#region Public Properties
	public System.Int32 TaxMasterId
	{
		get { return  _taxMasterId; }
		set { _taxMasterId = value; }
	}
    public System.Int32 AgeLimit
    {
        get { return _ageLimit; }
        set { _ageLimit = value; }
    }
    public System.Int32 SexType
    {
        get { return _sexType; }
        set { _sexType = value; }
    }
	public System.String TaxMasterName
	{
		get { return  _taxMasterName; }
		set { _taxMasterName = value; }
	}

	public System.DateTime Wef
	{
		get { return  _wef; }
		set { _wef = value; }
	}

	public System.String LastAction
	{
		get { return  _lastAction; }
		set { _lastAction = value; }
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
