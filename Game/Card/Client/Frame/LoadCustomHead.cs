//--------------------------------------
//modiby 20180302 by fanshufang 增加载入质量控制，用于支持大图下载
//--------------------------------------

using UnityEngine;
using System.Collections;
using Games.GlobeDefine;
using Games.Table;
using System.Collections.Generic;
using System.IO;
using Games;
using System;

public class LoadCustomHead : MonoBehaviour
{

    //private string path = Application.persistentDataPath + "/ImageCache/";
    static float _tickTime = 60 * 5.0f;//5分钟
    private const string m_defaultPicTail = ".jpg";
    private const string ApiFixActionStr = "personhead-1255700589.picsh.myqcloud.com";
    //http://101.226.84.18/2280228787860032590.png
    //http://personhead-1255700589.picsh.myqcloud.com/2280228787860032590.png
    private Queue<LoadTask> m_LoadTaskQueue = new Queue<LoadTask>();
    private Queue<QQWXLoadTask> m_QQWXLoadTaskQueue = new Queue<QQWXLoadTask>();
    private Queue<CustomPicLoadTask> m_CustomPicLoadQueue = new Queue<CustomPicLoadTask>();
    //  加载回来的Texture， AddTask传入的iconId
    public delegate void DelLoadFInish(Texture texture, ulong icon);
    public delegate void DelQQWXLoadFInish(Texture texture);
    public delegate void DelCustomPicLoadFInish(Texture texture);

    public static LoadCustomHead Ins;

    //只有头像进缓存 。大图防止内存占用，现用现载
    protected Dictionary<ulong, Texture> caches = new Dictionary<ulong, Texture>();
    protected Dictionary<string, Texture> qqwxCaches = new Dictionary<string, Texture>();
    protected Dictionary<string, Texture> customPicCaches = new Dictionary<string, Texture>();

    //清理其他玩家头像缓存
    public static void Clear()
    {
        if (Ins != null && Ins.caches != null) Ins.caches.Clear();
    }


    public class LoadTask
    {
        public UISprite Sprite;
        public UITexture UiTexture;
        public ulong Icon;
        public DelLoadFInish DelLoadFInish;
        public Texture Texture;
        public LoadImageStyle ImageStyle;
    }

    public class QQWXLoadTask
    {
        public string openid;
        public string url;
        public UITexture UiTexture;
        public DelQQWXLoadFInish DelLoadFinish;
        public Texture Texture;
    }

    public class CustomPicLoadTask
    {
        public string picId;
        public string url;
        public UITexture UiTexture;
        public DelCustomPicLoadFInish DelLoadFinish;
        public Texture Texture;
    }
    private string GetCachePath()
    {
        return Application.persistentDataPath + "/";
    }

    public enum LoadImageStyle
    {
        INVALID = 0,
        LITTLE = 1,      // 小图
        MEDIUM = 2,      // 中图
        LARGE = 3,       // 大图
    }
    string GetPicSizeAndFormat(LoadImageStyle style)
    {
        //示例：http://personhead-1255700589.picsh.myqcloud.com/2280228787860808598.png?imageView2/{0}/w/{1}/h/{2}/format/{3}/{4}
        /* 
         1、取值为1-5【详情见https://cloud.tencent.com/document/product/460/6929】
         */

        string sizeStr = "";
        switch (style)
        {
            case LoadImageStyle.INVALID:
                break;
            case LoadImageStyle.LITTLE:
                sizeStr = string.Format("?imageView2/{0}/w/{1}/h/{2}/format/{3}/{4}", 2, 70, 70, "jpg", 85);
                break;
            case LoadImageStyle.MEDIUM:
                sizeStr = string.Format("?imageView2/{0}/w/{1}/h/{2}/format/{3}/{4}", 2, 220, 220, "jpg", 85);
                break;
            case LoadImageStyle.LARGE:
                sizeStr = "";// 原图可不指定尺寸
                break;
            default:
                break;
        }

        return sizeStr;
    }
    void Awake()
    {
        Ins = this;
    }

    void OnDestroy()
    {
        Ins = null;

    }

