using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Games;
using Games.GlobeDefine;
using Games.Item;
using Games.Table;
using ProtobufPacket;

public class OrbmentController : MonoBehaviour
{

    private static OrbmentController m_Instance = null;

    public static OrbmentController Instance
    {
        get { return m_Instance; }
    }

    public struct QuartzClass
    {
        public QuartzClass(int nClassId, int nCount)
        {
            m_ClassId = nClassId;
            m_Count = nCount;
        }

        public int m_ClassId;
        public int m_Count;
    }

    public enum SHOW_TYPE
    {
        POS,
        SET,
        SELL
    }
    //-----------------------------Component---------------------
    //排序/筛选项 及其缓存变量
    public TabController m_QuartzOpTab;                 //观星/焚星 （观星界面）
    public TabController m_QuartzSellTab;
    private bool m_ShowSellList = false;                //是否打开焚星界面

    public ToggleActive m_QuartzPosClassToggle;                   //位置/类别
    public ToggleActive m_DownUpSortToggle;               //降序/升序

    public OptionList m_QuartzSortOption;                //排序规则

    public TabController m_QuartzSlotTypeTab;            //1/2/3/4/5/6

    public OptionList m_QuartzStarFilterTab;              //白色-神品

    public UIGrid m_OptionGrid;                          //选项Grid

    public GameObject m_QuartzSetListWindow;
    public ListWrapController m_QuartzSetListWrap;      //星宿class滑动列表
    public UIScrollView m_QuartzSetListScroll;
    public GameObject m_QuartzListWindow;
    public GameObject m_QuartzListGrid;
    public UIPanel m_QuartzListPanel;
    public UIScrollView m_QuartzListScroll;              //星魂背包ScrollView
    public ListWrapController m_QuartzListWrap;         //星魂背包滑动列表

    public GameObject m_QuartzSlotTypeObject;
    public QuartzTipsController m_QuartzTips;           //星魂信息展示
    public QuartzTipsWindow m_QuartzTips_Compared;
    public UILabel m_CoinLabel;
    public UILabel m_StrengthenStoneLabel;
    public UIWidget m_QuartzListWidget;
    public GameObject m_NoQuartzObject_Class;
    public GameObject m_NoQuartzObject_Pos;
    public UILabel m_QuartzTabCountLabel;            //当前tab分页筛选出的星魂数量 
    public GameObject m_QuartzSellObj;
    public UILabel m_QuartzWholeCountLabel;         //总数量
    public GameObject m_OptionSortObj;
    public GameObject m_PlayerResourcesObj;           //金币 星尘面板父物体

    public UISprite m_BGSprite;

    public GameObject[] m_hideGoList;   //隐藏跳转的gameobject列表

    //pos

    //set

    //class
    public GameObject m_ReturnQuartzClass;
    public UILabel m_QuartzClassNameLabel;
    public UISprite m_ClassIcon;
    public UISprite m_TutorialClassTab;
    public UISprite m_TutorialFifthPosTab;
    //-----------------------------------Data--------------------------------
    private Card m_Card;
    public Card Card
    {
        get { return m_Card; }
        set { m_Card = value; }
    }

    private List<Card> m_CardList = null;
    public List<Card> CardList
    {
        get { return m_CardList; }
    }


    //------------------------------------------Quartz----------------------------------------------
    private List<Quartz> m_QuartzList = new List<Quartz>();                   //当前星魂集合
    private ulong m_ChooseQuartzGuid = GlobeVar.INVALID_GUID;    //当前选中星魂的GUID
    private int m_DefaultSlotIndex = -1;

    //---------------------------------------------class-----------------------------------------------------
    public GameObject m_QuartzClassTitle;
    private List<QuartzClass> m_QuartzClassList = new List<QuartzClass>();    //当前星魂class集合

    private int m_ChooseClassId = GlobeVar.INVALID_ID;               //当前选择的星魂classID


    //-----------------------------------------------焚星---------------------------------------------
    private bool m_ShowQuartzBag = false;                           //是否打开星魂背包
    private bool m_IsChooseAll = false;                            // 当前是否全选
    private List<ulong> m_SellGuidList = new List<ulong>();        //出售的星魂guid序列
    private List<int> m_SlotTypeFilterList = new List<int>();
    public List<ulong> SellGuidList
    {
        get { return m_SellGuidList; }
    }

    //出售 当前出售的品质
    public int SellStar
    {
        get
        {
            if (m_QuartzSellTab.GetHighlightTab() == null)
                return -1;
            int nStar = -1;
            if (int.TryParse(m_QuartzSellTab.GetHighlightTab().name, out nStar))
                return nStar;
            return -1;
        }
    }

    private GameObject m_FirstClassItem = null;
    private bool m_bInited = false;
    private bool m_bClassInited = false;
    private bool m_bSetInited = false;
    private bool m_bSellInited = false;

    private static Vector3 QuartzListWindowPos_SlotType = new Vector3(-145, -155, 0);
    private static Vector4 QuartzListPanelRange_SlotType = new Vector4(12, -305, 425, 400);

    private static Vector3 QuartzListWindowPos_Class = new Vector3(-142, -55, 0);
    private static Vector4 QuartzListPanelRange_Class = new Vector4(8, -224, 440, 440);

    private static Vector3 QuartzListWindowPos_Sell = new Vector3(-145, -119, 0);
    private static Vector4 QuartzListPanelRange_Sell = new Vector4(8, -276, 440, 440);

    private int m_QuartzItemObjCount;

    private static int BG_Small_Height = 520;
    private static int BG_Big_Height = 570;
    ////星魂class UI的宽高
    private const int ClassItemWidth = 210;
    private const int ClassItemHeight = 260;

    //新手指引类型
    public static TutorialGroup m_TutorialGroupOnShow = TutorialGroup.Invalid;
    public static int m_TutorialStepOnShow = GlobeVar.INVALID_ID;
    private QuartzItem m_FirstQuartzItemLogic;

    void Awake()
    {
        m_Instance = this;

        m_QuartzOpTab.delTabChanged = OnQuartzOpTabChanged;                         //观星界面：观星/焚星
        m_QuartzPosClassToggle.onToggle = OnQuartzPosClassToggle;                   //位置/类别
        m_DownUpSortToggle.onToggle = OnDownUpSortToggle;
        m_QuartzSortOption.delOnOptionItemChoose = OnQuartzSortOptionChoose;        //排序规则
        m_QuartzSortOption.delOnOptionClick = OnClickSortOpList ;
        m_QuartzStarFilterTab.delOnOptionClick = OnClickStarOpList;
        m_QuartzStarFilterTab.delOnOptionItemChoose = OnQuartzStarOptionChoose;
        m_QuartzSlotTypeTab.delTabChanged = OnQuartzSlotTypeTabChanged;             //槽位筛选项 1/2/3/4/5/6
        m_QuartzSellTab.delTabChanged = OnQuartzSellTypeTabChanged;                 //焚星界面切换Tab后调

        PlayerData.delegateGoldCoinChanged += UpdateCoinLabel;
        PlayerData.delegateCommonPackItemChanged += UpdateStrengthenStoneLabel;
    }

