using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Games.Table;
using Games.GlobeDefine;
using Games;

public class CollectionLevel2StarDetail : MonoBehaviour {
    public GameObject m_PreviewObj;


    public UILabel m_NameLabel = null; // 星魂名称
    public UILabel m_NotOwnLabel = null; // 未获得标记
    public UILabel m_BackgoundStaticLabel = null; // 静态背景描述文字（未获得时需隐藏）
    public UILabel m_BackgroundLabel = null; // 星魂背景文字描述
    public UILabel m_EffectStaticLabel = null; // 效果静态文字（未获得时需隐藏）
    public UILabel m_EffectLabel = null; // 星魂效果文字描述

    public GameObject m_OwnStatic = null; // 拥有文字（未获得时需隐藏）
    public UISprite m_OwnIcon0 = null; // 拥有图标1
    public UISprite m_OwnIcon1 = null; // 拥有图标2
    public UISprite m_OwnIcon2 = null; // 拥有图标3
    public UISprite m_OwnIconShadow0 = null; // 图标1遮挡
    public UISprite m_OwnIconShadow1 = null; // 图标2遮挡
    public UISprite m_OwnIconShadow2 = null; // 图标3遮挡


    public UISprite m_PrevSprite = null; // 向前翻页按钮
    public UISprite m_NextSprite = null; // 向后翻页按钮

    public UITexture m_PhotoTexture = null; // 立绘
    public UITexture m_ShadowTexture = null; // 阴影

    private List<int> m_ShowStarClassList = null; // 当前展示的星魂预览列表
    private int m_CurStarClassId = GlobeVar.INVALID_ID; // 当前展示星魂类型的id



