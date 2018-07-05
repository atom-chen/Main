using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.Table;
using Games;
using ProtobufPacket;
using Games.GlobeDefine;
using Games.LogicObj;
//消费的货币类型
public enum CURRENCY_TYPE
{
    CURRENCY_INVALID=-1,
    CURRENCY_YUANBAO=0,
}
//亲密度的增加类型
public enum INTIMACY_ADD_TYPE
{
    INTIMACY_ADD_INVALID=-1,
    INTIMACY_ADD_GIVE_GIFT = 0,//赠送礼物
    INTIMACY_ADD_ANSWER=1,//回答问题
}
//奖励的类型
public enum INTIMACY_AWARD_TYPE
{
    INTIMACY_AWARD_INVALID=-1,
    INTIMACY_AWARD_ITEM=0,
    INTIMACY_AWARD_GOLD=1,
    INTIMACY_AWARD_YUANBAO=2,
    INTIMACY_AWARD_CARDPIECE=3,
}
public struct GiftPair
{
    public int ItemId;
    public int IntimacyAdd;
    public int Value;
}
public struct BuyLimitPair
{
    public int CurNum;
    public int Total;
}
public struct AwardItem
{
    public int ItemId;
    public int ItemCount;
}

//已购买的同步数据
public class IntimacyBuyLimit
{
    private static _DBINTIMACY_LIMIT_BUY_LIST m_ButLimitList = new _DBINTIMACY_LIMIT_BUY_LIST();
    
    //根据GC包去同步客户端数据
    public static void SynchronizationWithServer(GC_INTIMACY_BUY_LIMIT_SYC packet)
    {
        if(packet!=null)
        {
            m_ButLimitList = packet.RestrictionsList;
        }
    }

    //拿到List限制/总次数
    public static List<BuyLimitPair> GetItemLimit(List<int> ItemId)
    {
        if(ItemId==null)
        {
            return null;
        }
        List<BuyLimitPair> ItemLimit = new List<BuyLimitPair>();
        for (int i = 0; i < ItemId.Count;i++)
        {
            //从List中找到物品
            BuyLimitPair limitPair = GetItemLimit(ItemId[i]);
            ItemLimit.Add(limitPair);
        }
        return ItemLimit;
    }

    //拿到该物品的限制次数
    public static BuyLimitPair GetItemLimit(int ItemId)
    {
        BuyLimitPair limitPair = new BuyLimitPair();
        if(ItemId!=null && m_ButLimitList!=null)
        {
            for (int i = 0; i < m_ButLimitList.RestrictionsList.Count; i++)
            {
                if (m_ButLimitList.RestrictionsList[i].ItemId == ItemId)
                {
                    limitPair.CurNum = m_ButLimitList.RestrictionsList[i].LimitNum;
                    //去表中找数据
                    Tab_CardIntimacyGiftItem tGiftItem = TableManager.GetCardIntimacyGiftItemByID(ItemId, 0);
                    if (tGiftItem != null)
                    {
                        limitPair.Total = tGiftItem.BuyLimit;
                        break;
                    }
                }
            }
        }
        return limitPair;
    }

    private IntimacyBuyLimit()
    {

    }

}

public class IntimacyRoot : MonoBehaviour
{
#region 属性定义
    public const int m_GiftClassID = 3;//礼物1级分类ID
    public int GiftClassID
    {
        get
        {
            return m_GiftClassID;
        }
    }

    public const int m_GiftSubClassID = 12;//礼物2级分类ID
    public int GiftSubClassID
    {
        get
        {
            return m_GiftSubClassID;
        }
    }


    public GameObject m_IntimacyBag;
    public GameObject m_IntimacyWindow;
    public GameObject m_IntimacyStory;
    public float m_WaitTime = 2.0f;


