/********************************************************************
	created:	2015/11/05
	created:	5:11:2015   13:52
	author:		WD
	
	purpose:	UIRoot下的UI管理器，创建不同的根节点来控制不同功能的UI
*********************************************************************/

using System;
using UnityEngine;
using System.Collections.Generic;
using Games.GlobeDefine;
using System.Collections;
using Games;
using Games.LogicObj;

public class UIManager : MonoBehaviour
{
    public class UILayer
    {
        public UIPathData.UIType type { get; private set; }
        public Transform root { get; private set; }
        public Dictionary<string, GameObject> activedUI { get; private set; }
        //同layer互斥
        public bool mutexSame = false;
        public bool closeChangeScene = false;
        public UIStack stack { get; set; }

        public UILayer(UIPathData.UIType type,string rootName)
        {
            activedUI = new Dictionary<string, GameObject>();
            this.type = type;
            Transform mgrRoot = UIManager.m_instance.transform;

            root = mgrRoot.Find(rootName);
            if (root == null)
            {
                GameObject obj = new GameObject();
                AssetManager.SetObjParent(obj, mgrRoot);
                obj.name = rootName;
                root = obj.transform;
            }
        }

        public GameObject GetUI(string name)
        {
            GameObject go = null;
            if (activedUI.TryGetValue(name, out go))
            {
                return go;
            }
            return null;
        }

        public bool ContainsUI(string name)
        {
            return activedUI.ContainsKey(name);
        }

        public void AddUI(string name, GameObject go)
        {
            activedUI[name] = go;
        }

        public bool RemoveUI(string name)
        {
            return activedUI.Remove(name);
        }

