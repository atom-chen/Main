using System.Collections;
using System.Collections.Generic;
using Games;
using Games.GlobeDefine;
using Games.Table;
using ProtobufPacket;
using UnityEngine;

//星盘界面
public class QuartzEquipWindow : MonoBehaviour
{
    private static QuartzEquipWindow m_Instance = null;
    public static QuartzEquipWindow Instance
    {
        get { return m_Instance; }
    }
    //-----------------component-----------------------------------------------
    public ListWrapController m_CardWrap;
    public GameObject m_CardListObj;
    public UIEventTrigger m_IconTrigger;
    public UIEventTrigger m_CardListMask;
    public UISprite m_CardIcon;
    public UILabel m_LevelLabel;
    public GameObject[] m_StarSprite;                      //这个符灵几星
    public OrbmentSlotItem[] m_QuartzSlotItem;            //星魂槽
    public OrbmentAttrItem[] m_OrbmentAttrItem;          //星盘提供的总属性
    public UILabel[] m_SetAttr;                    //当前激活的套装效果
    public UITable m_SetAttrGrid;
    public GameObject[] m_AttrObject;                    //0 星盘总属性的父节点  1 星盘套装效果的父节点 

    public UIScrollView m_AttrScroll;
    public UITable m_AttrTable;


    public UIWidget m_TutorialSlotBG;
    public QuartzSetRecommendWindow m_SetRecommendWindow;
    public GameObject m_RecommendBtn;
    public GameObject m_AttrSetDescObj;
    //----------------Data------------------------------------
    private Card m_Card = null;                           //当前选中的Card
    private Card m_PreviewCard = new Card();             //上一张Card
    private List<Card> m_CardList = null;                //当前可以左右滑动的CardList
    private int m_ChooseIndex = GlobeVar.INVALID_ID;     //当前选中的符灵所在list索引
    private List<int> m_OldSetIdList = new List<int>();

    private const int OrbmentAttrClassCount = 8;

    private OrbmentSlotItem m_ChooseSlotItem = null;
    public OrbmentSlotItem ChooseSlotItem
    {
        get { return m_ChooseSlotItem; }
    }

    void Start()
    {
        m_IconTrigger.onClick.Add(new EventDelegate(OnCardClick));
        m_CardListMask.onClick.Add(new EventDelegate(OnCardListCloseClick));
    }

    void OnEnable()
    {
        m_Instance = this;
        
        if (OrbmentController.Instance != null && OrbmentController.Instance.Card != null)
        {
            m_Card = OrbmentController.Instance.Card;//GameManager.PlayerDataPool.PlayerCardBag.GetCard(OrbmentController.Instance.CardGuid);
            m_CardList = OrbmentController.Instance.CardList;
        }

        if (m_Card == null || m_CardList == null || m_Card.GetClassId()!=(int)CardClass.Normal)
        {
            return;
        }

        for (int i = 0; i < m_CardList.Count; i++)
        {
            if (m_CardList[i] == null || false == m_CardList[i].IsValid())
            {
                continue;
            }

            if (m_CardList[i].Guid == m_Card.Guid)
            {
                m_ChooseIndex = i;
                break;
            }
        }
        InitCardInfo();
        InitOrbment();
        InitOrbmentAttr(m_Card);
        InitOrbmentSetAttr();
        InitRecommendBtnState();
        m_AttrTable.Reposition();
        m_AttrScroll.ResetPosition();
    }

    void OnDisable()
    {
        m_Instance = null;

        m_Card = null;
        m_PreviewCard.CleanUp();
        m_CardList = null;
        m_ChooseIndex = GlobeVar.INVALID_ID;
        m_OldSetIdList.Clear();
        m_ChooseSlotItem = null;
    }

    //初始化卡牌信息
    void InitCardInfo()
    {
        if (m_Card == null)
        {
            return;
        }

        Tab_RoleBaseAttr tRoleBase = TableManager.GetRoleBaseAttrByID(m_Card.GetRoleBaseId(), 0);
        if (tRoleBase == null)
        {
            return;
        }

        Tab_CharModel tCharModel = TableManager.GetCharModelByID(tRoleBase.CharModelID, 0);
        if (tCharModel == null)
        {
            return;
        }

        m_CardIcon.spriteName = tCharModel.HeadPic;
        m_LevelLabel.text = m_Card.Level.ToString();

        for (int i = 0; i < m_StarSprite.Length; ++i)
        {
            m_StarSprite[i].SetActive(i < m_Card.Star);
        }
        OnCardListCloseClick();
    }

    void InitOrbment()
    {
        if (m_Card == null || m_Card.Orbment == null || m_Card.Orbment.QuartzSlot == null)
        {
            return;
        }

        for (int i = 0; i < m_Card.Orbment.QuartzSlot.Length && i < m_QuartzSlotItem.Length; i++)
        {
            if (m_Card.Orbment.QuartzSlot[i] == null || false == m_Card.Orbment.QuartzSlot[i].IsValid())
            {
                m_QuartzSlotItem[i].InitEmpty(i + 1);
            }
            else
            {
                m_QuartzSlotItem[i].Init(m_Card.Orbment.QuartzSlot[i]);
            }
        }
    }