    private static IntimacyRoot _Instance;
    public static IntimacyRoot Instance
    {
        get
        {
            return _Instance;
        }
    }
    void Awake()
    {
        _Instance = this;
        //同步已购买数据
        CG_INTIMACY_BUY_LIMIT_SYC_PAK pak = new CG_INTIMACY_BUY_LIMIT_SYC_PAK();
        pak.SendPacket();
    }
    private Card m_Card;
    private Tab_Card m_tCard;
    private Tab_CardIntimacyGift m_tGift;//当前card的Gift信息
    private List<int> m_GiftItemId=new List<int>();//当前符灵关心的物品ID

    public GameObject NonIntimacyNode;
    public GameObject LeftNode;
    public GameObject RightNode;
    public GameObject WindowNode;
    public Card card
    {
        get
        {
            return m_Card;
        }
    }

    public Tab_CardIntimacyGift Gift
    {
        get
        {
            return m_tGift;
        }
    }

    //更新当前card信息
    void OnEnable()
    {
        if (GameManager.PlayerDataPool!=null&&GameManager.PlayerDataPool.PlayerCardBag!=null &&
            CardInfoWindow.Instance != null && CardInfoWindow.Instance.Card != null)
        {
            m_Card = GameManager.PlayerDataPool.PlayerCardBag.GetCard(CardInfoWindow.Instance.Card.Guid);
        }
        PlayerData.delegateCommonPackItemChanged += UpdateIntimacyGiftBag;
        //拿到它的信息，更新到List
        if(m_Card!=null)
        {
            m_tCard = m_Card.GetTable_Card();
            m_tGift = m_Card.GetTableGift();
        }
        if(m_GiftItemId!=null)
        {
            m_GiftItemId.Clear();
        }

        //添加它关心的物品ID到List
        if(m_tGift!=null && m_GiftItemId!=null)
        {
            for (int i = 0; i < m_tGift.getItemIDCount(); i++)
            {
                if (m_tGift.GetItemIDbyIndex(i) != -1)
                {
                    m_GiftItemId.Add(m_tGift.GetItemIDbyIndex(i));
                }
            }
        }

        //默认打开Bag，关闭window
        if (m_IntimacyBag != null)
        {
            m_IntimacyBag.SetActive(true);
        }
        if (m_IntimacyWindow != null)
        {
            m_IntimacyWindow.SetActive(false);
        }
        if(m_IntimacyStory!=null)
        {
            m_IntimacyStory.SetActive(true);
        }
        //更新故事
        if (IntimacyStory.Instance != null)
        {
            IntimacyStory.Instance.UpdateStory(m_Card);
        }

        //显示关闭特殊卡界面
        Tab_Card CardTab = m_Card.GetTable_Card();
        if (CardTab != null)
        {
            if (CardTab.ClassId != (int)Card.CLASS_TYPE.NORMAL)
            {
                NonIntimacyNode.SetActive(true);
                RightNode.SetActive(false);
                LeftNode.SetActive(false);
                WindowNode.SetActive(false);
            }
            else
            {
                NonIntimacyNode.SetActive(false);
            }
        }
    }
    void OnDisable()
    {
        PlayerData.delegateCommonPackItemChanged -= UpdateIntimacyGiftBag;
    }
#endregion


    #region 功能函数
    /// <summary>
    /// 拿到该礼物对当前符灵增加的亲密度值
    /// </summary>
    /// <param name="ItemDataId">ItemID</param>
    /// <returns>增加的亲密度值</returns>
    public int GetIntimacyAddWithGift(int ItemDataId)
    {
        if (m_tGift != null)
        {
            for (int i = 0; i < m_tGift.getItemIDCount(); i++)
            {
                //如果找到了
                if (m_tGift.GetItemIDbyIndex(i)!=-1 &&m_tGift.GetItemIDbyIndex(i) == ItemDataId)
                {
                    return m_tGift.GetIntimacyAddbyIndex(i);
                }
            }
        }
        return -1;
    }

    /// <summary>
    /// 判断当前物品是否是符灵关心的
    /// </summary>
    /// <param name="ItemId">物品ID</param>
    /// <returns>是否关心</returns>
    public bool IsCardCare(int ItemId)
    {
        if(m_GiftItemId!=null)
        {
            return m_GiftItemId.Contains(ItemId);
        }
        return false;
    }

