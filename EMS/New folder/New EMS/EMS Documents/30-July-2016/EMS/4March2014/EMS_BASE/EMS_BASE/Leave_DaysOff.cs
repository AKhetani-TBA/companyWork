#region Includes
	using System;
	using System.Text;
#endregion

namespace VCM.EMS.Base
{

[Serializable]
public class Leave_DaysOff
{

	#region Constructor
    public Leave_DaysOff()
	{
		this._holidayId = -1;
		this._holidayName = string.Empty;
		this._holidayDate = string.Empty;
		this._lastAction = string.Empty;
		this._createdBy = string.Empty;
		this._modifyBy = string.Empty;
		this._modifyDate = DateTime.Now;
		this._cratedDate = DateTime.Now;
        this._isOptional = 0;
       
	}
	#endregion

	#region Private Variables 
	private System.Int64 _holidayId;
	private System.String _holidayName;
	private System.String _holidayDate;
	private System.String _lastAction;
	private System.String _createdBy;
	private System.String _modifyBy;
    private System.Int64 _isOptional;
	private System.DateTime _modifyDate;
	private System.DateTime _cratedDate;
   
	#endregion

	#region Public Properties
	public System.Int64 HolidayId
	{
		get { return  _holidayId; }
		set { _holidayId = value; }
	}
    public System.Int64 IsOptional
	{
        get { return _isOptional; }
        set { _isOptional = value; }
	}

	public System.String HolidayName
	{
		get { return  _holidayName; }
		set { _holidayName = value; }
	}

	public System.String HolidayDate
	{
		get { return  _holidayDate; }
		set { _holidayDate = value; }
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

   
   
	#endregion

}

}
