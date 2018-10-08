/********************************************************************************
 *	文件名：	StoryManager.cs
 *	全路径：	\Script\Player\Story\StoryManager.cs
 *	创建人：	李嘉
 *	创建时间：2017-01-12
 *
 *	功能说明：游戏主线任务控制器，目前是纯客户端实现
 *           管理器保存着当前剧情进行中的三大基本要素：场景，环境，事件
 *	修改记录：
*********************************************************************************/

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Games.GlobeDefine;
using Games.Table;
using Games.LogicObj;
using ProtobufPacket;
using Games;

public class StoryModePrecond
{
    public StoryModePrecond()
    {
        Reset();
    }

    public void Reset()
    {
        FirstLoadingFinished = false;
        EnterMode = ENTER_STORY_MODE_SOURCE.NORMAL;
        LastFinishedStoryLine = GlobeVar.INVALID_ID;
        SingleStoryId = GlobeVar.INVALID_ID;
    }

    public bool FirstLoadingFinished;   // 第一次进入故事模式时, 即使场景资源相同也强制加载

    // 进入故事模式的来源(场景, 回忆, 卡牌, gm, etc.)
    private ENTER_STORY_MODE_SOURCE mEnterMode;
    public ENTER_STORY_MODE_SOURCE EnterMode
    {
        get
        {
            return mEnterMode;
        }
        set
        {
            mEnterMode = value;
        }
    }  

    public int LastFinishedStoryLine;   // 从回忆录进入故事模式时, 所在的章节
    public int SingleStoryId;        // 回忆界面触发只播放一段tv, 可以拓展成只播放任何一个story, -1则为不生效
}

public class StoryManager : MonoBehaviour
{
    private StoryModePrecond m_Precondition = new StoryModePrecond();
    public StoryModePrecond Precondition
    {
        get { return m_Precondition; }
        //set { m_Precondition = value; }
    }

    private Tab_Story m_CurStoryTable = null;
    public Tab_Story CurStoryTable
    {
        get { return m_CurStoryTable; }
        //set { m_CurStoryTable = value; }
    }

    private Story m_CurStory = null;       //NPC当前的剧情
    public Story CurStory
    {
        get { return m_CurStory; }
        set { m_CurStory = value; }
    }
    private Dictionary<int, Obj_NPC> m_CurNPC = new Dictionary<int, Obj_NPC>();       //当前这一步的所有NPC,Key为SceneNPC的ID，Value为创建的Obj_NPC实体
    private Dictionary<int, Obj_NPC> m_LastStoryNPC = new Dictionary<int, Obj_NPC>();   //上一步的所有NPC

    public bool AllStoryFinish { get; set; }                //剧情全部结束标记位
    public int StoryModeScene { get; set; }             //当前进入剧情模式的场景ID
    private bool m_bStoryMode = false;                  //剧情模式
    public bool StoryMode
    {
        get
        {
            return m_bStoryMode;
        }
        set
        {
            //Debug.LogErrorFormat("switch story mode: {0}", value);
            m_bStoryMode = value;
        }
    }

    public GC_ENTER_STORY m_cachePacket = null;

    private bool mReloadMainPlayer = false;
    public bool ReloadMainPlayer { get; set; }

    //记录上一步ID和故事线，如果当前步骤失败，需要进行失败出来，对当前步骤回滚
    //目前暂时只有战斗可能涉及失败回滚
    //按照目前的机制，此数据无需存储，因为重新退出再进入当前剧情之后
    //会按照服务器记录的步骤重新初始化
    private int m_nLastStep = GlobeVar.INVALID_ID;
    public int LastStep
    {
        get { return m_nLastStep; }
        set { m_nLastStep = value; }
    }
    private int m_nLastStoryLine = GlobeVar.INVALID_ID;
    public int LastStoryLine
    {
        get { return m_nLastStoryLine; }
        set { m_nLastStoryLine = value; }
    }

    #region 剧情模式
    // firstLoadingFinished一般不设置, 只有新手第一次剧情的时候特殊处理, 防止登录时连续两次加载
    public void SetPrecondition(ENTER_STORY_MODE_SOURCE enterMode, int singleStoryId = GlobeVar.INVALID_ID, bool firstLoadingFinished = false)
    {
        Precondition.EnterMode = enterMode;
        Precondition.FirstLoadingFinished = firstLoadingFinished;
        Precondition.SingleStoryId = singleStoryId;
    }

    public void ClearPrecondition()
    {
        Precondition.Reset();
    }

    //进入剧情模式，bCheck表示无需检测直接进入，一般为true，只有在回退的时候使用false
    public void EnterStoryMode(bool bCheckID, bool bForceLoadScene)
    { 
        if (SkipMode)
        {
            m_CurStory.Interupted();
            m_bStoryStarted = false;
            //SkipMode = false;
        }

        if (null != ConvenientNoticeController.Instance)
        {
            ConvenientNoticeController.Close();
        }

        //进入剧情时，停止变身
        GameManager.PlayerDataPool.CheckAndStopRoleMask();

        //gm命令直接跳到某段剧情时不会走设置进入模式的流程, 在这里区分一下
        if (Precondition.EnterMode == ENTER_STORY_MODE_SOURCE.INVALID)
        {
            Precondition.EnterMode = ENTER_STORY_MODE_SOURCE.GM;
        }

        StoryMode = true;

        GameManager.PlayerDataPool.StoryState = (int)STORY_STATE.NORMAL;
        InitStory(GameManager.PlayerDataPool.m_nStroyID, GameManager.PlayerDataPool.StoryState, bCheckID, bForceLoadScene);
       
        //显示剧情模式专有UI
        if (null == StoryModeController.Instance())
        {
            UIManager.ShowUI(UIInfo.StoryStyle);
        }

        //隐藏MainUI
        if (null != MainUI.Ins)
        {
            MainUI.Ins.ShowMainMenu(false);
        }

        if (null != PlayerFrameController.Instance())
        {
            PlayerFrameController.Instance().ShowPlayerFrame(false);
        }

        if (QuestBallWindow.Instance != null)
        {
            QuestBallWindow.Instance.UpdateShow(false);
        }
        if (GameManager.PlayerDataPool.m_isHasRoleInOtherWorld && GameManager.m_bOffLineMode&& GameManager.PlayerDataPool.m_nStroyID==8510)
        {
            MessageBoxController.OnOKClick okAction = () =>
            {
                if (GameManager.PlayerDataPool != null && GameManager.storyManager != null)
                {
                    GameManager.PlayerDataPool.m_nStroyID = Games.GlobeDefine.GlobeVar.INVALID_ID;
                    GameManager.PlayerDataPool.m_nStoryLine = Games.GlobeDefine.GlobeVar.INVALID_ID;
                    GameManager.storyManager.LeaveStoryMode(Games.GlobeDefine.GlobeVar.INVALID_ID);
                }
            };
            MessageBoxController.OnCancelClick cancelAction = () => 
            {
                if (JoyStickController.Instance() != null)
                {
                    JoyStickController.Instance().UpdateTutorialMoveRot();
                }
            };

            MessageBoxController.OpenOKCancel(StrDictionary.GetDicByID(8262), StrDictionary.GetDicByID(8263), okAction, cancelAction);
        }
    }

