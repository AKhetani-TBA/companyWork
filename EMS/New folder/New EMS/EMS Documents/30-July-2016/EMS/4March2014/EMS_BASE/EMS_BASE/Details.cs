#region Includes
	using System;
	using System.Text;
#endregion

namespace VCM.EMS.Base
{

[Serializable]
public class Details
{

	#region Constructor
	public Details()
	{
		this._empId = -1;
		this._empName = string.Empty;
		this._empGender = string.Empty;
		this._empDOB = string.Empty;
        this._dept_Id =  -1;
		this._empNationality = string.Empty;
		this._empPermanentAdd = string.Empty;
		this._empTemporaryAdd = string.Empty;
		this._empDomicile = string.Empty;
		this._empMaritalStatus = string.Empty;
		this._empMotherTongue = string.Empty;
		this._empBloodGroup = string.Empty;
		this._empContactNo = string.Empty;
		this._empHireDate = string.Empty;
		this._empWorkEmail = string.Empty;
		this._empPersonalEmail = string.Empty;
		this._empPassportNo = string.Empty;
		this._empPanNo = string.Empty;
		this._empPassportExpDate = string.Empty;
        this._empExperience = string.Empty;
        this._resignedDate = string.Empty;
        this._domicile = string.Empty;
        this._lastQual = string.Empty;
        this._shiftflage = -1;
	}
	#endregion

	#region Private Variables 
	private System.Int64 _empId;
	private System.String _empName;
	private System.String _empGender;
	private System.String _empDOB;
    private System.Int64  _dept_Id;
	private System.String _empNationality;
	private System.String _empPermanentAdd;
	private System.String _empTemporaryAdd;
	private System.String _empDomicile;
	private System.String _empMaritalStatus;
	private System.String _empMotherTongue;
	private System.String _empBloodGroup;
	private System.String _empContactNo;
	private System.String _empHireDate;
	private System.String _empWorkEmail;
	private System.String _empPersonalEmail;
	private System.String _empPassportNo;
	private System.String _empPanNo;
	private System.String _empPassportExpDate;
    private System.String _empExperience;
    private System.String _resignedDate;
    private System.String _domicile;
    private System.String _lastQual;
    private System.Int32 _shiftflage;
	#endregion

	#region Public Properties
	public System.Int64 EmpId
	{
		get { return  _empId; }
		set { _empId = value; }
	}

	public System.String EmpName
	{
		get { return  _empName; }
		set { _empName = value; }
	}

	public System.String EmpGender
	{
		get { return  _empGender; }
		set { _empGender = value; }
	}

	public System.String EmpDOB
	{
		get { return  _empDOB; }
		set { _empDOB = value; }
	}
    public System.Int64 DeptId
    {
        get { return _dept_Id; }
        set { _dept_Id = value; }
    }

	public System.String EmpNationality
	{
		get { return  _empNationality; }
		set { _empNationality = value; }
	}

	public System.String EmpPermanentAdd
	{
		get { return  _empPermanentAdd; }
		set { _empPermanentAdd = value; }
	}

	public System.String EmpTemporaryAdd
	{
		get { return  _empTemporaryAdd; }
		set { _empTemporaryAdd = value; }
	}

	public System.String EmpDomicile
	{
		get { return  _empDomicile; }
		set { _empDomicile = value; }
	}

	public System.String EmpMaritalStatus
	{
		get { return  _empMaritalStatus; }
		set { _empMaritalStatus = value; }
	}

	public System.String EmpMotherTongue
	{
		get { return  _empMotherTongue; }
		set { _empMotherTongue = value; }
	}

	public System.String EmpBloodGroup
	{
		get { return  _empBloodGroup; }
		set { _empBloodGroup = value; }
	}

	public System.String EmpContactNo
	{
		get { return  _empContactNo; }
		set { _empContactNo = value; }
	}

	public System.String EmpHireDate
	{
		get { return  _empHireDate; }
		set { _empHireDate = value; }
	}

	public System.String EmpWorkEmail
	{
		get { return  _empWorkEmail; }
		set { _empWorkEmail = value; }
	}

	public System.String EmpPersonalEmail
	{
		get { return  _empPersonalEmail; }
		set { _empPersonalEmail = value; }
	}

	public System.String EmpPassportNo
	{
		get { return  _empPassportNo; }
		set { _empPassportNo = value; }
	}

	public System.String EmpPanNo
	{
		get { return  _empPanNo; }
		set { _empPanNo = value; }
	}

	public System.String EmpPassportExpDate
	{
		get { return  _empPassportExpDate; }
		set { _empPassportExpDate = value; }
	}

    public System.String EmpExperience
    {
        get { return _empExperience; }
        set { _empExperience = value; }
    }
   
    public System.String ResignedDate
    {
        get { return _resignedDate; }
        set { _resignedDate = value; }
    }
    public System.String Domicile
    {
        get { return _domicile; }
        set { _domicile = value; }
    }
    public System.String LastQual
    {
        get { return _lastQual; }
        set { _lastQual = value; }
    }
    public System.Int32 Shiftflage
    {
        get { return _shiftflage; }
        set { _shiftflage = value; }
    }
	#endregion
}
}
