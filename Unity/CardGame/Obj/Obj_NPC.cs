/********************************************************************************
 *	文件名：	Obj_NPC.cs
 *	全路径：	\Script\Obj\Obj_NPC.cs
 *	创建人：	李嘉
 *	创建时间：2017-02-14
 *
 *	功能说明：游戏中的NPC
 *	修改记录：
*********************************************************************************/

using System;
using UnityEngine;
using System.Collections;
using Games.GlobeDefine;

using Games.Table;
using ProtobufPacket;

namespace Games.LogicObj
{

    public class Obj_NPC : Obj_Char
    {
      
        #region 基础属性
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            //UpdateStep();

            if (IsMoving)
            {
                return;
            }

            UpdateHelloPlayer();

            if ( m_bFaceToSelecter)
            {
                UpdateFocusPlayer();
            }
        }


        //NPC触发时间

        #endregion


        #region 初始化

        public Obj_NPC()
        {            
        }

        protected override OBJ_TYPE _getObjType()
        {
            return OBJ_TYPE.OBJ_NPC; ;
        }

		public override bool Init(Obj_Init_Data initData)
        {
            if (base.Init(initData) == false)
            {
                return false;
            }

            if (RoleName.Equals(""))
            {
                RoleName = "NPC role";
            }

            if (m_bIsNeedNavAgent)
            {
                InitNavAgent();
            }
            
            return true;
        }

        #endregion

       
        public override void InitNameBoard(bool showName = false)
        {
            _showName = showName;
            AssetManager.LoadHeadInfoPrefab(UIInfo.NPCHeadInfo, gameObject, "NPCHeadInfo", OnLoadHeadInfo);
        }

        void OnLoadHeadInfo(GameObject resHeadInfo)
        {
            if (resHeadInfo == null)
            {
                return;
            }

            HeadInfo = resHeadInfo;
            m_NpcHeadInfoLogic = HeadInfo.GetComponent<NpcHeadInfoLogic>();

            if (m_NpcHeadInfoLogic == null)
            {
                return;
            }

            m_NpcHeadInfoLogic.Init(this, _showName);
            m_NpcHeadInfoLogic.UpdateHeadInfoColor(GetNameBoardColor());
            RefreshHeadFlag();
            float mountHeight = 0.2f;

            if (HeadInfo != null)
            {
                NameBoard nameBoard = HeadInfo.GetComponent<NameBoard>();
                if (nameBoard != null)
                {
                    nameBoard.SetHeightRefix(NameBoardRefixType.Mount, mountHeight);
                }
            }

            // 是否需要显示头顶新手特效
            if (m_NeedHeadTutorialEffect)
            {
                m_NpcHeadInfoLogic.PlayTutorialEffect();
                m_NeedHeadTutorialEffect = false;
            }

            // 玄学运势指引
            UpdateAugurEffect();
        }

        public void UpdateAugurEffect()
        {
            //摇签入口新手指引
            if (m_NpcHeadInfoLogic != null && (m_nSceneNPCID == GlobeVar.AUGUR_ENTRANCE_NPC || m_nSceneNPCID == GlobeVar.AUGUR_LOBBY_NPC))
            {
                if (TutorialManager.IsFunctionUnlock((int)FunctionUnlockId.Augur) 
                    && AugurTool.GetLeftTimes() > 0
                    && PlayerPreferenceData.ReadAugurRed() == false)
                {
                    //只有在没进行过摇签时 才显示引导特效
                    m_NpcHeadInfoLogic.PlayTutorialEffect();
                }
                else if(m_NpcHeadInfoLogic.IsTutorialEffectPlaying())
                {
                    m_NpcHeadInfoLogic.StopTutorialEffect();
                }
            }
        }

        #region 交互相关
        private bool m_bCanBeSelect = true; //是否可以被点选
        public bool CanBeSelect
        {
            get { return m_bCanBeSelect; }
            set
            {
                if (null != m_Collider)
                {
                    m_Collider.enabled = value;
                }
                m_bCanBeSelect = value;
            }
        }

        private bool m_bFaceToSelecter = true;      //靠近的时候是否跟随玩家
        public bool FaceToSelecter
        {
            get { return m_bFaceToSelecter; }
            set { m_bFaceToSelecter = value; }
        }
        
