#region Includes
	using System;
	using System.Text;
#endregion

namespace VCM.EMS.Base
{

[Serializable]
public class Bank_Details
{

	#region Constructor
	public Bank_Details()
	{
		this._empId = -1;
        this._bankId = -1;
		this._accountNo = string.Empty;
		this._bankName = string.Empty;
		this._bankBranch = string.Empty;
		this._isSalaryAccount = string.Empty;
	}
	#endregion

	#region Private Variables 
	private System.Int64 _empId;
	private System.String _accountNo;
	private System.String _bankName;
	private System.String _bankBranch;
    private System.Int64 _bankId;
	private System.String _isSalaryAccount;
	#endregion

	#region Public Properties
	public System.Int64 EmpId
	{
		get { return  _empId; }
		set { _empId = value; }
    }
    public System.Int64 BankId
    {
        get { return _bankId; }
        set { _bankId = value; }
    }

	public System.String AccountNo
	{
		get { return  _accountNo; }
		set { _accountNo = value; }
	}

	public System.String BankName
	{
		get { return  _bankName; }
		set { _bankName = value; }
	}

	public System.String BankBranch
	{
		get { return  _bankBranch; }
		set { _bankBranch = value; }
	}

	public System.String IsSalaryAccount
	{
		get { return  _isSalaryAccount; }
		set { _isSalaryAccount = value; }
	}

	#endregion

}

}
