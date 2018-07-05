using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Games;
using Games.Table;

//********************************************************************
// 描述: 图鉴/组合功能 Bag
// 作者: wangbiyu
// 创建时间: 2018-3-1
//
//
//********************************************************************
public class CollectionGroupBag : MonoBehaviour {
    public GameObject m_GroupList;

    public TweenAlpha m_TweenAlpha;

    public CollectionGroupItem m_ListItems;
    public CollectionGroupBagItem[] m_BagItems;

    public UITable m_Table;
    void Start()
    {
        if(m_TweenAlpha!=null)
        {
            m_TweenAlpha.AddOnFinished(new EventDelegate(OnTweenFinish));      
        } 
    }
    void OnEnable()
    {
        //播放动画
        if(m_TweenAlpha!=null)
        {
            m_TweenAlpha.PlayForward();
        }
        if(m_BagItems==null)
        {
            m_BagItems = this.GetComponentsInChildren<CollectionGroupBagItem>();
        }
        if(m_Table)
        {
            m_Table.Reposition();
        }
    }
    void OnTweenFinish()
    {
        //结束后关闭该游戏物体
        if(m_TweenAlpha.value==0)
        {
            //关闭后回到List
            if (m_GroupList != null)
            {
                m_GroupList.gameObject.SetActive(true);
            }
            this.gameObject.SetActive(false);
        }
    }
    //关闭背包的方法
    public void CloseGroupBag()
    {
        if (m_TweenAlpha==null)
        {
            return;
        }
        if (m_TweenAlpha.isActiveAndEnabled)
        {
            return;
        }
        if(m_TweenAlpha!=null)
        {
            m_TweenAlpha.PlayReverse();
        }
    }

    /// <summary>
    /// 更新Bag内容=ListItem+7个BagItems
    /// </summary>
    /// <param name="groupsInfo2">当前一级分类下的所有二级分类信息，用来初始化Item</param>
    public void UpdateBag(List<Tab_Handbook> groupsInfo2)
    {
        if (m_BagItems == null || groupsInfo2 == null || groupsInfo2.Count == 0)
        {
            return;
        }
        int done = 0;//已完成数量
        //先初始化bagItems
        for (int i = 0; i < m_BagItems.Length; i++)
        {
            if (m_BagItems[i] == null)
            {
                break;
            }
            //越界检测
            if (i >= groupsInfo2.Count)
            {
                m_BagItems[i].gameObject.SetActive(false);
                continue;
            }
            m_BagItems[i].gameObject.SetActive(true);
            if(m_BagItems[i].InitItem(groupsInfo2[i]))
            {
                done++;
            }
        }
        m_ListItems.InitItem(CollectionGroup.CollectionPicRoot + groupsInfo2[0].Group1Pic, groupsInfo2[0].Group1Name, done + "/" + groupsInfo2.Count);
        if (m_Table != null)
        {
            m_Table.Reposition();
        }
    }

    

    
}