        private int m_nDialogID = GlobeVar.INVALID_ID;       //当前NPC的基本对话ID
        public int DialogID
        {
            get { return m_nDialogID; }
            set
            {
                m_nDialogID = value;
                NpcDialog = TableManager.GetNpcDialogByID(m_nDialogID, 0);
            }
        }


        private Tab_NpcDialog m_NpcDialog;

        public Tab_NpcDialog NpcDialog
        {
            get { return m_NpcDialog; }
            set { m_NpcDialog = value; }
        }

        private int m_nSceneNPCID = GlobeVar.INVALID_ID;     //当前NPC在SceneNPC表格中的ID
        public int SceneNPCID
        {
            get { return m_nSceneNPCID; }
            set
            {
                m_nSceneNPCID = value; 
                var tab = TableManager.GetSceneNpcByID(m_nSceneNPCID, 0);
                if (tab != null)
                {
                    NpcHeadFlagIndex = tab.HeadFlag;
                }
            }
        }
        
		public StoryEvent GetStoryEvent()
		{
			//判断当前剧情是不是可以被触发
			if (null != GameManager.storyManager && 
				null != GameManager.storyManager.CurStory &&
				null != GameManager.storyManager.CurStory.EventList)
			{
				for (int i=0; i< GameManager.storyManager.CurStory.EventList.Count; ++i)
				{
					StoryEvent _curEvent = GameManager.storyManager.CurStory.EventList[i];
					if (null != _curEvent)
					{
						//如果当前的事件是对话事件
						if (_curEvent.GetStoryEventType() == STORY_EVENT.DIALOG)
						{
							//NPC要面向玩家
							if (null != ObjManager.MainPlayer && m_bFaceToSelecter)
							{
								FaceTo(ObjManager.MainPlayer.Position);
							}

							StoryEvent_Dialog _dialogEvent = _curEvent as StoryEvent_Dialog;
							if (null != _dialogEvent && _dialogEvent.IsNPCCanActiveEvent(m_nSceneNPCID))
							{
								return _dialogEvent;
							}
						}
						//如果当前事件是战斗事件
						else if (_curEvent.GetStoryEventType() == STORY_EVENT.BATTLE)
						{
							StoryEvent_Battle _battleEvent = _curEvent as StoryEvent_Battle;
							if (null != _battleEvent && _battleEvent.IsNPCCanEnterBattle(m_nSceneNPCID))
							{
								return _battleEvent;
							}
						}
						//如果当前事件是TV
						else if (_curEvent.GetStoryEventType() == STORY_EVENT.TV)
						{
							StoryEvent_TV _tvEvent = _curEvent as StoryEvent_TV;
							if (null != _tvEvent && _tvEvent.IsNPCCanEnterTV(m_nSceneNPCID))
							{
								return _tvEvent;
							}
						}
					}
				}
			}
			
			return null;
		}

        public delegate bool OnMoveToNpcDoneDel();

        public OnMoveToNpcDoneDel onMainPlayerMoveToNpcDone;

        public void OnMoveToNpcDone()
        {
            if (onMainPlayerMoveToNpcDone != null && onMainPlayerMoveToNpcDone())
            {
                return;
            }
            ShowDialog();
        }

        public void ShowDialog()
        {
            ShowDialog(m_nDialogID);
        }

