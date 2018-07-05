using Games;
using Games.Table;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryRoleShow : MonoBehaviour
{

    //static data
    public static Tab_StoryRoleShow tabRoleShow = null;
    public UITexture m_ShowTexture;
    public UILabel m_RoleNameLabel;
    public UILabel m_DescLabel;
    public UILabel m_FeatureLabel;
    public UILabel m_CVLabel;

    public TweenAlpha m_CloseTween;
    public UIEventTrigger m_ScreenTrigger;
    void Start()
    {
        m_CloseTween.AddOnFinished(new EventDelegate(OnCloseTweenFinish));
        m_ScreenTrigger.onClick.Add(new EventDelegate(OnClickScreen));
    }
    void OnEnable()
    {
        //根据ID去读表

        if (tabRoleShow != null)
        {
            m_CVLabel.text = tabRoleShow.CV;
            if (tabRoleShow.getDescCount() >= 3)
            {
                m_DescLabel.text = tabRoleShow.GetDescbyIndex(0);
                m_FeatureLabel.text = tabRoleShow.GetDescbyIndex(1);
                m_ShowTexture.mainTexture = Utils.LoadTexture(tabRoleShow.GetDescbyIndex(2));
            }
            m_RoleNameLabel.text = tabRoleShow.Name;
            if (StoryPlayerController.Instance() != null)
            {
                StoryPlayerController.Instance().IsPause = true;
            }

        }
        else
        {
            UIManager.CloseUI(UIInfo.StoryRoleShow);
        }
    }

    /// <summary>
    /// 点击屏幕，播放关闭动画
    /// </summary>
    void OnClickScreen()
    {
        m_CloseTween.ResetToBeginning();
        m_CloseTween.PlayForward();
    }

    //关闭
    void OnCloseTweenFinish()
    {

        //通知story模块继续
        if (GameManager.storyManager != null)
        {
            GameManager.storyManager.IsPause = false;
        }
        if(StoryPlayerController.Instance()!=null)
        {
            StoryPlayerController.Instance().IsPause = false;
        }
        UIManager.CloseUI(UIInfo.StoryRoleShow);
    }
}
