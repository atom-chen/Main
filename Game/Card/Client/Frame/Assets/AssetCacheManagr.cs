using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//统一的资源缓存层
//缓存的是加载后的资源，不是AB包

public class AssetCacheManagr : MonoBehaviour
{
    public class AssetCache<T> where T : Object
    {
        private Dictionary<string, T> caches = new Dictionary<string, T>();

        public bool TryGet(string path, out T asset)
        {
            bool has = caches.TryGetValue(path, out asset);
            if (has && asset == null)
            {
                caches.Remove(path);
                return false;
            }
            return has;
        }

        public void Cache(string path, T asset)
        {
            caches[path] = asset;
        }

        public void Remove(string path)
        {
            caches.Remove(path);
        }

        //清除引用，不会释放资源，需要配合UnloadUnused
        public HashSet<string> ClearUnRef()
        {
            HashSet<string> keys = new HashSet<string>();
            foreach (var pair in caches)
            {
                if (!AssetManager.refMgr.HasRef(pair.Value))
                {
                    keys.Add(pair.Key);
                }
            }
            foreach (var key in keys)
            {
                caches.Remove(key);
            }
            return keys;
        }

        //获取第一个没用引用的缓存
        public string GetFirstUnRef()
        {
            foreach (var pair in caches)
            {
                if (!AssetManager.refMgr.HasRef(pair.Value))
                {
                    return pair.Key;
                }
            }
            return null;
        }

        //清空缓存，不会释放资源，需要配合UnloadUnused
        public void Clear()
        {
            caches.Clear();
        }

        public int Count()
        {
            return caches.Count;
        }
    }

    public static  AssetCacheManagr Ins { get; private set; }

    //WWW加载贴图，引用计数，超数量后检查引用清理，切场景时清掉没用引用的
    public AssetCache<Texture2D> wwwCahce = new AssetCache<Texture2D>();
    //图集缓存，没有引用计数，切场景时全部清
    public AssetCache<UIAtlas> commonAtlas = new AssetCache<UIAtlas>();
    //始终缓存，不会释放
    public AssetCache<Object> notUnloadAssets = new AssetCache<Object>(); 

    void Awake()
    {
        Ins = this;
    }

    void Start()
    {
        StartCoroutine(AutoClearWWWCache());
    }

    void OnDestroy()
    {
        Ins = null;
    }

    IEnumerator AutoClearWWWCache()
    {
        while (true)
        {
            while (GameManager.LoadingScene)
            {
                yield return null;
            }
            while (wwwCahce.Count() < 10)
            {
                yield return null;
            }
            string unRef = wwwCahce.GetFirstUnRef();
            if (!string.IsNullOrEmpty(unRef))
            {
                Texture2D asset;
                if (wwwCahce.TryGet(unRef, out asset))
                {
                    Destroy(asset);
                }
                wwwCahce.Remove(unRef);
                //yield return AssetManager.UnloadUnusedAssetsAsync();
            }
            yield return null;
        }
    }

    public void ClearUnRef()
    {
        wwwCahce.ClearUnRef();
        commonAtlas.Clear();
    }
}