        public void ShowDialog(int dialogID)
        {
            //关闭MessageBox
            UIManager.CloseUI(UIInfo.MessageBox);

            //关闭小地图
            if (null != SmallMapController.Instance)
            {
                SmallMapController.Instance.OnClickSmallMapClose();
            }

            //停止靠近语音
            Tab_SceneNpc _sceneNPC = TableManager.GetSceneNpcByID(m_nSceneNPCID, 0);
            if (null != _sceneNPC)
            {
                if (GameManager.SoundManager.IsSoundEffectPlaying(_sceneNPC.HelloSoundID))
                {
                    GameManager.SoundManager.StopSoundEffect(_sceneNPC.HelloSoundID);
                }
            }

            StoryEvent _event = GetStoryEvent();
			if (null != _event)
			{
				//对话事件特殊处理面向玩家
				if (_event.GetStoryEventType() == STORY_EVENT.DIALOG)
				{
					//NPC要面向玩家
					if (null != ObjManager.MainPlayer && m_bFaceToSelecter)
					{
						FaceTo(ObjManager.MainPlayer.Position);
                        OnFocusPlayer();
					}
				}

				//统一调用事件的Start
				if (_event.State == STORY_EVENT_STATE.DEACTIVE)
				{
					_event.Start();
				    NpcHeadFlagIndex = GlobeVar.INVALID_ID;
				}
			}
            else if (dialogID != GlobeVar.INVALID_ID)
            {
                ShowCommonDialog(dialogID);

                //NPC要面向玩家
                if (null != ObjManager.MainPlayer && m_bFaceToSelecter)
                {
                    FaceTo(ObjManager.MainPlayer.Position);
                    OnFocusPlayer();
                }
            }

            if (m_NpcHeadInfoLogic != null && m_NpcHeadInfoLogic.IsTutorialEffectPlaying()
                && m_nSceneNPCID != GlobeVar.Achievement_SceneNpcId
                && m_nSceneNPCID != GlobeVar.AUGUR_ENTRANCE_NPC
                && m_nSceneNPCID != GlobeVar.AUGUR_LOBBY_NPC)
            {
                m_NpcHeadInfoLogic.StopTutorialEffect();
            }

            if (TutorialRoot.IsGroupStep(TutorialGroup.MoveRot, 3))
            {
                TutorialRoot.TutorialOver();
            }
        }

        //显示通用的对话框，ID为NPCDIalog中的ID
        private void ShowCommonDialog(int dialogID)
        {
            if (CommonDialogController.Instance() != null)
            {
                CommonDialogController.Instance().OnClose();
            }

            Tab_NpcDialog _tabDialog = TableManager.GetNpcDialogByID(dialogID, 0);
            if (null != _tabDialog)
            {
                for (int i = 0; i < _tabDialog.getOptionTypeCount(); ++i)
                {
                    if (_tabDialog.GetOptionTypebyIndex(i) == (int)DIALOG_TYPE.ENTERSTORY && i< _tabDialog.getOptionIntParamCount())
                    {
                        //如果是剧情NPC  并且当前没有可以体验的剧情时不弹对话
                        int intParam = _tabDialog.GetOptionIntParambyIndex(i);

                        var reason = StoryManager.StoryLineAvailableWithReason(intParam, GlobeVar.INVALID_GUID);
                        if (reason != StoryManager.StoryLineNAReason.AVAILABLE)
                        {
                            Utils.CenterNotice(StoryManager.CenterNoticeReasonText(reason, intParam));
                            return;
                        }
                    }
                }

                for (int i = 0; i < _tabDialog.getOptionTypeCount(); ++i)
                {
                    if (_tabDialog.GetOptionTypebyIndex(i) == (int)DIALOG_TYPE.DrawAugur)
                    {
                        if (!TutorialManager.IsFunctionUnlock((int)FunctionUnlockId.Augur))
                        {
                            Utils.CenterNotice(5747);
                            return;
                        }
                    }
                }
                if (m_nSceneNPCID == GlobeVar.TutorialAsyncPVP_SceneNpcId_In && 
                    AsyncPVPScene.m_TutorialGroupOnMoveOver == TutorialGroup.AsyncPVP && AsyncPVPScene.m_TutorialStepOnMoveOver == 4 &&
                    TutorialManager.IsOpenTutorial)
                {
                    // 异步PVP新手指引中先不弹对话
                    if (GameManager.CurScene is AsyncPVPScene)
                    {
                        AsyncPVPScene pvpScene = GameManager.CurScene as AsyncPVPScene;
                        pvpScene.UpdateTutorial(TutorialGroup.AsyncPVP, 4);

                        AsyncPVPScene.m_TutorialGroupOnMoveOver = TutorialGroup.Invalid;
                        AsyncPVPScene.m_TutorialStepOnMoveOver = GlobeVar.INVALID_ID;
                    }
                }
                else if (string.IsNullOrEmpty(_tabDialog.OpenDialog))
                {
                    UIManager.ShowUI(UIInfo.StoryTalk, OnShowCommonDialogOver, _tabDialog);
                }
                else
                {
                    UIPathData _pathData = Utils.GetUIPathDataByMemberName(_tabDialog.OpenDialog);
                    if (null != _pathData)
                    {
                        UIManager.ShowUI(_pathData, OnShowCommonDialogOver, _tabDialog, UIStack.StackType.PushAndPop);
                    }                 
                }
            }
        }

