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
        UIManager.ShowUI(UIInfo.PlayerFrame, OnLoadPlayerFrameUIFinish);
        UIManager.ShowUI(UIInfo.TargetFrame);
        UIManager.ShowUI(UIInfo.CenterNoticeEx);
        UIManager.ShowUI(UIInfo.RollNotice);
    }

    public override void OnCloseGameUI()
    {
        UIManager.CloseUI(UIInfo.HouseMainUI);
        UIManager.CloseUI(UIInfo.JoyStick);
        UIManager.CloseUI(UIInfo.PlayerFrame);
        UIManager.CloseUI(UIInfo.TargetFrame);
        UIManager.CloseUI(UIInfo.CenterNoticeEx);
        UIManager.CloseUI(UIInfo.RollNotice);
        UIManager.CloseUI(UIInfo.HouseRelationRoot);
        base.OnCloseGameUI();
    }

    void OnLoadPlayerFrameUIFinish(bool bSuccess, object param)
    {
        if (bSuccess)
        {
            if (null != PlayerFrameController.Instance())
            {
                if (GameManager.storyManager.StoryMode == false)
                {
                    PlayerFrameController.Instance().ShowPlayerFrame(true);
                }
                else
                {
                    PlayerFrameController.Instance().ShowPlayerFrame(false);
                }
            }
        }
    }
    private _Yard mYard;
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
        if (PreviewModel_ReturnScene != GlobeVar.INVALID_ID && TVId !=GlobeVar.INVALID_ID)
        {
            CutsceneManager.PlayCutscene(TVId, OnTVPlayFinish);          //播放TV
            return;
        }
        LoadScene(GameManager.PlayerDataPool.YardData.ProtoYard);
        
        GameManager.InputManager.onClickObj = (c, hit, click) =>
        {
            if (ObjManager.MainPlayer != null && ObjManager.MainPlayer.IsInRelaxAnim())
                return true;

            switch (mMode)
            {
                case HouseMode.EDIT:
                    Obj_Card oc = c as Obj_Card;

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
        };
        GameManager.InputManager.onClickScene = (RaycastHit hit) =>
        {
            //如果在竖屏交互中 不可移动 不播放点击特效
            if (RoleTouchController.Instance != null)
            {
                return false;
            }

            if (ObjManager.MainPlayer != null && ObjManager.MainPlayer.IsInRelaxAnim())
                return true;

            //故事模式不可移动时, 不处理点击事件
            if (GameManager.storyManager.StoryMode && !ObjManager.MainPlayer.EventMove)
            {
                return false;
            }

            if (!InMode(HouseMode.NORMAL))
                return false;

            if (ObjManager.MainPlayer != null)
            {
                var target = ObjManager.MainPlayer.m_selectTarget;
                if (target == null)
                {
                    CleanTarget();
                    //return true;
                }
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

            if (ObjManager.MainPlayer != null)
            {
                if (ObjManager.MainPlayer.IsCanOperate_Move())
                {
                    ObjManager.MainPlayer.MoveTo(hit.point, null, 0.0f, (int)CHAR_ANIM_ID.Run);
                }
            }

            return true;
        };

        Yard.msDelOnYardSync += OnYardSync;
        Yard.msDelOnYardCardUpdated += OnYardCardUpdated;

        mMode = HouseMode.NORMAL;
    }



    public override void OnLeaveScene()
    {
        Yard.msDelOnYardSync -= OnYardSync;
        Yard.msDelOnYardCardUpdated -= OnYardCardUpdated;

        GameManager.CameraManager.IsDragLock = false;

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
        LoadNavManager();
        Load(pYard);
        mCamStartTrans = gameObject.transform.Find("EditorCamStart");
        mIntimacyTarget = gameObject.transform.Find("IntimacyTarget");
        mIntimacyCard = gameObject.transform.Find("Intimacy_Card");
        mIntimacyPlayer = gameObject.transform.Find("Intimacy_Player");
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

        GameObject res = AssetManager.LoadResource("HouseNav/HouseNavRoot") as GameObject;
        if (res == null)
        {
            LogModule.WarningLog("cannot load HouseNavRoot");
            return;
        }
        GameObject navRoot = Instantiate(res, Vector3.zero, Quaternion.identity) as GameObject;
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

    void OnYardSync(ProtobufPacket.YardOp op, _Yard pYard)
    {
        switch (op)
        {
            case YardOp.YardOp_SYNC:
                Load(pYard);
                break;
        }
    }

    void OnYardCardUpdated(ProtobufPacket.YardOp op, _YardCard ycLoaded)
    {
        switch (op)
        {
            case YardOp.YardOp_SYNC:
                TakeCard(ycLoaded.CardGuid);
                PutCard(ycLoaded);
                break;
            case YardOp.YardOp_PUT:
                PutCard(ycLoaded);
                break;
            case YardOp.YardOp_TAKE:
                TakeCard(ycLoaded.CardGuid);
                break;
        }
    }

    void OnYardChangeSkin(bool suc, int skin)
    {
        if (!suc)
            return;

        Load(mYard);
    }

    System.Collections.IEnumerator IntimacyCardShowBubble()
    {
        if (mIntimacyObjCard == null)
        {
            yield break;
        }
        Tab_CardExtend tabCardEx = TableManager.GetCardExtendByID(mIntimacyObjCard.CardId, 0);
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
        Tab_CardExtend tabCardEx = TableManager.GetCardExtendByID(mIntimacyObjCard.CardId, 0);
        if(tabCardEx==null)
        {
            return;
        }
        Tab_CardIntimacyPhiz tabPhiz = null;
        INTIMACY_GIFT_LIKE likeDegree = sceneCard.card.GetIntimacyGiftLike(pak.itemID);       //拿到喜好等级
        switch(likeDegree)
        {
            case INTIMACY_GIFT_LIKE.FAVORITE:
                mIntimacyObjCard.PlayAnim(tabCardEx.FavoriteAnimation);
                if (tabCardEx.FavoriteSound != GameManager.SoundManager.GetCurRealSoundId())
                {
                    GameManager.SoundManager.PlayRealSound(tabCardEx.FavoriteSound);
                }
                tabPhiz = TableManager.GetCardIntimacyPhizByID(0, 0);
                break;
            case INTIMACY_GIFT_LIKE.NORMAL:
                mIntimacyObjCard.PlayAnim(tabCardEx.NormalAnimation);
                if (tabCardEx.NormalSound != GameManager.SoundManager.GetCurRealSoundId())
                {
                    GameManager.SoundManager.PlayRealSound(tabCardEx.NormalSound);
                }
                tabPhiz = TableManager.GetCardIntimacyPhizByID(1, 0);
                break;
            case INTIMACY_GIFT_LIKE.DISLIKE:
                mIntimacyObjCard.PlayAnim(tabCardEx.DislikeAnimation);
                if (tabCardEx.DislikeSound != GameManager.SoundManager.GetCurRealSoundId())
                {
                    GameManager.SoundManager.PlayRealSound(tabCardEx.DislikeSound);
                }
                tabPhiz = TableManager.GetCardIntimacyPhizByID(2, 0);
                break;
        }
        if (tabPhiz != null)
        {
            mIntimacyObjCard.Bubble(tabPhiz.GetPhizbyIndex(UnityEngine.Random.Range(0, tabPhiz.getPhizCount())));
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

        if (!CanPutCard(card))
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

            oc.name = card.CardId.ToString();
            oc.CardId = card.CardId;
            oc.MoveSpeed = mNavManager.Speed;
            oc.InitNavAgent();

            var ci = new SceneCardInfo(card, oc);
            mCards.Add(card.Guid, ci);

            // set nav agent
            var tab = TableManager.GetCardExtendByID(card.CardId, 0);
            if (tab==null)
            {
                return null;
            }
            int[] animList = CardTool.GetAnimList(tab);
            nav.SetAgent(oc, animList);
            nav.Run();

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

        if (card.IntimacyLevel < GlobeVar._GameConfig.m_nYardCardIntimacy)
            return false;

        return true;
    }

    #endregion

    #region Input

    void CleanTarget()
    {
        if (mTarget == null || mTarget.objCard == null)
            return;

        var nav = mNavManager.GetNavigator(mTarget.objCard);
        if (nav != null)
        {
            nav.Run();
        }
        mTarget = null;
    }

    private SceneCardInfo mTarget;

    public Obj_Card Target { get { return mTarget == null ? null : mTarget.objCard; } }

    bool IsCurTarget(Obj_Char t)
    {
        return (mTarget != null && mTarget.objCard != null && mTarget.objCard == t);
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
                TargetFrameController.Instance.RefreshTarget();
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
