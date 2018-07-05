using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoHelpLauncher : MonoBehaviour
{
    public int m_HelpID;
    /// <summary>
    /// 点击帮助视频按钮时调用
    /// </summary>
    public void OnShowHelpVideo()
    {
        if (VideoHelpController.Instance() == null)
        {
            return;
        }
        if(VideoHelpController.Instance().ShowVideo(m_HelpID))
        {
            
        }

    }
}