		private void OnShowCommonDialogOver(bool bSuccess, object param)
		{
			if (null == param)
			{
				return;
			}

			Tab_NpcDialog _tabDialog = param as Tab_NpcDialog;
			if (null == _tabDialog)
			{
				return;
			}

			if (null != CommonDialogController.Instance())
			{
				CommonDialogController.Instance().Init(_tabDialog);
			}

            //播放语音
            if (null != GameManager.SoundManager && _tabDialog.SoundID > GlobeVar.INVALID_ID)
            {
                GameManager.SoundManager.PlayRealSound(_tabDialog.SoundID);
            }
		}

        //public static int m_nCurFocusPlayerSceneNpcID = GlobeVar.INVALID_ID;        //当前关注主角的NPC的SceneNPCID
        public int m_nCurFocusPlayerSceneNpcID = GlobeVar.INVALID_ID;               //当前关注主角的NPC的SceneNPCID(改为靠近就是关注后，有可能存在多个Npc同时关注，所以去掉static)
        private float m_fFocusMaxDistance = 3.0f;                                   //NPC关注玩家的最远距离
        public float FocusMaxDistance
        {
            set { m_fFocusMaxDistance = value; }
        }
        private static int s_nCurFocusPlayerSoundNpcID = GlobeVar.INVALID_ID;        //当前关注语音的Npc（防止Npc不同，但语音相同，导致没有重新开始播放）
        public static void CancelFocusPlayerSound()
        {
            GameManager.SoundManager.StopNpcHelloSound();
            s_nCurFocusPlayerSoundNpcID = GlobeVar.INVALID_ID;
        }

        public void OnFocusPlayer()
        {
            m_nCurFocusPlayerSceneNpcID = m_nSceneNPCID;
        }

        public void OnUnFocusPlayer()
        {
            m_nCurFocusPlayerSceneNpcID = GlobeVar.INVALID_ID;
            if (m_nSceneNPCID != GlobeVar.INVALID_ID)
            {
                Tab_SceneNpc _sceneNPC = TableManager.GetSceneNpcByID(m_nSceneNPCID, 0);
                if (null != _sceneNPC)
                {
                   RotateToWithTween(new Vector3(0.0f, _sceneNPC.FaceDirection, 0.0f) - ObjTransform.rotation.eulerAngles, 0.4f);
                  //ObjTransform.localRotation = Quaternion.Euler(new Vector3(0.0f, _sceneNPC.FaceDirection, 0.0f));
                }
            }
        }

        public void UpdateFocusPlayer()
        {
            if (m_nCurFocusPlayerSceneNpcID == GlobeVar.INVALID_ID)
            {
                return;
            }

            if (m_fFocusMaxDistance < 0)    //靠近距离无效
            {
                return;
            }

            if (null == ObjManager.MainPlayer || null == ObjManager.MainPlayer.ObjTransform)
            {
                return;
            }

            //如果当前的关注目标是自己
            if (m_nCurFocusPlayerSceneNpcID == m_nSceneNPCID)//看着主角
            {
                //检查是否离开
                float fDistance = Vector3.Distance(ObjTransform.position, ObjManager.MainPlayer.ObjTransform.position);
                if (fDistance > m_fFocusMaxDistance)
                {
                    //距离过远，取消关注
                    OnUnFocusPlayer();
                    return;
                }

                //否则朝向主角
                FaceToWithTween(ObjManager.MainPlayer.ObjTransform.position, 0.2f);
            }
        }

        public void UpdateHelloPlayer()
        {
            if (m_nCurFocusPlayerSceneNpcID == m_nSceneNPCID)   //靠近ing
            {
                return;
            }

            if (null == ObjManager.MainPlayer || null == ObjManager.MainPlayer.ObjTransform)
            {
                return;
            }

            //检查是否玩家靠近
            float fDistance = Vector3.Distance(ObjTransform.position, ObjManager.MainPlayer.ObjTransform.position);
            if (fDistance <= m_fFocusMaxDistance)
            {
                //距离过近，开始关注
                m_nCurFocusPlayerSceneNpcID = m_nSceneNPCID;
               
                Tab_SceneNpc _sceneNPC = TableManager.GetSceneNpcByID(m_nSceneNPCID, 0);
                if (null != _sceneNPC)
                {
                    //action hello
                    if (1 == _sceneNPC.HelloAnm)
                    {
                        PlayAnim((int)CHAR_ANIM_ID.Hello);
                    }

                    //say hello
                    //if (!GameManager.SoundManager.IsSoundEffectPlaying(_sceneNPC.HelloSoundID))
                    //{
                    //    SoundManager.PlaySoundAtPosForSkill(_sceneNPC.HelloSoundID, ObjTransform.position);
                    //}
                    if (GlobeVar.INVALID_ID != _sceneNPC.HelloSoundID
                        && ( s_nCurFocusPlayerSoundNpcID != m_nSceneNPCID
                            || GameManager.SoundManager.GetCurNpcHelloSoundId() != _sceneNPC.HelloSoundID))
                    {
                        GameManager.SoundManager.PlayNpcHelloSound(_sceneNPC.HelloSoundID, ObjTransform.position);
                        s_nCurFocusPlayerSoundNpcID = m_nSceneNPCID;
                    }
                }
            }
        }

