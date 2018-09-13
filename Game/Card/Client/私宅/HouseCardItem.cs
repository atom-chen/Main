using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games;

public class HouseCardItem : MonoBehaviour
{
    public BattleCardItem m_CardInfo;
    public UIButtonScale m_ButtonScale;
    public UIEventTrigger m_ShowBtn;
    public UIEventTrigger m_HideBtn;
    public GameObject m_ChooseFrame;
    public GameObject m_AlreadShowSign;

    public Card curCard
    {
        get
        {
            return m_CardInfo.card;
        }
    }
    void Awake()
    {
        m_ChooseFrame.SetActive(false);
    }
    void Start()
    {
        m_ShowBtn.onClick.Add(new EventDelegate(OnClickShow));
        m_HideBtn.onClick.Add(new EventDelegate(OnClickHide));
    }

    public void Refresh(Card card)
    {
        if (card == null)
        {
            this.gameObject.SetActive(false);
            return;
        }
        this.gameObject.SetActive(true);
        if (m_CardInfo != null)
        {
            m_CardInfo.Refresh(card);
        }
        bool res = GameManager.PlayerDataPool.YardData.IsInCurYard(card.Guid);
        m_AlreadShowSign.SetActive(res);
        //已经展示的不接受点击
        m_ButtonScale.enabled = !res;
        SetChoose(false);
    }

    private void OnClickShow()
    {
        if (m_CardInfo!=null && m_CardInfo.card!=null)
        {
            Yard.SendPut(m_CardInfo.card.Guid);
        }

    }

    private void OnClickHide()
    {
        if (m_CardInfo != null && m_CardInfo.card != null)
        {
            Yard.SendTake(m_CardInfo.card.Guid);
        }
    }

    public void SetChoose(bool isChoose)
    {
        if (curCard == null)
        {
            return;
        }
        //如果没有选中
        if(!isChoose)
        {
            m_ShowBtn.gameObject.SetActive(false);
            m_ChooseFrame.SetActive(false);
        }
        //如果选中
        else
        {
            bool res = GameManager.PlayerDataPool.YardData.IsInCurYard(curCard.Guid);
            //如果已经展示，直接给提示
            if(res)
            {
                m_ChooseFrame.SetActive(false);
                Utils.CenterNotice(8354);
            }
            else
            {
                m_ShowBtn.gameObject.SetActive(!res);                 //弹出展示按钮
                m_ChooseFrame.SetActive(true);
            }

        }
    }
}
