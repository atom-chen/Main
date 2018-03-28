using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Games.Table;
using Games;
using Games.GlobeDefine;

public class CollectionLevel2FulingDetail : MonoBehaviour {
    public GameObject m_PreviewObj;
    public GameObject m_SkillDetail;

    public UISprite m_Quality = null; //品质（稀有度）
    public UILabel m_FulingName = null; //名称
    public UISprite m_Mark = null; //类型
    public UISprite m_EnvTypePic;//人妖衡
    public GameObject m_NotOwn = null; //尚未获得标记
    public UITexture m_Photo = null; //获得时立绘
    public UITexture m_Shadow = null; //未获得时阴影
    public UILabel m_Akira = null; //声优,文字描述
    public GameObject m_AkiraAll = null; // 整个声优组件
    public GameObject m_BackgoundStaticText = null; // 人物背景故事（静态文本）
    public UILabel m_Background = null; //人物背景故事（动态内容）
    public GameObject m_SkillStaticText = null; // 技能（静态文本）
    public UISprite[] m_Skill = null; //技能1
    public UISprite m_Prev = null; // 向前翻页按钮
    public UISprite m_Next = null; // 向后翻页按钮
    public CollectionDetailSkillWindow m_SkillDetailWindow = null; // 技能详细介绍界面

    private List<int> m_ShowCardList = null; // 当前展示的符灵预览列表
    private int m_CurCardId = GlobeVar.INVALID_ID; // 当前展示符灵的id
    private int[] m_InitSkillList = new int[GlobeVar.CARD_SKILLCOUNT]; // 初始技能列表
    private int[] m_AwakeSkillList = new int[GlobeVar.CARD_SKILLCOUNT]; // 觉醒技能列表

    public TweenAlpha m_Tween;


    #region 打开相关方法
    void OnEnable()
    {
        if(m_Tween!=null)
        {
            m_Tween.PlayForward();
        }
        if(m_SkillDetail!=null)
        {
            m_SkillDetail.SetActive(false);
        }
    }
    /// <summary>
    /// 更新当前卡牌信息
    /// </summary>
    /// <param name="cardid">卡牌ID</param>
    /// <param name="own">是否获得</param>
    public void UpdateCard(int cardid, bool own)
    {
        Tab_Card card = TableManager.GetCardByID(cardid, 0);
        if (null == card)
        {
            return;
        }
        m_CurCardId = cardid;


        if (m_Quality == null || m_Mark == null || m_EnvTypePic == null || m_FulingName == null || m_Photo == null || m_Shadow == null || m_NotOwn == null || m_Akira == null || m_Background==null
            || m_AkiraAll == null || m_SkillStaticText == null || m_BackgoundStaticText == null || m_Background == null || m_Prev == null || m_Next==null)
        {
            return;
        }
        // 品质
        m_Quality.spriteName = "Common_N";
        switch ((CARD_RARE)card.Rare)
        {
            case CARD_RARE.N:
                m_Quality.spriteName = "Common_N";
                break;
            case CARD_RARE.R:
                m_Quality.spriteName = "Common_R";
                break;
            case CARD_RARE.SR:
                m_Quality.spriteName = "Common_SR";
                break;
            case CARD_RARE.SSR:
                m_Quality.spriteName = "Common_SSR";
                break;
            case CARD_RARE.UR:
                m_Quality.spriteName = "Common_UR";
                break;
        }

        // 类型
        m_Mark.spriteName = CardTool.GetCardBattleTypeIconName(card.BattleType);

        m_EnvTypePic.spriteName = CardTool.GetCardEnvTypeIconName(card.EnvironmentType);
        // 名称
        Tab_RoleBaseAttr roleBase = TableManager.GetRoleBaseAttrByID(card.GetRoleBaseIDStepbyIndex(0), 0);
        if (null == roleBase)
        {
            return;
        }

        m_FulingName.text = roleBase.Name;

        if (own) // 获得时
        {
            m_Photo.mainTexture = Utils.LoadTexture(card.Portrait);
            m_Photo.gameObject.SetActive(true);
            m_Shadow.gameObject.SetActive(false);
            m_NotOwn.gameObject.SetActive(false);
            m_Akira.text = card.CVName;
            m_Background.text = card.Background;
            SetSkill(card);
        }
        else // 未获得时
        {
            m_Shadow.mainTexture = Utils.LoadTexture(card.Portrait);
            m_Photo.gameObject.SetActive(false);
            m_Shadow.gameObject.SetActive(true);
            m_NotOwn.gameObject.SetActive(true);
        }

        // 更新获得和未获得时状态不同的组件
        m_AkiraAll.SetActive(own);
        m_SkillStaticText.SetActive(own);
        m_BackgoundStaticText.SetActive(own);
        m_Background.gameObject.SetActive(own);

        // 更新向前翻页向后翻页按钮
        m_Prev.gameObject.SetActive(GlobeVar.INVALID_ID != GetPrevCard());
        m_Next.gameObject.SetActive(GlobeVar.INVALID_ID != GetNextCard());
    }