        #endregion


        #region 任务提示板
        private int m_NpcHeadFlagIndex = -1; //NPC头上任务图标在table中索引

        private bool m_NeedHeadTutorialEffect = false;
        public bool NeedHeadTutorialEffect
        {
            get { return m_NeedHeadTutorialEffect; }
            set { m_NeedHeadTutorialEffect = value; }
        }

        //名字版
        private NpcHeadInfoLogic m_NpcHeadInfoLogic = null;
        public NpcHeadInfoLogic NpcHeadInfoLogic
        {
            get { return m_NpcHeadInfoLogic; }
        }

        public bool IsHeLuoRiftSceneNpcHeadEffect()
        {
            return false == GameManager.PlayerDataPool.IsTutorialQuestFinish(GlobeVar.TutorialQuestId_HeLuoRift) &&
                false == GameManager.PlayerDataPool.IsTutorialQuestAward(GlobeVar.TutorialQuestId_HeLuoRift);
        }

        enum E_NPC_FLAG_STATUS
        {
            STATUS_UNKNOW,
            STATUS_FORCE_HIDE_BUT_ACTIVE,
            STATUS_FORCE_HIDE_BUT_UNACTIVE,
            STATUS_UNFORCE_HIDE_BUT_ACTIVE,
            STATUS_UNFORCE_HIDE_BUT_UNACTIVE,

        }

        E_NPC_FLAG_STATUS headFlagStatus = E_NPC_FLAG_STATUS.STATUS_UNKNOW;
        public int NpcHeadFlagIndex
        {
            set
            {
                m_NpcHeadFlagIndex = value;
                if (-1 == value)
                {
                    headFlagStatus = E_NPC_FLAG_STATUS.STATUS_UNFORCE_HIDE_BUT_UNACTIVE;
                }
                else
                {
                    headFlagStatus = E_NPC_FLAG_STATUS.STATUS_UNFORCE_HIDE_BUT_ACTIVE;
                }
            }

            get
            {
                return m_NpcHeadFlagIndex;
            }
        }

        private MissionBoardState _BoardState = MissionBoardState.MISSION_NONE;

        public MissionBoardState BoardState { get; set; }


        public void RefreshHeadFlag()
        {
            BoardState = GetState();
        }

        private MissionBoardState GetState()
        {

            if (GameManager.storyManager != null)
            {
                if (GameManager.storyManager.StoryMode)
                {
                    return MissionBoardState.MISSION_CANACCEPTED;
                }
            }

            var dialog = NpcDialog;
            if (dialog == null)
            {
                return BoardState;
            }

            if (string.IsNullOrEmpty(dialog.OpenDialog))
            {
                return DialogCore.GetHeandFlagState(dialog);
            }
            else
            {
                return DialogCore.GetHeandFlagState(dialog.OpenDialog);
            }

        }

        public bool isShowNameBoard()
        {
            bool ret = false;
            switch (headFlagStatus)
            {
                case E_NPC_FLAG_STATUS.STATUS_FORCE_HIDE_BUT_ACTIVE:
                case E_NPC_FLAG_STATUS.STATUS_FORCE_HIDE_BUT_UNACTIVE:
                    break;

                case E_NPC_FLAG_STATUS.STATUS_UNFORCE_HIDE_BUT_ACTIVE:
                    {
                        ret = true;
                    }
                    break;

                case E_NPC_FLAG_STATUS.STATUS_UNFORCE_HIDE_BUT_UNACTIVE:

                    break;

                default:
                    break;
            }

            return ret;
            
        }

