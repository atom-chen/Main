using UnityEngine;
using System.Collections;
using Games.GlobeDefine;
using Games;

public class OrbmentAttrItem : MonoBehaviour {

    public UILabel m_NameLabel;
    public UILabel m_RefixValueLabel;
    public UISprite m_UpIcon;
    public UIPlayEffect m_UpEffect;
    public UIPlayEffect m_DownEffect;

    private int m_OldRifexValue = GlobeVar.INVALID_ID;

	public void Init(Card card, AttrType type, bool bUpdateUp)
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

	    if (bUpdateUp)
	    {
	        if (nOrbmentRefix > m_OldRifexValue)
	        {
                m_UpIcon.gameObject.SetActive(false);
                m_UpIcon.gameObject.SetActive(true);
	            m_UpIcon.spriteName = "Star_52";

                m_UpEffect.Play();
            }
            else if (nOrbmentRefix < m_OldRifexValue)
            {
                m_UpIcon.gameObject.SetActive(false);
                m_UpIcon.gameObject.SetActive(true);
                m_UpIcon.spriteName = "Star_51";

                m_DownEffect.Play();
            }
            else
            {
                m_UpIcon.gameObject.SetActive(false);

                m_UpEffect.Stop();
                m_DownEffect.Stop();
            }
	    }
	    else
	    {
            m_UpIcon.gameObject.SetActive(false);

            m_UpEffect.Stop();
            m_DownEffect.Stop();

            m_OldRifexValue = nOrbmentRefix;
        }
    }

    public void CloseUpIcon()
    {
        m_UpIcon.gameObject.SetActive(false);
    }
}
