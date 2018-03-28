using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Games;
using Games.Table;
using Games.GlobeDefine;

public class CollectionLevel2Talisman : CollectionLevel2Base
{
    private static CollectionLevel2Talisman _instance;
    public static CollectionLevel2Talisman Instance
    {
        get
        {
            return _instance;
        }  
    }


    //详情
    public CollectionLevel2TalismanDetail m_TalismanDetail = null;
    //预览
    private CollectionLevel2TalismanPreview m_Preview = null;


    void Awake()
    {
        _instance = this;
    }

    void OnEnable()
    {
        base.OnEnable();
        if (m_Preview == null && m_PreviewObj != null)
        {
            m_Preview = m_PreviewObj.GetComponent<CollectionLevel2TalismanPreview>();
        }
        if (m_TalismanDetail != null)
        {
            m_TalismanDetail.gameObject.SetActive(false);
        }
    }



    /// <summary>
    /// 展示卡片详情：关闭preview后打开detail
    /// </summary>
    /// <param name="talismanClassId">卡片ID</param>
    public void ShowTalismanDetail(int talismanClassId)
    {
        if (!m_Preview.Close(PREVIEW_CLOSETYPE.CLOSETYPE_DETAIL))
        {
            return;
        }         
        if(m_TalismanDetail!=null)
        {
            m_TalismanDetail.ResetData(m_Preview.CurList);
            m_TalismanDetail.UpdateTalisman(talismanClassId, !GameManager.PlayerDataPool.IsTalismanGet(talismanClassId));
        }
    }



    /// <summary>
    /// 在Level2界面下按退出
    /// </summary>
    /// <returns>若回到level1，则返回true</returns>
    public bool OnCloseClick()
    {
        //如果处于preview状态，则该退出会关闭level2，回到level1
        if (m_Preview.gameObject.activeInHierarchy)
        {
            if (m_Preview.Close(PREVIEW_CLOSETYPE.CLOSETYPE_LEVEL1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
            //如果在detail状态点退出，则关闭detail 打开preview
        else if (m_TalismanDetail.gameObject.activeInHierarchy)
        {
            m_TalismanDetail.Close();
            return false;
        }
        //如果在group点退出
        else if(m_Group.gameObject.activeInHierarchy)
        {
            m_Group.Close();
            return false;
        }
        return false;
    }

}
