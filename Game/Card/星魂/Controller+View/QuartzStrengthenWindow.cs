using UnityEngine;
using System.Collections.Generic;
using Games.GlobeDefine;
using Games.Table;
using Games;
using ProtobufPacket;

//强化界面
public class QuartzStrengthenWindow : MonoBehaviour
{
    private const string CannotUse_SpriteName = "CommonNew_Buttom_6";
    private const string CanUse_SpriteName = "CommonNew_Buttom_7";

    private static QuartzStrengthenWindow m_Instance = null;
    public static QuartzStrengthenWindow Instance
    {
        get { return m_Instance; }
    }

    public UILabel m_ClassNameLabel;
    public UILabel m_StrengthenLabel;
    public UITexture m_ClassTexture;
    public UISprite m_SlotIcon;
    public UILabel m_MainAttrLabel;
    public UILabel[] m_AttachAttrLabel;
    public UILabel m_NextMainLabel;
    public UILabel m_NeedCoinLabel;
    public UILabel m_NeedStoneLabel;
    public GameObject m_SuccessEffect;
    public GameObject m_FailEffect;

    public UIEventTrigger m_ToggleTrigger;
    public UISprite m_ToggleSprite;

    //强化相关
    public UIEventTrigger m_StrengthenBtn;           //选择强化
    public UIEventTrigger m_Strengthen_Ten_Btn;      //十连强化
    public UILabel m_PercentLabel;                   //成功率
    public GameObject m_MaxObject;                   //当前等级满
    public GameObject m_UseStoneObj;                 //使用强化石的Obj
    public GameObject m_NeedCoinObj;                 //所需金币的Obj
    public UILabel m_Strengthen_Ten_Label;           //十连强化btn所显示文字
    public UISprite m_StrengthenTexture;
    public UILabel m_CoinLabel;                     //金币
    public UILabel m_StoneLabel;                    //强化石

    //data
    private Quartz m_Quartz = null;
    private ulong m_CardGuid = GlobeVar.INVALID_GUID;
    private bool m_bIsUseStone = false;                   //当前是否使用强化石
    private int m_StrengCount = GlobeVar.INVALID_ID;      //剩余强化次数 用于10连强化
    private Time m_LastClickStreng;

    public static TutorialGroup m_TutorialGroupOnShow = TutorialGroup.Invalid;
    public static int m_TutorialStepOnShow = GlobeVar.INVALID_ID;

    ParticleSystem[] m_SuccessParticle;
    ParticleSystem[] m_FailParticle; 
    void Awake()
    {
        m_Instance = this;
        PlayerData.delegateGoldCoinChanged += UpdateCoinLabel;
        PlayerData.delegateCommonPackItemChanged += UpdateStrengthenStoneLabel;  
    }
    void Start()
    {
        m_ToggleTrigger.onClick.Add(new EventDelegate(OnClickToggleUseStone));
        m_StrengthenBtn.onClick.Add(new EventDelegate(OnClickStreng));
        m_Strengthen_Ten_Btn.onClick.Add(new EventDelegate(OnClickStreng_Ten));
        UIRechargeTipEx.OnCacncel += StopStreng_Ten;
        m_SuccessParticle = m_SuccessEffect.GetComponentsInChildren<ParticleSystem>();
        m_FailParticle = m_FailEffect.GetComponentsInChildren<ParticleSystem>();
    }
    void OnEnable()
    {
        if(OrbmentController.Instance!=null)
        {
            OrbmentController.Instance.HandleStrengthenOpen();
        }
        m_SuccessEffect.SetActive(false);
        m_FailEffect.SetActive(false);
        m_bIsUseStone = false;
        m_StrengCount = GlobeVar.INVALID_ID;
        StopStreng_Ten();
        UpdateCoinLabel();
        UpdateStrengthenStoneLabel();
    }
    void OnDisable()
    {
        if (OrbmentController.Instance != null)
        {
            OrbmentController.Instance.HandleStrengthenClose();
        }
    }

