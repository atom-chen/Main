using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//错误提示面板

public class WaringWindow : MonoBehaviour {
  public Text m_ErrText;

  public void Close()
  {
    Destroy(this.gameObject);
  }

  public void Init(string text)
  {
    m_ErrText.text = text;
  }

}
