using System.Collections;
using System.Collections.Generic;
using Games;
using Games.GlobeDefine;
using Games.Table;
using UnityEngine;

public class QuartzSetRecommendItem : MonoBehaviour
{
    public UITexture m_ClassPic;
    public UILabel m_NameLabel;             
    public UILabel m_AttrLabel;
    public GameObject m_JumpBtn;      

    private int m_SetId = GlobeVar.INVALID_ID;
    private GameObject mParentWin;

    public void Init(int nSetId, GameObject parentWin)
    {
        mParentWin = parentWin;
        m_SetId = nSetId;
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

        gameObject.SetActive(true);

        m_ClassPic.mainTexture = Utils.LoadTexture("Texture/T_StarSoul/" + tClass.Pic);
        m_NameLabel.text = tClass.Name;

        m_AttrLabel.text = "";
        m_AttrLabel.text += QuartzTool.GetFormatSetAttr_Recomand(tSet);

        m_JumpBtn.SetActive(UIBattleEnd.s_EndFlag != UIBattleEnd.ENUM_ENDFLAG.OPENUI_FULINGLU);
    }

    public void OnJumpClick()
    {
        var tSet = TableManager.GetQuartzSetByID(m_SetId, 0);
        if (tSet == null)
        {
            return;
        }

        var tClass = TableManager.GetQuartzClassByID(tSet.NeedClassId, 0);
        if (tClass == null)
        {
            return;
        }

        var tGain = TableManager.GetItemGainByID(tClass.GainId, 0);
        if (tGain == null)
        {
            return;
        }

        if (tGain.NeedFunction > 0 && !TutorialManager.IsFunctionUnlock(tGain.NeedFunction))
        {
            TutorialManager.SendFunctionLockNotice(tGain.NeedFunction);
            return;
        }

        var qlist = GameManager.PlayerDataPool.PlayerQuartzBag.QuartzFilter(tClass.Id);
        if (qlist.Count > 0) //有星魂直接跳转到星魂列表
        {

            if (OrbmentController.Instance != null)
            {
                OrbmentController.Instance.m_QuartzTips.OnCloseClick();
                OrbmentController.Instance.HandleQuartzClassItemClick(tSet.NeedClassId);
            }
            if (mParentWin != null)
            {
                mParentWin.SetActive(false);
            }
        }
        else
        {
            ItemGain.QuartzSetGainJump(m_SetId);
        }
    }
}
