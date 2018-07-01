using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerItem : MonoBehaviour {
  public UILabel m_ServerNameLabel;
  private string m_ServerName;
  private ServerPropert m_ServerProperty;

  void Start()
  {
    UIDragScrollView mDrag = this.GetComponent<UIDragScrollView>();
    if(mDrag!=null)
    {
      UIScrollView scrollView = this.transform.parent.parent.GetComponent<UIScrollView>();
      if(scrollView!=null)
      {
        mDrag.scrollView = scrollView;
      }
    }
  }

  public void InitItem(ServerPropert server)
  {
    m_ServerNameLabel.text = server.Name;
    m_ServerProperty = server;
    UIButton btn = this.GetComponent<UIButton>();
    if(btn!=null)
    {
      btn.onClick.Add(new EventDelegate(OnClickItem));
    }
  }

  private void OnClickItem()
  {
    StartMenu.Instance.SetCurServer(m_ServerProperty);
  }
}