    /// <summary>
    /// 拿到当前符灵favorite的礼物List
    /// </summary>
    /// <returns>喜欢的的礼物List</returns>
    public List<GiftPair> GetFavoriteGiftList()
    {
        if(m_tGift==null || m_Card == null)
        {
            return null;
        }
        List<GiftPair> favoriteGiftList = new List<GiftPair>();
        //认为前1/3是喜爱的
        for (int i = 0; i < m_tGift.getItemIDCount()/3; i++)
        {
            if (m_tGift.GetItemIDbyIndex(i) != -1)
            {
                GiftPair pair = new GiftPair();
                pair.ItemId = m_tGift.GetItemIDbyIndex(i);
                pair.IntimacyAdd = m_tGift.GetIntimacyAddbyIndex(i);
                favoriteGiftList.Add(pair);
            }
        }
        return favoriteGiftList;
    }

    /// <summary>
    /// 更新亲密度信息显示
    /// </summary>
    public void UpdateIntimacyInfo()
    {
        if (m_Card == null)
        {
            return;
        }
        //更新亲密度信息显示
        if (IntimacyView.Instance != null)
        {
            IntimacyView.Instance.UpdateIntimacy(m_Card);
        }
        //亲密度改变造成符灵故事开启的改变
        if(IntimacyStory.Instance!=null)
        {
            IntimacyStory.Instance.UpdateStory(m_Card);
        }
    }


    /// <summary>
    /// 更新礼物背包显示
    /// </summary>
    public void UpdateIntimacyGiftBag()
    {
        if (m_Card == null)
        {
            return;
        }
        //更新礼物背包显示
        if (IntimacyGiftBag.Instance != null)
        {
            IntimacyGiftBag.Instance.UpdateAll();
        }
    }
    
    public void UpdateLimitBuy()
    {
        //更新限购次数
        if (IntimacyWindow.Instance != null && IntimacyWindow.Instance.IsBuyWindowOpen())
        {
            IntimacyWindow.Instance.UpdateBuyWindow();
        }
    }
    /// <summary>
    /// 拿到可选的下级称号List
    /// </summary>
    /// <returns>下级称号List</returns>
    public List<Tab_CardIntimacyTitle> GetNextLevelTitle()
    {
        if(m_Card==null)
        {
            return null;
        }
        List<Tab_CardIntimacyTitle> m_NextTitle = new List<Tab_CardIntimacyTitle>();
        //查表 找到哪个的上级指针指向当前Title
        var AllTitles = TableManager.GetCardIntimacyTitle();
        if(AllTitles!=null)
        {
            foreach (var item in AllTitles)
            {
                if (item.Value == null || item.Value.Count <= 0 || item.Value[0].PreIntimacyTitleID == -1)
                {
                    continue;
                }
                if (item.Value[0].PreIntimacyTitleID == m_Card.IntimacyTitleID)
                {
                    m_NextTitle.Add(item.Value[0]);
                }
            }
        }
        return m_NextTitle;
    }


    //是否还能增加亲密度
    public bool IsCanAddIntimacy()
    {
        if(m_Card==null)
        {
            return false;
        }
        Tab_CardIntimacyTitle curTitle=TableManager.GetCardIntimacyTitleByID(m_Card.IntimacyTitleID,0);
        if(curTitle==null)
        {
            return false;
        }
        Tab_CardIntimacyLevel nextLevel = TableManager.GetCardIntimacyLevelByID(curTitle.IntimacyLevel + 1, 0);
        if(nextLevel==null)
        {
            return false;
        }
        return true;
    }

    //拿到当前称号链上所有称号
    public List<int> GetLinkTitle()
    {
        if(m_Card==null)
        {
            return null;
        }
        List<int> link = new List<int>();
        int curTitleId = m_Card.IntimacyTitleID;
        while(curTitleId!=-1)
        {
            link.Add(curTitleId);
            Tab_CardIntimacyTitle title=TableManager.GetCardIntimacyTitleByID(curTitleId, 0);
            if(title!=null)
            {
                curTitleId = title.PreIntimacyTitleID;
            }
            else
            {
                curTitleId = -1;
            }
        }

        return link;
    }


