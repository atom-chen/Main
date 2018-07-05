using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games;
using Games.Table;

public class CollectionGroupItem : MonoBehaviour {
    public UITexture m_Photo;
    public UILabel m_Title;
    public UILabel m_Num;

    /// <summary>
    /// 初始化Item
    /// </summary>
    /// <param name="picPath">贴图地址</param>
    /// <param name="title">标题</param>
    /// <param name="num">已完成/总数</param>
    public void InitItem(string picPath,string title,string num)
    {
        if(m_Photo!=null)
        {
            m_Photo.mainTexture = Utils.LoadTexture(picPath);
        }
        if (m_Title!=null)
        {
            m_Title.text = title;
        }
        if(m_Num!=null)
        {
            m_Num.text = num;
        }     
    }


    
    public void OnClickItem()
    {
        if(CollectionRoot.Instance!=null)
        {
            CollectionRoot.Instance.ShowGroupItem(m_Title.text);
        }
    }

    
}
