using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.Table;

public class IntimacyIllustrateWindow : MonoBehaviour {
    //详情
    public UILabel m_Title;
    public UILabel m_Level;
    public UILabel m_Desc;
    public IntimacyChooseItem m_Award;

    private int m_CurWatchTitleID = -1;//当前注视的ID
    public IntimacyIllustrateTitleItem[] m_TitleItem;

    



    //1、给每个Item赋一个ID
    public void DrawTree(int cardTitleId)
    {
        Tab_CardIntimacyTitle tTitle=TableManager.GetCardIntimacyTitleByID(cardTitleId,0);
        if(tTitle==null)
        {
            return;
        }
        int baseNum=1;
        if(tTitle.Sex==2)
        {
            baseNum=1001;
        }
        List<int> link = IntimacyRoot.Instance.GetLinkTitle();
        //给ID赋值
        for(int i=0;i<m_TitleItem.Length;i++)
        {
            //如果当前节点在历史集合中，则点亮它
            if(link.Contains(i + baseNum))
            {
                m_TitleItem[i].InitTitleItem(i + baseNum, true, tTitle.Icon);
            }
            else
            {
                m_TitleItem[i].InitTitleItem(i + baseNum, false, tTitle.Icon);
            }
            //点亮现在的
            if (i + baseNum == cardTitleId)
            {
                m_TitleItem[i].PitchOn();//点亮时同时弹出提示
            }
        }
        m_CurWatchTitleID = cardTitleId;
    }

    /// <summary>
    /// 展示详情
    /// </summary>
    /// <param name="titleId">称号ID</param>
    public void ShowDetail(int titleId)
    {
        if(titleId==m_CurWatchTitleID)
        {
            return;
        }
        Tab_CardIntimacyTitle title = TableManager.GetCardIntimacyTitleByID(titleId, 0);
        if(title==null)
        {
            return;
        }
        //拿到它的奖励信息
        List<UIAtlas> Atlas = IntimacyRoot.Instance.GetUIAtlas(titleId);
        List<string> Icon = IntimacyRoot.Instance.GetSpriteName(titleId);
        List<AwardItem> Count = IntimacyRoot.Instance.GetItem(titleId);

        if (title!=null)
        {
            m_Award.InitChooseItem(title.Title, title.IntimacyLevel, title.Desc, title.Icon, title.Id, Atlas, Icon, Count);
        }

        //把之前的取消勾选
        for(int i=0;i<m_TitleItem.Length;i++)
        {
            if(m_CurWatchTitleID==m_TitleItem[i].TitleId)
            {
                m_TitleItem[i].CancelPitOn();
            }
        }
        m_CurWatchTitleID = titleId;
    }
    

    
}
