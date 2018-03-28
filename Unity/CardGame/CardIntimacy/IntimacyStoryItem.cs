using Games;
using Games.GlobeDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntimacyStoryItem : MonoBehaviour {
    private int m_StoryLineID=-1;

    public GameObject m_On;
    public GameObject m_Off;
    private bool m_bIsOpen = false;

    public void InitItem(int storyLineId,bool isFirst,bool isOpen)
    {
        if(m_On==null || m_Off==null)
        {
            return;
        }
        m_bIsOpen = isOpen;
        this.m_StoryLineID = storyLineId;
        m_On.SetActive(isFirst);
        m_Off.SetActive(!isFirst);
        this.gameObject.SetActive(isOpen);
    }

    public void OnClickBtn()
    {
        if(m_bIsOpen)
        {
            OnClickOn();
        }
        else
        {
            OnClickOff();
        }
    }
    private void OnClickOn()
    {
        if (-1 == m_StoryLineID|| null == CardInfoWindow.Instance.Card)
        {
            return;
        }

        if (CardInfoWindow.Instance != null)
        {
            CardInfoWindow.Instance.StopFakeClickSound();
        }

        GameManager.storyManager.SetPrecondition(ENTER_STORY_MODE_SOURCE.CARD_STORY, CardInfoWindow.Instance.Card.Guid);
        StoryHandler.ReqEnterStory(m_StoryLineID, false);
    }

    private void OnClickOff()
    {
        Utils.CenterNotice(6719);
    }
}