    void OnDestroy()
    {
        m_Instance = null;
        m_CardList = null;
        m_QuartzClassList.Clear();
        m_QuartzList.Clear();
        m_SlotTypeFilterList.Clear();
        m_ChooseClassId = GlobeVar.INVALID_ID;
        m_ShowQuartzBag = false;
        m_ShowSellList = false;
        m_SellGuidList.Clear();
        m_ChooseQuartzGuid = GlobeVar.INVALID_GUID;
        m_FirstClassItem = null;
        m_DefaultSlotIndex = -1;
        m_bInited = false;
        m_bClassInited = false;
        m_bSetInited = false;
        m_bSellInited = false;
        PlayerData.delegateGoldCoinChanged -= UpdateCoinLabel;
        PlayerData.delegateCommonPackItemChanged -= UpdateStrengthenStoneLabel;
    }

    void OnEnable()
    {
        UIManager.SetMainCameraStatesOnUIChange(MAINCAMERA_HIDE_UI.STAR, true);
        m_QuartzPosClassToggle.gameObject.SetActive(false);
        if (ConvenientNoticeController.Instance != null)
        {
            ConvenientNoticeController.Instance.gameObject.SetActive(false);
        }
        m_QuartzItemObjCount = m_QuartzListWrap.transform.childCount;

        RefreshUIForHideGo();
    }

    void OnDisable()
    {
        UIManager.SetMainCameraStatesOnUIChange(MAINCAMERA_HIDE_UI.STAR, false);
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("XingXiuGuanXingTime", MsdkController.GetReportTime(reportGuanxingTime));
        dic.Add("XingXiuFengXingTime", MsdkController.GetReportTime(reportFenxingTime));
        MsdkController.WGReportEvent("XingXiu", dic, true);//数据上报
        if (ConvenientNoticeController.Instance != null)
        {
            ConvenientNoticeController.Instance.gameObject.SetActive(true);
        }
        m_FirstQuartzItemLogic = null;
    }

    //入口
    public static void ShowOrbment(Card card, List<Card> cardList)
    {
        //过滤
        List<Card> paraCard = new List<Card>();
        foreach (Card temp in cardList)
        {
            if (temp.GetClassId() == (int)CardClass.Normal)
            {
                paraCard.Add(temp);
            }
        }
        List<object> Params = new List<object>() { card, paraCard, 0 };
        UIManager.ShowUI(UIInfo.OrbmentRoot, OnOpenOrbmentRoot, Params, UIStack.StackType.PushAndPop);
    }

    public static void ShowOrbment(Card card, List<Card> cardList, int defaultSlotIndex)
    {
        //过滤
        List<Card> paraCard = new List<Card>();
        foreach (Card temp in cardList)
        {
            if (temp.GetClassId() == (int)CardClass.Normal)
            {
                paraCard.Add(temp);
            }
        }
        List<object> Params = new List<object>() { card, paraCard, defaultSlotIndex };
        UIManager.ShowUI(UIInfo.OrbmentRoot, OnOpenOrbmentRoot, Params, UIStack.StackType.PushAndPop);
    }


    //打开该界面后调用
    static void OnOpenOrbmentRoot(bool bSuccess, object param)
    {
        if (bSuccess == false || m_Instance == null)
        {
            return;
        }

        List<object> Params = param as List<object>;
        if (Params == null || Params.Count != 3)
        {
            return;
        }

        m_Instance.m_Card = Params[0] as Card;
        if (m_Instance.m_Card == null)
        {
            return;
        }

        m_Instance.m_CardList = Params[1] as List<Card>;

        if (!int.TryParse(Params[2].ToString(), out m_Instance.m_DefaultSlotIndex))
        {
            m_Instance.m_DefaultSlotIndex = -1;
        }
        //初始化数量信息
        m_Instance.UpdateStrengthenStoneLabel();
        m_Instance.UpdateCoinLabel();
        m_Instance.InitQuartzWholeCount();
        m_Instance.StartCoroutine(m_Instance.DelayOnOpen());
    }

    IEnumerator DelayOnOpen()
    {
        yield return new WaitForEndOfFrame();

        m_ShowSellList = false;
        m_QuartzOpTab.ChangeTab("Tab01-Star");                        //默认选择 观星

        yield return null;
        UpdateTutorialOnShow();       
    }

    int GetChooseStar()
    {
        int nStar = -1;
        if (false == int.TryParse(m_QuartzStarFilterTab.GetCurOptionName(), out nStar))
        {
            return -1;
        }

        return nStar;
    }

    bool m_bNeedCancelChooseSlot = true;

    //--------------------------------------------------显示状态切换------------------------------
    //观星界面的Tab点击后调用
    void OnQuartzOpTabChanged(TabButton tab)
    {
        if (tab == null)
        {
            return;
        }

        m_ChooseClassId = GlobeVar.INVALID_ID;
        m_ChooseQuartzGuid = GlobeVar.INVALID_GUID;
        m_NoQuartzObject_Class.SetActive(false);
        m_SellGuidList.Clear();
        //观星
        if (tab.name == "Tab01-Star")
        {
            PlatformHelper.RecordEvent("符灵-星宿-观星");

            m_QuartzSortOption.SetDefaultOption("Option1-Star");
            m_QuartzStarFilterTab.SetDefaultOption("-1");
            UpdateOptionGrid();
            //显示观星界面的筛选项
            m_QuartzPosClassToggle.gameObject.SetActive(true);
            m_QuartzPosClassToggle.Refresh(1);           //默认选择 pos

            m_QuartzSellObj.SetActive(false);
        }
        //焚星
        else if (tab.name == "Tab02-Sell")
        {
            PlatformHelper.RecordEvent("符灵-星宿-焚星");

            //隐藏观星界面的筛选项
            m_OptionSortObj.SetActive(false);
            m_QuartzClassTitle.SetActive(false);
            m_QuartzListWindow.SetActive(true);
            m_QuartzSetListWindow.SetActive(false);
            m_QuartzSlotTypeObject.SetActive(false);
            m_ReturnQuartzClass.SetActive(false);
            m_QuartzListGrid.transform.localPosition = QuartzListWindowPos_Sell;
            m_QuartzListPanel.baseClipRegion = QuartzListPanelRange_Sell;
            m_BGSprite.height = BG_Big_Height;            

            m_QuartzPosClassToggle.gameObject.SetActive(false);

            m_QuartzSellObj.SetActive(true);
            m_QuartzSellTab.ChangeTab("1");

            m_IsChooseAll = false;
        }

        CloseOpList();

        if (m_QuartzTips.IsShow())
        {
            m_QuartzTips.OnCloseClick();
        }
    }

