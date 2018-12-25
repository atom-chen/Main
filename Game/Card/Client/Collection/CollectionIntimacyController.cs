using Games.Table;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.Table;
using Games.GlobeDefine;

public class CollectionIntimacyController : MonoBehaviour 
{
    private static CollectionIntimacyController _Ins = null;
    public static CollectionIntimacyController Instance
    {
        get { return _Ins; }
    }
    public PageWrapController m_Page;
    public UILabel m_GiftInfoLabel;
    public GameObject m_ShowTipsObj;
    public GameObject m_LeftObj;
    public GameObject m_RightObj;

    private List<int> cardIDList = new List<int>();
    [HideInInspector]
    public int mCurChooseCard = GlobeVar.INVALID_ID;
    void Awake()
    {
        _Ins = this;
    }
    void Start()
    {
        cardIDList.Clear();
        foreach (var item in TableManager.GetCardIntimacyAward().Values)
        {
            foreach (var v in item)
            {
                if (v == null)
                {
                    continue;
                }
                if(v.LetterID!=GlobeVar.INVALID_ID)
                {
                    cardIDList.Add(v.Id);
                }

            }
        }
        if(cardIDList.Count>0)
        {
            mCurChooseCard = cardIDList[0];
        }
        m_Page.Init(cardIDList.Count, OnUpdateItem, OnUpdatePage);
        HandleOnClickItem(mCurChooseCard);
    }
    void OnDestroy()
    {
        _Ins = null;
    }
    public void OnUpdateItem(GameObject item, int index)
    {
        if (item == null || cardIDList == null)
        {
            return;
        }
        if (index < 0 || index >= cardIDList.Count)
        {
            item.SetActive(false);
            return;
        }
        CollectionIntimacyItem itemLogic = item.GetComponent<CollectionIntimacyItem>();
        if (itemLogic == null)
        {
            item.SetActive(false);
            return;
        }
        item.SetActive(false);
        itemLogic.Init(cardIDList[index]);
    }
    public void OnUpdatePage(int cur, int total)
    {
        //m_PageLabel.text = string.Format("{0}/{1}", cur, total);
        //计算实际idx
        int idx = (cur-1) * m_Page.ItemPerPage;
        if(idx>=0 && idx<cardIDList.Count)
        {
            mCurChooseCard = cardIDList[idx];
        }
        else
        {
            mCurChooseCard = GlobeVar.INVALID_ID;
        }
        m_Page.Refresh();
        HandleOnClickItem(mCurChooseCard);
        m_LeftObj.SetActive(cur != 1);
        m_RightObj.SetActive(cur != total);
    }

    public void OnClickShowTips()
    {
        IntimacyLetter.Show();
    }


    public void OnClickLeft()
    {
        m_Page.PageDown();
    }

    public void OnClickRight()
    {
        m_Page.PageUp();
    }

    public void OnClickExit()
    {
        UIManager.CloseUI(UIInfo.CollectionIntimacyRoot);
    }

    public void HandleOnClickItem(int cardId)
    {
        m_GiftInfoLabel.text = "";
        Tab_Card tCard = TableManager.GetCardByID(cardId, 0);
        if(tCard == null)
        {
            return;
        }
        Tab_CardIntimacyAward _TabAward = TableManager.GetCardIntimacyAwardByID(cardId, 0);
        if (_TabAward == null)
            return;
        Tab_Item tItem = TableManager.GetItemByID(_TabAward.LetterID, 0);
        if (tItem == null)
            return;
        mCurChooseCard = cardId;
        m_Page.Refresh();
        if (GameManager.PlayerDataPool.CollectionData != null)
        {
            bool bSuc = GameManager.PlayerDataPool.CollectionData.IsGetIntimacyLetter(cardId);
            if (bSuc)
            {
                m_GiftInfoLabel.text = tItem.Tips;
                m_ShowTipsObj.SetActive(true);
            }
            else
            {
                Tab_RoleBaseAttr tRoleBase = TableManager.GetRoleBaseAttrByID(tCard.GetRoleBaseIDStepbyIndex(0), 0);
                if(tRoleBase!=null)
                {
                    m_GiftInfoLabel.text = StrDictionary.GetDicByID(8853, tRoleBase.Name);
                }
                m_ShowTipsObj.SetActive(false);
            }
        }
    }
}