    void OnDestroy()
    {
        m_Instance = null;

        m_Quartz = null;
        m_CardGuid = GlobeVar.INVALID_GUID;
        UIRechargeTipEx.OnCacncel -= StopStreng_Ten;

        PlayerData.delegateGoldCoinChanged -= UpdateCoinLabel;
        PlayerData.delegateCommonPackItemChanged -= UpdateStrengthenStoneLabel;
    }

    /// <summary>
    /// 展示强化界面UI
    /// </summary>
    /// <param name="quartzGuid">星魂GUID</param>
    /// <param name="cardGuid">卡牌GUID</param>
    public static void ShowStrengthen(ulong quartzGuid, ulong cardGuid)
    {
        List<object> param = new List<object>();
        param.Add(quartzGuid);
        param.Add(cardGuid);

        UIManager.ShowUI(UIInfo.QuartzStrengthen, OnOpenQuartzStrengthen, param);
    }

    //打开强化界面后调用
    private static void OnOpenQuartzStrengthen(bool bSuccess, object param)
    {
        if (bSuccess == false)
        {
            return;
        }

        if (m_Instance == null)
        {
            return;
        }

        List<object> Params = param as List<object>;
        if (Params == null || Params.Count < 2)
        {
            return;
        }

        ulong quartzGuid = (ulong)Params[0];
        m_Instance.m_CardGuid = (ulong)Params[1];

        if (m_Instance.m_CardGuid != GlobeVar.INVALID_GUID)
        {
            Card card = GameManager.PlayerDataPool.PlayerCardBag.GetCard(m_Instance.m_CardGuid);
            if (card != null && card.Orbment != null)
            {
                m_Instance.m_Quartz = card.Orbment.GetQuartz(quartzGuid);
            }
        }
        else
        {
            m_Instance.m_Quartz = GameManager.PlayerDataPool.PlayerQuartzBag.GetQuartz(quartzGuid);
        }

        // 加固判断 防止显示异常界面
        if (m_Instance.m_Quartz == null)
        {
            UIManager.CloseUI(UIInfo.QuartzStrengthen);
            return;
        }

        m_Instance.InitWindow();
        m_Instance.UpdateTutorialOnShow();
    }