    //位置/类别切换后调用
    void OnQuartzPosClassToggle(int index)
    {
        m_ChooseClassId = GlobeVar.INVALID_ID;
        m_ChooseQuartzGuid = GlobeVar.INVALID_GUID;

        //按类别
        if (index == 0)
        {
            m_OptionSortObj.SetActive(false);
            m_QuartzClassTitle.SetActive(true);
            m_QuartzListWindow.SetActive(false);
            m_QuartzSetListWindow.SetActive(true);
            m_QuartzSlotTypeObject.SetActive(false);

            m_NoQuartzObject_Class.SetActive(false);
            m_NoQuartzObject_Pos.SetActive(false);

            m_QuartzListGrid.transform.localPosition = QuartzListWindowPos_Class;
            m_QuartzListPanel.baseClipRegion = QuartzListPanelRange_Class;
            m_BGSprite.height = BG_Big_Height;

            InitQuartzSetList(true);         
        }
        //按位置分类
        else if (index == 1)
        {
            m_OptionSortObj.SetActive(true);
            m_QuartzClassTitle.SetActive(false);
            m_QuartzListWindow.SetActive(true);
            m_QuartzSetListWindow.SetActive(false);
            m_QuartzSlotTypeObject.SetActive(true);
            m_NoQuartzObject_Class.SetActive(false);
            m_QuartzListGrid.transform.localPosition = QuartzListWindowPos_SlotType;
            m_QuartzListPanel.baseClipRegion = QuartzListPanelRange_SlotType;
            m_BGSprite.height = BG_Small_Height;

            m_QuartzSortOption.SetDefaultOption("Option1-Star");
            m_QuartzStarFilterTab.SetDefaultOption("-1");
            UpdateOptionGrid();

            if (m_DefaultSlotIndex == -1)
            {
                int nChooseSlotType = QuartzEquipWindow.Instance != null
                    ? QuartzEquipWindow.Instance.GetChooseSlotType()
                    : GlobeVar.INVALID_ID;
                if (nChooseSlotType == GlobeVar.INVALID_ID)
                {
                    m_QuartzSlotTypeTab.ChangeTab("1");
                }
                else
                {
                    m_bNeedCancelChooseSlot = false;
                    m_QuartzSlotTypeTab.ChangeTab(nChooseSlotType.ToString());
                }
            }
            else
            {
                m_QuartzSlotTypeTab.ChangeTab((m_DefaultSlotIndex + 1).ToString());
                m_DefaultSlotIndex = -1;
            }
        }

        m_ReturnQuartzClass.SetActive(false);

        if (m_QuartzSortOption.IsOptionOpen())
        {
            m_QuartzSortOption.CloseOption();
        }
        if (m_QuartzStarFilterTab.IsOptionOpen())
        {
            m_QuartzStarFilterTab.CloseOption();
        }

        if (m_QuartzTips.IsShow())
        {
            m_QuartzTips.DirectCloseTips();
        }
    }

    //点击某个星魂Class -> 进入某class星魂UI
    public void HandleQuartzClassItemClick(int nClassId)
    {
        Tab_QuartzClass tClass = TableManager.GetQuartzClassByID(nClassId, 0);
        if (tClass == null)
        {
            return;
        }

        if (QuartzEquipWindow.Instance == null)
        {
            return;
        }
        m_QuartzPosClassToggle.Refresh(0, false, false);
        m_ChooseQuartzGuid = GlobeVar.INVALID_GUID;
        m_ChooseClassId = nClassId;

        m_ReturnQuartzClass.SetActive(true);
        m_QuartzClassNameLabel.text = tClass.Name;
        m_ClassIcon.spriteName = QuartzTool.GetQuartzIconByClassId(tClass.Id);

        m_OptionSortObj.SetActive(true);
        m_QuartzClassTitle.SetActive(false);
        m_QuartzListWindow.SetActive(true);
        m_QuartzSetListWindow.SetActive(false);
        m_QuartzSlotTypeObject.SetActive(false);

        m_QuartzListGrid.transform.localPosition = QuartzListWindowPos_SlotType;
        m_QuartzListPanel.baseClipRegion = QuartzListPanelRange_SlotType;
        m_BGSprite.height = BG_Small_Height;

        m_QuartzSortOption.SetDefaultOption("Option1-Star");
        m_QuartzStarFilterTab.SetDefaultOption("-1");
        UpdateOptionGrid();
        m_DownUpSortToggle.Refresh(0);
    }

    //从具体类型的星宿 返回到星宿预览
    public void OnReturnClassClick()
    {
        m_ChooseClassId = GlobeVar.INVALID_ID;
        m_ChooseQuartzGuid = GlobeVar.INVALID_GUID;
        m_OptionSortObj.SetActive(false);
        m_QuartzClassTitle.SetActive(true);
        m_QuartzListWindow.SetActive(false);
        m_QuartzSetListWindow.SetActive(true);
        m_QuartzSlotTypeObject.SetActive(false);
        m_NoQuartzObject_Class.SetActive(false);

        m_QuartzListGrid.transform.localPosition = QuartzListWindowPos_Class;
        m_QuartzListPanel.baseClipRegion = QuartzListPanelRange_Class;
        m_BGSprite.height = BG_Big_Height;
        InitQuartzSetList(false);

        m_ReturnQuartzClass.SetActive(false);

        if (m_QuartzSortOption.IsOptionOpen())
        {
            m_QuartzSortOption.CloseOption();
        }
        if (m_QuartzStarFilterTab.IsOptionOpen())
        {
            m_QuartzStarFilterTab.CloseOption();
        }

        if (m_QuartzTips.IsShow())
        {
            m_QuartzTips.DirectCloseTips();
        }
        m_QuartzPosClassToggle.Refresh(0, false, false);  //选择 类别
    }

    //----------------------------------------------------------筛选、排序 begin---------------------------------------------
    //在焚星界面 切换1/2/3/4/5/6
    public void OnQuartzSellTypeTabChanged(TabButton tab)
    {
        if (m_QuartzTips.IsShow())
        {
            m_QuartzTips.OnCloseClick();
        }

        InitQuartzList_Sell(true);

        if (QuartzSellWindow.Instance != null)
        {
            QuartzSellWindow.Instance.HandleSellTypeTabChanged();
        }
    }

