using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Games;
using Games.Table;
using Games.GlobeDefine;
//********************************************************************
// 描述: 符灵图鉴界面，管理Group，Preview,Detail
// 作者: wangbiyu
// 创建时间: 2018-2-28
//
//
//********************************************************************
public class CollectionLevel2Fuling : CollectionLevel2Base
{
    private static CollectionLevel2Fuling _instance;
    public static CollectionLevel2Fuling Instance
    {
        get
        {
            return _instance;
        }
    }

    //详情
    public CollectionLevel2FulingDetail m_FulingDetail = null;
    //预览
    public CollectionLevel2FulingPreview m_Preview = null;
    


    void Awake()
    {
        _instance = this;
    }
    /// <summary>
    /// 默认打开preview，关闭Detail和Group
    /// </summary>
    void OnEnable()
    {
        base.OnEnable();
        if(m_Preview==null && m_PreviewObj!=null)
        {
            m_Preview = m_PreviewObj.GetComponent<CollectionLevel2FulingPreview>();
        }
        if(m_FulingDetail!=null)
        {
            m_FulingDetail.gameObject.SetActive(false);
        }
    }
    #region 关闭相关


    /// <summary>
    /// Fuling的退出方法
    /// </summary>
    /// <returns>若回到level1，则返回true</returns>
    public bool OnCloseClick()
    {
        //如果处于preview状态
        if (m_Preview!=null && m_Preview.gameObject.activeInHierarchy)
        {
            if(m_Preview.Close(PREVIEW_CLOSETYPE.CLOSETYPE_LEVEL1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //如果在detail状态点退出
        else if (m_FulingDetail!=null && m_FulingDetail.gameObject.activeInHierarchy)
        {
            m_FulingDetail.Close();
            return false;
        }
        //如果处于Group状态点退出
        else if(m_Group!=null && m_Group.gameObject.activeInHierarchy)
        {
            m_Group.Close();
            return false;
        }
        return true;
    }
    #endregion

    #region 功能函数

    /// <summary>
    /// 展示卡片详情：关闭preview后打开detail
    /// </summary>
    public void ShowCardDetail(int cardid)
    {
        if(m_Preview==null)
        {
            return;
        }
        if (!m_Preview.Close(PREVIEW_CLOSETYPE.CLOSETYPE_DETAIL))
        {
            return;
        }
        if(m_FulingDetail!=null)
        {
            m_FulingDetail.ResetData(m_Preview.CurList);
            m_FulingDetail.UpdateCard(cardid, GameManager.PlayerDataPool.IsCardGet(cardid));
        }
    }


    #endregion
}
