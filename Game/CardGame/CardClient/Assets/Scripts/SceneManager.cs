using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager {
  private static GameObject m_CurrenUI;

  public static void SwitchScene(string name)
  {
    GameObject load = ResourceManager.Load("Prefabs/" + name);
    List<UIRoot> uiRoot = UIRoot.list;
    if (uiRoot.Count > 0)
    {
      NGUITools.AddChild(uiRoot[0].gameObject, load);
      load.transform.localScale = new Vector3(1, 1, 1);
      if (m_CurrenUI != null)
      {
        GameObject.Destroy(m_CurrenUI.gameObject);
      }
      m_CurrenUI = load;
    }
  }
}
