/********************************************************************
	created:	2015/11/03
	created:	3:11:2015   16:51
	author:		WD
	
	purpose:	资源使用入口，与游戏逻辑对接
*********************************************************************/

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Games.LogicObj;
using Games;
using Object = UnityEngine.Object;

public class AssetManager
{
    public static string ResDownloadPath { get { return m_resDownloadPath; } }      //资源下载路径
    public static string AssetBuildPath { get { return m_assetBuildPath; } }        //Bundle资源生成路径
    public static string ResBuildPath { get { return m_resBuildPath; } }            //资源生成根路径
    public static string AssetLocalPath { get { return m_assetLocalPath; } }        //本地资源加载路径
    public static string UserDataPath { get { return m_userDataPath; } }

	public static string m_gameBuildPath = Application.dataPath.Replace("Assets","") + "Build/Game";
    private static string m_resDownloadPath = Application.persistentDataPath + "/ResData";
	private static string m_assetBuildPath = Application.dataPath.Replace("Assets","") + "Build/Res/ResData";
	private static string m_resBuildPath =  Application.dataPath.Replace("Assets","") + "Build/Res";
    private static string m_assetLocalPath = Application.streamingAssetsPath + "/ResData";
    private static string m_userDataPath = Application.persistentDataPath + "/UserData";


    public static AssetRefManager refMgr = new AssetRefManager();

    #region GameObject操作

    public static Object LoadResource(string resPath, System.Type systemTypeInstance = null,bool cache = false)
    {
#if UNITY_EDITOR
        Stopwatch sw = new Stopwatch();
        sw.Start();
#endif
        UnityEngine.Object resObject = null;
        if (AssetCacheManagr.Ins != null && AssetCacheManagr.Ins.notUnloadAssets.TryGet(resPath, out resObject))
        {
            //Debug.Log("Load from cache");
            return resObject;
        }
        if (null == systemTypeInstance)
        {
            resObject = Resources.Load(resPath);
        }
        else
        {
            resObject = Resources.Load(resPath, systemTypeInstance);
        }
#if UNITY_EDITOR
        sw.Stop();
        if (sw.ElapsedMilliseconds >= 500)
        {
            LogModule.WarningLog(string.Format("Load Asset:{0} slow,cost time:{1}.ms", resPath, sw.ElapsedMilliseconds));
        }
#endif
        if (cache && AssetCacheManagr.Ins != null)
        {
            AssetCacheManagr.Ins.notUnloadAssets.Cache(resPath,resObject);
        }
        return resObject;
    }

    public static void DestroyObj(Object objToDestroy)
    {
        if (null != objToDestroy)
        {
            Object.Destroy(objToDestroy);
        }
    }

    public static void DestroyObjImmediate(Object objToDestroy)
    {
        if (null != objToDestroy)
        {
            Object.DestroyImmediate(objToDestroy);
        }
    }

    //加载一个Resource下的资源
    public static GameObject LoadObj(string resPath, string name = null)
    {
        GameObject resObject = AssetManager.LoadResource(resPath) as GameObject;
        if (null == resObject)
        {
            LogModule.ErrorLog("Load Obj Error:" + resPath);
            return null;
        }

        GameObject newObj = GameObject.Instantiate(resObject) as GameObject;
        if (null != newObj && !string.IsNullOrEmpty(name))
        {
            newObj.name = name;
        }
        return newObj;
    }

    public static GameObject LoadObjWithTransform(string resPath, out Vector3 localpos, out Quaternion localrot, string name = null)
    {
        localpos = new Vector3();
        localrot = new Quaternion();

        GameObject resObject = AssetManager.LoadResource(resPath) as GameObject;
        if (null == resObject)
        {
            LogModule.ErrorLog("Load Obj Error:" + resPath);
            return null;
        }

        localpos = resObject.transform.localPosition;
        localrot = resObject.transform.localRotation;

        GameObject newObj = GameObject.Instantiate(resObject) as GameObject;
        if (null != newObj && !string.IsNullOrEmpty(name))
        {
            newObj.name = name;
        }
        return newObj;
    }

