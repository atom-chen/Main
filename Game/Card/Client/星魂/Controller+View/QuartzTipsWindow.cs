using System;
using UnityEngine;
using System.Collections;
using Games.Table;
using Games.GlobeDefine;
using Games;
using ProtobufPacket;
using System.Collections.Generic;

//星魂信息tips View
public class QuartzTipsWindow : MonoBehaviour {
    public enum TipsType
    {
        Invalid,
        Bag,                //星宿背包
        Compared,           //比较
        Equiped,            //装备
        Info,               //仅展示属性
    }

    public UISprite m_QuartzIcon;
    public UISprite m_StarIcon;
    public UILabel m_NameLabel;
    public UILabel m_LevelLabel;
    public QuartzTipsAttrItem m_MainAttr;
    public QuartzTipsAttrItem[] m_AttachAttr;
    public UILabel m_OrbalLabel;
    public UILabel m_SetAttrLabel;
    public GameObject m_StrengthenBtn;
    public UISprite m_StrengthenBtnIcon;
    public GameObject m_EquipBtn;
    public UISprite m_EquipBtnIcon;
    public GameObject m_UnEquipBtn;
    public UIGrid m_BtnGrid;
    public GameObject m_EquipedIcon;
    public StateGroup m_LockSprite;
    public StateGroup m_ThrowSprite;
    public GameObject m_ShareObj;

    private Quartz m_Quartz = null;
    private TipsType m_TipsType = TipsType.Invalid;
    private Card m_Card = null;

    void Awake()
    {
        GameManager.PlayerDataPool.OnQuartzFlagChange += OnQuartzUpdate;
    }

    void OnDestroy()
    {
        GameManager.PlayerDataPool.OnQuartzFlagChange -= OnQuartzUpdate;
    }

    void OnDisable()
    {
        m_Quartz = null;
        m_TipsType = TipsType.Invalid;
        m_Card = null;
    }

    //Show符灵信息
    public void Show(Quartz quartz, TipsType type, Card card)
    {
        UpdateTutorialOnTipsShow();
        if (quartz == null)
        {
            return;
        }

        gameObject.SetActive(true);

        m_Quartz = quartz;
        m_TipsType = type;
        m_Card = card;

        Refresh();
    }

    private void Refresh()
    {
        if (m_StrengthenBtn != null)
        {
            m_StrengthenBtn.SetActive(m_TipsType == TipsType.Bag || m_TipsType == TipsType.Equiped);
        }
        if (m_EquipBtn != null)
        {
            m_EquipBtn.SetActive(m_TipsType == TipsType.Bag);
        }
        if (m_UnEquipBtn != null)
        {
            m_UnEquipBtn.SetActive(m_TipsType == TipsType.Equiped);
        }
        if (m_LockSprite != null && m_ThrowSprite != null)
        {
            m_LockSprite.gameObject.SetActive(m_TipsType != TipsType.Info);
            m_ThrowSprite.gameObject.SetActive(m_TipsType != TipsType.Info);
            //如果是已锁定
            if (m_Quartz.IsLock())
            {
                m_LockSprite.ChangeState(0);
                m_ThrowSprite.ChangeState(2);
            }
            //如果是未锁定 已弃置
            else if (m_Quartz.IsThrow())
            {
                m_LockSprite.ChangeState(1);
                m_ThrowSprite.ChangeState(0);
            }
            //未锁定 未弃置
            else
            {
                m_LockSprite.ChangeState(1);
                m_ThrowSprite.ChangeState(1);
            }
        }
        if (m_BtnGrid != null)
        {
            m_BtnGrid.Reposition();
        }
        if (m_ShareObj!=null)
        {
            m_ShareObj.SetActive(m_Quartz.Star >= GlobeVar._GameConfig.m_nShareQuartzNeedStar && (m_TipsType == TipsType.Bag || m_TipsType == TipsType.Equiped));
        }

        m_QuartzIcon.spriteName = m_Quartz.GetListIcon();
        m_StarIcon.spriteName = QuartzTool.GetQuartzStarIconName(m_Quartz.Star);
        m_NameLabel.text = m_Quartz.GetFullName();
        m_LevelLabel.gameObject.SetActive(m_Quartz.Strengthen > 0);
        m_LevelLabel.text = StrDictionary.GetClientDictionaryString("#{5160}", m_Quartz.Strengthen);
        m_MainAttr.Init(m_Quartz.MainAttr.RefixType, m_Quartz.MainAttr.AttrValue);
        for (int i = 0; i < m_AttachAttr.Length && i < m_Quartz.AttachAttr.Length; i++)
        {
            m_AttachAttr[i].InitAttach(m_Quartz.AttachAttr[i].RefixType, m_Quartz.AttachAttr[i].AttrValue);
        }

        m_SetAttrLabel.text = "";
        int nClassId = m_Quartz.GetClassId();
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

            if (tSet.NeedClassId != nClassId)
            {
                continue;
            }

            if (false == string.IsNullOrEmpty(m_SetAttrLabel.text))
            {
                m_SetAttrLabel.text += "\n";
            }
            m_SetAttrLabel.text += QuartzTool.GetFormatSetAttr(tSet);
        }