    void OnDownUpSortToggle(int index)
    {
        if (m_QuartzTips.IsShow())
        {
            m_QuartzTips.OnCloseClick();
        }
        if (m_QuartzPosClassToggle.Index == 0 && m_ChooseClassId != GlobeVar.INVALID_ID)
        {
            InitQuartzList_Class(true);
        }
        else if (m_QuartzPosClassToggle.Index == 1)
        {
            InitQuartzList_Bag(true);
        }
    }

    //1 2 3 4 5 6 变化
    public void OnQuartzSlotTypeTabChanged(TabButton tab)
    {
        if (m_QuartzStarFilterTab.IsOptionOpen())
        {
            m_QuartzStarFilterTab.CloseOption();
        }
        if (m_QuartzSortOption.IsOptionOpen())
        {
            m_QuartzSortOption.CloseOption();
        }
        if (m_QuartzPosClassToggle.Index == 1)
        {
            InitQuartzList_Bag(true);
        }
        else if (m_QuartzPosClassToggle.Index == 0 && m_ChooseClassId != GlobeVar.INVALID_ID)
        {
            InitQuartzList_Class(true);
        }

        //取消选中
        if (m_bNeedCancelChooseSlot)
        {
            if (m_QuartzTips.IsShow())
            {
                m_QuartzTips.OnCloseClick();
            }
            else
            {
                if (QuartzEquipWindow.Instance != null)
                {
                    QuartzEquipWindow.Instance.CancelChooseSlotOnTabChanged();
                }
            }
        }
        else
        {
            m_bNeedCancelChooseSlot = true;
        }

        UpdateTutorialOnTabChange();
    }

    //选择一个排序规则后调用
    void OnQuartzSortOptionChoose(OptionItem item)
    {
        if (item == null)
        {
            return;
        }

        UpdateOptionGrid();

        if (item.name == "Option1-Star")
        {
            m_QuartzStarFilterTab.SetDefaultOption("-1");   //默认选到全部

            if (m_ShowQuartzBag)
            {
                InitQuartzList_Bag(true);
            }
            else
            {
                InitQuartzList_Class(true);
            }

            if (m_QuartzTips.IsShow())
            {
                m_QuartzTips.OnCloseClick();
            }
        }
        else if (item.name == "Option3-RecentlyGet")
        {
            //当选择 时间排序时，隐藏升降序 默认降序
            m_QuartzStarFilterTab.SetDefaultOption("-1");   //默认选到全部
            m_DownUpSortToggle.Index = 0;
        }
        else if(item.name == "Option2-Level")
        {
            m_QuartzStarFilterTab.ChooseOption("-1");   //默认选到全部
            if (m_ShowQuartzBag)
            {
                InitQuartzList_Bag(true);
            }
            else
            {
                InitQuartzList_Class(true);
            }

            if (m_QuartzTips.IsShow())
            {
                m_QuartzTips.OnCloseClick();
            }
        }
    }

    void UpdateOptionGrid()
    {
        //策划需求：当选择“强化等级”，“最近获得”两个档位时，不显示星级筛选
        m_QuartzStarFilterTab.gameObject.SetActive(m_QuartzSortOption.GetCurOptionName() == "Option1-Star");
        m_DownUpSortToggle.gameObject.SetActive(m_QuartzSortOption.GetCurOptionName() != "Option3-RecentlyGet");


        m_OptionGrid.Reposition();
        if (m_QuartzSortOption.IsOptionOpen())
        {
            m_QuartzSortOption.CloseOption();
        }
        if (m_QuartzStarFilterTab.IsOptionOpen())
        {
            m_QuartzStarFilterTab.CloseOption();
        }
    }

    void CloseOpList()
    {
        if (m_QuartzStarFilterTab.IsOptionOpen())
        {
            m_QuartzStarFilterTab.CloseOption();
        }
        if (m_QuartzSortOption.IsOptionOpen())
        {
            m_QuartzSortOption.CloseOption();
        }
    }
    //选择一个稀有度后被调用
    void OnQuartzStarOptionChoose(OptionItem item)
    {
        if (m_QuartzTips.IsShow())
        {
            m_QuartzTips.OnCloseClick();
        }
        CloseOpList();
        if (m_ShowQuartzBag)
        {
            InitQuartzList_Bag(true);
        }
        else
        {
            InitQuartzList_Class(true);
        }
        OnQuartzTipsCloseClick();
    }
    //----------------------------------------------------------筛选、排序 end---------------------------------------------

    //--------------------------------------------------------------刷显示--------------------------------------
    void InitQuartzSetList(bool bResetPostion = false)
    {
        if (QuartzEquipWindow.Instance == null)
        {
            return;
        }

        m_QuartzClassList.Clear();
        int totalCount = 0;
        foreach (var pair in TableManager.GetQuartzSet())
        {
            if (pair.Value == null || pair.Value.Count < 1)
            {
                continue;
            }

            Tab_QuartzSet tSet = pair.Value[0];
            if (tSet == null)
            {
                continue;
            }

            if (IsQuartzClassListContain(tSet.NeedClassId))
            {
                continue;
            }

            int count = GameManager.PlayerDataPool.PlayerQuartzBag.GetQartzCountByClassIdAndSlot(tSet.NeedClassId, QuartzEquipWindow.Instance.GetChooseSlotType());
            totalCount += count;

            m_QuartzClassList.Add(new QuartzClass(tSet.NeedClassId, count));
        }

        m_QuartzClassList.Sort(QuartzTool.QuartzClassLstSort);

        if (false == m_bSetInited)
        {
            m_QuartzSetListWrap.InitList(m_QuartzClassList.Count, OnUpdateQuartzSetItem);
            m_bSetInited = true;
        }
        else
        {
            m_QuartzSetListWrap.UpdateItemCount(m_QuartzClassList.Count);

            if (bResetPostion)
            {
                m_QuartzSetListScroll.ResetPosition();
            }
        }

        m_QuartzTabCountLabel.text = StrDictionary.GetDicByID(8007, totalCount);
        m_QuartzTabCountLabel.gameObject.SetActive(false);
        m_QuartzTabCountLabel.gameObject.SetActive(true);

        m_ShowQuartzBag = false;
        m_ShowSellList = false;
    }

