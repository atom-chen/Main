

   //初始化所有游戏管理器
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
    };

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
    };

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
    };


//---------------------------------------------------------------------游戏场景管理 begin---------------------------------------------------------------------
    //根据场景类型 ADD一个类型的场景管理脚本
    private static void AttachSceneLogic(Tab_SceneClass sceneClass)
    {
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
            else
            {
                LogModule.ErrorLog("You should Add your scene.");
            }
            GameManager.CurScene = sceneLogic;
        }
        else
        {
            GameManager.CurScene = sceneLogic;
        }
    };

    //当场景加载完成之后调用
    private static void OnLoadingSceneDone()
    {
        //处理剧情缓存
        if (null != GameManager.storyManager.m_cachePacket)
        {
            StoryHandler.RetEnterStory(GameManager.storyManager.m_cachePacket);
            GameManager.storyManager.m_cachePacket = null;
        }

        if (GameManager.CurScene != null)
        {
            //15分钟一次自动资源释放
            CurScene.StartAutoClearRes(15 * 60);
        }

        FPSMgr.CheckCurSceneHide();

        if (hasNewEnterBattle)
        {
            hasNewEnterBattle = false;
            //再进一次
            GameManager.EnterBattle(CurBattlePacket);
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
    };

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
                m_CacheLoginDataUser.scene = GameManager.RunningScene;
                m_CacheLoginDataUser.posX = ObjManager.MainPlayer.Position.x;
                m_CacheLoginDataUser.posZ = ObjManager.MainPlayer.Position.z;

                if (false == bTeleport)
                {
                    LoginData.user.scene = GameManager.RunningScene;
                    LoginData.user.posX = ObjManager.MainPlayer.Position.x;
                    LoginData.user.posZ = ObjManager.MainPlayer.Position.z;
                }

                if (null == GameManager.EnvManager ||
                    null == GameManager.EnvManager.CurEvnTable)
                {
                    //记录当前场景默认环境
                    Tab_SceneClass _tabScenClass = TableManager.GetSceneClassByID(GameManager.RunningScene, 0);
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
                    m_CacheLoginDataUser.env = GameManager.EnvManager.CurEvnTable.Id;

                    if (false == bTeleport)
                    {
                        LoginData.user.env = GameManager.EnvManager.CurEvnTable.Id;
                    }
                }
            }
        }
    };
    //---------------------------------------------------------------------场景切换相关 begin---------------------------------------
    //返回大地图
    public static void EnterMap()
    {
        ReqChangeScene((int)SCENE_DEFINE.SCENE_MAP);
    };
    //离开地图 返回大厅
    public static void LeaveMap()
    {
        EnterMainLobby();
    };
        
    //进入战斗
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
    };

    //离开战斗
    static IEnumerator LeaveBattleCor()
    {
        int targetSceneId = LastSceneId;

        if (LastSceneId <= (int)SCENE_DEFINE.SCENE_LOADINGSCENE)
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

        while (m_bLoadingScene)
        {
            yield return null;
        }

        //判断当前剧情是不是可以被触发
        if (null != GameManager.storyManager &&
            null != GameManager.storyManager.CurStory &&
            null != GameManager.storyManager.CurStory.EventList)
        {
            for (int i = 0; i < GameManager.storyManager.CurStory.EventList.Count; ++i)
            {
                StoryEvent _curEvent = GameManager.storyManager.CurStory.EventList[i];
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
    };

    public static void EnterGameScene(int gameSceneID, bool forceLoad = false, bool bTeleport = false)
    {
        //检查要进入的是否为游戏场景
        if (gameSceneID <= (int)SCENE_DEFINE.SCENE_LOADINGSCENE)
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

        if (_EnterGameScene(gameSceneID, forceLoad))
        {
            //切换场景
            AssetLoader.Instance.StartCoroutine(LoadGameScene(gameSceneID, forceLoad));
        }
    };
    static bool _EnterGameScene(int gameSceneID, bool forceLoad)
    {
        if (m_bLoadingScene)
        {
            return false;
        }
        SCENE_DEFINE curActiveSceneType = (SCENE_DEFINE)SceneManager.GetActiveScene().buildIndex;
        if (curActiveSceneType == SCENE_DEFINE.SCENE_LAUNCH)
        {
            LogModule.ErrorLog("can not enter game from launch");
            return false;
        }
        //如果当前是战斗中，则不会重新记录上一个场景
        if (!(CurScene is BattleScene))
        {
            LastSceneId = GameManager.RunningScene; // 记录下最后一个场景id
        }
        //m_LastLoadSceneID用于Loading界面切换使用
        //需要记录战斗中的ID、所以重新记录一个
        m_LastLoadSceneID = GameManager.RunningScene;
        m_bLoadingScene = true;
        return true;
    }; 

    //进入任意一个游戏场景
    static IEnumerator LoadGameScene(int gameSceneID, bool forceLoad)
    {
        NPCMiscManager.OnEnterScene(gameSceneID);

        if (null != ConvenientNoticeController.Instance)
        {
            ConvenientNoticeController.Close();
        }
        UIManager.CloseUI(UIInfo.QuickIntercourseRoot);
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
            GameManager.PlayerDataPool.CheckAndStopRoleMask();
        }

        //是否是相同场景资源，如果相同则是位面逻辑
        bool bSameSceneRes = sceneClass.SceneResource == SceneManager.GetActiveScene().buildIndex;

        LoadSceneType loadType = LoadSceneType.FullLoad;

        if (!bSameSceneRes)
        {
            loadType = LoadSceneType.FullLoad; //正常切场景loading
        }
        else
        {
            if (forceLoad)
            {
                loadType = LoadSceneType.SameResLoad; //同场景loading
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
                        if (CurScene.GetSceneType() == SCENETYPE.ASYNCPVP || (SCENETYPE)sceneClass.Type == SCENETYPE.ASYNCPVP)
                        {
                            loadType = LoadSceneType.QuickSwitch;
                        }
                        else if (CurScene.GetSceneType() == SCENETYPE.BATTLE ||
                                 (SCENETYPE)sceneClass.Type == SCENETYPE.BATTLE)
                        {
                            loadType = LoadSceneType.QuickSwitch;
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

        if (bSameSceneRes && loadType == LoadSceneType.QuickSwitch)
        {
            quickLoadCount++;
            if (CurScene != null && CurScene.EffectCamera != null)
            {
                CurScene.EffectCamera.SetScreenEffect(SCREEN_EFFECT.FLOW, true, 2.5f);
            }
        }
        else
        {
            quickLoadCount = 0;
        }

        bool removeMainPlayer = loadType != LoadSceneType.QuickSwitch;
        if ((SCENETYPE)sceneClass.Type == SCENETYPE.BATTLE)
        {
            //进入战斗，始终要删除主角
            removeMainPlayer = true;
        }

        if (IsInGameScene() && null != m_gameSceneLogic)
        {
            m_RunningScene = gameSceneID;

            yield return AssetLoader.Instance.StartCoroutine(m_gameSceneLogic.OnLeaveScene(bSameSceneRes, removeMainPlayer));
        }
        else
        {
            m_RunningScene = gameSceneID;
            //第一次加载
            yield return SceneManager.LoadSceneAsync((int)SCENE_DEFINE.SCENE_LOADINGSCENE);
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
        }
        if (loadType == LoadSceneType.FullLoad)
        {
            //这里头开启Loading
            UIManager.Instance().CloseUIFromChangeScene();
            yield return new WaitForEndOfFrame();


            if (null != storyManager && storyManager.StoryMode)
            {
                if (StoryFadeInOutController.Instance() != null && !StoryFadeInOutController.Instance().IsAutoClose())
                {
                    UIManager.CloseUI(UIInfo.FadeInOut);
                }
            }


            if (null != m_gameSceneLogic)
                m_gameSceneLogic.ShowLoading(true, 0.1f, gameSceneID);
            LoadCustomHead.Clear();

            yield return new WaitForEndOfFrame();

            yield return SceneManager.LoadSceneAsync(GlobeVar.TRANSITION_SCENE_ID);
            AssetLoader.Instance.ReleaseBundlePool();
            //AssetManager.UnloadUnusedAssets();
            if (null != m_gameSceneLogic)
                m_gameSceneLogic.SetLoadingPrecent(0.3f);

            yield return SceneManager.LoadSceneAsync(sceneClass.SceneResource);

            Camera mainCamera = Camera.main;
            Camera.main.enabled = false;

            //场景逻辑
            AttachSceneLogic(sceneClass);

            if (FirstEnterGame)
            {
                FirstEnterGame = false;
                if (!hasPreloadTab)
                {
                    yield return PreloadTab(f =>
                    {
                        if (null != m_gameSceneLogic)
                            m_gameSceneLogic.SetLoadingPrecent(0.3f + f * 0.1f);
                    });
                }
            }

            if (sceneClass.DayResID != GlobeVar.INVALID_ID)
            {
                if (null != m_gameSceneLogic)
                    m_gameSceneLogic.SetLoadingPrecent(0.4f);
                yield return SceneManager.LoadSceneAsync(sceneClass.DayResID, LoadSceneMode.Additive);
            }

            if (sceneClass.NightResID != GlobeVar.INVALID_ID)
            {
                if (null != m_gameSceneLogic)
                    m_gameSceneLogic.SetLoadingPrecent(0.6f);
                yield return SceneManager.LoadSceneAsync(sceneClass.NightResID, LoadSceneMode.Additive);
            }

            if (null != m_gameSceneLogic)
                m_gameSceneLogic.SetLoadingPrecent(0.8f);

            yield return AssetManager.UnloadUnusedAssetsAsync();
            yield return new WaitForEndOfFrame();

            mainCamera.enabled = true;
            m_gameSceneLogic.SetLoadingPrecent(0.95f);
        }
        else if (loadType == LoadSceneType.SameResLoad)
        {
            UIManager.Instance().CloseUIFromChangeScene();
            yield return new WaitForEndOfFrame();
            m_gameSceneLogic.ShowLoading(true, 0.2f, gameSceneID);
            yield return AssetManager.UnloadUnusedAssetsAsync();
            m_gameSceneLogic.ShowLoading(true, 0.5f, gameSceneID);
            //场景逻辑
            AttachSceneLogic(sceneClass);
        }
        else
        {
            //场景逻辑
            UIManager.Instance().CloseUIFromChangeScene();
            yield return new WaitForEndOfFrame();
            AttachSceneLogic(sceneClass);
            if (quickLoadCount > 5)
            {
                quickLoadCount = 0;
                yield return AssetManager.UnloadUnusedAssetsAsync();
            }
        }

        m_gameSceneLogic.BeforeEnterScene(bSameSceneRes);
        yield return new WaitForEndOfFrame();
        yield return m_gameSceneLogic.CustomLoading();
        m_gameSceneLogic.SetLoadingPrecent(1f);

        if (null != ConvenientNoticeController.Instance)
        {
            ConvenientNoticeController.Show();
        }

        if (sceneClass.CameraCfgID != -1)
        {
            CameraManager.InitParam(sceneClass.CameraCfgID);
        }
        CameraManager.OnEnterScene();

        m_gameSceneLogic.OnEnterScene(bSameSceneRes);
        InputManager.OnEnterScene();
        Display.UpdateWaterEffect();

        yield return new WaitForEndOfFrame();
        if (false == m_bStillShowLoading)
        {
            m_gameSceneLogic.ShowLoading(false, 0.0f, gameSceneID);
        }

        if (CurScene != null)
        {
            CurScene.OnLoadGameUI();
            yield return null;
        }

        m_bLoadingScene = false;
        OnLoadingSceneDone();
    };

    //根据当前场景，切换环境
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
    };


    //回主城（洛阳）
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
                    GlobeVar._GameConfig.m_nNewPlayerStoryLine < GameManager.PlayerDataPool.m_nStoryLineStatusList.Length)
                {
                    StoryLineStatus _lineStatus = GameManager.PlayerDataPool.m_nStoryLineStatusList[GlobeVar._GameConfig.m_nNewPlayerStoryLine];
                    if (null != _lineStatus && _lineStatus.m_nFin <= 0)
                    {
                        //主线如果未完成，则进入第一个剧情
                        if (null != storyManager)
                        {
                            storyManager.SetPrecondition(ENTER_STORY_MODE_SOURCE.NORMAL, GlobeVar.INVALID_GUID, GlobeVar.INVALID_ID, true);
                        }
                        StoryHandler.ReqEnterStory(GlobeVar._GameConfig.m_nNewPlayerStoryLine, false);
                        return;
                    }
                }
            }
            ReqChangeScene(LoginData.user.scene);

        }
    };

    //切换到某个场景
    public static void ReqChangeScene(int nSceneId)
    {
        if (m_bOffLineMode)
        {
            EnterGameScene(nSceneId);
        }
        else if (nSceneId == (int)SCENE_DEFINE.SCENE_MAP
            || nSceneId == (int)SCENE_DEFINE.SCENE_TINGYUAN)
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
    };


    //传送到某个场景的某个点
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
    };

    private static void SendUpdateGuildSceneQuest()
    {
        CG_UPDATE_QUEST_PAK pak = new CG_UPDATE_QUEST_PAK();
        pak.data.questType = (int)UpdateQuestType.Tutorial;
        pak.data.updateType = (int)TutorialQuestClassId.EnterGuildScene;
        pak.SendPacket();
    };

    public static void EnterGuildScene()
    {
        // 等待进入抽卡房间中时不可切场景
        if (m_RunningScene == (int)SCENE_DEFINE.SCENE_JISI && DrawCardController.m_WaitRoomInfo != null)
        {
            return;
        }

        // sceneclass只是资源名, 会重复, 不能用作标识
        int sceneclass = GlobeVar.INVALID_ID;

        string currentProvince = Guild.GetDefaultProvince();
        if (!string.IsNullOrEmpty(currentProvince))
        {
            sceneclass = Guild.GetSceneClassFromProvince(currentProvince);
            // 表里没有当前区域名, 则直接使用"圈外"地图
            if (sceneclass == GlobeVar.INVALID_ID)
            {
                Tab_GuildLbs tabGuild = TableManager.GetGuildLbsByID(GlobeVar.GUILD_OUTOFRANGE_PROVINCE_ID, 0);
                if (tabGuild != null)
                {
                    currentProvince = tabGuild.Province;
                    sceneclass = tabGuild.SceneClass;
                }
            }
        }
        else
        {
            // 没开定位则随机一个
            var ret = Guild.RandProvince();
            sceneclass = ret.Value;
            currentProvince = ret.Key;
        }

        if (sceneclass == GlobeVar.INVALID_ID)
        {
            LogModule.ErrorLog("invalid guild params");
            return;
        }

        if (m_bLoadingScene && m_RunningScene == sceneclass)
        {
            return;
        }

        Guild.CurrentProvince = currentProvince;

        // 只有普通进入方式才请求区域列表
        Guild guild = GameManager.PlayerDataPool.GuildData;
        if (null != guild)
        {
            guild.SendReqLBSBriefList(Guild.CurrentProvince, false);
        }

        SendUpdateGuildSceneQuest();
        EnterGameScene(sceneclass);
    };

    //---------------------------------------------------------------------场景切换相关 end---------------------------------------

