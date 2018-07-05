using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.Item;
//********************************************************************
// 描述: 称号选择Item(简单的图片信息)
// 作者: wangbiyu
// 创建时间: 2018-3-9
//
//
//********************************************************************
public class IntimacyChooseItem : MonoBehaviour {

    public UILabel m_Title;
    public UILabel m_Level;
    public UILabel m_Desc;
    public UISprite m_Icon;
    public IntimacyChooseItemAward[] m_Award;
    public UIGrid m_Grid;

    private int m_TitleID = -1;
    void OnEnable()
    {
        if(m_Grid!=null)
        {
            m_Grid.Reposition();
        }
    }

    public void InitChooseItem(string title, int level, string desc, string spriteName, int titleId, List<UIAtlas> atlas, List<string> giftSpriteName, List<AwardItem> Count)
    {
        m_TitleID = titleId;
        if(m_Title!=null)
        {
            m_Title.text = title;
        }
        if(m_Desc!=null)
        {
            m_Desc.text = desc;
        }
        if(m_Icon!=null)
        {
            m_Icon.spriteName = spriteName;
        }
        if(m_Level!=null)
        {
            switch (level)
            {
                case 1:
                    m_Level.text = "一阶";
                    break;
                case 2:
                    m_Level.text = "二阶";
                    break;
                case 3:
                    m_Level.text = "三阶";
                    break;
                case 4:
                    m_Level.text = "四阶";
                    break;
                case 5:
                    m_Level.text = "五阶";
                    break;
                default:
                    m_Level.text = "不知道几阶";
                    break;
            }
        }

        if(m_Award!=null)
        {
            if (atlas==null || giftSpriteName==null || Count==null)
            {
                return;
            }
            //初始化礼物信息
            for(int i=0;i<m_Award.Length;i++)
            {
                //如果越界
                if (i >= atlas.Count || i >= giftSpriteName.Count || i >= Count.Count || atlas[i] == null || giftSpriteName[i] == null || Count[i].ItemCount < 1 || Count[i].ItemId==-1)
                {
                    m_Award[i].gameObject.SetActive(false);
                }
                else
                {
                    m_Award[i].gameObject.SetActive(true);
                    m_Award[i].InitAward(atlas[i], giftSpriteName[i], Count[i].ItemCount,Count[i].ItemId);
                }
            }
        }
        if (m_Grid != null)
        {
            m_Grid.Reposition();
        }
    }


    public void OnClickChoose()
    {
        MessageBoxController.OpenOKCancel(6804, -1, AddExpOK);
    }
    void AddExpOK()
    {
        CG_CARDINTIMACY_LEVELUP_PAK pak = new CG_CARDINTIMACY_LEVELUP_PAK();
        pak.data.CardGuid = IntimacyRoot.Instance.card.Guid;
        pak.data.ChooseTitleID = this.m_TitleID;
        pak.SendPacket();
    }

}