        //显示头顶上任务提示面板
        public override void ShowMissionBoard()
        {
            StoryLeave();
        }

        //隐藏头顶上任务提示面板
        public override void HideMissionBoard()
        {
            StoryEnter();
        }



        public void StoryEnter()
        {
            switch (headFlagStatus)
            {
                case E_NPC_FLAG_STATUS.STATUS_FORCE_HIDE_BUT_ACTIVE:
                case E_NPC_FLAG_STATUS.STATUS_FORCE_HIDE_BUT_UNACTIVE:
                    {
                        return;
                    }

                case E_NPC_FLAG_STATUS.STATUS_UNFORCE_HIDE_BUT_ACTIVE:
                    {
                        headFlagStatus = E_NPC_FLAG_STATUS.STATUS_FORCE_HIDE_BUT_ACTIVE;
                    }
                    break;

                case E_NPC_FLAG_STATUS.STATUS_UNFORCE_HIDE_BUT_UNACTIVE:
                    {
                        headFlagStatus = E_NPC_FLAG_STATUS.STATUS_FORCE_HIDE_BUT_UNACTIVE;
                    }
                    break;

                default:
                    LogModule.ErrorLog("E_NPC_FLAG_STATUS error  1:" + headFlagStatus + " " + m_RoleName);
                    headFlagStatus = E_NPC_FLAG_STATUS.STATUS_UNKNOW;
                    break;
            }


        }

        public void StoryLeave()
        {

            switch (headFlagStatus)
            {
                case E_NPC_FLAG_STATUS.STATUS_FORCE_HIDE_BUT_ACTIVE:
                    {
                        headFlagStatus = E_NPC_FLAG_STATUS.STATUS_UNFORCE_HIDE_BUT_ACTIVE;
                    }
                    break;

                case E_NPC_FLAG_STATUS.STATUS_FORCE_HIDE_BUT_UNACTIVE:
                    {
                        headFlagStatus = E_NPC_FLAG_STATUS.STATUS_UNFORCE_HIDE_BUT_UNACTIVE;
                    }
                    break;

                case E_NPC_FLAG_STATUS.STATUS_UNFORCE_HIDE_BUT_ACTIVE:
                case E_NPC_FLAG_STATUS.STATUS_UNFORCE_HIDE_BUT_UNACTIVE:
                    {

                    }
                    break;

                default:
                    LogModule.ErrorLog("E_NPC_FLAG_STATUS error  2:" + headFlagStatus + " " + m_RoleName);
                    headFlagStatus = E_NPC_FLAG_STATUS.STATUS_UNKNOW;
                    break;
            }
        }

        private GameObject offscrTipsGo;

        public void SetOffScrTipsActive(bool active)
        {
            if (active)
            {
                if (offscrTipsGo == null)
                {
                    GameObject go = new GameObject("offscrTips");
                    go.transform.SetParent(transform);
                    go.AddComponent<MeshRenderer>();
                    go.AddComponent<OffScrListener>();
                    offscrTipsGo = go;
                }
                if (HeadNameborad != null)
                {
                    float h = HeadNameborad.GetFinalPosRefix();
                    Vector3 objPos = ObjTransform.position;
                    offscrTipsGo.transform.position = new Vector3(objPos.x, objPos.y + h, objPos.z);
                }
            }

            if (offscrTipsGo != null && offscrTipsGo.activeSelf != active)
            {
                offscrTipsGo.SetActive(active);
            }
        }
        #endregion

        #region 特殊交互

        public NPCMiscBase AddNpcMisc(int miscid)
        {
            Tab_NPCMisc misc = TableManager.GetNPCMiscByID(miscid, 0);
            if (misc == null)
                return null;

            return AddNpcMisc(misc);
        }

