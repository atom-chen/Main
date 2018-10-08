/********************************************************************************
 *	文件名：	HouseScene.cs
 *	全路径：	\Script\Scene\Scene\HouseScene.cs
 *	创建人：	樊姝放
 *	创建时间：2018-05-08
 *
 *	功能说明：私宅，可布置家居摆设等
 *	修改记录：
*********************************************************************************/
using System.Collections.Generic;
using UnityEngine;
using Games.LogicObj;
using Games.GlobeDefine;
using System;
using System.Linq;
using ProtobufPacket;
using Games;
using Games.Table;
//using UnityEditorInternal;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public partial class HouseScene : RealTimeScene
{
    public override SCENETYPE GetSceneType()
    {
        return SCENETYPE.HOUSE;
    }

    public override void OnLoadGameUI()
    {
        UIManager.ShowUI(UIInfo.HouseMainUI);
        UIManager.ShowUI(UIInfo.JoyStick);
        UIManager.ShowUI(UIInfo.TargetFrame);
        UIManager.ShowUI(UIInfo.CenterNoticeEx);
        UIManager.ShowUI(UIInfo.RollNotice);
    }

    public override void OnCloseGameUI()
    {
        UIManager.CloseUI(UIInfo.HouseMainUI);
        UIManager.CloseUI(UIInfo.QuickIntercourseRoot);
        UIManager.CloseUI(UIInfo.JoyStick);
        UIManager.CloseUI(UIInfo.TargetFrame);
        UIManager.CloseUI(UIInfo.CenterNoticeEx);
        UIManager.CloseUI(UIInfo.RollNotice);
        UIManager.CloseUI(UIInfo.HouseRelationRoot);
        base.OnCloseGameUI();
    }

    private _Yard mYard;
    public _Yard YardData
    {
        get { return mYard; }
    }

    private HouseNavigatorManager mNavManager;

    private Transform mCamStartTrans;
    private Transform mIntimacyTarget;
    private Transform mIntimacyCard;
    private Transform mIntimacyPlayer;

    private Vector3 mIntimacyPlayerPos;
    private Vector3 mIntimacyCardPos;
    private Obj_Card mIntimacyObjCard;

    private float m_LastReceiveGift = 0;   //上次收到礼物时间
    
    public class SceneCardInfo
    {
        public Card card;
        public Obj_Card objCard;
        public float animStartTime;

        public SceneCardInfo(Card c, Obj_Card o)
        {
            card = c;
            objCard = o;
            animStartTime = 0;
        }

        public void Clean()
        {
            card = null;
            objCard = null;
            animStartTime = 0;
        }

        public bool Valid()
        {
            return card != null && objCard != null;
        }

        public void PlayAnim(int anim)
        {
            if (objCard == null)
                return;
            objCard.PlayAnim(anim);
            animStartTime = Time.time;
        }

        public bool IsAnimInCD()
        {
            return (Time.time - animStartTime <= 2);
        }

        public Vector3 Position
        {
            get
            {
                if (objCard == null)
                    return Vector3.zero;
                return objCard.Position;
            }
            set
            {
                if (objCard == null)
                    return;
                objCard.Position = value;
            }
        }

        public float YardDistRadius
        {
            get
            {
                if (objCard == null)
                    return 0f;
                Tab_CharModel model = TableManager.GetCharModelByID(objCard.ModelID, 0);
                if (model == null)
                    return 0f;
                return model.YardDistance;
            }
        }

        public void AddEffectUpdater(SceneCardInfo[] list)
        {
            if (objCard == null)
                return;
            var com = objCard.gameObject.AddComponent<HouseTargetEffectUpdater>();
            if (com != null)
            {
                com.Setup(this, list);
            }
        }

        public void RemoveEffectUpdater()
        {
            if (objCard == null)
                return;
            var com = objCard.gameObject.GetComponent<HouseTargetEffectUpdater>();
            if (com != null)
                Destroy(com);
        }

        public Vector3 RefixPosition
        {
            get
            {
                if (objCard == null)
                    return Vector3.zero;
                var com = objCard.gameObject.GetComponent<HouseTargetEffectUpdater>();
                if (com != null)
                {
                    return com.LastValidPos;
                }
                return objCard.Position;
            }
        }
    }

    private Dictionary<UInt64, SceneCardInfo> mCards = new Dictionary<UInt64, SceneCardInfo>();

    public SceneCardInfo GetSceneCardInfo(Obj_Card oc)
    {
        if (oc == null)
            return null;
        foreach (var sci in mCards.Values)
        {
            if (sci == null)
            {
                continue;
            }

            if (sci.objCard == oc)
            {
                return sci;
            }
        }
        return null;
    }

    private static int PreviewModel_ReturnScene = GlobeVar.INVALID_ID;    //预览模式需要返回的原场景
    private static int TVId = GlobeVar.INVALID_ID;
    //播放私宅TV
    public static void PlayTV(Tab_HouseSkin tabHouseSkin)
    {
        if(tabHouseSkin == null || tabHouseSkin.PreviewCamTVID==GlobeVar.INVALID_ID)
        {
            return;
        }
        PreviewModel_ReturnScene = GameManager.RunningScene;        //记录原场景
        TVId = tabHouseSkin.PreviewCamTVID;                        //记录TVID
        //切换场景
        GameManager.EnterGameScene(tabHouseSkin.SceneClass, true);
    }
    //私宅TV播放结束，场景回退
    public void OnTVPlayFinish(object oParam)
    {
        if (PreviewModel_ReturnScene!=GlobeVar.INVALID_ID)
        {
            GameManager.EnterGameScene(PreviewModel_ReturnScene, true);
        }
        PreviewModel_ReturnScene = GlobeVar.INVALID_ID;
        TVId = GlobeVar.INVALID_ID;
    }
    public override void OnEnterScene(bool bSameSceneResource)
    {
        base.OnEnterScene(bSameSceneResource);

        GameManager.EnvManager.SetDefault();

        if (PreviewModel_ReturnScene != GlobeVar.INVALID_ID && TVId !=GlobeVar.INVALID_ID)
        {
            CutsceneManager.PlayCutscene(TVId, OnTVPlayFinish);          //播放TV
            return;
        }

        LoadScene(GameManager.PlayerDataPool.YardData.ProtoYard);

        CreateProdNpc();
        RefreshProdNpcState();
        RefreshStealNpc();

        if (GameManager.InputManager != null)
        {
            GameManager.InputManager.onClickObj = OnClickObj;
            GameManager.InputManager.onClickScene = OnClickScene;
        }

        Yard.msDelOnYardSync += OnYardSync;
        Yard.msDelOnYardCardUpdated += OnYardCardUpdated;

        mMode = HouseMode.NORMAL;
    }
    
    public override void OnLeaveScene()
    {
        Yard.msDelOnYardSync -= OnYardSync;
        Yard.msDelOnYardCardUpdated -= OnYardCardUpdated;

        GameManager.CameraManager.IsDragLock = false;
        if (ObjManager.MainPlayer != null)
        {
            ObjManager.MainPlayer.EventMove = true;
        }

        base.OnLeaveScene();

        foreach (var card in mCards)
        {
            if (card.Value != null && ObjManager.IsObjExist(card.Value.objCard))
            {
                ObjManager.RemoveObj(card.Value.objCard);
            }
        }
        mCards.Clear();
        
        UnloadNavManager();
    }

    const float UPDATE_TIME = 1f;
    private float mUpdateTimer = UPDATE_TIME;
    void FixedUpdate()
    {
        if (!InMode(HouseMode.NORMAL))
            return;

        mUpdateTimer -= Time.fixedDeltaTime;
        if (mUpdateTimer <= 0)
        {
            mUpdateTimer = UPDATE_TIME;
            RefreshProdNpcState();
            RefreshStealNpc();
        }
    }

    public void Load(_Yard pYard)
    {
        CleanYard();

        mYard = pYard;
        if (mYard == null)
            return;

        var YCardList = mYard.CardList;
        Vector3 pos = Vector3.zero;
        foreach (var yc in YCardList)
        {
            PutCard(yc);
        }
    }

    void LoadScene(_Yard pYard)
    {
        mYard = pYard;

        LoadNavManager();
        LoadPos();
        Load(pYard);
    }
    
    void CleanYard()
    {
        foreach (var cp in mCards)
        {
            ObjManager.RemoveObj(cp.Value.objCard);
        }
        mCards.Clear();
    }

    private void LoadNavManager()
    {
        UnloadNavManager();

        var skin = GetCurSkin();
        if (skin == null)
            return;

        GameObject res = AssetManager.LoadResource("Prefab/" + skin.NavRes) as GameObject;
        if (res == null)
        {
            LogModule.WarningLog("cannot load HouseNavRoot");
            return;
        }
        GameObject navRoot = AssetManager.InstantiateObjToParent(res,null);
        if (navRoot == null)
        {
            LogModule.WarningLog("Load HouseNavRoot Failed");
            return;
        }
        mNavManager = navRoot.GetComponent<HouseNavigatorManager>();
        if (mNavManager == null)
        {
            LogModule.WarningLog("HouseNavigatorManager not found");
            return;
        }
    }

    private void UnloadNavManager()
    {
        if (mNavManager != null)
        {
            DestroyImmediate(mNavManager.gameObject);
            mNavManager = null;
        }
    }

    private void LoadPos()
    {
        UnloadPos();

        var skin = GetCurSkin();
        if (skin == null)
            return;

        GameObject res = AssetManager.LoadResource("Prefab/" + skin.PosRes) as GameObject;
        if (res == null)
        {
            LogModule.WarningLog("cannot load HousePos");
            return;
        }
        GameObject root = AssetManager.InstantiateObjToParent(res, null);
        if (root == null)
        {
            LogModule.WarningLog("Load HousePos Failed");
            return;
        }

        HousePosManager rootcom = root.GetComponent<HousePosManager>();
        if (rootcom == null)
        {
            LogModule.WarningLog("HousePosManager not found");
            return;
        }

        mCamStartTrans = rootcom.camStartTrans;
        mIntimacyTarget = rootcom.intimacyTarget;
        mIntimacyCard = rootcom.intimacyCard;
        mIntimacyPlayer = rootcom.intimacyPlayer;
    }

    private void UnloadPos()
    {
        mCamStartTrans = null;
        mIntimacyTarget = null;
        mIntimacyCard = null;
        mIntimacyPlayer = null;
    }

    void OnYardSync(ProtobufPacket.YardOp op, _Yard pYard)
    {
        mYard = pYard;
        switch (op)
        {
            case YardOp.YardOp_SYNC:
            case YardOp.YardOp_ON_OWNER_ENTER:
                Load(pYard);
                break;
            case YardOp.YardOp_STEAL:
                RefreshStealNpc();
                break;
        }
    }

    void OnYardCardUpdated(ProtobufPacket.YardOp op, _YardCard ycLoaded)
    {
        switch (op)
        {
            case YardOp.YardOp_SYNC:
                SyncCard(ycLoaded);
                break;
            case YardOp.YardOp_PUT:
                PutCard(ycLoaded);
                break;
            case YardOp.YardOp_TAKE:
                TakeCard(ycLoaded.CardGuid);
                break;
        }
    }

    System.Collections.IEnumerator IntimacyCardShowBubble()
    {
        if (mIntimacyObjCard == null)
        {
            yield break;
        }
        Tab_CardExtend tabCardEx = TableManager.GetCardExtendByID(CardTool.GetCardDefaultExtendId(mIntimacyObjCard.CardId), 0);
        if (tabCardEx == null)
        {
            yield break;
        }
        yield return new WaitForSeconds(5.0f);
        while (mIntimacyObjCard != null)
        {
            //等待玩家送礼时间不够
            if(Time.time - m_LastReceiveGift < 5.0f)
            {
                yield return new WaitForSeconds(0.5f);
                continue;
            }
            //等待气泡
            int ran=UnityEngine.Random.Range(0,tabCardEx.getWaitBubbleCount());
            string content=tabCardEx.GetWaitBubblebyIndex(ran);
            mIntimacyObjCard.Bubble(content, 2.5f);
            yield return new WaitForSeconds(5.0f);
        }
    }

    //收到礼物 播放特效
    public void HandleCardReceiveGift(GC_CARD_INTIMACY_UPDATE_VIEW pak)
    {
        if(mIntimacyObjCard == null)
        {
            return;
        }
        SceneCardInfo sceneCard = GetSceneCardInfo(mIntimacyObjCard);
        if( sceneCard== null || sceneCard.card == null)
        {
            return;
        }
        Tab_CardExtend tabCardEx = TableManager.GetCardExtendByID(CardTool.GetCardDefaultExtendId(mIntimacyObjCard.CardId), 0);
        if(tabCardEx==null)
        {
            return;
        }
        string tips = "";
        INTIMACY_GIFT_LIKE likeDegree = IntimacyTools.GetGiftItemLikeLevel(sceneCard.card, pak.itemID); //sceneCard.card.GetIntimacyGiftLike(pak.itemID);       //拿到喜好等级
        tips = IntimacyTools.GetReceiveGiftTips((int)likeDegree);
        switch(likeDegree)
        {
            case INTIMACY_GIFT_LIKE.FAVORITE:
                mIntimacyObjCard.PlayAnim(tabCardEx.FavoriteAnimation);
                if (!GameManager.SoundManager.IsRealSoundPlaying() || tabCardEx.FavoriteSound != GameManager.SoundManager.GetCurRealSoundId())
                {
                    GameManager.SoundManager.PlayRealSound(tabCardEx.FavoriteSound);
                }

                break;
            case INTIMACY_GIFT_LIKE.NORMAL:
                mIntimacyObjCard.PlayAnim(tabCardEx.NormalAnimation);
                if (!GameManager.SoundManager.IsRealSoundPlaying() || tabCardEx.NormalSound != GameManager.SoundManager.GetCurRealSoundId())
                {
                    GameManager.SoundManager.PlayRealSound(tabCardEx.NormalSound);
                }
                break;
            case INTIMACY_GIFT_LIKE.DISLIKE:
                mIntimacyObjCard.PlayAnim(tabCardEx.DislikeAnimation);
                if (!GameManager.SoundManager.IsRealSoundPlaying() || tabCardEx.DislikeSound != GameManager.SoundManager.GetCurRealSoundId())
                {
                    GameManager.SoundManager.PlayRealSound(tabCardEx.DislikeSound);
                }
                break;
        }
        if (!String.IsNullOrEmpty(tips))
        {
            mIntimacyObjCard.Bubble(tips);
        }
        m_LastReceiveGift = Time.time;
    }
    #region SceneOp
    private void SetPlayerVisible(bool v)
    {
        if (ObjManager.MainPlayer != null)
        {
            ObjManager.MainPlayer.SetVisible(ObjVisibleLayer.Story, v);
        }
    }

    public Obj_Card PutCard(_YardCard yc)
    {
        Card card = new Card(yc);
        return PutCard(card);
    }

    public delegate void OnSceneAddCard(SceneCardInfo ci);

    public OnSceneAddCard DelSceneAddCard;
    public Obj_Card PutCard(Card card)
    {
        if (card == null || !card.IsValid())
            return null;

        if (mCards.ContainsKey(card.Guid))
        {
            LogModule.ErrorLogFormat("duplicate card {0}, {1}, {2}", card.CardId, card.GetName(), card.Guid);
            return null;
        }
        if (mCards.Count >= GlobeVar._GameConfig.m_nYardMaxCardNum)
        {
            Utils.CenterNotice(8023);
            return null;
        }
        if (card.GetIntimacyLevel() < GlobeVar._GameConfig.m_nYardCardIntimacy)
            return null;
        
        if (mNavManager == null)
            return null;
        
        var nav = mNavManager.GetNext();
        if (nav == null)
            return null;

        Vector3 pos = nav.transform.position;
        Vector3 rot = nav.transform.eulerAngles;

        Obj_Init_Data initData = new Obj_Init_Data();
        initData.m_nModelID = card.GetCharModelId();
        initData.m_CreatePos = pos;
        initData.m_CreateRot = rot;
        initData.m_nDyeColorId = card.DyeColorID;
        initData.m_OrnamentEffectTabId = card.OrnamentEffectId;
        var oc = ObjManager.CreateCard(initData);
        if (oc != null)
        {
            Utils.SetAllChildLayer(oc.ObjTransform, LayerMask.NameToLayer("NPC_Far"));

            Tab_HouseSkin skin = GetCurSkin();
            if (skin != null && skin.PutCardEffect != GlobeVar.INVALID_ID)
            {
                PlayEffect(skin.PutCardEffect, oc.Position);
            }

            oc.name = card.CardId.ToString();
            oc.CardId = card.CardId;
            oc.CurSkinId = card.CurSkinId;
            oc.MoveSpeed = mNavManager.Speed;
            oc.RoleName = card.GetName();

            var ci = new SceneCardInfo(card, oc);
            mCards.Add(card.Guid, ci);

            // set nav agent
            var tab = card.GetTable_CardExtend();
            if (tab==null)
            {
                return null;
            }
            int[] animList = CardTool.GetAnimList(tab);
            nav.SetAgent(oc, animList);
            nav.Run(mNavManager.Speed);

            if (DelSceneAddCard != null)
            {
                DelSceneAddCard(ci);
            }
        }
        return oc;
    }

    public delegate void OnSceneDelCard(UInt64 cardGuid);

    public OnSceneDelCard DelSceneDelCard;
    public bool TakeCard(UInt64 guid)
    {
        SceneCardInfo oc = null;
        if (mCards.TryGetValue(guid, out oc))
        {
            if (mNavManager != null)
            {
                var nav = mNavManager.GetNavigator(oc.objCard);
                if (nav != null)
                {
                    nav.Stop();
                    nav.ClearAgent();
                }
            }

            if (IsCurTarget(oc.objCard))
            {
                CleanTarget();
            }

            mCards.Remove(guid);

            if (oc != null && oc.objCard != null)
            {
                if (ObjManager.RemoveObj(oc.objCard))
                {
                    if (DelSceneDelCard != null)
                    {
                        DelSceneDelCard(guid);
                    }
                    return true;
                }
            }
        }
        return false;
    }
    public bool SyncCard(_YardCard yc)
    {
        if (yc == null)
            return false;

        SceneCardInfo oc = null;
        if (mCards.TryGetValue(yc.CardGuid, out oc))
        {
            if (oc != null && oc.card != null && oc.objCard != null)
            {
                oc.card.UpdateCard(yc);

                int model = oc.card.GetCharModelId();
                if (model != oc.objCard.ModelID)
                {
                    AvatarLoadInfo avatarLoad = new AvatarLoadInfo();
                    avatarLoad.m_BodyId = oc.card.GetCharModelId();
                    Tab_DyeColor tabDyeColor = TableManager.GetDyeColorByID(oc.card.DyeColorID, 0);
                    if (tabDyeColor != null)
                    {
                        avatarLoad.LoadDyeColor(oc.card.DyeColorID);
                    }
                    else
                    {
                        avatarLoad.LoadDyeColor(GlobeVar.INVALID_ID);
                    }
                    oc.objCard.ChangeAvatar(avatarLoad);
                }
                return true;
            }
        }
        return false;
    }

    public bool HasCard(UInt64 guid)
    {
        return mCards.ContainsKey(guid);
    }

    public bool CanPutCard(Card card)
    {
        if (card == null || !card.IsValid())
            return false;

        if (mCards.Count >= GlobeVar._GameConfig.m_nYardMaxCardNum)
            return false;

        if (card.GetIntimacyLevel() < GlobeVar._GameConfig.m_nYardCardIntimacy)
            return false;

        return true;
    }
    
    void RefreshStealNpc()
    {
        if (mYard == null)
            return;

        bool showSteal = mYard.YardProd != null && mYard.YardProd.ProdId > 0 &&
                         mYard.YardProd.FinishTime < GameManager.ServerAnsiTime;

        var skin = GetCurSkin();
        if (skin == null)
            return;

        for (int i = 0; i < skin.getStealNpcCount(); ++i)
        {
            int npcid = skin.GetStealNpcbyIndex(i);
            if (npcid == GlobeVar.INVALID_ID)
                continue;

            if (showSteal)
            {
                bool create = true;
                var stealList = mYard.YardProd.StealList;
                foreach (var sc in stealList)
                {
                    if (sc.Index == i)
                    {
                        RemoveNpcImpl(npcid, skin.StealNpcEffect);
                        create = false;
                    }
                }

                if (create)
                {
                    if (ObjManager.GetObjNPCBySceneNPCID(npcid) == null)
                    {
                        Tab_SceneNpc tabnpc = TableManager.GetSceneNpcByID(npcid, 0);
                        if (tabnpc == null)
                            continue;

                        Tab_RoleBaseAttr tabroleBase = TableManager.GetRoleBaseAttrByID(Utils.GetNPCDataIDByEnv(tabnpc), 0);
                        if (tabroleBase == null)
                            continue;

                        //创建NPC
                        Obj_Init_Data info = new Obj_Init_Data();
                        info.m_CreatePos = new Vector3(tabnpc.PosX, 0.0f, tabnpc.PosZ);
                        info.m_CreateRot = new Vector3(0.0f, tabnpc.FaceDirection, 0.0f);
                        info.m_nModelID = tabroleBase.CharModelID;
                        info.m_bShowNameBoard = tabnpc.HeadFlag != GlobeVar.INVALID_ID;
                        info.m_nDyeColorId = tabnpc.DyeColorID;
                        Obj_NPC npc = ObjManager.CreateNPC(info, string.Format("Steal_{0}_{1}", i, npcid));

                        if (npc == null)
                            continue;
                        //npc.AddNpcMisc(tabnpc.MiscID);
                        npc.onMainPlayerMoveToNpcDone = OnClickStealNpc;
                        npc.CanBeSelect = true;
                        npc.SceneNPCID = npcid;
                        npc.FocusMaxDistance = tabnpc.HelloRange;
                        npc.FaceToSelecter = (tabnpc.HelloFaceTo != -1 && tabnpc.HelloRange > 0);
                    }
                }
            }
            else
            {
                RemoveNpcImpl(npcid, skin.StealNpcEffect);
            }
        }
    }

    private void RemoveNpcImpl(int scenenpcid, int effect)
    {
        var rn = ObjManager.GetObjNPCBySceneNPCID(scenenpcid);
        if (rn != null)
        {
            PlayEffect(effect, rn.Position);
            ObjManager.RemoveObj(rn);
        }
    }

    void CreateProdNpc()
    {
        Tab_HouseSkin skin = GetCurSkin();
        if (skin == null)
            return;

        Obj_NPC obj = GetProdNpc();
        if (obj != null)
            return;

        Tab_SceneNpc tabnpc = TableManager.GetSceneNpcByID(skin.ProdNpc, 0);
        if (tabnpc == null)
            return;

        Tab_RoleBaseAttr tabroleBase = TableManager.GetRoleBaseAttrByID(Utils.GetNPCDataIDByEnv(tabnpc), 0);
        if (tabroleBase == null)
            return;

        //创建NPC
        Obj_Init_Data info = new Obj_Init_Data();
        info.m_CreatePos = new Vector3(tabnpc.PosX, 0.0f, tabnpc.PosZ);
        info.m_CreateRot = new Vector3(0.0f, tabnpc.FaceDirection, 0.0f);
        info.m_nModelID = tabroleBase.CharModelID;
        info.m_bShowNameBoard = tabnpc.HeadFlag != GlobeVar.INVALID_ID;
        info.m_nDyeColorId = tabnpc.DyeColorID;
        Obj_NPC npc = ObjManager.CreateNPC(info, string.Format("Prod_{0}", skin.ProdNpc));

        if (npc == null)
            return;

        npc.CanBeSelect = true;
        npc.SceneNPCID = skin.ProdNpc;
        npc.FocusMaxDistance = tabnpc.HelloRange;
        npc.FaceToSelecter = (tabnpc.HelloFaceTo != -1 && tabnpc.HelloRange > 0);
        // 回调跟npc一起销毁了, 不用注销
        npc.onMainPlayerMoveToNpcDone = OnClickProdNpc;
    }

    void RefreshProdNpcState()
    {
        if (mYard == null)
            return;

        Obj_NPC npc = GetProdNpc();
        if (npc == null)
            return;

        int anime = GlobeVar.YARD_PROD_NPC_ANIM_OPEN;
        if (mYard.YardProd == null || mYard.YardProd.ProdId <= 0)
        {
            npc.BoardState = MissionBoardState.MISSION_CANACCEPTED;
            anime = GlobeVar.YARD_PROD_NPC_ANIM_CLOSE;
        }
        else if (mYard.YardProd.FinishTime <= GameManager.ServerAnsiTime)
        {
            npc.BoardState = MissionBoardState.MISSION_CANCOMPLETED;
        }
        else
        {
            npc.BoardState = MissionBoardState.MISSION_NONE;
        }

        if (npc.CurAnimId != anime)
        {
            npc.PlayAnim(anime);
        }
    }

    #endregion

    #region Input

    public void CleanTarget()
    {
        if (mTarget == null || mTarget.objCard == null)
            return;

        var nav = mNavManager.GetNavigator(mTarget.objCard);
        if (nav != null)
        {
            nav.Run(mNavManager.Speed);
        }
        mTarget = null;
    }

    private SceneCardInfo mTarget;

    public Obj_Card Target { get { return mTarget == null ? null : mTarget.objCard; } }
    public Card TargetCard { get { return mTarget == null ? null : mTarget.card; } }

    bool IsCurTarget(Obj_Char t)
    {
        return (mTarget != null && mTarget.objCard != null && mTarget.objCard == t);
    }

    bool OnClickProdNpc(Obj_NPC npc)
    {
        if (ObjManager.MainPlayer != null && ObjManager.MainPlayer.IsInRelaxAnim())
        {
            Utils.CenterNotice(7009);
            return true;
        }

        if (!InMode(HouseMode.NORMAL))
            return true;

        if (mYard == null)
            return true;

        var prod = mYard.YardProd;
        if (IsOwner(LoginData.user.guid))
        {
            if (prod == null || prod.ProdId <= 0)
            {
                HouseProducePrepear.Open();
            }
            else
            {
                HouseProducing.Open();
            }
        }
        else
        {
            if (prod != null && prod.ProdId > 0 && prod.FinishTime > GameManager.ServerAnsiTime)
            {
                Yard.SendHelp();
            }
            else
            {
                Utils.CenterNotice(8024);
            }
        }

        return true;
    }

    bool OnClickStealNpc(Obj_NPC target)
    {
        if (ObjManager.MainPlayer != null && ObjManager.MainPlayer.IsInRelaxAnim())
        {
            Utils.CenterNotice(7009);
            return true;
        }

        if (!InMode(HouseMode.NORMAL))
            return true;

        if (mYard == null || target == null)
            return true;

        var prod = mYard.YardProd;
        if (!IsOwner(LoginData.user.guid) && prod.ProdId > 0 && prod.FinishTime <= GameManager.ServerAnsiTime)
        {
            Tab_HouseSkin skin = GetCurSkin();
            if (skin == null)
                return true;

            if (ObjManager.MainPlayer != null)
            {
                ObjManager.MainPlayer.PlayAnim(skin.ClickStealNpcAnim);
            }
            
            for (int i = 0; i < skin.getStealNpcCount(); ++i)
            {
                int npc = skin.GetStealNpcbyIndex(i);
                if (npc != GlobeVar.INVALID_ID && npc == target.SceneNPCID)
                {
                    Yard.SendSteal(i);
                    return true;
                }
            }
        }
        
        return true;
    }

    bool OnClickObj(Obj_Char c, RaycastHit hit, bool isDoubleClick)
    {
        if (ObjManager.MainPlayer != null && ObjManager.MainPlayer.IsInRelaxAnim())
            return true;

        switch (mMode)
        {
            case HouseMode.EDIT:
                break;
            case HouseMode.NORMAL:
                if (OnSelectObj(c))
                    return true;
                if (OnSelectNpc(c, hit))
                    return true;
                break;
            case HouseMode.INTIMACY:
                break;
        }

        return false;
    }

    bool OnClickScene(RaycastHit hit)
    {
        //如果在竖屏交互中 不可移动 不播放点击特效
        if (RoleTouchController.Instance != null)
        {
            return false;
        }

        //判断主角是否创建
        if (null == ObjManager.MainPlayer)
        {
            return false;
        }

        if (ObjManager.MainPlayer.IsInRelaxAnim())
            return true;

        //故事模式不可移动时, 不处理点击事件
        if (GameManager.storyManager.StoryMode && !ObjManager.MainPlayer.EventMove)
        {
            return false;
        }

        if (!InMode(HouseMode.NORMAL))
            return false;

        // 选中目标不走统一选中逻辑, 在这里手动清除
        if (ObjManager.MainPlayer.m_selectTarget != null)
        {
            ObjManager.MainPlayer.m_selectTarget = null;
            if (TargetFrameController.Instance != null)
            {
                TargetFrameController.Instance.RefreshTarget();
            }
            CleanTarget();
        }

        NavMeshHit navHit;
        if (!NavMesh.SamplePosition(hit.point, out navHit, 0.1f, NavMesh.AllAreas))
        {
            PlayEffect(GlobeVar.CLICK_SCENE_EFFECT_NIGHT, hit.point);

            return false;
        }

        //根据不同环境显示不同点地特效
        if (null != GameManager.EnvManager && null != GameManager.EnvManager.CurEvnTable)
        {
            if (GameManager.EnvManager.CurEvnTable.EnvirType == (int)SCENE_ENVIRONMENT_TYPE.NIGHT)
            {
                PlayEffect(GlobeVar.CLICK_SCENE_EFFECT_NIGHT, hit.point);
            }
            else
            {
                PlayEffect(GlobeVar.CLICK_SCENE_EFFECT_DAY, hit.point);
            }
        }
        else
        {
            PlayEffect(GlobeVar.CLICK_SCENE_EFFECT_DAY, hit.point);
        }

        if (ObjManager.MainPlayer.IsCanOperate_Move())
        {
            ObjManager.MainPlayer.MoveTo(hit.point, null, 0.0f, (int)CHAR_ANIM_ID.Run);
        }

        return true;
    }

    bool OnSelectObj(Obj_Char obj)
    {
        if (!InMode(HouseMode.NORMAL) || obj == null)
            return false;

        if (obj.IsPlayer() && !obj.IsMainPlayer())
        {
            if (ObjManager.MainPlayer != null)
            {
                CleanTarget();

                ObjManager.MainPlayer.m_selectTarget = obj;

                if (TargetFrameController.Instance != null)
                {
                    TargetFrameController.Instance.RefreshTarget();
                }
                
                return true;
            }
        }
        else if (obj.IsCard())
        {
            if (mTarget != null && mTarget.objCard != null)
            {
                if (obj == mTarget.objCard)
                {
                    //不作处理, 也不让选中事件传递下去
                    return true;
                }
                CleanTarget();
            }

            SceneCardInfo oc = GetSceneCardInfo(obj as Obj_Card);
            if (oc == null)
                return false;
            mTarget = oc;

            if (mNavManager == null)
                return false;
            var nav = mNavManager.GetNavigator(obj);
            if (nav == null)
                return false;
            nav.Stop();
            
            if (ObjManager.MainPlayer != null)
            {
                ObjManager.MainPlayer.FaceTo(obj.Position);
                obj.FaceTo(ObjManager.MainPlayer.Position);

                ObjManager.MainPlayer.MoveTo(obj.Position, null, 2f, (int) CHAR_ANIM_ID.Run);
            }

            if (IsOwner(LoginData.user.guid))
            {
                if (ObjManager.MainPlayer != null)
                {
                    ObjManager.MainPlayer.m_selectTarget = obj;
                }
                if (TargetFrameController.Instance != null)
                {
                    TargetFrameController.Instance.RefreshTarget();
                }
            }

            return true;
        }
        return false;
    }

    bool OnSelectNpc(Obj_Char obj, RaycastHit hit)
    {
        if (!InMode(HouseMode.NORMAL))
            return false;

        Obj_NPC npc = obj as Obj_NPC;
        if (npc == null)
            return false;

        if (npc.CanBeSelect)
        {
            //特殊处理，在RayCastAll的时候，只选择有任务的NPC
            //因为在这个情况下，未选中的时候会有一个RaycastAllFailed进行兜底处理
            if (GameManager.storyManager.StoryMode && GameManager.InputManager.isRaycastAll)
            {
                if (npc.GetStoryEvent() == null)
                {
                    return false;
                }
            }
            //广播出去
            npc.SendMessage("OnClick", hit, SendMessageOptions.DontRequireReceiver);
        }

        if (hit.collider != null)
        {
            float stopRange = -1.0f;
            if (ObjManager.MainPlayer != null)
            {
                if (!ObjManager.MainPlayer.IsCanOperate_Move() && GameManager.storyManager.StoryMode)
                {
                    // 故事模式主角不能移动时, 如果距离npc过远则无法触发事件, 在这里强制扩大距离
                    stopRange = 100f;
                }
                ObjManager.MainPlayer.MoveTo(npc, stopRange, (int)CHAR_ANIM_ID.Run);
            }
        }

        return true;
    }

    #endregion

    #region Utils
    public bool PositionOnNavMesh(Vector3 pos)
    {
        NavMeshHit navHit;
        return NavMesh.SamplePosition(pos, out navHit, 0.1f, NavMesh.AllAreas);
    }

    public bool IsOwner(UInt64 guid)
    {
        return (mYard != null && mYard.OwnerGuid == guid);
    }

    public Tab_HouseSkin GetCurSkin()
    {
        if (mYard == null)
            return null;

        Tab_HouseSkin skin = TableManager.GetHouseSkinByID(mYard.SkinId, 0);
        if (skin == null)
            skin = Yard.GetDefaultSkin();

        return skin;
    }

    public Obj_NPC GetProdNpc()
    {
        Tab_HouseSkin skin = GetCurSkin();
        if (skin == null)
        {
            LogModule.ErrorLogFormat("house skin {0} not found", mYard.SkinId);
            return null;
        }

        return ObjManager.GetObjNPCBySceneNPCID(skin.ProdNpc);
    }

    static public Vector3 GetScreenPos(Vector3 objPos)
    {
        var cam = GameManager.CameraManager.MainCamera;
        if (cam == null)
            return Vector3.zero;

        Vector3 screenPos = cam.WorldToScreenPoint(objPos) - new Vector3(Screen.width / 2, Screen.height / 2, 0f);
        screenPos.x *= GameManager.m_fUIScale;
        screenPos.y *= GameManager.m_fUIScale;
        screenPos.z = 0f;
        return screenPos;
    }

    static public bool ScreenPos2WorldSpace(Vector3 mousePos, float dist, out Vector3 finalPosition)
    {
        var cam = GameManager.CameraManager.MainCamera;
        if (cam == null)
        {
            finalPosition = Vector3.zero;
            return false;
        }
        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit hit;

        //Debug.DrawLine(ray.origin, ray.origin + dist*ray.direction.normalized, Color.red);
        
        if (Physics.Raycast(ray, out hit, dist, 1 << (int)EditorLayer.MOVELAYER))
        {
            finalPosition = hit.point;
            //Debug.DrawLine(finalPosition, finalPosition + Vector3.up*2, Color.green);
            return true;
        }
        
        Vector3 point = ray.origin + dist*ray.direction.normalized;
        ray = new Ray(point, Vector3.down);
        if (Physics.Raycast(ray, out hit, dist, 1 << (int)EditorLayer.MOVELAYER))
        {
            finalPosition = hit.point;
            //Debug.DrawLine(ray.origin, finalPosition, Color.green);
            return true;
        }

        finalPosition = Vector3.zero;
        return false;
    }
    #endregion

    

    //public bool HasEditorTarget { get { return mEditTarget != null && mEditTarget.Valid(); } }

    //private Vector3 mLastMousePosition;
    //private Vector3 mLastMousePositionDelta;

    //public void DragStart(Card card)
    //{
    //    if (card == null)
    //        return;

    //    SceneCardInfo sci = null;
    //    if (mCards.TryGetValue(card.Guid, out sci))
    //    {
    //        return;
    //    }
    //    else
    //    {
    //        CleanTarget();

    //        if (mCards.Count >= GlobeVar._GameConfig.m_nYardMaxCardNum)
    //        {
    //            Utils.CenterNotice(7309);
    //            return;
    //        }

    //        var obj = PutCard(card);
    //        if (obj != null)
    //        {
    //            SwitchTarget(obj);
    //            SwitchEditorState(EditorState.MOVE);
    //        }
    //        else
    //        {
    //            Utils.CenterNotice(7309);
    //        }
    //    }
    //}

    //[SerializeField] private float dist = 20f;
    //protected override void Update() 
    //{
    //    base.Update();

    //    switch (mEditorState)
    //    {
    //        case EditorState.MOVE:
    //            Move();
    //            break;
    //        case EditorState.ROTATE:
    //            Rotate();
    //            break;
    //    }
    //}

    //public void DragEnd(Card card)
    //{   
    //    SwitchEditorState(EditorState.NONE);
    //}

    //public void Move()
    //{
    //    if (mEditTarget == null || !mEditTarget.Valid())
    //        return;

    //    //moving & updating effect
    //    Vector3 finalPos = Vector3.zero;
    //    if (ScreenPos2WorldSpace(mLastMousePosition, dist, out finalPos))
    //    {
    //        NavMeshHit navHit;
    //        if (NavMesh.SamplePosition(finalPos, out navHit, 0.1f, NavMesh.AllAreas))
    //        {
    //            mEditTarget.objCard.ObjTransform.localPosition = finalPos;
    //        }
    //    }
    //}

    //public void Rotate()
    //{
    //    if (mEditTarget == null || !mEditTarget.Valid())
    //        return;

    //    var rot = mEditTarget.objCard.ObjTransform.eulerAngles;
    //    rot.y -= mLastMousePositionDelta.x;
    //    mEditTarget.objCard.ObjTransform.eulerAngles = rot;
    //}

    //void LateUpdate()
    //{
    //    if (InMode(HouseMode.EDIT))
    //    {
    //        var pos = Input.mousePosition;
    //        mLastMousePositionDelta = pos - mLastMousePosition;
    //        mLastMousePosition = pos;
    //    }
    //}
}