    /// <summary>
    /// Set初始化
    /// </summary>
    void InitQuartzList_Class(bool bResetPostion = false)
    {
        if (QuartzEquipWindow.Instance == null)
        {
            return;
        }

        int nChooseStar = GetChooseStar();
        int nChooseSlotType = QuartzEquipWindow.Instance.GetChooseSlotType();
        m_QuartzList = GameManager.PlayerDataPool.PlayerQuartzBag.QuartzFilter((Quartz quartz) =>
        {
            if (quartz == null || false == quartz.IsValid())
            {
                return false;
            }

            if (quartz.GetClassId() != m_ChooseClassId)
            {
                return false;
            }

            if (nChooseSlotType != GlobeVar.INVALID_ID && quartz.GetSlotType() != nChooseSlotType)
            {
                return false;
            }

            if (nChooseStar != -1 && quartz.Star != nChooseStar)
            {
                return false;
            }

            return true;
        });

        if (m_DownUpSortToggle.Index == 0)
        {
            if (m_QuartzSortOption.GetCurOptionName() == "Option1-Star")
            {
                m_QuartzList.Sort(QuartzTool.QuartzClassSort_Star_Down);
            }
            else if (m_QuartzSortOption.GetCurOptionName() == "Option2-Level")
            {
                m_QuartzList.Sort(QuartzTool.QuartzClassSort_Strengthen_Down);
            }
            else if (m_QuartzSortOption.GetCurOptionName() == "Option3-RecentlyGet")
            {
                m_QuartzList.Sort(QuartzTool.QuartzClassSort_NewGet_Down);
            }
        }
        else
        {
            if (m_QuartzSortOption.GetCurOptionName() == "Option1-Star")
            {
                m_QuartzList.Sort(QuartzTool.QuartzClassSort_Star_Up);
            }
            else if (m_QuartzSortOption.GetCurOptionName() == "Option2-Level")
            {
                m_QuartzList.Sort(QuartzTool.QuartzClassSort_Strengthen_Up);
            }
            else if (m_QuartzSortOption.GetCurOptionName() == "Option3-RecentlyGet")
            {
                m_QuartzList.Sort(QuartzTool.QuartzClassSort_NewGet_Up);
            }
        }

        if (false == m_bClassInited)
        {
            m_QuartzListWrap.InitList(m_QuartzList.Count, OnUpdateQuartzItem);
            m_bClassInited = true;
        }
        else
        {
            m_QuartzListWrap.UpdateItemCount(m_QuartzList.Count);

            if (bResetPostion || m_QuartzList.Count <= m_QuartzItemObjCount)
            {
                m_QuartzListWrap.InitList(m_QuartzList.Count, OnUpdateQuartzItem);
                //m_QuartzListScroll.ResetPosition();
            }
        }

        m_QuartzTabCountLabel.text = StrDictionary.GetDicByID(8007, m_QuartzList.Count.ToString());
        m_QuartzTabCountLabel.gameObject.SetActive(false);
        m_QuartzTabCountLabel.gameObject.SetActive(true);
        m_NoQuartzObject_Class.SetActive(m_QuartzList.Count <= 0);

        m_ShowQuartzBag = false;
        m_ShowSellList = false;
    }

    /// <summary>
    /// 根据当前筛选项、排序规则刷新当前星宿背包显示
    /// </summary>
    void InitQuartzList_Bag(bool bResetPosition = false)
    {
        if (m_QuartzSlotTypeTab.GetHighlightTab() == null)
        {
            return;
        }

        int nTabSlotType;
        if (false == int.TryParse(m_QuartzSlotTypeTab.GetHighlightTab().name, out nTabSlotType))  //拿到玩家当前关注的槽
        {
            return;
        }

        int nChooseStar = GetChooseStar();
        m_QuartzList = GameManager.PlayerDataPool.PlayerQuartzBag.QuartzFilter((Quartz quartz) =>
        {
            if (quartz == null || false == quartz.IsValid())
            {
                return false;
            }

            if (quartz.GetSlotType() != nTabSlotType)
            {
                return false;
            }

            if (nChooseStar != -1 && quartz.Star != nChooseStar)
            {
                return false;
            }

            return true;
        });

        if (m_DownUpSortToggle.Index == 0)
        {
            if (m_QuartzSortOption.GetCurOptionName() == "Option1-Star")
            {
                m_QuartzList.Sort(QuartzTool.QuartzBagSort_Star_Down);
            }
            else if (m_QuartzSortOption.GetCurOptionName() == "Option2-Level")
            {
                m_QuartzList.Sort(QuartzTool.QuartzBagSort_Strengthen_Down);
            }
            else if (m_QuartzSortOption.GetCurOptionName() == "Option3-RecentlyGet")
            {
                m_QuartzList.Sort(QuartzTool.QuartzBagSort_NewGet_Down);
            }
        }
        else
        {
            if (m_QuartzSortOption.GetCurOptionName() == "Option1-Star")
            {
                m_QuartzList.Sort(QuartzTool.QuartzBagSort_Star_Up);
            }
            else if (m_QuartzSortOption.GetCurOptionName() == "Option2-Level")
            {
                m_QuartzList.Sort(QuartzTool.QuartzBagSort_Strengthen_Up);
            }
            else if (m_QuartzSortOption.GetCurOptionName() == "Option3-RecentlyGet")
            {
                m_QuartzList.Sort(QuartzTool.QuartzBagSort_NewGet_Up);
            }
        }

        if (false == m_bInited)
        {
            m_QuartzListWrap.InitList(m_QuartzList.Count, OnUpdateQuartzItem);
            m_bInited = true;
        }
        else
        {
            m_QuartzListWrap.UpdateItemCount(m_QuartzList.Count);

            if (bResetPosition || m_QuartzList.Count <= m_QuartzItemObjCount)
            {
                m_QuartzListWrap.InitList(m_QuartzList.Count, OnUpdateQuartzItem);
                //m_QuartzListScroll.ResetPosition();
            }
        }

        m_QuartzTabCountLabel.text = StrDictionary.GetDicByID(8007, m_QuartzList.Count.ToString());
        m_QuartzTabCountLabel.gameObject.SetActive(false);
        m_QuartzTabCountLabel.gameObject.SetActive(true);
        m_NoQuartzObject_Pos.SetActive(m_QuartzList.Count <= 0);

        m_ShowQuartzBag = true;
        m_ShowSellList = false;
    }

    /// <summary>
    /// 初始化 焚星列表
    /// </summary>
    void InitQuartzList_Sell(bool bResetPostion = false)
    {
        m_SellGuidList.Clear();

        // 如果当前正在全选 此时自动取消全选 但只更新标记
        m_IsChooseAll = false;

        m_SlotTypeFilterList.Clear();
        for (int i = 1; i <= GlobeVar.ORBMENT_SLOT_SIZE; i++)
        {
            m_SlotTypeFilterList.Add(i);
        }

        m_QuartzList = GameManager.PlayerDataPool.PlayerQuartzBag.QuartzFilter((Quartz quartz) => { return quartz.Star == SellStar; });
        m_QuartzList.Sort(QuartzTool.QuartzSort_Sell);

        if (false == m_bSellInited)
        {
            m_QuartzListWrap.InitList(m_QuartzList.Count, OnUpdateQuartzItem);
            m_bSellInited = true;
        }
        else
        {
            m_QuartzListWrap.UpdateItemCount(m_QuartzList.Count);

            if (bResetPostion || m_QuartzList.Count <= m_QuartzItemObjCount)
            {
                m_QuartzListWrap.InitList(m_QuartzList.Count, OnUpdateQuartzItem);
               // m_QuartzListScroll.ResetPosition();
            }
        }

        m_QuartzTabCountLabel.text = StrDictionary.GetDicByID(8007, m_QuartzList.Count.ToString());
        m_QuartzTabCountLabel.gameObject.SetActive(false);
        m_QuartzTabCountLabel.gameObject.SetActive(true);
        m_NoQuartzObject_Pos.SetActive(m_QuartzList.Count <= 0);

        m_ShowQuartzBag = false;
        m_ShowSellList = true;
    }

