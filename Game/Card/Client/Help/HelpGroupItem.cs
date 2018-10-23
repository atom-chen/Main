using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.Table;

public class HelpGroupItem : MonoBehaviour 
{
    public UIGrid m_SubClassGrid;
    public UILabel m_ClassName;
    public GameObject m_OnObj;
    public GameObject m_OffObj;

    private List<HelpGroupSubItem> m_SubClassList = new List<HelpGroupSubItem>();
    private Tab_HelpClass m_TabHelpClass = null;
    public Tab_HelpClass TabData
    {
        get { return m_TabHelpClass; }
    }
    public void Init(Tab_HelpClass tabHelpClass,List<Tab_HelpSubClass> tabSubClassList,GameObject subItemPrefab)
    {
        if(tabHelpClass == null)
        {
            return;
        }
        m_TabHelpClass = tabHelpClass;
        m_ClassName.text = tabHelpClass.Name;
        if(tabSubClassList == null)
        {
            return;
        }
        foreach (Tab_HelpSubClass tabSubClass in tabSubClassList)
        {
            //再加载N个孩子
            GameObject subObj = NGUITools.AddChild(m_SubClassGrid.gameObject, subItemPrefab);
            HelpGroupSubItem item = subObj.GetComponent<HelpGroupSubItem>();
            if(item!=null)
            {
                item.Init(tabSubClass);
            }
            m_SubClassList.Add(item);
        }
        ShowSubClassList(false);
        m_SubClassGrid.Reposition();
    }

    public void OnClickItem()
    {
        if(HelpRootController.Instance!=null)
        {
            HelpRootController.Instance.HandleOnClickItem(this);
        }
    }

    public void ShowSubClassList(bool bShow)
    {
        m_SubClassGrid.gameObject.SetActive(bShow);
        m_OnObj.SetActive(bShow);
        m_OffObj.SetActive(!bShow);
        m_SubClassGrid.Reposition();
    }

    public void OnChooseSubClassItem(HelpGroupSubItem item)
    {
        foreach(HelpGroupSubItem temp in m_SubClassList)
        {
            temp.Choose(item == temp);
        }
    }
}
