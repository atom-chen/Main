using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.Table;
using Games.GlobeDefine;

public class StoryExInfoController : MonoBehaviour 
{
    public UITexture m_BGTexture;          //贴图
    public UILabel m_ExInfoName;           //增强展示 展示名称
    public UILabel m_ExInfoDesc1;           //增强展示 描述
    public UILabel m_ExInfoDesc2;           //增强展示 描述
    public UILabel m_ExInfoDesc3;           //增强展示 描述
    public UILabel m_ExInfoCV;             //增强展示 左边声优
    public UITweener m_ExInfoBGTween;      //bgtween动画
    public UITweener m_ExInfoDescTween;      //desc tween动画
    public UITweener m_ExInfoNameTween;     //name tween动画
    public UITweener m_ExInfoCVTween;     //CV tween动画

    private Tab_StoryRoleShow m_TabRoleShow = null;
    private float m_TweenWait = 0.5f;
    private bool m_bIsExInfoFakeShow = false;   //在exinfo逻辑中，fake是否已经显示
	void Start () 
    {
        m_ExInfoBGTween.AddOnFinished(new EventDelegate(OnBGTweenPlayFinish));
        m_ExInfoNameTween.AddOnFinished(new EventDelegate(OnNameTweenPlayFinish));
        m_ExInfoDescTween.AddOnFinished(new EventDelegate(OnDescTweenPlayFinish));
        //m_ExInfoCVTween.AddOnFinished(new EventDelegate(OnCVTweenPlayFinish));
	}
    void OnEnable()
    {
        ResetExInfo();
    }
    /// <summary>
    /// 开始显示的逻辑
    /// </summary>
    /// <param name="tab">数据源</param>
    public void ShowRoleExInfo(Tab_StoryRoleShow tab)
    {
        if(tab!=null)
        {
            m_TabRoleShow = tab;
            StartCoroutine(ShowRoleExInfo());
        }
    }

    /// <summary>
    /// 将ExInfo的所有状态初始化
    /// </summary>
    private void ResetExInfo()
    {
        m_bIsExInfoFakeShow = false;
        m_ExInfoBGTween.ResetToBeginning();
        m_ExInfoNameTween.ResetToBeginning();
        m_ExInfoCVTween.ResetToBeginning();
        m_ExInfoDescTween.ResetToBeginning();
        StopCoroutine(ShowRoleExInfo());
    }

    /// <summary>
    /// 走ExInfo逻辑
    /// </summary>
    /// <param name="flag">0 更新左边信息  1更新右边信息</param>
    IEnumerator ShowRoleExInfo()
    {
        if (m_TabRoleShow == null)
        {
            yield break;
        }
        yield return new WaitForSeconds(m_TweenWait);
        if (m_TabRoleShow != null)
        {
            m_ExInfoBGTween.PlayForward();
            if (m_TabRoleShow.getDescCount() >= 3)
            {
                m_ExInfoDesc1.text = m_TabRoleShow.GetDescbyIndex(0);
                m_ExInfoDesc2.text = m_TabRoleShow.GetDescbyIndex(1);
                m_ExInfoDesc3.text = m_TabRoleShow.GetDescbyIndex(2);
            }
            m_ExInfoCV.text = "声优 " + m_TabRoleShow.CV;
            m_ExInfoName.text = m_TabRoleShow.Name;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 把当前ExInfo的所有拖到结束状态
    /// </summary>
    public void SetExInfoEnd()
    {
        m_ExInfoBGTween.ResetToBeginning(false);
        m_ExInfoNameTween.ResetToBeginning(false);
        m_ExInfoCVTween.ResetToBeginning(false);
        m_ExInfoDescTween.ResetToBeginning(false);
    }

    /// <summary>
    /// BG动画播放结束的回调
    /// </summary>
    void OnBGTweenPlayFinish()
    {
        if (m_bIsExInfoFakeShow || StoryPlayerController.Instance()==null)
        {
            return;
        }
        m_bIsExInfoFakeShow = true;
        if (StoryPlayerController.Instance() != null)
        {
            StoryPlayerController.Instance().RefreshFakeObj(m_TabRoleShow.Area, Vector3.one * 1.5f);
        }
        m_ExInfoNameTween.PlayForward();
    }

    /// <summary>
    /// Name动画播放结束的回调
    /// </summary>
    void OnNameTweenPlayFinish()
    {
        m_ExInfoDescTween.PlayForward();
    }

    /// <summary>
    /// DESC动画播放结束的回调
    /// </summary>
    void OnDescTweenPlayFinish()
    {
        m_ExInfoCVTween.PlayForward();
    }


}