    //玩家已拥有的星魂class是否有参数类型的
    bool IsQuartzClassListContain(int nClassId)
    {
        for (int i = 0; i < m_QuartzClassList.Count; i++)
        {
            if (m_QuartzClassList[i].m_ClassId == nClassId)
            {
                return true;
            }
        }

        return false;
    }

    //星宿class Wrap滑动时
    void OnUpdateQuartzSetItem(GameObject item, int index)
    {
        if (item == null)
        {
            return;
        }

        QuartzClassItem itemLogic = item.GetComponent<QuartzClassItem>();
        if (itemLogic == null)
        {
            return;
        }

        if (index < 0 || index >= m_QuartzClassList.Count)
        {
            return;
        }

        if (index == 0)
        {
            m_FirstClassItem = itemLogic.gameObject;
        }

        itemLogic.Init(m_QuartzClassList[index].m_ClassId, m_QuartzClassList[index].m_Count);
    }


    //星魂List的Wrap滑动时被调用
    void OnUpdateQuartzItem(GameObject item, int index)
    {
        if (item == null)
        {
            return;
        }

        if (index < 0 || index >= m_QuartzList.Count)
        {
            return;
        }

        QuartzItem itemLogic = item.GetComponent<QuartzItem>();
        if (itemLogic == null)
        {
            return;
        }

        if (m_QuartzList[index] == null)
        {
            return;
        }

        itemLogic.Init(m_QuartzList[index], m_SellGuidList.Contains(m_QuartzList[index].Guid));


        if (TutorialRoot.IsGroupStep(TutorialGroup.QuartzEquip, 12) ||
            m_TutorialGroupOnShow == TutorialGroup.QuartzEquip && m_TutorialStepOnShow == 10 ||
            m_TutorialGroupOnShow == TutorialGroup.QuartzSuit && m_TutorialStepOnShow == 5)
        {
            if (index == 0)
            {
                m_FirstQuartzItemLogic = itemLogic;
            }
        }
    }



    //点击星魂背包的某个星魂 -> 弹出星魂Tips
    public void HandleQuartzItemClick(QuartzItem item)
    {
        if (item == null)
        {
            return;
        }

        if (m_QuartzOpTab.GetHighlightTab() == null)
        {
            return;
        }
        if( item.Quartz == null)
        {
            return;
        }
        CloseOpList();
        //如果在观星界面
        if (m_QuartzOpTab.GetHighlightTab().name == "Tab01-Star")
        {
            if (item.GetGuid() == m_ChooseQuartzGuid)
            {
                QuartzItem[] allItem = m_QuartzListWrap.GetComponentsInChildren<QuartzItem>();
                for (int i = 0; i < allItem.Length; i++)
                {
                    allItem[i].UpdateChoose(false);
                }

                m_QuartzTips.OnCloseClick();
                m_ChooseQuartzGuid = GlobeVar.INVALID_GUID;

                if (QuartzEquipWindow.Instance != null)
                {
                    QuartzEquipWindow.Instance.HandleQuartzItemCancel();
                }
            }
            //如果选了不同的
            else
            {
                QuartzItem[] allItem = m_QuartzListWrap.GetComponentsInChildren<QuartzItem>();
                for (int i = 0; i < allItem.Length; i++)
                {
                    allItem[i].UpdateChoose(allItem[i].GetGuid() == item.GetGuid());
                }

                m_QuartzTips.Show(item.Quartz, QuartzTipsWindow.TipsType.Bag, m_Card);
                m_ChooseQuartzGuid = item.GetGuid();

                if (QuartzEquipWindow.Instance != null)
                {
                    QuartzEquipWindow.Instance.HandleQuartzItemClick(item.Quartz);
                }
            }
        }
        else if (m_QuartzOpTab.GetHighlightTab().name == "Tab02-Sell")
        {
            if (m_IsChooseAll)
            {
                // 如果当前正在全选 此时自动取消全选 但只更新标记
                m_IsChooseAll = false;
            }

            if (m_SellGuidList.Contains(item.GetGuid()))
            {
                m_SellGuidList.Remove(item.GetGuid());
                m_QuartzTips.OnCloseClick();
            }
            else
            {
                m_QuartzTips.Show(item.Quartz, QuartzTipsWindow.TipsType.Bag);
                if(item.Quartz.IsLock())
                {
                    return;
                }
                m_SellGuidList.Add(item.GetGuid());
            }

            item.UpdateSellChoose(m_SellGuidList.Contains(item.GetGuid()));

            if (QuartzSellWindow.Instance != null)
            {
                QuartzSellWindow.Instance.HandleQuartzItemClick();
            }
        }
    }

    public void HandleOrbmentSlotItemClick()
    {
        if (QuartzEquipWindow.Instance == null)
        {
            return;
        }

        if (QuartzEquipWindow.Instance.ChooseSlotItem != null &&
            QuartzEquipWindow.Instance.ChooseSlotItem.Quartz != null)
        {
            // 显示新tips
            m_QuartzTips.Show(
                QuartzEquipWindow.Instance.ChooseSlotItem.Quartz, 
                QuartzTipsWindow.TipsType.Equiped,
                m_Card);
        }
        else
        {
            // 选中空槽位 关闭tips
            if (m_QuartzTips.IsShow())
            {
                m_QuartzTips.DirectCloseTips();
            }
        }

        m_bNeedCancelChooseSlot = false;
        HandleChooseSlotChange(true);
    }

    public void HandleChooseSlotChange(bool bNeedInit = false)
    {
        if (QuartzEquipWindow.Instance == null)
        {
            return;
        }

        if (m_QuartzPosClassToggle.Index == 0)
        {
            if (m_ChooseClassId == GlobeVar.INVALID_ID)
            {
                // 查看类别列表
                InitQuartzSetList();
            }
            else
            {
                // 查看某一类别的星魂列表 点击槽位视为筛选
                InitQuartzList_Class(bNeedInit);
            }
        }
        else if (m_QuartzPosClassToggle.Index == 1)
        {
            int nSlotType = QuartzEquipWindow.Instance.GetChooseSlotType();
            if (nSlotType != GlobeVar.INVALID_ID)
            {
                // 按位置查看星魂列表 点击槽位视为切换分页 取消选择时分页不变
                m_QuartzSlotTypeTab.ChangeTab(nSlotType.ToString());
            }
        }
    }