        if (m_EquipedIcon != null)
        {
            m_EquipedIcon.SetActive(m_TipsType == TipsType.Compared);
        }
    }
    
    private void OnQuartzUpdate(Quartz quartz)
    {
        if(quartz == null || m_Quartz == null ||quartz.Guid != m_Quartz.Guid)
        {
            return;
        }
        m_Quartz = quartz;
        Refresh();
    }
    //点击强化
    public void OnStrenthenClick()
    {

        if (false == TutorialManager.IsFunctionUnlock((int)FunctionUnlockId.QuartzStrengthen))
        {
            TutorialManager.SendFunctionLockNotice((int)FunctionUnlockId.QuartzStrengthen);
            return;
        }

        if (m_Quartz == null)
        {
            return;
        }

        if (m_TipsType != TipsType.Bag && m_TipsType != TipsType.Equiped)
        {
            return;
        }

        bool bQuartzValid = false;
        ulong cardGuid = GlobeVar.INVALID_GUID;
        if (m_Card != null && m_Card.Orbment != null && m_Card.Orbment.GetQuartz(m_Quartz.Guid) != null)
        {
            bQuartzValid = true;
            cardGuid = m_Card.Guid;
        }
        else if (GameManager.PlayerDataPool.PlayerQuartzBag.GetQuartz(m_Quartz.Guid) != null)
        {
            bQuartzValid = true;
        }

        if (bQuartzValid)
        {
            QuartzStrengthenWindow.ShowStrengthen(m_Quartz.Guid, cardGuid);
        }
        else
        {
            Utils.CenterNotice(6373);
        }

        if (QuartzTipsController.Instance != null)
        {
            QuartzTipsController.Instance.OnCloseClick();
        }
        else if(CardOrbmentWindow.Instance!=null)
        {
            CardOrbmentWindow.Instance.OnTipsCloseClick();
        }
    }

    //点击装备
    public void OnEquipClick()
    {
        UpDateTutorialOnEquipBtnClick();
        if (m_Quartz == null)
        {
            return;
        }

        if (m_TipsType != TipsType.Bag)
        {
            return;
        }

        if (QuartzEquipWindow.Instance != null)
        {
            QuartzEquipWindow.Instance.HandleTipsEquipClick(m_Quartz);
        }

        if (QuartzTipsController.Instance != null)
        {
            QuartzTipsController.Instance.HandleTipsEquipClick();
        }
        if(CardOrbmentWindow.Instance!=null)
        {
            CardOrbmentWindow.Instance.HandleTipsEquipClick(m_Quartz);
        }
    }

    //点击卸下
    public void OnUnEquipClick()
    {
        if (m_Quartz == null || m_Card == null)
        {
            return;
        }

        if (m_TipsType != TipsType.Equiped)
        {
            return;
        }

        CG_QUARTZ_UNEQUIP_PAK pak = new CG_QUARTZ_UNEQUIP_PAK();
        pak.data.QuartzGuid = m_Quartz.Guid;
        pak.data.CardGuid = m_Card.Guid;
        pak.SendPacket();

        if (QuartzTipsController.Instance != null)
        {
            QuartzTipsController.Instance.OnCloseClick();
        }
        else if(CardOrbmentWindow.Instance!=null)
        {
            CardOrbmentWindow.Instance.OnTipsCloseClick();
        }
    }

    public void OnClickLock()
    {
        if(m_Quartz == null)
        {
            return;
        }
        if(m_Quartz.IsLock())
        {
            CG_QUARTZ_UNLOCK_PAK pak = new CG_QUARTZ_UNLOCK_PAK();
            pak.data.QuartzGuid = m_Quartz.Guid;
            pak.SendPacket();
        }
        else
        {
            CG_QUARTZ_LOCK_PAK pak = new CG_QUARTZ_LOCK_PAK();
            pak.data.QuartzGuid = m_Quartz.Guid;
            pak.SendPacket();
        }
    }

    public void OnClickThrow()
    {
        if (m_Quartz == null)
        {
            return;
        }
        if(m_Quartz.IsLock())
        {
            //已锁定的无法弃置
            return;
        }
        if (m_Quartz.IsThrow())
        {
            CG_QUARTZ_UNTHROW_PAK pak = new CG_QUARTZ_UNTHROW_PAK();
            pak.data.QuartzGuid = m_Quartz.Guid;
            pak.SendPacket();
        }
        else
        {
            CG_QUARTZ_THROW_PAK pak = new CG_QUARTZ_THROW_PAK();
            pak.data.QuartzGuid = m_Quartz.Guid;
            pak.SendPacket();
        }
    }

    public void OnClickShare()
    {
        ShareController.Show(m_Quartz);
        //关闭Tips
        if(QuartzTipsController.Instance!=null)
        {
            QuartzTipsController.Instance.OnCloseClick();
        }
        else if(CardOrbmentWindow.Instance!=null)
        {
            CardOrbmentWindow.Instance.OnTipsCloseClick();
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    void UpdateTutorialOnTipsShow()
    {
        if (TutorialRoot.IsGroupStep(TutorialGroup.QuartzEquip, 10))
        {
            TutorialRoot.ShowTutorial(TutorialGroup.QuartzEquip, 11, m_EquipBtnIcon.gameObject, m_EquipBtnIcon.width, m_EquipBtnIcon.height);
        }
        else if (TutorialRoot.IsGroupStep(TutorialGroup.QuartzEquip, 13))
        {
            TutorialRoot.ShowTutorial(TutorialGroup.QuartzEquip, 14, m_EquipBtnIcon.gameObject, m_EquipBtnIcon.width, m_EquipBtnIcon.height);
        }
    }

    void UpDateTutorialOnEquipBtnClick()
    {
        if (TutorialRoot.IsGroupStep(TutorialGroup.QuartzEquip, 11))
        {
            if (OrbmentController.Instance != null)
            {
                OrbmentController.Instance.UpdateTutorialOnEquipBtnClick(TutorialGroup.QuartzEquip, 12);
            }
            else
            {
                TutorialRoot.TutorialOver();
            }
        }
        else if (TutorialRoot.IsGroupStep(TutorialGroup.QuartzEquip, 14))
        {
            if (QuartzEquipWindow.Instance != null)
            {
                QuartzEquipWindow.Instance.UpdateTutorialOnEquipBtnClick(TutorialGroup.QuartzEquip, 15);
            }
            else
            {
                TutorialRoot.TutorialOver();
            }
        }

    }
}
