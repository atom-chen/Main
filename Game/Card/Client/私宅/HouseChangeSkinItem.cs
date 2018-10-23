using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.Table;
using Games;
using Games.GlobeDefine;

public class HouseChangeSkinItem : MonoBehaviour 
{
    private Tab_HouseSkin m_TabHouse;
    public UITexture m_UnLockTexture;  //已解锁
    public UITexture m_LockTexture;   //未解锁
    public UILabel m_NameLabel;
    public UILabel m_PriceLabel;

    public GameObject m_BuyObj;
    public GameObject m_ChooseObj;
    public GameObject m_UseObj;
    public GameObject m_PreviewObj;

    void OnDisable()
    {
        m_TabHouse = null;
    }
    public void Refresh(Tab_HouseSkin tabHouseSkin)
    {
        m_TabHouse = tabHouseSkin;
        if(tabHouseSkin == null || Yard.Instance == null)
        {
            this.gameObject.SetActive(false);
            return;
        }
        
        this.gameObject.SetActive(true);
        m_PriceLabel.text = Utils.GetPriceStr((int)SpecialItemID.BindYuanbao, tabHouseSkin.Price);
        m_PreviewObj.SetActive(false);     //默认不允许预览
        //如果是当前场景，则选中
        var curSkin = Yard.GetCurSkin();
        if (curSkin != null && curSkin.Id == tabHouseSkin.Id)
        {
            m_UnLockTexture.gameObject.SetActive(true);
            AssetManager.SetTexture(m_UnLockTexture, tabHouseSkin.Icon);
            m_LockTexture.gameObject.SetActive(false);

            m_ChooseObj.SetActive(true);
            m_UseObj.SetActive(false);
            m_BuyObj.SetActive(false);
        }
        //如果已拥有，则高亮
        else if (GameManager.PlayerDataPool.YardData.IsSkinUnlocked(m_TabHouse.Id))
        {
            m_UnLockTexture.gameObject.SetActive(true);
            AssetManager.SetTexture(m_UnLockTexture, tabHouseSkin.Icon);
            m_LockTexture.gameObject.SetActive(false);

            m_ChooseObj.SetActive(false);
            m_BuyObj.SetActive(false);
            m_UseObj.SetActive(true);
        }
        //如果未拥有，则置灰
        else
        {
            m_UnLockTexture.gameObject.SetActive(false);
            AssetManager.SetTexture(m_LockTexture, tabHouseSkin.Icon);
            m_LockTexture.gameObject.SetActive(true);

            m_ChooseObj.SetActive(false);
            m_PreviewObj.SetActive(m_TabHouse.PreviewCamTVID != GlobeVar.INVALID_ID);
            m_BuyObj.SetActive(true);
            m_UseObj.SetActive(false);
        }
        m_NameLabel.text = tabHouseSkin.Name;
    }

    //预览
    public void OnClickPreview()
    {
        var curSkin = Yard.GetCurSkin();
        //如果是正在使用的场景，不给任何反应
        if (curSkin == null || m_TabHouse == null || curSkin.Id == m_TabHouse.Id || GameManager.PlayerDataPool.YardData.IsSkinUnlocked(m_TabHouse.Id))
        {
            Utils.CenterNotice(7948);
            return;
        }
        HouseScene.PlayTV(m_TabHouse);
    }

    //使用
    public void OnClickUseSkin()
    {
        if (m_TabHouse == null)
        {
            return;
        }
        //如果是正在使用的场景，不给任何反应
        var curSkin = Yard.GetCurSkin();
        if (curSkin == null || curSkin.Id == m_TabHouse.Id)
        {
            Utils.CenterNotice(7948);
            return;
        }
        string str = StrDictionary.GetDicByID(7915, m_TabHouse.Name);
        string ok = StrDictionary.GetDicByID(7936);
        string cencel = StrDictionary.GetDicByID(7935);
        string title = StrDictionary.GetDicByID(7938);
        MessageBoxController.OpenOKCancel(str, title, Use_OK, null, ok, cencel);
    }
    private void Use_OK()
    {
        if (m_TabHouse == null)
        {
            return;
        }
        Yard.SendChangeSkin(m_TabHouse.Id);
    }

    //购买
    public void OnClickBuySkin()
    {
        if (m_TabHouse == null)
        {
            return;
        }
        //如果是正在使用的场景，不给任何反应
        var curSkin = Yard.GetCurSkin();
        if (curSkin == null || curSkin.Id == m_TabHouse.Id)
        {
            Utils.CenterNotice(7948);
            return;
        }
        string str = StrDictionary.GetDicByID(7916, m_TabHouse.Name,m_TabHouse.Price.ToString());
        string ok = StrDictionary.GetDicByID(7917);
        string cencel = StrDictionary.GetDicByID(7935);
        string title = StrDictionary.GetDicByID(7939);
        MessageBoxController.OpenOKCancel(str, title, Buy_OK, null, ok, cencel);
    }
    private void Buy_OK()
    {
        if (m_TabHouse == null)
        {
            return;
        }
        Yard.SendUnlockSkin(m_TabHouse.Id);
    }
    //点击
    public void OnClickItem()
    {
        if(m_TabHouse == null)
        {
            return;
        }
        //如果是正在使用的场景，不给任何反应
        var curSkin = Yard.GetCurSkin();
        if (curSkin == null || curSkin.Id == m_TabHouse.Id)
        {

        }
        //如果已拥有，则弹出更换皮肤
        else if(GameManager.PlayerDataPool.YardData.IsSkinUnlocked(m_TabHouse.Id))
        {
            OnClickUseSkin();
        }
        //如果未拥有，则置灰
        else
        {
            OnClickBuySkin();
        }
    }
}
