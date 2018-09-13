using Games;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardOrbmentWindow_AttrItem : MonoBehaviour 
{
    public UILabel m_NameLabel;
    public UILabel m_RefixValueLabel;

    public void Init(Card card, AttrType type)
    {
        if (card == null)
        {
            return;
        }

        int nAttrValue = card.GetAttrValue(type);
        int nAttrValue_WithoutOrbment = card.GetAttrValueWithoutOrbment(type);
        int nOrbmentRefix = nAttrValue - nAttrValue_WithoutOrbment;

        gameObject.SetActive(true);
        m_NameLabel.text = Utils.GetAttrName(type);

        if (type == AttrType.CritChance || type == AttrType.CritEffect ||
            type == AttrType.ImpactChance || type == AttrType.ImpactResist)
        {
            m_RefixValueLabel.text = string.Format(" +{0}%", (int)(nOrbmentRefix / 100.0f));
        }
        else
        {
            m_RefixValueLabel.text = string.Format(" +{0}", nOrbmentRefix);
        }
    }
}
