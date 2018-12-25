
using Games.GlobeDefine;
using Games.Item;
using Games.Table;
using ProtobufPacket;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionData
{
    public static string FulingShadowPath = "Texture/FuLing/Card_Shadow/";
    public static string StarPicPath = "Texture/T_StarSoul/";
    public static string StarShadowPath = FulingShadowPath;
    public static string TalismanPortraitPath = "Texture/Common/";
    public static string TalismanShadowPath = FulingShadowPath;
    public _DBPlayerCollection m_CardCollection = new _DBPlayerCollection();      //卡牌收集状态
    public _DBPlayerCollection m_TalismanCollection = new _DBPlayerCollection();  //法宝收集状态
    public _DBPlayerCollection m_QuartzCollection = new _DBPlayerCollection();     //星魂收集状态
    public _DBPlayerCollection m_IntimacyCollection = new _DBPlayerCollection();    //亲密度收集状态
    public _CollectionGroupFinishList CollectionGroupFinishList = new _CollectionGroupFinishList();       //组合收集状态
    public void HandlePacket(GC_SYS_COLLECTIONGROUPLIST packet)
    {
        if (packet == null || CollectionGroupFinishList == null)
        {
            return;
        }
        CollectionGroupFinishList.AlreadyReceived = packet.AlreadyReceived;
    }
    public void HandlePacket(GC_COLLECTIONGROUP_RECEIVEAWARD packet)
    {
        if (CollectionGroupFinishList.AlreadyReceived == null)
        {
            return;
        }
        CollectionGroupFinishList.AlreadyReceived.Add(packet.GroupId);
    }
    public bool GetCollection(_DBPlayerCollection collection, int id, int nBitsIdx)
    {
        if (null == collection || null == collection.items)
        {
            return false;
        }

        if (nBitsIdx < 0 || nBitsIdx >= GlobeVar.COLLECTION_MAX_IDX)
        {
            return false;
        }

        uint flag = 0;
        foreach (var item in collection.items)
        {
            if (null != item && item.id == id)
            {
                flag = item.flag;
                break;
            }
        }
        return ((flag & (1 << nBitsIdx)) > 0) ? true : false;
    }

    //某张卡牌是否获得过
    public bool IsGetCard(int nCardID)
    {
        Tab_Card _tabCard = TableManager.GetCardByID(nCardID, 0);
        if (_tabCard == null || m_CardCollection == null)
        {
            return false;
        }
        return GetCollection(m_CardCollection, nCardID, (int)CARD_COLLECT_IDX.GET);
    }

    //判断某张卡牌是否觉醒过
    public bool IsCardAwake(int nCardID)
    {
        Tab_Card _tabCard = TableManager.GetCardByID(nCardID, 0);
        if (_tabCard == null || m_CardCollection == null)
        {
            return false;
        }
        return GetCollection(m_CardCollection, nCardID, (int)CARD_COLLECT_IDX.AWAKE1) || GetCollection(m_CardCollection, nCardID, (int)CARD_COLLECT_IDX.AWAKE2)
            || GetCollection(m_CardCollection, nCardID, (int)CARD_COLLECT_IDX.AWAKE3);
    }

    public bool IsCardAwake(int nCardID,int AwakeLevel)
    {
        Tab_Card _tabCard = TableManager.GetCardByID(nCardID, 0);
        if (_tabCard == null || m_CardCollection == null)
        {
            return false;
        }
        if(AwakeLevel<=(int)CARD_COLLECT_IDX.INVALID || AwakeLevel>=(int)CARD_COLLECT_IDX.NUM)
        {
            return false;
        }
        switch(AwakeLevel)
        {
            case 1: return GetCollection(m_CardCollection, nCardID,(int) CARD_COLLECT_IDX.AWAKE1);
            case 2: return GetCollection(m_CardCollection, nCardID, (int)CARD_COLLECT_IDX.AWAKE2);
            case 3: return GetCollection(m_CardCollection, nCardID, (int)CARD_COLLECT_IDX.AWAKE3);
        }
        return false;
    }

    //判断某个法宝是否获得
    public bool IsGetTalisman(int nTalismanID)
    {
        Tab_Talisman talisman = TableManager.GetTalismanByID(nTalismanID, 0);
        if (null == talisman)
        {
            return false;
        }
        return GetCollection(m_TalismanCollection, nTalismanID, (int)TALISMAN_COLLECT_IDX.GET);
    }
    public bool IsGetIntimacyLetter(int cardID)
    {
        Tab_Card _TabCard = TableManager.GetCardByID(cardID, 0);
        if (_TabCard==null)
        {
            return false;
        }
        return GetCollection(m_IntimacyCollection,cardID,(int)INTIMACY_COLLECT_IDX.GET);
    }
    // 判断某个星魂类型是否获得
    public bool IsQuartzClassGet(int nQuartzClassID)
    {
        return IsQuartzClassGet(nQuartzClassID, QUARTZ_COLLECT_IDX.QUARTZ_COLLECT_IDX_SLOT1)
            || IsQuartzClassGet(nQuartzClassID, QUARTZ_COLLECT_IDX.QUARTZ_COLLECT_IDX_SLOT2)
            || IsQuartzClassGet(nQuartzClassID, QUARTZ_COLLECT_IDX.QUARTZ_COLLECT_IDX_SLOT3)
            || IsQuartzClassGet(nQuartzClassID, QUARTZ_COLLECT_IDX.QUARTZ_COLLECT_IDX_SLOT4)
            || IsQuartzClassGet(nQuartzClassID, QUARTZ_COLLECT_IDX.QUARTZ_COLLECT_IDX_SLOT5)
            || IsQuartzClassGet(nQuartzClassID, QUARTZ_COLLECT_IDX.QUARTZ_COLLECT_IDX_SLOT6);
    }

    //某个星魂的装备槽是否获得
    public bool IsQuartzClassGet(int nQuartzClassID, QUARTZ_COLLECT_IDX idx)
    {
        Tab_QuartzClass quartzClass = TableManager.GetQuartzClassByID(nQuartzClassID, 0);
        if (quartzClass == null)
        {
            return false;
        }
        return GetCollection(m_QuartzCollection, nQuartzClassID, (int)idx);
    }
    //某个图鉴是否获得奖励
    public bool IsGetGroupAccess(int id)
    {
        if (CollectionGroupFinishList == null || CollectionGroupFinishList.AlreadyReceived == null)
        {
            return false;
        }
        return CollectionGroupFinishList.AlreadyReceived.Contains(id);
    }
}
