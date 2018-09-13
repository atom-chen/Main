/********************************************************************************
 *	文件名：	GameManager.cs
 *	全路径：	\Script\GlobalSystem\Manager\GameManager.cs
 *	创建人：	李嘉
 *	创建时间：2016-12-28
 *
 *	功能说明：游戏全局管理器，在第一次进入游戏后加载
 *	修改记录：
*********************************************************************************/

using System;
using UnityEngine;
using System.Collections;
using Games.GlobeDefine;
using Games.Table;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using BattleCore;
using ProtobufPacket;
using UnityEngine.SceneManagement;
using XLua;
using Utils = Games.Utils;
using Games.LogicObj;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public class GameManager
{
    private GameManager() { }

    //////////////////////////////////////////////////////////////////////////
    //场景相关
    //////////////////////////////////////////////////////////////////////////

    private static int m_RunningScene = -1;
    public static int RunningScene
    { // 这个值是和sceneclass的id对应的
        get { return m_RunningScene; }
        set { m_RunningScene = value; }
    }

    //lijia todo
    //暂时增加离线模式
    public static bool m_bOffLineMode = false;
    public static bool m_bQuickBattle = false;
    public static int m_gmBattleSpeed = 0;

    public static bool skipPrepare = false;
    private static bool m_isInitGameFinish = false;
    public static bool IsInitGameFinish
    {
        get { return m_isInitGameFinish; }
    }
    private static bool m_IsLogin = true;
    public static bool IsLogin
    {
        get { return m_IsLogin; }
        set { m_IsLogin = value; }
    }
    
    public static bool IsInGameScene()
    {
        return m_RunningScene > (int)SCENE_DEFINE.SCENE_GAMESCENE;
    }

    private static GameSceneLogic m_gameSceneLogic = null;
    private static LoadingController m_LoadingRoot=null;
    //新的场景系统，之后会废弃ActiveSceneLogic
    private static Scene m_CurScene = null;
    public static Scene CurScene
    {
        get { return m_CurScene; }
        set
        {
            m_CurScene = value;

            if (ConvenientNoticeController.Instance!=null)
            {
                ConvenientNoticeController.Instance.IsShowInCurScene();
            }
        }
    }

    private static bool m_bReqTeleporting = false;
    public static bool ReqTeleporting
    {
        get { return m_bReqTeleporting; }
        set { m_bReqTeleporting = value; }
    }

    private static bool m_bLoadingScene = false;
    public static bool LoadingScene
    {
        get { return m_bLoadingScene; }
    }

    private static int m_nLastSceneId = 0;
    public static int LastSceneId
    {
        set { m_nLastSceneId = value; }
        get { return m_nLastSceneId; }
    }
    private static int m_LastLoadSceneID;
    public static int LastLoadSceneID
    {
        get { return m_LastLoadSceneID; }
    }
    public static float m_fUIScale
    {
        get
        {
            return (float)UIRootAdapter.GetLogicHeight() / Screen.height;
        }
    }

    private static bool m_IsHorizontalScreen = true;
    public static bool IsHorizontalScreen
    {
        get { return m_IsHorizontalScreen; }
        set { m_IsHorizontalScreen = value; }
    }

    //////////////////////////////////////////////////////////////////////////
    //自动寻路控件
    //////////////////////////////////////////////////////////////////////////
    //private static AutoSearchAgent m_AutoSearch;
    //public static AutoSearchAgent AutoSearch
    //{
    //    get { return m_AutoSearch; }
    //    set { m_AutoSearch = value; }
    //}

    //////////////////////////////////////////////////////////////////////////
    //剧情管理器
    //////////////////////////////////////////////////////////////////////////
    private static StoryManager m_storyManager;
    public static StoryManager storyManager
    {
        get { return m_storyManager; }
        set { m_storyManager = value; }
    }

    public static ProcessInput InputManager { get; set; }
    public static CameraController CameraManager { get; private set; }

    //////////////////////////////////////////////////////////////////////////
    //声音相关
    //////////////////////////////////////////////////////////////////////////
    private static SoundManager m_SoundManager;
    public static SoundManager SoundManager
    {
        get { return m_SoundManager; }
        set { m_SoundManager = value; }
    }

    //////////////////////////////////////////////////////////////////////////
    //网络管理器
    //////////////////////////////////////////////////////////////////////////
    private static NetManager m_NetManager;
    public static NetManager NetManager
    {
        get { return m_NetManager; }
        set { m_NetManager = value; }
    }

    //////////////////////////////////////////////////////////////////////////
    //网络管理器
    //////////////////////////////////////////////////////////////////////////
    private static MsdkSafeManager m_MsdkSafeManager;
    public static MsdkSafeManager MsdkSafeManager
    {
        get { return m_MsdkSafeManager; }
        set { m_MsdkSafeManager = value; }
    }

    //////////////////////////////////////////////////////////////////////////
    //自定义头像加载
    //////////////////////////////////////////////////////////////////////////
    private static LoadCustomHead m_LoadCustomHead;

    //////////////////////////////////////////////////////////////////////////
    //腾讯地图定位服务
    //////////////////////////////////////////////////////////////////////////
    public static TencentLocationService m_LocationService;

    //////////////////////////////////////////////////////////////////////////
    //腾讯UIC（屏蔽字、图片）
    //////////////////////////////////////////////////////////////////////////
    private static TencentUIC m_TencentUIC;

    //////////////////////////////////////////////////////////////////////////
    //万象优图上传
    //////////////////////////////////////////////////////////////////////////
    private static UploadImageManager m_UploadImageManager;

    //////////////////////////////////////////////////////////////////////////
    //心跳
    //////////////////////////////////////////////////////////////////////////
    public static DeviceStatusWindow m_DeviceStatusWindow;

    public static NPCMiscManager m_NPCMiscManager;
    //////////////////////////////////////////////////////////////////////////
    //全局冷却管理器
    //////////////////////////////////////////////////////////////////////////
    private static CoolDownManager m_CDManager;
    public static CoolDownManager CDManager
    {
        get { return m_CDManager; }
        set { m_CDManager = value; }
    }

    //////////////////////////////////////////////////////////////////////////
    //系统公告管理器
    //////////////////////////////////////////////////////////////////////////
    private static RollNoticeManager m_SystemRollMgr;
    public static RollNoticeManager SystemRollMgr
    {
        get { return m_SystemRollMgr; }
        set { m_SystemRollMgr = value; }
    }

    //////////////////////////////////////////////////////////////////////////
    //环境相关
    //////////////////////////////////////////////////////////////////////////
    private static EnvironmentManager m_EnvManager;
    public static EnvironmentManager EnvManager
    {
        get { return m_EnvManager; }
        set { m_EnvManager = value; }
    }
    //////////////////////////////////////////////////////////////////////////
    //玩家数据
    //////////////////////////////////////////////////////////////////////////
    private static PlayerData m_PlayerDataPool = new PlayerData();
    public static PlayerData PlayerDataPool
    {
        get { return m_PlayerDataPool; }
        set { m_PlayerDataPool = value; }
    }
    //Fps
    public static FPSMgr m_FPSMangger;
    public static FPSMgr FPSMangger
    {
        get { return m_FPSMangger; }
        set { m_FPSMangger = value; }
    }

    #region Lua
    public static LuaEnv L = null;
    [CSharpCallLua]
    public delegate void delReLua(string path);
    private static delReLua reluaFunc;

    public static void InitLua()
    {
        if (L != null)
        {
            L.Dispose();
        }
        L = new LuaEnv();
        //string setPbPath = string.Format("lua_root = '{0}/Lua/'", UnityEngine.Application.streamingAssetsPath);
        string setPbPath = "lua_root = ''";
        L.DoString(setPbPath);
        L.DoString(@"require('startup')");
        reluaFunc = L.Global.Get<delReLua>("relua");
    }

    public static void ReLua()
    {
        if (L == null)
        {
            return;
        }
        foreach (var value in TableManager.GetBattleScript())
        {
            foreach (var tabBattleScript in value.Value)
            {
                reluaFunc(tabBattleScript.Path);
            }
        }
        //LogModule.DebugLog("relua done");
    }

    #endregion

    #region GameTableManager
    //////////////////////////////////////////////////////////////////////////
    //表格管理器
    //////////////////////////////////////////////////////////////////////////
    private static TableManager m_TableManager = new TableManager();
    public static TableManager TableManager { get { return m_TableManager; } }
    #endregion

    private static GameObject m_objGameManager = null;

    //当前切换场景是否使用Loading特效
    public static bool IsUseLoadingEffect { get { return m_bUseLoadingEffect; } }
    private static bool m_bUseLoadingEffect = false;

    //////////////////////////////////////////////////////////////////////////
    //横竖屏管理器
    //////////////////////////////////////////////////////////////////////////
    private static ScreenOrientationManager m_ScreenOrientationManager = null;
    public static ScreenOrientationManager ScreenOrientationManager
    {
        get { return m_ScreenOrientationManager; }
    }

    #region Init Game

    //初始化游戏管理器
    public static void InitGame()
    {
        if (null != m_objGameManager)
        {
            return;
        }

        Application.targetFrameRate = 30;
        Application.runInBackground = true;

        m_objGameManager = new GameObject("GameManager");
        GameObject.DontDestroyOnLoad(m_objGameManager);

        GameObject assetCacheGo = new GameObject("AssetCache");
        assetCacheGo.transform.SetParent(m_objGameManager.transform);
        assetCacheGo.AddComponent<AssetCacheManagr>();

        DebugInfo.Create();

        AssetManager.LoadObj("Prefab/Logic/FingerGestures", "FingerGestures");

        //声音管理器
        m_SoundManager = Utils.TryAddComponent<SoundManager>(m_objGameManager);

        //环境管理器
        m_EnvManager = Utils.TryAddComponent<EnvironmentManager>(m_objGameManager);

        //冷却管理器
        m_CDManager = Utils.TryAddComponent<CoolDownManager>(m_objGameManager);

        //公告管理器
        m_SystemRollMgr = Utils.TryAddComponent<RollNoticeManager>(m_objGameManager);

        m_NPCMiscManager = Utils.TryAddComponent<NPCMiscManager>(m_objGameManager);

        //任务管理器
        m_storyManager = Utils.TryAddComponent<StoryManager>(m_objGameManager);

        InputManager = Utils.TryAddComponent<ProcessInput>(m_objGameManager);

        CameraManager = Utils.TryAddComponent<CameraController>(m_objGameManager);
        Utils.TryAddComponent<PinchRecognizer>(m_objGameManager);

        //平台管理器

        GameObject objPlatformListener = new GameObject("PlatformListener");
        GameObject.DontDestroyOnLoad(objPlatformListener);
        Utils.TryAddComponent<PlatformListener>(objPlatformListener);

        GameObject objAndroidCallback = new GameObject("AndroidCallback");
        GameObject.DontDestroyOnLoad(objAndroidCallback);
        Utils.TryAddComponent<AndroidCallback>(objAndroidCallback);

        GameObject objMSDK = new GameObject("MsdkWakeupCallback");
        GameObject.DontDestroyOnLoad(objMSDK);
        Utils.TryAddComponent<MsdkWakeupCallback>(objMSDK);

        Utils.TryAddComponent<AudioListener>(m_objGameManager);

        //网络管理器
        m_NetManager = Utils.TryAddComponent<NetManager>(m_objGameManager);

        m_MsdkSafeManager = Utils.TryAddComponent<MsdkSafeManager>(m_objGameManager);

        m_LoadCustomHead = Utils.TryAddComponent<LoadCustomHead>(m_objGameManager);

        m_DeviceStatusWindow = Utils.TryAddComponent<DeviceStatusWindow>(m_objGameManager);       
        //FPS
        m_FPSMangger = Utils.TryAddComponent<FPSMgr>(m_objGameManager);

        //m_TableManager.InitTable();

        //初始化玩家数据
        m_PlayerDataPool = new PlayerData();

        if (null != LoginData.user)
        {
            m_PlayerDataPool.Name = LoginData.user.name;
        }

        InitLua();

        // 横竖屏管理器
        m_ScreenOrientationManager = Utils.TryAddComponent<ScreenOrientationManager>(m_objGameManager);
        m_isInitGameFinish = true;
        //腾讯地图定位
        m_LocationService = Utils.TryAddComponent<TencentLocationService>(m_objGameManager);

        //腾讯UIC（屏蔽字、图片）
        m_TencentUIC = Utils.TryAddComponent<TencentUIC>(m_objGameManager);

        //万象优图上传
        m_UploadImageManager = Utils.TryAddComponent<UploadImageManager>(m_objGameManager);
    }
    #endregion

    #region 游戏场景逻辑
    //进入一个游戏场景
    public static void EnterGameScene(int gameSceneID, bool forceLoad = false, bool bTeleport = false)
    {
        //检查要进入的是否为游戏场景
        if (gameSceneID < (int)SCENE_DEFINE.SCENE_LOGIN)
        {
            return;
        }

        if (LoadingScene)
        {
            m_NewEnterSceneId = gameSceneID;
            m_NewEnterSceneForceLoad = forceLoad;
            m_NewEnterSceneTeleport = bTeleport;
            return;
        }

        //记录上一个场景的信息
        CacheLastLobbySceneInfo(bTeleport);
        if (QuickChatController.Ins != null)
        {
            QuickChatController.Ins.m_QuickIntercourseRoot.SetActive(false);
            QuickChatController.Ins.gameObject.SetActive(true);
            QuickChatController.Ins.m_QuickChatTweenPosition.ResetToBeginning();
        }
        if (_EnterGameScene(gameSceneID, forceLoad))
        {
            //切换场景
            AssetLoader.Instance.StartCoroutine(LoadGameScene(gameSceneID, forceLoad));
        }
    }

    static bool _EnterGameScene(int gameSceneID, bool forceLoad)
    {
        if (m_bLoadingScene)
        {
            return false;
        }
        SCENE_DEFINE curActiveSceneType = (SCENE_DEFINE)SceneManager.GetActiveScene().buildIndex;
        //if (curActiveSceneType == SCENE_DEFINE.SCENE_LAUNCH)
        //{
        //    LogModule.ErrorLog("can not enter game from launch");
        //    return false;
        //}
        //如果当前是战斗中，则不会重新记录上一个场景
        if (!(CurScene is BattleScene))
        {
            LastSceneId = RunningScene; // 记录下最后一个场景id
        }
        //m_LastLoadSceneID用于Loading界面切换使用
        //需要记录战斗中的ID、所以重新记录一个
        m_LastLoadSceneID = RunningScene;
        m_bLoadingScene = true;
        return true;
    }

    static public bool m_bStillShowLoading = false;
    static public void HideLoadingWindow()
    {
        m_bStillShowLoading = false;
        if (null != m_gameSceneLogic)
        {
            m_LoadingRoot.ShowLoading(false, 0.0f, GlobeVar.INVALID_ID);
        }
    }

    enum LoadSceneType
    {
        FullLoad,   //完整加载
        SameResLoad, //同场景资源，Loading切换
        QuickSwitch, //快速切换
    }


    private static bool FirstEnterGame = true;
    private static int quickLoadCount = 0;
    //游戏场景加载协程
    static IEnumerator LoadGameScene(int gameSceneID, bool forceLoad)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        NPCMiscManager.OnEnterScene(gameSceneID);

        if (null != ConvenientNoticeController.Instance)
        {
            ConvenientNoticeController.Close();
        }
        UIManager.CloseUI(UIInfo.StoryTalk);

        Tab_SceneClass sceneClass = TableManager.GetSceneClassByID(gameSceneID, 0);
        if (null == sceneClass)
        {
            LogModule.WarningLog("Load Scene failed,no such scene in sceneClass:" + gameSceneID);
            m_bLoadingScene = false;
            yield break;
        }

        //检查是否变身状态
        if (0 == sceneClass.EnableMask)
        {
            PlayerDataPool.CheckAndStopRoleMask();
        }

        
        //是否是相同场景资源，如果相同则是位面逻辑
        bool bSameSceneRes = sceneClass.SceneResource == SceneManager.GetActiveScene().buildIndex;

        LoadSceneType loadType = LoadSceneType.FullLoad;
        bool shouldPlayQuickSwitchEffect = false;
#region 加载形式判定
        if (!bSameSceneRes)
        {
            loadType = LoadSceneType.FullLoad; //正常切场景loading
        }
        else
        {
            if (forceLoad)
            {
                loadType = LoadSceneType.FullLoad; //强制正常切场景loading
            }
            else
            {
                if (CurScene != null)
                {
                    if (IsSameLogic(CurScene.GetSceneType(), (SCENETYPE)sceneClass.Type))
                    {
                        loadType = LoadSceneType.QuickSwitch;
                    }
                    else
                    {
                        if (CurScene.GetSceneType() == SCENETYPE.BATTLE ||
                                 (SCENETYPE)sceneClass.Type == SCENETYPE.BATTLE)
                        {
                            bool hasLoading = false;

                            //去竞技场战斗总是走loading
                            if (CurBattlePacket != null)
                            {
                                if (CurBattlePacket.battleRetType == (int) BattleRetType.Arena)
                                {
                                    hasLoading = true;
                                }
                            }
                            
                            loadType = hasLoading ? LoadSceneType.SameResLoad 
                                : LoadSceneType.QuickSwitch;
                        }
                        else
                        {
                            loadType = LoadSceneType.SameResLoad;
                        }
                    }
                }
                else
                {
                    loadType = LoadSceneType.SameResLoad;
                }
            }
        }


        //同资源加载，但是是离开战斗，走快速切换，但是有全屏遮罩，不需要水波纹特效
        if (loadType == LoadSceneType.QuickSwitch)
        {
            if (CurScene != null && CurScene.GetSceneType() == SCENETYPE.BATTLE &&
                (SCENETYPE) sceneClass.Type != SCENETYPE.BATTLE)
            {
                shouldPlayQuickSwitchEffect = false;
            }
            else
            {
                shouldPlayQuickSwitchEffect = true;
            }
        }
        #endregion

        bool removeMainPlayer = loadType != LoadSceneType.QuickSwitch;
        if ((SCENETYPE)sceneClass.Type == SCENETYPE.BATTLE)
        {
            //进入战斗，始终要删除主角
            removeMainPlayer = true;
        }

        //if (IsInGameScene() && null != m_gameSceneLogic)
        if (null != m_gameSceneLogic)
        {
            m_RunningScene = gameSceneID;

            m_gameSceneLogic.OnLeaveScene(bSameSceneRes, removeMainPlayer);
        }
        else
        {
#region 第一次加载
            m_RunningScene = gameSceneID;
            //第一次加载
            yield return SceneManager.LoadSceneAsync((int)SCENE_DEFINE.SCENE_GAMESCENE);
            GameObject objSceneLogic = GameObject.Find("GameSceneLogic");
            if (null == objSceneLogic)
            {
                LogModule.ErrorLog("SceneLogicObj not find in cur scene");
                m_bLoadingScene = false;
                yield break;
            }

            m_gameSceneLogic = objSceneLogic.GetComponent<GameSceneLogic>();
            if (null == m_gameSceneLogic)
            {
                LogModule.ErrorLog("SceneLogic not find in SceneLogic Obj");
                m_bLoadingScene = false;
                yield break;
            }
            GameObject objSceneLoading= GameObject.Find("Loading");
            if (objSceneLoading == null)
            {
                LogModule.ErrorLog("SceneLogicObj not find in LoadingRoot Obj");
                m_bLoadingScene = false;
                yield break;
            }
            m_LoadingRoot = objSceneLoading.GetComponent<LoadingController>();
            if (m_LoadingRoot == null)
            {
                LogModule.ErrorLog("LoadingRoot not find in m_LoadingRoot Obj");
                m_bLoadingScene = false;
                yield break;
            }
#endregion
        }

        if (loadType != LoadSceneType.QuickSwitch)
        {
            quickLoadCount = 0;
        }

        if (loadType == LoadSceneType.FullLoad)
        {
#region 全加载
            //这里头开启Loading
            m_LoadingRoot.ShowLoading(true, 0.1f, gameSceneID);
            yield return new WaitForEndOfFrame();
            UIManager.Instance().CloseUIFromChangeScene();
            if (null != storyManager && storyManager.StoryMode)
            {
                if (StoryFadeInOutController.Instance() != null && !StoryFadeInOutController.Instance().IsAutoClose())
                {
                    UIManager.CloseUI(UIInfo.FadeInOut);
                }
            }
            LoadCustomHead.Clear();

            AssetManager.ClearPool();
            m_LoadingRoot.SetLoadingPrecent(0.2f);
            //yield return new WaitForEndOfFrame();

            yield return SceneManager.LoadSceneAsync(GlobeVar.TRANSITION_SCENE_ID);
            yield return SceneManager.LoadSceneAsync(sceneClass.SceneResource);

            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                mainCamera.enabled = false;
            }
            //场景逻辑
            AttachSceneLogic(sceneClass);

            m_LoadingRoot.SetLoadingPrecent(0.3f);
            yield return new WaitForEndOfFrame();

            if (FirstEnterGame)
            {
                FirstEnterGame = false;
                if (!hasPreloadTab)
                {
                    yield return PreloadTab(f =>
                    {
                        if (null != m_gameSceneLogic)
                            m_LoadingRoot.SetLoadingPrecent(0.3f + f * 0.1f);
                    });
                }
            }
            
            //加载子场景，登陆场景特殊处理，默认不加载子场景，customloading里，需要选人时加载子场景
            if (sceneClass.Id != (int)SCENE_DEFINE.SCENE_LOGIN && sceneClass.DayResID != GlobeVar.INVALID_ID)
            {
                m_LoadingRoot.SetLoadingPrecent(0.4f);
                yield return SceneManager.LoadSceneAsync(sceneClass.DayResID, LoadSceneMode.Additive);
                //stopwatch.Stop();
                //UnityEngine.Debug.Log(stopwatch.ElapsedMilliseconds / 1000.0);
                //stopwatch.Start();
            }

            if (sceneClass.Id != (int)SCENE_DEFINE.SCENE_LOGIN && sceneClass.NightResID != GlobeVar.INVALID_ID)
            {
                m_LoadingRoot.SetLoadingPrecent(0.6f);
                yield return SceneManager.LoadSceneAsync(sceneClass.NightResID, LoadSceneMode.Additive);
                //stopwatch.Stop();
                //UnityEngine.Debug.Log(stopwatch.ElapsedMilliseconds / 1000.0);
                //stopwatch.Start();
            }

            m_LoadingRoot.SetLoadingPrecent(0.8f);

            
            {
                yield return AssetManager.UnloadUnusedAssetsAsync();
                yield return new WaitForEndOfFrame();
            }

            //if (Display.IsPostAA())
            //{
            //    Display.ApplyPostAAToCamera(mainCamera);
            //}

            if (mainCamera != null)
            {
                mainCamera.enabled = true;
            }
            m_LoadingRoot.SetLoadingPrecent(0.95f);
#endregion
        }
        else if (loadType == LoadSceneType.SameResLoad)
        {
            m_LoadingRoot.ShowLoading(true, 0.2f, gameSceneID);
            yield return new WaitForEndOfFrame();
            UIManager.Instance().CloseUIFromChangeScene();
            yield return AssetManager.UnloadUnusedAssetsAsync();

            //m_LoadingRoot.ShowLoading(true, 0.5f, gameSceneID);
            m_LoadingRoot.SetLoadingPrecent(0.5f);
            //场景逻辑
            AttachSceneLogic(sceneClass);
        }
        else
        {
            //场景逻辑
            UIManager.Instance().CloseUIFromChangeScene();
            quickLoadCount++;
            if (quickLoadCount > 8)
            {
                quickLoadCount = 0;
                yield return AssetManager.UnloadUnusedAssetsAsync();
                yield return new WaitForEndOfFrame();
            }
            AttachSceneLogic(sceneClass);
        }

        //stopwatch.Stop();
        //UnityEngine.Debug.Log(stopwatch.ElapsedMilliseconds / 1000.0);
        //stopwatch.Start();

        m_gameSceneLogic.BeforeEnterScene(bSameSceneRes);

        if (sceneClass.CameraCfgID != -1)
        {
            CameraManager.InitParam(sceneClass.CameraCfgID);
        }
        CameraManager.OnEnterScene();
        InputManager.OnEnterScene();
        
        yield return m_gameSceneLogic.CustomLoading();

        m_LoadingRoot.SetLoadingPrecent(1f);

        if (null != ConvenientNoticeController.Instance)
        {
            ConvenientNoticeController.Show();
        }

        //stopwatch.Stop();
        //UnityEngine.Debug.Log(stopwatch.ElapsedMilliseconds / 1000.0);
        //stopwatch.Start();
        
        m_gameSceneLogic.OnEnterScene(bSameSceneRes);
        Display.UpdateWaterEffect();

        //stopwatch.Stop();
        //UnityEngine.Debug.Log(stopwatch.ElapsedMilliseconds / 1000.0);
        //stopwatch.Start();

        if (CurScene != null)
        {
            CurScene.OnLoadGameUI();
        }

        yield return new WaitForEndOfFrame();

        if (shouldPlayQuickSwitchEffect)
        {
            if (CurScene != null && CurScene.EffectCamera != null)
            {
                yield return new WaitForEndOfFrame();
                CurScene.EffectCamera.SetScreenEffect(SCREEN_EFFECT.FLOW, true, 2.5f);
                //播放水波纹音效
                SoundManager.PlaySoundEffect(130);
                yield return new WaitForEndOfFrame();
            }
        }

        if (false == m_bStillShowLoading)
        {
            m_LoadingRoot.ShowLoading(false, 0.0f, gameSceneID);
        }
        m_bLoadingScene = false;
        OnLoadingSceneDone();
        stopwatch.Stop();
        Debug.Log("loading time:" + (stopwatch.ElapsedMilliseconds / 1000.0));
    }

    public static bool hasPreloadTab = false;

    public static IEnumerator PreloadTab(Action<float> cb)
    {
        hasPreloadTab = false;

        //yield return new WaitForEndOfFrame();
        //TableManager.InitTable();
        //yield return new WaitForEndOfFrame();

        TableManager.InitTable_SkillHit();
        cb(0.1f);
        yield return new WaitForEndOfFrame();

        TableManager.InitTable_SkillEx();
        cb(0.3f);
        yield return new WaitForEndOfFrame();

        TableManager.InitTable_Impact();
        cb(0.6f);
        yield return new WaitForEndOfFrame();

        TableManager.InitTable_Battle();
        cb(0.7f);
        yield return new WaitForEndOfFrame();

        TableManager.InitTable_Monster();
        cb(0.8f);
        yield return new WaitForEndOfFrame();

        TableManager.InitTable_RoleAttrEx();
        cb(0.9f);
        yield return new WaitForEndOfFrame();

        TableManager.InitTable_SkillParams();
        cb(1f);
        yield return new WaitForEndOfFrame();

        hasPreloadTab = true;
    }

    private static void AttachSceneLogic(Tab_SceneClass sceneClass)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        GameObject logicGo = GameObject.Find("Logic");
        if (logicGo == null)
        {
            LogModule.ErrorLog("SceneLogic not find in Logic Obj");
            logicGo = new GameObject("Logic");
        }

        Scene sceneLogic = logicGo.GetComponent<Scene>();
        bool shouldAddNew = false;
        if (sceneLogic == null)
        {
            shouldAddNew = true;
        }
        else
        {
            if (!IsSameLogic(sceneLogic.GetSceneType(), (SCENETYPE)sceneClass.Type))
            {
                Object.Destroy(sceneLogic);
                shouldAddNew = true;
            }
        }

        if (shouldAddNew)
        {
            if (sceneClass.Type == (int)SCENETYPE.LOBBY || sceneClass.Type == (int)SCENETYPE.STORY)
            {
                sceneLogic = logicGo.AddComponent<LobbyScene>();
            }
            else if (sceneClass.Type == (int)SCENETYPE.BATTLE)
            {
                sceneLogic = logicGo.AddComponent<BattleScene>();
            }
            else if (sceneClass.Type == (int)SCENETYPE.HELUO)
            {
                sceneLogic = logicGo.AddComponent<HeLuoScene>();
            }
            else if (sceneClass.Type == (int)SCENETYPE.STORY_CS)
            {
                sceneLogic = logicGo.AddComponent<StoryCopyScene>();
            }
            else if (sceneClass.Type == (int)SCENETYPE.FUNCTION)
            {
                sceneLogic = logicGo.AddComponent<FunctionScene>();
            }
            else if (sceneClass.Type == (int)SCENETYPE.ASYNCPVP)
            {
                sceneLogic = logicGo.AddComponent<AsyncPVPScene>();
            }
            else if (sceneClass.Type == (int)SCENETYPE.REALTIME)
            {
                sceneLogic = logicGo.AddComponent<RealTimeScene>();
            }
            else if (sceneClass.Type == (int)SCENETYPE.HOUSE)
            {
                sceneLogic = logicGo.AddComponent<HouseScene>();
            }
            else if (sceneClass.Type == (int)SCENETYPE.GUILDROOM)
            {
                sceneLogic = logicGo.AddComponent<GuildRoomScene>();
            }
            else
            {
                LogModule.ErrorLog("You should Add your scene.");
            }
            CurScene = sceneLogic;
        }
        else
        {
            CurScene = sceneLogic;
        }
    }

    public static bool IsSameLogic(SCENETYPE type0, SCENETYPE type1)
    {
        if (type0 != type1)
        {
            bool isLobby0 = type0 == SCENETYPE.LOBBY || type0 == SCENETYPE.STORY;
            bool isLobby1 = type1 == SCENETYPE.LOBBY || type1 == SCENETYPE.STORY;
            if (isLobby0 && isLobby1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    public static void LeaveBattle()
    {
        //清掉之前的记录
        replayingRecord = null;
        if (CurScene != null && CurScene is BattleScene)
        {
            AssetLoader.Instance.StartCoroutine(LeaveBattleCor());
        }
        //安全SDK设置为对战结束模式
#if (UNITY_IPHONE || UNITY_ANDROID)&&!UNITY_EDITOR
        TssSdk.TssSdkSetGameStatus(TssSdk.EGAMESTATUS.GAME_STATUS_END_PVP);
#endif
    }

    //当场景加载完成之后调用
    private static void OnLoadingSceneDone()
    {
        //处理剧情缓存
        if (null != storyManager.m_cachePacket)
        {
            StoryHandler.RetEnterStory(storyManager.m_cachePacket);
            storyManager.m_cachePacket = null;
        }

        if (CurScene != null)
        {
            //15分钟一次自动资源释放
            CurScene.StartAutoClearRes(15 * 60);
        }

        FPSMgr.CheckCurSceneHide();

        if (hasNewEnterBattle)
        {
            hasNewEnterBattle = false;
            //再进一次
            EnterBattle(CurBattlePacket);
        }
        else if (m_NewEnterSceneId != GlobeVar.INVALID_ID)
        {
            EnterGameScene(m_NewEnterSceneId, m_NewEnterSceneForceLoad, m_NewEnterSceneTeleport);
        }
        else if (HeLuoScene.m_LogoutRecoverCSInfo != null && m_CurScene != null && false == m_CurScene.IsBattle())
        {
            // 河洛大退恢复 且不是组队战斗中大退 直接回到河洛
            HeLuoScene.RecoverHeLuoRift();
        }
        else
        {
            if (CurScene != null)
            {
                CurScene.OnEnterSceneDone();
            }

            //正常切的场景
            FacebookManger.ShowFacebook();      //显示拍脸图

            //切场景完成后调用
            if (UIDelegate.AfterEnterScene != null)
            {
                UIDelegate.AfterEnterScene();
                UIDelegate.AfterEnterScene = null;
            }
        }      
        m_NewEnterSceneId = GlobeVar.INVALID_ID;
        m_NewEnterSceneForceLoad = false;
        m_NewEnterSceneTeleport = false;

        if (BeforeLoginSceneLogic.m_isShowCreateRoleWindow)
        {
            BeforeLoginSceneLogic.m_isShowCreateRoleWindow = false;
            if (LoginController.Instance() != null)
            {
                LoginController.Instance().EnterShowSelectRole();
            }
        }
        if (QuickChatController.Ins!=null)
        {
            QuickChatController.Ins.m_QuickIntercourseRoot.SetActive(false);
            QuickChatController.Ins.gameObject.SetActive(true);
            QuickChatController.Ins.m_QuickChatTweenPosition.ResetToBeginning();
        }
        else
        {
            UIManager.ShowUI(UIInfo.QuickIntercourseRoot, QuickChatController.OnHideUIDelFinish);
        }
    }

    static IEnumerator LeaveBattleCor()
    {
        int targetSceneId = LastSceneId;

        if (LastSceneId <= (int)SCENE_DEFINE.SCENE_GAMESCENE)
        {
            //回到主城
            EnterMainLobby();
        }
        else
        {
            // 河洛组队模式中大退可直接恢复 此时要返回副本
            if (HeLuoScene.m_LogoutRecoverCSInfo != null)
            {
                HeLuoScene.RecoverHeLuoRift();
            }
            else if (HeLuoRiftCenterMapController.m_IsQuit || HeLuoRiftCenterMapController.m_IsFinish ||
                HeLuoRiftMapController.m_IsQuit || HeLuoRiftMapController.m_IsFinish)
            {
                if (HeLuoRiftCenterMapController.m_IsQuit)
                {
                    HeLuoRiftCenterMapController.m_IsQuit = false;

                    HeLuoRiftCenterMapController.m_IsReturnHeLuo = true;
                    LeaveHeLuoScene();
                }
                else if (HeLuoRiftCenterMapController.m_IsFinish)
                {
                    HeLuoRiftCenterMapController.CleanUp();
                    LeaveHeLuoScene();
                }
                else if (HeLuoRiftMapController.m_IsQuit)
                {
                    HeLuoRiftMapController.m_IsQuit = false;

                    HeLuoRiftMapController.m_IsReturnHeLuo = true;
                    LeaveHeLuoScene();
                }
                else if (HeLuoRiftMapController.m_IsFinish)
                {
                    HeLuoRiftMapController.CleanUp();
                    LeaveHeLuoScene();
                }
            }
            else if (null != storyManager &&
                    null != storyManager.CurStory &&
                    null != storyManager.CurStory.EventList)
            {
                for (int i = 0; i < storyManager.CurStory.EventList.Count; ++i)
                {
                    StoryEvent _curEvent = storyManager.CurStory.EventList[i];
                    if (null != _curEvent)
                    {
                        //如果当前的事件是对话事件
                        if (_curEvent.GetStoryEventType() == STORY_EVENT.BATTLE)
                        {
                            //判断当前战斗是否胜利,只有胜利了才能继续下一步
                            bool succ = false;
                            if (m_PlayerDataPool != null && m_PlayerDataPool.m_LastBattleResult != null)
                            {
                                bool isWin = m_PlayerDataPool.m_LastBattleResult.isWin;

                                StoryEvent_Battle _battleEvent = _curEvent as StoryEvent_Battle;
                                int battleId = m_PlayerDataPool.m_LastBattleResult.battleTabId;

                                if (null != _battleEvent && _battleEvent.IsBattleCanLeaveEvent(battleId))
                                {
                                    if (isWin || _battleEvent.isBattleAlwaysSucc(battleId))
                                    {
                                        _battleEvent.Leave();
                                        succ = true;
                                    }
                                }
                            }

                            if (!succ)
                            {
                                _curEvent.Failed();
                            }
                        }
                    }
                }
            }
            else
            {
                //如果是回到lobby
                if (UIBattleEnd.s_EndFlag == UIBattleEnd.ENUM_ENDFLAG.BACKHOME)
                {
                    MapScene.ClearMapData();
                    UIBattleEnd.s_EndFlag = UIBattleEnd.ENUM_ENDFLAG.BACKHOME;
                    if (BattleController.CurBattleController == null
                        || BattleController.CurBattleController.BattleRetType != (int)BattleRetType.StoryLevel)
                    {
                        EnterMainLobby();
                    }
                }
                else
                {
                    //加载战斗前的场景
                    if (!_EnterGameScene(targetSceneId, false))
                    {
                        EnterMainLobby();
                    }
                    else
                    {
                        yield return LoadGameScene(targetSceneId, false);
                    }
                }
            }
        }

        while (m_bLoadingScene)
        {
            yield return null;
        }

        //清理当前战斗记录
        CurBattlePacket = null;

        if (m_PlayerDataPool != null && m_PlayerDataPool.m_LastBattleResult != null)
        {
            if (m_PlayerDataPool.m_LastBattleResult.retType == (int)BattleRetType.Adventure)
            {
                if (AdventureController._instance != null)
                {
                    AdventureController.isCloseAdventureRoot = true;
                }
            }
            else if (m_PlayerDataPool.m_LastBattleResult.retType == (int)BattleRetType.Arena)
            {
                UIManager.ShowUI(UIInfo.ArenaRoot);
            }
            else if (m_PlayerDataPool.m_LastBattleResult.retType == (int)BattleRetType.Melee)
            {
                UIManager.ShowUI(UIInfo.MeleeRoot);
            }
        }

    }

    public static _User m_CacheLoginDataUser = new _User();
    //保存上一个主城场景的信息
    public static void CacheLastLobbySceneInfo(bool bTeleport)
    {
        //当前场景为空，直接不记录
        if (null == m_CurScene)
        {
            return;
        }

        //当前场景必须为Lobby或多人场景并且不是剧情场景，则进行记录
        if ((m_CurScene.IsLobby() || m_CurScene.IsRealTimeScene()) && false == m_CurScene.IsStoryScene())
        {
            if (null != LoginData.user && null != ObjManager.MainPlayer)
            {
                m_CacheLoginDataUser.scene = RunningScene;
                m_CacheLoginDataUser.posX = ObjManager.MainPlayer.Position.x;
                m_CacheLoginDataUser.posZ = ObjManager.MainPlayer.Position.z;

                if (false == bTeleport)
                {
                    LoginData.user.scene = RunningScene;
                    LoginData.user.posX = ObjManager.MainPlayer.Position.x;
                    LoginData.user.posZ = ObjManager.MainPlayer.Position.z;
                }

                if (null == EnvManager ||
                    null == EnvManager.CurEvnTable)
                {
                    //记录当前场景默认环境
                    Tab_SceneClass _tabScenClass = TableManager.GetSceneClassByID(RunningScene, 0);
                    if (null != _tabScenClass)
                    {
                        m_CacheLoginDataUser.env = _tabScenClass.DefaultEnv;

                        if (false == bTeleport)
                        {
                            LoginData.user.env = _tabScenClass.DefaultEnv;
                        }
                    }
                }
                else
                {
                    m_CacheLoginDataUser.env = EnvManager.CurEvnTable.Id;

                    if (false == bTeleport)
                    {
                        LoginData.user.env = EnvManager.CurEvnTable.Id;
                    }
                }
            }
        }
    }

    public static bool IsLastLobbySceneValid()
    {
        if (null == LoginData.user)
        {
            return false;
        }

        if (LoginData.user.scene == GlobeVar.INVALID_ID)
        {
            return false;
        }

        return true;
    }

    //进入登录场景
    public static void EnterLoginScene()
    {
        if (m_bLoadingScene && m_RunningScene == (int)SCENE_DEFINE.SCENE_LOGIN)
        {
            return;
        }

        int nLoadSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //如果在Login直接返回
        if (nLoadSceneIndex == (int)SCENE_DEFINE.SCENE_LOGIN)
        {
            LogModule.ErrorLog("already in login scene");
            return;
        }

        Tab_SceneClass tabSceneClass = TableManager.GetSceneClassByID((int)SCENE_DEFINE.SCENE_LOGIN, 0);
        if (tabSceneClass == null)
        {
            LogModule.ErrorLog("login scene inexistence");
            return;
        }
        if (nLoadSceneIndex == (int)SCENE_DEFINE.SCENE_LAUNCH)
        {
            //RunningScene = (int)SCENE_DEFINE.SCENE_LOGIN;
            //SceneManager.LoadScene(tabSceneClass.SceneResource);
            //SceneManager.LoadScene(tabSceneClass.DayResID, LoadSceneMode.Additive);
            //SceneManager.LoadScene(tabSceneClass.NightResID, LoadSceneMode.Additive);
            EnterGameScene((int)SCENE_DEFINE.SCENE_LOGIN);

            return;
        }

        //剩下的一定是游戏场景
        if (null != m_gameSceneLogic)
        {
            ResetOnBackToLogin();
        }
        //m_RunningScene = (int)SCENE_DEFINE.SCENE_LOGIN;
        //SceneManager.LoadScene(tabSceneClass.SceneResource);
        //SceneManager.LoadScene(tabSceneClass.DayResID, LoadSceneMode.Additive);
        //SceneManager.LoadScene(tabSceneClass.NightResID, LoadSceneMode.Additive);
        EnterGameScene((int)SCENE_DEFINE.SCENE_LOGIN);
        m_IsLogin = true;
    }

    public static void ResetOnBackToLogin()
    {
        if (null != m_gameSceneLogic)
        {
            m_gameSceneLogic.OnDestroyScene();
            //返回Login界面的时候的清理放在这里
            PlayerDataPool = new PlayerData();

            m_storyManager.ClearStoryData(true);
            MapScene.ClearMapData();

            HeLuoRiftMapController.CleanUp();
            HeLuoRiftCenterMapController.CleanUp();
            StoryCopyScene.ClearData();
            Yard.ReturnToLogin();
            SocialCircleOptionList.CleanUp();
        }

        //清理引用计数
        AssetManager.refMgr.Clear();

        //m_gameSceneLogic = null;
        m_CacheLoginDataUser.scene = 0;

        if (CameraManager != null)
        {
            CameraManager.enabled = true;
        }

        if (null != SystemRollMgr)
        {
            SystemRollMgr.HideNotice();
        }

        if (BattleController.CurBattleController != null)
        {
            GameObject.DestroyImmediate(BattleController.CurBattleController);
        }

        Guild.ResetAllDelegates();
    }
    public static Action<BattleSide> offLineModeBattleFinish;

    public static GC_ENTER_BATTLE CurBattlePacket = null;

    //缓存客户端处于切场景状态时的服务器控制消息
    public static Queue<BattleMsg> EventsMsgCached = new Queue<BattleMsg>();
    public static Queue<BattleMsgWithTimestamp> ReplayEventsMsgCached;
    public static SyncBattleState syncCached;
    public static GC_SYNC_BATTLE_ROOM_INFO battleRoomInfoCached;
    private static bool hasNewEnterBattle = false;

    private static int m_NewEnterSceneId = GlobeVar.INVALID_ID;
    private static bool m_NewEnterSceneForceLoad = false;
    private static bool m_NewEnterSceneTeleport = false;

    public static void EnterBattle(GC_ENTER_BATTLE packet)
    {
        Tab_Battle battle = TableManager.GetBattleByID(packet.battleId, 0);
        if (battle == null)
        {
            LogModule.WarningLog("No Such Battle:" + packet.battleId);
            return;
        }
        CurBattlePacket = packet;
        EventsMsgCached.Clear();
        PlayerDataPool.m_LastBattleResult = null;

        //正在切场景，等切完后再处理这个消息
        if (LoadingScene)
        {
            hasNewEnterBattle = true;
            return;
        }
        //安全SDK设置为对战开始模式
#if (UNITY_IPHONE || UNITY_ANDROID) && !UNITY_EDITOR
       TssSdk.TssSdkSetGameStatus(TssSdk.EGAMESTATUS.GAME_STATUS_START_PVP);
#endif
        EnterGameScene(battle.SceneClassID);
    }

    public static void SaveReplay(string fileName, bool forceSave = false)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            return;
        }
        if (BattleController.CurBattleController != null)
        {
            BattleController.CurBattleController.SaveRecord(fileName, forceSave);
        }
    }

    public static BattleRecord replayingRecord { get; private set; }

    //重播一场战斗
    //录像文件名
    //是否只是使用初始化数据，手动操作战斗
    public static void ReplayBattle(string fileName, bool onlyInitData = false)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            return;
        }
        string path = BattleController.GetRecordPath(fileName);
        try
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            //bytes = CLZF2.Decompress(bytes);
            BattleRecord record = BattleRecord.Deserialize(bytes);
            replayingRecord = record;
            EnterBattle(new GC_ENTER_BATTLE()
            {
                battleId = record.battleId,
                battleSeed = record.seed,
                battleType = onlyInitData ? GC_ENTER_BATTLE.BattleType.Client : GC_ENTER_BATTLE.BattleType.Replay,
                userObjId = 1,
                initData = record.initData,
            });

            if (!onlyInitData && record.events != null)
            {
                ReplayEventsMsgCached = new Queue<BattleMsgWithTimestamp>();
                foreach (var battleMsg in record.events)
                {
                    ReplayEventsMsgCached.Enqueue(battleMsg);
                }
            }
        }
        catch (Exception)
        {
            Utils.CenterNotice("录像文件损坏");
        }
    }

    public static void DiffClientServerRecord(int seed)
    {
        BattleRecord rClient = ReadRecord(seed, false);
        BattleRecord rServer = ReadRecord(seed, true);
        if (null == rClient || null == rServer)
        {
            return;
        }

        //Debug.Log(rClient.result.winSide + "," + rServer.result.winSide);
        //Debug.Log(rClient.result.seed + "," + rServer.result.seed);
        //Debug.Log(rClient.cmdRecords.Count + "," + rServer.cmdRecords.Count);
        //Debug.Log(rClient.battleId + "," + rServer.battleId);

        byte[] bytes = BattleInitData.SerializeToBytes(rClient.initData);
        byte[] bytes2 = BattleInitData.SerializeToBytes(rServer.initData);
        //Debug.Log(bytes.Length + "," + bytes2.Length);
        for (int i = 0; i < bytes.Length; i++)
        {
            if (bytes[i] != bytes2[i])
            {
                //Debug.Log("wrong!");
                return;
            }
        }
        //Debug.Log("InitData Same");
        for (int i = 0; i < rClient.cmdRecords.Count; i++)
        {
            CmdRecord cmd1 = rClient.cmdRecords[i];
            CmdRecord cmd2 = rServer.cmdRecords[i];

            byte[] b1 = CmdRecord.SerializeToBytes(cmd1);
            byte[] b2 = CmdRecord.SerializeToBytes(cmd2);
            for (int j = 0; j < b1.Length; j++)
            {
                if (b1[j] != b2[j])
                {
                    //Debug.Log("cmd wrong!");
                    return;
                }
            }
        }
        //Debug.Log("Cmd Same");
    }

    public static BattleRecord ReadRecord(int seed, bool isServer)
    {
        try
        {
            string fileName = String.Format("{0}_{1}.txt", seed, isServer ? "server" : "client");
            byte[] bytes = System.IO.File.ReadAllBytes(fileName);
            BattleRecord record = BattleRecord.Deserialize(bytes);
            return record;
        }
        catch (Exception e)
        {
            //Debug.Log(e);
            return null;
        }
    }

    //断线处理
    public static void OnConnectLost()
    {
        if (RoleTouchController.Instance != null)
        {
            RoleTouchController.Instance.OnConnectLost();
        }

        // 单人场景和符灵交互时打断
        if (ObjManager.MainPlayer != null)
        {
            if (ObjManager.MainPlayer.IsRelaxWithCard)
            {
                ObjManager.MainPlayer.StopRelaxAnimWithCard();
            }

            if (ObjManager.MainPlayer.IsRelaxWithHero)
            {
                if (ObjManager.MainPlayer.RelaxWithTrans != null)
                {
                    Obj_Player withPlayer = ObjManager.MainPlayer.RelaxWithTrans.GetComponent<Obj_Player>();
                    if (withPlayer != null)
                    {
                        withPlayer.StopRelaxAnimWithHero();
                    }
                }

                ObjManager.MainPlayer.StopRelaxAnimWithHero();
            }
        }

        if (storyManager == null || !storyManager.StoryMode)
        {
            if (CurScene != null && CurScene.CutsceneManager != null)
            {
                CurScene.CutsceneManager.OnConnectLost();
            }

            if (UIManager.Instance() != null)
            {
                UIManager.Instance().HandleConnectLost();

                if (CurScene == null || CurScene.GetSceneType() != SCENETYPE.JISI)
                {
                    //如果UI被关闭 引导必须关闭 不然会卡死 (抽卡引导除外 抽卡引导不可中断 中断会影响到游戏的进程)
                    //如果抽卡引导时断线重连 会在其DrawCardController 中特殊处理
                    if (TutorialRoot.Instance() != null)
                    {
                        TutorialRoot.Instance().AutoClose();
                        TutorialRoot.TutorialHide();
                    }
                }
            }
        }
    }

    //断线重连成功
    public static void OnReconnect()
    {
        //如果当前正在剧情模式中 且剧情没有结束
        if (null != m_storyManager && true == m_storyManager.StoryMode && StoryFinishController.Instance == null)
        {
            // 剧情改成本地跑以后, 断线重连不会重新加载, 所以这里不会出现战斗中强制被拉回剧情的情况
            //if (CurScene != null && !(CurScene is BattleScene))
            {
                if (null != m_storyManager.CurStoryTable)
                {
                    //m_storyManager.Precondition.FirstLoadingFinished = false; //强制刷一遍加载, 恢复npc上次的位置
                    // 断线重连恢复现场, 不需要设置precondition
                    StoryHandler.ReqEnterStory(m_storyManager.CurStoryTable.LindID, false, GlobeVar.INVALID_ID, true);
                }
            }
        }
        if (CurScene != null)
        {
            CurScene.OnReconnect();
        }

        UIManager.CloseUI(UIInfo.DigitalKeyboard);

        //每日签到，断线重连时，弹出UI
        //UIDailySign.TryShow();
        FacebookManger.ShowFacebook();
    }

    public static void EnterMap()
    {
        ReqChangeScene((int)SCENE_DEFINE.SCENE_MAP);
    }

    public static void LeaveMap()
    {
        EnterMainLobby();
    }

    #endregion

    #region 服务器时间
    public static int ServerAnsiTime = 0;       // 服务器同步的时间
    public static int DValueTime = 0;           //服务器于客户端时间差

    #endregion

    #region gamemanager上的audiolistener处理
    public static void SetAudioListenerEnable(bool bEnable)
    {
        if (m_objGameManager == null)
            return;
        AudioListener al = m_objGameManager.GetComponent<AudioListener>();
        if (al != null)
        {
            al.enabled = bEnable;
        }
    }
    #endregion

    //加速档位

    public static float[] SpeedLv =
    {
        1.0f,
        1.4f,
        2.0f,
    };

    private static int m_GameSpeedLv;

    public static int GameSpeedLv
    {
        get { return m_GameSpeedLv; }
        set
        {
            if (value < 0 || value >= SpeedLv.Length)
            {
                LogModule.WarningLog("Not Support Speed Lv:" + value);
                return;
            }
            m_GameSpeedLv = value;
            SetGameSpeed(SpeedLv[value]);
        }
    }

    public static void SetGameSpeed(float speed)
    {
        Time.timeScale = speed;
    }

    public static float GetGameSpeed()
    {
        return Time.timeScale;
    }

    public static GameObject GetUIRoot()
    {
        if (m_gameSceneLogic == null)
        {
            return null;
        }

        return m_gameSceneLogic.uiRoot;
    }

    public static void ToggleEnv()
    {
        if (EnvManager == null)
        {
            return;
        }

        if (EnvManager.CurEvnTable != null)
        {
            Tab_SceneClass tab = TableManager.GetSceneClassByID(m_RunningScene, 0);
            if (tab == null)
            {
                return;
            }
            int target = m_EnvManager.CurEvnTable.EnvirType == (int)SCENE_ENVIRONMENT_TYPE.DAY
                ? tab.NightEvnID
                : tab.DayEnvID;
            EnvManager.Switch(target, false);
        }
        else
        {
            LogModule.WarningLog("No CurEnv,Can not switch Env");
        }
    }

    public static void EnterMainLobby()
    {
        if (m_bOffLineMode)
        {
            //ReqChangeScene((int)SCENE_DEFINE.SCENE_TINGYUAN);
            ReqChangeScene((int)SCENE_DEFINE.SCENE_LUOYANG);
        }
        else if (m_CacheLoginDataUser.scene != 0)
        {
            ReqChangeScene(m_CacheLoginDataUser.scene);
        }
        else if (null != LoginData.user)
        {
            if (PlayerDataPool.m_FirstReceiveStoryData && m_bOffLineMode == false)
            {
                PlayerDataPool.m_FirstReceiveStoryData = false;
                Tab_StoryLine pStoryLine = TableManager.GetStoryLineByID(GlobeVar._GameConfig.m_nNewPlayerStoryLine, 0);
                if (null != pStoryLine &&
                    GlobeVar._GameConfig.m_nNewPlayerStoryLine >= 0 &&
                    GlobeVar._GameConfig.m_nNewPlayerStoryLine < PlayerDataPool.m_nStoryLineStatusList.Length)
                {
                    StoryLineStatus _lineStatus = PlayerDataPool.m_nStoryLineStatusList[GlobeVar._GameConfig.m_nNewPlayerStoryLine];
                    if (null != _lineStatus && _lineStatus.m_nFin <= 0)
                    {
                        //主线如果未完成，则进入第一个剧情
                        if (null != storyManager)
                        {
                            storyManager.SetPrecondition(ENTER_STORY_MODE_SOURCE.NORMAL, GlobeVar.INVALID_ID, true);
                        }
                        StoryHandler.ReqEnterStory(GlobeVar._GameConfig.m_nNewPlayerStoryLine, false);
                        return;
                    }
                }
                Tab_StoryLine TutorialBattle2StoryLine = TableManager.GetStoryLineByID(GlobeVar.TutorialBattle2_StoryLineId, 0);
                if (null != TutorialBattle2StoryLine && TutorialManager.IsOpenTutorial &&
                    TutorialBattle2StoryLine.Id > 0 && TutorialBattle2StoryLine.Id < PlayerDataPool.m_nStoryLineStatusList.Length &&
                    TutorialManager.IsGroupFinish(TutorialGroup.DrawCard_1))
                {
                    StoryLineStatus tutorialBattle2LineStatus = PlayerDataPool.m_nStoryLineStatusList[TutorialBattle2StoryLine.Id];
                    if (null != tutorialBattle2LineStatus && tutorialBattle2LineStatus.m_nFin <= 0)
                    {
                        //如果第一次抽卡引导已经完成并且 序章没有完成 则进入序章
                        if (null != storyManager)
                        {
                            storyManager.SetPrecondition(ENTER_STORY_MODE_SOURCE.NORMAL, GlobeVar.INVALID_ID, true);
                        }
                        StoryHandler.ReqEnterStory(GlobeVar.TutorialBattle2_StoryLineId, false);
                        return;
                    }
                }

            }
            ReqChangeScene(LoginData.user.scene);

        }
    }

    public static void EnterJiSiScene()
    {
        ReqChangeScene((int)SCENE_DEFINE.SCENE_JISI);
    }

    public static void LeaveJiSiScene()
    {
        ReqChangeScene(LastSceneId);
    }

    public static void EnterHouseScene()
    {
        //庭院改为不是主城，而为私宅后，相应sv也需要进行处理
        //例如不在作为主城记录当前mapid
        //界面显示也要做相关调整
        //ReqChangeScene((int)SCENE_DEFINE.SCENE_TINGYUAN);
        if (LoginData.user != null)
        {
            Yard.SendEnter(LoginData.user.guid);
        }
    }

    public static void LeaveHouseScene()
    {
        //ReqChangeScene(LastSceneId);
        CG_CHANGE_CONTAINER_SCENE_PAK pak = new CG_CHANGE_CONTAINER_SCENE_PAK();
        pak.data.manager = 0;
        pak.data.scene = (int)SCENE_DEFINE.SCENE_LUOYANG;
        pak.SendPacket();
    }

    public static bool EnterGuildRoomScene()
    {
        if (!GlobeVar._GameConfig.m_bIsGuildOpen)
        {
            Utils.CenterNotice(5480);
            return false;
        }

        if (!GlobeVar._GameConfig.m_bIsGuildSceneOpen)
        {
            Utils.CenterNotice(5480);
            return false;
        }

        if (ObjManager.MainPlayer != null && ObjManager.MainPlayer.IsInRelaxAnim())
        {
            Utils.CenterNotice(6480);
            return false;
        }

        if (null != m_CurScene && m_CurScene.IsGuildRoomScene())
        {
            Utils.CenterNotice(8087);
            return false;
        }

        CG_GUILD_OPERATION_PAK pak = new CG_GUILD_OPERATION_PAK();
        pak.data.OpType = GuildOperation.GO_REQ_ENTER_GUILD_ROOM_SCENE;
        pak.SendPacket();
        return true;
    }

    public static void LeaveGuildRoomScene()
    {
        if (ObjManager.MainPlayer != null && ObjManager.MainPlayer.IsInRelaxAnim())
        {
            Utils.CenterNotice(6480);
            return;
        }

        EnterLuoYang();
    }


    private static void SendUpdateGuildSceneQuest()
    {
        CG_UPDATE_QUEST_PAK pak = new CG_UPDATE_QUEST_PAK();
        pak.data.questType = (int)UpdateQuestType.Tutorial;
        pak.data.updateType = (int)TutorialQuestClassId.EnterGuildScene;
        pak.SendPacket();
    }

    public static bool EnterGuildScene()
    {
        if (!GlobeVar._GameConfig.m_bIsGuildOpen)
        {
            Utils.CenterNotice(5480);
            return false;
        }

        if (ObjManager.MainPlayer != null && ObjManager.MainPlayer.IsInRelaxAnim())
        {
            Utils.CenterNotice(6480);
            return false;
        }

        // 等待进入抽卡房间中时不可切场景
        if (m_RunningScene == (int)SCENE_DEFINE.SCENE_JISI && DrawCardController.m_WaitRoomInfo != null)
        {
            return false;
        }

        if (null != m_CurScene && m_CurScene.IsGuild())
        {
            Utils.CenterNotice(8086);
            return false;
        }

        SendUpdateGuildSceneQuest();
        EnterGameScene((int)SCENE_DEFINE.SCENE_GUILD);
        return true;
    }

    //public static void EnterGuildScene(_GuildBrief brief)
    //{
    //    if (null == brief || null == brief.Location)
    //    {
    //        return;
    //    }

    //    Guild.ReservedBrief = brief;
    //    Guild.SceneState = GuildSceneState.REMOTE_BRIEF;

    //    Guild.CurrentProvince = brief.Location.Province;
    //    int sceneclass = Guild.GetSceneClassFromProvince(Guild.CurrentProvince);
    //    if (sceneclass != GlobeVar.INVALID_ID)
    //    {
    //        EnterGameScene(sceneclass);
    //    }
    //    else
    //    {
    //        LogModule.ErrorLog("cannot find scene class with province " + brief.Location.Province);
    //    }
    //}

    public static void LeaveGuildScene()
    {
        if (ObjManager.MainPlayer != null && ObjManager.MainPlayer.IsInRelaxAnim())
        {
            Utils.CenterNotice(6480);
            return;
        }

        EnterLuoYang();
    }

    private static bool m_HasChatRedPoint;
    public static bool HasChatRedPoint
    {
        get { return m_HasChatRedPoint; }
        set
        {
            m_HasChatRedPoint = value;

            if (QuickChatController.Ins != null)
            {
                QuickChatController.Ins.ActiveChatRedPoint();
            }

            if (UIGuildController.Instance() != null)
            {
                UIGuildController.Instance().ActiveChatRedPoint();
            }
        }
    }

    public static void EnterHeLuoRift_Side(int nKeyId, int nAreaId, List<int> chooseEventId, _HeLuoRiftRoomInfo roomInfo)
    {
        Tab_HeLuoRiftKey tKey = TableManager.GetHeLuoRiftKeyByID(nKeyId, 0);
        if (tKey == null)
        {
            return;
        }

        HeLuoScene.m_AreaId = nAreaId;
        HeLuoScene.m_ChooseEventId = chooseEventId;
        HeLuoScene.m_IsCenter = false;
        HeLuoScene.m_RoomInfo = roomInfo;

        if (tKey.HeLuoType == (int)HeLuoRiftType.HeTu)
        {
            ReqChangeScene((int)SCENE_DEFINE.SCENE_HETU_PREPARE);
        }
        else if (tKey.HeLuoType == (int)HeLuoRiftType.LuoShu)
        {
            ReqChangeScene((int)SCENE_DEFINE.SCENE_LUOSHU_PREPARE);
        }
    }

    public static void EnterHeLuoRift_Center(int nCopySceneType, List<_HeLuoRiftPoint> elitePoint, _HeLuoRiftRoomInfo roomInfo)
    {
        HeLuoScene.m_CopySceneType = nCopySceneType;
        HeLuoScene.m_ElitePoint = elitePoint;
        HeLuoScene.m_IsCenter = true;
        HeLuoScene.m_RoomInfo = roomInfo;

        if (nCopySceneType == (int)HeLuoRiftType.HeTu)
        {
            ReqChangeScene((int)SCENE_DEFINE.SCENE_HETU_PREPARE);
        }
        else if (nCopySceneType == (int)HeLuoRiftType.LuoShu)
        {
            ReqChangeScene((int)SCENE_DEFINE.SCENE_LUOSHU_PREPARE);
        }
    }

    public static void LeaveHeLuoScene()
    {
        EnterMap();
    }

    public static Obj_Player CreateMainPlayer()
    {
        //Debug.LogErrorFormat("CreateMainPlayer, Cur Has Main {0}", ObjManager.MainPlayer != null);

        //创建一个主角
        bool bMainPlayerVisible = true;

        Obj_Player obj = ObjManager.MainPlayer;
        bool forceReload = false;
        if (storyManager != null)
        {
            forceReload = obj != null && storyManager.ReloadMainPlayer;
            storyManager.ReloadMainPlayer = false;
        }

        if (null == obj || forceReload)
        {
            Obj_Init_Data _createData = new Obj_Init_Data();

            HeroData heroData = PlayerDataPool.PlayerHeroData;
            Hero curHero = heroData.GetCurHero();
            if (m_bOffLineMode ||
                null == PlayerDataPool.PlayerHeroData || 
                curHero == null)
            {
                _createData.m_nModelID = 0;
            }
            else
            {
                _createData.m_nModelID = curHero.GetCharModelId();
                _createData.m_nSoulWareModelID = curHero.GetSoulWareModelId();
                _createData.m_nCallCardId = PlayerDataPool.PlayerCardBag.GetCallCardId();
                _createData.m_nCallCardCurSkinId = PlayerDataPool.PlayerCardBag.GetCallCardCurSkinlId();
                _createData.m_nCallCardModelId = PlayerDataPool.PlayerCardBag.GetCallCardModelId();
                _createData.m_nCallCardDyeColorId = PlayerDataPool.PlayerCardBag.GetCallCardDyeColorId();
                _createData.m_nHeadIcon = PlayerDataPool.Icon;
                _createData.m_nLevel = heroData.Level;
                _createData.m_CurHeroId = heroData.CurHeroId;
                _createData.m_nRoleMaskModelId = PlayerDataPool.RoleMaskModelId;
                _createData.m_nDyeColorId = curHero.DyeColorID;
                _createData.m_OrnamentEffectTabId = curHero.OrnamentEffectId;
                _createData.m_nCallCardOrnamentEffectTabId = PlayerDataPool.PlayerCardBag.GetCallCardOrnamentEffect();
                _createData.m_HangPieceDic = curHero.HangPieceSlot.HangPieceDic;
            }

            //根据剧情配置，确认是否需要换装
            if (storyManager != null && storyManager.CurStoryTable != null)
            {
                bMainPlayerVisible = (storyManager.CurStoryTable.StoryModel != GlobeVar.INVALID_ID);

                // 剧情中不显示召唤符灵
                _createData.m_nCallCardId = GlobeVar.INVALID_ID;
                _createData.m_nCallCardCurSkinId = GlobeVar.INVALID_ID;
                _createData.m_nCallCardModelId = GlobeVar.INVALID_ID;
                _createData.m_nCallCardDyeColorId = GlobeVar.INVALID_ID;
                _createData.m_nCallCardOrnamentEffectTabId = GlobeVar.INVALID_ID;
                //剧情里去掉装饰特效
                _createData.m_OrnamentEffectTabId = GlobeVar.INVALID_ID;
                //新需求，在剧情里始终去掉染色
                //_createData.m_nDyeColorId = GlobeVar.INVALID_ID;

                //如果剧情对主角ID有特殊需求
                if (storyManager.CurStoryTable.StoryModel != GlobeVar.MAIN_PLAYER_SP_ID)
                {
                    //先判断是不是特殊ID
                    int nHeroID = Utils.GetSpecialHeroID(storyManager.CurStoryTable.StoryModel);
                    if (nHeroID != GlobeVar.INVALID_ID)
                    {
                        _createData.m_nRefixModelID = Utils.GetModelByHeroId(nHeroID);

                        var hero = GameManager.PlayerDataPool.PlayerHeroData.GetHero(nHeroID);
                        if (hero != null)
                        {
                            _createData.m_nDyeColorId = hero.DyeColorID;
                        }
                        else
                        {
                            _createData.m_nDyeColorId = GlobeVar.INVALID_ID;
                        }
                        if (hero!=null)
                        {
                            _createData.m_HangPieceDic = hero.HangPieceSlot.HangPieceDic;
                        }
                    }
                    else if (bMainPlayerVisible)
                    {
                        _createData.m_nRefixModelID = storyManager.CurStoryTable.StoryModel;

                        //有过特殊设置，去掉染色
                        _createData.m_nDyeColorId = GlobeVar.INVALID_ID;
                    }

                    _createData.m_nSoulWareModelID = GlobeVar.INVALID_ID; // 剧情模式不显示魂器
                    _createData.m_nHeadIcon = PlayerDataPool.Icon;
                    HeroData _hd = PlayerDataPool.PlayerHeroData;
                    if (null != _hd)
                    {
                        _createData.m_nLevel = _hd.Level;
                        _createData.m_CurHeroId = _hd.CurHeroId;
                    }
                }
            }
            else if (m_CurScene != null && m_CurScene.IsAsyncPVPScene())
            {
                _createData.m_nCallCardId = GlobeVar.INVALID_ID;
                _createData.m_nCallCardCurSkinId = GlobeVar.INVALID_ID;
                _createData.m_nCallCardModelId = GlobeVar.INVALID_ID;
                _createData.m_nCallCardDyeColorId = GlobeVar.INVALID_ID;
                _createData.m_nCallCardOrnamentEffectTabId = GlobeVar.INVALID_ID;
            }

            obj = ObjManager.CreateMainPlayer(_createData, forceReload);
        }
        else
        {
            //已经存在，无需重复创建
            ObjManager.RefixMainPlayerStoryPosition();

            if (storyManager == null || storyManager.CurStoryTable == null)
            {
                // 不是剧情场景
                // 主角可能从剧情中返回 模型可能不同 重新加载
                Hero curHero = PlayerDataPool.PlayerHeroData.GetCurHero();
                if (curHero != null)
                {
                    obj.UpdatePlayerModel(curHero.GetCharModelId(), curHero.GetSoulWareModelId(), curHero.DyeColorID,curHero.HangPieceSlot.HangPieceDic);
                }

                // 如果是进入异步PVP以外的场景 创建符灵
                if (m_CurScene != null && false == m_CurScene.IsAsyncPVPScene() && !m_CurScene.IsHouseScene())
                {
                    Card callCard = PlayerDataPool.PlayerCardBag.GetCallCard();
                    if (callCard != null)
                    {
                        obj.CreateCallCard(callCard.CardId, callCard.CurSkinId, callCard.GetCharModelId(), callCard.DyeColorID, callCard.OrnamentEffectId);
                    }
                }

                // 跟随法宝的guid已在离开场景删除所有obj时置回默认 如果有会在mainplayer的update中自动创建
            }
        }

        if (null != obj)
        {
            if (storyManager == null || !storyManager.StoryMode)
                obj.SetVisible(ObjVisibleLayer.Story, bMainPlayerVisible);
            CameraManager.FollowTarget = obj.ObjTransform;
            if (CurScene != null && CurScene.m_CullObjOccludeTarget != null)
            {
                CurScene.m_CullObjOccludeTarget.target = obj.transform;
            }

            //挂实时阴影
            if (CurScene != null)
            {
                CurScene.InitRealTimeShadow(obj.gameObject);
            }

            if (obj.ShadowTrans != null)
            {
                obj.ShadowTrans.gameObject.SetActive(Display.IsShowLowShadow());
            }
        }

        return obj;
    }

    public static void EnterAugurScene()
    {
        ReqChangeScene((int)SCENE_DEFINE.SCENE_AUGUR);
    }

    public static void LeaveAugurScene()
    {
        EnterMainLobby();
    }

    public static void EnterLuoYang()
    {
        CG_CHANGE_CONTAINER_SCENE_PAK pak = new CG_CHANGE_CONTAINER_SCENE_PAK();
        pak.data.manager = 0;
        pak.data.scene = (int)SCENE_DEFINE.SCENE_LUOYANG;
        pak.SendPacket();
    }

    public static void ReqChangeScene(int nSceneId)
    {
        if (m_bOffLineMode)
        {
            EnterGameScene(nSceneId);
        }
        else if (nSceneId == (int)SCENE_DEFINE.SCENE_MAP)
        {
            // 等待进入抽卡房间中时不可切场景
            if (m_RunningScene == (int)SCENE_DEFINE.SCENE_JISI && DrawCardController.m_WaitRoomInfo != null)
            {
                return;
            }

            if (m_bLoadingScene && m_RunningScene == (int)SCENE_DEFINE.SCENE_MAP)
            {
                return;
            }

            EnterGameScene(nSceneId);
        }
        else if (nSceneId == (int)SCENE_DEFINE.SCENE_HETU_PREPARE)
        {
            EnterGameScene(nSceneId);
        }
        else if (nSceneId == (int)SCENE_DEFINE.SCENE_LUOSHU_PREPARE)
        {
            EnterGameScene(nSceneId);
        }
        else
        {
            CG_CHANGE_SCENE_PAK pak = new CG_CHANGE_SCENE_PAK();
            pak.data.SceneId = nSceneId;
            pak.SendPacket();

            ReqTeleporting = true;
        }
    }

    public static void ReqTeleportScene(int nTeleportId)
    {
        if (m_bOffLineMode)
        {
            Tab_TeleportPoint teleportPoint = TableManager.GetTeleportPointByID(nTeleportId, 0);
            if (teleportPoint == null)
            {
                return;
            }

            EnterGameScene(teleportPoint.SceneID);
        }
        else
        {
            CG_REQ_TELEPORT_PAK pak = new CG_REQ_TELEPORT_PAK();
            pak.data.id = nTeleportId;
            pak.SendPacket();

            ReqTeleporting = true;
        }
    }
}