        public bool IsUIShow(string name = null)
        {
            foreach (var pair in activedUI)
            {
                if (string.IsNullOrEmpty(name) || pair.Key == name)
                {
                    if (pair.Value.activeInHierarchy)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void CloseAll(bool fromChangeScene = false)
        {
            var keys = activedUI.Keys;
            string[] cloesedUI = new string[keys.Count];
            keys.CopyTo(cloesedUI,0);
            foreach (var key in cloesedUI)
            {
                CloseUI(key, fromChangeScene);
            }
        }

        public void CloseUI(string name,bool fromChangeScene = false)
        {
            closeUIFunc(this, name,fromChangeScene);
        }

        public delegate void DelCloseUI(UILayer layer, string name, bool fromChangeScene = false);

        public DelCloseUI closeUIFunc;
    }

    //private static int m_sCloseUICount = 0;

    public delegate void OnOpenUIDelegate(bool bSuccess, object param);
    public delegate void OnLoadUIItemDelegate(GameObject resItem, object param1);

    public delegate void OnShowUIDel(UIPathData pathData);
    public static OnShowUIDel onShowUI;

    public delegate void OnUICloseDel(UIPathData pathData);
    public static OnUICloseDel onCloseUI;

    //private Transform m_baseUIRoot;      // 位于UI最底层，常驻场景，基础交互
    //private Transform m_popUIRoot;       // 位于UI上层，弹出式，互斥
    //private Transform m_storyUIRoot;     // 故事背景层
    //private Transform m_tipUIRoot;       // 位于UI顶层，弹出重要提示信息等
    //private Transform m_messageUIRoot;
    //private Transform m_deathUIRoot;
    //private Transform m_FloatUIRoot;    //  悬浮层，独立于所有UI

    private Dictionary<UIPathData.UIType,UILayer> m_AllUIlayer = new Dictionary<UIPathData.UIType, UILayer>();

    private Dictionary<string, GameObject> m_dicCacheUI = new Dictionary<string, GameObject>();

    private static List<UIPathData> m_loadingList = new List<UIPathData>();
    public static List<UIPathData> loadingList
    {
        get { return m_loadingList; }
    } 

    private static List<HashSet<UIPathData.UIType>> m_mutexList = new List<HashSet<UIPathData.UIType>>(); 

    private bool m_bPopMode = false;        //POP模式会隐藏baseui
    private bool m_bStoryMode = false;      //storymode会隐藏其他所有UI
    //private bool m_bDeathMode = false;      //deathmode会隐藏其他所有UI
    //private bool m_bHideMenuBar = false;    //是否是由MenuBar打开的弹出UI
    //public bool HideMenuBar
    //{
    //    get { return m_bHideMenuBar; }
    //    set { m_bHideMenuBar = value; }
    //}

    //private bool m_bHideCopySceneRoot = false; //是否是由活动界面打开的弹出UI
    //public bool HideCopySceneRoot
    //{
    //    get { return m_bHideCopySceneRoot; }
    //    set { m_bHideCopySceneRoot = value; }
    //}

    public UILayer GetUILayer(UIPathData.UIType type)
    {
        UILayer layer = null;
        if (m_AllUIlayer.TryGetValue(type, out layer))
        {
            return layer;
        }
        return null;
    }

    //保存一些状态
    private static UIManager m_instance;
    public static UIManager Instance()
    {
        return m_instance;
    }

    private static bool m_shouldGc = false;
    private static float m_gcTimer = 0;
    #region public static fun

    public static bool LoadItem(UIPathData pathData, OnLoadUIItemDelegate delLoadItem, object param = null)
    {
        if (null == m_instance)
        {
            LogModule.ErrorLog("game manager is not init");
            return false;
        }
        AssetManager.LoadUI(pathData, m_instance.LoadUIItemBundleFinish, delLoadItem, param);
        return true;
    }

    // 展示UI，根据类型不同，触发不同行为
    public static bool ShowUI(UIPathData pathData, OnOpenUIDelegate delOpenUI = null, object param = null,UIStack.StackType stackType = UIStack.StackType.None)
    {
        if (null == m_instance)
        {
            LogModule.ErrorLog("game manager is not init");
            return false;
        }

        UILayer layer = m_instance.GetUILayer(pathData.uiType);
                
        if (null == layer)
        {
            return false;
        }

        if (pathData.path == "RoleTouchRoot")
        {
            bool bRet = false;
            do
            {
                if (m_loadingList.Count > 0)
                {
                    break;
                }

                if (IsPopUIShow() || IsStoryUIShow() || IsMessageUIShowExcept("CenterNoticeEx", "RollNoticeRoot") ||
                    IsTipUIShow() || IsMessageBoxUIShow() || TutorialRoot.IsShow())
                {
                    break;
                }

                if (MainUI.Ins != null && MainUI.Ins.IsPlayingUnlockEffect)
                {
                    break;
                }

                if (MainUI.Ins != null && false == MainUI.Ins.TweenDone)
                {
                    break;
                }

                bRet = true;
            } while (false);

            if (bRet == false)
            {
                GameManager.ScreenOrientationManager.ForceLeaveRoleTouch();
                return false;
            }
        }

        if (onShowUI != null)
        {
            onShowUI(pathData);
        }

        if (pathData.uiType == UIPathData.UIType.TYPE_POP || pathData.uiType == UIPathData.UIType.TYPE_POPCENTER)
        {
            StopUnload();
        }

        if (layer.stack != null)
        {
            if (stackType == UIStack.StackType.PushAndPop)
            {
                layer.stack.SetUIEnterStack(pathData.path, true);
                layer.stack.SetUIPopStack(pathData.path, true);
            }
            else if (stackType == UIStack.StackType.PopOnly)
            {
                layer.stack.SetUIEnterStack(pathData.path, false);
                layer.stack.SetUIPopStack(pathData.path, true);
            }
            else
            {
                layer.stack.SetUIEnterStack(pathData.path, false);
                layer.stack.SetUIPopStack(pathData.path, false);
            }
        }


        GameObject cachedUI = null;
        if (m_instance.m_dicCacheUI.TryGetValue(pathData.path,out cachedUI))
        {
            layer.AddUI(pathData.path,cachedUI);
            m_instance.m_dicCacheUI.Remove(pathData.path);
        }

        GameObject ui = layer.GetUI(pathData.path);
        if (ui != null)
        {
            if(ui.activeSelf)
            {
                if (null != delOpenUI) delOpenUI(true, param);
            }
            else
            {
                ui.SetActive(true);
                m_instance.DoAddUI(pathData, ui, delOpenUI, param);
            }
            return true;
        }

        if(m_loadingList.Contains(pathData))
        {
            return false;
        }
        m_loadingList.Add(pathData);

        AssetManager.LoadUI(pathData, m_instance.LoadUIBundleFinish, delOpenUI, param);

        return true;
    }

    public static bool IsLoadingUI()
    {
        return m_loadingList.Count > 0;
    }

    public static bool IsLoadingUI(UIPathData info)
    {
        return m_loadingList.Contains(info);
    }

    //在开启全屏UI的时候，主摄像机停止拍摄
    private static BitArray m_MainCameraHideFlag = new BitArray((int)MAINCAMERA_HIDE_UI.NUM);
    //private static Coroutine m_CloseMainCamCoroutine = null;

    public static void SetMainCameraStatesOnUIChange(MAINCAMERA_HIDE_UI hideType, bool bOpenUI)
    {
        Camera mainCamera = Camera.main;
        if (null == mainCamera)
        {
            return;
        }

        //只处理几个全屏UI
        if (bOpenUI)
        {
            //打开UI的时候
            m_MainCameraHideFlag.Set((int)hideType, true);

            //如果主摄像机还在渲染，则关闭掉
            if (mainCamera.cullingMask != 0)
            {
                //if (m_CloseMainCamCoroutine != null)
                //{
                //AssetLoader.Instance.StopCoroutine(m_CloseMainCamCoroutine);
                //m_CloseMainCamCoroutine = null;
                //}
                //m_CloseMainCamCoroutine = AssetLoader.Instance.StartCoroutine(doCloseCam());

                mainCamera.cullingMask = 0;
            }
        }
        else
        {
            //if (m_CloseMainCamCoroutine != null)
            //{
                //AssetLoader.Instance.StopCoroutine(m_CloseMainCamCoroutine);
                //m_CloseMainCamCoroutine = null;
            //}
            //关闭UI的时候
            m_MainCameraHideFlag.Set((int)hideType, false);

            //判断是否还有界面打开
            bool bOpen = false;
            for (int i = 0; i < m_MainCameraHideFlag.Count; ++i)
            {
                bOpen |= m_MainCameraHideFlag[i];
            }

            //全部关闭了，恢复主摄像机CullingMask
            if (false == bOpen)
            {
                if (GameManager.CurScene != null)
                {
                    mainCamera.cullingMask = GameManager.CurScene.CameraMask;
                }
            }
        }
    }

    //在开启非全屏UI的时候，隐藏名字版
    private static BitArray m_HeadBoardHideFlag = new BitArray((int)HEADBOARD_HIDE_UI.NUM);
    public static void SetHeadBoardStatesOnUIChange(HEADBOARD_HIDE_UI hideType,bool bOpenUI)
    {
        if (GameManager.CurScene==null|| GameManager.CurScene.NameBoardRoot==null)
        {
            return;
        }
        if (bOpenUI)
        {
            m_HeadBoardHideFlag.Set((int)hideType,true);
            GameManager.CurScene.NameBoardRoot.gameObject.SetActive(false);
        }
        else
        {
            m_HeadBoardHideFlag.Set((int)hideType, false);
            //判断是否还有界面打开
            bool bOpen = false;
            for (int i = 0; i < m_HeadBoardHideFlag.Count; i++)
            {
                bOpen |= m_HeadBoardHideFlag[i];
            }
            //全部关闭了，恢复名字版
            if (bOpen==false)
            {
                GameManager.CurScene.NameBoardRoot.gameObject.SetActive(true);
            }
        }
    }
    private static IEnumerator doCloseCam()
    {
        yield return null;
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            yield break;
        }
        mainCamera.cullingMask = 0;
    }


    // 关闭UI，根据类型不同，触发不同行为
    public static void CloseUI(UIPathData pathData,bool fromChangeScene = false)
    {
        if (null == m_instance)
        {
            return;
        }

        UILayer layer = m_instance.GetUILayer(pathData.uiType);
        if (layer == null)
        {
            return;
        }

        loadingList.Remove(pathData);

        if (onCloseUI != null)
        {
            onCloseUI(pathData);
        }

        GameObject ui = layer.GetUI(pathData.path);
        if (ui != null)
        {
            ui.SendMessage("OnUIClose", SendMessageOptions.DontRequireReceiver);
        }

        layer.CloseUI(pathData.path,fromChangeScene);

        //switch (pathData.uiType)
        //{
        //    case UIPathData.UIType.TYPE_BASE:
        //        m_instance.CloseBaseUI(pathData.path);
        //        break;
        //    case UIPathData.UIType.TYPE_POP:
        //    case UIPathData.UIType.TYPE_POPCENTER:
        //        m_instance.ClosePopUI(pathData.path);
        //        break;
        //    case UIPathData.UIType.TYPE_STORY:
        //        m_instance.CloseStoryUI(pathData.path);
        //        break;
        //    default:
        //        UILayer layer = m_instance.GetUILayer(pathData.uiType);
        //        if (layer != null)
        //        {
        //            m_instance.TryDestroyUI(layer.activedUI, pathData.path);
        //        }
        //        break;
        //}
    }

    public static bool IsSubUIShow()
    {
        if (m_instance != null)
        {
            UILayer storyLayer = m_instance.GetUILayer(UIPathData.UIType.TYPE_STORY);
            if (storyLayer != null)
            {
                if (storyLayer.ContainsUI("PingBiReturn"))
                {
                    return false;
                }
            }
            if (m_instance.IsLayerUIShow(UIPathData.UIType.TYPE_POP))
            {
                return true;
            }
            if (m_instance.IsLayerUIShow(UIPathData.UIType.TYPE_POPCENTER))
            {
                return true;
            }
            if (m_instance.IsLayerUIShow(UIPathData.UIType.TYPE_STORY))
            {
                return true;
            }
            if (m_instance.IsLayerUIShow(UIPathData.UIType.TYPE_TIP))
            {
                return true;
            }
            //return (m_instance.m_dicPopUI.Count + m_instance.m_dicStoryUI.Count + m_instance.m_dicTipUI.Count) > 0 &&
            //    false == m_instance.m_dicStoryUI.ContainsKey("PingBiReturn");
        }
        return false;
    }
    #endregion

    void Awake()
    {
        m_dicCacheUI.Clear();
        m_AllUIlayer.Clear();
        m_instance = this;

        UIStack popStack = new UIStack();

        m_AllUIlayer.Add(UIPathData.UIType.TYPE_BASE, new UILayer(UIPathData.UIType.TYPE_BASE, "BaseUIRoot")
        {
            closeUIFunc = CloseBaseDestroyUI,
            closeChangeScene = true,
        });

        m_AllUIlayer.Add(UIPathData.UIType.TYPE_POP, new UILayer(UIPathData.UIType.TYPE_POP, "PopUIRoot")
        {
            closeUIFunc = ClosePopUI,
            mutexSame = true,
            closeChangeScene = true,
            stack = popStack,
        });
        m_AllUIlayer.Add(UIPathData.UIType.TYPE_POPCENTER, new UILayer(UIPathData.UIType.TYPE_POP, "PopUIRoot")
        {
            closeUIFunc = ClosePopUI,
            mutexSame = true,
            closeChangeScene = true,
            stack = popStack,
        });

        m_AllUIlayer.Add(UIPathData.UIType.TYPE_STORY, new UILayer(UIPathData.UIType.TYPE_STORY, "StoryUIRoot")
        {
            closeUIFunc = CloseStoryUI,
        });

        m_AllUIlayer.Add(UIPathData.UIType.TYPE_TIP, new UILayer(UIPathData.UIType.TYPE_TIP, "TipUIRoot")
        {
            closeUIFunc = CloseCommonUI,
            closeChangeScene = true,
        });
        m_AllUIlayer.Add(UIPathData.UIType.TYPE_MESSAGE, new UILayer(UIPathData.UIType.TYPE_MESSAGE, "MessageUIRoot")
        {
            closeUIFunc = CloseCommonUI,
            closeChangeScene = true,
        });
        m_AllUIlayer.Add(UIPathData.UIType.TYPE_MESSAGEBOX,new UILayer(UIPathData.UIType.TYPE_MESSAGEBOX, "MessageUIRoot")
        {
            closeUIFunc = CloseCommonUI,
            mutexSame = true,
            closeChangeScene = true,
        });
        m_AllUIlayer.Add(UIPathData.UIType.TYPE_FLOAT, new UILayer(UIPathData.UIType.TYPE_FLOAT, "FloatUIRoot")
        {
            closeUIFunc = CloseCommonUI,
        });
        m_AllUIlayer.Add(UIPathData.UIType.TYPE_BASE_DESTROY, new UILayer(UIPathData.UIType.TYPE_BASE_DESTROY, "BaseUIRoot")
        {
            closeUIFunc = CloseBaseDestroyUI,
            closeChangeScene = true,
        });

        //互斥注册，可以填多个
        RegisterMutex(UIPathData.UIType.TYPE_POP,UIPathData.UIType.TYPE_POPCENTER);
    }

    private void RegisterMutex(UIPathData.UIType type,params UIPathData.UIType[] types)
    {
        HashSet<UIPathData.UIType> set = new HashSet<UIPathData.UIType>();
        set.Add(type);
        foreach (var uiType in types)
        {
            set.Add(uiType);
        }
        m_mutexList.Add(set);
    }

    public bool IsMutex(UIPathData.UIType type1, UIPathData.UIType type2)
    {
        foreach (var set in m_mutexList)
        {
            if (set.Contains(type1) && set.Contains(type2))
            {
                return true;
            }
        }
        return false;
    }

    public HashSet<UIPathData.UIType> GetMutexLayers(UIPathData.UIType type)
    {
        HashSet<UIPathData.UIType> set = new HashSet<UIPathData.UIType>();
        foreach (var mutexSet in m_mutexList)
        {
            if (mutexSet.Contains(type))
            {
                foreach (var uiType in mutexSet)
                {
                    if (uiType != type)
                    {
                        set.Add(uiType);
                    }
                }
            }
        }
        return set;
    }

    void OnDestroy()
    {
        m_instance = null;
    }

    //void FixedUpdate()
    //{
    //    if (m_shouldGc)
    //    {
    //        m_gcTimer -= Time.fixedDeltaTime;
    //        if (m_gcTimer <= 0)
    //        {
    //            AssetManager.UnloadUnusedAssets();
    //            m_shouldGc = false;
    //        }
    //    }
    //}

    static void StartUnload(float delay = 1f)
    {
        m_shouldGc = true;
        m_gcTimer = delay;
    }

    static void StopUnload()
    {
        m_shouldGc = false;
        m_gcTimer = 0f;
    }

    void LoadUIBundleFinish(BundleTask task)
    {
        ProcessLoadUI(task, false);
    }

    void LoadUIItemBundleFinish(BundleTask task)
    {
        ProcessLoadUI(task, true);
    }

    void ProcessLoadUI(BundleTask task, bool isItem)
    {
        if (null == task) return;

        UIPathData uiData = task.GetParamByIndex(0) as UIPathData;
        if (null == uiData)
        {
            LogModule.ErrorLog("load ui error");
            return;
        }

        if(uiData.uiType != UIPathData.UIType.TYPE_ITEM && !m_loadingList.Contains(uiData))
        {
            return;
        }

        m_loadingList.Remove(uiData);
        GameObject curWindow = task.GetFinishObj() as GameObject;
        if (curWindow == null)
        {
            LogModule.ErrorLog("load ui:" + uiData.path + " failed");
            return;
        }
        object delOpenUI = task.GetParamByIndex(1);
        object param = task.GetParamByIndex(2);

        if(isItem)
        {
            DoLoadUIItem(uiData, curWindow, delOpenUI, param);
        }
        else
        {
            DoAddUI(uiData, curWindow, delOpenUI, param);
        }
    }

    private bool isAddingUI = false;
    private int delayPopVal = 0; //-1 false,0 none,1 true

    void DoAddUI(UIPathData uiData, GameObject curWindow, object fun, object param)
    {
        if (null != curWindow)
        {
            UILayer layer = GetUILayer(uiData.uiType);

            if (layer == null)
            {
                return;
            }

            isAddingUI = true;
            //检查互斥
            CheckCloseMutex(layer, uiData.path);

            //if (uiData.uiType == UIPathData.UIType.TYPE_POP || uiData.uiType == UIPathData.UIType.TYPE_POPCENTER)
            //{
            //    CheckCloseMutex(layer, uiData.path);
            //}
            //else if (uiData.uiType == UIPathData.UIType.TYPE_MESSAGEBOX)
            //{
            //    CheckCloseMutex(layer, uiData.path);
            //}

            GameObject ui = layer.GetUI(uiData.path);
            if (ui != null)
            {
                ui.SetActive(true);
            }
            else if (layer.root != null)
            {
                GameObject newWindow = AssetManager.InstantiateObjToParent(curWindow, layer.root, uiData.GetUIName());
                if (null != newWindow)
                {
                    newWindow.SetActive(true);
                    ui = newWindow;
                    layer.AddUI(uiData.path,newWindow);
                }
            }

            if (uiData.uiType == UIPathData.UIType.TYPE_POP)
            {
                SetPopMode(true);

                if (RoleTouchController.Instance != null)
                {
                    if (IsStoryUIShow() || IsMessageUIShowExcept("CenterNoticeEx", "RollNoticeRoot") || IsTipUIShow() ||
                        IsMessageBoxUIShow() || GameManager.LoadingScene || GameManager.ReqTeleporting || TutorialRoot.IsShow())
                    {
                        RoleTouchController.Instance.ForceCloseRoleTouch();
                    }
                }

                if (MainUI.Ins != null && MainUI.Ins.IsPlayingUnlockEffect)
                {
                    MainUI.Ins.CancelInvoke("FunctionEffectOver");
                    MainUI.Ins.IsPlayingUnlockEffect = false;
                }

                if (ObjManager.MainPlayer != null && ObjManager.MainPlayer.IsMoving)
                {
                    ObjManager.MainPlayer.StopMove(false);
                }
            }
            else
            {
                if (delayPopVal != 0)
                {
                    SetPopMode(delayPopVal == 1);
                }
            }

            delayPopVal = 0;

            if (uiData.uiType == UIPathData.UIType.TYPE_TIP)
            {
                if (ObjManager.MainPlayer != null && ObjManager.MainPlayer.IsMoving)
                {
                    ObjManager.MainPlayer.StopMove(false);
                }
            }

            if (uiData.uiType == UIPathData.UIType.TYPE_STORY)
            {
                SetStoryMode(true);

                if (RoleTouchController.Instance != null)
                {
                    RoleTouchController.Instance.ForceCloseRoleTouch();
                }
            }
            else if (uiData.uiType == UIPathData.UIType.TYPE_MESSAGE)
            {
                if (RoleTouchController.Instance != null)
                {
                    RoleTouchController.Instance.ForceCloseRoleTouch();
                }
            }

            if (ui != null)
            {
                ui.SendMessage("OnUIOpen", SendMessageOptions.DontRequireReceiver);
            }

            //else if (uiData.uiType == UIPathData.UIType.TYPE_DEATH)
            //{
            //    SetDeathMode(true);
            //}

            //if (relativeDic != null)
            //{
            //    if (relativeDic.ContainsKey(uiData.path))
            //    {
            //        relativeDic[uiData.path].SendMessage("OnUIOpen", SendMessageOptions.DontRequireReceiver);
            //    }
            //}
        }

        isAddingUI = false;


        if (null != fun)
        {
            OnOpenUIDelegate delOpenUI = fun as OnOpenUIDelegate;
            if (delOpenUI != null)
            {
                delOpenUI(curWindow != null, param);
            }
        }
    }

    void DoLoadUIItem(UIPathData uiData, GameObject curItem, object fun, object param)
    {
        if (null != fun)
        {
            OnLoadUIItemDelegate delLoadItem = fun as OnLoadUIItemDelegate;
            delLoadItem(curItem, param);
        }
    }

    void ClosePopUI(UILayer layer,string name,bool fromChangeScene)
    {
        if (TryDestroyUI(layer, name))
        {
            //检查一下是否要弹栈
            bool isRestoreThisLayer = false;
            if (layer.stack != null)
            {
                UIPathData pathData;
                UIPathData.m_DicUIName.TryGetValue(name, out pathData);
                if (pathData != null)
                {
                    isRestoreThisLayer = layer.stack.OnUIClose(pathData);
                }
            }
            if (isAddingUI)
            {
                //从一个pop，跳到另一个pop时，会一帧只能触发active、deactive
                delayPopVal = isRestoreThisLayer ? 1 : -1;
            }
            else
            {
                SetPopMode(isRestoreThisLayer);
            }
        }
    }

    void CloseStoryUI(UILayer layer, string name,bool fromChangeScene)
    {
        if (TryDestroyUI(layer, name))
        {
            SetStoryMode(false);
        }
    }

    void CloseBaseUI(UILayer layer, string name,bool fromChangeScene)
    {
        if (layer.activedUI.ContainsKey(name))
        {
            GameObject go = layer.activedUI[name];
            if (go != null)
            {
                go.SetActive(false);
            }
            else
            {
                layer.activedUI.Remove(name);
            }
        }
    }

    void CloseBaseDestroyUI(UILayer layer, string name, bool fromChangeScene)
    {
        if (fromChangeScene)
        {
            TryDestroyUI(layer, name);
        }
        else
        {
            GameObject go = layer.GetUI(name);
            if (go != null)
            {
                go.SetActive(false);
            }
        }
    }

    void CloseCommonUI(UILayer layer, string name,bool fromChangeScene)
    {
        TryDestroyUI(layer, name);
    }

    //void CloseDeathUI(string name)
    //{
    //    if ( TryDestroyUI(m_dicDeathUI, name) )
    //    {
    //        SetDeathMode(false);
    //    }
    //}


    //void DestroyUI(string name, GameObject obj)
    //{
    //    Destroy(obj);
    //    //BundleManager.OnUIDestroy(name);

    //}

    private void CheckCloseMutex(UILayer layer, string curName)
    {
        if (layer.mutexSame)
        {
            //关掉同级的
            CloseMutex(layer, curName);
        }
        //关掉互斥的其他层级
        HashSet<UIPathData.UIType> others = GetMutexLayers(layer.type);
        if (others != null)
        {
            foreach (var uiType in others)
            {
                UILayer otherLayer = GetUILayer(uiType);
                if (otherLayer != null)
                {
                    CloseMutex(otherLayer,curName);
                }
            }
        }
    }

    private void CloseMutex(UILayer layer,string curName)
    {
        if (layer == null)
        {
            return;
        }

        List<string> objToRemove = new List<string>();
        List<UIPathData> needCloseUI = new List<UIPathData>();

        if (layer.activedUI.Count <= 0)
        {
            return;
        }

        objToRemove.Clear();
        foreach (KeyValuePair<string, GameObject> objs in layer.activedUI)
        {
            if (curName == objs.Key)
            {
                continue;
            }

            UIPathData pathData = null;
            if (UIPathData.m_DicUIName.TryGetValue(objs.Key, out pathData))
            {
                if (layer.stack != null)
                {
                    //如果入栈的话，不会关闭，只会deactive掉
                    bool isEnterStack = layer.stack.IsEnterStack(objs.Key);
                    if (isEnterStack)
                    {
                        layer.stack.PushToStack(objs.Value, pathData);
                    }
                    else
                    {
                        //不入栈，但是弹栈，互斥顶掉的UI，不再触发弹栈
                        if (layer.stack.IsWillPopStack(pathData))
                        {
                            layer.stack.SetUIPopStack(objs.Key, false);
                        }
                        needCloseUI.Add(pathData);
                    }
                }
                else
                {
                    needCloseUI.Add(pathData);
                }
            }
            else
            {
                objToRemove.Add(objs.Key);
                m_dicCacheUI[objs.Key] = objs.Value;
            }
            //objToRemove.Add(objs.Key);
            //if (false == UIPathData.m_DicUIName.ContainsKey(objs.Key))
            //{
            //    m_dicCacheUI.Add(objs.Key, objs.Value);
            //}
        }

        foreach (var name in objToRemove)
        {
            layer.CloseUI(name);
        }

        foreach (var uiPathData in needCloseUI)
        {
            UIManager.CloseUI(uiPathData);
        }
    }

    public static void ClearStack(UIPathData.UIType type = UIPathData.UIType.TYPE_POP)
    {
        if (m_instance == null)
        {
            return;
        }
        UILayer layer = m_instance.GetUILayer(type);
        if (layer == null)
        {
            return;
        }
        if (layer.stack != null)
        {
            layer.stack.ClearStack();
        }
    }

    public static UIPathData PeekStack(UIPathData.UIType type = UIPathData.UIType.TYPE_POP)
    {
        if (m_instance == null)
        {
            return null;
        }
        UILayer layer = m_instance.GetUILayer(type);
        if (layer == null)
        {
            return null;
        }
        return layer.stack.PeekStackUI();
    }

    //private void OnLoadNewPopUI(UILayer layer, string curName)
    //{
    //    if (layer == null)
    //    {
    //        return;
    //    }
        
    //    List<string> objToRemove = new List<string>();

    //    if (layer.activedUI.Count > 0)
    //    {
    //        objToRemove.Clear();
    //        foreach (KeyValuePair<string, GameObject> objs in layer.activedUI)
    //        {
    //            if (curName == objs.Key)
    //            {
    //                continue;
    //            }
    //            //if (objs.Key == "MenuBarRoot")
    //            //{
    //            //    m_bHideMenuBar = true;
    //            //}

    //            objToRemove.Add(objs.Key);
    //            if (false == UIPathData.m_DicUIName.ContainsKey(objs.Key))
    //            {
    //                m_dicCacheUI.Add(objs.Key, objs.Value);
    //            }
    //        }

    //        foreach (var name in objToRemove)
    //        {
    //            layer.CloseUI(name);
    //        }
    //    }
    //}

    private bool TryDestroyUI(UILayer layer, string curName)
    {
        m_loadingList.RemoveAll(data => data.path == curName);

        if (layer == null)
        {
            return false;
        }

        GameObject ui = layer.GetUI(curName);
        if (ui == null)
        {
            return false;
        }

        UIPathData pathData = null;
        UIPathData.m_DicUIName.TryGetValue(curName, out pathData);

        if (pathData != null && !pathData.isDestroyOnUnload)
        {
            ui.SetActive(false);
            m_dicCacheUI.Add(curName, ui);
        }
        else
        {
            Destroy(ui);
        }

        layer.RemoveUI(curName);

        StartUnload();

        return true;
    }

    private void SetStoryMode(bool bEnter)
    {
        UILayer layer = GetUILayer(UIPathData.UIType.TYPE_STORY);
        if (bEnter == false && null != layer && layer.activedUI.Count > 0)
        {
            return;
        }

        m_bStoryMode = bEnter;
        if(bEnter)
        {
            SetLayerActive(UIPathData.UIType.TYPE_BASE, false);
            SetLayerActive(UIPathData.UIType.TYPE_TIP,false);
            SetLayerActive(UIPathData.UIType.TYPE_POP,false);
            SetLayerActive(UIPathData.UIType.TYPE_MESSAGE,false);
            SetLayerActive(UIPathData.UIType.TYPE_STORY,true);
        }
        else
        {
            SetLayerActive(UIPathData.UIType.TYPE_BASE, !m_bPopMode);
            SetLayerActive(UIPathData.UIType.TYPE_TIP, true);
            SetLayerActive(UIPathData.UIType.TYPE_POP, true);
            SetLayerActive(UIPathData.UIType.TYPE_MESSAGE, true);
            SetLayerActive(UIPathData.UIType.TYPE_STORY, true);
        }
    }

    private void SetPopMode(bool bEnter)
    {
        m_bPopMode = bEnter;

        if (bEnter)
        {
            SetLayerActive(UIPathData.UIType.TYPE_BASE, false);
            SetLayerActive(UIPathData.UIType.TYPE_TIP, !m_bStoryMode);
            SetLayerActive(UIPathData.UIType.TYPE_POP, !m_bStoryMode);
            SetLayerActive(UIPathData.UIType.TYPE_MESSAGE, !m_bStoryMode);
            SetLayerActive(UIPathData.UIType.TYPE_STORY, true);
        }
        else
        {
            SetLayerActive(UIPathData.UIType.TYPE_BASE, !m_bStoryMode);
            SetLayerActive(UIPathData.UIType.TYPE_TIP, !m_bStoryMode);
            SetLayerActive(UIPathData.UIType.TYPE_POP, !m_bStoryMode);
            SetLayerActive(UIPathData.UIType.TYPE_MESSAGE, !m_bStoryMode);
            SetLayerActive(UIPathData.UIType.TYPE_STORY, true);
        }
    }

    //private void SetDeathMode(bool bEnter)
    //{
    //    m_bDeathMode = bEnter;

    //    if (bEnter)
    //    {
    //        if (null != m_baseUIRoot)
    //            m_baseUIRoot.gameObject.SetActive(false);
    //        if (null != m_tipUIRoot)
    //            m_tipUIRoot.gameObject.SetActive(false);
    //        if (null != m_popUIRoot)
    //            m_popUIRoot.gameObject.SetActive(false);
    //        if (null != m_messageUIRoot)
    //            m_messageUIRoot.gameObject.SetActive(true);
    //        if (null != m_storyUIRoot)
    //            m_storyUIRoot.gameObject.SetActive(false);
    //        if (null != m_deathUIRoot)
    //            m_deathUIRoot.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        if (null != m_baseUIRoot)
    //            m_baseUIRoot.gameObject.SetActive(!m_bPopMode && !m_bStoryMode);
    //        if (null != m_tipUIRoot)
    //            m_tipUIRoot.gameObject.SetActive(!m_bStoryMode);
    //        if (null != m_popUIRoot)
    //            m_popUIRoot.gameObject.SetActive(!m_bStoryMode);
    //        if (null != m_messageUIRoot)
    //            m_messageUIRoot.gameObject.SetActive(!m_bStoryMode);
    //        if (null != m_storyUIRoot)
    //            m_storyUIRoot.gameObject.SetActive(true);
    //        if (null != m_deathUIRoot)
    //            m_deathUIRoot.gameObject.SetActive(true);
    //    }
    //}

    public void SetVerticalMode(bool bEnter)
    {
        SetLayerActive(UIPathData.UIType.TYPE_BASE, !bEnter);
        SetLayerActive(UIPathData.UIType.TYPE_TIP, bEnter);
        SetLayerActive(UIPathData.UIType.TYPE_POP, bEnter);
        SetLayerActive(UIPathData.UIType.TYPE_MESSAGE, bEnter);
        SetLayerActive(UIPathData.UIType.TYPE_STORY, !bEnter);
    }

    //public void SetRootActive(bool bActive)
    //{
    //    if (bActive)
    //    {
    //        if (null != m_baseUIRoot)
    //            m_baseUIRoot.gameObject.SetActive(!(m_bPopMode || m_bStoryMode));
    //        if (null != m_tipUIRoot)
    //            m_tipUIRoot.gameObject.SetActive(!m_bStoryMode);
    //        //if (null != m_popUIRoot)
    //            //m_popUIRoot.gameObject.SetActive(!m_bStoryMode);
    //        if (null != m_messageUIRoot)
    //            m_messageUIRoot.gameObject.SetActive(!m_bStoryMode);
    //        if (null != m_storyUIRoot)
    //            m_storyUIRoot.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        if (null != m_baseUIRoot)
    //            m_baseUIRoot.gameObject.SetActive(false);
    //        if (null != m_tipUIRoot)
    //            m_tipUIRoot.gameObject.SetActive(false);
    //        //if (null != m_popUIRoot)
    //            //m_popUIRoot.gameObject.SetActive(false);
    //        if (null != m_messageUIRoot)
    //            m_messageUIRoot.gameObject.SetActive(false);
    //        if (null != m_storyUIRoot)
    //            m_storyUIRoot.gameObject.SetActive(false);
    //    }
    //}

    public void CloseUIFromChangeScene()
    {
        foreach (var pair in m_AllUIlayer)
        {
            if (pair.Value.closeChangeScene)
            {
                CloseUIFromChangeScene(pair.Value);
            }
        }
        if (loadingList.Count > 0)
        {
            List<UIPathData> needCloseUI = null;
            foreach (var loadingUI in loadingList)
            {
                UILayer layer = GetUILayer(loadingUI.uiType);
                if (null != layer && layer.closeChangeScene)
                {
                    if (needCloseUI == null)
                    {
                        needCloseUI = new List<UIPathData>();
                    }
                    needCloseUI.Add(loadingUI);
                }
            }
            if (needCloseUI != null)
            {
                foreach (var uiPathData in needCloseUI)
                {
                    CloseUI(uiPathData);
                }
            }
        }
    }

    private void CloseUIFromChangeScene(UILayer layer)
    {
        if (layer == null)
        {
            return;
        }
        List<UIPathData> needCloseUI = new List<UIPathData>();
        List<string> popKeys = new List<string>();

        foreach (string curPopUI in layer.activedUI.Keys)
        {
            UIPathData pathData;
            if (UIPathData.m_DicUIName.TryGetValue(curPopUI, out pathData))
            {
                if (!pathData.isCloseChangeScene)
                {
                    continue;
                }
                else
                {
                    needCloseUI.Add(pathData);
                }
            }
            else
            {
                popKeys.Add(curPopUI);
            }
        }

        foreach (var popKey in popKeys)
        {
            layer.CloseUI(popKey);
        }

        foreach (var uiPathData in needCloseUI)
        {
            CloseUI(uiPathData,true);
        }
    }

    public void CloseAllMessageUI()
    {
        CloseLayerUI(UIPathData.UIType.TYPE_MESSAGE);
        CloseLayerUI(UIPathData.UIType.TYPE_MESSAGEBOX);
    }

    public void CloseAllTipUI()
    {
        CloseLayerUI(UIPathData.UIType.TYPE_TIP);
    }

    public void CloseAllStoryUI()
    {
        CloseLayerUI(UIPathData.UIType.TYPE_STORY);
    }

    public void CloseLayerUI(UIPathData.UIType type)
    {
        UILayer layer = GetUILayer(type);
        if (layer != null)
        {
            layer.CloseAll();
        }
    }

    public static bool IsPopUIShow()
    {
        if (m_instance == null)
        {
            return false;
        }
        return m_instance.IsLayerUIShow(UIPathData.UIType.TYPE_POP) || m_instance.IsLayerUIShow(UIPathData.UIType.TYPE_POPCENTER);
    }

    public static bool IsStoryUIShow()
    {
        if (m_instance == null)
        {
            return false;
        }
        return m_instance.IsLayerUIShow(UIPathData.UIType.TYPE_STORY);
    }

    public static bool IsMessageUIShow()
    {
        if (m_instance == null)
        {
            return false;
        }
        return m_instance.IsLayerUIShow(UIPathData.UIType.TYPE_MESSAGE);
    }

    public static bool IsTipUIShow()
    {
        if (m_instance == null)
        {
            return false;
        }
        return m_instance.IsLayerUIShow(UIPathData.UIType.TYPE_TIP);
    }

    public static bool IsMessageBoxUIShow()
    {
        if (m_instance == null)
        {
            return false;
        }
        return m_instance.IsLayerUIShow(UIPathData.UIType.TYPE_MESSAGEBOX);
    }

    public bool IsLayerUIShow(UIPathData.UIType type)
    {
        UILayer layer = m_instance.GetUILayer(type);
        if (layer == null)
        {
            return false;
        }
        return layer.IsUIShow();
    }

    public void SetLayerActive(UIPathData.UIType type,bool active)
    {
        UILayer layer = m_instance.GetUILayer(type);
        if (layer != null && layer.root != null)
        {
            if (layer.root.gameObject.activeSelf != active)
            {
                layer.root.gameObject.SetActive(active);
            }
        }
    }

    public static bool IsMessageUIShowExcept(params string[] szUIName)
    {
        UILayer layer = m_instance.GetUILayer(UIPathData.UIType.TYPE_MESSAGE);
        if (layer == null)
        {
            return false;
        }
        foreach (KeyValuePair<string, GameObject> pair in layer.activedUI)
        {
            if (pair.Value.activeInHierarchy)
            {
                bool bContain = false;
                for (int i = 0; i < szUIName.Length; i++)
                {
                    if (szUIName[i] == pair.Key)
                    {
                        bContain = true;
                        break;
                    }
                }

                if (false == bContain)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static GameObject PlayEffect(string effName, Vector3 pos, float duration, int layer = 15)
    {
        if (GameManager.CurScene != null && GameManager.CurScene.UIRoot != null)
        {
            return PlayEffect(effName, GameManager.CurScene.UIRoot.transform, pos, duration, layer);
        }
        return null;
    }

    public static GameObject PlayEffect(string effName, Transform parent,Vector3 offset, float duration, int layer = 15)
    {
        if (string.IsNullOrEmpty(effName))
        {
            return null;
        }

#if UNITY_EDITOR
        //编辑器模式下，提供一个预览的途径
        if (SkillBulletMgr.Ins == null)
        {
            SkillBulletMgr.CreateRoot();
        }
#endif

        GameObject go = SkillBulletMgr.Ins.PlayAt(effName, Vector3.zero, duration, false);
        if (go == null)
        {
            return null;
        }

        Transform trans = go.transform;
        trans.SetParent(parent);
        trans.localPosition = offset;
        trans.localRotation = Quaternion.identity;
        trans.localScale = Vector3.one;
        Utils.SetAllChildLayer(go.transform, layer);
        return go;
    }

    public static void StopEffect(GameObject go)
    {
        if (go == null)
        {
            return;
        }
        if (SkillBulletMgr.Ins != null)
        {
            SkillBulletMgr.Ins.DestroyBullet(go);
        }
        else
        {
            DestroyImmediate(go);
        }
    }
}
