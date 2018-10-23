using System;
using System.Collections.Generic;
using System.Linq;
using ProtobufPacket;
using Games.GlobeDefine;
using Games.Table;

public class Yard
{
    public void CleanUp()
    {
        m_ProtoYard = new _Yard();
        m_MyProtoYard = new _Yard();

    }

    public static void ReturnToLogin()
    {
        msDelOnYardCheckOtherProdState = null;
        msDelOnYardSync = null;
        msDelOnYardCardUpdated = null;
        msDelOnYardUnlockSkin = null;
        msDelOnSyncVisitingCard = null;
    }

    public delegate void OnYardSync(ProtobufPacket.YardOp op, _Yard pYard);
    public static OnYardSync msDelOnYardSync = null;
    public void HandlePacket(GC_YARD_SYNC packet)
    {
        if (packet == null || LoginData.user == null)
        {
            return;
        }

        if (packet.Operation != YardOp.YardOp_LOGIN)
        {
            m_ProtoYard = packet.YardData;            
            CheckUpdateMyYard(m_ProtoYard);
        }
        else
        {
            if (GameManager.PlayerDataPool != null)
            {
                GameManager.PlayerDataPool.m_Login_ProtoYard = packet.YardData;
            }
            CheckUpdateMyYard(packet.YardData);
        }

        switch (packet.Operation)
        {
            case YardOp.YardOp_SYNC:
            case YardOp.YardOp_LEVEL_UP:
                {
                    var skin = GetCurSkin();
                    if (skin != null)
                        GameManager.EnterGameScene(GetSceneClassID(skin, ProtoYard.Level), true);
                }
                break;
            case YardOp.YardOp_ON_OWNER_ENTER:
                {
                    if (m_ProtoYard.OwnerGuid == LoginData.user.guid)
                    {
                        var skin = GetCurSkin();
                        if (skin != null)
                            GameManager.EnterGameScene(GetSceneClassID(skin, ProtoYard.Level), true);
                    }
                }
                break;
            case YardOp.YardOp_UPDATE_PROD:
                break;
            case YardOp.YardOp_START_PROD:
                {
                    if (IsCurOwner(LoginData.user.guid))
                    {
                        HouseProducePrepear.Close();
                        HouseProducing.Open();
                    }
                }
                break;
            case YardOp.YardOp_HARVEST:
                HouseProducing.Close();
                break;
        }
        
        if (msDelOnYardSync != null)
        {
            msDelOnYardSync(packet.Operation, ProtoYard);
        }
    }

    public delegate void OnYardCardUpdated(ProtobufPacket.YardOp op, _YardCard card);
    public static OnYardCardUpdated msDelOnYardCardUpdated = null;
    public void HandlePacket(GC_YARD_UPDATECARD packet)
    {
        if (packet == null)
        {
            return;
        }

        switch (packet.Operation)
        {
            case YardOp.YardOp_SYNC:
                SyncCard(packet.YardCard);
                break;
            case YardOp.YardOp_PUT:
                PutCard(packet.YardCard);
                break;
            case YardOp.YardOp_TAKE:
                TakeCard(packet.YardCard);
                break;
        }

        CheckUpdateMyYard(m_ProtoYard);

        if (msDelOnYardCardUpdated != null)
        {
            msDelOnYardCardUpdated(packet.Operation, packet.YardCard);
        }
    }

    public delegate void OnYardUnlockSkin(bool suc, int skin);
    public static OnYardUnlockSkin msDelOnYardUnlockSkin = null;
    public void HandlePacket(GC_SYNC_UNLOCKED_YARD_SKIN packet)
    {
        if (packet == null)
        {
            return;
        }
        m_UnlockedSkins = packet.UnlockedYardSkin;
    }

    public void HandlePacket(GC_YARD_UNLOCK_SKIN packet)
    {
        if (packet == null)
        {
            return;
        }

        if (packet.Suc && m_UnlockedSkins.All(unlocked => unlocked != packet.SkinId))
        {
            m_UnlockedSkins.Add(packet.SkinId);
        }

        if (msDelOnYardUnlockSkin != null)
        {
            msDelOnYardUnlockSkin(packet.Suc, packet.SkinId);
        }
    }

