#region Includes
	using System;
	using System.Text;
#endregion

namespace VCM.EMS.Base
{

[Serializable]
public class Employers
{

	#region Constructor
	public Employers()
	{
		this._emplrId = -1;
		this._empId = -1;
		this._emplrName = string.Empty;
		this._title = string.Empty;
		this._location = string.Empty;
		this._from = string.Empty;
		this._to = string.Empty;
		this._description = string.Empty;
	}
	#endregion

	#region Private Variables 
	private System.Int64 _emplrId;
	private System.Int64 _empId;
	private System.String _emplrName;
	private System.String _title;
	private System.String _location;
	private System.String _from;
	private System.String _to;
	private System.String _description;
	#endregion

	#region Public Properties
	public System.Int64 EmplrId
	{
		get { return  _emplrId; }
		set { _emplrId = value; }
	}

	public System.Int64 EmpId
	{
		get { return  _empId; }
		set { _empId = value; }
	}

	public System.String EmplrName
	{
		get { return  _emplrName; }
		set { _emplrName = value; }
	}

	public System.String Title
	{
		get { return  _title; }
		set { _title = value; }
	}

	public System.String Location
	{
		get { return  _location; }
		set { _location = value; }
	}

	public System.String From
	{
		get { return  _from; }
		set { _from = value; }
	}

	public System.String To
	{
		get { return  _to; }
		set { _to = value; }
	}

	public System.String Description
	{
		get { return  _description; }
		set { _description = value; }
	}

	#endregion

}

}
