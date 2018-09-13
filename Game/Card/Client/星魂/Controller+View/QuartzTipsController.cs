using System.Collections;
using System.Collections.Generic;
using Games.GlobeDefine;
using UnityEngine;

//星魂详细信息TipsController
public class QuartzTipsController : MonoBehaviour
{
    private static QuartzTipsController m_Instance = null;
    public static QuartzTipsController Instance
    {
        get { return m_Instance; }
    }

    public QuartzTipsWindow m_QuartzTips;     //展示UI
    public QuartzTipsWindow m_ComparedTips;   //比较UI

    void Awake()
    {
        m_Instance = this;

        m_QuartzTips.gameObject.SetActive(false);
        m_ComparedTips.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        m_Instance = null;
    }

    //展示UI
    public void Show(Quartz quartz, QuartzTipsWindow.TipsType type, Card card = null)
    {
        this.gameObject.SetActive(true);
        if (quartz == null)
        {
            return;
        }
        m_QuartzTips.gameObject.SetActive(true);  
        m_QuartzTips.Show(quartz, type, card);

        if (type == QuartzTipsWindow.TipsType.Bag && card != null && card.Orbment != null && card.Orbment.QuartzSlot != null)
        {
            int nSlotIndex = quartz.GetSlotType() - 1;
            if (nSlotIndex < 0 || nSlotIndex >= card.Orbment.QuartzSlot.Length)
            {
                return;
            }

            if (card.Orbment.QuartzSlot[nSlotIndex] == null)
            {
                return;
            }

            if (card.Orbment.QuartzSlot[nSlotIndex].IsValid())
            {
                m_ComparedTips.Show(card.Orbment.QuartzSlot[nSlotIndex], QuartzTipsWindow.TipsType.Compared, card);
            }
            else
            {
                m_ComparedTips.gameObject.SetActive(false);
            }
        }
    }

    //反勾选
    public void OnCloseClick()
    {
        m_QuartzTips.gameObject.SetActive(false);
        m_ComparedTips.gameObject.SetActive(false);

        if (QuartzEquipWindow.Instance != null)
        {
            QuartzEquipWindow.Instance.HandleTipsCloseClick();
        }

        if (OrbmentController.Instance != null)
        {
            OrbmentController.Instance.OnQuartzTipsCloseClick();
        }
    }

    public void DirectCloseTips()
    {
        m_QuartzTips.gameObject.SetActive(false);
        m_ComparedTips.gameObject.SetActive(false);
    }

    //装备星魂后
    public void HandleTipsEquipClick()
    {
        m_QuartzTips.gameObject.SetActive(false);
        m_ComparedTips.gameObject.SetActive(false);

        if (OrbmentController.Instance != null)
        {
            OrbmentController.Instance.OnQuartzTipsCloseClick();
        }
    }

    public bool IsShow()
    {
        return m_QuartzTips.gameObject.activeSelf || m_ComparedTips.gameObject.activeSelf;
    }
}
