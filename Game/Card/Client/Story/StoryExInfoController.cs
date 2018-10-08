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
    public GameObject m_CVObj;
    public UILabel m_ExInfoCV;             //增强展示 左边声优
    public UITweener m_ExInfoBGTween;      //bgtween动画
    public UITweener m_ExInfoDescTween;      //desc tween动画
    public UITweener m_ExInfoNameTween;     //name tween动画
    public UITweener m_ExInfoCVTween;     //CV tween动画

    public TweenAlpha m_CloseTween;

    private Tab_StoryRoleShow m_TabRoleShow = null;
    private float m_TweenWait = 0.0f;
    private bool m_bIsExInfoFakeShow = false;   //在exinfo逻辑中，fake是否已经显示
    private const float Scale = 1.0f;       //缩放比例
	void Start () 
    {
        m_ExInfoBGTween.AddOnFinished(new EventDelegate(OnBGTweenPlayFinish));
        m_ExInfoNameTween.AddOnFinished(new EventDelegate(OnNameTweenPlayFinish));
        m_ExInfoDescTween.AddOnFinished(new EventDelegate(OnDescTweenPlayFinish));
        m_ExInfoCVTween.AddOnFinished(new EventDelegate(OnCVTweenPlayFinish));
        m_CloseTween.AddOnFinished(new EventDelegate(OnCloseTweenPlayFinish));
	}
    void OnEnable()
    {
        ResetExInfo();
    }

    public void ExInfoFadeOut()
    {
        m_CloseTween.PlayForward();
    }

    /// <summary>
    /// 开始显示的逻辑
    /// </summary>
    /// <param name="tab">数据源</param>
    public void ShowRoleExInfo(Tab_StoryRoleShow tab)
    {
        if(IsHideing)
        {
            return;
        }
        if(tab!=null)
        {
            gameObject.SetActive(true);
            m_TabRoleShow = tab;
            Transform tr = transform;
            if(tab.Area == 0)
            {
                tr.localPosition = new Vector3(tab.GetINTParabyIndex(0), tr.localPosition.y, tr.localPosition.z);
            }
            else if(tab.Area == 1)
            {
                tr.localPosition = new Vector3(tab.GetINTParabyIndex(0), tr.localPosition.y, tr.localPosition.z);
            }
            else
            {
                this.gameObject.SetActive(false);
            }
            StartCoroutine(ShowRoleExInfo());
        }
    }

    /// <summary>
    /// 将ExInfo的所有状态初始化
    /// </summary>
    private void ResetExInfo()
    {
        m_bIsExInfoFakeShow = false;
        UITweener[] tweens = this.GetComponentsInChildren<UITweener>();
        foreach (UITweener temp in tweens)
        {
            temp.enabled = false;
            temp.ResetToBeginning();
        }
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
            gameObject.SetActive(false);
            yield break;
        }
        if (m_TabRoleShow.getSTRParaCount() >= 1)
        {
            List<string> content = StoryRoleShow.FormatDescContent(m_TabRoleShow.GetSTRParabyIndex(0));
            m_ExInfoDesc1.text = 0 < content.Count ? content[0] : "";
            m_ExInfoDesc2.text = 1 < content.Count ? content[1] : "";
            m_ExInfoDesc3.text = 2 < content.Count ? content[2] : "";
        }
        if (m_CVObj != null)
        {
            string cvName = m_TabRoleShow.CV;
            if (string.IsNullOrEmpty(cvName))
            {
                m_CVObj.SetActive(false);
            }
            else
            {
                m_CVObj.SetActive(true);
                m_ExInfoCV.text = StrDictionary.GetClientDictionaryString("#{5048}", cvName);
            }
        }
        m_ExInfoName.text = m_TabRoleShow.Name;
        yield return new WaitForSeconds(m_TweenWait);
        m_ExInfoBGTween.PlayForward();
        yield return new WaitForSeconds(m_TabRoleShow.FLOATPara1);
        ExInfoFadeOut();
    }
    /// <summary>
    /// 把当前ExInfo的所有拖到结束状态
    /// </summary>
    public void SetExInfoEnd()
    {
        m_ExInfoBGTween.ResetToBeginning(false);
        m_ExInfoBGTween.enabled = false;
        m_ExInfoNameTween.ResetToBeginning(false);
        m_ExInfoNameTween.enabled = false;
        m_ExInfoCVTween.ResetToBeginning(false);
        m_ExInfoCVTween.enabled = false;
        m_ExInfoDescTween.ResetToBeginning(false);
        m_ExInfoDescTween.enabled = false;
        m_bIsExInfoFakeShow = true;
        if (StoryPlayerController.Instance() != null &&  m_TabRoleShow!=null)
        {
            if (m_TabRoleShow.Area == 0)
            {
                StoryPlayerController.Instance().RefreshFakeObj(m_TabRoleShow.Area, Vector3.one * Scale);
            }
            else if (m_TabRoleShow.Area == 1)
            {
                StoryPlayerController.Instance().RefreshFakeObj(m_TabRoleShow.Area, Vector3.one * Scale);
            }
        }
        m_TabRoleShow = null;
    }

    /// <summary>
    /// BG动画播放结束的回调
    /// </summary>
    void OnBGTweenPlayFinish()
    {
        if (m_bIsExInfoFakeShow || StoryPlayerController.Instance() == null || m_TabRoleShow==null)
        {
            return;
        }
        m_bIsExInfoFakeShow = true;
        if (StoryPlayerController.Instance() != null)
        {
            if (m_TabRoleShow.Area == 0)
            {
                StoryPlayerController.Instance().RefreshFakeObj(m_TabRoleShow.Area, Vector3.one * Scale);
            }
            else if (m_TabRoleShow.Area == 1)
            {
                StoryPlayerController.Instance().RefreshFakeObj(m_TabRoleShow.Area, Vector3.one * Scale);
            }
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

    void OnCVTweenPlayFinish()
    {

    }

    void OnCloseTweenPlayFinish()
    {
        if(m_CloseTween.value == 0)
        {
            m_TabRoleShow = null;
            StopCoroutine(ShowRoleExInfo());
            gameObject.SetActive(false);
        }
    }

    public bool Active
    {
        get
        {
            return this.gameObject.activeSelf && (m_ExInfoBGTween.isActiveAndEnabled || m_ExInfoCVTween.isActiveAndEnabled ||
              m_ExInfoNameTween.isActiveAndEnabled || m_ExInfoDescTween.isActiveAndEnabled);
        }
    }

    public bool IsHideing
    {
        get { return m_CloseTween.enabled == true; }
    }
}
