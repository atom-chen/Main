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
    //-----------------------------Component---------------------
    //排序/筛选项 及其缓存变量
    public TabController m_QuartzOpTab;                 //观星/焚星 （观星界面）
    public TabController m_QuartzSellTab;             
    private bool m_ShowSellList = false;                //是否打开焚星界面

    public ToggleActive m_QuartzTab;                   //位置/类别
    public ToggleActive m_DownUpSortTab;               //降序/升序

    public OptionList m_QuartzSortOption;                //排序规则

    public TabController m_QuartzSlotTypeTab;            //1/2/3/4/5/6
    private int m_QuartzSlot_CurChoose = 1;              //当前所选槽位

    public OptionList m_QuartzStarFilterTab;              //白色-神品
    private int m_Star_CurChoose = GlobeVar.INVALID_ID;   //当前所关注星级

    public UIGrid m_OptionGrid;                          //选项Grid

    public GameObject m_QuartzSetListWindow;
    public ListWrapController m_QuartzSetListWrap;      //星宿class滑动列表
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
    public GameObject m_NoQuartzObject;
    public UILabel m_QuartzTabCountLabel;            //出售星魂 当前tab分页筛选出的星魂数量 
    public GameObject m_QuartzSellObj;
    public UILabel m_QuartzWholeCountLabel;
    public GameObject m_OptionSortObj;
    public GameObject m_PlayerResourcesObj;           //金币 星尘面板父物体
    //pos

    //set

    //class
    public GameObject m_ReturnQuartzClass;
    public UILabel m_QuartzClassNameLabel;
    public UISprite m_ClassIcon;

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
    private int m_DefaultSlotIndex = 0;

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

    private static Vector3 QuartzListWindowPos_SlotType = new Vector3(-142, -98, 0);
    private static Vector3 QuartzListWindowPos_Class = new Vector3(-142, -55, 0);
    private static Vector4 QuartzListPanelRange_SlotType = new Vector4(8, -244, 440, 400);
    private static Vector4 QuartzListPanelRange_Class = new Vector4(8, -224, 440, 440);
    //星魂class UI的宽高
    private const int ClassItemWidth = 210;
    private const int ClassItemHeight = 260;

    //新手指引类型
    public static TutorialGroup m_TutorialGroupOnShow = TutorialGroup.Invalid;
    public static int m_TutorialStepOnShow = GlobeVar.INVALID_ID;
    private QuartzItem m_FirstQuartzItemLogic;

    void Awake()
    {
        m_Instance = this;

        m_QuartzTab.onToggle = OnQuartzTabChanged;                                  //位置/类别
        m_DownUpSortTab.onToggle = OnDownOrUpChanged;
        m_QuartzSortOption.delOnOptionItemChoose = OnQuartzSortOptionChoose;        //排序规则
        m_QuartzStarFilterTab.delOnOptionItemChoose = OnQuartzCareOptionChoose;
        m_QuartzSlotTypeTab.delTabChanged = OnQuartzSlotTypeTabChanged;             //槽位筛选项 1/2/3/4/5/6




        m_QuartzOpTab.delTabChanged = OnQuartzOpTabChanged;                         //观星界面：观星/焚星
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
        m_QuartzSlot_CurChoose = 0;
        m_SellGuidList.Clear();
        m_ChooseQuartzGuid = GlobeVar.INVALID_GUID;
        m_FirstClassItem = null;
        m_DefaultSlotIndex = 0;
        PlayerData.delegateGoldCoinChanged -= UpdateCoinLabel;
        PlayerData.delegateCommonPackItemChanged -= UpdateStrengthenStoneLabel;
    }

    void OnEnable()
    {
        UIManager.SetMainCameraStatesOnUIChange(MAINCAMERA_HIDE_UI.STAR, true);
        m_QuartzTab.gameObject.SetActive(false);
    }

    void OnDisable()
    {
        UIManager.SetMainCameraStatesOnUIChange(MAINCAMERA_HIDE_UI.STAR, false);
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("XingXiuGuanXingTime", MsdkController.GetReportTime(reportGuanxingTime));
        dic.Add("XingXiuFengXingTime", MsdkController.GetReportTime(reportFenxingTime));
        MsdkController.WGReportEvent("XingXiu", dic, true);//数据上报
    }

    //入口
    public static void ShowOrbment(Card card, List<Card> cardList)
    {
        List<object> Params = new List<object>() { card,cardList, 0};
        UIManager.ShowUI(UIInfo.OrbmentRoot, OnOpenOrbmentRoot, Params, UIStack.StackType.PushAndPop);
    }

    public static void ShowOrbment(Card card, List<Card> cardList,int defaultSlotIndex)
    {
        List<object> Params = new List<object>() { card, cardList, defaultSlotIndex };
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

        if(!int.TryParse(Params[2].ToString(),out m_Instance.m_DefaultSlotIndex))
        {
            m_Instance.m_DefaultSlotIndex = 0;
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

        InitTab();
        yield return null;
        UpdateTutorialOnShow();
        m_DefaultSlotIndex = 0;
    }

    void InitTab()
    {
        m_QuartzOpTab.ChangeTab("Tab01-Star");                        //默认选择 观星
        m_ShowSellList = false;
        m_QuartzTab.Refresh(1);                               //默认选择 位置
        ReSetStatus();
    }

    //初始化4个筛选项和排序规则（不包括页面切换）
    private void ReSetStatus()
    {
        SetSlotTab(m_DefaultSlotIndex);                                  //默认选择0号槽
        m_QuartzSortOption.ChooseOption("Option1-Star");          //默认按 稀有度 排序
        m_DownUpSortTab.Refresh(0);                                         //默认 降序
        SetRareTab(-1);                                    //默认 查看全部稀有度星宿
    }

    //设置筛选规则
    private void SetRareTab(int index)
    {
        m_QuartzStarFilterTab.ChooseOption(index.ToString());   //默认 查看全部稀有度星宿
        int nStar = -1;
        if (int.TryParse(m_QuartzStarFilterTab.GetCurOptionName(), out nStar))
        {
            m_Star_CurChoose = nStar;
        }
        else
        {
            m_Star_CurChoose = GlobeVar.INVALID_ID;
        }
    }

    bool m_bNeedCloseTips = true;
    //设置筛选槽
    public void SetSlotTab(int index,bool needCloseTips=false)
    {
        m_bNeedCloseTips = needCloseTips;
        if (m_bNeedCloseTips)
        {
            m_QuartzTips.OnCloseClick();
        }
        m_QuartzSlotTypeTab.ChangeTab(index);                                    
        m_QuartzSlot_CurChoose = index;

        m_bNeedCloseTips = true;
    }
//--------------------------------------------------显示状态切换------------------------------
    //观星界面的Tab点击后调用
    void OnQuartzOpTabChanged(TabButton tab)
    {
        ReSetStatus();
        if (tab == null)
        {
            return;
        }
        m_ChooseClassId = GlobeVar.INVALID_ID;
        m_ChooseQuartzGuid = GlobeVar.INVALID_GUID;
        //观星
        if (tab.name == "Tab01-Star")
        {
            //显示观星界面的筛选项
            m_QuartzTab.gameObject.SetActive(true);
            m_OptionSortObj.SetActive(true);
            m_SellGuidList.Clear();
            m_QuartzTab.Refresh(1);           //默认选择 pos
        }
        //焚星
        else if (tab.name == "Tab02-Sell")
        {
            //隐藏观星界面的筛选项
            m_OptionSortObj.SetActive(false);
            m_QuartzClassTitle.SetActive(false);

            m_ReturnQuartzClass.SetActive(false);
            m_QuartzSlotTypeObject.SetActive(false);
            m_QuartzListGrid.transform.localPosition = QuartzListWindowPos_Class;
            m_QuartzListPanel.baseClipRegion = QuartzListPanelRange_Class;

            m_QuartzTab.gameObject.SetActive(false);
            m_QuartzListWindow.SetActive(true);
            m_QuartzSetListWindow.SetActive(false);
            m_QuartzSellObj.SetActive(true);
            m_QuartzSellTab.ChangeTab("1");
            m_IsChooseAll = false;
            InitQuartzList_Sell();
        }

        if (m_QuartzSortOption.IsOptionOpen())
        {
            m_QuartzSortOption.CloseOption();
        }

        if (m_QuartzTips.IsShow())
        {
            m_QuartzTips.OnCloseClick();
        }
    }

    //位置/类别切换后调用
    void OnQuartzTabChanged(int index)
    {
        ReSetStatus();
        if (m_QuartzTips.IsShow())
        {
            m_QuartzTips.OnCloseClick();
        }
        m_ChooseClassId = GlobeVar.INVALID_ID;
        m_ChooseQuartzGuid = GlobeVar.INVALID_GUID;
        //按类别
        if (index == 0)
        {
            //取消所有装备槽勾选
            OnQuartzTipsCloseClick();
            if (QuartzEquipWindow.Instance != null)
            {
                QuartzEquipWindow.Instance.HandleTipsCloseClick();
            }

            //隐藏筛选项
            m_OptionSortObj.SetActive(false);

            //显示标题
            m_QuartzClassTitle.SetActive(true);

            m_QuartzListWindow.SetActive(false);

            m_QuartzSetListWindow.SetActive(true);

            InitQuartzSetList();

            m_QuartzSlotTypeObject.SetActive(false);
            m_NoQuartzObject.SetActive(false);
        }
        //按位置分类
        else if (index == 1)
        {
            //显示筛选项
            m_OptionSortObj.SetActive(true);

            m_QuartzListWindow.SetActive(true);
            m_QuartzSetListWindow.SetActive(false);
            m_QuartzSellObj.SetActive(false);
            m_QuartzClassTitle.SetActive(false);

            if (m_QuartzSlot_CurChoose == GlobeVar.INVALID_ID)
            {
                m_QuartzSlotTypeTab.ChangeTab("1");
            }
            else
            {
                m_QuartzSlotTypeTab.ChangeTab(m_QuartzSlot_CurChoose > 5 ? 5 : m_QuartzSlot_CurChoose);
                m_QuartzSlot_CurChoose = 0;
            }

            m_QuartzSlotTypeObject.SetActive(true);
            m_QuartzListGrid.transform.localPosition = QuartzListWindowPos_SlotType;
            m_QuartzListPanel.baseClipRegion = QuartzListPanelRange_SlotType;
            InitQuartzList_Bag();
        }

        OnQuartzTipsCloseClick();

        if (m_QuartzSortOption.IsOptionOpen())
        {
            m_QuartzSortOption.CloseOption();
        }
    }

    //点击某个星魂Class -> 进入某class星魂UI
    public void HandleQuartzClassItemClick(int nClassId)
    {
        m_ChooseClassId = GlobeVar.INVALID_ID;
        m_ChooseQuartzGuid = GlobeVar.INVALID_GUID;
        ReSetStatus();
        Tab_QuartzClass tClass = TableManager.GetQuartzClassByID(nClassId, 0);
        if (tClass == null)
        {
            return;
        }
        m_OptionSortObj.SetActive(true);
        m_ChooseClassId = nClassId;


        m_QuartzTab.gameObject.SetActive(false);
        m_ReturnQuartzClass.SetActive(true);
        m_QuartzClassNameLabel.text = tClass.Name;
        m_ClassIcon.spriteName = QuartzTool.GetQuartzIconByClassId(tClass.Id);

        m_QuartzListWindow.SetActive(true);
        m_QuartzSetListWindow.SetActive(false);
        m_QuartzSellObj.SetActive(false);

        InitQuartzList_Class();
        //显示筛选项
        m_OptionSortObj.SetActive(true);

        m_QuartzListWindow.SetActive(true);
        m_QuartzSetListWindow.SetActive(false);
        m_QuartzSellObj.SetActive(false);
        m_QuartzClassTitle.SetActive(false);


        m_QuartzSlotTypeObject.SetActive(false);
        //m_QuartzListGrid.transform.localPosition = QuartzListWindowPos_Class;
        m_QuartzListGrid.transform.localPosition = QuartzListWindowPos_SlotType;
        m_QuartzListPanel.baseClipRegion = QuartzListPanelRange_SlotType;

        UpdateTutorialOnQuartzClassItemClick();
    }

    //从具体类型的星宿 返回到星宿预览
    public void OnReturnClassClick()
    {
        m_ChooseClassId = GlobeVar.INVALID_ID;
        m_ChooseQuartzGuid = GlobeVar.INVALID_GUID;
        ReSetStatus();
        m_QuartzTab.gameObject.SetActive(true);
        m_ReturnQuartzClass.SetActive(false);

        m_QuartzListWindow.SetActive(false);

        m_QuartzSetListWindow.SetActive(true);
        InitQuartzSetList();
        m_QuartzSlotTypeObject.SetActive(false);
        m_NoQuartzObject.SetActive(false);

        //隐藏筛选项
        m_OptionSortObj.SetActive(false);

        //显示标题
        m_QuartzClassTitle.SetActive(true);
        OnQuartzTipsCloseClick();
    }

//----------------------------------------------------------筛选、排序 begin---------------------------------------------
    //在焚星界面 切换1/2/3/4/5/6
    public void OnQuartzSellTypeTabChanged(TabButton tab)
    {
        if (m_QuartzTips.IsShow())
        {
            m_QuartzTips.OnCloseClick();
        }

        InitQuartzList_Sell();

        if (QuartzSellWindow.Instance != null)
        {
            QuartzSellWindow.Instance.HandleSellTypeTabChanged();
        }
    }

    void OnDownOrUpChanged(int index)
    {
        if(m_QuartzTab.Index == 1)
        {
            InitQuartzList_Bag();
        }
        else if(m_QuartzTab.Index == 0 && m_ChooseClassId != GlobeVar.INVALID_ID)
        {
            InitQuartzList_Class();
        }
    }

    //1 2 3 4 5 6 变化
    public void OnQuartzSlotTypeTabChanged(TabButton tab)
    {
        
        //位置下
        if(m_QuartzTab.Index == 1)
        {
            InitQuartzList_Bag();

            if (m_QuartzSortOption.IsOptionOpen())
            {
                m_QuartzSortOption.CloseOption();
            }
        }
        //取消选中
        if(m_bNeedCloseTips)
        {
            m_QuartzTips.OnCloseClick();
        }
    }

    //选择一个排序规则后调用
    void OnQuartzSortOptionChoose(OptionItem item)
    {
        if (item == null || item.name == null)
        {
            return;
        }
        if (m_QuartzTips.IsShow())
        {
            m_QuartzTips.OnCloseClick();
        }
        //策划需求：当选择“强化等级”，“最近获得”两个档位时，不显示星级筛选
        if (!item.name.Equals("Option1-Star"))
        {
            SetRareTab(-1);      //默认选到全部
        }
        if (m_ShowQuartzBag)
        {
            InitQuartzList_Bag();
        }
        else
        {
            InitQuartzList_Class();
        }
        OnQuartzTipsCloseClick();
    }

    //选择一个稀有度后被调用
    void OnQuartzCareOptionChoose(OptionItem item)
    {
        int nStar = -1;
        if (int.TryParse(m_QuartzStarFilterTab.GetCurOptionName(), out nStar))
        {
            m_Star_CurChoose = nStar;
        }
        else
        {
            m_Star_CurChoose = GlobeVar.INVALID_ID;
        }

        if (m_QuartzTips.IsShow())
        {
            m_QuartzTips.OnCloseClick();
        }
        if (m_QuartzStarFilterTab.IsOptionOpen())
        {
            m_QuartzStarFilterTab.CloseOption();
        }

        if (m_ShowQuartzBag)
        {
            InitQuartzList_Bag();
        }
        else
        {
            InitQuartzList_Class();
        }
        OnQuartzTipsCloseClick();
    }
    //----------------------------------------------------------筛选、排序 end---------------------------------------------

    //--------------------------------------------------------------刷显示--------------------------------------
    void InitQuartzSetList()
    {
        m_ShowQuartzBag = false;
        m_QuartzClassList.Clear();
        
        //如果玩家选择全部

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
            int count = 0;
            //如果当前没有选中槽
            if (QuartzEquipWindow.Instance == null || QuartzEquipWindow.Instance.CurChooseSlot == GlobeVar.INVALID_ID)
            {
                count = GameManager.PlayerDataPool.PlayerQuartzBag.GetQartzCountByClassId(tSet.NeedClassId);
            }
            //根据玩家选择的槽 做出操作
            else
            {
                count = GameManager.PlayerDataPool.PlayerQuartzBag.GetQartzCountByClassIdAndSlot
                    (tSet.NeedClassId, QuartzEquipWindow.Instance.CurChooseSlot);
            }
            m_QuartzClassList.Add(new QuartzClass(tSet.NeedClassId, count));
        }

        m_QuartzClassList.Sort(QuartzTool.QuartzClassLstSort);

        if (m_TutorialGroupOnShow == TutorialGroup.QuartzEquip && m_TutorialStepOnShow == 5)
        {
            QuartzClass tutorialQuartzClass = new QuartzClass();
            for (int i = 0; i < m_QuartzClassList.Count; i++)
            {
                if (m_QuartzClassList[i].m_ClassId == GlobeVar.TutorialQuartzEquip_JiaomujiaoID)
                {
                    tutorialQuartzClass = m_QuartzClassList[i];
                    m_QuartzClassList.RemoveAt(i);
                    break;
                }
            }
            if (tutorialQuartzClass.m_ClassId != GlobeVar.INVALID_ID)
            {
                //将角木蛟放到第一位
                m_QuartzClassList.Insert(0, tutorialQuartzClass);
            }
        }
        m_QuartzSetListWrap.InitList(m_QuartzClassList.Count, OnUpdateQuartzSetItem);
    }

    /// <summary>
    /// Set初始化
    /// </summary>
    void InitQuartzList_Class()
    {
        m_QuartzStarFilterTab.gameObject.SetActive("Option1-Star".Equals(m_QuartzSortOption.GetCurOptionName()) ? true : false);

        //如果玩家未对槽进行过滤
        if (QuartzEquipWindow.Instance == null || QuartzEquipWindow.Instance.CurChooseSlot == GlobeVar.INVALID_ID)
        {
            //检测玩家是否对星级进行过滤
            if (m_Star_CurChoose == GlobeVar.INVALID_ID)
            {
                m_QuartzList = GameManager.PlayerDataPool.PlayerQuartzBag.QuartzFilter(m_ChooseClassId);    //仅筛选类别
            }
            //筛选星级和类别
            else
            {
                m_QuartzList = GameManager.PlayerDataPool.PlayerQuartzBag.QuartzFilter((Quartz quartz) =>
                {
                    return quartz.Star == m_Star_CurChoose
                        && quartz.GetClassId() == m_ChooseClassId;                 //对星级和槽进行筛选
                });
            }

        }
        //玩家想对槽进行筛选
        else
        {
            //如果玩家不对 星级进行筛选
            if (m_Star_CurChoose == GlobeVar.INVALID_ID)
            {
                m_QuartzList = GameManager.PlayerDataPool.PlayerQuartzBag.QuartzFilter((Quartz quartz) =>
                {
                    return quartz.GetSlotType() == QuartzEquipWindow.Instance.CurChooseSlot
                        && quartz.GetClassId() == m_ChooseClassId;                 //对槽和类别进行筛选
                });
            }
            //筛选星级、类别、槽
            else
            {
                m_QuartzList = GameManager.PlayerDataPool.PlayerQuartzBag.QuartzFilter((Quartz quartz) =>
                {
                    return quartz.Star == m_Star_CurChoose
                        && quartz.GetClassId() == m_ChooseClassId
                        && quartz.GetSlotType() == QuartzEquipWindow.Instance.CurChooseSlot;
                });
            }
        }

        if (m_DownUpSortTab.Index == 0)
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
        m_QuartzListWrap.InitList(m_QuartzList.Count, OnUpdateQuartzItem);
        m_QuartzListScroll.ResetPosition();
        m_QuartzTabCountLabel.text = m_QuartzList.Count.ToString();

        m_NoQuartzObject.SetActive(m_QuartzList.Count <= 0);


        m_QuartzListWrap.InitList(m_QuartzList.Count, OnUpdateQuartzItem);
        m_QuartzListScroll.ResetPosition();
        m_OptionGrid.Reposition();         //筛选项重排序

        //根据当前Class类型 显示正确的指示

        m_ShowQuartzBag = false;
        m_ShowSellList = false;
    }

    /// <summary>
    /// 根据当前筛选项、排序规则刷新当前星宿背包显示
    /// </summary>
    void InitQuartzList_Bag()
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
        m_QuartzStarFilterTab.gameObject.SetActive("Option1-Star".Equals(m_QuartzSortOption.GetCurOptionName()) ? true : false);

        //拿到当前指定的槽的星魂（根据当前筛选规则，拿到选项内的星魂）
        if (m_Star_CurChoose != GlobeVar.INVALID_ID)
        {
            m_QuartzList = GameManager.PlayerDataPool.PlayerQuartzBag.QuartzFilter((Quartz quartz) =>
            {
                return quartz.Star == m_Star_CurChoose
                    && quartz.GetSlotType() == nTabSlotType;                 //对星级和槽进行筛选
            });
        }
        else
        {
            m_QuartzList = GameManager.PlayerDataPool.PlayerQuartzBag.QuartzFilter((Quartz quartz) =>
            {
                return quartz.GetSlotType() == nTabSlotType;                 //仅筛选槽
            });
        }

        if (m_DownUpSortTab.Index == 0)
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

        m_QuartzListWrap.InitList(m_QuartzList.Count, OnUpdateQuartzItem);
        m_QuartzListScroll.ResetPosition();
        m_QuartzTabCountLabel.text = m_QuartzList.Count.ToString();

        m_NoQuartzObject.SetActive(m_QuartzList.Count <= 0);
        m_OptionGrid.Reposition();         //筛选项重排序
        m_ShowQuartzBag = true;
        m_ShowSellList = false;
    }

    /// <summary>
    /// 初始化 焚星列表
    /// </summary>
    void InitQuartzList_Sell()
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
        m_QuartzListWrap.InitList(m_QuartzList.Count, OnUpdateQuartzItem);
        m_QuartzListScroll.ResetPosition();
        m_QuartzTabCountLabel.text = m_QuartzList.Count.ToString();

        m_NoQuartzObject.SetActive(m_QuartzList.Count <= 0);

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

        if (TutorialRoot.IsGroupStep(TutorialGroup.QuartzEquip, 6))
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
                m_SellGuidList.Add(item.GetGuid());
                m_QuartzTips.Show(item.Quartz, QuartzTipsWindow.TipsType.Info);
            }

            item.UpdateSellChoose(m_SellGuidList.Contains(item.GetGuid()));

            if (QuartzSellWindow.Instance != null)
            {
                QuartzSellWindow.Instance.HandleQuartzItemClick();
            }
        }

        if (m_QuartzSortOption.IsOptionOpen())
        {
            m_QuartzSortOption.CloseOption();
        }
    }

    //点击了不同的槽位
    public void HandleOrbmentSlotItemClick_Diff(OrbmentSlotItem slotItem,int index)
    {
        if (slotItem == null)
        {
            return;
        }

        //这个槽没有星魂
        if (slotItem.Quartz == null || false == slotItem.Quartz.IsValid())
        {
            m_QuartzTips.gameObject.SetActive(false);           //点选后需要视为当前选中，但是为空的话 不应该显示Tips
            m_QuartzSlot_CurChoose = slotItem.SlotType;
        }
        else
        {
            // 有星魂 弹tips
            m_QuartzTips.Show(slotItem.Quartz, QuartzTipsWindow.TipsType.Equiped, m_Card);
        }
        if (m_ShowSellList)
        {

        }
        //如果当前还在选择class
        else if(m_QuartzTab.Index == 0 && m_ChooseClassId == GlobeVar.INVALID_ID)
        {
            InitQuartzSetList();
        }
        //如果当前在查看该类型的星魂
        else if (m_QuartzTab.Index == 0)
        {
            InitQuartzList_Class();
        }
        //如果在按位置查看
        else if(m_QuartzTab.Index == 1)
        {
            //如果该槽为空，需要关闭tips
            InitQuartzList_Bag();
            SetSlotTab(index,false);          //选中该槽（按点击槽位定tab）
        }
    }

    //点击了相同的槽位
    public void HandleOrbmentSlotItemClick_Same(OrbmentSlotItem slotItem,int index)
    {
        if(m_ShowSellList)
        {

        }
        //如果当前还在选择class
        else if (m_QuartzTab.Index == 0 && m_ChooseClassId == GlobeVar.INVALID_ID)
        {
            //做反勾选处理
            m_QuartzTips.OnCloseClick();
        }
        else if(m_QuartzTab.Index == 0)
        {
            //做反勾选处理
            m_QuartzTips.OnCloseClick();
        }
        //pos界面不对点击相同槽做处理
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
        //如果在焚星，不做处理
        if (m_ShowSellList)
        {

        }
        //如果还在选星魂Set
        else if(m_QuartzTab.Index == 0 && m_ChooseClassId == GlobeVar.INVALID_ID)
        {
            InitQuartzSetList();
        }
        else if(m_QuartzTab.Index == 0)
        {
            //只刷界面
            InitQuartzList_Class();
        }
        else if(m_QuartzTab.Index == 1)
        {
            InitQuartzList_Bag();
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


    void InitQuartzWholeCount()
    {
        m_QuartzWholeCountLabel.text = string.Format("{0}/{1}",
            GameManager.PlayerDataPool.PlayerQuartzBag.QuartzList.Count, GlobeVar.QUARTZBAG_SIZE);
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
        }
        else
        {
            //全选
            m_SellGuidList.Clear();
            for (int i = 0; i < m_QuartzList.Count; i++)
            {
                m_SellGuidList.Add(m_QuartzList[i].Guid);
            }

            m_IsChooseAll = true;
        }

        m_QuartzListWrap.UpdateAllItem();

        //更新价格
        if (QuartzSellWindow.Instance != null)
        {
            QuartzSellWindow.Instance.HandleQuartzItemClick();
        }
    }

    public void OnClickStrengthenPlus()
    {

        MessageBoxController.OpenOKCancel(7358, 7357, () =>
        {
            m_QuartzOpTab.ChangeTab("Tab02-Sell");
        }, null, 7360, 7359);
    }

    //tt14464 由于ui栈，商店界面会回到星魂界面，不会直接返回到卡包界面，不用特殊处理
    //public void OnClickShop()
    //{
    //    ShopController.OpenCommonShopBindYuanbaoPage(OnShopClose);
    //}

    //private static void OnShopClose(object userdata)
    //{
    //    if(CardBagController.Instance != null)
    //    {
    //        CardBagController.Instance.OrbmentShopCloseCallBack();
    //    }
    //}

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

        if (m_TutorialGroupOnShow == TutorialGroup.QuartzEquip && m_TutorialStepOnShow == 5)
        {
            TutorialRoot.ShowTutorial(TutorialGroup.QuartzEquip, 5);
        }
        else if (m_TutorialGroupOnShow == TutorialGroup.QuartzStrengthen && m_TutorialStepOnShow == 4)
        {
            TutorialRoot.ShowTutorial(TutorialGroup.QuartzStrengthen, 4, m_FirstClassItem, ClassItemWidth, ClassItemHeight);
        }

        m_TutorialGroupOnShow = TutorialGroup.Invalid;
        m_TutorialStepOnShow = GlobeVar.INVALID_ID;
    }

    void UpdateTutorialOnQuartzClassItemClick()
    {
        if (TutorialRoot.IsGroupStep(TutorialGroup.QuartzStrengthen, 4))
        {
            TutorialRoot.ShowTutorial(TutorialGroup.QuartzStrengthen, 5, m_QuartzListWidget.gameObject, m_QuartzListWidget.width, m_QuartzListWidget.height);
        }
        else if (TutorialRoot.IsGroupStep(TutorialGroup.QuartzEquip, 6))
        {
            TutorialRoot.ShowTutorial(TutorialGroup.QuartzEquip, 7);
        }

    }

    public void UpdateTutorialOnTutorialBottomClick()
    {

        if (TutorialRoot.IsGroupStep(TutorialGroup.QuartzEquip, 5))
        {
            if (m_FirstClassItem != null)
            {
                TutorialRoot.ShowTutorial(TutorialGroup.QuartzEquip, 6, m_FirstClassItem, ClassItemWidth, ClassItemHeight);
            }
            else
            {
                TutorialRoot.TutorialOver();
            }
        }
        else if (TutorialRoot.IsGroupStep(TutorialGroup.QuartzEquip, 7))
        {
            if (m_FirstQuartzItemLogic != null)
            {
                TutorialRoot.ShowTutorial(TutorialGroup.QuartzEquip, 8, m_FirstQuartzItemLogic.gameObject, m_FirstQuartzItemLogic.m_QuartzIcon.width,
              m_FirstQuartzItemLogic.m_QuartzIcon.height);
            }
            else
            {
                TutorialRoot.TutorialOver();
            }

        }
        else if (TutorialRoot.IsGroupStep(TutorialGroup.QuartzEquip, 10))
        {
            TutorialRoot.TutorialOver();
        }
    }
}
