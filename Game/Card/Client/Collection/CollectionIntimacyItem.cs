using Games;
using Games.GlobeDefine;
using Games.Table;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionIntimacyItem : MonoBehaviour 
{
    public ItemSlot m_Icon;
    public UILabel m_IntimacyNameLabel;
    public UISprite m_RareFrameSprite;
    public GameObject m_ChooseObj;


    private int m_CardID;

    public void Init(int cardID)
    {
        gameObject.SetActive(true);
        m_CardID = cardID;
        Tab_CardIntimacyAward _TabAward = TableManager.GetCardIntimacyAwardByID(cardID, 0);
        if (_TabAward == null)
            return;
        int iTemID = _TabAward.LetterID;
        Tab_Item tItem = TableManager.GetItemByID(iTemID, 0);
        if (tItem == null)
            return;
        m_Icon.Init(tItem.Id, null, null,null);
        m_RareFrameSprite.spriteName = Utils.GetColorIcon(tItem.Color);
        //是否收集
        if (GameManager.PlayerDataPool.CollectionData != null)
        {
            bool bSuc = GameManager.PlayerDataPool.CollectionData.IsGetIntimacyLetter(cardID);
            m_IntimacyNameLabel.text = tItem.Name;
            if (bSuc)
            {
                m_Icon.m_Icon.color = GlobeVar.NORMALCOLOR;
            }
            else
            {
                m_Icon.m_Icon.color = GlobeVar.GRAYCOLOR;
            }
        }
        if(CollectionIntimacyController.Instance!=null)
        {
            m_ChooseObj.SetActive(m_CardID == CollectionIntimacyController.Instance.mCurChooseCard);
        }
    }
    public void OnClick()
    {
        if (CollectionIntimacyController.Instance != null)
        {
            CollectionIntimacyController.Instance.HandleOnClickItem(m_CardID);
        }
    }
}
