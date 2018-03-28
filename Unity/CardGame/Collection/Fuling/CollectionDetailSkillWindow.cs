using UnityEngine;
using System.Collections;
using Games.Table;
using Games;

public class CollectionDetailSkillWindow : MonoBehaviour
{
    //需要信息：技能名称1，技能图标1，技能TAB1，技能描述1，人妖界技能信息1，各等级技能信息
    public CollectionLevel2FulingDetail m_ParentFulingDetail;
    public UILabel m_SkillName = null; // 技能名称
    public UILabel[] m_SkillNameTab = null; // tab1
    public UISprite m_SkillIcon;//技能Icon 
    public UILabel m_Desc = null; // 技能描述
    public UILabel m_RenjieDesc = null; // 人妖界变化描述
    public UILabel m_YaojieDesc = null; // 人妖界变化描述

    public UILabel[] m_SkillLevelDesc;//技能等级描述
    public UITable m_SkillLevelGrid;//技能详情界面的Table
    public UIScrollView m_SkillScrollView;
    

    private int m_preSkillID = -1;

    void OnDisable()
    {
        m_preSkillID = -1;
    }

    public void UpdateWindow(int skillExId)
    {
        //如果是同一个ID，就关闭窗口，如果不同就照常显示
        if(m_preSkillID==skillExId)
        {
            this.gameObject.SetActive(false);
            return;
        }
        m_preSkillID = skillExId;
        Tab_SkillEx skillEx = TableManager.GetSkillExByID(skillExId, 0);
        if(skillEx==null)
        {
            return;
        }
        Tab_SkillBase skillBase = TableManager.GetSkillBaseByID(skillEx.BaseID, 0);
        if (null == skillBase)
        {
            return;
        }
        if(m_SkillIcon!=null)
        {
            m_SkillIcon.spriteName = skillBase.Icon;//Icon
        }
        if (m_SkillName!=null)
        {
            m_SkillName.text = skillBase.Name;//技能名称
        }
        Tab_SkillDesc tSkillDesc = TableManager.GetSkillDescByID(skillEx.Id, 0);
        if (tSkillDesc == null)
        {
            return;
        }
        if(m_RenjieDesc!=null)
        {
            m_RenjieDesc.text = tSkillDesc.EnvironmentExDescDay;//人界
        }
        if(m_YaojieDesc!=null)
        {
            m_YaojieDesc.text = tSkillDesc.EnvironmentExDescNight;//妖界
        }

        for (int i = 0; i < tSkillDesc.getDamageTypeCount() && i < m_SkillNameTab.Length; i++)
        {
            if(m_SkillNameTab[i]!=null)
            {
                m_SkillNameTab[i].text = Utils.GetSkillDamageTypeName(tSkillDesc.GetDamageTypebyIndex(i));//TAB
            }
        }
        if(m_Desc!=null)
        {
            m_Desc.text = skillEx.Description;//技能描述
        }
        if (m_SkillLevelDesc==null)
        {
            return;
        }
        bool isNotLevelInfo = false;
        //各等级技能信息
        for (int i = 0; i < m_SkillLevelDesc.Length; i++)
        {
            if (m_SkillLevelDesc[i]==null)
            {
                continue;
            }
            //取出信息
            Tab_SkillDesc desc = TableManager.GetSkillDescByID(skillEx.Id + i + 1, 0);
            if (desc != null && !isNotLevelInfo)
            {
                m_SkillLevelDesc[i].gameObject.SetActive(true);
                m_SkillLevelDesc[i].text = desc.EnhanceDesc;
                if( desc.EnhanceDesc.Equals("该技能无法升级"))
                {
                    isNotLevelInfo = true;
                }
            } 
            else
            {
                m_SkillLevelDesc[i].gameObject.SetActive(false);
            }
        }
        //重排
        if(m_SkillLevelGrid!=null)
        {
            m_SkillLevelGrid.Reposition();
        }
        if(m_SkillScrollView!=null)
        {
            m_SkillScrollView.ResetPosition();
        }

    }

    public void ClickCloseWindow()
    {
        this.gameObject.SetActive(false);
    }
}
