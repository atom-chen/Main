using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuController : MonoBehaviour {
  private static StartMenuController _Instance;
  public static StartMenuController Instance
  {
    get
    {
      return _Instance;
    }
  }

  public GameObject m_StartObj;
  public LoginMenu m_LoginMenu;
  public RegisterMenu m_RegisterMenu;

  private string m_CurUserName;
  void Awake()
  {
    _Instance = this;
  }
  void OnEnable()
  {
    InitUI();
  }
  public void OnUserNameClick()
  {
    m_StartObj.SetActive(false);
    m_LoginMenu.gameObject.SetActive(true);

  }

  public void OnServerClick()
  {
    //直接发一个包
    m_LoginMenu.Login();
  }


  public void OnEnterGameClick()
  {

  }

  public void OnClickRegister()
  {
    m_StartObj.SetActive(false);
    m_LoginMenu.gameObject.SetActive(false);
    m_RegisterMenu.gameObject.SetActive(true);
  }
   public void GotoLoginMenu()
  {
    m_StartObj.SetActive(false);
    m_LoginMenu.gameObject.SetActive(true);
    m_RegisterMenu.gameObject.SetActive(false);
  }
  public void InitUI()
  {
    m_StartObj.SetActive(true);
    m_LoginMenu.gameObject.SetActive(false);
    m_RegisterMenu.gameObject.SetActive(false);
  }

  public void SetUserName(string name)
  {
    m_CurUserName = name;
  }

}
