using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.Table;
//********************************************************************
// 描述: 符灵亲密度window
// 作者: wangbiyu
// 创建时间: 2018-3-8
//
//
//********************************************************************
public class IntimacyWindow : MonoBehaviour {
    private static IntimacyWindow _Instance;
    public static IntimacyWindow Instance
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
    public IntimacyBuyWindow m_BuyWindow;//购买窗口
    public IntimacyChooseWindow m_ChooseWindow;//称号选择窗口
    public IntimacyUpWindow m_UpWindow;//提升窗口
    public IntimacyIllustrateWindow m_PreviewWindow;//称号预览窗口

    void OnEnable()
    {
        if (m_BuyWindow != null)
        {
            m_BuyWindow.gameObject.SetActive(false);
        }
        if(m_UpWindow!=null)
        {
            m_UpWindow.gameObject.SetActive(false);
        }
        if(m_ChooseWindow!=null)
        {
            m_ChooseWindow.gameObject.SetActive(false);
        }
        if(m_PreviewWindow!=null)
        {
            m_PreviewWindow.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 打开购买窗口
    /// </summary>
    public void OpenBuyWindow(List<GiftPair> favoriteGiftList)
    {
        if(m_BuyWindow!=null)
        {
            m_BuyWindow.gameObject.SetActive(true);
            m_BuyWindow.UpdateAllItem(favoriteGiftList);
        }
    }
    public void UpdateBuyWindow()
    {
        if(m_BuyWindow!=null)
        {
            m_BuyWindow.UpdateAllItem();
        }
    }
    //购买窗口是否处于打开状态
    public bool IsBuyWindowOpen()
    {
        if(m_BuyWindow!=null)
        {
            return m_BuyWindow.gameObject.activeInHierarchy;
        }
        return false;
    }

    public void OpenChooseWindow(List<Tab_CardIntimacyTitle> nextLvTitle,int cardId)
    {
        if(m_ChooseWindow!=null)
        {
            m_ChooseWindow.gameObject.SetActive(true);
            m_ChooseWindow.InitChooseWindow(nextLvTitle, cardId);
        }
        if (m_BuyWindow != null)
        {
            m_BuyWindow.gameObject.SetActive(false);
        }
        if (m_UpWindow != null)
        {
            m_UpWindow.gameObject.SetActive(false);
        }
    }
    //打开等级提升window
    public void OpenUpWindow(int nextTitleID)
    {
        Tab_CardIntimacyTitle intimacyTitle = TableManager.GetCardIntimacyTitleByID(nextTitleID, 0);
        if (intimacyTitle==null)
        {
            return;
        }
        if(m_UpWindow!=null)
        {
            m_UpWindow.gameObject.SetActive(true);
            m_UpWindow.InitUpdateWindow(intimacyTitle.IntimacyLevel);
        }
    }
    
    //打开称号预览窗口
    public void OpenTitlePreviewWindow(int cardTitle)
    {
        if(m_PreviewWindow!=null)
        {
            m_PreviewWindow.gameObject.SetActive(true);
            m_PreviewWindow.DrawTree(cardTitle);
        }
    }

    public void CloseChooseWindow()
    {
        //关闭称号选择窗口
        if (m_ChooseWindow != null)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void CloseWindow()
    {
        if(m_ChooseWindow!=null &&m_ChooseWindow.gameObject.activeInHierarchy)
        {
            return;
        }
        this.gameObject.SetActive(false);
    }


}
