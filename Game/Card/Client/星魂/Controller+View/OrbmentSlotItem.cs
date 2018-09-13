using UnityEngine;
using System.Collections;
using Games.Table;
using Games.GlobeDefine;

public class OrbmentSlotItem : MonoBehaviour
{
    public enum SlotState
    {
        Normal,
        Empty,
    }

    public GameObject m_QuartzObject;
    public UISprite m_QuartzIcon;
    public GameObject m_ChooseIcon;
    public UILabel m_StrengthenLabel;
    public UISprite m_ColorIcon;
    private Quartz m_Quartz = null;
    public Quartz Quartz
    {
        get { return m_Quartz; }
    }

    private SlotState m_SlotItemState = SlotState.Empty;
    public SlotState SlotItemState
    {
        get { return m_SlotItemState; }
    }

    private int m_SlotType = GlobeVar.INVALID_ID;
    public int SlotType
    {
        get { return m_SlotType; }
    }

    private GameObject m_QuartzSetEffect = null;

    private const string SetEffectName = "e_ui_star_taozhuangjiqi";

    public ulong GetGuid()
    {
        return m_Quartz != null ? m_Quartz.Guid : GlobeVar.INVALID_GUID;
    }

    public bool IsValid()
    {
        return m_Quartz != null && m_Quartz.IsValid();
    }

    public bool IsQuartzClassId(int nClassId)
    {
        if (m_Quartz == null || false == m_Quartz.IsValid())
        {
            return false;
        }

        Tab_Quartz tQuartz = TableManager.GetQuartzByID(m_Quartz.QuartzId, 0);
        if (tQuartz == null)
        {
            return false;
        }

        return tQuartz.ClassId == nClassId;
    }

    public void Init(Quartz quartz)
    {
        //为空，显示数字
        if (quartz == null)  
        {
            m_ColorIcon.gameObject.SetActive(true);
            m_ColorIcon.spriteName = QuartzTool.GetQuartzStarIconName_Circle(0);
            m_QuartzObject.SetActive(false);
            return;
        }

        m_QuartzObject.SetActive(true);

        m_QuartzIcon.gameObject.SetActive(true);
        m_QuartzIcon.spriteName = quartz.GetIcon();
        if (m_ChooseIcon!=null)
        {
            m_ChooseIcon.SetActive(false);
        }      

        m_StrengthenLabel.text = string.Format("+{0}", quartz.Strengthen);
        m_ColorIcon.gameObject.SetActive(true);
        m_ColorIcon.spriteName = QuartzTool.GetQuartzStarIconName_Circle(quartz.Star);

        int nSlotType = quartz.GetSlotType();

        if (m_QuartzSetEffect != null)
        {
            m_QuartzSetEffect.SetActive(false);
        }


        m_Quartz = quartz;
        m_SlotItemState = SlotState.Normal;
        m_SlotType = nSlotType;
    }

    public void InitEmpty(int nSlotType)
    {
        m_QuartzObject.SetActive(false);
        if (m_ChooseIcon != null)
        {
            m_ChooseIcon.SetActive(false);
        }

        if (m_QuartzSetEffect != null)
        {
            m_QuartzSetEffect.SetActive(false);
        }

        m_Quartz = null;
        m_SlotItemState = SlotState.Empty;
        m_SlotType = nSlotType;
    }

    public void UpdateChoose(bool bChoose)
    {
        if (m_ChooseIcon!=null)
        {
            m_ChooseIcon.SetActive(bChoose);
        }       
    }

    public void OnItemClick()
    {
        if (QuartzEquipWindow.Instance != null)
        {
            QuartzEquipWindow.Instance.HandleOrbmentSlotItemClick(this);
        }
        //星魂、动态属性查看
        if (QuartzDynamicWindow.Instance != null)
        {
            QuartzDynamicWindow.Instance.HandleOrbmentSlotItemClick(this);
        }
        //符灵界面的星盘
        if(CardOrbmentWindow.Instance!=null)
        {
            CardOrbmentWindow.Instance.HandleOrbmentSlotItemClick(this);
        }
    }

    public void PlayQuartzSetEffect()
    {
        CancelInvoke("QuartzSetEffectOver");

        if (m_QuartzSetEffect != null)
        {
            m_QuartzSetEffect.SetActive(false);
            m_QuartzSetEffect.SetActive(true);
        }
        else
        {
            m_QuartzSetEffect = AssetManager.LoadObjAndBindToParent("Bundle/Effect/" + SetEffectName, transform);
        }

        Invoke("QuartzSetEffectOver", 1.0f);
    }

    void QuartzSetEffectOver()
    {
        if (m_QuartzSetEffect != null)
        {
            m_QuartzSetEffect.SetActive(false);
        }
    }
}
