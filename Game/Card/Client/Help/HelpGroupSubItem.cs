using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.Table;

public class HelpGroupSubItem : MonoBehaviour 
{
    public UILabel m_NameLabel;
    public GameObject m_OnObj;
    public GameObject m_OffObj;

    private Tab_HelpSubClass m_TabData = null;
    public  Tab_HelpSubClass TabData
    {
        get { return m_TabData; }
    }
    void OnEnable()
    {
        Choose(false);
    }
    public void Init(Tab_HelpSubClass tabSubClass)
    {
        if(tabSubClass == null)
        {
            return;
        }
        m_TabData = tabSubClass;
        m_NameLabel.text = m_TabData.Name;
    }

    public void OnClickItem()
    {
        if(HelpRootController.Instance!=null)
        {
            HelpRootController.Instance.HandleOnClickSubItem(this);
        }
    }

    public void Choose(bool bChoose)
    {
        m_OnObj.SetActive(bChoose);
        m_OffObj.SetActive(!bChoose);
    }
}