    //初始化强化界面
    void InitWindow()
    {
        if (m_Quartz == null)
        {
            return;
        }

        Tab_QuartzClass tClass = TableManager.GetQuartzClassByID(m_Quartz.GetClassId(), 0);
        if (tClass == null)
        {
            return;
        }

        m_MainAttrLabel.text = "";
        for (int i = 0; i < m_AttachAttrLabel.Length; i++)
        {
            m_AttachAttrLabel[i].text = "";
        }
        m_NextMainLabel.text = "";
        //激活默认游戏物体
        m_StrengthenBtn.gameObject.SetActive(true);
        m_Strengthen_Ten_Btn.gameObject.SetActive(true);
        m_PercentLabel.gameObject.SetActive(true);
        m_UseStoneObj.SetActive(true);
        m_NeedCoinObj.SetActive(true);
        m_NextMainLabel.gameObject.SetActive(true);
        m_Strengthen_Ten_Label.text = StrDictionary.GetDicByID(7773);
        //赋值 
        m_ClassNameLabel.text = tClass.Name;
        m_StrengthenLabel.text = m_Quartz.Strengthen > 0 ? string.Format("+{0}", m_Quartz.Strengthen) : "";
        m_ClassTexture.mainTexture = Utils.LoadTexture("Texture/T_StarSoul/" + tClass.Pic);
        m_SlotIcon.spriteName = QuartzTool.GetQuartzSlotTypeIcon(m_Quartz.GetSlotType());


        //显示主属性
        if (m_Quartz.MainAttr != null && m_Quartz.MainAttr.RefixType != GlobeVar.INVALID_ID)
        {
            m_MainAttrLabel.text += Utils.GetAttrRefixName((AttrRefixType)m_Quartz.MainAttr.RefixType);

            if (m_Quartz.MainAttr.RefixType == (int)AttrRefixType.MaxHPPercent || m_Quartz.MainAttr.RefixType == (int)AttrRefixType.MaxHPFinal ||
                m_Quartz.MainAttr.RefixType == (int)AttrRefixType.AttackPercent || m_Quartz.MainAttr.RefixType == (int)AttrRefixType.AttackFinal ||
                m_Quartz.MainAttr.RefixType == (int)AttrRefixType.DefensePercent || m_Quartz.MainAttr.RefixType == (int)AttrRefixType.DefenseFinal ||
                m_Quartz.MainAttr.RefixType == (int)AttrRefixType.SpeedPercent || m_Quartz.MainAttr.RefixType == (int)AttrRefixType.SpeedFinal)
            {
                m_MainAttrLabel.text += string.Format(" +{0}%", (int)(m_Quartz.MainAttr.AttrValue / 100.0f));
            }
            else
            {
                if (m_Quartz.MainAttr.RefixType == (int)AttrRefixType.CritChanceAdd ||
                    m_Quartz.MainAttr.RefixType == (int)AttrRefixType.CritEffectAdd ||
                    m_Quartz.MainAttr.RefixType == (int)AttrRefixType.ImpactChanceAdd ||
                    m_Quartz.MainAttr.RefixType == (int)AttrRefixType.ImpactResistAdd)
                {
                    m_MainAttrLabel.text += string.Format(" +{0}%", (int)(m_Quartz.MainAttr.AttrValue / 100.0f));
                }
                else
                {
                    m_MainAttrLabel.text += string.Format(" +{0}", m_Quartz.MainAttr.AttrValue);
                }
            }

            int nStrengthenValue = m_Quartz.GetMainAttrStrengthenValue();
            string szStrengthenValue = "";
            if (m_Quartz.MainAttr.RefixType == (int)AttrRefixType.MaxHPPercent ||
                m_Quartz.MainAttr.RefixType == (int)AttrRefixType.MaxHPFinal ||
                m_Quartz.MainAttr.RefixType == (int)AttrRefixType.AttackPercent ||
                m_Quartz.MainAttr.RefixType == (int)AttrRefixType.AttackFinal ||
                m_Quartz.MainAttr.RefixType == (int)AttrRefixType.DefensePercent ||
                m_Quartz.MainAttr.RefixType == (int)AttrRefixType.DefenseFinal ||
                m_Quartz.MainAttr.RefixType == (int)AttrRefixType.SpeedPercent ||
                m_Quartz.MainAttr.RefixType == (int)AttrRefixType.SpeedFinal)
            {
                szStrengthenValue = string.Format("{0}%", (int)(nStrengthenValue / 100.0f));
            }
            else
            {
                if (m_Quartz.MainAttr.RefixType == (int)AttrRefixType.CritChanceAdd ||
                    m_Quartz.MainAttr.RefixType == (int)AttrRefixType.CritEffectAdd ||
                    m_Quartz.MainAttr.RefixType == (int)AttrRefixType.ImpactChanceAdd ||
                    m_Quartz.MainAttr.RefixType == (int)AttrRefixType.ImpactResistAdd)
                {
                    szStrengthenValue = string.Format("{0}%", (int)(nStrengthenValue / 100.0f));
                }
                else
                {
                    szStrengthenValue = nStrengthenValue.ToString();
                }
            }

            m_NextMainLabel.text = StrDictionary.GetClientDictionaryString("#{5368}", szStrengthenValue, Utils.GetAttrRefixName((AttrRefixType)m_Quartz.MainAttr.RefixType));
        }

        //显示附加属性
        if (m_Quartz.AttachAttr != null)
        {
            for (int i = 0; i < m_Quartz.AttachAttr.Length && i < m_AttachAttrLabel.Length; i++)
            {
                if (m_Quartz.AttachAttr[i] == null)
                {
                    continue;
                }

                if (m_Quartz.AttachAttr[i].RefixType == GlobeVar.INVALID_ID)
                {
                    continue;
                }

                m_AttachAttrLabel[i].text += Utils.GetAttrRefixName((AttrRefixType)m_Quartz.AttachAttr[i].RefixType);

                if (m_Quartz.AttachAttr[i].RefixType == (int)AttrRefixType.MaxHPPercent || m_Quartz.AttachAttr[i].RefixType == (int)AttrRefixType.MaxHPFinal ||
                    m_Quartz.AttachAttr[i].RefixType == (int)AttrRefixType.AttackPercent || m_Quartz.AttachAttr[i].RefixType == (int)AttrRefixType.AttackFinal ||
                    m_Quartz.AttachAttr[i].RefixType == (int)AttrRefixType.DefensePercent || m_Quartz.AttachAttr[i].RefixType == (int)AttrRefixType.DefenseFinal ||
                    m_Quartz.AttachAttr[i].RefixType == (int)AttrRefixType.SpeedPercent || m_Quartz.AttachAttr[i].RefixType == (int)AttrRefixType.SpeedFinal)
                {
                    m_AttachAttrLabel[i].text += string.Format(" +{0}%", (int)(m_Quartz.AttachAttr[i].AttrValue / 100.0f));
                }
                else
                {
                    if (m_Quartz.AttachAttr[i].RefixType == (int)AttrRefixType.CritChanceAdd ||
                        m_Quartz.AttachAttr[i].RefixType == (int)AttrRefixType.CritEffectAdd ||
                        m_Quartz.AttachAttr[i].RefixType == (int)AttrRefixType.ImpactChanceAdd ||
                        m_Quartz.AttachAttr[i].RefixType == (int)AttrRefixType.ImpactResistAdd)
                    {
                        m_AttachAttrLabel[i].text += string.Format(" +{0}%", (int)(m_Quartz.AttachAttr[i].AttrValue / 100.0f));
                    }
                    else
                    {
                        m_AttachAttrLabel[i].text += string.Format(" +{0}", m_Quartz.AttachAttr[i].AttrValue);
                    }
                }
            }
        }

        //如果强化等级已经是最大
        if (m_Quartz.Strengthen >= GlobeVar.QUARTZ_STRENGTHEN_MAX)
        {
            m_MaxObject.SetActive(true);
            //隐藏强化按钮
            m_StrengthenBtn.gameObject.SetActive(false);
            m_Strengthen_Ten_Btn.gameObject.SetActive(false);
            //隐藏概率、勾选强化石、金币
            m_PercentLabel.gameObject.SetActive(false);
            m_UseStoneObj.SetActive(false);
            m_NeedCoinObj.SetActive(false);
            m_NextMainLabel.gameObject.SetActive(false);
        }

        else
        {
            m_MaxObject.SetActive(false);
            //计算所需强化石个数
            Tab_QuartzStrengthen tStrengthen = TableManager.GetQuartzStrengthenByID(m_Quartz.Strengthen + 1, 0);
            if (tStrengthen != null)
            {
                m_NeedCoinLabel.text = Utils.GetPriceStr((int)SpecialItemID.Gold, tStrengthen.GetNeedMoneybyIndex(m_Quartz.Star - 1));   //所需金币
                m_NeedStoneLabel.text = StrDictionary.GetDicByID(7765, tStrengthen.StoneCount.ToString());                                 //所需强化石
            }
            //展示强化概率
            ShowPercent();
        }
        //是否处于10连强化
        if(m_StrengCount>=1)
        {
            m_Strengthen_Ten_Label.text = StrDictionary.GetDicByID(7767, m_StrengCount.ToString());
            m_StrengthenTexture.spriteName = CannotUse_SpriteName;
        }
        else
        {
            m_StrengthenTexture.spriteName = CanUse_SpriteName;
        }
    }

