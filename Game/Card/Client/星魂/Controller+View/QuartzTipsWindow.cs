using System;
using UnityEngine;
using System.Collections;
using Games.Table;
using Games.GlobeDefine;
using Games;

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
    public UISprite m_SlotIcon;
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
    public UISprite m_BGSprite;

    private Quartz m_Quartz = null;
    private TipsType m_TipsType = TipsType.Invalid;
    private Card m_Card = null;

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
        if (m_BtnGrid != null)
        {
            m_BtnGrid.Reposition();
        }

        m_QuartzIcon.spriteName = m_Quartz.GetListIcon();
        m_SlotIcon.spriteName = QuartzTool.GetQuartzSlotTypeIcon(m_Quartz.GetSlotType());
        m_StarIcon.spriteName = QuartzTool.GetQuartzStarIconName(m_Quartz.Star);
        m_NameLabel.text = m_Quartz.GetName();
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
