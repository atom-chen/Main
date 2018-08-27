﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerItem : MonoBehaviour
{
    public UILabel m_ServerNameLabel;
    private string m_ServerName;
    private Tab_Server m_ServerProperty;

    void Start()
    {
        UIDragScrollView mDrag = this.GetComponent<UIDragScrollView>();
        if (mDrag != null)
        {
            UIScrollView scrollView = this.transform.parent.parent.GetComponent<UIScrollView>();
            if (scrollView != null)
            {
                mDrag.scrollView = scrollView;
            }
        }
        UIButton btn = this.GetComponent<UIButton>();
        if (btn != null)
        {
            btn.onClick.Add(new EventDelegate(OnClickItem));
        }
    }

    public void InitItem(Tab_Server server)
    {
        m_ServerNameLabel.text = server.name;
        m_ServerProperty = server;
    }

    private void OnClickItem()
    {
        LaunchSceneLogic.Instance.HandleOnChooseServer(m_ServerProperty);
    }
}
