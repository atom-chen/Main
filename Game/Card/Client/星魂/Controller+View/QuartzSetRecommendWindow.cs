using System.Collections;
using System.Collections.Generic;
using Games.GlobeDefine;
using Games.Table;
using UnityEngine;
using Games;

public class QuartzSetRecommendWindow : MonoBehaviour
{
    private const string RareSpriteName = "Card_quality";//稀有度SpriteName前缀
    public UITexture m_CardShow;
    public UILabel m_CardName;          //名称：类别
    public UILabel m_CardIntroduce;     //推荐理由
    public UISprite m_RareFrame;        //稀有度边框
    public UISprite m_NameBg;

    public UIGrid m_ItemGrid;
    public QuartzSetRecommendItem[] m_RecommendItem;

    public void Show(int nCardId)
    {
        var tCard = TableManager.GetCardByID(nCardId, 0);
        if (tCard == null)
        {
            return;
        }
        Tab_CardSetRecommend tRecommend = TableManager.GetCardSetRecommendByID(tCard.RecommendSet, 0);
        if (tRecommend == null)
        {
            return;
        }

        gameObject.SetActive(true);

        //左边
        if (tCard.getHeadIconCount() >= 1)
        {
            m_CardShow.mainTexture = Utils.LoadFuLingIconTecture(tCard.GetHeadIconbyIndex(0));
        }
        string attr = "";
        switch (tCard.BattleType)
        {
            case (int)CardBattleType.Attack:
                attr = StrDictionary.GetDicByID(7805);
                break;
            case (int)CardBattleType.Defence:
                attr = StrDictionary.GetDicByID(7806);
                break;
            case (int)CardBattleType.Curse:
                attr = StrDictionary.GetDicByID(7807);
                break;
            case (int)CardBattleType.Enhance:
                attr = StrDictionary.GetDicByID(7808);
                break;
        }
        //名称：类别
        // 名称
        Tab_RoleBaseAttr roleBase = TableManager.GetRoleBaseAttrByID(tCard.GetRoleBaseIDStepbyIndex(0), 0);
        if (null != roleBase)
        {
            m_CardName.text = StrDictionary.GetDicByID(7801, roleBase.Name, attr);
        }
        m_RareFrame.spriteName = RareSpriteName + (tCard.Rare + 1);

        //右边
        if (tCard.RecommendSet != GlobeVar.INVALID_ID)
        {
            m_CardIntroduce.text = tRecommend.REASON;
            for (int i = 0; i < m_RecommendItem.Length; i++)
            {
                if (tRecommend.GetRecommendSetbyIndex(i) != GlobeVar.INVALID_ID)
                {
                    m_RecommendItem[i].gameObject.SetActive(true);
                    m_RecommendItem[i].Init(tRecommend.GetRecommendSetbyIndex(i), gameObject);
                }
                else
                {
                    m_RecommendItem[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < m_RecommendItem.Length; i++)
            {
                m_RecommendItem[i].gameObject.SetActive(false);
            }
        }
        m_ItemGrid.Reposition();
        m_NameBg.ResetAnchors();
    }

    public void OnCloseClick()
    {
        gameObject.SetActive(false);
    }
}