    //勾选或反勾选强化石
    void OnClickToggleUseStone()
    {
        m_bIsUseStone = !m_bIsUseStone;
        if (m_bIsUseStone)
        {
            m_ToggleSprite.gameObject.SetActive(true);
        }
        else
        {
            m_ToggleSprite.gameObject.SetActive(false);
        }
        ShowPercent();
    }

    //点击十连强化
    void OnClickStreng_Ten()
    {
        if(m_StrengCount >0)
        {
            //先终止再说
            StopStreng_Ten();
        }
        else if (m_StrengCount <= 0)                //不处于10连强化 则开启10连强化
        {
            StopStreng_Ten();
            m_StrengCount = 10;
            InitWindow();
            m_StrengCount--;
            Streng();
        }
    }
    //点击强化
    void OnClickStreng()
    {
        //正在进行十连强化 直接打回去
        if(m_StrengCount > 0)
        {
            Utils.CenterNotice(7802);
            return;
        }
        StopStreng_Ten();
        //进行单次强化
        Streng();
    }

    //现在走统一的强化接口：根据当前是否勾选强化石 决定执行哪个分支 
    void Streng()
    {
        if (m_bIsUseStone)
        {
            OnStoneStrengthenClick();
        }
        else
        {
            OnStrengthenClick();
        }
    }