    //获取当前奖励的Atlas
    public List<UIAtlas> GetUIAtlas(int IntimacyTitleId)
    {
        if (m_Card == null)
        {
            return null;
        }
        Tab_CardIntimacyAward award = TableManager.GetCardIntimacyAwardByID(m_Card.CardId, 0);
        if (award != null)
        {
            Tab_CardIntimacyTitle title = TableManager.GetCardIntimacyTitleByID(IntimacyTitleId, 0);
            int level = -1;
            if (title != null)
            {
                level = title.IntimacyLevel;
            }
            List<UIAtlas> atlas = new List<UIAtlas>();
            //去取该阶的奖励
            switch (level)
            {
                case 2:
                    //去找2级的信息
                    for (int i = 0; i < award.getAwardType2Count(); i++)
                    {
                        if (award.GetAwardType2byIndex(i) == -1)
                        {
                            continue;
                        }
                        else
                        {
                            //去读信息
                            Tab_Item item = TableManager.GetItemByID(award.GetAwardItemID2byIndex(i), 0);
                            if (item != null)
                            {
                                UIAtlas temp = AssetManager.GetAtlas(item.Atlas);
                                atlas.Add(temp);
                            }

                        }
                    }
                    break;
                case 3:
                    //去找3级的信息
                    for (int i = 0; i < award.getAwardType3Count(); i++)
                    {
                        if (award.GetAwardType3byIndex(i) == -1)
                        {
                            continue;
                        }
                        else
                        {
                            //去读信息
                            Tab_Item item = TableManager.GetItemByID(award.GetAwardItemID3byIndex(i), 0);
                            if (item != null)
                            {
                                UIAtlas temp = AssetManager.GetAtlas(item.Atlas);
                                atlas.Add(temp);
                            }
                        }
                    }
                    break;
                case 4:
                    //去找4级的信息
                    //去找2级的信息
                    for (int i = 0; i < award.getAwardType4Count(); i++)
                    {
                        if (award.GetAwardType4byIndex(i) == -1)
                        {
                            continue;
                        }
                        else
                        {
                            //去读信息
                            Tab_Item item = TableManager.GetItemByID(award.GetAwardItemID4byIndex(i), 0);
                            if (item != null)
                            {
                                UIAtlas temp = AssetManager.GetAtlas(item.Atlas);
                                atlas.Add(temp);
                            }

                        }
                    }
                    break;
            }
            return atlas;
        }
        return null;
    }

    /// <summary>
    /// 获取当前奖励的SpriteName
    /// </summary>
    public List<string> GetSpriteName(int IntimacyTitleId)
    {
        if(m_Card==null)
        {
            return null;
        }
        Tab_CardIntimacyAward award = TableManager.GetCardIntimacyAwardByID(m_Card.CardId, 0);//当前符灵的奖励
        if (award != null)
        {
            Tab_CardIntimacyTitle title = TableManager.GetCardIntimacyTitleByID(IntimacyTitleId, 0);
            int level = -1;
            if(title!=null)
            {
                level = title.IntimacyLevel;
            }
            List<string> spriteName = new List<string>();
            //去取该阶的奖励
            switch (level)
            {
                case 2:
                    //去找2级的信息
                    for (int i = 0; i < award.getAwardType2Count(); i++)
                    {
                        if (award.GetAwardType2byIndex(i) == -1)
                        {
                            continue;
                        }
                        else
                        {
                            //去读信息
                            Tab_Item item = TableManager.GetItemByID(award.GetAwardItemID2byIndex(i), 0);
                            if (item != null)
                            {
                                string temp = item.Icon;
                                spriteName.Add(temp);
                            }

                        }
                    }
                    break;
                case 3:
                    //去找3级的信息
                    for (int i = 0; i < award.getAwardType3Count(); i++)
                    {
                        if (award.GetAwardType3byIndex(i) == -1)
                        {
                            continue;
                        }
                        else
                        {
                            //去读信息
                            Tab_Item item = TableManager.GetItemByID(award.GetAwardItemID3byIndex(i), 0);
                            if (item != null)
                            {
                                string temp = item.Icon;
                                spriteName.Add(temp);
                            }

                        }
                    }
                    break;
                case 4:
                    //去找4级的信息
                    for (int i = 0; i < award.getAwardType4Count(); i++)
                    {
                        if (award.GetAwardType4byIndex(i) == -1)
                        {
                            continue;
                        }
                        else
                        {
                            //去读信息
                            Tab_Item item = TableManager.GetItemByID(award.GetAwardItemID4byIndex(i), 0);
                            if (item != null)
                            {
                                string temp = item.Icon;
                                spriteName.Add(temp);
                            }
                        }
                    }
                    break;
            }
            return spriteName;
        }
        return null;
    }