//---------------------------------------------------------------------游戏场景管理 end---------------------------------------------------------------------



//--------------------------------------------------------------------------战斗相关 begin--------------------------------------------------------------------
    //保存一场战斗记录
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
    };

    //录播一场战斗记录
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
    };

    //比较客户端和服务器的记录
    public static void DiffClientServerRecord(int seed)
    {
        BattleRecord rClient = ReadRecord(seed, false);
        BattleRecord rServer = ReadRecord(seed, true);
        if (null == rClient || null == rServer)
        {
            return;
        }

        byte[] bytes = BattleInitData.SerializeToBytes(rClient.initData);
        byte[] bytes2 = BattleInitData.SerializeToBytes(rServer.initData);
        for (int i = 0; i < bytes.Length; i++)
        {
            if (bytes[i] != bytes2[i])
            {
                return;
            }
        }
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
                    return;
                }
            }
        }
    };
//--------------------------------------------------------------------------战斗相关 end--------------------------------------------------------------------

//--------------------------------------------------------------------------游戏性相关 begin------------------------------------------------------------------
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
                ObjManager.MainPlayer.StopRelaxAnimWithHero();
            }
        }

        if (CurScene != null && CurScene.CutsceneManager != null)
        {
            CurScene.CutsceneManager.OnConnectLost();
        }
    };
    //断线重连调用
    public static void OnReconnect()
    {
        if (CurScene != null)
        {
            CurScene.OnReconnect();
        }

        UIManager.CloseUI(UIInfo.DigitalKeyboard);

        //每日签到，断线重连时，弹出UI
        FacebookManger.ShowFacebook();
    };

    //-----------------------------------------游戏世界的时间流速begin-----------------------------------------------
    public static float[] SpeedLv =                            //加速档位
    {
        1.0f,
        1.4f,
        2.0f,
    };
    private static int m_GameSpeedLv;
    //get set时间流速
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
    };
    public static float GetGameSpeed()
    {
        return Time.timeScale;
    };
    //-----------------------------------------游戏世界的时间流速end----------------------------------------------- 

