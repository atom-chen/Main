using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
  private static Hashtable m_Resources = new Hashtable();

  private ResourceManager()
  {

  }

  public static T Load<T>(string path, bool cache = false) where T : UnityEngine.Object
  {
    if (m_Resources.ContainsKey(path))
    {
      return m_Resources[path] as T;
    }
    T asset = Resources.Load<T>(path);
    if (asset != null)
    {
      if (cache)
      {
        m_Resources.Add(path, asset);
      }
      return asset;
    }
    else
    {
      Debug.LogError("获取失败,path= " + path);
      return null;
    }
  }

  public static GameObject Load(string path, bool cache = false)
  {
    if (m_Resources.ContainsKey(path))
    {
      return m_Resources[path] as GameObject;
    }
    GameObject obj = Resources.Load<GameObject>(path);
    if (obj != null)
    {
      if (cache)
      {
        m_Resources.Add(path, obj);
      }
    }
    else
    {
      Debug.LogError("获取GameObject失败,path= " + path);
    }
    return obj;
  }

  //创建一个GameObject
  public static GameObject CreateGameObject(string path, bool cache = false)
  {
    GameObject asset = ResourceManager.Load(path, cache);
    GameObject obj = GameObject.Instantiate(asset);
    if (obj == null)
    {
      Debug.LogError("创建GameObject失败,path= " + path);
    }
    return obj;
  }

}
