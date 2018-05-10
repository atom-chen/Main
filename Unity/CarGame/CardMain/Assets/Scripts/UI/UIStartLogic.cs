using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIStartLogic : MonoBehaviour {

  public UIInput m_NameInput;
  public UIEventTrigger m_Commit;
	void Start () {
		if(PlayerPrefs.HasKey("name"))
    {
      m_NameInput.value = PlayerPrefs.GetString("name");
    }
    m_Commit.onClick.Add(new EventDelegate(OnCommitClick));
	}


  private void OnCommitClick()
  {
    //保存，并跳转
    PlayerPrefs.SetString("name", m_NameInput.value.ToString());
    BG.Instance.OnSwitchScene(1);
    SceneManager.LoadScene(1);
  }
}