    //加载一个Resource下的资源并绑定到父节点
    public static GameObject LoadObjAndBindToParent(string resPath, Transform parentTransform, string name = null)
    {
        GameObject resObject = LoadResource(resPath) as GameObject;
        if (null == resObject)
        {
            return null;
        }

        return InstantiateObjToParent(resObject, parentTransform, name);
    }

    public static bool SetObjParent(GameObject child, Transform parent)
    {
        if (null == child || null == parent)
        {
            return false;
        }
        Transform childTrans = child.transform;
        childTrans.SetParent(parent,false);
        childTrans.localPosition = Vector3.zero;
        childTrans.localScale = Vector3.one;
        childTrans.localRotation = Quaternion.identity;
        return true;
    }

    // 复制物体并绑定到父节点，将坐标置0
    public static GameObject InstantiateObjToParent(UnityEngine.Object resObject, Transform parentTransform, string name = null)
    {
        if (null == resObject || null == parentTransform)
        {
            return null;
        }

        GameObject newObj = GameObject.Instantiate(resObject,Vector3.zero,Quaternion.identity,parentTransform) as GameObject;
        if (null == newObj)
        {
            return null;
        }

        Transform childTrans = newObj.transform;
        childTrans.localPosition = Vector3.zero;
        childTrans.localScale = Vector3.one;
        childTrans.localRotation = Quaternion.identity;

        if (null != name)
        {
            newObj.name = name;
        }
        return newObj;
    }
    #endregion

    public static void LoadBundle(BundleTask task, AssetLoader.LoadQueueType loadType = AssetLoader.LoadQueueType.COMMON)
    {
        if (GameManager.storyManager != null 
            && GameManager.storyManager.StoryMode 
            && !(GameManager.CurScene is BattleScene))
        {
            //AssetLoader.Instance.LoadBundleSync(task);
            //AssetLoader.Instance.AddFastBundleTask(task);
            //return;

            if (loadType == AssetLoader.LoadQueueType.COMMON)
            {
                loadType = AssetLoader.LoadQueueType.FAST;
            }
        }

        switch (loadType)
        {
            case AssetLoader.LoadQueueType.FAST:
                AssetLoader.Instance.AddFastBundleTask(task);
                break;
            //case AssetLoader.LoadQueueType.PLAYER:
            //    AssetLoader.Instance.AddPlayerBundleTask(task);
            //    break;
            case AssetLoader.LoadQueueType.UI:
                AssetLoader.Instance.AddUITask(task);
                break;
            case AssetLoader.LoadQueueType.SYNC:
                AssetLoader.Instance.LoadBundleSync(task);
                break;
            default:
                AssetLoader.Instance.AddBundleTask(task);
                break;
        }
    }

    public static void LoadUI(UIPathData uiData, BundleTask.DelBundleLoadFinish delLoadFinish, object delFun, object param)
    {
        BundleTask task = new BundleTask(delLoadFinish);
        task.AddParam(uiData);
        task.AddParam(delFun);
        task.AddParam(param);
        task.Add(BundleTask.BundleType.UI, uiData.bundleName, "", !string.IsNullOrEmpty(uiData.groupSubName), uiData.groupSubName);
        if (uiData.isLoadSync)
        {
            AssetLoader.Instance.LoadBundleSync(task);
        }
        else
        {
            AssetLoader.Instance.AddUITask(task);
        }
    }

    public static void Preload(BundleTask task)
    {
        AssetLoader.Instance.LoadBundleSync(task);
    }

    public delegate void LoadHeadInfoDelegate(GameObject objHeadInfo);
    public static void LoadHeadInfoPrefab(UIPathData uiData, GameObject billParent, string strPrefabName, LoadHeadInfoDelegate delFun)
    {
        if (null == GameManager.CurScene ||
            null == GameManager.CurScene.NameBoardPool)
        {
            LogModule.ErrorLog("scene is not init when load headinfo");
            return;
        }

        GameManager.CurScene.NameBoardPool.CreateUIFromBundle(uiData, strPrefabName, OnLoadHeadInfo, billParent, delFun);
    }

