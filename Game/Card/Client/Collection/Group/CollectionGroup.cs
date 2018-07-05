using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games;
using Games.Table;

//********************************************************************
// 描述: 符灵Group总管
// 作者: wangbiyu
// 创建时间: 2018-2-
//
//
//********************************************************************
public class CollectionGroup : MonoBehaviour {
    public const string CollectionPicRoot = "Texture/T_Collection/";

    public TweenRotation m_BtnTween;
    //它管辖内的List和Bag
    public CollectionGroupList m_GroupList;
    public CollectionGroupBag m_GroupBag;
    private string itemName = "";

    void OnEnable()
    {
        //默认打开List
        if(m_GroupList!=null)
        {
            m_GroupList.gameObject.SetActive(true);
        }
        if(m_GroupBag!=null)
        {
            m_GroupBag.gameObject.SetActive(false);
        }
        if(m_BtnTween!=null)
        {
            m_BtnTween.gameObject.SetActive(true);
        }
    }
    void OnDisable()
    {
        m_BtnTween.gameObject.SetActive(false);
    }


    /// <summary>
    /// 如果是在2级界面 则回到1级界面
    /// 如果是在1级界面 则播放结束动画
    /// </summary>
    public void Close()
    {
        if(m_BtnTween.isActiveAndEnabled)
        {
            return;
        }
        if (m_GroupBag != null && m_GroupBag.gameObject.activeInHierarchy)
        {
            //关闭二级界面
            m_GroupBag.CloseGroupBag();
        }
        else
        {
            if(m_GroupList!=null)
            {
                if (m_GroupList.CloseList(GROUPCLOSETYPE.GROUPCLOSETYPE_PREVIEW))
                {
                    //播放按钮动画
                    if (m_BtnTween != null)
                    {
                        m_BtnTween.ResetToBeginning();
                        m_BtnTween.PlayForward();
                    }
                }
            }
        }
    }

    

    /// <summary>
    /// 关闭List->展示Bag
    /// </summary>
    /// <param name="itemName"></param>
    public void ShowGroupItem(string itemName)
    {
        if(itemName==null || "".Equals(itemName))
        {
            return;
        }
        this.itemName = itemName;
        if (m_GroupList != null && m_GroupBag!=null)
        {
            if (m_GroupList.CloseList(GROUPCLOSETYPE.GROUPCLOSETYPE_BAG))
            {
                //用Bag进行初始化
                List<Tab_Handbook> groupsInfo2 = CollectionGroupController.Instance.GetGroup2NameInfo(itemName);
                m_GroupBag.UpdateBag(groupsInfo2);
            }
        }
    }


    //刷新Bag显示
    public void GC_ShowGroupItem()
    {
        if (CollectionGroupController.Instance != null && m_GroupBag != null)
        {
            List<Tab_Handbook> groupsInfo2 = CollectionGroupController.Instance.GetGroup2NameInfo(itemName);
            m_GroupBag.UpdateBag(groupsInfo2);
        }


    }

}
