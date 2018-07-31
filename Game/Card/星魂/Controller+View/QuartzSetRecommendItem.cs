﻿using System.Collections;
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

    private int m_SetId = GlobeVar.INVALID_ID;
    private GameObject mParentWin;

    public void Init(int nSetId, GameObject parentWin)
    {
        mParentWin = parentWin;

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
        m_AttrLabel.text += StrDictionary.GetClientDictionaryString("#{5155}", tSet.NeedCount);
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
                m_AttrLabel.text += ",";
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

            m_AttrLabel.text += string.Format("{0}+{1}", Utils.GetAttrRefixName((AttrRefixType)nRefixType), szAttrValue);
        }

        for (int i = 0; i < tSet.getImpactIdCount(); i++)
        {
            Tab_Impact tImpact = TableManager.GetImpactByID(tSet.GetImpactIdbyIndex(i), 0);
            if (tImpact == null)
            {
                continue;
            }

            m_AttrLabel.text += ",";
            m_AttrLabel.text += StrDictionary.GetClientDictionaryString("#{5154}", tImpact.Name);
        }

        m_SetId = nSetId;
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
