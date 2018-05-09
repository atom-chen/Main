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

    GameObject asset = ResourceManager.Load("Prefabs/" + path);
    Transform obj = GameObject.Instantiate(asset).transform;

    return obj.gameObject;
  }

  public static void Loading(int SceneID, float time, Transform parent = null)
  {
    GameObject obj = UIManager.CreateUI("Loading", parent);
  }

}