    static void SetDefaltIcon(LoadTask curTask)
    {
        if (curTask != null && curTask.Sprite != null)
        {
            curTask.Sprite.gameObject.SetActive(true);
            curTask.Sprite.spriteName = Utils.GetIconStrByID((ulong)GlobeVar.UserDebfaltIconId);
            if (curTask.UiTexture != null)
            {
                curTask.UiTexture.mainTexture = null;
            }
        }
    }

    void Start()
    {
        StartCoroutine(LoadQueue());
        StartCoroutine(QQWXLoadQueue());
        StartCoroutine(CustomPicLoadQueue());
    }

    public static bool IsSystemIcon(ulong icon)
    {
        return icon < GlobeVar.SystemIcon;
    }

    public static bool IsNotSystemOrMsdk(ulong icon)
    {
        return !IsSystemIcon(icon) && 
            !IsMsdkDefaultIcon(icon);
    }

    public static bool IsMsdkDefaultIcon(ulong icon)
    {
        return icon == GlobeVar.MsdkDefaultIcon;
    }

    //iconId,    回调，   sprite，  UITexture
    public void AddTask(ulong icon, DelLoadFInish delLoadFInish = null, UISprite sprite = null, UITexture uiTexture = null, LoadImageStyle imageStyle = LoadImageStyle.MEDIUM, bool refreshEverytime = false/*是否需要刷新tex*/)
    {
        if (uiTexture != null)
        {
            uiTexture.mainTexture = null;
        }

        if (IsSystemIcon(icon))//系统头像
        {
            if (sprite != null)
            {
                sprite.gameObject.SetActive(true);
                sprite.spriteName = Utils.GetIconStrByID(icon, true);
            }

            if (uiTexture != null)
            {
                uiTexture.gameObject.SetActive(false);
            }

            return;
        }

        if (IsMsdkDefaultIcon(icon))
        {
            if (sprite != null)
            {
                sprite.gameObject.SetActive(true);
                sprite.spriteName = Utils.GetIconStrByID(1, true);//默认头像
            }

            if (uiTexture != null)
            {
                uiTexture.gameObject.SetActive(false);
            }

            return;
        }

        if (uiTexture != null
            && LoadCustomHead.LoadImageStyle.MEDIUM == imageStyle
            && caches.ContainsKey(icon) && !refreshEverytime)
        {
            var texture = caches[icon];

            if (texture != null)
            {

                uiTexture.mainTexture = texture;
                if (uiTexture.mainTexture.height < 50f || uiTexture.mainTexture.width < 50f)//防止出现？图片
                {
                    if (sprite != null)
                    {
                        sprite.gameObject.SetActive(true);
                        sprite.spriteName = Utils.GetIconStrByID(1, true);//默认头像
                    }
                }
                else
                {
                    uiTexture.gameObject.SetActive(true);
                    if (sprite != null) sprite.gameObject.SetActive(false);
                }
            }

            return;
        }

        var task = new LoadTask();
        task.Sprite = sprite;
        task.UiTexture = uiTexture;
        task.DelLoadFInish = delLoadFInish;
        task.Icon = icon;
        task.ImageStyle = imageStyle;

        m_LoadTaskQueue.Enqueue(task);
    }

    public void AddQQWXTask(string openid, string url, DelQQWXLoadFInish delLoadFInish = null, UITexture uiTexture = null)
    {
        if (DebugInfo.m_OpenQQWXFriendListDebugLog)
        {
            Debug.Log("----------------------AddQQWXTask--------------------------");
            Debug.Log("----------------------url = " + url + "--------------------------");
        }

        if (uiTexture != null)
        {
            uiTexture.mainTexture = null;
        }

        if (uiTexture != null && qqwxCaches.ContainsKey(url))
        {
            if (DebugInfo.m_OpenQQWXFriendListDebugLog)
            {
                Debug.Log("----------------------AddQQWXTask qqwxCaches--------------------------");
            }

            uiTexture.mainTexture = qqwxCaches[url];
            uiTexture.gameObject.SetActive(true);
        }
        else
        {
            if (DebugInfo.m_OpenQQWXFriendListDebugLog)
            {
                Debug.Log("----------------------AddQQWXTask Enqueue--------------------------");
            }

            var task = new QQWXLoadTask();
            task.openid = openid;
            task.url = url;
            task.UiTexture = uiTexture;
            task.DelLoadFinish = delLoadFInish;

            m_QQWXLoadTaskQueue.Enqueue(task);
        }
    }

