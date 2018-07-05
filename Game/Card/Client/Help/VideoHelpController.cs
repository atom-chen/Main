using UnityEngine;
using System.Collections;
using Games.Table;
using UnityEngine.Video;
using System;

public class VideoHelpController : UIControllerBase<UIHelpController>
{
    private static VideoHelpController m_instance;
    public static VideoHelpController Instance()
    {
        return m_instance;
    }
    private const string m_VideoRootPath = "Video/";
    void Awake()
    {
        m_instance = this;
    }
    public bool ShowVideo(int helpID)
    {
#if UNITY_IPHONE || UNITY_ANDROID
   string videoName = TableManager.GetVideoByID(helpID,0).Name+".mp4";      
   return Handheld.PlayFullScreenMovie(m_VideoRootPath + videoName, Color.black, FullScreenMovieControlMode.CancelOnInput,FullScreenMovieScalingMode.AspectFit);
#else
        return false;
#endif
    }
}