    //关闭星魂Tips
    public void OnQuartzTipsCloseClick()
    {
        m_ChooseQuartzGuid = GlobeVar.INVALID_GUID;

        QuartzItem[] allItem = m_QuartzListGrid.GetComponentsInChildren<QuartzItem>();
        for (int i = 0; i < allItem.Length; i++)
        {
            allItem[i].UpdateChoose(false);
        }
    }

    public void HandleOrbmentResetClick()
    {
        m_QuartzTips.OnCloseClick();
    }

    //强化界面时调用
    public void HandleStrengthenOpen()
    {
        m_PlayerResourcesObj.SetActive(false);
    }

    //关闭强化界面调用
    public void HandleStrengthenClose()
    {
        m_PlayerResourcesObj.SetActive(true);
    }

    void OnClickSortOpList()
    {
        if (m_QuartzStarFilterTab.IsOptionOpen())
        {
            m_QuartzStarFilterTab.CloseOption();
        }
    }

    void OnClickStarOpList()
    {
        if (m_QuartzSortOption.IsOptionOpen())
        {
            m_QuartzSortOption.CloseOption();
        }
    }

    //关闭星魂界面，返回符灵界面
    public void OnCloseClick()
    {
        if (TutorialRoot.IsGroup(TutorialGroup.QuartzEquip))
        {
            TutorialRoot.TutorialGroupOver(TutorialGroup.QuartzEquip);
        }
        UIManager.CloseUI(UIInfo.OrbmentRoot);
        if (m_Card != null)
        {
            CardBagController.OrbmentReturnCardBag(m_Card.Guid);
        }
    }

    public void OnClickJump()
    {
        Tab_QuartzClass tClass = TableManager.GetQuartzClassByID(m_ChooseClassId, 0);
        if (tClass == null)
        {
            return;
        }
        ItemGain.ItemGainJumpByGainId(tClass.GainId,null,true);
    }
    public void HandleTipClose()
    {
        m_QuartzTips.OnCloseClick();
    }


    //星魂增加-》刷新全部显示，拿到Model层新数据
    public void HandleQuartzAdd()
    {
        if (m_QuartzListWindow.activeSelf)
        {
            if (m_ShowQuartzBag)
            {
                InitQuartzList_Bag();
            }
            else if (m_ShowSellList)
            {
                InitQuartzList_Sell();
            }
            else
            {
                InitQuartzList_Class();
            }
        }
        else if (m_QuartzSetListWindow.activeSelf)
        {
            InitQuartzSetList();
        }

        InitQuartzWholeCount();
    }

    /// <summary>
    /// 星魂出售-> 刷新全部星魂显示
    /// </summary>
    /// <param name="bEquiped">该星魂之前是否装备</param>
    public void HandleQuartzSell(bool bEquiped)
    {
        m_SellGuidList.Clear();

        if (bEquiped)
        {
        }
        else
        {
            if (m_QuartzListWindow.activeSelf)
            {
                if (m_ShowQuartzBag)
                {
                    InitQuartzList_Bag();
                }
                else if (m_ShowSellList)
                {
                    InitQuartzList_Sell(true);
                }
                else
                {
                    InitQuartzList_Class();
                }
            }
            else if (m_QuartzSetListWindow.activeSelf)
            {
                InitQuartzSetList();
            }

            InitQuartzWholeCount();
        }
    }

    /// <summary>
    /// 星魂强化后 刷新显示
    /// </summary>
    /// <param name="bEquiped">是否装备</param>
    public void HandleQuartzStrengthen(bool bEquiped)
    {
        if (bEquiped)
        {
        }
        else
        {
            if (m_QuartzListWindow.activeSelf)
            {
                if (m_ShowQuartzBag)
                {
                    InitQuartzList_Bag();
                }
                else if (m_ShowSellList)
                {
                    InitQuartzList_Sell(true);
                }
                else
                {
                    InitQuartzList_Class();
                }
            }
            else if (m_QuartzSetListWindow.activeSelf)
            {
                InitQuartzSetList();
            }
        }
    }

    /// <summary>
    /// 星魂装备后 刷新显示
    /// </summary>
    public void HandleQuartzEquip()
    {
        if (m_QuartzListWindow.activeSelf)
        {
            if (m_ShowQuartzBag)
            {
                InitQuartzList_Bag();
            }
            else if (m_ShowSellList)
            {
                InitQuartzList_Sell();
            }
            else
            {
                InitQuartzList_Class();
            }
        }
        else if (m_QuartzSetListWindow.activeSelf)
        {
            InitQuartzSetList();
        }

        InitQuartzWholeCount();
    }

    /// <summary>
    /// 卸下星魂后 刷新显示
    /// </summary>
    public void HandleQuartzUnEquip()
    {
        if (m_QuartzListWindow.activeSelf)
        {
            if (m_ShowQuartzBag)
            {
                InitQuartzList_Bag();
            }
            else if (m_ShowSellList)
            {
                InitQuartzList_Sell();
            }
            else
            {
                InitQuartzList_Class();
            }
        }
        else if (m_QuartzSetListWindow.activeSelf)
        {
            InitQuartzSetList();
        }

        InitQuartzWholeCount();

    }




    //更新强化石显示
    public void UpdateStrengthenStoneLabel()
    {
        m_StrengthenStoneLabel.text =
            GameManager.PlayerDataPool.CommonPack.GetItemCountByDataId(GlobeVar.ORBMENT_ITEMID_STRHENGTHENSTONE).ToString();
    }

    //更新当前金币显示
    public void UpdateCoinLabel()
    {
        m_CoinLabel.text = ItemIconStr.GetCurrencyStringForShow(GameManager.PlayerDataPool.GetGold());
    }

    //刷新总数量
    void InitQuartzWholeCount()
    {
        string countStr = string.Format("{0}/{1}",
            GameManager.PlayerDataPool.PlayerQuartzBag.QuartzList.Count, GlobeVar.QUARTZBAG_SIZE);
        m_QuartzWholeCountLabel.text = StrDictionary.GetDicByID(8006, countStr);
        m_QuartzWholeCountLabel.gameObject.SetActive(false);
        m_QuartzWholeCountLabel.gameObject.SetActive(true);
    }

    public ulong GetCardGuid()
    {
        return m_Card != null ? m_Card.Guid : GlobeVar.INVALID_GUID;
    }