    private IEnumerator StartLeaveStoryMode(int nLastSceneID)
    {
        //Debug.LogErrorFormat("StartLeaveStoryMode 1");
        while (true)
        {
            if (TutorialQuestCompleteWindow.ExistShowInfo)
            {
                yield return new WaitForFixedUpdate();
            }
            else
            {
                break;
            }
        }

        //Debug.LogErrorFormat("StartLeaveStoryMode 2");

        //LogModule.DebugLog("leave story mode: " + Precondition.EnterMode);
        if (null == m_CurStoryTable)
        {
            //Debug.LogErrorFormat("StartLeaveStoryMode abort");
            yield break;
        }

        if (CurStory != null)
        {
            CurStory.Interupted();
        }

        int nStoryLineId = m_CurStoryTable.LindID;
        if (Precondition.EnterMode == ENTER_STORY_MODE_SOURCE.STORY_MEMORY
            || Precondition.EnterMode == ENTER_STORY_MODE_SOURCE.STORY_MEMORY_MV)
        {
            Precondition.LastFinishedStoryLine = nStoryLineId;
        }

        ClearStoryData(Precondition.EnterMode == ENTER_STORY_MODE_SOURCE.NORMAL || Precondition.EnterMode == ENTER_STORY_MODE_SOURCE.GM);

        //Debug.LogErrorFormat("StartLeaveStoryMode 3");

        if (GameManager.m_bOffLineMode)
        {
            if (BeforeLoginSceneLogic.IsStoryBeforeLogin)
            {
                BeforeLoginSceneLogic.FinishStoryBeforeLogin();
            }
            else
            {
                GameManager.EnterMainLobby();
            }

        }
        else
        {
            if (null != ConvenientNoticeController.Instance)
            {
                ConvenientNoticeController.Show();
            }

            if (nStoryLineId == GlobeVar.TutorialStory_StoryLineId && GameManager.PlayerDataPool.GetStoryFin(nStoryLineId) == 1 &&
                false == TutorialManager.IsTutorialComplete(TutorialGroup.DrawCard_1, 1))
            {
                //完成此剧情后 触发抽卡引导
                MainUI.m_TutorialGroupOnMenuBtn = TutorialGroup.DrawCard_1;
                MainUI.m_TutorialStepOnMenuBtn = 1;
                GameManager.ReqChangeScene(nLastSceneID);
                //Debug.LogErrorFormat("StartLeaveStoryMode ChangeScene 1");
            }
            else if (nStoryLineId == GlobeVar.TutorialLeaveStory_StoryLineId && GameManager.PlayerDataPool.GetStoryFin(nStoryLineId) == 1 &&
                false == TutorialManager.IsTutorialComplete(TutorialGroup.LeaveStory, 1))
            {
                // 退出剧情模式教学
                if (StoryModeController.Instance() != null)
                {
                    StoryModeController.Instance().UpdateTutorialOnLeaveStory(TutorialGroup.LeaveStory, 1, nLastSceneID);
                }

                if (false == TutorialManager.IsOpenTutorial)
                {
                    GameManager.ReqChangeScene(nLastSceneID);
                    //Debug.LogErrorFormat("StartLeaveStoryMode ChangeScene 2");
                }
            }
            else if (nStoryLineId == GlobeVar.TutorialFilterChoose_StoryLineID)
            {
                var tabTel = TableManager.GetTeleportPointByID(GlobeVar.TutorialMainQuest_StoryLineTeleport, 0);
                Tab_StoryLine tabLine = TableManager.GetStoryLineByID(nStoryLineId, 0);
                if (tabLine!= null && GameManager.PlayerDataPool.GetStoryFin(GlobeVar.TutorialFilterChoose_StoryLineID) == 1 &&
                    (GameManager.PlayerDataPool.GetStoryStep(GlobeVar.TutorialFilterChoose_StoryLineID) == GlobeVar.INVALID_ID ||
                    GameManager.PlayerDataPool.GetStoryStep(GlobeVar.TutorialFilterChoose_StoryLineID) == tabLine.BeginID) &&
                    tabTel != null)
                {
                    GameManager.PlayerDataPool.ForceRefixRTPos(tabTel.PosX, tabTel.PosZ,
                        Utils.DirClientToServer(GlobeVar._GameConfig.TutorialFinishFaceDir));
                    GameManager.ReqTeleportScene(GlobeVar.TutorialMainQuest_StoryLineTeleport);
                }
                else
                {
                    GameManager.ReqChangeScene(nLastSceneID);
                    //Debug.LogErrorFormat("StartLeaveStoryMode ChangeScene 3");
                }
            }
            else if (MapScene.s_waitOpenMap)
            {
                GameManager.ReqChangeScene((int)SCENE_DEFINE.SCENE_MAP);
            }
            else
            {
                //离开的同时需要清理UI数据
                GameManager.ReqChangeScene(nLastSceneID);
                //Debug.LogErrorFormat("StartLeaveStoryMode ChangeScene 4");
            }
        }
    }

    public void LeaveStoryMode(int nLastSceneID)
    {
        StartCoroutine(StartLeaveStoryMode(nLastSceneID));
    }

    public void ClearStoryData(bool clearPrecond)
    {
        if (clearPrecond)
        {
            ClearPrecondition();
        }

        StoryMode = false;
        m_CurStoryTable = null;
        m_CurStory = null;
        SkipMode = false;

        //恢复移动
        if (null != ObjManager.MainPlayer)
        {
            ObjManager.MainPlayer.EventMove = true;
        }

        GameManager.CameraManager.IsDragLock = false;
    }

    public int GetCurBranchCnt()
    {
        return GetBranchCnt(m_CurStoryTable);
    }
    #endregion

    #region 初始化部分
    void InitStoryScene(bool forceLoadScene)
    {
        if (null == m_CurStoryTable)
        {
            return;
        }

        //根据内容创建玩家和NPC
        //如果发现需要切换场景，这里只做场景切换的操作
        if (GameManager.RunningScene != m_CurStoryTable.SceneID)
        {
            //清理缓存NPC
            m_CurNPC.Clear();

            //记录当前剧情场景的ID
            StoryModeScene = m_CurStoryTable.SceneID;

            //切场景
            GameManager.EnterGameScene(m_CurStoryTable.SceneID, forceLoadScene);
        }
    }

    private bool m_StoryLoadScene = false;
    private int m_StoryLoadSceneState = -1;
    public void OnEnterScene()
    {
        if (StoryMode && m_StoryLoadScene)
        {
            _InitStory(m_StoryLoadSceneState, true);
            m_StoryLoadScene = false;
        }
    }

    //初始化环境
    void InitEnv(bool switchImm = false)
    {
        if (null == GameManager.EnvManager ||
            null == m_CurStoryTable)
        {
            return;
        }

        //看之前步骤和当前步骤的场景环境是否一样
        if (null == GameManager.EnvManager.CurEvnTable || m_CurStoryTable.Enviroment != GameManager.EnvManager.CurEvnTable.Id)
        {
            if (GlobeVar.INVALID_ID == m_CurStoryTable.Enviroment)
            {
                GameManager.EnvManager.SetDefault();
            }
            else
            {
                bool imm = switchImm || m_CurStoryTable.EnviromentSwitchMode == 1;
                GameManager.EnvManager.Switch(m_CurStoryTable.Enviroment, imm);
            }
        }
    }

    void InitStoryMusic()
    {
        if (null == m_CurStoryTable)
        {
            return;
        }

        if (GlobeVar.INVALID_ID != m_CurStoryTable.SoundID)
        {
            Tab_Sounds _storySound = TableManager.GetSoundsByID(m_CurStoryTable.SoundID, 0);
            if (null != _storySound)
            {
                GameManager.SoundManager.PlayBGMusic(m_CurStoryTable.SoundID, _storySound.FadeOutTime, _storySound.FadeInTime);
            }
        }
    }

