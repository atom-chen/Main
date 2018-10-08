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
    private int m_Count = 0;

    public void Init(int nClassId, int nCount)
    {
        Tab_QuartzClass tClass = TableManager.GetQuartzClassByID(nClassId, 0);
        if (tClass == null)
        {
            return;
        }

        m_ClassId = nClassId;
        m_Count = nCount;

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
            m_SetAttrLabel.text = QuartzTool.GetFormatSetAttr(tSet);
        }
    }

    public void OnItemClick()
    {
        if (m_Count <= 0)
        {
            Tab_QuartzClass tClass = TableManager.GetQuartzClassByID(m_ClassId, 0);
            if(tClass == null)
            {
                return;
            }
            ItemGain.ItemGainJumpByGainId(tClass.GainId);
            //if (QuartzEquipWindow.Instance != null && QuartzEquipWindow.Instance.GetChooseSlotType() == GlobeVar.INVALID_ID)
            //{

            //    Utils.CenterNotice(7990);
            //}
            //else
            //{
            //    Utils.CenterNotice(8005);
            //}
            return;
        }

        if (OrbmentController.Instance != null)
        {
            OrbmentController.Instance.HandleQuartzClassItemClick(m_ClassId);
        }
    }
}