//--------------------------------------------------游戏性相关 end---------------------------------------------------------------

//--------------------------------------------------------音频管理 begin---------------------------------------------------------
    public static void SetAudioListenerEnable(bool bEnable)
    {
        if (m_objGameManager == null)
            return;
        AudioListener al = m_objGameManager.GetComponent<AudioListener>();
        if (al != null)
        {
            al.enabled = bEnable;
        }
    };
//--------------------------------------------------------音频管理 end---------------------------------------------------------

//----------------------------------------------------------------------OBJ管理 begin---------------------------------------------
    //创建主角
    public static Obj_Player CreateMainPlayer()
    {
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

            HeroData heroData = GameManager.PlayerDataPool.PlayerHeroData;
            Hero curHero = heroData.GetCurHero();
            if (m_bOffLineMode || null == PlayerDataPool ||
                null == PlayerDataPool.PlayerHeroData || curHero == null)
            {
                _createData.m_nModelID = 0;
            }
            else
            {
                _createData.m_nModelID = curHero.GetCharModelId();
                _createData.m_nSoulWareModelID = curHero.GetSoulWareModelId();
                _createData.m_nCallCardId = GameManager.PlayerDataPool.PlayerCardBag.GetCallCardId();
                _createData.m_nCallCardModelId = GameManager.PlayerDataPool.PlayerCardBag.GetCallCardModelId();
                _createData.m_nCallCardDyeColorId = PlayerDataPool.PlayerCardBag.GetCallCardDyeColorId();
                _createData.m_nHeadIcon = GameManager.PlayerDataPool.Icon;
                _createData.m_nLevel = heroData.Level;
                _createData.m_CurHeroId = heroData.CurHeroId;
                _createData.m_nRoleMaskModelId = GameManager.PlayerDataPool.RoleMaskModelId;
                _createData.m_nDyeColorId = curHero.DyeColorID;
                _createData.m_OrnamentEffectTabId = curHero.OrnamentEffectId;
                _createData.m_nCallCardOrnamentEffectTabId = PlayerDataPool.PlayerCardBag.GetCallCardOrnamentEffect();
            }

            //根据剧情配置，确认是否需要换装
            if (GameManager.storyManager != null && GameManager.storyManager.CurStoryTable != null)
            {
                bMainPlayerVisible = (GameManager.storyManager.CurStoryTable.StoryModel != GlobeVar.INVALID_ID);

                // 剧情中不显示召唤符灵
                _createData.m_nCallCardId = GlobeVar.INVALID_ID;
                _createData.m_nCallCardModelId = GlobeVar.INVALID_ID;
                _createData.m_nCallCardDyeColorId = GlobeVar.INVALID_ID;
                _createData.m_nCallCardOrnamentEffectTabId = GlobeVar.INVALID_ID;
                //剧情里去掉装饰特效
                _createData.m_OrnamentEffectTabId = GlobeVar.INVALID_ID;
                //新需求，在剧情里始终去掉染色
                //_createData.m_nDyeColorId = GlobeVar.INVALID_ID;

                //如果剧情对主角ID有特殊需求
                //if (bMainPlayerVisible &&
                if (GameManager.storyManager.CurStoryTable.StoryModel != GlobeVar.MAIN_PLAYER_SP_ID)
                {
                    //先判断是不是特殊ID
                    int nHeroID = Utils.GetSpecialHeroID(GameManager.storyManager.CurStoryTable.StoryModel);
                    if (nHeroID != GlobeVar.INVALID_ID)
                    {
                        Tab_Hero _tabHero = TableManager.GetHeroByID(nHeroID, 0);
                        if (null != _tabHero)
                        {
                            Tab_RoleBaseAttr _tabRoleBase = TableManager.GetRoleBaseAttrByID(_tabHero.RoleBaseId, 0);
                            if (null != _tabRoleBase)
                            {
                                _createData.m_nRefixModelID = _tabRoleBase.CharModelID;
                            }
                        }

                        //获取染色数据
                        HeroData _heroData = PlayerDataPool.PlayerHeroData;
                        if (null != _heroData)
                        {
                            _createData.m_nDyeColorId = _heroData.GetHeroDyeColor(nHeroID);
                        }
                    }
                    else
                    {
                        _createData.m_nRefixModelID = GameManager.storyManager.CurStoryTable.StoryModel;

                        //有过特殊设置，去掉染色
                        _createData.m_nDyeColorId = GlobeVar.INVALID_ID;
                    }

                    _createData.m_nSoulWareModelID = GlobeVar.INVALID_ID; // 剧情模式不显示魂器
                    _createData.m_nHeadIcon = GameManager.PlayerDataPool.Icon;
                    _createData.m_nLevel = GameManager.PlayerDataPool.PlayerHeroData.Level;
                    _createData.m_CurHeroId = GameManager.PlayerDataPool.PlayerHeroData.CurHeroId;
                }
            }
            else if (m_CurScene != null && m_CurScene.IsAsyncPVPScene())
            {
                _createData.m_nCallCardId = GlobeVar.INVALID_ID;
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

            if (GameManager.storyManager == null || GameManager.storyManager.CurStoryTable == null)
            {
                // 不是剧情场景
                // 主角可能从剧情中返回 模型可能不同 重新加载
                Hero curHero = PlayerDataPool.PlayerHeroData.GetCurHero();
                if (curHero != null)
                {
                    obj.UpdatePlayerModel(curHero.GetCharModelId(), curHero.GetSoulWareModelId(), curHero.DyeColorID);
                }

                // 如果是进入异步PVP以外的场景 创建符灵
                if (m_CurScene != null && false == m_CurScene.IsAsyncPVPScene() && !m_CurScene.IsHouseScene())
                {
                    Card callCard = PlayerDataPool.PlayerCardBag.GetCallCard();
                    if (callCard != null)
                    {
                        obj.CreateCallCard(callCard.CardId, callCard.GetCharModelId(), callCard.DyeColorID, callCard.OrnamentEffectId);
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
            if (CurScene != null && GameManager.CurScene.m_CullObjOccludeTarget != null)
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
//----------------------------------------------------------------------OBJ管理 end---------------------------------------------    