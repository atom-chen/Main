using Games;
using Games.Table;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.GlobeDefine;
public class StoryRoleShow : MonoBehaviour
{
    public static float KeepTime = 0;      //在该界面逗留的时间
    private float startTime;                  //开始时间
    //static data
    public static Tab_StoryRoleShow tabRoleShow = null;
    public PaintNormalizer m_ShowTexturePaint;
    public UILabel m_RoleNameLabel;
    public UILabel m_DescLabel1;
    public UILabel m_DescLabel2;
    public UILabel m_DescLabel3;
    public UILabel m_FeatureLabel;
    public GameObject m_CVObj;
    public UILabel m_CVLabel;
    public StateGroup m_RoleTypeTexture;

    public UITweener m_CloseTween;
    public UIEventTrigger m_ScreenTrigger;

    private UITweener[] tweens;
    public static void Show(Tab_StoryRoleShow tabRoleShow)
    {
        if (tabRoleShow != null)
        {
            StoryRoleShow.tabRoleShow = tabRoleShow;
            UIManager.ShowUI(UIInfo.StoryRoleShow);
        }
        KeepTime = 0;
    }

    void Start()
    {
        tweens = this.GetComponentsInChildren<UITweener>();
        if (tweens != null)
        {
            foreach (UITweener t in tweens)
            {
                t.ResetToBeginning();
            }
        }
        m_CloseTween.AddOnFinished(new EventDelegate(OnCloseTweenFinish));
        m_ScreenTrigger.onClick.Add(new EventDelegate(OnClickScreen));
        startTime = Time.time;
    }

    void OnEnable()
    {
        //根据ID去读表
        if (tabRoleShow != null)
        {
            if (m_CVObj != null)
            {
                string cvName = tabRoleShow.CV;
                if (string.IsNullOrEmpty(cvName))
                {
                    m_CVObj.SetActive(false);
                }
                else
                {
                    m_CVObj.SetActive(true);
                    m_CVLabel.text = StrDictionary.GetClientDictionaryString("#{5048}", cvName);
                }
            }

            if (tabRoleShow.getSTRParaCount() >= 2)
            {
                List<string> content = null;
                if(tabRoleShow.GetINTParabyIndex(0) != GlobeVar.INVALID_ID)
                {
                    Tab_CardStory tabCardStory = TableManager.GetCardStoryByID(tabRoleShow.GetINTParabyIndex(0), 0);
                    if(tabCardStory !=null)
                    {
                        content = FormatDescContent(tabCardStory.Intro);
                    }
                }
                //如果上面的读取失败
                if(content == null)
                {
                    content = FormatDescContent(tabRoleShow.GetSTRParabyIndex(0));
                }
                m_DescLabel1.text = 0 < content.Count ? content[0] : "" ;
                m_DescLabel2.text = 1 < content.Count ? content[1] : "" ;
                m_DescLabel3.text = 2 < content.Count ? content[2] : "" ;

                m_FeatureLabel.text = tabRoleShow.GetSTRParabyIndex(1);
                if(tabRoleShow.getINTParaCount() >=3)
                {
                    m_ShowTexturePaint.Setup(tabRoleShow.GetINTParabyIndex(1));    //根据charmodel初始化图片
                    if (m_RoleTypeTexture!=null)
                    {
                        m_RoleTypeTexture.ChangeState(tabRoleShow.GetINTParabyIndex(2));
                    }
                }
            }
            m_RoleNameLabel.text = tabRoleShow.Name;
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
        if (Active)
        {
            return;
        }
        m_CloseTween.PlayForward();
    }

    //关闭
    void OnCloseTweenFinish()
    {
        tabRoleShow = null;
        KeepTime = Time.time - startTime;
        if(StoryPlayerController.Instance()!=null)
        {
            StoryPlayerController.Instance().IsPause = false;
            if(!StoryPlayerController.Instance().IsAutoPlay)
            {
                StoryPlayerController.Instance().PlayNext(true);
            }
        }
        UIManager.CloseUI(UIInfo.StoryRoleShow);
        //如果不是自动播放，则手动让剧情继续
    }

    public static List<string> FormatDescContent(string content)
    {
        int startIndex = 0;
        List<string> ret = new List<string>();
        char[] cArr = content.ToCharArray();
        try
        {
            for (int i = 0; i < cArr.Length - 1; i++)
            {
                if (cArr[i] == '#')
                {
                    if (cArr[i + 1] == 'r')
                    {
                        string str = new string(cArr, startIndex, i - startIndex);
                        ret.Add(str);
                        i += 2;                 //越过#r
                        startIndex = i;
                    }
                }
            }
            ret.Add(new string(cArr, startIndex, cArr.Length - startIndex));
        }
        catch(System.Exception ex)
        {
            Debug.LogError("StoryRoleShow FormatDescContent Error!" + ex.Message);
        }
        return ret;
    }
    
    public bool Active
    {
        get
        {
            if(!this.gameObject.activeSelf)
            {
                return false;
            }
            if(tweens !=null)
            {
                foreach(UITweener temp in tweens)
                {
                    if(temp.isActiveAndEnabled)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