    public void AddCustomPicTask(string picId, string url, DelCustomPicLoadFInish delLoadFInish = null, UITexture uiTexture = null)
    {
        if (uiTexture != null)
        {
            uiTexture.mainTexture = null;
        }

        if (uiTexture != null && customPicCaches.ContainsKey(url))
        {
            uiTexture.mainTexture = customPicCaches[url];
            uiTexture.gameObject.SetActive(true);
        }
        else
        {
            var task = new CustomPicLoadTask();
            task.picId = picId;
            task.url = url;
            task.UiTexture = uiTexture;
            task.DelLoadFinish = delLoadFInish;

            m_CustomPicLoadQueue.Enqueue(task);
        }
    }
    IEnumerator LoadQueue()
    {
        var tickTime = Utils.GetTimeStamp();
        while (true)
        {
            //var count = 0;
            while (m_LoadTaskQueue.Count > 0)
            {
                var curTask = m_LoadTaskQueue.Dequeue();

                yield return LoadImage(curTask, curTask.ImageStyle, true);

                if (curTask.Texture != null && curTask.ImageStyle == LoadImageStyle.MEDIUM)//缓存
                {
                    caches[curTask.Icon] = curTask.Texture;
                }

                try
                {
                    if (null != curTask.DelLoadFInish)
                    {
                        curTask.DelLoadFInish(curTask.Texture, curTask.Icon);
                    }

                    if (null == curTask.Texture || null == curTask.DelLoadFInish)
                    {
                        SetDefaltIcon(curTask);
                    }
                }
                catch (System.Exception e)
                {
                    SetDefaltIcon(curTask);
                    LogModule.ExceptionLog(e);
                }

            }

            if (caches.Count > 0 && Utils.GetTimeStamp() - tickTime > _tickTime)
            {
                tickTime = Utils.GetTimeStamp();
                caches.Clear();
            }

            yield return null;
            yield return null;
        }
    }



    IEnumerator QQWXLoadQueue()
    {
        var tickTime = Utils.GetTimeStamp();
        while (true)
        {
            //var count = 0;
            while (m_QQWXLoadTaskQueue.Count > 0)
            {
                if (DebugInfo.m_OpenQQWXFriendListDebugLog)
                {
                    Debug.Log("----------------------QQWXLoadQueue--------------------------");
                }

                var curTask = m_QQWXLoadTaskQueue.Dequeue();

                yield return QQWXLoadImage(curTask, true);

                if (DebugInfo.m_OpenQQWXFriendListDebugLog)
                {
                    Debug.Log("----------------------QQWXLoadImage Over--------------------------");
                }

                if (curTask.Texture != null)//缓存
                {
                    qqwxCaches[curTask.url] = curTask.Texture;
                }

                try
                {
                    if (null != curTask.DelLoadFinish)
                    {
                        if (DebugInfo.m_OpenQQWXFriendListDebugLog)
                        {
                            Debug.Log("----------------------curTask.DelLoadFinish()--------------------------");
                        }

                        curTask.DelLoadFinish(curTask.Texture);
                    }
                }

                catch (System.Exception e)
                {
                    LogModule.ExceptionLog(e);
                }

            }

            if (caches.Count > 0 && Utils.GetTimeStamp() - tickTime > _tickTime)
            {
                tickTime = Utils.GetTimeStamp();
                caches.Clear();
            }

            yield return null;
            yield return null;
        }
    }

