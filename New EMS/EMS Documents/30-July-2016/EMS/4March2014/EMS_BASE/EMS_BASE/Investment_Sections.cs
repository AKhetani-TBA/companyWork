#region Includes
	using System;
	using System.Text;
#endregion

namespace VCM.EMS.Base
{

[Serializable]
public class Investment_Sections
{

	#region Constructor
	public Investment_Sections()
	{
		this._sectionId = -1;
		this._sectionName = string.Empty;
		this._sectionLimit = -1;
        this._sectionOrder = -1;
	}
	#endregion

	#region Private Variables 
	private System.Int32 _sectionId;
	private System.String _sectionName;
	private System.Int32 _sectionLimit;
    private System.Int32 _sectionOrder;
	#endregion

	#region Public Properties
	public System.Int32 SectionId
	{
		get { return  _sectionId; }
		set { _sectionId = value; }
	}

	public System.String SectionName
	{
		get { return  _sectionName; }
		set { _sectionName = value; }
	}

	public System.Int32 SectionLimit
	{
		get { return  _sectionLimit; }
		set { _sectionLimit = value; }
	}

    public System.Int32 SectionOrder
	{
        get { return _sectionOrder; }
        set { _sectionOrder = value; }
	}

	#endregion

}

}