    //public delegate void OnYardChangeSkin(bool suc, int skin);
    //public static OnYardChangeSkin msDelOnYardChangeSkin = null;
    public void HandlePacket(GC_YARD_CHANGE_SKIN packet)
    {
        if (packet == null)
        {
            return;
        }

        if (packet.Suc)
        {
            m_ProtoYard.SkinId = packet.SkinId;

            Tab_HouseSkin skin = TableManager.GetHouseSkinByID(m_ProtoYard.SkinId, 0);
            if (skin == null)
            {
                skin = GetDefaultSkin();
            }

            if (skin == null)
            {
                LogModule.ErrorLogFormat("house skin {0} not found", m_ProtoYard.SkinId);
                return;
            }

            GameManager.EnterGameScene(GetSceneClassID(skin, m_ProtoYard.Level), true);
        }

        //if (msDelOnYardChangeSkin != null)
        //{
        //    msDelOnYardChangeSkin(packet.Suc, packet.SkinId);
        //}
    }

    public delegate void OnYardCheckOtherProdState(UInt64 guid, ProtobufPacket.YardProdState state);
    public static OnYardCheckOtherProdState msDelOnYardCheckOtherProdState = null;
    public void HandlePacket(GC_YARD_CHECK_PROD_STATE packet)
    {
        if (packet == null)
        {
            return;
        }

        if (msDelOnYardCheckOtherProdState != null)
        {
            msDelOnYardCheckOtherProdState(packet.Guid, packet.State);
        }
    }

    public delegate void OnSyncVisitingCard(int cardid);
    public static OnSyncVisitingCard msDelOnSyncVisitingCard = null;
    public void HandlePacket(GC_SYNC_CARD_VISITING packet)
    {
        if (packet == null)
        {
            return;
        }

        m_VisitingCard = packet.CardID;
        if (msDelOnSyncVisitingCard != null)
        {
            msDelOnSyncVisitingCard(m_VisitingCard);
        }
    }

    public static void SendEnter(UInt64 ownerId)
    {
        if (GameManager.CurScene != null && GameManager.CurScene.IsHouseScene() && IsCurOwner(ownerId))
        { 
            Games.Utils.CenterNotice(8182);
            return;
        }

        if (LoginData.user != null && ownerId == LoginData.user.guid)
        {
            CG_YARD_ENTER_MINE_PAK pak = new CG_YARD_ENTER_MINE_PAK();
            pak.SendPacket();
        }
        else
        {
            CG_YARD_ENTER_OTHERS_PAK pak = new CG_YARD_ENTER_OTHERS_PAK();
            pak.data.targetGuid = ownerId;
            pak.SendPacket();
        }
    }

    public static void SendPut(UInt64 cardId)
    {
        CG_YARD_PUTCARD_PAK pak = new CG_YARD_PUTCARD_PAK();
        pak.data.CardGuid = cardId;
        pak.SendPacket();
    }

    public static void SendTake(UInt64 cardId)
    {
        CG_YARD_TAKECARD_PAK pak = new CG_YARD_TAKECARD_PAK();
        pak.data.CardGuid = cardId;
        pak.SendPacket();
    }

    public static void SendUnlockSkin(int skin)
    {
        CG_YARD_UNLOCK_SKIN_PAK pak = new CG_YARD_UNLOCK_SKIN_PAK();
        pak.data.SkinId = skin;
        pak.SendPacket();
    }

    public static void SendChangeSkin(int skin)
    {
        CG_YARD_CHANGE_SKIN_PAK pak = new CG_YARD_CHANGE_SKIN_PAK();
        pak.data.SkinId = skin;
        pak.SendPacket();
    }

    public static void SendStartProd(int prodId)
    {
        CG_YARD_START_PROD_PAK pak = new CG_YARD_START_PROD_PAK();
        pak.data.ProdId = prodId;
        pak.SendPacket();
    }

    public static void SendHarvest()
    {
        CG_YARD_HARVEST_PAK pak = new CG_YARD_HARVEST_PAK();
        pak.SendPacket();
    }

    public static void SendHelp()
    {
        CG_YARD_HELP_PAK pak = new CG_YARD_HELP_PAK();
        pak.SendPacket();
    }

    public static void SendSteal(int index)
    {
        CG_YARD_STEAL_PAK pak = new CG_YARD_STEAL_PAK();
        pak.data.Index = index;
        pak.SendPacket();
    }

    public static void SendCheckState(UInt64 guid)
    {
        CG_YARD_CHECK_PROD_STATE_PAK pak = new CG_YARD_CHECK_PROD_STATE_PAK();
        pak.data.Guid = guid;
        pak.SendPacket();
    }

