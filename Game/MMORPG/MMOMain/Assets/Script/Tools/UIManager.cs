using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager
{
  private static Transform m_PopUI;
  public static Transform PopUI
  {
    get
    {
      if (m_PopUI == null || !m_PopUI.gameObject.activeInHierarchy)
      {
        m_PopUI = GameObject.Find("PopUI").transform;
      }
      return m_PopUI;
    }
  }
  public static GameObject CreateUI(string path, Transform parent = null)
  {
    GameObject asset = ResourceManager.Load("Prefabs/UI/" + path);
    Transform obj = GameObject.Instantiate(asset).transform;
    if (parent != null)
    {
      obj.parent = parent;
      obj.localScale = new Vector3(1, 1, 1);
      obj.localPosition = new Vector3(0, 0, 0);
    }
    else
    {
      List<UIRoot> uiRoot = UIRoot.list;
      if (uiRoot.Count > 0)
      {
        NGUITools.AddChild(uiRoot[0].gameObject, obj);
        obj.localScale = new Vector3(1, 1, 1);
        obj.localPosition = new Vector3(0, 0, 0);
      }
    }

    return obj.gameObject;
  }


  public static void Loading(int SceneID, float time, Transform parent = null)
  {
    GameObject obj = UIManager.CreateUI("Loading", parent);
    SceneLoading load = obj.GetComponent<SceneLoading>();
    if (load != null)
    {
      load.Init(SceneID, time);
    }
  }

}
