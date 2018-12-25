using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.Table;
using Games.GlobeDefine;

public class CollectionStarItem : MonoBehaviour 
{
    public UISprite m_QuartzIcon;
    public GameObject m_ChooseObj;
    public UILabel m_QuartzClassNameLabel;
    private Tab_QuartzClass m_tQuartzClass = null;
    public Tab_QuartzClass QuartzClass
    {
        get { return m_tQuartzClass; }
    }
    public void Refresh(Tab_QuartzClass quartzClass)
    {
        if(quartzClass == null)
        {
            m_tQuartzClass = null;
            this.gameObject.SetActive(false);
            return;
        }
        m_tQuartzClass = quartzClass;
        this.gameObject.SetActive(true);
        m_QuartzIcon.spriteName = QuartzTool.GetQuartzIconByClassId(quartzClass.Id);
        m_QuartzClassNameLabel.text = quartzClass.Name;
        if(CollectionStarController.Instance!=null)
        {
            m_ChooseObj.SetActive(CollectionStarController.Instance.m_ChooseClassId == m_tQuartzClass.Id);
        }
        if (GameManager.PlayerDataPool.CollectionData != null)
        {
            bool bSuc = GameManager.PlayerDataPool.CollectionData.IsQuartzClassGet(quartzClass.Id);
            if (bSuc)
            {
                m_QuartzIcon.color = GlobeVar.NORMALCOLOR;
            }
            else
            {
                m_QuartzIcon.color = GlobeVar.GRAYCOLOR;
            }
        }
    }

    public void OnClickItem()
    {
        if(CollectionStarController.Instance!=null)
        {
            CollectionStarController.Instance.HandleOnChooseItem(m_tQuartzClass);
        }
    }

    public void Choose(bool bChoose)
    {
        m_ChooseObj.SetActive(bChoose);
    }
}
