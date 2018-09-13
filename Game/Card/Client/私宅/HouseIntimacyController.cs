using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.GlobeDefine;
using Games.LogicObj;
using Games.Table;
using UnityEngine;
public class HouseIntimacyController : MonoBehaviour 
{
    private static HouseIntimacyController _Ins;
    public static HouseIntimacyController Instance { get { return _Ins; } }

    //--------------------component------------------------
    public IntimacyGiftItem[] m_GiftItem;
    public UIEventTrigger m_ExitEvent;

    //------------------------data-----------------------------
    public static Card mCard;
    private List<int> m_GiftList;

    void Awake()
    {
        _Ins = this;
    }
    void OnEnable()
    {
        if (mCard == null)
        {
            return;
        }
        m_GiftList = mCard.GetIntimacyGift();
        for (int i = 0; i < m_GiftItem.Length; i++)
        {
            if (i < m_GiftList.Count)
            {
                m_GiftItem[i].InitItem(m_GiftList[i], mCard);
            }
            else
            {
                m_GiftItem[i].InitItem(GlobeVar.INVALID_ID, null);
            }
        }
    }
    void Start()
    {
        PlayerData.delegateCommonPackItemChanged += Refresh;
        m_ExitEvent.onClick.Add(new EventDelegate(Close));
    }
    void OnDestroy()
    {
        PlayerData.delegateCommonPackItemChanged -= Refresh;
        _Ins = null;
        mCard = null;
    }

    public static void Show()
    {
        if (mCard != null)
        {
            UIManager.ShowUI(UIInfo.HouseIntimacyRoot, null, null, UIStack.StackType.PushAndPop);
        }
    }
    public static void Show(Card card)
    {
        if(card !=null)
        {
            mCard = card;
            UIManager.ShowUI(UIInfo.HouseIntimacyRoot,null,null,UIStack.StackType.PushAndPop);
        }
    }
    public static void Close()
    {
        UIManager.CloseUI(UIInfo.HouseIntimacyRoot);
        HouseScene scene = GameManager.CurScene as HouseScene;
        if (scene != null)
        {
            scene.SwitchMode(HouseScene.HouseMode.NORMAL);
        }
    }

    public void Refresh()
    {
        if (mCard == null || m_GiftList == null)
        {
            return;
        }
        for (int i = 0; i < m_GiftItem.Length; i++)
        {
            if (i < m_GiftList.Count)
            {
                m_GiftItem[i].InitItem(m_GiftList[i], mCard);
            }
            else
            {
                m_GiftItem[i].InitItem(GlobeVar.INVALID_ID, null);
            }
        }
    }
}
