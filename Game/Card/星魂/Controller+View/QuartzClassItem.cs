using UnityEngine;
using System.Collections;
using Games.Table;
using Games.GlobeDefine;
using Games;

public class QuartzClassItem : MonoBehaviour {

    public UITexture m_SetTexture;
    public UILabel m_NameLabel;
    public UILabel m_CountLabel;
    public UILabel m_SetAttrLabel;
    public GameObject m_NoQuartzMask;

    // 新手指引
    public UISprite m_BgSprite;

    private int m_ClassId = GlobeVar.INVALID_ID;

    public void Init(int nClassId, int nCount)
    {
        Tab_QuartzClass tClass = TableManager.GetQuartzClassByID(nClassId, 0);
        if (tClass == null)
        {
            return;
        }

        m_ClassId = nClassId;

        m_SetTexture.mainTexture = Utils.LoadTexture("Texture/T_StarSoul/" + tClass.Pic);
        m_NameLabel.text = tClass.Name;
        m_CountLabel.text = nCount.ToString();
        m_NoQuartzMask.SetActive(nCount <= 0);

        m_SetAttrLabel.text = "";
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

            m_SetAttrLabel.text = StrDictionary.GetClientDictionaryString("#{5155}", tSet.NeedCount);

            for (int i = 0; i < tSet.getAttrRefixTypeCount() && i < tSet.getAttrRefixValueCount(); i++)
            {
                int nRefixType = tSet.GetAttrRefixTypebyIndex(i);
                int nRefixValue = tSet.GetAttrRefixValuebyIndex(i);

                if (nRefixType == GlobeVar.INVALID_ID || nRefixValue <= 0)
                {
                    continue;
                }

                if (i != 0)
                {
                    m_SetAttrLabel.text += ",";
                }

                string szAttrValue = "";
                if (nRefixType == (int)AttrRefixType.MaxHPPercent || nRefixType == (int)AttrRefixType.MaxHPFinal ||
                    nRefixType == (int)AttrRefixType.AttackPercent || nRefixType == (int)AttrRefixType.AttackFinal ||
                    nRefixType == (int)AttrRefixType.DefensePercent || nRefixType == (int)AttrRefixType.DefenseFinal ||
                    nRefixType == (int)AttrRefixType.SpeedPercent || nRefixType == (int)AttrRefixType.SpeedFinal)
                {
                    szAttrValue = string.Format("{0}%", (int)(nRefixValue / 100.0f));
                }
                else
                {
                    if (nRefixType == (int)AttrRefixType.CritChanceAdd || nRefixType == (int)AttrRefixType.CritEffectAdd ||
                        nRefixType == (int)AttrRefixType.ImpactChanceAdd || nRefixType == (int)AttrRefixType.ImpactResistAdd)
                    {
                        szAttrValue = string.Format("{0}%", (int)(nRefixValue / 100.0f));
                    }
                    else
                    {
                        szAttrValue = string.Format("{0}", nRefixValue);
                    }
                }

                m_SetAttrLabel.text += string.Format("{0}+{1}", Utils.GetAttrRefixName((AttrRefixType)nRefixType), szAttrValue);
            }

            for (int i = 0; i < tSet.getImpactIdCount(); i++)
            {
                Tab_Impact tImpact = TableManager.GetImpactByID(tSet.GetImpactIdbyIndex(i), 0);
                if (tImpact == null)
                {
                    continue;
                }

                m_SetAttrLabel.text += ",";
                m_SetAttrLabel.text += StrDictionary.GetClientDictionaryString("#{5154}", tImpact.Name);
            }

            break;
        }
    }

    public void OnItemClick()
    {
        if (OrbmentController.Instance != null)
        {
            OrbmentController.Instance.HandleQuartzClassItemClick(m_ClassId);
        }
    }
}
