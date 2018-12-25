using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.GlobeDefine;
using Games;

public class CollectionRoot : MonoBehaviour 
{
    public GameObject m_FulingRedPoint;
    void Awake()
    {
        CollectionRedTool.sUpdateEvent += RefreshRedPoint;
    }
    void OnDestroy()
    {
        CollectionRedTool.sUpdateEvent -= RefreshRedPoint;
    }
    private void OnEnable()
    {
        UIManager.SetMainCameraStatesOnUIChange(MAINCAMERA_HIDE_UI.COLLECTION, true);
        RefreshRedPoint();
    }
    private void OnDisable()
    {
        UIManager.SetMainCameraStatesOnUIChange(MAINCAMERA_HIDE_UI.COLLECTION, false);
    }

    public static void Show()
    {
        if (!GlobeVar._GameConfig.m_IsCollectionOpen)
        {
            Utils.CenterNotice(6719);
            return;
        }
        UIManager.ShowUI(UIInfo.CollectionRoot);
    }

    public void OnClickCard()
    {
        if(!GlobeVar._GameConfig.m_IsCollectionCardOpen)
        {
            Utils.CenterNotice(6719);
            return;
        }
        UIManager.ShowUI(UIInfo.CollectionCardRoot);
    }

    public void OnClickStar()
    {
        if (!GlobeVar._GameConfig.m_IsCollectionStarOpen)
        {
            Utils.CenterNotice(6719);
            return;
        }
        UIManager.ShowUI(UIInfo.CollectionStarRoot);
    }

    public void OnClickIntimacy()
    {
        if (!GlobeVar._GameConfig.m_IsIntimacyOpen)
        {
            Utils.CenterNotice(6719);
            return;
        }
        UIManager.ShowUI(UIInfo.CollectionIntimacyRoot) ;
    }

    public void OnClickTalisman()
    {
        Utils.CenterNotice(6719);
    }

    public void OnClickYiZhiKao()
    {
        Utils.CenterNotice(6719);
    }

    public void OnClickFengWuZhi()
    {
        Utils.CenterNotice(6719);
    }

    public void OnCliclExit()
    {
        UIManager.CloseUI(UIInfo.CollectionRoot);
    }

    public void RefreshRedPoint()
    {
        m_FulingRedPoint.SetActive(CollectionRedTool.IsCanAward_Card());
    }
}
