/********************************************************************
	created:	2015/11/03
	created:	3:11:2015   16:50
	author:		WD
	
	purpose:	用于加载Bundle资源
*********************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class AssetLoader : MonoBehaviour
{

    public enum LoadQueueType
    {
        UI, //使用UI队列加载
        COMMON, //通用队列（比较慢）
        FAST, //较快资源队列，按照数量等待
        SYNC,   //同步加载
    }

    public static AssetLoader Instance
    {
        get
        {
            if (m_instance == null)
            {
                Create();
            }
            return m_instance;
        }
    }

    public delegate void DelLoadPercent(float percent);

    private static AssetLoader m_instance;

    private ABLoader abLoader = new ABLoader();

    private WWWLoader wwwTexLoader = new WWWLoader()
    {
        cachePath = "ImageCache",
    };

    static void Create()
    {
        GameObject go = new GameObject("AssetLoader");
        m_instance = go.AddComponent<AssetLoader>();
#if UNITY_EDITOR
        if (Application.isPlaying)
        {
            GameObject.DontDestroyOnLoad(m_instance);
        }
#else
        GameObject.DontDestroyOnLoad(m_instance);
#endif
    }

    private Queue<BundleTask> m_bundleLoadTaskQueue = new Queue<BundleTask>();
    private Queue<BundleTask> m_fastBundleLoadTaskQueue = new Queue<BundleTask>();
    //private Queue<BundleTask> m_playerBundleLoadTaskQueue = new Queue<BundleTask>();
    private Queue<BundleTask> m_uiLoadTaskQueue = new Queue<BundleTask>();

    private Dictionary<string, bool> m_dicFilePathCache = new Dictionary<string, bool>(); //文件路径缓存，防止多次判断file.exist

    void Start()
    {
        StartCoroutine(FastBundleLoadQueue());
        StartCoroutine(BundleLoadQueue());
        //StartCoroutine(PlayerBundleLoadQueue());
        StartCoroutine(UILoadQueue());
        StartCoroutine(WWWLoadQueue());
    }

    void Update()
    {
        Singleton<Games.Events.EventSystem>.GetInstance().UpdateDelayEventQueue();
    }

    // 增加一个加载Bundle任务
    public void AddBundleTask(BundleTask task)
    {
        if (task == null) return;
        m_bundleLoadTaskQueue.Enqueue(task);
    }

    // 增加一个加载Bundle任务
    public void AddFastBundleTask(BundleTask task)
    {
        if (task == null) return;
        m_fastBundleLoadTaskQueue.Enqueue(task);
    }

    // 增加一个加载UI任务
    public void AddUITask(BundleTask task)
    {
        if (task == null) return;
        m_uiLoadTaskQueue.Enqueue(task);
    }

    public void AddWWWTask(WWWLoader.WWWLoadTask task)
    {
        wwwTexLoader.AddTask(task);
    }

    // 获取在不同条件下bundle资源路径
    string GetResLoadPath(BundleTask.BundleType type, string name, out bool bBundlePath)
    {
        string subFolder = "/";
        bBundlePath = false;
        switch (type)
        {
            case BundleTask.BundleType.MODEL:
                subFolder = "/Model/";
                break;
            case BundleTask.BundleType.EFFECT:
                subFolder = "/Effect/";
                break;
            case BundleTask.BundleType.SOUND:
                subFolder = "/Sound/";
                break;
            case BundleTask.BundleType.UI:
                subFolder = "/UI/Prefab/";
                break;
            case BundleTask.BundleType.UIRES:
                subFolder = "/UI/UIRes/";
                break;
            case BundleTask.BundleType.SHADER:
                subFolder = "/Shader/";
                break;
            case BundleTask.BundleType.COMMON:
                subFolder = "/";
                break;

        }

        if (PlatformHelper.IsEnableUpdate())
        {
            //string curLoadPath = AssetManager.ResDownloadPath + (subFolder + name).ToLower() + ".data";
            string curLoadPath = string.Format("{0}{1}.data",AssetManager.ResDownloadPath,((subFolder + name).ToLower()));
            if (m_dicFilePathCache.ContainsKey(curLoadPath))
            {
                bBundlePath = m_dicFilePathCache[curLoadPath];
            }
            else
            {
                bBundlePath = System.IO.File.Exists(curLoadPath);
                m_dicFilePathCache.Add(curLoadPath, bBundlePath);
            }

            if (bBundlePath)
            {
                return curLoadPath;
            }
        }

        // #if UNITY_ANDROID && !UNITY_EDITOR
        //         return AssetManager.AssetLocalPath + (subFolder + name).ToLower() + ".data";
        // #elif UNITY_EDITOR
        //         return "file://" + AssetManager.AssetBuildPath + (subFolder + name).ToLower() + ".data";
        // #else
        //         return "file://" + AssetManager.AssetLocalPath  + (subFolder + name).ToLower() + ".data";
        // #endif
        // 

        //string resLoadHead = "Assets/Game/Bundle";
        //string resLoadTail = ".prefab";
        string resLoadHead = "Bundle";
        string resLoadTail = "";
        if (type == BundleTask.BundleType.SOUND)
        {
            //return resLoadHead + subFolder + name;
            return string.Format("{0}{1}{2}", resLoadHead, subFolder, name);
        }
        else
        {
            //return resLoadHead + subFolder + name + resLoadTail;
            return string.Format("{0}{1}{2}{3}",resLoadHead,subFolder,name,resLoadTail);
        }

    }

    Object LoadAssetRes(string assetPath, System.Type type)
    {
        return Resources.Load(assetPath, type);
        //UnityEditor.AssetDatabase.LoadAssetAtPath(path + ".ogg", typeof(AudioClip));
    }

    ResourceRequest LoadAssetAsync(string assetPath, System.Type type)
    {
        return Resources.LoadAsync(assetPath, type);
    }

    // 加载资源
    public Object LoadDataInternal(BundleTask.BundleType bundleType, string name, string groupSubName)
    {
        Object retObj = null;
        //#if UNITY_EDITOR
        string loadName = name;
        if (!string.IsNullOrEmpty(groupSubName))
        {
            loadName = name + "/" + groupSubName;
        }

        bool bBundlePath = false;
        string path = GetResLoadPath(bundleType, loadName, out bBundlePath);

        if (bundleType == BundleTask.BundleType.SOUND)
        {
            retObj = LoadAssetRes(path, typeof(AudioClip));
        }
        else
        {
            retObj = LoadAssetRes(path, typeof(GameObject));
        }

        if (null == retObj)
        {
            LogModule.ErrorLog("load failed:{" + path + ".}");
        }
        //#endif
        return retObj;
    }

    ResourceRequest LoadDataInternalAsync(BundleTask.BundleType bundleType, string name, string groupSubName)
    {
        ResourceRequest req;
        //#if UNITY_EDITOR
        string loadName = name;
        if (!string.IsNullOrEmpty(groupSubName))
        {
            loadName = name + "/" + groupSubName;
        }

        bool bBundlePath = false;
        string path = GetResLoadPath(bundleType, loadName, out bBundlePath);

        if (bundleType == BundleTask.BundleType.SOUND)
        {
            req = LoadAssetAsync(path, typeof(AudioClip));
        }
        else
        {
            req = LoadAssetAsync(path, typeof(GameObject));
        }

        if (null == req)
        {
            LogModule.ErrorLog("load failed:{" + path + " }");
        }
        //#endif
        return req;
    }

    public void LoadFromAB(string abPath,BundleTaskUnit unit)
    {
        abLoader.Load(abPath,unit);
    }

    IEnumerator UILoadQueue()
    {
        while (true)
        {
            while (m_uiLoadTaskQueue.Count > 0)
            {
                BundleTask curTask = m_uiLoadTaskQueue.Dequeue();
                if (null != curTask)
                {
                    while (curTask.PeekDoing() != null)
                    {
                        BundleTaskUnit curUnit = curTask.PopDoing();
                        if (null != curUnit)
                        {
                            yield return LoadUnitAsync(curUnit);
                            curTask.AssetLoaded(curUnit);
                        }
                    }

                    //yield return null;
                    yield return new WaitForEndOfFrame();

                    curTask.Finish();
                }
            }

            yield return null;
        }

    }

    private Stopwatch bundleQueueStopWatch = new Stopwatch();
    private struct AsyncTask
    {
        public ResourceRequest resReq;
        public BundleTaskUnit unit;
    }
    private List<AsyncTask> faskReqList = new List<AsyncTask>();

    // 资源加载队列
    IEnumerator BundleLoadQueue()
    {
        while (true)
        {
            while (m_bundleLoadTaskQueue.Count > 0)
            {
                BundleTask curTask = m_bundleLoadTaskQueue.Dequeue();
                if (null != curTask)
                {
                    while (curTask.PeekDoing() != null)
                    {
                        BundleTaskUnit curUnit = curTask.PopDoing();
                        if (null != curUnit)
                        {
                            bundleQueueStopWatch.Reset();
                            bundleQueueStopWatch.Start();

                            bool bBundlePath = false;
                            string curBundlePath = GetResLoadPath(curUnit.bundleType, curUnit.bundeName, out bBundlePath);
                            bool sameFrame = true;
                            if (bBundlePath)
                            {
                                LoadFromAB(curBundlePath, curUnit);
                            }
                            else
                            {
                                int frame = Time.frameCount;
                                ResourceRequest req = LoadDataInternalAsync(curUnit.bundleType, curUnit.bundeName, curUnit.groupSubName);
                                if (req.asset != null)
                                {
                                    curUnit.assetLoaded = req.asset;
                                }
                                else
                                {
                                    yield return req;
                                }

                                if (Time.frameCount > frame)
                                {
                                    sameFrame = false;
                                }
                            }

                            curTask.AssetLoaded(curUnit);
                            bundleQueueStopWatch.Stop();
                            if (sameFrame)
                            {
                                yield return new WaitForEndOfFrame();
                            }
                        }
                    }
                }

                bundleQueueStopWatch.Reset();
                bundleQueueStopWatch.Start();
                if (null != curTask)
                {
                    curTask.Finish();
                }

                bundleQueueStopWatch.Stop();
                if (bundleQueueStopWatch.ElapsedMilliseconds > 5)
                {
                    yield return new WaitForEndOfFrame();
                }
            }
            yield return null;
        }
    }

    private IEnumerator WaitAsyncTask(List<AsyncTask> reqList)
    {
        //等待资源加载完
        for (int i = 0; i < reqList.Count; i++)
        {
            AsyncTask t = reqList[i];
            if (t.resReq.asset != null)
            {
                t.unit.assetLoaded = t.resReq.asset;
            }
            else
            {
                yield return t.resReq;
                if (t.resReq.asset == null)
                {
                    bool b = false;
                    string path = GetResLoadPath(t.unit.bundleType, t.unit.bundeName,out b);
                    LogModule.ErrorLog("load failed:{" + path + ".}");
                }
                t.unit.assetLoaded = t.resReq.asset;
            }
        }
        reqList.Clear();
    }

    // 重要资源加载队列
    IEnumerator FastBundleLoadQueue()
    {
        while (true)
        {
            while (m_fastBundleLoadTaskQueue.Count > 0)
            {
                BundleTask curTask = m_fastBundleLoadTaskQueue.Dequeue();
                while (curTask.PeekDoing() != null)
                {
                    BundleTaskUnit curUnit = curTask.PopDoing();
                    if (null != curUnit)
                    {
                        bool bBundlePath = false;
                        string curBundlePath = GetResLoadPath(curUnit.bundleType, curUnit.bundeName, out bBundlePath);
                        if (bBundlePath)
                        {
                            LoadFromAB(curBundlePath,curUnit);
                        }
                        else
                        {
                            ResourceRequest req = LoadDataInternalAsync(curUnit.bundleType, curUnit.bundeName, curUnit.groupSubName);
                            if (req.asset != null)
                            {
                                curUnit.assetLoaded = req.asset;
                            }
                            else
                            {
                                faskReqList.Add(new AsyncTask()
                                {
                                    resReq = req,
                                    unit = curUnit,
                                });
                                if (faskReqList.Count >= 3)
                                {
                                    yield return WaitAsyncTask(faskReqList);
                                }
                            }
                            //curUnit.assetLoaded = LoadDataInternal(curUnit.bundleType, curUnit.bundeName, curUnit.groupSubName);
                        }
                        yield return WaitAsyncTask(faskReqList);
                        curTask.AssetLoaded(curUnit);
                    }
                }
                curTask.Finish();
            }
            yield return null;
        }
    }

    IEnumerator WWWLoadQueue()
    {
        while (true)
        {
            yield return wwwTexLoader.Loading();
            yield return null;
        }
    }

    public void LoadBundleSync(BundleTask task)
    {
        while (task.PeekDoing() != null)
        {
            BundleTaskUnit curUnit = task.PopDoing();
            if (null != curUnit)
            {
                LoadUnit(curUnit);
                task.AssetLoaded(curUnit);
            }
        }
        task.Finish();
    }

    public void ReleaseBundlePool()
    {
        abLoader.ReleaseBundlePool();
    }

    public bool LoadUnit(BundleTaskUnit curUnit)
    {
        bool bBundlePath = false;
        string curBundlePath = GetResLoadPath(curUnit.bundleType, curUnit.bundeName, out bBundlePath);
        if (bBundlePath)
        {
            LoadFromAB(curBundlePath, curUnit);
        }
        else
        {
            curUnit.assetLoaded = LoadDataInternal(curUnit.bundleType, curUnit.bundeName, curUnit.groupSubName);
        }
        return curUnit.assetLoaded != null;
    }

    public IEnumerator LoadUnitAsync(BundleTaskUnit curUnit)
    {
        bool bBundlePath = false;
        string curBundlePath = GetResLoadPath(curUnit.bundleType, curUnit.bundeName, out bBundlePath);
        if (bBundlePath)
        {
            yield return abLoader.LoadAsync(curBundlePath, curUnit);
        }
        else
        {
            ResourceRequest req = LoadDataInternalAsync(curUnit.bundleType, curUnit.bundeName, curUnit.groupSubName);
            if (req.asset == null)
            {
                yield return req;
            }
            curUnit.assetLoaded = req.asset;
        }
    }
}