    //根据当前是否勾选强化石 显示强化概率
    void ShowPercent()
    {
        Tab_QuartzStrengthenPercent tPercent = TableManager.GetQuartzStrengthenPercentByID(m_Quartz.Strengthen + 1, 0);
        if (tPercent != null)
        {
            if (m_bIsUseStone)
            {
                int nStonePercent = (tPercent.ChancePercent + tPercent.StoneChancePercent) > 100 ? 100 : tPercent.ChancePercent + tPercent.StoneChancePercent;
                m_PercentLabel.text = StrDictionary.GetDicByID(6294, nStonePercent);
            }
            else
            {
                m_PercentLabel.text = StrDictionary.GetDicByID(6294, tPercent.ChancePercent);
            }
        }
    }

    //普通强化分支 检查
    private void OnStrengthenClick()
    {
        if (TutorialRoot.IsGroupStep(TutorialGroup.QuartzStrengthen, 7))
        {
            TutorialRoot.TutorialOver();
        }

        if (m_Quartz == null)
        {
            StopStreng_Ten();
            return;
        }

        Tab_QuartzStrengthen tStrengthen = TableManager.GetQuartzStrengthenByID(m_Quartz.Strengthen + 1, 0);
        if (tStrengthen == null)
        {
            StopStreng_Ten();
            return;
        }
        int nNeedMoney = tStrengthen.GetNeedMoneybyIndex(m_Quartz.Star - 1);
        if (nNeedMoney <= 0)
        {
            StopStreng_Ten();
            return;
        }
        UIRechargeTipEx.ShowTips(nNeedMoney, DoStreng);
    }

    //使用强化石强化 检查
    private void OnStoneStrengthenClick()
    {
        if (m_Quartz == null)
        {
            return;
        }
        Tab_QuartzStrengthen tStrengthen = TableManager.GetQuartzStrengthenByID(m_Quartz.Strengthen + 1, 0);
        if (tStrengthen == null)
        {
            StopStreng_Ten();
            return;
        }

        if (TutorialRoot.IsGroupStep(TutorialGroup.QuartzStrengthen, 7))
        {
            if (GameManager.PlayerDataPool.GetGold() < tStrengthen.GetNeedMoneybyIndex(m_Quartz.Star - 1))
            {
                StopStreng_Ten();
                Utils.CenterNotice(5169);
                return;
            }

            if (GameManager.PlayerDataPool.CommonPack.GetItemCountByDataId(GlobeVar.ORBMENT_ITEMID_STRHENGTHENSTONE) < tStrengthen.StoneCount)
            {
                StopStreng_Ten();
                Utils.CenterNotice(5170);
                return;
            }

            TutorialRoot.TutorialOver();
        }

        int nNeedMoney = tStrengthen.GetNeedMoneybyIndex(m_Quartz.Star - 1);
        if (nNeedMoney <= 0)
        {
            StopStreng_Ten();
            return;
        }

        UIRechargeTipEx.ShowTips(nNeedMoney, DoStoneStreng);
    }

