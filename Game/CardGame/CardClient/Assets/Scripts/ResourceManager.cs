using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager{

  public static GameObject LoadResources(string path)
  {
    GameObject obj = Resources.Load<GameObject>(path);
    return obj;
  }

  public static GameObject CreateObjAsChild(string path,GameObject parent)
  {
    GameObject obj = LoadResources(path);
    if(obj!=null)
    {
      NGUITools.AddChild(parent, obj);
    }
    return obj;
  }
}