    IEnumerator CustomPicLoadQueue()
    {
        var tickTime = Utils.GetTimeStamp();
        while (true)
        {
            //var count = 0;
            while (m_CustomPicLoadQueue.Count > 0)
            {

                var curTask = m_CustomPicLoadQueue.Dequeue();

                yield return CustomPicLoadImage(curTask, true);

                if (curTask.Texture != null)//缓存
                {
                    customPicCaches[curTask.url] = curTask.Texture;
                }

                try
                {
                    if (null != curTask.DelLoadFinish)
                    {
                        curTask.DelLoadFinish(curTask.Texture);
                    }
                }

                catch (System.Exception e)
                {
                    LogModule.ExceptionLog(e);
                }

            }

            if (customPicCaches.Count > 0 && Utils.GetTimeStamp() - tickTime > _tickTime)
            {
                tickTime = Utils.GetTimeStamp();
                customPicCaches.Clear();
                AssetManager.UnloadUnusedAssetsAsync();
            }

            yield return null;
            yield return null;
        }
    }

    private string GetIconPath(LoadTask task)//处理msdk头像问题
    {
        string url = "http://";

        switch (task.ImageStyle)
        {
            case LoadImageStyle.LITTLE:
                {
                    url += ApiFixActionStr + "/" + task.Icon.ToString() + m_defaultPicTail + GetPicSizeAndFormat(LoadImageStyle.LITTLE);
                    break;
                }
            case LoadImageStyle.MEDIUM:
                {
                    url += ApiFixActionStr + "/" + task.Icon.ToString() + m_defaultPicTail + GetPicSizeAndFormat(LoadImageStyle.MEDIUM);
                    break;
                }
            case LoadImageStyle.LARGE:
                {
                    url += ApiFixActionStr + "/" + task.Icon.ToString() + m_defaultPicTail + GetPicSizeAndFormat(LoadImageStyle.LARGE);
                    break;
                }
            default:
                {
                    url += ApiFixActionStr + "/" + task.Icon.ToString() + m_defaultPicTail + GetPicSizeAndFormat(LoadImageStyle.MEDIUM);
                    break;
                }

        }
        return url;
    }

    private string GetIconLocalPath(LoadTask task)
    {
        switch (task.ImageStyle)
        {
            case LoadImageStyle.LARGE:
                {
                    return GetCachePath() + task.Icon.ToString() + m_defaultPicTail;//暂时使用缺省图
                }
            default:
                {
                    return GetCachePath() + task.Icon.ToString() + m_defaultPicTail;
                }
        }
    }

    private string GetQQWXIconLocalPath(QQWXLoadTask task)
    {
        if (task == null)
        {
            return "";
        }

        return GetCachePath() + task.openid + m_defaultPicTail;
    }

    private string GetCustomPicLocalPath(CustomPicLoadTask task)
    {
        if (task == null)
        {
            return "";
        }

        return GetCachePath() + task.picId + m_defaultPicTail;
    }

