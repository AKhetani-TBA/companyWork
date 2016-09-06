#region Includes
	using System;
	using System.Text;
#endregion

namespace VCM.EMS.Base
{

[Serializable]
public class Emp_Investment
{

	#region Constructor
	public Emp_Investment()
	{
		this._empSectionId = -1;
        this._wef = DateTime.Now;
		this._empId = -1;
		this._sectionDetailId = -1;
		this._eligibleAmount = -1;
		this._lastAction = string.Empty;
		this._createdBy = string.Empty;
		this._modifyBy = string.Empty;
		this._modifyDate = DateTime.Now;
		this._createdDate = DateTime.Now;
	}
	#endregion

	#region Private Variables 
	private System.Int64 _empSectionId;
	private System.Int64 _empId;
    private System.Int64 _forTheYear;
    private System.DateTime _wef;
	private System.Int32 _sectionDetailId;
	private System.Int32 _eligibleAmount;
	private System.String _lastAction;
	private System.String _createdBy;
	private System.String _modifyBy;
	private System.DateTime _modifyDate;
	private System.DateTime _createdDate;
	#endregion

	#region Public Properties
	public System.Int64 EmpSectionId
	{
		get { return  _empSectionId; }
		set { _empSectionId = value; }
	}

	public System.Int64 EmpId
	{
		get { return  _empId; }
		set { _empId = value; }
	}
    public System.Int64 ForTheYear
    {
        get { return _forTheYear; }
        set { _forTheYear = value; }
    }


	public System.Int32 SectionDetailId
	{
		get { return  _sectionDetailId; }
		set { _sectionDetailId = value; }
	}

	public System.Int32 EligibleAmount
	{
		get { return  _eligibleAmount; }
		set { _eligibleAmount = value; }
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
    public System.DateTime WEF
    {
        get { return _wef; }
        set { _wef = value; }
    }

	public System.DateTime CreatedDate
	{
		get { return  _createdDate; }
		set { _createdDate = value; }
	}

	#endregion

}

}
