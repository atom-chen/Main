using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//********************************************************************
// 描述: 图鉴系统各界面的基类
// 作者: wangbiyu
// 创建时间: 2018-3-14
//********************************************************************
public class CollectionLevel2Base : MonoBehaviour {

    protected List<int> m_ShowCardList = new List<int>();

    //Preview界面
    public GameObject m_PreviewObj;
    //组合界面
    public CollectionGroup m_Group;

    protected void OnEnable()
    {
        if (m_PreviewObj != null)
        {
            m_PreviewObj.SetActive(true);
        }

        if (m_Group != null)
        {
            m_Group.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 展示该一级组合下的所有二级组合（HandBook表的ID是二级组合的唯一标识）
    /// </summary>
    /// <param name="itemName">组合名称</param>
    public void ShowGroupItem(string itemName)
    {
        //调用Group去show
        if(m_Group!=null)
        {
            m_Group.ShowGroupItem(itemName);
        }

    }

    /// <summary>
    /// 根据传来的GC包更新界面
    /// </summary>
    public void GC_UpdateGroupView()
    {
        //1、在领取组合奖励后更新组合界面
        if(m_Group!=null)
        {
            m_Group.GC_ShowGroupItem();
        }

    }
}
