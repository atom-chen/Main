using System;
using UnityEngine;
using Games.LogicObj;
using Games.GlobeDefine;
using System.Collections;
using Games.Table;
using System.Collections.Generic;
using Games;

public class HouseCardShowController : MonoBehaviour 
{
    private static HouseCardShowController _Ins;
    public static HouseCardShowController Instance { get { return _Ins; } }

    //---------------------------------------Component------------------
    public ListWrapController m_Wrap;
    public UIEventTrigger m_ExitBtn;
    public UILabel m_Num;

    //SetActive
    public GameObject m_KongKongRuYe;
    public GameObject m_BagRoot;

    //---------------------------Data---------------------------
    private List<Card> m_CardList;
    private HouseCardItem[] m_CardItemArray;
    void Awake()
    {
        _Ins = this;
    }

    void Start()
    {
        m_CardList = GameManager.PlayerDataPool.PlayerCardBag.GetCardByIntimacyLevel_UpperBound(
            GlobeVar._GameConfig.m_nYardCardIntimacy);                        //拿到亲密度符合要求的符灵

        if (null != m_CardList)
        {
            m_CardList.Sort(CardTool.GetSortFunc(CardSortType.Rare));
            if (m_CardList.Count > 0)
            {
                m_BagRoot.SetActive(true);
                m_KongKongRuYe.SetActive(false);
            }
            else
            {
                m_BagRoot.SetActive(false);
                m_KongKongRuYe.SetActive(true);
            }

            if (GameManager.PlayerDataPool.YardData.ProtoYard != null &&
                GameManager.PlayerDataPool.YardData.ProtoYard.CardList != null)
            {
                m_Num.text = GameManager.PlayerDataPool.YardData.ProtoYard.CardList.Count.ToString();
            }
            
            m_Wrap.InitList(m_CardList.Count, OnUpdateCardItem, false);
        }

        m_ExitBtn.onClick.Add(new EventDelegate(CloseUI));
        HouseScene scene = GameManager.CurScene as HouseScene;
        if(scene!=null)
        {
            scene.DelSceneAddCard += OnAddCard;
            scene.DelSceneDelCard += OnDelCard;
        }

        m_CardItemArray = m_Wrap.GetComponentsInChildren<HouseCardItem>();
        //加委托
        if (m_CardItemArray != null)
        {
            foreach (HouseCardItem item in m_CardItemArray)
            {
                if (item == null || item.m_CardInfo == null)
                {
                    continue;
                }

                item.m_CardInfo.onClickCard += OnClickCard;
            }
        }
    }

    void OnDestroy()
    {
        _Ins = null;
        HouseScene scene = GameManager.CurScene as HouseScene;
        if (scene != null)
        {
            scene.DelSceneAddCard -= OnAddCard;
            scene.DelSceneDelCard -= OnDelCard;
        }
    }

    public static void Open()
    {
        UIManager.ShowUI(UIInfo.HouseCardShowRoot);
    }
    private static void CloseUI()
    {
        var scene = GameManager.CurScene as HouseScene;
        if (scene == null)
        {
            return;
        }
        scene.SwitchMode(HouseScene.HouseMode.NORMAL);
    }

    //wrap回调
    void OnUpdateCardItem(GameObject obj, int index)
    {
        if(obj == null || m_CardList== null)
        {
            return;
        }
        HouseCardItem item = obj.GetComponent<HouseCardItem>();
        if(item!=null)
        {
            if(index < m_CardList.Count)
            {
                item.Refresh(m_CardList[index]);
            }
            else
            {
                item.Refresh(null);
            }
        }
    }

    void OnAddCard(HouseScene.SceneCardInfo ci)
    {
        m_Wrap.UpdateAllItem();
        m_Num.text = GameManager.PlayerDataPool.YardData.ProtoYard.CardList.Count.ToString();
    }

    void OnDelCard(ulong guid)
    {
        m_Wrap.UpdateAllItem();
        m_Num.text = GameManager.PlayerDataPool.YardData.ProtoYard.CardList.Count.ToString();
    }

    //点击某张卡片
    private void OnClickCard(Card card, BattleCardItem item)
    {
        foreach(HouseCardItem temp in m_CardItemArray)
        {
            if (temp.m_CardInfo == item)
            {
                if(temp.m_ChooseFrame.activeSelf)
                {
                    temp.SetChoose(false);
                }
                else
                {
                    temp.SetChoose(true);
                }
            }
            else
            {
                temp.SetChoose(false);
            }

        }
    }

    private void CancelAllChoose()
    {
        foreach (HouseCardItem temp in m_CardItemArray)
        {
            temp.SetChoose(false);
        }
    }

}
