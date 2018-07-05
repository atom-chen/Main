using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.GlobeDefine;
using Games;
using Games.Table;
//********************************************************************
// 描述: 亲密度总界面
// 作者: wangbiyu
// 创建时间: 2018-3-8
//
//
//********************************************************************
public class IntimacyView : MonoBehaviour
{
    public UILabel m_Title;
    public UISlider m_IntimacySlider;
    public UILabel m_SliderText;

    public IntimacyGiftBag m_ItemList;
    public GameObject m_Default;



    private static IntimacyView _Instance;
    public static IntimacyView Instance
    {
        get
        {
            return _Instance;
        }
    }
    void Awake()
    {
        _Instance = this;
    }
    void OnEnable()
    {
        if(IntimacyRoot.Instance!=null)
        {
            UpdateIntimacy(IntimacyRoot.Instance.card);
        }
        if(m_ItemList!=null)
        {
            m_ItemList.gameObject.SetActive(true);
        }
    }
    //更新窗口显示
    public void UpdateIntimacy(Card m_Card)
    {
        if(m_Card==null)
        {
            return;
        }
        int intLv = m_Card.CalcIntimacyLevel();//拿到当前亲密度等级
        //如果读不到，说明数据有误
        if(intLv==-1)
        {
            if(m_Title!=null)
            {
                m_Title.text = "读不到称号";
            }
            if(m_IntimacySlider!=null)
            {
                m_IntimacySlider.value = 0.22f;
            }
            if(m_SliderText!=null)
            {
                m_SliderText.text = StrDictionary.GetDicByID(GlobeVar.CARD_INTIMACY_PROG, m_Card.Intimacy, 0); // 亲密度: XXX/XXX
            }
            return;
        }
        Tab_CardIntimacyLevel intimacyLevel = TableManager.GetCardIntimacyLevelByID(intLv+1, 0);
        float nextLvExp = 0;
        //如果下一级为空说明满级
        if (intimacyLevel == null)
        {
            nextLvExp = -1;
        }
        else
        {
            nextLvExp = intimacyLevel.Require;//下一级所需经验
        }
        Tab_CardIntimacyTitle intimacyTitle = TableManager.GetCardIntimacyTitleByID(m_Card.IntimacyTitleID, 0);//拿到当前亲密度称号
        //当前已满级情况
        if (nextLvExp == -1)
        {
            if(m_IntimacySlider!=null)
            {
                m_IntimacySlider.value = 1f;
            }
            if(m_SliderText!=null)
            {
                m_SliderText.text = StrDictionary.GetDicByID(GlobeVar.CARD_INTIMACY_PROG_MAX); // 亲密度: 已满
            }
        }
       //经验不满的情况
        else if (nextLvExp - m_Card.Intimacy > 0)
        {
            if(m_IntimacySlider!=null)
            {
                m_IntimacySlider.value = (float)(m_Card.Intimacy / nextLvExp);
            }
            if(m_SliderText!=null)
            {
                m_SliderText.text = StrDictionary.GetDicByID(GlobeVar.CARD_INTIMACY_PROG, m_Card.Intimacy, nextLvExp); // 亲密度: XXX/XXX
            }
        } 
        //经验值满的情况
        else if (nextLvExp - m_Card.Intimacy <= 0)
        {
            //弹出选择窗口
            if(IntimacyRoot.Instance!=null)
            {
                IntimacyRoot.Instance.OpenChooseWindow();
            }
            if(m_IntimacySlider!=null)
            {
                //更新亲密度显示
                m_IntimacySlider.value = 1f;
            }
            if(m_SliderText!=null)
            {
                m_SliderText.text = StrDictionary.GetDicByID(GlobeVar.CARD_INTIMACY_PROG, m_Card.Intimacy, nextLvExp); // 亲密度: XXX/XXX
            }
        }
        //称号名展示
        if (intimacyTitle != null && m_Title!=null)
        {
            m_Title.text = intimacyTitle.Title;
        }
    }



}
