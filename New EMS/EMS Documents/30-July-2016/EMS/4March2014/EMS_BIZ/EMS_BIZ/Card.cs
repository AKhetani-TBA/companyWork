#region Includes
	using System;
	using System.Collections.Generic;
	using System.Text;
#endregion

namespace VCM.EMS.Biz
{

[Serializable]
public class Card
{
	#region Private Declarations
	private VCM.EMS.Dal.Card objCard;
	#endregion

	#region Constructor
	public Card()
	{
		objCard = new VCM.EMS.Dal.Card();
	}
	#endregion

	#region Public Methods
	public int SaveCard(VCM.EMS.Base.Card objCardEntity)
	{
		return objCard.SaveCard(objCardEntity);
	}

	public int DeleteCard(System.Int64 serialId)
	{
		return objCard.DeleteCard(serialId);
	} 

	public int ActivateInactivateCard(string strIDs, int modifiedBy, bool isActive)
	{
		return objCard.ActivateInactivateCard(strIDs, modifiedBy, isActive);
	} 

	public VCM.EMS.Base.Card GetCardByID(System.Int64 serialId)
	{
		return objCard.GetCardByID(serialId);
	}

	public List<VCM.EMS.Base.Card> GetAll(Boolean isActive)
	{
		return objCard.GetAll(isActive);
	}

	#endregion

}

}