    // 设置技能显示
    private void SetSkill(Tab_Card card)
    {
        if (null == m_InitSkillList || null == m_AwakeSkillList || 3 != GlobeVar.CARD_SKILLCOUNT || card==null)
        {
            return;
        }
        

        // 基础和3阶觉醒对应属性
        Tab_RoleBaseAttr[] attr = new Tab_RoleBaseAttr[4];

        for (int i = 0; i < attr.Length && i < card.getRoleBaseIDStepCount(); ++i)
        {
            attr[i] = TableManager.GetRoleBaseAttrByID(card.GetRoleBaseIDStepbyIndex(i), 0);//拿到各阶觉醒技能
        }

        for (int i = 0; i < GlobeVar.CARD_SKILLCOUNT; ++i)
        {
            m_InitSkillList[i] = -1;
            m_AwakeSkillList[i] = -1;
        }

        for (int i = 0; i < GlobeVar.CARD_SKILLCOUNT; ++i)
        {
            if (null == attr[0])
            {
                break;
            }

            // 1阶技能
            m_InitSkillList[i] = m_AwakeSkillList[i] = attr[0].GetSkillbyIndex(i);

            // 2阶技能
            if (null != attr[1] && m_InitSkillList[i] != attr[1].GetSkillbyIndex(i))
            {
                m_AwakeSkillList[i] = attr[1].GetSkillbyIndex(i);
                continue;
            }

            // 3阶技能
            if (null != attr[2] && m_InitSkillList[i] != attr[2].GetSkillbyIndex(i))
            {
                m_AwakeSkillList[i] = attr[2].GetSkillbyIndex(i);
                continue;
            }

            // 4阶技能
            if (null != attr[3] && m_InitSkillList[i] != attr[3].GetSkillbyIndex(i))
            {
                m_AwakeSkillList[i] = attr[3].GetSkillbyIndex(i);
                continue;
            }

        }

        // 设置技能图标
        SetSkillIcon(m_Skill[0], m_InitSkillList[0]);
        SetSkillIcon(m_Skill[1], m_InitSkillList[1]);
        SetSkillIcon(m_Skill[2], m_InitSkillList[2]);
    }

    // 设置技能图标
    private void SetSkillIcon(UISprite icon, int skillExId)
    {
        Tab_SkillBase skill = GetSkillBase(skillExId);
        if (null == skill || skillExId == -1)
        {
            icon.gameObject.SetActive(false);

        }
        else
        {
            icon.gameObject.SetActive(true);
            icon.spriteName = skill.Icon;
        }
    }
    #endregion

    #region 关闭相关方法
    /// <summary>
    /// 关闭详情界面：逆向播放动画->关闭界面->让Pre界面出现
    /// </summary>
    public void Close()
    {
        if (m_Tween == null)
        {
            return;
        }
        if(m_Tween.isActiveAndEnabled)
        {
            return;
        }
        m_Tween.PlayReverse();
    }
    //动画结束的委托
    public void OnTweenFinish()
    {
        if (m_Tween != null && m_Tween.value== 0)
        {
            if (m_PreviewObj != null)
            {
                m_PreviewObj.gameObject.SetActive(true);
            }
            this.gameObject.SetActive(false);
        }
    }
    #endregion

