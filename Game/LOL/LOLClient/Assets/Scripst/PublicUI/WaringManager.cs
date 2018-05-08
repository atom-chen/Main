using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaringManager
{

  public static void ShowWaring(string text)
  {
    //动态加载一个提示窗
    GameObject obj = UIManager.CreateUI("Waring");
    WaringWindow window = obj.GetComponent<WaringWindow>();
    if (window != null)
    {
      window.Init(text);
    }
  }
  private WaringManager()
  {

  }


}