    //不使用强化石进行强化
    private void DoStreng(COST_TYPE eCost)
    {
        StartCoroutine(SendDoStrengPackage(false, eCost, m_Quartz, m_CardGuid));
    }

    //使用强化石进行强化
    private void DoStoneStreng(COST_TYPE eCost)
    {
        StartCoroutine(SendDoStrengPackage(true, eCost, m_Quartz, m_CardGuid));
    }

    //使用播放动画前的数据，防止一些未知问题
    System.Collections.IEnumerator SendDoStrengPackage(bool isUseStone, COST_TYPE eCost, Quartz quartz, ulong cardGuid)
    {       
        //判断实际所需资源够不够 不够直接打回去 不发包
        Tab_QuartzStrengthen tStrengthen = TableManager.GetQuartzStrengthenByID(quartz.Strengthen + 1, 0);
        if (tStrengthen!=null)
        {
            if (tStrengthen.HeroLevel > GameManager.PlayerDataPool.PlayerHeroData.Level)
            {
                StopStreng_Ten();
                string tip = StrDictionary.GetDicByID(7287, tStrengthen.HeroLevel);
                Utils.CenterNotice(tip);
                yield break;
            }
            int needCoin;
            switch(eCost)
            {
                case COST_TYPE.GoldCoin:
                    if (GameManager.PlayerDataPool.GetGold() < tStrengthen.GetNeedMoneybyIndex(m_Quartz.Star - 1))
                    {
                        StopStreng_Ten();
                        Utils.CenterNotice(5169);
                        yield break;
                    }
                    break;
                case COST_TYPE.GoldCoinAndBindYuanbao:
                    needCoin = tStrengthen.GetNeedMoneybyIndex(m_Quartz.Star - 1);
                    if (GameManager.PlayerDataPool.GetBindYuanBao() < UIRechargeTipEx.CalYuanBaoNeedEx(needCoin))
                    {
                        StopStreng_Ten();
                        Utils.CenterNotice(5169);
                        yield break;
                    }
                    break;
                case COST_TYPE.GoldCoinAndYuanbao:
                    needCoin = tStrengthen.GetNeedMoneybyIndex(m_Quartz.Star - 1);
                    if (GameManager.PlayerDataPool.GetYuanBao() < UIRechargeTipEx.CalYuanBaoNeedEx(needCoin))
                    {
                        StopStreng_Ten();
                        Utils.CenterNotice(5169);
                        yield break;
                    }
                    break;
            }
            if (isUseStone && GameManager.PlayerDataPool.CommonPack.GetItemCountByDataId(GlobeVar.ORBMENT_ITEMID_STRHENGTHENSTONE) < tStrengthen.StoneCount)
            {
                StopStreng_Ten();
                Utils.CenterNotice(5170);
                yield break;
            }
        }
        //播放等待特效 TODO
        //yield return new WaitForSeconds(1.0f);
        CG_QUARTZ_STRENGTHEN_PAK pak = new CG_QUARTZ_STRENGTHEN_PAK();
        pak.data.QuartzGuid = quartz.Guid;
        pak.data.UseStone = isUseStone;
        if (cardGuid != GlobeVar.INVALID_GUID)
        {
            pak.data.CardGuid = cardGuid;
        }
        pak.data.CostType = (int)eCost;
        pak.SendPacket();
    }

    //点击终止10连强化
    void StopStreng_Ten()
    {
        StopAllCoroutines();
        m_StrengCount = GlobeVar.INVALID_ID;
        InitWindow();    //刷新一下显示
    }


    public void OnCloseClick()
    {
        UIManager.CloseUI(UIInfo.QuartzStrengthen);
    }

