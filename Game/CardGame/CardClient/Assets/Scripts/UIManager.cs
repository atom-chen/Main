using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager{

  public static GameObject CreateUI(string path, Transform parent = null)
  {
    GameObject asset = ResourceManager.Load("Prefabs/" + path);
    Transform obj=GameObject.Instantiate(asset).transform;
    if(parent!=null)
    {
      obj.parent = parent;
      obj.localScale = new Vector3(1, 1, 1);
    }
    else
    {
      List<UIRoot> uiRoot = UIRoot.list;
      if (uiRoot.Count > 0)
      {
        NGUITools.AddChild(uiRoot[0].gameObject, obj);
        obj.localScale = new Vector3(1, 1, 1);
      }
    }

    return obj.gameObject;
  }

  public static void Loading(int SceneID,float time,Transform parent=null)
  {
    GameObject obj=UIManager.CreateUI("Loading", parent);
    SceneLoading load = obj.GetComponent<SceneLoading>();
    if(load!=null)
    {
      load.Init(SceneID, time);
    }
  }

}
