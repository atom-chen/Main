using Games;
using Games.GlobeDefine;
using Games.Table;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuartzDynamicWindow : MonoBehaviour
{
    private static QuartzDynamicWindow m_Instance;

    public static QuartzDynamicWindow Instance
    {
        get
        {
            return m_Instance;
        }
    }
    public OrbmentSlotItem[] m_QuartzSlotItem;
    public UILabel[] m_SetAttr;
    public CardOrbmentWindow_AttrItem[] m_AttrItem;              //总属性槽
    public UITable m_SetAttrGrid;
    public UIGrid m_AttrGrid;
    public UITable m_AttrListTable;
    public QuartzTipsWindow m_QuartzTips;

    private const int OrbmentAttrClassCount = 8;
    private Card m_Card = null;
    private Quartz m_ChooseQuartz = null;
    private void OnEnable()
    {
        if (CardDynamicInfoWindow.Instance == null || CardDynamicInfoWindow.Instance.CurCard == null)
        {
            return;
        }
        m_Card = CardDynamicInfoWindow.Instance.CurCard;
        if (m_Card == null)
        {
            return;
        }
        m_Instance = this;
        InitOrbment();
        InitOrbmentAttr();
        InitOrbmentSetAttr();
        m_AttrListTable.Reposition();
        OnTipsCloseClick();
        m_ChooseQuartz = null;
    }

    //初始化星盘
    private void InitOrbment()
    {
        if (m_Card == null || m_Card.Orbment == null|| m_Card.Orbment.QuartzSlot==null)
        {
            return;
        }

        for (int i = 0; i < m_Card.Orbment.QuartzSlot.Length && i < m_QuartzSlotItem.Length; i++)
        {
            if (m_Card.Orbment.QuartzSlot[i] == null || !m_Card.Orbment.QuartzSlot[i].IsValid())
            {
                m_QuartzSlotItem[i].InitEmpty(i + 1);
            }
            else
            {
                m_QuartzSlotItem[i].Init(m_Card.Orbment.QuartzSlot[i]);
            }
        }
    }

    void InitOrbmentAttr()
    {
        if (m_Card == null || m_Card.Orbment == null)
        {
            return;
        }

        for (int i = 0; i < m_AttrItem.Length; i++)
        {
            m_AttrItem[i].gameObject.SetActive(false);
        }

        if (m_AttrItem.Length < OrbmentAttrClassCount)
        {
            return;
        }

        m_AttrItem[0].Init(m_Card, AttrType.Attack);
        m_AttrItem[1].Init(m_Card, AttrType.Defense);
        m_AttrItem[2].Init(m_Card, AttrType.MaxHP);
        m_AttrItem[3].Init(m_Card, AttrType.Speed);
        m_AttrItem[4].Init(m_Card, AttrType.CritChance);
        m_AttrItem[5].Init(m_Card, AttrType.CritEffect);
        m_AttrItem[6].Init(m_Card, AttrType.ImpactChance);
        m_AttrItem[7].Init(m_Card, AttrType.ImpactResist);

        m_AttrGrid.Reposition();
    }

    //初始化套装属性
    void InitOrbmentSetAttr()
    {
        if (m_Card == null || m_Card.Orbment == null)
        {
            return;
        }

        for (int i = 0; i < m_SetAttr.Length; i++)
        {
            m_SetAttr[i].gameObject.SetActive(false);
        }

        List<int> activeSetIdList = m_Card.Orbment.GetQuartzSetId();

        for (int i = 0; i < activeSetIdList.Count && i < m_SetAttr.Length; i++)
        {
            m_SetAttr[i].gameObject.SetActive(true);
            Tab_QuartzSet tSet = TableManager.GetQuartzSetByID(activeSetIdList[i], 0);
            if (tSet != null)
            {
                m_SetAttr[i].text = QuartzTool.GetFormatSetAttr(tSet);      //往套装属性里写入格式化字符串
            }
        }
        m_SetAttrGrid.Reposition();
    }

    //星盘被点击 弹出Tips
    public void HandleOrbmentSlotItemClick(OrbmentSlotItem slotItem)
    {
        if (slotItem == null || slotItem.Quartz == null || !slotItem.Quartz.IsValid())
        {
            return;
        }
        //点了一个相同的 则取消勾选
        if(m_ChooseQuartz!=null && m_ChooseQuartz.Guid == slotItem.Quartz.Guid)
        {
            OnTipsCloseClick();
        }
        else
        {
            m_ChooseQuartz = slotItem.Quartz;
            // 有星魂 弹tips
            for (int i = 0; i < m_QuartzSlotItem.Length; i++)
            {
                m_QuartzSlotItem[i].UpdateChoose(m_QuartzSlotItem[i] == slotItem);
            }
            //展示属性
            m_QuartzTips.Show(slotItem.Quartz, QuartzTipsWindow.TipsType.Info, m_Card);
        }

    }

    //关掉tips
    public void OnTipsCloseClick()
    {
        m_ChooseQuartz = null;
        for (int i = 0; i < m_QuartzSlotItem.Length; i++)
        {
            m_QuartzSlotItem[i].UpdateChoose(false);
        }
        m_QuartzTips.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        m_Instance = null;
        m_Card = null;
        m_ChooseQuartz = null;
    }
}
