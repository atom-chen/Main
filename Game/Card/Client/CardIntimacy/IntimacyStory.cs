using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.Table;
using Games.LogicObj;
//********************************************************************
// 描述: 符灵亲密度-故事脚本
// 作者: wangbiyu
// 创建时间: 2018-3-13
//
//
//********************************************************************
public class IntimacyStory : MonoBehaviour {
    public IntimacyStoryItem[] m_StoryItem;
    private static IntimacyStory _Instance;
    public static IntimacyStory Instance
    {
        get
        {
            return _Instance;
        }
    }
    void Awake()
    {
        _Instance = this;
    }

    void OnEnable()
    {
        if(IntimacyRoot.Instance!=null)
        {
            UpdateStory(IntimacyRoot.Instance.card);
        }
    }

    public void UpdateStory(Card card)
    {
        if(card==null)
        {
            return;
        }
        Tab_CardIntimacyTitle tTatle = TableManager.GetCardIntimacyTitleByID(card.IntimacyTitleID, 0);
        if(tTatle==null)
        {
            return;
        }
        //根据Card级别决定显示的StoryItem开启情况->i表示第i+2级别
        for(int i=0;i<m_StoryItem.Length;i++)
        {
            //开启
            if(i+2<=tTatle.IntimacyLevel)
            {
                //去取得该符灵的奖励ID
                int storyId=IntimacyRoot.Instance.GetAwardStoryID(i + 2);//输入的参数表示阶级
                //红点
                bool redpoint = StoryManager.IsNewStoryLine(storyId, false, card.Guid);
                if(storyId!=-1)
                {
                    m_StoryItem[i].InitItem(storyId, redpoint, true);
                }
                else
                {
                    m_StoryItem[i].InitItem(storyId, redpoint, false);
                }

            }
            //关闭
            else
            {
                m_StoryItem[i].InitItem(-1, false, false);
            }
        }

    }
}