    void InitStoryNPC()
    {
        if (null == m_CurStoryTable)
        {
            return;
        }

        //缓存之前的NPC，当创建完成后，之前的NPC需要销毁
        m_LastStoryNPC.Clear();
        //string lastNpcStr = "last story npc: ";
        foreach (KeyValuePair<int, Obj_NPC> _pair in m_CurNPC)
        {
            //lastNpcStr += _pair.Key + ", ";
            m_LastStoryNPC[_pair.Key] = _pair.Value;
        }
        //Debug.LogError(lastNpcStr);
        
        //将新的清空
        m_CurNPC.Clear();

        //场景一致，进行NPC的加载
        int nStoryNPCCount = m_CurStoryTable.getStroyNPCCount();
        for (int i = 0; i < nStoryNPCCount; ++i)
        {
            int nStoryNpcID = m_CurStoryTable.GetStroyNPCbyIndex(i);
            if (nStoryNpcID >= 0)
            {
                Tab_StoryNpc _tabSceneNpc = TableManager.GetStoryNpcByID(nStoryNpcID, 0);
                if (null == _tabSceneNpc)
                {
                    LogModule.ErrorLog("npc " + nStoryNpcID + " tab data not found");
                    break;
                }
                Obj_NPC npc = InitNPC(_tabSceneNpc, m_CurStoryTable.GetStroyNPCHeadFlagbyIndex(i));
            }
        }

        //加载NPC组
        if (m_CurStoryTable.NPCGroup != GlobeVar.INVALID_ID)
        {
            Tab_NPCMisc groupmisc = null;
            LineupData lineupData = null;
           // bool first = true;
            Tab_NPCGroup _tabGroup = TableManager.GetNPCGroupByID(m_CurStoryTable.NPCGroup, 0);
            if (null != _tabGroup)
            {
                int nGroupCount = _tabGroup.getNPCIDCount();
                for (int i = 0; i < nGroupCount; ++i)
                {
                    int nStoryNpcID = _tabGroup.GetNPCIDbyIndex(i);
                    if (nStoryNpcID >= 0)
                    {
                        Tab_StoryNpc _tabStoryNpc = TableManager.GetStoryNpcByID(nStoryNpcID, 0);
                        if (null == _tabStoryNpc)
                        {
                            LogModule.ErrorLog("npc " + nStoryNpcID + " tab data not found");
                            break;
                        }

                        //// 取全组第一个npc的misc作为组id
                        //if (first && groupmisc == null && _tabSceneNpc.MiscID != GlobeVar.INVALID_ID)
                        //{
                        //    Tab_NPCMisc misc = TableManager.GetNPCMiscByID(_tabSceneNpc.MiscID, 0);
                        //    if (misc != null && misc.LogicType == (int)NpcMiscLogicType.NML_LineUp)
                        //    {
                        //        // 在剧情npc组中, 只有lineup奇遇会生效
                        //        groupmisc = misc;
                        //        lineupData = new LineupData();
                        //        lineupData.Setup(groupmisc);
                        //    }
                        //}
                        //first = false;

                        Obj_NPC npc = InitNPC(_tabStoryNpc, GlobeVar.INVALID_ID);
                        if (npc != null && groupmisc != null)
                        {
                            NPCMiscLineUp lineupMisc = npc.AddNpcMisc(groupmisc) as NPCMiscLineUp;
                            if (lineupMisc != null)
                            {
                                lineupMisc.lineupData = lineupData;
                                lineupMisc.index = i;
                            }
                        }
                    }
                }
            }
        }

        //创建完毕，把不需要的NPC移除
        foreach (int _key in m_LastStoryNPC.Keys)
        {
            if (m_CurNPC.ContainsKey(_key) == false)
            {
                //移除掉这个NPC
                if (ObjManager.IsObjExist(m_LastStoryNPC[_key]))
                {
                    ObjManager.RemoveObj(m_LastStoryNPC[_key]);
                }
            }
        }

        //再次清空
        m_LastStoryNPC.Clear();
    }

