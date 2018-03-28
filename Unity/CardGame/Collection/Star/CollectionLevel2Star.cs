using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Games;
using Games.Table;
using Games.GlobeDefine;

public class CollectionLevel2Star : CollectionLevel2Base
{
    private static CollectionLevel2Star _instance;
    public static CollectionLevel2Star Instance
    {
        get
        {
            return _instance;
        }
    }



    //详情
    public CollectionLevel2StarDetail m_StarDetail = null;

    public CollectionLevel2StarPreview m_Preview;

    void Awake()
    {
        _instance = this;
    }


    void OnEnable()
    {
        base.OnEnable();
        if (m_Preview == null && m_PreviewObj != null)
        {
            m_Preview = m_PreviewObj.GetComponent<CollectionLevel2StarPreview>();
        }
        if (m_StarDetail != null)
        {
            m_StarDetail.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 展示卡片详情：关闭preview后打开detail
    /// </summary>
    public void ShowStarDetail(int starClassId)
    {
        if (!m_Preview.Close(PREVIEW_CLOSETYPE.CLOSETYPE_DETAIL))
        {
            return;
        }
        if(m_StarDetail!=null)
        {
            m_StarDetail.ResetData(m_Preview.CurList);
            m_StarDetail.UpdateStar(starClassId, GameManager.PlayerDataPool.IsQuartzClassGet(starClassId));
        }
    }



    /// <summary>
    /// 在Level2界面下按退出
    /// </summary>
    /// <returns>若回到level1，则返回true</returns>
    public bool OnCloseClick()
    {
        //如果处于preview状态
        if (m_PreviewObj != null && m_PreviewObj.gameObject.activeInHierarchy)
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
        else if (m_StarDetail != null && m_StarDetail.gameObject.activeInHierarchy)
        {
            m_StarDetail.Close();
            return false;
        }
        //如果处于Group状态点退出
        else if (m_Group != null && m_Group.gameObject.activeInHierarchy)
        {
            m_Group.Close();
            return false;
        }
        return true;
    }
}