    static void OnLoadHeadInfo(GameObject resObj, object billParent, object fun)
    {
        if (resObj == null)
        {
            return;
        }

        if (GameManager.CurScene == null)
        {
            GameObject.DestroyImmediate(resObj);
            return;
        }

        if (resObj.GetComponent<BattleHeadInfoLogic>() != null)
        {
            if (GameManager.CurScene.BattleNameBoardRoot == null)
            {
                GameObject.DestroyImmediate(resObj);
                return;
            }
            else
            {
                SetObjParent(resObj, GameManager.CurScene.BattleNameBoardRoot);
            }
        }
        else
        {
            if (GameManager.CurScene.NameBoardRoot == null)
            {
                GameObject.DestroyImmediate(resObj);
                return;
            }
            else
            {
                SetObjParent(resObj, GameManager.CurScene.NameBoardRoot);
            }
        }

        resObj.transform.localRotation = Quaternion.identity;

        NameBoard nameBoard = Utils.TryAddComponent<NameBoard>(resObj);
        if (null == nameBoard)
        {
            return;
        }
        else
        {
            GameObject go = billParent as GameObject;
            if (go)
            {
                nameBoard.m_BindObj = go.transform;
                nameBoard.ResetNameBoardHeight();

                //如果隐藏名字版，则判断是否显示
                if (PlayerPreferenceData.PlayerNameBoard == false && resObj.GetComponent<PlayerHeadInfoLogic>() != null)
                {
                    if (null != ObjManager.MainPlayer && go == ObjManager.MainPlayer.gameObject)
                    {
                        resObj.SetActive(true);
                    }
                    else
                    {
                        //当前场景是否 不隐藏玩家名字版
                        if (GameManager.CurScene != null && GameManager.CurScene.IsDiableHidePlayerNameBord())
                        {
                            resObj.SetActive(true);
                        }
                        else
                        {
                            resObj.SetActive(false);
                        }
                    }
                }
                else if (PlayerPreferenceData.NPCNameBoard == false && resObj.GetComponent<NpcHeadInfoLogic>() != null)
                {
                    resObj.SetActive(false);
                }
                else
                {
                    resObj.SetActive(true);
                }
            }
            else
            {
                //已经被销毁，则该headInfo不再有效
                UnLoadHeadInfoPrefab(resObj);
                return;
            }
        }

        LoadHeadInfoDelegate delFun = fun as LoadHeadInfoDelegate;
        if (null != delFun) delFun(resObj);
    }

    public static void UnLoadHeadInfoPrefab(GameObject headInfo)
    {
        if (null == headInfo)
        {
            return;
        }

        //会导致本来要隐藏的节点出现

        // 可能在TV中被隐藏
        //Transform trans = headInfo.transform;
        //for (int i = 0; i < trans.childCount; i++)
        //{
        //    trans.GetChild(i).gameObject.SetActive(true);
        //}

        //在池子中置为未使用
        if (null != GameManager.CurScene &&
            null != GameManager.CurScene.NameBoardPool)
        {
            GameManager.CurScene.NameBoardPool.Remove(headInfo);
        }
        else
        {
            GameObject.DestroyImmediate(headInfo);
        }
    }

    public static UIAtlas GetAtlas(string atlasName)
    {
        if (null == AssetCacheManagr.Ins || null == AssetCacheManagr.Ins.commonAtlas)
        {
            return null;
        }
        UIAtlas atlas;
        string path = string.Format("Bundle/UI/Atlas/{0}", atlasName);
        if (!AssetCacheManagr.Ins.commonAtlas.TryGet(path, out atlas))
        {
            GameObject resObj = LoadResource(path, typeof(GameObject)) as GameObject;
            if (null != resObj)
            {
                atlas = resObj.GetComponent<UIAtlas>();
                AssetCacheManagr.Ins.commonAtlas.Cache(path, atlas);
            }
        }
        return atlas;
    }

    public static void ClearPool()
    {
        AssetCacheManagr.Ins.ClearUnRef();
        AssetLoader.Instance.ReleaseBundlePool();
    }

    public static IEnumerator UnloadUnusedAssetsAsync()
    {
        yield return Resources.UnloadUnusedAssets();
    }

    public static void UnloadUnusedAssets()
    {
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }

    //慎用，注意引用依赖
    public static void UnloadAsset(Object asset)
    {
        Resources.UnloadAsset(asset);
    }
}