    // default position: 如果之前的Story有移动的话, 按最后一个的终点算, 否则取SceneNpc里的默认位置
    // ps. 只有在新创建的时候才会生效, 避免刷新位置把之前Story移动过的npc拉回的情况
    // default rotation(涉及太多暂时没做): 如果上述移动成立, 则取倒数第二次移动->最后一次移动的终点为朝向
    //                   若只存在一次移动, 则为初始位置->最后一次移动的终点为朝向
    static public void GetStoryNPCTrans(int sceneNpcID, Tab_Story tabCurStory, ref Vector3 position)
    {
        if (tabCurStory == null)
            return;

        Tab_StoryLine tabStoryLine = TableManager.GetStoryLineByID(tabCurStory.LindID, 0);
        if (tabStoryLine == null || tabCurStory.Id == tabStoryLine.BeginID)
            return;

        Tab_Story tabRootStory = TableManager.GetStoryByID(tabStoryLine.BeginID, 0);
        if (tabRootStory == null)
            return;


        bool hitPos = false;
        //bool hitRot = false;

        List<Tab_Story> route = new List<Tab_Story>();
        if (FindStoryRoute(tabRootStory, tabCurStory, ref route))
        {
            // 逆向遍历route, 找到最后一个同场景, 移动event, 同npc
            route.Reverse();
            foreach (Tab_Story story in route)
            {
                if (story.SceneID != tabCurStory.SceneID && IsNpcInStory(story, sceneNpcID))
                    return;

                for (int i = 0; i < story.getEventIDCount(); ++i)
                {
                    int eventID = story.GetEventIDbyIndex(i);
                    if (eventID == GlobeVar.INVALID_ID)
                        continue;

                    Tab_StoryEvent tabEvent = TableManager.GetStoryEventByID(eventID, 0);
                    if (tabEvent == null)
                        continue;

                    if (tabEvent.GetINTPARAMbyIndex(0) == sceneNpcID)
                    {
                        if (!hitPos && (tabEvent.Type == (int)STORY_EVENT.ACTION_EX || tabEvent.Type == (int)STORY_EVENT.OBJMOVE))
                        {
                            float x = tabEvent.GetFLOATPARAMbyIndex(0);
                            float z = tabEvent.GetFLOATPARAMbyIndex(1);
                            if (x > 0f && z > 0f)
                            {
                                position.x = x;
                                position.z = z;

                                hitPos = true;
                                //Debug.LogFormat("!!!!last story pos found ({0}, {1}), from event id {2}", x, z, tabEvent.Id);
                                return;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            //LogModule.DebugLog("story routine not found" + tabCurStory.Id);
        }
    }

    static public bool IsNpcInStory(Tab_Story story, int npc)
    {
        for (int i = 0; i < story.getStroyNPCCount(); ++i)
        {
            if (story.GetStroyNPCbyIndex(i) == npc)
                return true;
        }
        return false;
    }

    // 获得从某storyline到当前story所执行的所有story(不包括当前节点)
    static public bool FindStoryRoute(Tab_Story story, Tab_Story targetStory, ref List<Tab_Story> route)
    {
        route.Add(story);

        for (int i = 0; i < story.getBranchCount(); ++i)
        {
            int branch = story.GetBranchbyIndex(i);
            if (branch == GlobeVar.INVALID_ID)
            {
                continue;
            }

            if (branch == targetStory.Id)
            {
                return true;
            }

            Tab_Story nextStory = TableManager.GetStoryByID(branch, 0);
            if (nextStory != null && nextStory.LindID == targetStory.LindID &&
                FindStoryRoute(nextStory, targetStory, ref route))
            {
                return true;
            }
        }

        route.Remove(story);
        return false;
    }

    //创建剧情NPC专用函数
    private Obj_NPC InitNPC(Tab_StoryNpc _tabStoryNpc, int npcHeadFlagIndex)
    {
        if (null == _tabStoryNpc)
        {
            return null;
        }

        //判断是否重复
        if (m_LastStoryNPC.ContainsKey(_tabStoryNpc.Id))
        {
            //有了，直接插入
            Obj_NPC npc = m_LastStoryNPC[_tabStoryNpc.Id];
            if (null != npc)
            {
                npc.NpcHeadFlagIndex = npcHeadFlagIndex;
                if (npcHeadFlagIndex != GlobeVar.INVALID_ID && npc.HeadInfo == null)
                {
                    //如果上一个剧情中的npc没有创建名字版 并且当前剧情需要名字版则创建
                    npc.InitNameBoard();
                }

                //如果断线重连，修正位置
                if (!Precondition.FirstLoadingFinished)
                {
                    Vector3 resetPos = new Vector3(_tabStoryNpc.PosX, 0.0f, _tabStoryNpc.PosZ);
                    GetStoryNPCTrans(_tabStoryNpc.Id, m_CurStoryTable, ref resetPos);
                    ObjManager.ForceSetObjPos(npc, resetPos.x, resetPos.z);
                }

                m_CurNPC.Add(_tabStoryNpc.Id, npc);
            }

            return npc;
        }

            // 默认scene npc表中位置
        Vector3 defaultPos = new Vector3(_tabStoryNpc.PosX, 0.0f, _tabStoryNpc.PosZ);
        Vector3 defaultRot = new Vector3(0.0f, _tabStoryNpc.FaceDirection, 0.0f);

            //// 如果有奇遇, 则使用奇遇的位置
            //Tab_NPCMisc tabMisc = null;
            //if (_tabSceneNpc.MiscID > GlobeVar.INVALID_ID)
            //{
            //    tabMisc = TableManager.GetNPCMiscByID(_tabSceneNpc.MiscID, 0);
            //    if (tabMisc != null && tabMisc.LogicType != (int)ProtobufPacket.NpcMiscLogicType.NML_LineUp)
            //    {
            //        NPCMiscBase.RandPos(tabMisc, ref defaultPos);
            //    }
            //}

            //还原之前剧情的位置
            if (!Precondition.FirstLoadingFinished)
            {
                GetStoryNPCTrans(_tabStoryNpc.Id, m_CurStoryTable, ref defaultPos);
            }

            defaultPos.y = _tabStoryNpc.PosY;

            //创建NPC
            Obj_Init_Data _cardInfo = new Obj_Init_Data();
            _cardInfo.m_CreatePos = defaultPos;
            _cardInfo.m_CreateRot = defaultRot;
            _cardInfo.m_nModelID = Utils.GetRealCharModel(_tabStoryNpc.CharModelId);
            if (npcHeadFlagIndex != GlobeVar.INVALID_ID)
            {
              _cardInfo.m_bShowNameBoard = true;
            }
            _cardInfo.m_nDyeColorId = Utils.GetRealDyeColorId(_tabStoryNpc.DyeColorID);
            Obj_NPC _missionNPC = ObjManager.CreateNPC(_cardInfo, Utils.GetRealName(_tabStoryNpc.CharModelId));
            if (null != _missionNPC)
            {
                //将NPC插入到当前节点中
                m_CurNPC.Add(_tabStoryNpc.Id, _missionNPC);

                //设置SceneNPC的ID
                _missionNPC.SceneNPCID = _tabStoryNpc.Id;
                _missionNPC.DialogID = GlobeVar.INVALID_ID;

                //根据表格配置的不同状态确定NPC是否可以被选中和是否跟随选中者
                if ((int)NPC_SECELT_TYPE.UNFACETO == _tabStoryNpc.CanBeSelect)
                {
                    _missionNPC.CanBeSelect = true;
                    //_missionNPC.FaceToSelecter = false;
                }
                else if ((int)NPC_SECELT_TYPE.UNSELECTED == _tabStoryNpc.CanBeSelect)
                {
                    _missionNPC.CanBeSelect = false;
                    //_missionNPC.FaceToSelecter = false;
                }
                else
                {
                    _missionNPC.CanBeSelect = true;
                    //_missionNPC.FaceToSelecter = true;
                }
                _missionNPC.FocusMaxDistance = GlobeVar.INVALID_ID;
                _missionNPC.FaceToSelecter = false;

                _missionNPC.NpcHeadFlagIndex = npcHeadFlagIndex;

                //挂特效
                if (_tabStoryNpc.EffectID != GlobeVar.INVALID_ID)
                {
                    _missionNPC.PlayEffect(_tabStoryNpc.EffectID);
                }

                if (_tabStoryNpc.PosY > 0.01f)
                {
                    _missionNPC.AutoCreateNavAgent = false;
                }
                if(_tabStoryNpc.Animtion != GlobeVar.INVALID_ID)
                {
                    _missionNPC.animationId = _tabStoryNpc.Animtion;
                }
            }
            return _missionNPC;
    }

    public void InitStoryEvent()
    {
        if (null == m_CurStoryTable)
        {
            return;
        }

        m_CurStory = new Story(m_CurStoryTable);
    }

    public void InitStory(int nStoryID, int nState, bool bCheckID, bool forceLoadScene)
    {
        //如果发现之前已经初始化过了，不会再次初始化
        //已经初始化过的可能性是进行了一次且场景操作
        if (bCheckID && null != m_CurStoryTable && m_CurStoryTable.Id == nStoryID)
        {
            //Debug.LogErrorFormat("duplicate story initialzation: story {0}", nStoryID);
            return;
        }

        //找到当前对应表格
        m_CurStoryTable = TableManager.GetStoryByID(nStoryID, 0);
        //Debug.LogErrorFormat("update story table: story {0}", nStoryID);
        if (null == m_CurStoryTable)
        {
            OnStroyAllFinished();
            return;
        }

        //如果发现主线任务和当前场景不一致，则切场景
        //否则加载NPC
        if (m_CurStoryTable.SceneID != (int)GameManager.RunningScene)
        {
            //Debug.LogErrorFormat("init story {0}, change scene", nStoryID);

            InitStoryScene(forceLoadScene);
            //如果资源不变，则不清理环境
            Tab_SceneClass oldScene = TableManager.GetSceneClassByID((int)GameManager.RunningScene, 0);
            Tab_SceneClass newScene = TableManager.GetSceneClassByID(m_CurStoryTable.SceneID, 0);
            if (null != oldScene && null != newScene)
            {
                //判断资源是否完全一致
                if (oldScene.SceneResource != newScene.SceneResource)
                {
                    GameManager.EnvManager.ClearEnv();
                }
            }

            m_StoryLoadScene = true;
            m_StoryLoadSceneState = nState;
            return;
        }
        else
        {
            _InitStory(nState, false);
        }
    }

    void _InitStory(int nState, bool switchImm)
    {
        if (GameManager.EnvManager.IsSwitching())
        {
            GameManager.EnvManager.FinishSwitching();
        }

        Precondition.FirstLoadingFinished = true;   //提前设置

        InitEnv(switchImm);
        InitStoryMusic();
        InitCamera();

        if (nState == (int)STORY_STATE.NORMAL)
        {
            InitStoryNPC();
            InitStoryEvent();
            InitPlayer();
            
            //开启通用接口
            OnStoryStart();
        }
        else if (nState == (int)STORY_STATE.BRANCH)
        {
            UIManager.ShowUI(UIInfo.StoryBranch);
        }

        //Precondition.FirstLoadingFinished = true;
    }

    void InitCamera()
    {
        if (null == m_CurStoryTable)
        {
            return;
        }

        if (GlobeVar.INVALID_ID != m_CurStoryTable.CameraCfgID)
        {
            if (GameManager.CameraManager != null)
            {
                GameManager.CameraManager.InitParam(m_CurStoryTable.CameraCfgID);
                GameManager.CameraManager.InitCameraController();
            }
        }
    }

    void InitPlayer()
    {
        if (null == CurStoryTable)
        {
            return;
        }

        //根据剧情需求对主角换装
        if (null != ObjManager.MainPlayer)
        {
            if (CurStoryTable.StoryModel == GlobeVar.INVALID_ID)
            {
                ObjManager.MainPlayer.SetVisible(ObjVisibleLayer.Story, false);
                //ObjManager.RefixMainPlayerStoryPosition();
            }
            else
            {
                // 如果模型不同, 显示放在加载完成之后, 不然会出现一闪而过的不正确的主角模型
                bool showMainPlayer = true;
                //如果是主角ID
                if (CurStoryTable.StoryModel == GlobeVar.MAIN_PLAYER_SP_ID)
                {
                    //判断之前是否也是主角的模型
                    if (ObjManager.MainPlayer.ModelID != ObjManager.MainPlayer.OrgAvatarInfo.m_BodyId)
                    {
                        showMainPlayer = false;
                        ObjManager.MainPlayer.RevertAvatar(null);
                    }
                }
                //如果该步骤为非主角ID
                else
                {
                    //如果有特殊定制模型ID，判断和目前是否一样
                    //首先判断是不是几个固定主角ID
                    int nHeroID = Utils.GetSpecialHeroID(CurStoryTable.StoryModel);
                    if (GlobeVar.INVALID_ID != nHeroID)
                    {
                        if (null != GameManager.PlayerDataPool.PlayerHeroData)
                        {
                            Hero hero = GameManager.PlayerDataPool.PlayerHeroData.GetHero(nHeroID);
                            if (hero == null)
                            {
                                // 尝试获取默认皮肤
                                var tabHero = TableManager.GetHeroByID(nHeroID, 0);
                                if (tabHero == null)
                                    return;

                                AvatarLoadInfo _info = new AvatarLoadInfo();
                                var tabSkin = TableManager.GetHeroSkinDataByID(tabHero.DefaultSkin, 0);
                                if (tabSkin != null)
                                {
                                    _info.m_BodyId = tabSkin.CharModelId;
                                }
                                else
                                {
                                    // 没有默认皮肤, 直接取rolebase
                                    Tab_RoleBaseAttr tabRolebase = TableManager.GetRoleBaseAttrByID(tabHero.RoleBaseId, 0);
                                    if (tabRolebase == null)
                                        return;
                                    _info.m_BodyId = tabRolebase.CharModelID;
                                }
                                ObjManager.MainPlayer.ChangeAvatar(_info);
                            }
                            else
                            {
                                int playModelId = hero.GetCharModelId();
                                if (playModelId == GlobeVar.INVALID_ID)
                                {
                                    return;
                                }

                                //特殊指定主角
                                AvatarLoadInfo _info = new AvatarLoadInfo();
                                _info.m_BodyId = playModelId;
                                _info.LoadDyeColor(hero.DyeColorID);
                                _info.m_HangPieceDict = hero.HangPieceSlot.HangPieceDic;
                                ObjManager.MainPlayer.ChangeAvatar(_info);
                            }
                        }
                    }
                    else if (ObjManager.MainPlayer.ModelID != CurStoryTable.StoryModel || ObjManager.MainPlayer.CurrAvatarInfo.m_HasDyeColor)
                    {
                        //showMainPlayer = false; reloadavatar目前是同步加载, 这里不用隐藏后等回调再显示. 后续如果修改avatar加载方式, 这里需要改
                        ObjManager.MainPlayer.ChangeAvatar(CurStoryTable.StoryModel);
                    }
                }

                ObjManager.MainPlayer.SetVisible(ObjVisibleLayer.Story, showMainPlayer);
            }

            if (SkipMode)
            {
                // 主角位置会带着摄像机移动, 所以无论是否显示主角, 这里都要尝试恢复默认位置
                ObjManager.RefixMainPlayerStoryPosition();
            }
            SkipMode = false;
        }
    }

    #endregion

    #region 通用剧情结束接口
    private bool m_bStoryStarted = false;       //记录一个标记位，在未开始的时候不更新故事状态
    public void OnStoryStart()
    {
        m_bStoryStarted = true;

        UpdateMainPlayerEventMoveFlag();
    }

    public void OnStoryFailed(int returnStoryId = GlobeVar.INVALID_ID)
    {
        //故事失败，回滚到上一个步骤
        //如果没有被赋值，有可能是进入剧情模式第一步就失败了
        //所以将Last赋值
        //if (GlobeVar.INVALID_ID == m_nLastStoryLine || GlobeVar.INVALID_ID == m_nLastStep)
        {
            m_nLastStoryLine = GameManager.PlayerDataPool.m_nStoryLine;
            if(returnStoryId > 0)
            {
                m_nLastStep = returnStoryId;
            }
            else
            {
                m_nLastStep = GameManager.PlayerDataPool.m_nStroyID;
            }

        }

        //首先判断是否记录了上一步的故事线ID和步骤ID
        if (GlobeVar.INVALID_ID != m_nLastStoryLine && GlobeVar.INVALID_ID != m_nLastStep)
        {
            //之后判断上一步的故事线是否和当前故事线一致
            if (m_nLastStoryLine == GameManager.PlayerDataPool.m_nStoryLine)
            {
                //判断成功，回退上一步
                Precondition.FirstLoadingFinished = false;//强制刷一遍加载, 恢复npc上次的位置
                SkipMode = true; //用于强制重刷主角位置
                GameManager.PlayerDataPool.m_nStroyID = LastStep;
                StoryHandler.ReqEnterNextStep(m_nLastStoryLine, m_nLastStep);
                return;
            }
        }

        //走到这一步说明处理剧情失败的操作失败了，这时候暂时考虑退出剧情模式
        StoryHandler.ReqLeaveStory();
    }

    public void OnStoryFinish()
    {
        m_bStoryStarted = false;

        if (SkipMode)
            return;
        
        //当前任务结束，进入下一步
        if (null != m_CurStoryTable)
        {
            //string szFinishString = string.Format("Story Finished, Chapter{0}, Section{1}, Step{2}", m_CurStoryTable.Chapter, m_CurStoryTable.Section, m_CurStoryTable.Step);
            //LogModule.DebugLog(szFinishString);

            int nStoryLineId = m_CurStoryTable.LindID;
            int nStoryId = m_CurStoryTable.Id;

            // 如果是只播放一段回放
            if (Precondition.SingleStoryId != GlobeVar.INVALID_ID)
            {
                StoryHandler.ReqLeaveStory();
            }
            else
            {
                //这里判断是否进入分支选项
                List<int> storyBranchList = new List<int>();
                for (int i = 0; i < m_CurStoryTable.getBranchCount(); ++i)
                {
                    int nBranchID = m_CurStoryTable.GetBranchbyIndex(i);
                    if (nBranchID != GlobeVar.INVALID_ID)
                    {
                        storyBranchList.Add(nBranchID);
                    }
                }

                if (storyBranchList.Count == 1)
                {
                    StoryHandler.ReqEnterNextStep(GameManager.PlayerDataPool.m_nStoryLine, storyBranchList[0]);
                }
                else if (storyBranchList.Count == 0)
                {
                    //如果BranchList为空，说明全部剧情结束
                    GameManager.PlayerDataPool.m_nStroyID = GlobeVar.INVALID_ID;
                    OnStroyAllFinished();
                    
                    //发送INVALID，服务器进行下一步判断
                    StoryHandler.ReqEnterNextStep(GameManager.PlayerDataPool.m_nStoryLine, GlobeVar.INVALID_ID);

                    if (GameManager.m_bOffLineMode && BeforeLoginSceneLogic.IsStoryBeforeLogin)
                    {
                        BeforeLoginSceneLogic.FinishStoryBeforeLogin();
                    }
                }
                else
                {
                    //否则进入分支选择
                    UIManager.ShowUI(UIInfo.StoryBranch);
                }
            }
        }
    }

    //切换场景的时候调用
    public void OnChangeScene()
    {
        //由于NPC切场景肯定消失，所以这里清空
        //之后再根据需要重新创建
        m_CurNPC.Clear();
    }

    //有新的事件开始了
    public void OnEventStart()
    {
    }

    //事件进入触发
    public void OnEventEnter(bool refreshNpc)
    {
        if (refreshNpc)
        {
            foreach (var npc in m_CurNPC.Values)
            {
                if (null != npc)
                {
                    npc.StoryEnter();
                }
            }
        }

        //有新的事件开启，更新移动标记
        UpdateMainPlayerEventMoveFlag();
    }


    //事件离开触发
    public void OnEventLeave(STORY_EVENT eventType)
    {
        if (eventType != STORY_EVENT.FOLLOW)
        {
            foreach (var npc in m_CurNPC.Values)
            {
                if (null != npc)
                {
                    npc.StoryLeave();
                }
            }
        }

        if (eventType == STORY_EVENT.DIALOG)
        {
            //其他事件开始执行
            if (null != m_CurStory)
            {
                m_CurStory.CheckStartEventByTalkEnd();
            }
        }

        //解决摇杆闪现的问题, 摇杆统一由Story和StoryEvent开始的时候更新
        //UpdateMainPlayerEventMoveFlag();
    }

    //所有剧情全部结束
    private void OnStroyAllFinished()
    {
        AllStoryFinish = true;

        ScreenOrientationManager.SetIsOpen(true);
    }
    #endregion
    
    #region 跳过
    public bool SkipMode { get; set; }

    private float mStopPauseTime = 0;

    private bool mPause = false;
    public bool IsPause
    {
        get
        {
            return mPause;
        }
        set
        {
            mPause = value;
        }
    }

    private int mSkipLine = -1;
    private int mSkipStory = -1;
    private bool mSkipToFinish = false;

    public bool RequestSkip()
    {
        if (CurStoryTable == null)
            return false;

        Tab_Story next = FindNextSkipStop(CurStoryTable);

        if (next == null)
            return false;

        int branchCnt = GetBranchCnt(next);

        if (branchCnt >= 0)
        {
            SkipMode = true;
        }

        mPause = true;
        mStopPauseTime = Time.time + GlobeVar.STORY_SKIP_TIME_GAP;

        if (branchCnt == 0)
        {
            // req finish
            //StoryHandler.ReqSkipTo(next.LindID, next.Id, true);
            mSkipLine = next.LindID;
            mSkipStory = next.Id;
            mSkipToFinish = CurStoryTable.Id == next.Id;//true;           
            Debug.LogFormat("request skip start: (L {0}, S {1})->(L {2}, S {3}), to finish {4}", 
                GameManager.PlayerDataPool.m_nStoryLine, GameManager.PlayerDataPool.m_nStroyID,
                next.LindID, next.Id, true);
            return true;
        }
        else if (branchCnt == 1)
        {
            // req next
            //StoryHandler.ReqSkipTo(next.LindID, next.Id, false);
            mSkipLine = next.LindID;
            mSkipStory = next.Id;
            mSkipToFinish = false;
            Debug.LogFormat("request skip start: (L {0}, S {1})->(L {2}, S {3}), to finish {4}",
                GameManager.PlayerDataPool.m_nStoryLine, GameManager.PlayerDataPool.m_nStroyID,
                next.LindID, next.Id, false);
            return true;
        }
        else if (branchCnt > 1)
        {
            //StoryHandler.ReqSkipTo(next.LindID, next.Id, false);
            mSkipLine = next.LindID;
            mSkipStory = next.Id;
            mSkipToFinish = false;
            Debug.LogFormat("request skip start: (L {0}, S {1})->(L {2}, S {3}), to finish {4}",
                GameManager.PlayerDataPool.m_nStoryLine, GameManager.PlayerDataPool.m_nStroyID,
                next.LindID, next.Id, false);
            return true;
        }

        return false;
    }

    void CheckAsyncSkip()
    {
        if (!SkipMode || mSkipLine == GlobeVar.INVALID_ID)
            return;
        
        Debug.LogFormat("request skip end: (L {0}, S {1})->(L {2}, S {3}), to finish {4}",
                GameManager.PlayerDataPool.m_nStoryLine, GameManager.PlayerDataPool.m_nStroyID,
                mSkipLine, mSkipStory, mSkipToFinish);

        StoryHandler.ReqSkipTo(mSkipLine, mSkipStory, mSkipToFinish);

        mSkipLine = -1;
        mSkipStory = -1;
        mSkipToFinish = false;
    }

    #endregion

    #region 系统自动调用接口

    const float STORY_FINISH_TIMEOUT = 3f;
    void Update()
    {
        if (!m_bStoryStarted)
        {
            var state = GameManager.PlayerDataPool.StoryState;
            if (Time.time - GameManager.PlayerDataPool.StoryStateTime >= STORY_FINISH_TIMEOUT && state != (int)STORY_STATE.NORMAL)
            {
                switch (state)
                {
                    case (int)STORY_STATE.WAITING_FINISH:
                        LogModule.DebugLogFormat("retry: STORY_STATE.WAITING_FINISH, line {0}", GameManager.PlayerDataPool.m_nStoryLine);
                        StoryHandler.ReqEnterNextStep(GameManager.PlayerDataPool.m_nStoryLine, GlobeVar.INVALID_ID);
                        break;
                    case (int)STORY_STATE.WAITING_FINISH_SKIP:
                        LogModule.DebugLogFormat("retry: STORY_STATE.WAITING_FINISH_SKIP, line {0}", GameManager.PlayerDataPool.m_nStoryLine);
                        StoryHandler.ReqSkipTo(GameManager.PlayerDataPool.m_nStoryLine, GlobeVar.INVALID_ID, true);
                        break;
                    case (int)STORY_STATE.WAITING_LEAVE:
                        LogModule.DebugLogFormat("retry: STORY_STATE.WAITING_LEAVE, line {0}", GameManager.PlayerDataPool.m_nStoryLine);
                        StoryHandler.ReqLeaveStory();
                        break;
                }
            }
            return;
        }

        if (mPause)
        {
            if (mStopPauseTime > Time.time)
                return;
            mPause = false;
        }

        if (SkipMode)
        {
            CheckAsyncSkip();
        }
        else
        {
            if (null != m_CurStory)
            {
                m_CurStory.Update();
            }
        }
    }
    #endregion

    #region 新手引导
    public int GetCurTutorialSceneNpcId()
    {
        if (false == TutorialManager.IsOpenTutorial)
        {
            return GlobeVar.INVALID_ID;
        }

        if (m_CurStoryTable == null)
        {
            return GlobeVar.INVALID_ID;
        }

        if (m_CurStoryTable.LindID == GlobeVar.TutorialStory_StoryLineId &&
            m_CurStoryTable.Id == GlobeVar.TutorialStory_StoryId)
        {
            return GlobeVar.TutorialStory_SceneNpcId;
        }

        for (int i = 0; i < GlobeVar.TutorialOtherStory_StoryInfo.Length; i++)
        {
            if (m_CurStoryTable.LindID == GlobeVar.TutorialOtherStory_StoryInfo[i].m_StoryLineId &&
                m_CurStoryTable.Id == GlobeVar.TutorialOtherStory_StoryInfo[i].m_StoryId)
            {
                return GlobeVar.TutorialOtherStory_StoryInfo[i].m_SceneNpcId;
            }
        }

        return GlobeVar.INVALID_ID;
    }

    #endregion

    #region 查询接口
    //有新的事件开启，更新移动标记
    private void UpdateMainPlayerEventMoveFlag()
    {
        if (null == m_CurStory)
        {
            return;
        }

        if (null != ObjManager.MainPlayer)
        {
            bool can = CanMove();
            ObjManager.MainPlayer.EventMove = can;
            GameManager.CameraManager.IsDragLock = !can;

            if (can)
            {
                UIManager.ShowUI(UIInfo.JoyStick);
            }
            else
            {
                UIManager.CloseUI(UIInfo.JoyStick);
            }

            if (StoryModeController.Instance() != null)
            {
                StoryModeController.Instance().UpdateCloseBtn();
            }

        }
    }

    public bool CanMove()
    {
        bool can = true;
        bool hit = false;

        if (m_CurStory != null)
        {
            for (int i = 0; i < m_CurStory.EventList.Count; ++i)
            {
                if (m_CurStory.EventList[i] != null && m_CurStory.EventList[i].State == STORY_EVENT_STATE.ACITVE)
                {
                    hit = true;
                    can &= m_CurStory.EventList[i].m_IsCanMove;
                }
            }
            // 如果当前没有激活的事件, 则按Story表中默认配置更新是否可移动
            if (!hit)
            {
                can = m_CurStory.DefaultCanMove;
            }
        }
        return can;
    }


    static public bool StoryLineAvailable(int storylineId)
    {
        return StoryLineAvailableWithReason(storylineId) == StoryLineNAReason.AVAILABLE;
    }

    public enum StoryLineNAReason { AVAILABLE, UNKNOWN_ERROR, LEVEL, STORY_FLAG, HAS_CARD, CARD_AWAKE, CARD_LEVEL, CARD_INTIMACY, CARD_STAR, CARD_BATTLE_CNT, TUTORIAL_AWARD}

    static public StoryLineNAReason StoryLineAvailableWithReason(int storylineId)
    {
        Tab_StoryLine _tabLine = TableManager.GetStoryLineByID(storylineId, 0);
        if (_tabLine == null)
            return StoryLineNAReason.UNKNOWN_ERROR;

        //是否领取任务奖励
        if (_tabLine.TutorialIDAwardFin != GlobeVar.INVALID_ID)
        {
            if (!GameManager.PlayerDataPool.IsTutorialQuestFinish(_tabLine.TutorialIDAwardFin, TutorialQuestType.Main))
            {
                return StoryLineNAReason.TUTORIAL_AWARD;
            }
        }
       
        //等级
        if (GameManager.PlayerDataPool.PlayerHeroData.Level < _tabLine.Level)
        {
            return StoryLineNAReason.LEVEL;
        }

        //标记位
        if (_tabLine.StoryFlag > GlobeVar.INVALID_ID && _tabLine.StoryFlag < GameManager.PlayerDataPool.m_bStoryFlagList.Length)
        {
            if (GameManager.PlayerDataPool.m_bStoryFlagList[_tabLine.StoryFlag] == false)
            {
                return StoryLineNAReason.STORY_FLAG;
            }
        }



        if (storylineId < 0 || storylineId >= GameManager.PlayerDataPool.m_nStoryLineStatusList.Length)
        {
            return StoryLineNAReason.UNKNOWN_ERROR;
        }
        

        //卡牌条件判断
        if (_tabLine.CardLevelID > GlobeVar.INVALID_ID  && false == GameManager.PlayerDataPool.m_nStoryLineStatusList[storylineId].GetLineFlag((int)STORYLINE_FLAG.LEVEL))
        {
            return StoryLineNAReason.CARD_LEVEL;
        }
        if (_tabLine.CardIntimacyID > GlobeVar.INVALID_ID && false == GameManager.PlayerDataPool.m_nStoryLineStatusList[storylineId].GetLineFlag((int)STORYLINE_FLAG.INTIMACY))
        {
            return StoryLineNAReason.CARD_INTIMACY;
        }
        if (_tabLine.CardStarID > GlobeVar.INVALID_ID && false == GameManager.PlayerDataPool.m_nStoryLineStatusList[storylineId].GetLineFlag((int)STORYLINE_FLAG.STAR))
        {
            return StoryLineNAReason.CARD_STAR;
        }
        if (_tabLine.AwakeCardID > GlobeVar.INVALID_ID && false == GameManager.PlayerDataPool.m_nStoryLineStatusList[storylineId].GetLineFlag((int)STORYLINE_FLAG.AWAKE))
        {
            return StoryLineNAReason.CARD_AWAKE;
        }
        if (_tabLine.CollectCardID > GlobeVar.INVALID_ID && false == GameManager.PlayerDataPool.m_nStoryLineStatusList[storylineId].GetLineFlag((int)STORYLINE_FLAG.COLLECTION))
        {
            return StoryLineNAReason.HAS_CARD;
        }
        return StoryLineNAReason.AVAILABLE;
    }

    // 返回所有剧情不合法原因, 满足所有条件则返回空容器
    static public HashSet<StoryLineNAReason> StoryLineAvailableWithAllReason(int storylineId)
    {
        HashSet<StoryLineNAReason> ret = new HashSet<StoryLineNAReason>();

        Tab_StoryLine _tabLine = TableManager.GetStoryLineByID(storylineId, 0);
        if (_tabLine == null)
        {
            ret.Add(StoryLineNAReason.UNKNOWN_ERROR);
            return ret;
        }

        //是否领取任务奖励
        if (_tabLine.TutorialIDAwardFin != GlobeVar.INVALID_ID)
        {
            if (!GameManager.PlayerDataPool.IsTutorialQuestFinish(_tabLine.TutorialIDAwardFin, TutorialQuestType.Main))
            {
                ret.Add(StoryLineNAReason.TUTORIAL_AWARD);
            }
        }

        //等级
        if (GameManager.PlayerDataPool.PlayerHeroData.Level < _tabLine.Level)
        {
            ret.Add(StoryLineNAReason.LEVEL);
        }

        //标记位
        if (_tabLine.StoryFlag > GlobeVar.INVALID_ID && _tabLine.StoryFlag < GameManager.PlayerDataPool.m_bStoryFlagList.Length)
        {
            if (GameManager.PlayerDataPool.m_bStoryFlagList[_tabLine.StoryFlag] == false)
            {
                ret.Add(StoryLineNAReason.STORY_FLAG);
            }
        }
        

        if (storylineId < 0 || storylineId >= GameManager.PlayerDataPool.m_nStoryLineStatusList.Length)
        {
            ret.Add(StoryLineNAReason.UNKNOWN_ERROR);
        }

        //卡牌条件判断
        if (_tabLine.CardLevelID > GlobeVar.INVALID_ID && false == GameManager.PlayerDataPool.m_nStoryLineStatusList[storylineId].GetLineFlag((int)STORYLINE_FLAG.LEVEL))
        {
            ret.Add(StoryLineNAReason.CARD_LEVEL); 
        }
        if (_tabLine.CardIntimacyID > GlobeVar.INVALID_ID && false == GameManager.PlayerDataPool.m_nStoryLineStatusList[storylineId].GetLineFlag((int)STORYLINE_FLAG.INTIMACY))
        {
            ret.Add(StoryLineNAReason.CARD_INTIMACY); 
        }
        if (_tabLine.CardStarID > GlobeVar.INVALID_ID && false == GameManager.PlayerDataPool.m_nStoryLineStatusList[storylineId].GetLineFlag((int)STORYLINE_FLAG.STAR))
        {
            ret.Add(StoryLineNAReason.CARD_STAR); 
        }
        if (_tabLine.CardIntimacyID > GlobeVar.INVALID_ID && false == GameManager.PlayerDataPool.m_nStoryLineStatusList[storylineId].GetLineFlag((int)STORYLINE_FLAG.AWAKE))
        {
            ret.Add(StoryLineNAReason.CARD_AWAKE);
        }
        if (_tabLine.CollectCardID > GlobeVar.INVALID_ID && false == GameManager.PlayerDataPool.m_nStoryLineStatusList[storylineId].GetLineFlag((int)STORYLINE_FLAG.COLLECTION))
        {
            ret.Add(StoryLineNAReason.HAS_CARD);
        }
        return ret;
    }

    static public string CenterNoticeReasonText(StoryLineNAReason reason, int lineId)
    {
        string ret = StrDictionary.GetDicByID(48);//无法进入剧情
        Tab_StoryLine _tabStoryLine = TableManager.GetStoryLineByID(lineId, 0);
        if (null == _tabStoryLine)
        {
            return ret;
        }
        
        switch (reason)
        {
            case StoryLineNAReason.UNKNOWN_ERROR:
            case StoryLineNAReason.LEVEL:
                return StrDictionary.GetDicByID(49, _tabStoryLine.Level);
            case StoryLineNAReason.STORY_FLAG:
                return StrDictionary.GetDicByID(50);
            case StoryLineNAReason.HAS_CARD:
            {
                Tab_Card _tabCard = TableManager.GetCardByID(_tabStoryLine.CollectCardID, 0);
                if (null != _tabCard)
                {
                    Tab_RoleBaseAttr _tabRoleBase =
                        TableManager.GetRoleBaseAttrByID(_tabCard.GetRoleBaseIDStepbyIndex(0), 0);
                    if (null != _tabRoleBase)
                        return StrDictionary.GetDicByID(51, _tabRoleBase.Name);
                }
                return ret;
            }
            case StoryLineNAReason.CARD_AWAKE:
            {
                Tab_Card _tabCard = TableManager.GetCardByID(_tabStoryLine.AwakeCardID, 0);
                if (null != _tabCard)
                {
                    Tab_RoleBaseAttr _tabRoleBase =
                        TableManager.GetRoleBaseAttrByID(_tabCard.GetRoleBaseIDStepbyIndex(0), 0);
                    if (null != _tabRoleBase)
                        return StrDictionary.GetDicByID(52, _tabRoleBase.Name);
                }
                return ret;
            }
            case StoryLineNAReason.CARD_LEVEL:
            {
                Tab_Card _tabCard = TableManager.GetCardByID(_tabStoryLine.CardLevelID, 0);
                if (null != _tabCard)
                {
                    Tab_RoleBaseAttr _tabRoleBase =
                        TableManager.GetRoleBaseAttrByID(_tabCard.GetRoleBaseIDStepbyIndex(0), 0);
                    if (null != _tabRoleBase)
                        return StrDictionary.GetDicByID(56, _tabRoleBase.Name, _tabStoryLine.CardLevel);
                }
                return ret;
            }
            case StoryLineNAReason.CARD_INTIMACY:
                {
                    Tab_Card _tabCard = TableManager.GetCardByID(_tabStoryLine.CardIntimacyID, 0);
                    if (null != _tabCard)
                    {
                        Tab_RoleBaseAttr _tabRoleBase =
                            TableManager.GetRoleBaseAttrByID(_tabCard.GetRoleBaseIDStepbyIndex(0), 0);
                        if (null != _tabRoleBase)
                            return StrDictionary.GetDicByID(57, _tabRoleBase.Name, _tabStoryLine.CardIntimacy);
                    }
                    return ret;
                }
            case StoryLineNAReason.CARD_STAR:
                {
                    Tab_Card _tabCard = TableManager.GetCardByID(_tabStoryLine.CardStarID, 0);
                    if (null != _tabCard)
                    {
                        Tab_RoleBaseAttr _tabRoleBase =
                            TableManager.GetRoleBaseAttrByID(_tabCard.GetRoleBaseIDStepbyIndex(0), 0);
                        if (null != _tabRoleBase)
                            return StrDictionary.GetDicByID(58, _tabRoleBase.Name, _tabStoryLine.CardStar);
                    }
                    return ret;
                }
            case StoryLineNAReason.TUTORIAL_AWARD:
                {
                    Tab_TutorialQuest tQuest = TableManager.GetTutorialQuestByID(_tabStoryLine.TutorialIDAwardFin, 0);
                    if (tQuest != null)
                    {
                        return StrDictionary.GetDicByID(8230, tQuest.Name);
                    }
                    return ret;
                }
                //case StoryLineNAReason.CARD_BATTLE_CNT:
                //    break;
        }
        
        return ret;
    }

    static public bool IsNewStoryLine(int storylineId, bool checkClicked, System.UInt64 cardGuid)
    {
        if (!StoryManager.StoryLineAvailable(storylineId))
            return false;

        if (GameManager.PlayerDataPool == null)
            return false;

        bool isNew = !GameManager.PlayerDataPool.IsStoryEntered(storylineId);
        if (checkClicked)
        {
            isNew &= !PlayerPreferenceData.IsClickedCardStory(storylineId);
        }

        return isNew;
    }

    static public bool IsCardBiographyAvailable(Card card, int index)
    {
        if (card == null)
            return false;

        var tabCardStory = TableManager.GetCardStoryByID(card.CardId, 0);
        if (tabCardStory == null)
            return false;

        if (index < 0 || index >= tabCardStory.getBiographyIntimacyCount())
            return false;

        int inLv = tabCardStory.GetBiographyIntimacybyIndex(index);
        if (inLv <= 0)
            return false;

        if (card.GetIntimacyLevel() < inLv)
            return false;

        return true;
    }

    static public bool IsNewCardBiography(Card card, int index)
    {
        return IsCardBiographyAvailable(card, index) && !PlayerPreferenceData.IsClickedCardBiography(card.CardId, index);
    }
    #region 亲密度人物志
    static public bool HasNewCardLegend(Card card)
    {
        if (card == null)
            return false;

        var tabCardStory = TableManager.GetCardStoryByID(card.CardId, 0);
        if (tabCardStory == null)
            return false;

        for (int i = 0; i < tabCardStory.getBiographyIntimacyCount(); ++i)
        {
            int inLv = tabCardStory.GetBiographyIntimacybyIndex(i);
            if (inLv <= 0)
                continue;

            if (card.GetIntimacyLevel() >= inLv && !PlayerPreferenceData.IsClickCardLegend(card.CardId, i))
                return true;
        }

        return false;
    }
    #endregion


    static public bool IsStoryLineAwardAvailable(int storylineId)
    {
        if (storylineId < 0 || storylineId >= GlobeVar.STORY_LINE_CAP)
            return false;

        if (GameManager.PlayerDataPool == null)
            return false;

        var state = GameManager.PlayerDataPool.m_nStoryLineStatusList[storylineId];

        return state.m_nFin > 0 && !state.m_bAwardAccepted;
    }

    public bool IsCurStoryLineFirstEnter()
    {
        if (m_CurStoryTable == null)
        {
            return false;
        }

        return GameManager.PlayerDataPool.GetStoryFin(m_CurStoryTable.LindID) == 0;
    }

    static public int GetBranchCnt(Tab_Story story)
    {
        int count = 0;
        for (int i = 0; i < story.getBranchCount(); ++i)
        {
            if (story.GetBranchbyIndex(i) > 0)
            {
                count++;
            }
        }
        return count;
    }

    static public Tab_Story FindNextSkipStop(Tab_Story start)
    {
        if (start == null)
            return null;
        
        Tab_Story next = null;
        FindNextSkipStopImpl(start, start, ref next);
        
        return next;
    }

    static void FindNextSkipStopImpl(Tab_Story root, Tab_Story current, ref Tab_Story result)
    {
        if (current == null)
            return;

        if (current.SkipStop == 1 && current != root)
        {
            result = current;
            return;
        }

        int curBranchcount = 0;
        int next = 0;
        for (int i = 0; i < current.getBranchCount(); ++i)
        {
            if (current.GetBranchbyIndex(i) > 0)
            {
                next = current.GetBranchbyIndex(i);
                curBranchcount++;
            }
        }

        if (curBranchcount == 0 || curBranchcount > 1)
        {
            result = current;
            return;
        }
        
        Tab_Story nextStory = TableManager.GetStoryByID(next, 0);
        if (nextStory == null)
            return;

        FindNextSkipStopImpl(root, nextStory, ref result);
    }

    #endregion
}