    //拿到当前奖励物品
    public List<AwardItem> GetItem(int IntimacyTitleId)
    {
        if (m_Card == null)
        {
            return null;
        }
        Tab_CardIntimacyAward award = TableManager.GetCardIntimacyAwardByID(m_Card.CardId, 0);
        if (award != null)
        {
            Tab_CardIntimacyTitle title = TableManager.GetCardIntimacyTitleByID(IntimacyTitleId, 0);
            int level = -1;
            if (title != null)
            {
                level = title.IntimacyLevel;
            }
            List<AwardItem> count = new List<AwardItem>();
            //去取该阶的奖励
            switch (level)
            {
                case 2:
                    //去找2级的信息
                    for (int i = 0; i < award.getAwardType2Count()&&i<award.getAwardCount2Count(); i++)
                    {
                        if (award.GetAwardType2byIndex(i) == -1)
                        {
                            continue;
                        }
                        if (award.GetAwardCount2byIndex(i) != -1)
                        {
                            AwardItem item=new AwardItem();
                            item.ItemId=award.GetAwardItemID2byIndex(i);
                            item.ItemCount=award.GetAwardCount2byIndex(i);
                            count.Add(item);
                        }
                    }

                    break;
                case 3:
                    //去找3级的信息
                    for (int i = 0; i < award.getAwardType3Count() && i < award.getAwardCount3Count(); i++)
                    {
                        if (award.GetAwardType3byIndex(i) == -1)
                        {
                            continue;
                        }
                        if (award.GetAwardCount3byIndex(i) != -1)
                        {
                            AwardItem item = new AwardItem();
                            item.ItemId = award.GetAwardItemID3byIndex(i);
                            item.ItemCount = award.GetAwardCount3byIndex(i);
                            count.Add(item);
                        }
                    }
                    break;
                case 4:
                    //去找3级的信息
                    for (int i = 0; i < award.getAwardType4Count() && i < award.getAwardCount4Count(); i++)
                    {
                        if (award.GetAwardType4byIndex(i) == -1)
                        {
                            continue;
                        }
                        if (award.GetAwardCount4byIndex(i) != -1)
                        {
                            AwardItem item = new AwardItem();
                            item.ItemId = award.GetAwardItemID4byIndex(i);
                            item.ItemCount = award.GetAwardCount4byIndex(i);
                            count.Add(item);
                        }
                    }
                    break;
            }
            return count;
        }
        return null;
    }


    //拿到当前奖励故事的ID
    public int GetAwardStoryID(int intimecyLevel)
    {
        Tab_CardIntimacyAward tAward = TableManager.GetCardIntimacyAwardByID(card.CardId, 0);
        if(tAward!=null)
        {
            switch(intimecyLevel)
            {
                //如果是2级
                case 2:
                    return tAward.Award2StoryID;
                case 3:
                    return tAward.Award3StoryID;
                case 4:
                    return tAward.Award4StoryID;
            }
        }
        return -1;
    }

