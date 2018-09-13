using UnityEngine;
using System.Collections;
using Games;

public class QuartzTipsAttrItem : MonoBehaviour {

    public UILabel m_RefixTypeLabel;
    public UILabel m_RefixValueLabel;

    public void Init(int type, int value)
    {
        if (type >= 0)
        {
            m_RefixTypeLabel.text = Utils.GetAttrRefixName((AttrRefixType)type);
            if (type == (int)AttrRefixType.MaxHPPercent || type == (int)AttrRefixType.MaxHPFinal ||
                type == (int)AttrRefixType.AttackPercent || type == (int)AttrRefixType.AttackFinal ||
                type == (int)AttrRefixType.DefensePercent || type == (int)AttrRefixType.DefenseFinal ||
                type == (int)AttrRefixType.SpeedPercent || type == (int)AttrRefixType.SpeedFinal)
            {
                m_RefixValueLabel.text = string.Format("+{0}%", (int)(value / 100.0f));
            }
            else
            {
                if (type == (int)AttrRefixType.CritChanceAdd || type == (int)AttrRefixType.CritEffectAdd ||
                    type == (int)AttrRefixType.ImpactChanceAdd || type == (int)AttrRefixType.ImpactResistAdd)
                {
                    m_RefixValueLabel.text = string.Format("+{0}%", (int)(value / 100.0f));
                }
                else
                {
                    m_RefixValueLabel.text = string.Format("+{0}", value);
                }
            }
        }
        else
        {
            m_RefixTypeLabel.text = "";
            m_RefixValueLabel.text = "";
        }
    }

    public void InitAttach(int type, int value)
    {
        if (type >= 0)
        {
            m_RefixTypeLabel.text = Utils.GetAttrRefixName((AttrRefixType)type);

            if (type == (int)AttrRefixType.MaxHPPercent || type == (int)AttrRefixType.MaxHPFinal ||
                type == (int)AttrRefixType.AttackPercent || type == (int)AttrRefixType.AttackFinal ||
                type == (int)AttrRefixType.DefensePercent || type == (int)AttrRefixType.DefenseFinal ||
                type == (int)AttrRefixType.SpeedPercent || type == (int)AttrRefixType.SpeedFinal)
            {
                m_RefixValueLabel.text = string.Format("+{0}%", (int)(value / 100.0f));
            }
            else
            {
                if (type == (int)AttrRefixType.CritChanceAdd || type == (int)AttrRefixType.CritEffectAdd ||
                    type == (int)AttrRefixType.ImpactChanceAdd || type == (int)AttrRefixType.ImpactResistAdd)
                {
                    m_RefixValueLabel.text = string.Format("+{0}%", (int)(value / 100.0f));
                }
                else
                {
                    m_RefixValueLabel.text = string.Format("+{0}", value);
                }
            }
        }
        else
        {
            m_RefixTypeLabel.text = "";
            m_RefixValueLabel.text = "";
        }
    }
}
