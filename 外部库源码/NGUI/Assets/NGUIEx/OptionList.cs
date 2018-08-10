using UnityEngine;
using System.Collections.Generic;

public class OptionList : MonoBehaviour {

    public Transform m_ItemList;                       //所有选项的父节点
    public UILabel m_CurOptionLabel;                   //当前选项的Label
    public BoxCollider m_OptionBox;
    public int m_DefaultOption;                        //默认选项
    public int m_DefaultTextDic = -1;
    public GameObject m_ItemListPanel;
    public delegate void OnItemChoose(OptionItem item);
    private OnItemChoose m_delOnOptionItemChoose = null;
    public OnItemChoose delOnOptionItemChoose
    {
        get { return m_delOnOptionItemChoose; }
        set { m_delOnOptionItemChoose = value; }
    }
    public delegate void DelOnOptionClick();
    private DelOnOptionClick m_delOnOptionClick = null;
    public DelOnOptionClick delOnOptionClick
    {
        set { m_delOnOptionClick = value;  }
    }
    private OptionItem[] m_AllItem = null;
    public OptionItem m_CurOptionItem = null;


    private bool m_bInited = false;

    private bool m_EnableClick = true;
    public bool EnableClick
    {
        get { return m_EnableClick; }
        set { m_EnableClick = value; }
    }

    private void Awake()
    {
        if (m_ItemListPanel == null && m_ItemList != null)
        {
            m_ItemListPanel = m_ItemList.gameObject;
        }
    }

    void Start()
    {
        Init();

        if (m_DefaultOption >= 0)
        {
            Transform trans = m_ItemList.GetChild(m_DefaultOption);           
            if (trans != null && trans.GetComponent<OptionItem>() != null)
            {
                trans.GetComponent<OptionItem>().Choose();
            }
        }
        else if (m_CurOptionLabel != null && m_DefaultTextDic != -1)
        {
            m_CurOptionLabel.text = StrDictionary.GetDicByID(m_DefaultTextDic);
        }
    }
    void Init()
    {
        if (m_bInited)
        {
            return;
        }

        if (m_AllItem == null)
        {
            m_AllItem = m_ItemList.GetComponentsInChildren<OptionItem>();               //获取5个选项
        }

        m_bInited = true;
    }

    public void SetDefaultOption(string szItemName)
    {
        if (false == m_bInited)       
        {
            Init();
        }

        Transform itemTrans = m_ItemList.Find(szItemName);                    //拿到传进来的选项
        if (itemTrans == null)
        {
            return;
        }

        m_CurOptionItem = itemTrans.GetComponent<OptionItem>();                //设置当前选项
        if (m_CurOptionItem == null)
        {
            return;
        }

        if (m_CurOptionLabel != null)
        {
            m_CurOptionLabel.text = m_CurOptionItem.text;
        }
    }

    public void OnOptionClick()
    {
        if (false == m_EnableClick)
        {
            return;
        }

        if (m_ItemListPanel != null)
        {
            m_ItemListPanel.gameObject.SetActive(!m_ItemListPanel.gameObject.activeSelf);
        }

        if (IsOptionOpen())
        {
            if (m_AllItem != null)
            {
                for (int i = 0; i < m_AllItem.Length; i++)
                {
                    if (m_CurOptionItem != null && m_AllItem[i] == m_CurOptionItem)
                    {
                        m_AllItem[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        m_AllItem[i].gameObject.SetActive(true);
                    }
                }
            }

            UIGrid grid = m_ItemList.GetComponent<UIGrid>();
            if (grid != null)
            {
                grid.Reposition();
            }
        }

        if (m_delOnOptionClick != null)
        {
            m_delOnOptionClick();
        }
    }
    /// <summary>
    /// 当选项被点击时发生作用
    /// </summary>
    /// <param name="item">选中的选项</param>
    public void OnOptionItemClick(OptionItem item)
    {
        if (item == null)
        {
            return;
        }

        m_CurOptionItem = item;                    //修改当前选项
        if (m_CurOptionLabel != null)
        {
            m_CurOptionLabel.text = item.text;
        }
        CloseOption();

        if (m_delOnOptionItemChoose != null)
        {
            m_delOnOptionItemChoose(m_CurOptionItem);
        }
    }
    /// <summary>
    /// 获取当前选项的string
    /// </summary>
    /// <returns>当前选项string</returns>
    public string GetCurOptionName()
    {
        return m_CurOptionItem != null ? m_CurOptionItem.name : "";
    }
    public void ChooseOption(string szItemName)
    {
        if (false == m_bInited)
        {
            Init();
        }

        Transform itemTrans = m_ItemList.Find(szItemName);
        if (itemTrans == null)
        {
            return;
        }

        OptionItem option = itemTrans.GetComponent<OptionItem>();
        if (option != null)
        {
            option.Choose();
        }
    }

    public bool IsOptionOpen()
    {
        if (m_ItemListPanel != null)
        {
            return m_ItemListPanel.gameObject.activeSelf;
        }
        return false;
    }

    public void CloseOption()
    {
        if (m_ItemListPanel != null)
        {
            m_ItemListPanel.gameObject.SetActive(false);
        }
       
    }
}
