using System.Collections;
using System.Collections.Generic;
//using Games;
using UnityEngine;

public class DynamicChooseMenu : MonoBehaviour
{
    public GameObject m_ChooseItem;
    public UIGrid m_ItemGrid;
    public UILabel m_CurItemLabel;
    public BoxCollider m_MenuBox;
    public GameObject m_MenuEnableIcon;

    public delegate void OnItemChoose();
    public OnItemChoose delOnItemChoose = null;

    public delegate bool CanItemChoose();
    public CanItemChoose delCanItemChoose = null;

    private int m_ItemIndex = 0;

    private object m_CurParam = null;
    public object CurParam
    {
        get { return m_CurParam; }
    }

    void Start()
    {
        m_ItemGrid.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        delOnItemChoose = null;
        m_ItemIndex = 0;
    }

    public void UpdateEnable(bool bEnable)
    {
        if (m_MenuBox != null)
        {
            m_MenuBox.enabled = bEnable;
        }

        if (m_MenuEnableIcon != null)
        {
            m_MenuEnableIcon.SetActive(bEnable);
        }
    }

    //public void AddItem(string szName, object param, bool bDefault)
    //{
    //    var item = AssetManager.InstantiateObjToParent(m_ChooseItem, m_ItemGrid.transform, m_ItemIndex.ToString());
    //    if (item == null)
    //    {
    //        return;
    //    }

    //    var itemLogic = item.GetComponent<DynamicChooseItem>();
    //    if (itemLogic == null)
    //    {
    //        return;
    //    }

    //    itemLogic.Init(szName, param, this);

    //    m_ItemGrid.repositionNow = true;

    //    if (bDefault)
    //    {
    //        m_CurItemLabel.text = szName;
    //        m_CurParam = param;
    //    }

    //    m_ItemIndex += 1;
    //}

    //public void CleanUp()
    //{
    //    m_CurItemLabel.text = "";
    //    Utils.CleanGrid(m_ItemGrid.gameObject);

    //    m_ItemIndex = 0;
    //}

    public void OnCurClick()
    {
        m_ItemGrid.gameObject.SetActive(!m_ItemGrid.gameObject.activeSelf);
    }

    public void HandleItemClick(string szName, object param)
    {
        m_ItemGrid.gameObject.SetActive(false);

        if (delCanItemChoose != null && false == delCanItemChoose())
        {
            return;
        }

        m_CurItemLabel.text = szName;
        m_CurParam = param;

        if (delOnItemChoose != null)
        {
            delOnItemChoose();
        }
    }

    public void CloseMenu()
    {
        m_ItemGrid.gameObject.SetActive(false);
    }
}
