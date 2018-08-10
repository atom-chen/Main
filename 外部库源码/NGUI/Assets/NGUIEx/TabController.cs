using UnityEngine;
using System.Collections.Generic;
public class TabController : MonoBehaviour
{

    public int startSelectTab = 0;          // 初始高亮索引
    public GameObject tabRoot = null;

    public TabButton curHighLightTab;      // 当前高亮的索引
    private Transform _tabRootTrans;               // 子结点必须有一个GRID

    public Transform tabRootTrans
    {
        get
        {
            if (_tabRootTrans == null)
            {
                InitData();
            }
            return _tabRootTrans;
        }
    }

    private bool m_bEnableClick;

    public delegate void TabChangedDelegate(TabButton curButton);
    public TabChangedDelegate delTabChanged = null;

    public delegate bool TabWillChangeDelegate(TabButton curButton);
    public TabWillChangeDelegate delTabWillChange = null;
    private List<TabButton> tabs; 

    void Awake()
    {
        //InitData();
    }
     
    void Start()
    {
        InitData();
    }

    private bool inited = false;
    

    public void InitData(bool force = false)
    {
        if (!force && inited)
        {
            return;
        }

        inited = true;
        tabs = new List<TabButton>();

        _tabRootTrans = tabRoot != null ? tabRoot.transform : transform;

        if (null == tabRootTrans)
        {
            return;
        }

        tabRootTrans.GetComponentsInChildren<TabButton>(tabs);
        for (int i = 0; i < tabs.Count; i++)
        {
            tabs[i].Index = i;
            tabs[i].HighLightTab(i == startSelectTab);
        }
        if (startSelectTab != -1)
        {
            curHighLightTab = GetTabButton(startSelectTab);
        }
        m_bEnableClick = true;
    }



    public void OnTabClicked(TabButton curTab)
    {
        if (curTab == null)
        {
            return;
        }

        if (!m_bEnableClick)
        {
            return;
        }

        if (curHighLightTab == curTab)
        {
            return;
        }

        if(null != delTabWillChange)
        {
            if(!delTabWillChange(curTab))
            {
                return;
            }
        }

        DoChangeTab(curTab);
    }

    public GameObject ChangeTab(int index)
    {
        return DoChangeTab(GetTabButton(index));
    }

    public void SetTabDisabled(int index,bool val)
    {
        TabButton button = GetTabButton(index);
        if (button != null)
        {
            button.SetDisabled(val);
        }
    }

    public void SetTabDisabled(string szTabName, bool val)
    {
        TabButton button = GetTabButton(szTabName);
        if (button != null)
        {
            button.SetDisabled(val);
        }
    }

    // 切换标签
    public GameObject ChangeTab(string tabName)
    {
        return DoChangeTab(GetTabButton(tabName));
    }

    // 控制是否接收点击事件
    public void EnableClick(bool bEnable)
    {
        m_bEnableClick = bEnable;
    }

    // 根据名称获取按钮
    public TabButton GetTabButton(string tabName)
    {
        if (null == tabRootTrans)
        {
            return null;
        }

        Transform curTabTrans = tabRootTrans.Find(tabName);
        if (null == curTabTrans)
        {
            LogModule.ErrorLog("can not find tabButton:" + tabName);
            return null;
        }

        return curTabTrans.GetComponent<TabButton>();
    }

    public TabButton GetTabButton(int index)
    {
        if (null == tabRootTrans)
        {
            return null;
        }

        if (index < 0 || index >= tabs.Count)
        {
            LogModule.ErrorLog("can not find tabButton:" + index);
            return null;
        }

        return tabs[index];
    }

    public int GetTabButtonIndex(TabButton tab)
    {
        return  tabs.IndexOf(tab);
    }

    // 获取当前选中TabButton
    public TabButton GetHighlightTab()
    {
        return curHighLightTab;
    }

    GameObject DoChangeTab(TabButton curTab)
    {
        if (curTab == null)
        {
            return null;
        }

        if (null != curHighLightTab)
        {
            if (curHighLightTab.name != curTab.name)
            {
                curHighLightTab.HighLightTab(false);
            }
        }
        curHighLightTab = curTab;
        curTab.HighLightTab(true);

        if (false == curHighLightTab.m_LoadTarget)
        {
            if (null != delTabChanged) delTabChanged(curHighLightTab);
        }

        if (curHighLightTab.GetComponent<TabButton>() == null)
        {
            return null;
        }

        return curHighLightTab.GetComponent<TabButton>().targetObject;
    }

    public void TabButtonLoadBundleOver()
    {
        if (null != delTabChanged) delTabChanged(curHighLightTab);
    }

    public void SortTabGrid()
    {
        if (null != tabRootTrans)
        {
            if (null != tabRootTrans.gameObject.GetComponent<UIGrid>())
            {
                tabRootTrans.gameObject.GetComponent<UIGrid>().Reposition();
            }

            /*if (null != m_grid.gameObject.GetComponent<UITopGrid>())
            {
                m_grid.gameObject.GetComponent<UITopGrid>().Recenter(true);
            }*/
        }
    }

    public void ClearHighLight()
    {
        if (null != curHighLightTab)
        {
            curHighLightTab.HighLightTab(false);
        }
        curHighLightTab = null;
    }
}
