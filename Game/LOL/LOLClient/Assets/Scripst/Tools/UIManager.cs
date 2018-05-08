using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager
{
  private static GameObject m_PopUI;
  public static GameObject CreateUI(string path, Transform parent = null)
  {
    if (m_PopUI == null || !m_PopUI.activeInHierarchy)
    {
      m_PopUI = GameObject.Find("PopUI");
    }
    GameObject asset = ResourceManager.Load("Prefabs/" + path);
    Transform obj = GameObject.Instantiate(asset).transform;

    if (parent != null)
    {
      obj.parent = parent;
      obj.localPosition = new Vector3(0, 0, 0);
      obj.localScale = new Vector3(1, 1, 1);
    }
    else
    {
      obj.transform.parent = m_PopUI.transform;
      obj.localPosition = new Vector3(0, 0, 0);
      obj.localScale = new Vector3(1, 1, 1);
    }

    return obj.gameObject;
  }

  public static void Loading(int SceneID, float time, Transform parent = null)
  {
    GameObject obj = UIManager.CreateUI("Loading", parent);
    //SceneLoading load = obj.GetComponent<SceneLoading>();
    //if (load != null)
    //{
    //  load.Init(SceneID, time);
    //}
  }

}
