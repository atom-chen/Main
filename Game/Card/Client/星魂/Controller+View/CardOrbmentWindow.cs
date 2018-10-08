using Games.Table;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.GlobeDefine;
using Games;
using ProtobufPacket;

public class CardOrbmentWindow : MonoBehaviour
{
    //component
    public OrbmentSlotItem[] m_SlotItem;                        //星魂槽
    public CardOrbmentWindow_AttrItem[] m_AttrItem;              //总属性槽
    public GameObject m_AttrSetDescObj;
    public UIGrid m_AttrGrid;
    public QuartzTipsWindow m_Tips;
    public UIEventTrigger m_TipsMask;

    public UILabel[] m_SetAttr;
    public UITable m_SetAttrGrid;
    public UIScrollView m_AttrSV;

    public UIButton m_OpenOrbmentUIBtn;                    //中间的Btn
    public UISprite m_TutorialOrbmentBtnSprite;   
    //Data
    private const int OrbmentAttrClassCount = 8;
    private Card m_Card;
    private Quartz m_ChooseQuartz;
    private int m_ChooseEmptySlotIndex = 0;
    public int ChooseEmptySlotIndex { get { return m_ChooseEmptySlotIndex; } }
    public static TutorialGroup m_TutorialGroupOnShow = TutorialGroup.Invalid;
    public static int m_TutorialStepOnShow = GlobeVar.INVALID_ID;
    //Instance
    private static CardOrbmentWindow _Ins;
    public static CardOrbmentWindow Instance
    {
        get { return _Ins; }
    }
    void Awake()
    {
        _Ins = this;
    }
    void Start()
    {
        m_OpenOrbmentUIBtn.onClick.Add(new EventDelegate(ShowOrbmentUI));
        m_TipsMask.onClick.Add(new EventDelegate(OnTipsCloseClick));
    }
    void OnDestroy()
    {
        _Ins = null;
    }
    void OnEnable()
    {
        m_ChooseEmptySlotIndex = 0;
        m_ChooseQuartz = null;
        m_Tips.gameObject.SetActive(false);
        if (CardBagController.Instance!=null)
        {
            m_Card = GameManager.PlayerDataPool.PlayerCardBag.GetCard(CardBagController.Instance.CurCard.Guid);
            InitOrbment();
            InitOrbmentAttr();
            InitOrbmentSetAttr();
        }
        else
        {
            this.gameObject.SetActive(false);
        }
        UpdateTutorialOnShow();
    }

    //初始化星盘
    void InitOrbment()
    {
        if(m_Card == null)
        {
             return; 
        }

        if (m_Card == null || m_Card.Orbment == null || m_Card.Orbment.QuartzSlot == null)
        {
            return;
        }
        for (int i = 0; i < m_Card.Orbment.QuartzSlot.Length && i < m_SlotItem.Length; i++)
        {
            if (m_Card.Orbment.QuartzSlot[i] == null || false == m_Card.Orbment.QuartzSlot[i].IsValid())
            {
                m_SlotItem[i].InitEmpty(i + 1);
            }
            else
            {
                m_SlotItem[i].Init(m_Card.Orbment.QuartzSlot[i]);
            }
        }
        OnTipsCloseClick();
    }

    //初始化属性
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

        m_AttrSetDescObj.SetActive(activeSetIdList.Count != 0);
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
        //重置ScrollView
        m_AttrSV.ResetPosition();
    }



    //点击一个槽位
    public void HandleOrbmentSlotItemClick(OrbmentSlotItem slotItem)
    {
        if (slotItem == null)
        {
            return;
        }

        if (m_Card == null)
        {
            return;
        }

        if (slotItem.Quartz != null && slotItem.Quartz.IsValid())
        {
            //如果是反勾选
            if(m_ChooseQuartz!=null && m_ChooseQuartz.Guid==slotItem.Quartz.Guid)
            {
                OnTipsCloseClick();
            }
            else
            {
                // 有星魂 弹tips
                for (int i = 0; i < m_SlotItem.Length; i++)
                {
                    m_SlotItem[i].UpdateChoose(m_SlotItem[i] == slotItem);
                }
                //展示属性
                m_Tips.Show(slotItem.Quartz, QuartzTipsWindow.TipsType.Equiped, m_Card);
                m_ChooseQuartz = slotItem.Quartz;
            }
        }
        else
        {
            for (int i = 0; i < m_SlotItem.Length; i++)
            {
                if(m_SlotItem[i] == slotItem)
                {
                    m_ChooseEmptySlotIndex = i;
                }
            }
            ShowOrbmentUI();         //如果为空，进入星宿UI
        }
    }

    //在星魂背包点击装备：为当前符灵装备星魂
    public void HandleTipsEquipClick(Quartz quartz)
    {
        if (quartz == null)
        {
            return;
        }

        if (m_Card == null)
        {
            return;
        }


        CG_QUARTZ_EQUIP_PAK pak = new CG_QUARTZ_EQUIP_PAK();
        pak.data.QuartzGuid = quartz.Guid;
        pak.data.CardGuid = m_Card.Guid;
        pak.SendPacket();
    }

    //关掉tips
    public void OnTipsCloseClick()
    {
        m_ChooseQuartz = null;
        for (int i = 0; i < m_SlotItem.Length; i++)
        {
            m_SlotItem[i].UpdateChoose(false);
        }
        m_Tips.gameObject.SetActive(false);
    }

    //穿上装备
    public void HandleQuartzEquip(GC_QUARTZ_EQUIP packet)
    {
        if (packet == null || packet.Quartz == null)
        {
            return;
        }

        if (m_Card == null)
        {
            return;
        }

        Tab_Quartz tQuartz = TableManager.GetQuartzByID(packet.Quartz.QuartzId, 0);
        if (tQuartz == null)
        {
            return;
        }

        InitOrbment();
        InitOrbmentAttr();
        InitOrbmentSetAttr();
    }

    //卸下装备
    public void HandleQuartzUnEquip()
    {
        InitOrbment();
        InitOrbmentAttr();
        InitOrbmentSetAttr();
    }

    //强化
    public void HandleQuartzStrengthen()
    {
        InitOrbment();
        InitOrbmentAttr();
        InitOrbmentSetAttr();
    }

    //打开星魂总界面
    void ShowOrbmentUI()
    {
        PlatformHelper.RecordEvent("符灵-星魂界面");
        if(CardBagController.Instance!=null)
        {
            CardBagController.Instance.OnOrbmentClick();
        }
    }

    void UpdateTutorialOnShow()
    {
        if (m_TutorialGroupOnShow == TutorialGroup.Invalid && m_TutorialStepOnShow == GlobeVar.INVALID_ID)
        {
            return;
        }

        if (m_TutorialGroupOnShow == TutorialGroup.QuartzEquip && m_TutorialStepOnShow == 9)
        {
            TutorialRoot.ShowTutorial(TutorialGroup.QuartzEquip, 9, m_TutorialOrbmentBtnSprite.gameObject, m_TutorialOrbmentBtnSprite.width, m_TutorialOrbmentBtnSprite.height);
        }
        else if (m_TutorialGroupOnShow == TutorialGroup.QuartzSuit && m_TutorialStepOnShow == 4)
        {
            TutorialRoot.ShowTutorial(TutorialGroup.QuartzSuit, 4, m_TutorialOrbmentBtnSprite.gameObject, m_TutorialOrbmentBtnSprite.width, m_TutorialOrbmentBtnSprite.height);
        }

        m_TutorialGroupOnShow = TutorialGroup.Invalid;
        m_TutorialStepOnShow = GlobeVar.INVALID_ID;
    }
}