    public void ResetData(List<int> cardList)
    {
        m_ShowCardList = cardList;
    }


    private Tab_SkillBase GetSkillBase(int skillExId)
    {
        Tab_SkillEx skillEx = TableManager.GetSkillExByID(skillExId, 0);
        if (null != skillEx)
        {
            return TableManager.GetSkillBaseByID(skillEx.BaseID, 0);
        }
        return null;
    }
    #region Detail的点击事件

    // 获取预览中上一个符灵
    int GetPrevCard()
    {
        if (null == m_ShowCardList)
        {
            return GlobeVar.INVALID_ID;
        }

        if (m_CurCardId != GlobeVar.INVALID_ID)
        {
            int idx = m_ShowCardList.IndexOf(m_CurCardId);
            if (-1 != idx && idx - 1 >= 0)
            {
                return m_ShowCardList[idx - 1];
            }
        }

        return GlobeVar.INVALID_ID;
    }

    // 获取预览中下一个符灵
    int GetNextCard()
    {
        if (null == m_ShowCardList)
        {
            return GlobeVar.INVALID_ID;
        }

        if (m_CurCardId != GlobeVar.INVALID_ID)
        {
            int idx = m_ShowCardList.IndexOf(m_CurCardId);
            if (-1 != idx && idx + 1 < m_ShowCardList.Count)
            {
                return m_ShowCardList[idx + 1];
            }
        }

        return GlobeVar.INVALID_ID;
    }

    // 查看上一个符灵
    public void OnPrevCardClick()
    {
        int prev = GetPrevCard();
        if (GlobeVar.INVALID_ID != prev && null != GameManager.PlayerDataPool)
        {
            UpdateCard(prev, GameManager.PlayerDataPool.IsCardGet(prev));
            m_SkillDetailWindow.gameObject.SetActive(false);
        }
    }

    // 查看下一个符灵
    public void OnNextCardClick()
    {
        int next = GetNextCard();
        if (GlobeVar.INVALID_ID != next && null != GameManager.PlayerDataPool)
        {
            UpdateCard(next, GameManager.PlayerDataPool.IsCardGet(next));
            m_SkillDetailWindow.gameObject.SetActive(false);
        }
    }

    // 查看技能详细介绍
    public void OnShowSkillDetail(UISprite button)
    {
        if (null == button || m_SkillDetailWindow==null)
        {
            return;
        }
        m_SkillDetailWindow.gameObject.SetActive(true);
        //展示技能1
        if (button == m_Skill[0])
        {
            m_SkillDetailWindow.UpdateWindow(m_InitSkillList[0]);
        }
            //展示技能2
        else if (button == m_Skill[1])
        {
            m_SkillDetailWindow.UpdateWindow(m_InitSkillList[1]);
        }
            //展示技能3
        else if (button == m_Skill[2])
        {
            m_SkillDetailWindow.UpdateWindow(m_InitSkillList[2]);
        } 
        else
        {
            return;
        }

        

    }

    // 播放符灵的语音，在配音表中随机选取一个配音播放
    public void OnPlaySoundClick()
    {
        Tab_Card card = TableManager.GetCardByID(m_CurCardId, 0);
        if (null == card)
        {
            return;
        }

        List<int> validSound = new List<int>();
        if (null == validSound)
        {
            return;
        }

        for (int i = 0; i < card.getClickSoundIDCount(); ++i)
        {
            if (GlobeVar.INVALID_ID != card.GetClickSoundIDbyIndex(i))
            {
                validSound.Add(card.GetClickSoundIDbyIndex(i));
            }
        }

        if (0 == validSound.Count)
        {
            return;
        }

        SoundManager sm = GameManager.SoundManager;
        if (null != sm)
        {
            sm.PlayRealSound(validSound[Random.Range(0, validSound.Count)]);
        }
    }
    #endregion
}
