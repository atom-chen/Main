using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games;
using Games.Table;
//********************************************************************
// 描述:图鉴组合List界面，由m_GroupType区分
// 作者: wangbiyu
// 创建时间: 2018-2-28
//
//********************************************************************
public enum GROUPCLOSETYPE
{
    GROUPCLOSETYPE_PREVIEW,
    GROUPCLOSETYPE_BAG
}
public class CollectionGroupList : MonoBehaviour {
    public CollectionLevel2PreviewBase m_Preview;

    public COLLECTIONGROUPTYPE m_GroupType;    //图鉴类型
    private TweenAlpha m_TweenAlpha;      //开场动画
    public CollectionGroupBag m_Bag;  //二级界面
    public CollectionGroup m_Group;

    public CollectionGroupItem[] m_Items;

    private GROUPCLOSETYPE m_CloseType = GROUPCLOSETYPE.GROUPCLOSETYPE_PREVIEW;   //关闭类型，决定下一步去哪个界面

    void Start()
    {
        if(m_TweenAlpha!=null)
        {
            m_TweenAlpha.AddOnFinished(OnTweenFinish);
        }
    }
    //激活时播放动画
    void OnEnable()
    {
        if(m_TweenAlpha==null)
        {
            m_TweenAlpha = this.GetComponent<TweenAlpha>();
        }
        if(m_TweenAlpha!=null)
        {
            m_TweenAlpha.PlayForward();
        }
        ShowName1();
    }
    /// <summary>
    /// 关闭List
    /// </summary>
    public bool CloseList(GROUPCLOSETYPE type)
    {
        if(m_TweenAlpha==null)
        {
            return false;
        }
        if (m_TweenAlpha.isActiveAndEnabled)
        {
            return false;
        }
        if(m_TweenAlpha!=null)
        {
            m_TweenAlpha.PlayReverse();
        }
        this.m_CloseType = type;
        return true;
    }
    //播放结束时执行该方法
    private void OnTweenFinish()
    {
        //如果是收，则关闭该界面
        if(m_TweenAlpha==null)
        {
            return;
        }
        if(m_TweenAlpha.value==0)
        {
            //去到Preview
            if (m_CloseType == GROUPCLOSETYPE.GROUPCLOSETYPE_PREVIEW && m_Preview!=null && m_Group!=null)
            {
                m_Preview.gameObject.SetActive(true);
                m_Group.gameObject.SetActive(false);
            }
            //去到Bag
            else if (m_CloseType == GROUPCLOSETYPE.GROUPCLOSETYPE_BAG)
            {
                if (m_Bag != null)
                {
                    m_Bag.gameObject.SetActive(true);
                    this.gameObject.SetActive(false);
                }
            }
        }
    }
    // 展示一级分类面板
    void ShowName1()
    {
        //拿到数据
        List<Group1Item> name1List = CollectionGroupController.Instance.GetGroup1NameInfo(m_GroupType);
        if(name1List==null)
        {
            return;
        }
        //展示
        for(int i=0;i<m_Items.Length;i++)
        {
            //如果越界了
            if(i>=name1List.Count)
            {
                if(m_Items[i]!=null)
                {
                    m_Items[i].gameObject.SetActive(false);
                }
                continue;
            }
            m_Items[i].gameObject.SetActive(true);
            m_Items[i].InitItem(CollectionGroup.CollectionPicRoot + name1List[i].picPath, name1List[i].title, name1List[i].done + "/" + name1List[i].total); 
        }
    }
   
}