    //点击一键焚星
    public void OnClickSellAll()
    {
        if (m_IsChooseAll)
        {
            // 取消全选
            m_SellGuidList.Clear();

            m_IsChooseAll = false;
            m_QuartzListWrap.UpdateAllItem();

            //更新价格
            if (QuartzSellWindow.Instance != null)
            {
                QuartzSellWindow.Instance.HandleQuartzItemClick();
            }
        }
        else
        {
            //全选
            m_SellGuidList.Clear();
            MessageBoxController.OpenOKCancel(8433, 8431,OnSellAllThrow,OnSellAllOK,8432,7748);
        }
    }

    private void OnSellAllOK()
    {
        for (int i = 0; i < m_QuartzList.Count; i++)
        {
            if(!m_QuartzList[i].IsLock())
            {
                m_SellGuidList.Add(m_QuartzList[i].Guid);
            }

        }
        m_QuartzListWrap.UpdateAllItem();

        //更新价格
        if (QuartzSellWindow.Instance != null)
        {
            QuartzSellWindow.Instance.HandleQuartzItemClick();
        }
        m_IsChooseAll = true;
    }

    private void OnSellAllThrow()
    {
        for (int i = 0; i < m_QuartzList.Count; i++)
        {
            if(m_QuartzList[i].IsThrow() && !m_QuartzList[i].IsLock())
            {
                m_SellGuidList.Add(m_QuartzList[i].Guid);
            }
        }
        m_QuartzListWrap.UpdateAllItem();

        //更新价格
        if (QuartzSellWindow.Instance != null)
        {
            QuartzSellWindow.Instance.HandleQuartzItemClick();
        }
        m_IsChooseAll = true;
    }

    public void OnClickStrengthenPlus()
    {
        MessageBoxController.OpenOKCancel(7358, 7357, () =>
        {
            m_QuartzOpTab.ChangeTab("Tab02-Sell");
            //关闭强化界面
            UIManager.CloseUI(UIInfo.QuartzStrengthen);
        }, null, 7360, 7359);
    }

    public void HandleCardClick()
    {
        if (m_QuartzTips.IsShow())
        {
            m_QuartzTips.OnCloseClick();
        }
        CloseOpList();
    }

    public void HandleSetRecommendClick()
    {
        if (m_QuartzTips.IsShow())
        {
            m_QuartzTips.OnCloseClick();
        }
        CloseOpList();
    }

    #region 系统消耗时长
    private float _updateTime = 0f;
    private int reportGuanxingTime;
    private int reportFenxingTime;
    private void Update()
    {
        if (m_QuartzOpTab.GetHighlightTab() == null)
        {
            return;
        }
        int index = m_QuartzOpTab.GetHighlightTab().Index;
        if (index == 0)
        {
            _updateTime += Time.deltaTime;
            if (_updateTime >= 1.0f)  // 一秒刷新一次
            {
                reportGuanxingTime += 1;
                _updateTime = 0;
            }
        }

        if (index == 1)
        {
            _updateTime += Time.deltaTime;
            if (_updateTime >= 1.0f)  // 一秒刷新一次
            {
                reportFenxingTime += 1;
                _updateTime = 0;
            }
        }

    }

    #endregion



    void UpdateTutorialOnShow()
    {
        if (m_TutorialGroupOnShow == TutorialGroup.Invalid && m_TutorialStepOnShow == GlobeVar.INVALID_ID)
        {
            return;
        }
        if (m_TutorialGroupOnShow == TutorialGroup.QuartzEquip && m_TutorialStepOnShow == 10)
        {
            if (m_FirstQuartzItemLogic != null)
            {
                TutorialRoot.ShowTutorial(TutorialGroup.QuartzEquip, 10, m_FirstQuartzItemLogic.m_QuartzIcon.gameObject, m_FirstQuartzItemLogic.m_QuartzIcon.width, m_FirstQuartzItemLogic.m_QuartzIcon.height);
            }
            else
            {
                TutorialRoot.TutorialOver();
            }
        }
        if (m_TutorialGroupOnShow == TutorialGroup.QuartzSuit && m_TutorialStepOnShow == 5)
        {
            TutorialRoot.ShowTutorial(TutorialGroup.QuartzSuit, 5, m_TutorialClassTab.gameObject, m_TutorialClassTab.width, m_TutorialClassTab.height);
        }
        m_TutorialGroupOnShow = TutorialGroup.Invalid;
        m_TutorialStepOnShow = GlobeVar.INVALID_ID;
    }

    void UpdateTutorialOnTabChange()
    {
        if (TutorialRoot.IsGroupStep(TutorialGroup.QuartzEquip, 12))
        {
            if (m_FirstQuartzItemLogic != null)
            {
                TutorialRoot.ShowTutorial(TutorialGroup.QuartzEquip, 13, m_FirstQuartzItemLogic.m_QuartzIcon.gameObject, m_FirstQuartzItemLogic.m_QuartzIcon.width, m_FirstQuartzItemLogic.m_QuartzIcon.height);
            }
            else
            {
                TutorialRoot.TutorialOver();
            }
        }
    }

    public void UpdateTutorialOnEquipBtnClick(TutorialGroup group, int nStep)
    {
        if (group == TutorialGroup.QuartzEquip && nStep == 12)
        {
            m_FirstQuartzItemLogic = null;
            TutorialRoot.ShowTutorial(TutorialGroup.QuartzEquip, 12, m_TutorialFifthPosTab.gameObject, m_TutorialFifthPosTab.width, m_TutorialFifthPosTab.height);
        }
    }
    public void UpdateTutorialOnTutorialMaskClick()
    {
        if (TutorialRoot.IsGroupStep(TutorialGroup.QuartzSuit, 5))
        {
            if (m_FirstQuartzItemLogic != null)
            {
                TutorialRoot.ShowTutorial(TutorialGroup.QuartzSuit, 6, m_FirstQuartzItemLogic.m_QuartzIcon.gameObject, m_FirstQuartzItemLogic.m_QuartzIcon.width, m_FirstQuartzItemLogic.m_QuartzIcon.height);
            }
            else
            {
                TutorialRoot.TutorialOver();
            }
        }
        else if (TutorialRoot.IsGroupStep(TutorialGroup.QuartzSuit, 6))
        {
            TutorialRoot.TutorialOver();
        }
    }

    //隐藏跳转btn
    public void RefreshUIForHideGo()
    {
        if (null != m_hideGoList)
        {
            for (int i = 0; i < m_hideGoList.Length; ++i)
            {
                if (m_hideGoList[i] != null)
                {
                    m_hideGoList[i].SetActive(UIBattleEnd.s_EndFlag != UIBattleEnd.ENUM_ENDFLAG.OPENUI_FULINGLU);
                }
            }
        }
    }
}
