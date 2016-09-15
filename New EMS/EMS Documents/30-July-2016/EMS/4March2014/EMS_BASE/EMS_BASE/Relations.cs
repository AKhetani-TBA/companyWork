#region Includes
	using System;
	using System.Text;
#endregion

namespace VCM.EMS.Base
{

[Serializable]
public class Relations
{

	#region Constructor
	public Relations()
	{
		this._empId = -1;
		this._relationId = -1;
		this._relationship = string.Empty;
		this._relativeName = string.Empty;
		this._relativeContactNo = string.Empty;
		this._relativeDOB = string.Empty;
	}
	#endregion

	#region Private Variables 
	private System.Int64 _empId;
	private System.Int32 _relationId;
	private System.String _relationship;
	private System.String _relativeName;
	private System.String _relativeContactNo;
	private System.String _relativeDOB;
	#endregion

	#region Public Properties
	public System.Int64 EmpId
	{
		get { return  _empId; }
		set { _empId = value; }
	}

	public System.Int32 RelationId
	{
		get { return  _relationId; }
		set { _relationId = value; }
	}

	public System.String Relationship
	{
		get { return  _relationship; }
		set { _relationship = value; }
	}

	public System.String RelativeName
	{
		get { return  _relativeName; }
		set { _relativeName = value; }
	}

	public System.String RelativeContactNo
	{
		get { return  _relativeContactNo; }
		set { _relativeContactNo = value; }
	}

	public System.String RelativeDOB
	{
		get { return  _relativeDOB; }
		set { _relativeDOB = value; }
	}

	#endregion

}

}
