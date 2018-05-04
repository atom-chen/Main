using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchSceneLogic : MonoBehaviour {
  public LaunMenu m_Main; 
  public LoginManager m_Login;
  public RegisterManager m_Register;
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
    OpenLoginPanel();
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
    m_GongGao.gameObject.SetActive(true);
  }
  public void CloseGongGao()
  {
    m_GongGao.gameObject.SetActive(false);
  }


}
