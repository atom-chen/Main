using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.Table;
using Games;
using Games.GlobeDefine;

public class OrbmentSetAttrItem : MonoBehaviour
{
    public UITexture m_ClassPic;
    public UILabel m_NameLabel;
    public UILabel m_CountLabel;
    public GameObject m_ActiveObject;
    public GameObject m_NormalObject;

    private int m_SetId = GlobeVar.INVALID_ID;

    public void Init(int nSetId, bool bActive)
    {
        Tab_QuartzSet tSet = TableManager.GetQuartzSetByID(nSetId, 0);
        if (tSet == null)
        {
            return;
        }

        Tab_QuartzClass tClass = TableManager.GetQuartzClassByID(tSet.NeedClassId, 0);
        if (tClass == null)
        {
            return;
        }

        m_SetId = nSetId;

        m_ClassPic.mainTexture = Utils.LoadTexture("Texture/T_StarSoul/" + tClass.Pic);
        m_NameLabel.text = tClass.Name;
        m_CountLabel.text = StrDictionary.GetClientDictionaryString("#{5155}", tSet.NeedCount);
        m_ActiveObject.SetActive(bActive);
        m_NormalObject.SetActive(!bActive);
    }

}