        public NPCMiscBase AddNpcMisc(Tab_NPCMisc tab)
        {
            if (tab == null) return null;

            var type = (NpcMiscLogicType)tab.LogicType;
            NPCMiscBase misc = null;
            switch (type)
            {
                case NpcMiscLogicType.NML_ClickChange:
                    misc = Utils.TryAddComponent<NPCMiscClickAndChange>(gameObject);
                    break;
                case NpcMiscLogicType.NML_GeneralRule:
                    misc = Utils.TryAddComponent<NPCMiscGeneralRule>(gameObject);
                    break;
                case NpcMiscLogicType.NML_StoryFlag:
                    misc = Utils.TryAddComponent<NPCMiscStoryFlag>(gameObject);
                    break;
                case NpcMiscLogicType.NML_LineUp:
                    misc = Utils.TryAddComponent<NPCMiscLineUp>(gameObject);
                    break;
                case NpcMiscLogicType.NML_ClickBattle:
                    misc = Utils.TryAddComponent<NPCMiscClickAndBattle>(gameObject);
                    break;
                case NpcMiscLogicType.NML_ClickMove:
                    misc = Utils.TryAddComponent<NPCMiscClickAndMove>(gameObject);
                    break;
                case NpcMiscLogicType.NML_ClickCreate:
                    misc = Utils.TryAddComponent<NPCMiscClickAndCreate>(gameObject);
                    break;
                case NpcMiscLogicType.NML_ClickNewLineUp:
                    misc = Utils.TryAddComponent<NPCMiscClickAndNewLineUp>(gameObject);
                    break;
                case NpcMiscLogicType.NML_SceneWatting:
                    misc = Utils.TryAddComponent<NPCMiscWaiting>(gameObject);
                    break;
                case NpcMiscLogicType.NML_DialogAndYelling:
                    misc = Utils.TryAddComponent<NpcMiscDialogAndYelling>(gameObject);
                    break;
                case NpcMiscLogicType.NML_DialogAndDisappear:
                    misc = Utils.TryAddComponent<NPCMiscDialogAndDisappear>(gameObject);
                    break;
                case NpcMiscLogicType.NML_DialogAndDisappear2:
                    misc = Utils.TryAddComponent<NPCMiscDialogAndDisappear2>(gameObject);
                    break;
                case NpcMiscLogicType.NML_TriggerBindNpc:
                    //BoxCollider collider = gameObject.GetComponentInChildren<BoxCollider>();
                    //if (collider != null)
                    //{
                    //    misc = Utils.TryAddComponent<NPCMiscTriggerBindNpc>(collider.gameObject);
                    //}
                    misc = Utils.TryAddComponent<NPCMiscTriggerBindNpc>(gameObject);
                    break;
                case NpcMiscLogicType.NML_DialogByFuncUnlock:
                    misc = Utils.TryAddComponent<NPCMiscDialogByFuncUnlock>(gameObject);
                    break;
                case NpcMiscLogicType.NML_GatherPoint:
                    misc = Utils.TryAddComponent<NPCMiscGatherPoint>(gameObject);
                    break;
                default:
                    break;
            }

            if (misc != null)
            {
                misc.Setup(tab);
            }
            return misc;
        }

        private IEnumerator _SendMiscPack2Server(int miscId, float delay)
        {
            yield return new WaitForSeconds(4.5f);
            NPCMiscHandler.SendTriggerMisc(miscId, (uint)SceneNPCID);
        }

        public void SendMiscPack2Server(int miscId, float delay)
        {
            StartCoroutine(_SendMiscPack2Server(miscId, delay));
        }

        #endregion

        public bool IsGatherVigour()
        {
            Tab_SceneNpc tSceneNpc = TableManager.GetSceneNpcByID(m_nSceneNPCID, 0);
            if (tSceneNpc == null)
            {
                return false;
            }

            //Tab_NpcDialog tDialog = TableManager.GetNpcDialogByID(tSceneNpc.DialogID, 0);
            //if (tDialog == null)
            //{
            //    return false;
            //}

            //return tDialog.GetOptionTypebyIndex(0) == (int)DIALOG_TYPE.GATHER_VIGOUR;
            Tab_NPCMisc tNPCMisc = TableManager.GetNPCMiscByID(tSceneNpc.MiscID, 0);
            if (tNPCMisc == null)
            {
                return false;
            }
            return tNPCMisc.LogicType == (int)NpcMiscLogicType.NML_GatherPoint;
        }

        public bool IsStoryEnterNpc()
        {
            Tab_SceneNpc tSceneNpc = TableManager.GetSceneNpcByID(m_nSceneNPCID, 0);
            if (tSceneNpc == null)
            {
                return false;
            }

            Tab_NpcDialog tDialog = TableManager.GetNpcDialogByID(tSceneNpc.DialogID, 0);
            if (tDialog == null)
            {
                return false;
            }

            return tDialog.GetOptionTypebyIndex(0) == (int)DIALOG_TYPE.ENTERSTORY;
        }
    }
}