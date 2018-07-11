

public class GameManager
{
    private GameManager() { }

    //////////////////////////////////////////////////////////////////////////
    //场景相关
    //////////////////////////////////////////////////////////////////////////

    private static int m_RunningScene = 0;
    public static int RunningScene
    { // 这个值是和sceneclass的id对应的
        get { return m_RunningScene; }
        set { m_RunningScene = value; }
    }

    private static GameSceneLogic m_gameSceneLogic = null;


    //当前场景
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
        get{return (float)UIRootAdapter.GetLogicHeight() / Screen.height;}
    }

    private static bool m_IsHorizontalScreen = true;
    public static bool IsHorizontalScreen
    {
        get { return m_IsHorizontalScreen; }
        set { m_IsHorizontalScreen = value; }
    }


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
    #region gamemanager上的audiolistener处理
    public static void SetAudioListenerEnable(bool bEnable);                     //打开或关闭声音
    #endregion

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


    //全局lua环境
    #region Lua
    public static LuaEnv L = null;
    [CSharpCallLua]
    public delegate void delReLua(string path);
    private static delReLua reluaFunc;

    public static void InitLua();
    public static void ReLua();


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
    { get { return m_ScreenOrientationManager; }}

    #region Init Game

    //初始化游戏管理器
    public static void InitGame();
    #endregion

//------------------------------------------------------------场景管理接口 begin-------------------------------------------------------------
    #region 游戏场景逻辑
    //进入一个游戏场景
    public static void EnterGameScene(int gameSceneID, bool forceLoad = false, bool bTeleport = false);
    //ReLoad部分表格
    public static IEnumerator PreloadTab(Action<float> cb);
    //离开战斗场景
    public static void LeaveBattle();

    public static _User m_CacheLoginDataUser = new _User();
    //保存上一个主城场景的信息
    public static void CacheLastLobbySceneInfo(bool bTeleport);

    //进入登录场景
    public static void EnterLoginScene();

    //切换到某个场景
    public static void ReqChangeScene(int nSceneId);
    //传送到某个场景的某个点
    public static void ReqTeleportScene(int nTeleportId);


    //进出河洛
    public static void EnterHeLuoRift_Side(int nKeyId, int nAreaId, List<int> chooseEventId, _HeLuoRiftRoomInfo roomInfo);
    public static void EnterHeLuoRift_Center(int nCopySceneType, List<_HeLuoRiftPoint> elitePoint, _HeLuoRiftRoomInfo roomInfo);
    public static void LeaveHeLuoScene();
    //进出抽奖
    public static void EnterAugurScene();
    public static void LeaveAugurScene();
    //返回大地图
    public static void EnterMap();
    public static void LeaveMap();
    //进入登陆场景
    public static void EnterLoginScene();
    //根据当前场景切换环境
    public static void ToggleEnv();   


//---------------------------------------------------------------------------场景管理接口 end-----------------------------------------------------



//---------------------------------------------------------------------------战斗相关  begin---------------------------------------------------------
    //缓存客户端处于切场景状态时的服务器控制消息
    //...一些变量

    //进入战斗场景
    public static void EnterBattle(GC_ENTER_BATTLE packet);

    //保存一场战斗记录
    public static void SaveReplay(string fileName, bool forceSave = false);
    public static BattleRecord replayingRecord { get; private set; }                  //实际记录战斗的数据

    //重播一场战斗
    //录像文件名    是否只是使用初始化数据，手动操作战斗
    public static void ReplayBattle(string fileName, bool onlyInitData = false);
    //比较客户端和服务器的记录
    public static void DiffClientServerRecord(int seed);
    //拿到战斗记录信息
    public static BattleRecord ReadRecord(int seed, bool isServer);

//-----------------------------------------------------------------------其他功能性接口------------------------------------------------------
    //断线处理
    public static void OnConnectLost();

    //断线重连成功
    public static void OnReconnect();

    #region 服务器时间
    public static int ServerAnsiTime = 0;       // 服务器同步的时间
    public static int DValueTime = 0;           //服务器于客户端时间差

    //加速档位
    public static float[] SpeedLv ={1.0f,1.4f,2.0f};
    private static int m_GameSpeedLv;
    //游戏世界的时间流速
    public static void SetGameSpeed(float speed);
    public static float GetGameSpeed();
    #endregion  


    private static void SendUpdateGuildSceneQuest();

    //创建主角
    public static Obj_Player CreateMainPlayer();


}
