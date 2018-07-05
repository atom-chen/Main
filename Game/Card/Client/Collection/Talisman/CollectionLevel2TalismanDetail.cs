using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Games.Table;
using Games;
using Games.GlobeDefine;

public class CollectionLevel2TalismanDetail : MonoBehaviour {
    public GameObject m_PreviewObj;

    public GameObject M_GetObj;
    public UISprite m_QualitySprite = null; //品质（稀有度）
    public UILabel m_TalismanNameLabel = null; //名称
    public UILabel m_NotOwnLabel = null; //尚未获得标记
    public UITexture m_PhotoTexture = null; //获得时立绘
    public UITexture m_ShadowTexture = null; //未获得时阴影
    public GameObject m_BackgoundStaticText = null; // 人物背景故事（静态文本）
    public UILabel m_BackgroundLabel = null; //人物背景故事（动态内容）
    public GameObject m_EffectStaticText = null; // 效果（静态文本，要在未获得时隐藏显示）
    public UILabel m_EffectLabel = null; // 效果
    public UISprite m_PrevSprite = null; // 向前翻页按钮
    public UISprite m_NextSprite = null; // 向后翻页按钮

    private List<int> m_ShowTalismanList = null; // 当前展示的法宝预览列表
    private int m_CurTalismanId = GlobeVar.INVALID_ID; // 当前展示法宝的id

    public TweenAlpha m_Tween;

    void OnEnable()
    {
        if (m_Tween != null)
        {
            m_Tween.PlayForward();
        }
    }
    /// <summary>
    /// 打开preview
    /// </summary>
    void OnDisable()
    {
        if (m_PreviewObj != null)
        {
            m_PreviewObj.SetActive(true);
        }
    }
    /// <summary>
    /// 关闭详情界面：逆向播放动画->关闭界面->让Pre界面出现
    /// </summary>
    public void Close()
    {
        if (m_Tween == null)
        {
            return;
        }
        if (m_Tween.isActiveAndEnabled)
        {
            return;
        }
        m_Tween.PlayReverse();
    }
    //动画结束的委托
    public void OnTweenFinish()
    {
        if (m_Tween != null && m_Tween.value == 0)
        {
            if (m_PreviewObj != null)
            {
                m_PreviewObj.gameObject.SetActive(true);
            }
            this.gameObject.SetActive(false);
        }
    }
    public void ResetData(List<int> TalismanList)
    {
        m_ShowTalismanList = TalismanList;
    }

    public void UpdateTalisman(int Talismanid, bool own)
    {
        Tab_Talisman talisman = TableManager.GetTalismanByID(Talismanid, 0);

        if (null == talisman)
        {
            return;
        }

        m_CurTalismanId = Talismanid;

        // 品质
        m_QualitySprite.spriteName = "N";
        switch ((CARD_RARE)talisman.Rare)
        {
            case CARD_RARE.N: m_QualitySprite.spriteName = "N"; break;
            case CARD_RARE.R: m_QualitySprite.spriteName = "R"; break;
            case CARD_RARE.SR: m_QualitySprite.spriteName = "SR"; break;
            case CARD_RARE.SSR: m_QualitySprite.spriteName = "SSR"; break;
            case CARD_RARE.UR: m_QualitySprite.spriteName = "UR"; break;
        }

        // 名称
        m_TalismanNameLabel.text = talisman.Name;
        
        if (own) // 获得时
        {
            m_PhotoTexture.mainTexture = Utils.LoadTexture(CollectionRoot.TalismanPortraitPath + talisman.Portrait);
            m_PhotoTexture.gameObject.SetActive(true);
            m_ShadowTexture.gameObject.SetActive(false);
            m_NotOwnLabel.gameObject.SetActive(false);
            m_BackgroundLabel.text = talisman.Background;
            SetEffect();
        }
        else // 未获得时
        {
            m_ShadowTexture.mainTexture = Utils.LoadTexture(CollectionRoot.TalismanPortraitPath + talisman.Portrait);
            m_PhotoTexture.gameObject.SetActive(false);
            m_ShadowTexture.gameObject.SetActive(true);
            m_NotOwnLabel.gameObject.SetActive(true);
        }

        // 更新获得和未获得时状态不同的组件
        //m_BackgoundStaticText.SetActive(own);
        //m_BackgroundLabel.gameObject.SetActive(own);
        //m_EffectStaticText.SetActive(own);
        //m_EffectLabel.gameObject.SetActive(own);
        M_GetObj.SetActive(own);

        // 更新向前翻页向后翻页按钮
        m_PrevSprite.gameObject.SetActive(GlobeVar.INVALID_ID != GetPrevTalisman());
        m_NextSprite.gameObject.SetActive(GlobeVar.INVALID_ID != GetNextTalisman());
    }

    // 设置法宝效果
    private void SetEffect()
    {
        Tab_Talisman talisman = TableManager.GetTalismanByID(m_CurTalismanId, 0);
        if (null != talisman)
        {
            Tab_SkillEx skillEx = TableManager.GetSkillExByID(talisman.SkillId, 0);
            if (null != skillEx)
            {
                m_EffectLabel.text = skillEx.Description;
            }
        }
    }

    /// <summary>
    /// 这一部分是详细界面向前翻页向后翻页相关逻辑
    /// </summary>
    /// <returns></returns>

    // 获取预览中上一个法宝
    int GetPrevTalisman()
    {
        if (null == m_ShowTalismanList)
        {
            return GlobeVar.INVALID_ID;
        }

        if (m_CurTalismanId != GlobeVar.INVALID_ID)
        {
            int idx = m_ShowTalismanList.IndexOf(m_CurTalismanId);
            if (-1 != idx && idx - 1 >= 0)
            {
                return m_ShowTalismanList[idx - 1];
            }
        }

        return GlobeVar.INVALID_ID;
    }

    // 获取预览中下一个法宝
    int GetNextTalisman()
    {
        if (null == m_ShowTalismanList)
        {
            return GlobeVar.INVALID_ID;
        }

        if (m_CurTalismanId != GlobeVar.INVALID_ID)
        {
            int idx = m_ShowTalismanList.IndexOf(m_CurTalismanId);
            if (-1 != idx && idx + 1 < m_ShowTalismanList.Count)
            {
                return m_ShowTalismanList[idx + 1];
            }
        }

        return GlobeVar.INVALID_ID;
    }

    // 查看上一个法宝
    public void OnPrevTalismanClick()
    {
        if (null == GameManager.PlayerDataPool)
        {
            return;
        }

        int prev = GetPrevTalisman();
        if (GlobeVar.INVALID_ID != prev)
        {
            UpdateTalisman(prev, GameManager.PlayerDataPool.IsTalismanGet(prev));
        }
    }

    // 查看下一个法宝
    public void OnNextTalismanClick()
    {
        if (null == GameManager.PlayerDataPool)
        {
            return;
        }

        int next = GetNextTalisman();
        if (GlobeVar.INVALID_ID != next)
        {
            UpdateTalisman(next, GameManager.PlayerDataPool.IsTalismanGet(next));
        }
    }
}