    //强化后刷新界面
    public void HandleQuartzStrengthen(bool bSuccess)
    {
        InitWindow();
        StartCoroutine(ShowStrengEff(bSuccess));
    }
    System.Collections.IEnumerator ShowStrengEff(bool bSuccess)
    {
        ////等待前一次播放结束再处理新的请求
        //if(m_FailParticle!=null && m_SuccessParticle!=null)
        //{
        //    //检查所有粒子是否结束
        //    if(m_FailEffect.activeSelf)
        //    {
        //        for (int i = 0; i < m_FailParticle.Length; )
        //        {
        //            if (m_FailParticle[i].isPlaying)
        //            {
        //                yield return new WaitForSeconds(0.05f);
        //            }
        //            else
        //            {
        //                i++;
        //            }
        //        }
        //    }
        //    if(m_SuccessEffect.activeSelf)
        //    {
        //        for (int i = 0; i < m_SuccessParticle.Length; i++)
        //        {
        //            if (m_SuccessParticle[i].isStopped)
        //            {
        //                i++;
        //            }
        //            else
        //            {
        //                yield return new WaitForSeconds(0.05f);
        //            }
        //        }
        //    }
        //}
        if (bSuccess)
        {
            m_SuccessEffect.SetActive(false);
            m_SuccessEffect.SetActive(true);


            GameManager.SoundManager.StopSoundEffect(GlobeVar.QUARTZ_SOUND_STRENGTHENSUCCESS);
            GameManager.SoundManager.PlaySoundEffect(GlobeVar.QUARTZ_SOUND_STRENGTHENSUCCESS);
        }
        else
        {
            m_FailEffect.SetActive(false);
            m_FailEffect.SetActive(true);


            GameManager.SoundManager.StopSoundEffect(GlobeVar.QUARTZ_SOUND_STRENGTHENFAIL);
            GameManager.SoundManager.PlaySoundEffect(GlobeVar.QUARTZ_SOUND_STRENGTHENFAIL);
        }
        //播放动画的时间
        yield return new WaitForSeconds(1.0f);
        //如果还没有强化完，则继续强化
        if (m_StrengCount >= 1)
        {
            m_StrengCount--;
            Streng();
        }
        //如果播放结束，准备关闭
        else
        {
            Invoke("OnEffectOver", 6.0f);
        }
    }

    public void HandleQuartzStrengError(GC_QUARTZ_STRENGTHEN_ERROR pak)
    {
        StopStreng_Ten();
        if(pak.has_dicId && pak.dicId>0)
        {
            Utils.CenterNotice(pak.dicId);
        }

    }
    void OnEffectOver()
    {
        m_SuccessEffect.SetActive(false);
        m_FailEffect.SetActive(false);
    }

    public void UpdateStrengthenStoneLabel()
    {
        m_StoneLabel.text =
            GameManager.PlayerDataPool.CommonPack.GetItemCountByDataId(GlobeVar.ORBMENT_ITEMID_STRHENGTHENSTONE).ToString();
    }

    //更新当前金币显示
    public void UpdateCoinLabel()
    {
        m_CoinLabel.text = ItemIconStr.GetCurrencyStringForShow(GameManager.PlayerDataPool.GetGold());
    }

    //更新新手教程
    void UpdateTutorialOnShow()
    {
        if (m_TutorialGroupOnShow == TutorialGroup.Invalid && m_TutorialStepOnShow == GlobeVar.INVALID_ID)
        {
            return;
        }

        if (m_TutorialGroupOnShow == TutorialGroup.QuartzStrengthen && m_TutorialStepOnShow == 7)
        {
            TutorialRoot.ShowTutorial(TutorialGroup.QuartzStrengthen, 7);
        }

        m_TutorialGroupOnShow = TutorialGroup.Invalid;
        m_TutorialStepOnShow = GlobeVar.INVALID_ID;
    }

    //更新新手教程
    public void UpdateTutorialOnTutorialBottomClick()
    {
        if (TutorialRoot.IsGroupStep(TutorialGroup.QuartzStrengthen, 7))
        {
            TutorialRoot.TutorialOver();
        }
    }
}
