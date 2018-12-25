using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.GlobeDefine;
using Games.Table;
public class CollectionFulingItem : MonoBehaviour 
{
    public UILabel m_CardNameLabel;
    public UISprite m_CardHeadSprite;
    public GameObject m_ChooseObj;
    public UISprite m_BG;

    private int mCardId = GlobeVar.INVALID_ID;
    public void Refresh(int cardId)
    {
        Tab_Card tCard = TableManager.GetCardByID(cardId,0);
        if(tCard == null)
        {
            return;
        }
        mCardId = cardId;
        Tab_RoleBaseAttr tRoleBase = TableManager.GetRoleBaseAttrByID(tCard.GetRoleBaseIDStepbyIndex(0), 0);
        if(tRoleBase==null)
        {
            return;
        }
        Tab_CharModel tModel = TableManager.GetCharModelByID(tRoleBase.CharModelID, 0);
        if(tModel == null)
        {
            return;
        }
        m_CardNameLabel.text = tRoleBase.Name;
        m_CardHeadSprite.spriteName = tModel.HeadPic;
        if (CollectionFulingController.Instance != null && m_ChooseObj!=null)
        {
            m_ChooseObj.SetActive(cardId == CollectionFulingController.Instance.mCurChooseCard);
        }
        if(GameManager.PlayerDataPool.CollectionData.IsGetCard(mCardId))
        {
            m_CardHeadSprite.color = GlobeVar.NORMALCOLOR;
        }
        else
        {
            m_CardHeadSprite.color = GlobeVar.GRAYCOLOR;
        }
    }
    public void OnClick()
    {
        if(CollectionFulingController.Instance!=null)
        {
            CollectionFulingController.Instance.HandleOnClickItem(mCardId);
        }
    }

}
