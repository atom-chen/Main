using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WWWLoader
{
    public string cachePath;


    public class WWWLoadTask
    {
        public string url;
        public Texture2D texture;
        public Dictionary<string, string> header;
        public bool isNeedToCacheToLocal = true;
        public bool isCacheToMemory = true;
        public bool shouldLoadFromCache = true;
        public string cahceName = null;

        public delegate void OnLoaded(WWWLoadTask task);
        public OnLoaded onLoaded;

        public string GetCacheName()
        {
            if (!string.IsNullOrEmpty(cahceName))
            {
                return cahceName;
            }

            return Path.GetFileName(url);
        }

        public WWWLoadTask(string url,OnLoaded loaded,string cahceFileName)
        {
            this.url = url;
            onLoaded = loaded;
            cahceName = cahceFileName;
        }

        public WWWLoadTask(string url)
        {
            this.url = url;
        }

        public void Finish()
        {
            try
            {
                if (null != onLoaded) onLoaded(this);
            }
            catch (System.Exception e)
            {
                LogModule.ExceptionLog(e);
            }
        }
    }

    private Queue<WWWLoadTask> queue = new Queue<WWWLoadTask>();

    public void ClearFileCache()
    {
        string folderPath = GetCacheFolderPath();

        if (!Directory.Exists(folderPath))
        {
            Directory.Delete(folderPath, true);
        }
    }

    public void AddTask(WWWLoadTask task)
    {
        Texture2D asset;
        if (AssetCacheManagr.Ins.wwwCahce.TryGet(task.url, out asset))
        {
            //Debug.Log("Load From Memory");
            task.texture = asset;
            task.Finish();
        }
        else
        {
            queue.Enqueue(task);
        }
    }

    public virtual IEnumerator Loading()
    {
        while (queue.Count > 0)
        {
            yield return DoLoad(queue.Dequeue());
        }
    }

    private IEnumerator DoLoad(WWWLoadTask task)
    {
        if (null == task)
        {
            yield break;
        }

        string cacheFilePath = GetCachePath(task);
        Texture2D tex = null;
        string url = task.url;
        bool isLoadFromCache = false;
        CheckCreateFolder();
        if (task.shouldLoadFromCache && File.Exists(cacheFilePath))
        {
            //如果之前不存在缓存文件
            url = GetCacheUrl(cacheFilePath);
            isLoadFromCache = true;
        }

        WWW www;
        if (task.header != null)
        {
            www = new WWW(url, null, task.header);
        }
        else
        {
            www = new WWW(url);
        }
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            LogModule.ErrorLog(www.error);
        }
        else
        {
            tex = www.textureNonReadable;
            tex.name = task.GetCacheName();

            if (task.isNeedToCacheToLocal && !isLoadFromCache)
            {
                //缓存数据
                File.WriteAllBytes(cacheFilePath, www.bytes);
                //Debug.Log("Save Cache");
            }

            //if (isLoadFromCache)
            //{
            //    Debug.Log("Load FromCache");
            //}
        }
        www.Dispose();

        if (tex != null)
        {
            task.texture = tex;
            if (task.isCacheToMemory)
            {
                AssetCacheManagr.Ins.wwwCahce.Cache(task.url,tex);
            }
        }
        
        task.Finish();
    }

    public void CheckCreateFolder()
    {
        string folderPath = GetCacheFolderPath();

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
    }

    public string GetCacheFolderPath()
    {
        return string.Format("{0}/{1}"
            , Application.persistentDataPath
            , cachePath
           );
    }

    public string GetCachePath(WWWLoadTask task)
    {
        return string.Format("{0}/{1}/{2}"
            ,Application.persistentDataPath
            ,cachePath
            ,task.GetCacheName()
            );
    }

    public string GetCacheUrl(string path)
    {
        return string.Format("file:///{0}", path);
    }
}