    public IEnumerator LoadImage(LoadTask task, LoadImageStyle style, bool isNeedToCacheToLocal = false)
    {
        if (null == task)
        {
            yield break;
        }

        string url = GetIconPath(task);
        string filePath = GetIconLocalPath(task);

        if (!File.Exists(filePath))
        {
            //如果之前不存在缓存文件
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers["host"] = ApiFixActionStr;
            WWW www = new WWW(url, null, headers);//加host
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                LogModule.ErrorLog(www.error);
            }
            else
            {
                task.Texture = www.texture;
                task.Texture.name = task.Icon.ToString();

                if (isNeedToCacheToLocal)
                {
                    //将图片保存至缓存路径
                    byte[] pngData = www.texture.EncodeToJPG();
                    File.WriteAllBytes(filePath, pngData);
                }
            }

            www.Dispose();
        }
        else
        {
            string localPath = "file:///" + filePath;
            WWW www = new WWW(localPath);
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                LogModule.ErrorLog(www.error);
            }
            else
            {
                task.Texture = www.texture;
                task.Texture.name = task.Icon.ToString();
            }
            www.Dispose();
        }
    }

    public IEnumerator QQWXLoadImage(QQWXLoadTask task, bool isNeedToCacheToLocal = false)
    {
        if (DebugInfo.m_OpenQQWXFriendListDebugLog)
        {
            Debug.Log("----------------------QQWXLoadImage IEnumerator--------------------------");
        }

        if (null == task)
        {
            yield break;
        }

        if (DebugInfo.m_OpenQQWXFriendListDebugLog)
        {
            Debug.Log("----------------------task not null--------------------------");
        }

        string url = task.url;
        string filePath = GetQQWXIconLocalPath(task);

        if (!File.Exists(filePath))
        {
            if (DebugInfo.m_OpenQQWXFriendListDebugLog)
            {
                Debug.Log("----------------------File.Exists(filePath) == false--------------------------");
            }

            //如果之前不存在缓存文件
            WWW www = new WWW(url);
            yield return www;

            if (DebugInfo.m_OpenQQWXFriendListDebugLog)
            {
                Debug.Log("----------------------WWW Download OK--------------------------");
            }

            if (!string.IsNullOrEmpty(www.error))
            {
                if (DebugInfo.m_OpenQQWXFriendListDebugLog)
                {
                    Debug.Log("----------------------WWW Download www.error = " + www.error + "--------------------------");
                }

                LogModule.ErrorLog(www.error);
            }
            else
            {
                task.Texture = www.texture;
                task.Texture.name = task.openid.ToString();

                if (isNeedToCacheToLocal)
                {
                    //将图片保存至缓存路径
                    byte[] pngData = www.texture.EncodeToPNG();
                    File.WriteAllBytes(filePath, pngData);
                }
            }

            www.Dispose();
        }
        else
        {
            if (DebugInfo.m_OpenQQWXFriendListDebugLog)
            {
                Debug.Log("----------------------File.Exists(filePath)--------------------------");
            }

            string localPath = "file:///" + filePath;
            WWW www = new WWW(localPath);
            yield return www;

            if (DebugInfo.m_OpenQQWXFriendListDebugLog)
            {
                Debug.Log("----------------------WWW LoadFile OK--------------------------");
            }

            if (!string.IsNullOrEmpty(www.error))
            {
                if (DebugInfo.m_OpenQQWXFriendListDebugLog)
                {
                    Debug.Log("----------------------WWW LoadFile www.error" + www.error + "--------------------------");
                }

                LogModule.ErrorLog(www.error);
            }
            else
            {
                task.Texture = www.texture;
                task.Texture.name = task.openid.ToString();
            }
            www.Dispose();
        }
    }

    public IEnumerator CustomPicLoadImage(CustomPicLoadTask task, bool isNeedToCacheToLocal = false)
    {
        if (null == task)
        {
            yield break;
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        string url = "file://" +task.url;
#else
        string url = "file:///" + task.url;
#endif
        string filePath = GetCustomPicLocalPath(task);

        if (!File.Exists(filePath))
        {
            //如果之前不存在缓存文件
            WWW www = new WWW(url);
            yield return www;

            if (!string.IsNullOrEmpty(www.error))
            {
                LogModule.ErrorLog(www.error);
            }
            else
            {
                task.Texture = www.texture;
                task.Texture.name = task.picId;

                if (isNeedToCacheToLocal)
                {
                    //将图片保存至缓存路径
                    byte[] pngData = www.texture.EncodeToPNG();
                    File.WriteAllBytes(filePath, pngData);
                }
            }
            www.Dispose();
        }
        else
        {
            string localPath = "file:///" + filePath;
            WWW www = new WWW(localPath);
            yield return www;

            if (!string.IsNullOrEmpty(www.error))
            {
                LogModule.ErrorLog(www.error);
            }
            else
            {
                task.Texture = www.texture;
                task.Texture.name = task.picId.ToString();
            }
            www.Dispose();
        }
    }
    public static void SaveToLocal(Texture2D tex)
    {
        byte[] bytes = tex.EncodeToJPG();
        System.IO.File.WriteAllBytes(ImagePath(), bytes);
    }
    public static string ImagePath()
    {
        string imagePath = "", Path_save = "";
#if UNITY_ANDROID
        imagePath = Application.persistentDataPath;
#elif UNITY_IPHONE
		imagePath = Application.persistentDataPath;  
#elif UNITY_EDITOR
        imagePath = Application.persistentDataPath;  
#endif
        Path_save = imagePath + "/ScreenPhoto.jpg";
        if (!Directory.Exists(Path_save))
        {
            Directory.CreateDirectory(Path_save);
        }
        return Path_save;
    }


}