    public static void SendLevelUp(int targetLevel)
    {
        CG_YARD_LEVEL_UP_PAK pak = new CG_YARD_LEVEL_UP_PAK();
        pak.data.targetLevel = targetLevel;
        pak.SendPacket();
    }

    private _Yard m_ProtoYard;
    public _Yard ProtoYard
    {
        get { return m_ProtoYard; }
    }

    private _Yard m_MyProtoYard;
    public _Yard MyProtoYard
    {
        get { return m_MyProtoYard; }
    }

    private List<int> m_UnlockedSkins = new List<int>();
    public List<int> UnlockedSkins 
    {
        get { return m_UnlockedSkins; }
    }

    private int m_VisitingCard;
    public int VisitingCard
    {
        get { return m_VisitingCard; }
    }

    void CheckUpdateMyYard(ProtobufPacket._Yard newData)
    {
        if (newData != null && newData.OwnerGuid == LoginData.user.guid && m_MyProtoYard != null)
        {
            m_MyProtoYard.CardList.Clear();
            m_MyProtoYard.CardList.AddRange(newData.CardList);
            m_MyProtoYard.Level = newData.Level;
        }
    }

    public void SyncCard(_YardCard ycLoaded)
    {
        if (ycLoaded == null)
        {
            return;
        }

        var l = m_ProtoYard.CardList;
        for (int i = 0; i < l.Count; ++i)
        {
            if (l[i].CardGuid == ycLoaded.CardGuid)
            {
                l[i] = ycLoaded;
                break;
            }
        }
    }

    public void PutCard(_YardCard ycLoaded)
    {
        if (ycLoaded == null)
        {
            return;
        }

        if (IsIn(ycLoaded.CardGuid, m_ProtoYard))
            return;
        m_ProtoYard.CardList.Add(ycLoaded);
    }

    public void TakeCard(_YardCard ycLoaded)
    {
        if (ycLoaded == null)
        {
            return;
        }

        var l = m_ProtoYard.CardList;
        for (int i = 0; i < l.Count; ++i)
        {
            if (l[i].CardGuid == ycLoaded.CardGuid)
            {
                l.RemoveAt(i);
                break;
            }
        }
    }

    public int GetTotalIntimacy()
    {
        if (m_ProtoYard == null || m_ProtoYard.CardList == null)
            return 0;
        int ret = 0;
        foreach (var card in m_ProtoYard.CardList)
        {
            ret += card.Intimacy;
        }
        return ret;
    }

    public int GetCardCount()
    {
        if (m_ProtoYard != null && m_ProtoYard.CardList != null)
        {
            return m_ProtoYard.CardList.Count;
        }
        return 0;
    }

    public bool IsIn(ulong guid, ProtobufPacket._Yard yard)
    {
        if (yard == null || yard.CardList == null)
            return false;

        foreach (var v in yard.CardList)
        {
            if (v.CardGuid == guid)
                return true;
        }

        return false;
    }

    public bool IsInMyYard(UInt64 card)
    {
        return IsIn(card, m_MyProtoYard);
    }

    public bool IsInCurYard(UInt64 card)
    {
        return IsIn(card, m_ProtoYard);
    }

    public static Tab_HouseSkin GetDefaultSkin()
    {
        var tab = TableManager.GetHouseSkin().Values;
        foreach (var skin in tab)
        {
            if (skin.Count == 0)
                continue;
            if (skin[0].ForSale != 1 && skin[0].Serving == 1)
                return skin[0];
        }
        return null;
    }
    
