using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicChooseItem : MonoBehaviour
{
    public UILabel m_NameLabel;

    private object m_Param = null;
    private DynamicChooseMenu m_ParentMenu = null;

    public void Init(string szName, object param, DynamicChooseMenu parentMenu)
    {
        m_NameLabel.text = szName;

        m_Param = param;
        m_ParentMenu = parentMenu;
    }

    void OnClick()
    {
        if (m_ParentMenu != null)
        {
            m_ParentMenu.HandleItemClick(m_NameLabel.text, m_Param);
        }
    }
}