    //展示符灵对该礼物的喜爱程度（根据下标）
    public void ShowLikeDegree(int ItemId)
    {
        if(m_tCard==null || m_Card==null)
        {
            return;
        }
        Tab_CardIntimacyGift tGift = TableManager.GetCardIntimacyGiftByID(m_tCard.GiftItemID, 0);
        if (tGift == null)
        {
            return;
        }
        int ItemIndex = -1;
        for(int i=0;i<tGift.getItemIDCount();i++)
        {
            if(tGift.GetItemIDbyIndex(i)==ItemId)
            {
                ItemIndex = i;
                break;
            }
        }
        //认为是喜欢
        if(ItemIndex<tGift.getItemIDCount()/3)
        {
            Utils.CenterNotice(StrDictionary.GetClientDictionaryString("#{6739}", this.m_Card.GetName()));
        }
        else
        {
            //认为一般喜欢
            if(ItemIndex<tGift.getItemIDCount()/3*2)
            {
                Utils.CenterNotice(StrDictionary.GetClientDictionaryString("#{6740}", this.m_Card.GetName()));
            }
            //认为是不喜欢
            else
            {
                Utils.CenterNotice(StrDictionary.GetClientDictionaryString("#{6741}", this.m_Card.GetName()));
            }
        }

    }


    //播放升阶动画
    private void PlayIntimacyUpAnimation()
    {
        Tab_CardIntimacyAward tAward = m_Card.GetIntimacyAward();
        if (tAward==null)
        {
            return;
        }
        if(CardInfoWindow.Instance!=null)
        {
            Obj_Fake obj = CardInfoWindow.Instance.Fake;
            if(obj!=null)
            {
                obj.PlayAnimation(tAward.AnimationID);
            }
            if (null != GameManager.SoundManager)
            {
                GameManager.SoundManager.PlayRealSound(tAward.SoundID);
            }
        }

    }

#endregion


#region 事件函数
    //点击礼物购买
    public void OnClickBuy()
    {
        if (m_IntimacyWindow!=null)
        {
            m_IntimacyWindow.SetActive(true);
            if(IntimacyWindow.Instance!=null)
            {
                IntimacyWindow.Instance.OpenBuyWindow(GetFavoriteGiftList());
            }
        }
    }

    //弹出称号选择窗口
    public void OpenChooseWindow()
    {
        if (m_IntimacyWindow != null && m_Card!=null)
        {
            m_IntimacyWindow.SetActive(true);
            if(IntimacyWindow.Instance!=null)
            {
                IntimacyWindow.Instance.OpenChooseWindow(GetNextLevelTitle(), m_Card.CardId);
            }
        }
    }

    //弹出等级提升提示
    public void UpdateIntimacyTitle()
    {
        if (CardBagController.Instance != null)
        {
            CardBagController.Instance.RefreshCardRed();
        } 
        
        PlayIntimacyUpAnimation();
        if (m_IntimacyWindow != null)
        {
            m_IntimacyWindow.SetActive(true);
            IntimacyWindow.Instance.CloseChooseWindow();
        }
        StartCoroutine(OpenIntimacyUpWindow());
    }
    IEnumerator OpenIntimacyUpWindow()
    {
        yield return new WaitForSeconds(m_WaitTime);

        if (m_IntimacyWindow!= null && m_Card!=null)
        {
            m_IntimacyWindow.SetActive(true);
            if(IntimacyWindow.Instance!=null)
            {
                IntimacyWindow.Instance.OpenUpWindow(m_Card.IntimacyTitleID);
            }
        }
        //更新界面表现
        if(IntimacyWindow.Instance!=null)
        {
            IntimacyView.Instance.UpdateIntimacy(m_Card);
        }
    }

    //称号预览窗口
    public void OpenPreviewWindow()
    {
        if(m_IntimacyWindow!=null && m_Card!=null)
        {
            m_IntimacyWindow.SetActive(true);
            if(IntimacyWindow.Instance!=null)
            {
                IntimacyWindow.Instance.OpenTitlePreviewWindow(m_Card.IntimacyTitleID);
            }
        }
    }

#endregion
}