    public static Tab_HouseSkin GetCurSkin()
    {
        var yard = Yard.Instance;
        if (yard == null)
            return null;

        Tab_HouseSkin skin = TableManager.GetHouseSkinByID(yard.ProtoYard.SkinId, 0);
        if (skin == null)
            skin = GetDefaultSkin();

        if (skin == null)
            LogModule.ErrorLogFormat("house skin {0} not found", yard.ProtoYard.SkinId);
        
        return skin;
    }
    public List<_YardNote> GetSortedNote()
    {
        List<_YardNote> ret = new List<_YardNote>();
        if (m_ProtoYard == null || m_ProtoYard.messageBoard == null)
        {
            return ret;
        }
        if (m_ProtoYard.messageBoard.wonderfulNoteData != null)
        {
            for (int i = 0; i < m_ProtoYard.messageBoard.wonderfulNoteData.Count; i++)
            {
                ret.Add(m_ProtoYard.messageBoard.wonderfulNoteData[i]);
            }
        }
        if (m_ProtoYard.messageBoard.noteData != null)
        {
            for (int i = 0; i < m_ProtoYard.messageBoard.noteData.Count; i++)
            {
                ret.Add(m_ProtoYard.messageBoard.noteData[i]);
            }
        }
        return ret;
    }
    public bool IsWonderFulNote(ulong guid)
    {
        if (m_ProtoYard == null || m_ProtoYard.messageBoard == null)
        {
            return false;
        }
        if (m_ProtoYard.messageBoard.wonderfulNoteData != null)
        {
            for (int i = 0; i < m_ProtoYard.messageBoard.wonderfulNoteData.Count; i++)
            {
                if (m_ProtoYard.messageBoard.wonderfulNoteData[i].guid == guid)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public static int GetIntimacyLevel(int totalIntimacy)
    {
        var table = TableManager.GetHouseIntimacyLevel();
        if (table == null)
            return GlobeVar.INVALID_ID;

        int ret = GlobeVar.INVALID_ID;
        foreach (var vlist in table.Values)
        {
            if (vlist.Count > 0)
            {
                ret = vlist[0].Id;
                if (totalIntimacy < vlist[0].UpperBound)
                {
                    break;
                }
            }
        }
        return ret;
    }
    public bool IsNoteRedPoint()
    {
        if (m_ProtoYard == null || m_ProtoYard.messageBoard == null)
        {
            return false;
        }
        foreach (var note in m_ProtoYard.messageBoard.wonderfulNoteData)
        {
            if (HouseTool.IsNoteNew(note))
            {
                return true;
            }
        }
        foreach (var note in m_ProtoYard.messageBoard.noteData)
        {
            if (HouseTool.IsNoteNew(note))
            {
                return true;
            }
        }
        return false;
    }
    public static List<Tab_HouseProd> GetProdByParam(int type, int hour, int intlv)
    {
        var table = TableManager.GetHouseProd();
        if (table == null)
            return null;

        return (from vlist in table.Values
               where vlist.Count > 0
               where vlist[0].IntimacyLevel == intlv && vlist[0].Type == type && vlist[0].Hour == hour
               select vlist[0]).ToList();
    }

    public bool IsSkinUnlocked(int skinId)
    {
        var tab = TableManager.GetHouseSkinByID(skinId, 0);
        if (tab == null)
            return false;
        if (tab.ForSale != 1 && tab.Serving == 1)
            return true;

        if (m_UnlockedSkins == null)
        {
            LogModule.DebugLogFormat("invalid data state @ IsSkinUnlocked");
            return false;
        }

        return m_UnlockedSkins.Any(unlocked => unlocked == skinId);
    }

    public Games.GlobeDefine.GlobeVar.HouseProdState GetProdState()
    {
        if (ProtoYard == null || ProtoYard.YardProd == null || ProtoYard.YardProd.ProdId <= 0)
        {
            return Games.GlobeDefine.GlobeVar.HouseProdState.NONE;
        }
        else if (ProtoYard.YardProd.FinishTime <= GameManager.ServerAnsiTime)
        {
            return Games.GlobeDefine.GlobeVar.HouseProdState.FINISH;
        }
        else
        {
            return Games.GlobeDefine.GlobeVar.HouseProdState.PRODING;
        }
    }

    public static int GetSceneClassID(Tab_HouseSkin tabSkin, int level)
    {
        if (tabSkin == null || level < 1 || level > GlobeVar.YARD_MAX_LEVEL)
        {
            return GlobeVar.INVALID_ID;
        }
        return tabSkin.GetSceneClassLvbyIndex(level - 1);
    }

    static public bool IsCurOwner(UInt64 guid)
    {
        if (GameManager.PlayerDataPool == null ||
            GameManager.PlayerDataPool.YardData == null ||
            GameManager.PlayerDataPool.YardData.ProtoYard == null)
        {
            return false;
        }
        var protoYard = GameManager.PlayerDataPool.YardData.ProtoYard;
        return protoYard.OwnerGuid == guid;
    }

    static public Yard Instance
    {
        get
        {
            if (GameManager.PlayerDataPool == null || GameManager.PlayerDataPool.YardData == null)
                return null;
            return GameManager.PlayerDataPool.YardData;
        }
    }
}
