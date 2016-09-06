#region Includes
	using System;
	using System.Text;
#endregion

namespace VCM.EMS.Base
{

[Serializable]
public class DeductionEarning
{

	#region Constructor
	public DeductionEarning()
	{
		this._slabId = -1;
		this._slabName = string.Empty;
		this._lastAction = string.Empty;
		this._createdBy = string.Empty;
		this._createdDate = DateTime.Now;
		this._modifyBy = string.Empty;
		this._modifyDate = DateTime.Now;
        this._slabType = string.Empty;
        this._slabOrder = -1;
	}
	#endregion

	#region Private Variables 
	private System.Int64 _slabId;
	private System.String _slabName;
    private System.String _slabType;
	private System.String _lastAction;
	private System.String _createdBy;
	private System.DateTime _createdDate;
	private System.String _modifyBy;
	private System.DateTime _modifyDate;
    private System.Int32 _slabOrder;
	#endregion

	#region Public Properties
	public System.Int64 SlabId
	{
		get { return  _slabId; }
		set { _slabId = value; }
	}

	public System.String SlabName
	{
		get { return  _slabName; }
		set { _slabName = value; }
	}
    public System.String SlabType
    {
        get { return _slabType; }
        set { _slabType = value; }
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
    public System.Int32 SlabOrder
    {
        get { return _slabOrder; }
        set { _slabOrder = value; }
    }

	#endregion

}

}
