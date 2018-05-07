using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchSceneLogic : MonoBehaviour {
  public LaunchMenu m_Main; 
  public LoginMenu m_Login;
  public RegisterMenu m_Register;
  public GongGaoPanel m_GongGao;

  private static LaunchSceneLogic _Instance;
  public static LaunchSceneLogic Instance
  {
    get
    {
      return _Instance;
    }
  }
  void Awake()
  {
    _Instance = this;
  }
  void Start()
  {
    OpenLaunchPanel();
  }
  public void ToMainScene()
  {
    //假装读条的样子
    UIManager.Loading(1, 2, this.transform.root);
    Destroy(this.gameObject);
  }
  public void OpenLoginPanel()
  {
    m_Main.gameObject.SetActive(false);
    m_Login.gameObject.SetActive(true);
    m_Register.gameObject.SetActive(false);
  }
  public void OpenRegisterPanel()
  {
    m_Main.gameObject.SetActive(false);
    m_Login.gameObject.SetActive(false);
    m_Register.gameObject.SetActive(true);
  }
  public void OpenLaunchPanel()
  {
    m_Main.gameObject.SetActive(true);
    m_Login.gameObject.SetActive(false);
    m_Register.gameObject.SetActive(false);
  }

  public void OnClickGongGao()
  {
    if (m_GongGao.gameObject.activeInHierarchy)
    {
      m_GongGao.gameObject.SetActive(false);
    }
    else
    {
      m_GongGao.gameObject.SetActive(true);
    }

  }
  

}
