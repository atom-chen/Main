using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UICarSelectLogic : MonoBehaviour {
  public UIEventTrigger m_Commit;

  void Start()
  {
    m_Commit.onClick.Add(new EventDelegate(OnClickCommit));
  }

  //点击确定
  private void OnClickCommit()
  {
    BG.Instance.RandomStartGame();
  }
}
