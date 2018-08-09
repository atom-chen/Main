using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseCardItem : MonoBehaviour
{
    public BattleCardItem m_CardInfo;
    public UIEventTrigger m_ShowBtn;
    public UIEventTrigger m_HideBtn;
    public GameObject m_ChooseFrame;
    public GameObject m_ShowSign;

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
        bool res = GameManager.PlayerDataPool.YardData.IsIn(card.Guid);
        m_ShowSign.SetActive(res);
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
        m_ChooseFrame.SetActive(isChoose);
        //如果没有选中
        if(!isChoose)
        {
            m_ShowBtn.gameObject.SetActive(false);
            m_HideBtn.gameObject.SetActive(false);
        }
        else
        {
            bool res = GameManager.PlayerDataPool.YardData.IsIn(curCard.Guid);
            //如果选中
            m_ShowBtn.gameObject.SetActive(!res);
            m_HideBtn.gameObject.SetActive(res);
        }
    }
}