    public TweenAlpha m_Tween;
    void OnEnable()
    {
        if (m_Tween != null)
        {
            m_Tween.PlayForward();
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

    // 获取对应位置的星魂
    private Tab_Quartz GetQuartzBySlot(OrbmentSlotType slot)
    {
        Dictionary<int, List<Tab_Quartz>> allQuartz = TableManager.GetQuartz();
        if (null == allQuartz)
        {
            return null;
        }

        foreach (var item in allQuartz)
        {
            List<Tab_Quartz> quartzSet = item.Value;
            if (null == quartzSet || quartzSet.Count < 1)
            {
                continue;
            }

            Tab_Quartz quartz = quartzSet[0];
            if (null != quartz && quartz.ClassId == m_CurStarClassId && quartz.SlotType == (int)slot)
            {
                return quartz;
            }
        }

        return null;
    }

    public void ResetData(List<int> starClassList)
    {
        m_ShowStarClassList = starClassList;
    }

    public void UpdateStar(int starClassID, bool own)
    {
        Tab_QuartzClass quartzClass = TableManager.GetQuartzClassByID(starClassID, 0);

        if (null == quartzClass)
        {
            return;
        }

        m_CurStarClassId = starClassID;
        m_NameLabel.text = quartzClass.Name;

        if (own) // 获得时
        {
            m_PhotoTexture.mainTexture = Utils.LoadTexture(CollectionRoot.StarPicPath + quartzClass.Pic);
            m_BackgroundLabel.text = quartzClass.Background;
            m_PhotoTexture.gameObject.SetActive(true);
            m_ShadowTexture.gameObject.SetActive(false);
            m_NotOwnLabel.gameObject.SetActive(false);
            SetEffect();
            SetOwnIcon();
        }
        else // 未获得时
        {
            m_ShadowTexture.mainTexture = Utils.LoadTexture(CollectionRoot.StarPicPath + quartzClass.Pic);
            m_PhotoTexture.gameObject.SetActive(false);
            m_ShadowTexture.gameObject.SetActive(true);
            m_NotOwnLabel.gameObject.SetActive(true);
        }

        // 更新获得和未获得时状态不同的组件
        m_BackgroundLabel.gameObject.SetActive(own);
        m_EffectLabel.gameObject.SetActive(own);
        m_OwnIcon0.gameObject.SetActive(own);
        m_OwnIcon1.gameObject.SetActive(own);
        m_OwnIcon2.gameObject.SetActive(own);
        m_BackgoundStaticLabel.gameObject.SetActive(own);
        m_EffectStaticLabel.gameObject.SetActive(own);
        m_OwnStatic.gameObject.SetActive(own);

        // 更新向前翻页向后翻页按钮
        m_PrevSprite.gameObject.SetActive(GlobeVar.INVALID_ID != GetPrevStarClass());
        m_NextSprite.gameObject.SetActive(GlobeVar.INVALID_ID != GetNextStarClass());
    }

    // 设置拥有图标
    private void SetOwnIcon()
    {
        if (null == GameManager.PlayerDataPool)
        {
            return;
        }

        Tab_Quartz quartz0 = GetQuartzBySlot(OrbmentSlotType.Inside);
        Tab_Quartz quartz1 = GetQuartzBySlot(OrbmentSlotType.Middle);
        Tab_Quartz quartz2 = GetQuartzBySlot(OrbmentSlotType.Outside);

        if (null != quartz0)
        {
            m_OwnIcon0.spriteName = quartz0.ListIcon;
            m_OwnIconShadow0.gameObject.SetActive(!GameManager.PlayerDataPool.IsQuartzClassGet(m_CurStarClassId, QUARTZ_COLLECT_IDX.Inside_Get));
        }

        if (null != quartz1)
        {
            m_OwnIcon1.spriteName = quartz1.ListIcon;
            m_OwnIconShadow1.gameObject.SetActive(!GameManager.PlayerDataPool.IsQuartzClassGet(m_CurStarClassId, QUARTZ_COLLECT_IDX.Middle_Get));
        }

        if (null != quartz2)
        {
            m_OwnIcon2.spriteName = quartz2.ListIcon;
            m_OwnIconShadow2.gameObject.SetActive(!GameManager.PlayerDataPool.IsQuartzClassGet(m_CurStarClassId, QUARTZ_COLLECT_IDX.Outside_Get));
        }
    }

    // 设置技能效果
    private void SetEffect()
    {
        m_EffectLabel.text = "";
        Tab_QuartzSet quartzSet = TableManager.GetQuartzSetByID(m_CurStarClassId, 0);
        if (null != quartzSet)
        {
            for (int i = 0; i < quartzSet.getAttrRefixTypeCount(); ++i)
            {
                int attrType = quartzSet.GetAttrRefixTypebyIndex(i);
                int attrValue = quartzSet.GetAttrRefixValuebyIndex(i);
                if (GlobeVar.INVALID_ID != attrType && GlobeVar.INVALID_ID != attrValue)
                {
                    m_EffectLabel.text += (Utils.GetAttrRefixName((AttrRefixType)attrType) + " +" + attrValue.ToString() + "  ");
                }
            }
        }
    }

    // 获取预览中上一个星魂
    int GetPrevStarClass()
    {
        if (null == m_ShowStarClassList)
        {
            return GlobeVar.INVALID_ID;
        }

        if (m_CurStarClassId != GlobeVar.INVALID_ID)
        {
            int idx = m_ShowStarClassList.IndexOf(m_CurStarClassId);
            if (-1 != idx && idx - 1 >= 0)
            {
                return m_ShowStarClassList[idx - 1];
            }
        }

        return GlobeVar.INVALID_ID;
    }

    // 获取预览中下一个星魂
    int GetNextStarClass()
    {
        if (null == m_ShowStarClassList)
        {
            return GlobeVar.INVALID_ID;
        }

        if (m_CurStarClassId != GlobeVar.INVALID_ID)
        {
            int idx = m_ShowStarClassList.IndexOf(m_CurStarClassId);
            if (-1 != idx && idx + 1 < m_ShowStarClassList.Count)
            {
                return m_ShowStarClassList[idx + 1];
            }
        }

        return GlobeVar.INVALID_ID;
    }

    // 查看上一个星魂
    public void OnPrevStarClassClick()
    {
        if (null == GameManager.PlayerDataPool)
        {
            return;
        }

        int prev = GetPrevStarClass();
        if (GlobeVar.INVALID_ID != prev)
        {
            UpdateStar(prev, GameManager.PlayerDataPool.IsQuartzClassGet(prev));
        }
    }

    // 查看下一个星魂
    public void OnNextStarClassClick()
    {
        if (null == GameManager.PlayerDataPool)
        {
            return;
        }

        int next = GetNextStarClass();
        if (GlobeVar.INVALID_ID != next)
        {
            UpdateStar(next, GameManager.PlayerDataPool.IsQuartzClassGet(next));
        }
    }

}
