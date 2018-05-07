using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginMenu : MonoBehaviour {
  public UIInput m_UserName;
  public UIInput m_PassWord;
  public GameObject btn;
  public GameObject exit;
	void Start () {
    //进入游戏
    if (btn != null)
    {
      UIEventListener listener = UIEventListener.Get(btn);
      listener.onClick += ButtonClick;
    }
    //退出
    if (exit != null)
    {
      UIEventListener listener = UIEventListener.Get(exit);
      listener.onClick += Exit;
    }
	}
	
  public void ButtonClick(GameObject obj)
  {
    Debug.Log(m_UserName.value + "  " + m_PassWord.value);
    LaunchSceneLogic.Instance.ToMainScene();
  }

  public void Exit(GameObject obj)
  {
    LaunchSceneLogic.Instance.OpenLaunchPanel();
  }
}
