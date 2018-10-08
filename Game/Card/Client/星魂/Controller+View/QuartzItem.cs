using UnityEngine;
using System.Collections;
using Games.GlobeDefine;

public class QuartzItem : MonoBehaviour {

    public UISprite m_QuartzIcon;
    public UILabel m_StrengthenLabel;
    public UISprite m_StarIcon;
    public GameObject m_ChooseIcon;
    public GameObject m_SellChoose;
    public GameObject m_NewIcon;

    // 新手指引
    public UISprite m_TutorialSprite = null;

    private Quartz m_Quartz = null;
    public Quartz Quartz
    {
        get { return m_Quartz; }
    }

    public ulong GetGuid()
    {
        return m_Quartz != null ? m_Quartz.Guid : GlobeVar.INVALID_GUID;
    }

	public void Init(Quartz quartz, bool bSellChoose)
    {
        if (quartz == null)
        {
            return;
        }

        m_Quartz = quartz;

        m_QuartzIcon.spriteName = m_Quartz.GetListIcon();
	    m_StrengthenLabel.text = string.Format("+{0}", m_Quartz.Strengthen);
	    m_StarIcon.spriteName = QuartzTool.GetQuartzStarIconName(m_Quartz.Star);
        m_ChooseIcon.SetActive(false);
        m_SellChoose.SetActive(bSellChoose);
        m_NewIcon.SetActive(GameManager.PlayerDataPool.PlayerQuartzBag.IsHaveNewQuartzGuid(m_Quartz.Guid));
    }

    public void OnItemClick()
    {
        if (m_Quartz == null)
        {
            return;
        }

        if (OrbmentController.Instance != null)
        {
            OrbmentController.Instance.HandleQuartzItemClick(this);
        }

        if (m_NewIcon.activeSelf)
        {
            GameManager.PlayerDataPool.PlayerQuartzBag.DelNewQuartzGuid(m_Quartz.Guid);
            m_NewIcon.SetActive(false);
        }
    }

    public void UpdateChoose(bool bChoose)
    {
        m_ChooseIcon.SetActive(bChoose);
    }

    public void UpdateSellChoose(bool bChoose)
    {
        m_SellChoose.SetActive(bChoose);
    }
}
