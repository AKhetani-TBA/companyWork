#region Includes
	using System;
	using System.Text;
#endregion

namespace VCM.EMS.Base
{

[Serializable]
public class Investment_Sub_Sections
{

	#region Constructor
	public Investment_Sub_Sections()
	{
		this._sectionDetailId = -1;
		this._sectionId = -1;
		this._subSectionName = string.Empty;
		this._downLimit = -1;
		this._upLimit = -1;
		this._lastAction = string.Empty;
		this._createdBy = string.Empty;
		this._modifyBy = string.Empty;
		this._modifyDate = DateTime.Now;
		this._createdDate = DateTime.Now;
	}
	#endregion

	#region Private Variables 
	private System.Int32 _sectionDetailId;
	private System.Int32 _sectionId;
	private System.String _subSectionName;
	private System.Int32 _downLimit;
	private System.Int32 _upLimit;
	private System.String _lastAction;
	private System.String _createdBy;
	private System.String _modifyBy;
	private System.DateTime _modifyDate;
	private System.DateTime _createdDate;
	#endregion

	#region Public Properties
	public System.Int32 SectionDetailId
	{
		get { return  _sectionDetailId; }
		set { _sectionDetailId = value; }
	}

	public System.Int32 SectionId
	{
		get { return  _sectionId; }
		set { _sectionId = value; }
	}

	public System.String SubSectionName
	{
		get { return  _subSectionName; }
		set { _subSectionName = value; }
	}

	public System.Int32 DownLimit
	{
		get { return  _downLimit; }
		set { _downLimit = value; }
	}

	public System.Int32 UpLimit
	{
		get { return  _upLimit; }
		set { _upLimit = value; }
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