    void InitOrbmentAttr(Card card, bool bShowUp = false)
    {
        if (card == null || card.Orbment == null)
        {
            return;
        }

        for (int i = 0; i < m_OrbmentAttrItem.Length; i++)
        {
            m_OrbmentAttrItem[i].gameObject.SetActive(false);
        }

        if (m_OrbmentAttrItem.Length < OrbmentAttrClassCount)
        {
            return;
        }

        m_OrbmentAttrItem[0].Init(card, AttrType.Attack, bShowUp);
        m_OrbmentAttrItem[1].Init(card, AttrType.Defense, bShowUp);
        m_OrbmentAttrItem[2].Init(card, AttrType.MaxHP, bShowUp);
        m_OrbmentAttrItem[3].Init(card, AttrType.Speed, bShowUp);
        m_OrbmentAttrItem[4].Init(card, AttrType.CritChance, bShowUp);
        m_OrbmentAttrItem[5].Init(card, AttrType.CritEffect, bShowUp);
        m_OrbmentAttrItem[6].Init(card, AttrType.ImpactChance, bShowUp);
        m_OrbmentAttrItem[7].Init(card, AttrType.ImpactResist, bShowUp);
    }

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
    }

    private void InitRecommendBtnState()
    {
        if (m_Card == null)
        {
            return;
        }
        var tCard = TableManager.GetCardByID(m_Card.CardId, 0);
        if (tCard == null)
        {
            return;
        }
        bool bShow = false;

        if (tCard.RecommendSet != GlobeVar.INVALID_ID)
        {
            Tab_CardSetRecommend tRecommend = TableManager.GetCardSetRecommendByID(tCard.RecommendSet, 0);
            if(tRecommend!=null)
            {
                for (int i = 0; i < tRecommend.getRecommendSetCount();i++)
                {
                    if(tRecommend.GetRecommendSetbyIndex(i)!=GlobeVar.INVALID_ID)
                    {
                        bShow = true;
                        break;
                    }
                }
            }
        }

        m_RecommendBtn.SetActive(bShow);
        m_SetRecommendWindow.gameObject.SetActive(false);
    }

    //点击星魂Item
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

        if (m_ChooseSlotItem != slotItem)
        {
            //选中
            m_ChooseSlotItem = slotItem;
            for (int i = 0; i < m_QuartzSlotItem.Length; i++)
            {
                m_QuartzSlotItem[i].UpdateChoose(m_QuartzSlotItem[i] == slotItem);
            }
        }
        else
        {
            // 取消选中
            m_ChooseSlotItem = null;
            for (int i = 0; i < m_QuartzSlotItem.Length; i++)
            {
                m_QuartzSlotItem[i].UpdateChoose(false);
            }
        }

        if (OrbmentController.Instance != null)
        {
            OrbmentController.Instance.HandleOrbmentSlotItemClick();
        }
    }

    public void CancelChooseSlotOnTabChanged()
    {
        m_ChooseSlotItem = null;
        for (int i = 0; i < m_QuartzSlotItem.Length; i++)
        {
            m_QuartzSlotItem[i].UpdateChoose(false);
        }
    }

    public int GetChooseSlotType()
    {
        return m_ChooseSlotItem != null ? m_ChooseSlotItem.SlotType : GlobeVar.INVALID_ID;
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

        m_OldSetIdList = m_Card.Orbment.GetQuartzSetId();

        CG_QUARTZ_EQUIP_PAK pak = new CG_QUARTZ_EQUIP_PAK();
        pak.data.QuartzGuid = quartz.Guid;
        pak.data.CardGuid = m_Card.Guid;
        pak.SendPacket();
    }

    //反勾选
    public void HandleTipsCloseClick()
    {
        // 取消选中
        //如果出现了反勾选行为，则通知星魂背包重置
        if (OrbmentController.Instance != null)
        {
            bool bIsCancelChoose = m_ChooseSlotItem != null;
            m_ChooseSlotItem = null;
            OrbmentController.Instance.HandleChooseSlotChange(bIsCancelChoose);
            for (int i = 0; i < m_QuartzSlotItem.Length; i++)
            {
                m_QuartzSlotItem[i].UpdateChoose(false);
            }

            if (m_PreviewCard != null && m_PreviewCard.IsValid())
            {
                m_PreviewCard.CleanUp();
                InitOrbmentAttr(m_Card);
            }
        }
    }

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
        InitOrbmentAttr(m_Card);
        InitOrbmentSetAttr();

        List<int> newSetIdList = m_Card.Orbment.GetQuartzSetId();
        for (int i = 0; i < newSetIdList.Count; i++)
        {
            if (false == m_OldSetIdList.Contains(newSetIdList[i]))
            {
                int nSetId = newSetIdList[i];
                Tab_QuartzSet tSet = TableManager.GetQuartzSetByID(nSetId, 0);
                if (tSet == null)
                {
                    continue;
                }

                for (int j = 0; j < m_QuartzSlotItem.Length; j++)
                {
                    if (m_QuartzSlotItem[j].IsQuartzClassId(tSet.NeedClassId))
                    {
                        m_QuartzSlotItem[j].PlayQuartzSetEffect();
                    }
                }
            }
        }

        m_ChooseSlotItem = null;
        m_PreviewCard.CleanUp();
    }

    public void HandleQuartzUnEquip()
    {
        InitOrbment();
        InitOrbmentAttr(m_Card, true);
        InitOrbmentSetAttr();
        StartCoroutine(OnUnEquip_Delta(1.0f));
    }
   IEnumerator OnUnEquip_Delta(float delta)
    {
       yield return new WaitForSeconds(delta);
       foreach(OrbmentAttrItem item in m_OrbmentAttrItem)
       {
           item.CloseUpIcon();
       }
    }
    public void HandleQuartzStrengthen()
    {
        InitOrbment();
        InitOrbmentAttr(m_Card);
        InitOrbmentSetAttr();
    }

    //点击卡牌头像，弹出可选择的Card wrap
    private void OnCardClick()
    {
        PlatformHelper.RecordEvent("符灵-星宿-卡牌头像");
        m_CardListObj.SetActive(true);
        m_CardWrap.InitList(m_CardList.Count - 1, OnCardWrapSlide, false );
        //关闭tips
        if (OrbmentController.Instance != null)
        {
            OrbmentController.Instance.HandleCardClick();
        }
    }

    private void OnCardWrapSlide(GameObject obj,int index)
    {
        if(obj == null || index <0 || index>=m_CardList.Count)
        {
            return;
        }
        //取出Index 显示
        CardBagItemLogic item = obj.GetComponent<CardBagItemLogic>();
        if(item == null)
        {
            return;
        }
        //拿到真实index
        index = index < m_ChooseIndex ? index : index + 1;
        if (index > m_CardList.Count || m_CardList[index] == null || false == m_CardList[index].IsValid())
        {
            return;
        }
        item.InitNormal(m_CardList[index],false);
    }

    //选择一张卡
    public void HandleChooseCardItem(CardBagItemLogic nCardItemLogic)
    {
        //找到这张卡的信息
        for (int i = 0; i < m_CardList.Count;i++ )
        {
            if(m_CardList[i].Guid == nCardItemLogic.GetGuid())
            {
                Card nCard = GameManager.PlayerDataPool.PlayerCardBag.GetCard(m_CardList[i].Guid);
                if(nCard != null)
                {
                    m_Card = nCard;
                    m_ChooseIndex = i;
                    break;
                }
            }
        }

        InitCardInfo();
        InitOrbment();
        InitOrbmentAttr(m_Card);
        InitOrbmentSetAttr();
        InitRecommendBtnState();
        if (OrbmentController.Instance != null && m_Card != null)
        {
            OrbmentController.Instance.Card = m_Card;
        }
    }

    private void OnCardListCloseClick()
    {
        m_CardListObj.SetActive(false);
    }

    public void OnSetRecommendClick()
    {
        PlatformHelper.RecordEvent("符灵-星宿-推荐");
        if (m_Card == null)
        {
            return;
        }
        //关闭tips
        if (OrbmentController.Instance != null)
        {
            OrbmentController.Instance.HandleSetRecommendClick();
        }
        m_SetRecommendWindow.Show(m_Card.CardId);
    }

    public void HandleQuartzItemClick(Quartz quartz)
    {
        if (quartz == null)
        {
            return;
        }

        int nSlotIndex = quartz.GetSlotType() - 1;

        m_PreviewCard.CopyFrom(m_Card);
        if (nSlotIndex < 0 || nSlotIndex >= m_PreviewCard.Orbment.QuartzSlot.Length)
        {
            return;
        }

        if (m_PreviewCard.Orbment.QuartzSlot[nSlotIndex] == null)
        {
            return;
        }

        m_PreviewCard.Orbment.QuartzSlot[nSlotIndex].CopyFrom(quartz);
        InitOrbmentAttr(m_PreviewCard, true);
    }

    public void HandleQuartzItemCancel()
    {
        m_PreviewCard.CleanUp();
        InitOrbmentAttr(m_Card);
    }

    public void UpdateTutorialOnEquipBtnClick(TutorialGroup group, int nStep)
    {
        if (group == TutorialGroup.QuartzEquip && nStep == 15)
        {
            TutorialRoot.ShowTutorial(TutorialGroup.QuartzEquip, 15, m_CardIcon.gameObject, m_CardIcon.width, m_CardIcon.height);
        }
    }

    public void UpdateTutorialOnTutorialMaskClick()
    {
        if (TutorialRoot.IsGroupStep(TutorialGroup.QuartzEquip, 15))
        {
            TutorialRoot.TutorialOver();
        }
    }
}
