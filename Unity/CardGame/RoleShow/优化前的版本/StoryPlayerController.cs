/********************************************************************************
 *	文件名：	StoryPlayerController.cs
 *	全路径：	\Script\GUI\Story\StoryPlayerController.cs
 *	创建人：	李嘉
 *	创建时间：2017-02-06
 *
 *	功能说明：对话框UI脚本
 *	修改记录：
*********************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Games.GlobeDefine;

using Games.Table;
using Games.LogicObj;
using Games;

public class StoryPlayerController : MonoBehaviour
{
    //公共变量
    public UILabel m_szContentLabel;       	//文字内容
    public TypewriterEffect m_ContentTE;    //文字打字机效果
    public UILabel m_szNameLabelL;         	//左侧说话者名字
    public UILabel m_szNameLabelR;          //右侧说话者名字
	public GameObject m_LeftFakeDecorate;	//左侧半身像周围的装饰
	public GameObject m_LeftFakeTop;		//左侧半身像的TopUI
	public GameObject m_RightFakeDecorate;	//右侧半身像周围的装饰
	public GameObject m_RightFakeTop;       //右侧半身像的TopUI

    public GameObject m_BtnSkip;


    public GameObject m_LeftExInfoRoot;        //左边增强展示根节点
    public UITexture m_LeftBGTexture;          //左边贴图
    public UILabel m_LeftExInfoName;           //增强展示 左边展示名称
    public UILabel m_LeftExInfoDesc1;           //增强展示 左边描述
    public UILabel m_LeftExInfoDesc2;           //增强展示 左边描述
    public UILabel m_LeftExInfoDesc3;           //增强展示 左边描述
    public UILabel m_LeftExInfoCV;             //增强展示 左边声优
    public UITweener m_LeftExInfoBGTween;      //左边  bgtween动画
    public UITweener m_LeftExInfoDescTween;      //左边  desc tween动画
    public UITweener m_LeftExInfoNameTween;     //左边 name tween动画
    public UITweener m_LeftExInfoCVTween;     //左边 CV tween动画
    private Vector3 m_LeftFakeObjScale=Vector3.one;
    private Vector3 m_RightFakeObjScale=Vector3.one;
    private bool m_bIsExInfoFakeShow = false;   //在exinfo逻辑中，fake是否已经显示
    private bool m_IsTweenPlayFinish = false;   //在exinfo逻辑中，所有动画是否已经播放完成
    public GameObject m_RightExInfoRoot;        //右边增强展示根节点
    public UITexture m_RightBGTexture;          //右边贴图
    public UILabel m_RightExInfoName;           //增强展示 右边展示名称
    public UILabel m_RightExInfoDesc1;           //增强展示 右边描述
    public UILabel m_RightExInfoDesc2;           //增强展示 右边描述
    public UILabel m_RightExInfoDesc3;           //增强展示 右边描述
    public UILabel m_RightExInfoCV;              //增强展示 右边声优
    public UITweener m_RightExInfoBGTween;       //右边bgtween动画
    public UITweener m_RightExInfoDescTween;      //右边desctween动画
    public UITweener m_RightExInfoNameTween;     //右边 name tween动画
    public UITweener m_RightExInfoCVTween;       //右边 CV tween动画
    private float m_TweenWait = 0.5f;

    [SerializeField]
    FakeRtInstance[] m_FakeList;            //半身像rt

    //缓存的相关数据
    private int m_nStoryID = GlobeVar.INVALID_ID;   //缓存当前的故事ID
    private int m_RealSoundId = GlobeVar.INVALID_ID;
    private int m_nLastStoryID = GlobeVar.INVALID_ID;   //上一次对话时候的故事ID
    private List<Tab_StoryContent> m_StoryContent = null;
    private Tab_StoryContent m_TabContent = null;
    private StoryEvent_Dialog m_Event = null;
    private int m_nCurStep;     //记录在多部的时候的当前步骤
    private float m_fStoryShowTime = -1.0f;    //本次剧情的播放时间
    private float m_fStoryLifeTime = 0.0f;      //本次剧情的持续时间
    private bool m_bForceAutoPlay = false;//是否强制自动播放

    bool m_Finished = false;

    private static StoryPlayerController m_Instance = null;
    public static StoryPlayerController Instance()
    {
        return m_Instance;
    }

    void Awake()
    {
        m_Instance = this;
    }

    void Start()
    {
        m_RightExInfoBGTween.AddOnFinished(new EventDelegate(OnBGTweenPlayFinish));
        m_LeftExInfoBGTween.AddOnFinished(new EventDelegate(OnBGTweenPlayFinish));
        m_LeftExInfoNameTween.AddOnFinished(new EventDelegate(OnNameTweenPlayFinish));
        m_RightExInfoNameTween.AddOnFinished(new EventDelegate(OnNameTweenPlayFinish));
        m_LeftExInfoDescTween.AddOnFinished(new EventDelegate(OnDescTweenPlayFinish));
        m_RightExInfoDescTween.AddOnFinished(new EventDelegate(OnDescTweenPlayFinish));
        m_LeftExInfoCVTween.AddOnFinished(new EventDelegate(OnCVTweenPlayFinish));
        m_RightExInfoCVTween.AddOnFinished(new EventDelegate(OnCVTweenPlayFinish));
    }

    void OnDestroy()
    {
        m_Instance = null;
    }

    void OnDisable()
    {
		if (null != GameManager.SoundManager && m_RealSoundId == GameManager.SoundManager.GetCurRealSoundId())
		{
			GameManager.SoundManager.StopRealSound();
		}
    }

    void FixedUpdate()
    {
        // 为避免服务器没返回消息这边事件结束发了其他消息影响剧情流程, 一旦点了skip就停止更新本事件
        if (GameManager.storyManager!=null && GameManager.storyManager.SkipMode)
            return;

        if (m_fStoryShowTime > 0.0f && m_fStoryLifeTime > 0.0f)
        {
            if ((m_bForceAutoPlay || PlayerPreferenceData.AutoPlayDialog ) && Time.time - m_fStoryShowTime >= m_fStoryLifeTime)
            {
                m_bIsUserSkip = false;
                PlayNext();
            }
        }
    }

    void ResetDialogTime()
    {
        m_fStoryLifeTime = 0.0f;
        m_fStoryShowTime = -0.1f;
    }

    void CleanUp()
    {
        m_nCurStep = 0;
        m_StoryContent = null;
        m_Event = null;
        m_Finished = false;

        if (GameManager.storyManager != null)
            GameManager.storyManager.SkipMode = false;

        ResetDialogTime();
    }

    public bool InitUI(int nStoryID, StoryEvent_Dialog sEvent)
    {
        CleanUp();

        m_nStoryID = nStoryID;
        m_nCurStep = 0;
        m_StoryContent = TableManager.GetStoryContentByID(nStoryID);
        m_Event = sEvent;
        if (null == m_StoryContent || m_StoryContent.Count <= 0)
        {
            UIManager.CloseUI(UIInfo.StoryPlayer);
            return false;
        }

        for (int i = 0; i < m_FakeList.Length; ++i)
        {
            m_FakeList[i].Reset();
        }

        //播放剧情
        PlayStory();

        return true;
    }

    //播放剧情
    private void PlayStory()
    {
        ResetExInfo();
        //获取当前的列表项
        if (m_nCurStep < 0 || m_nCurStep >= m_StoryContent.Count)
        {
            return;
        }

        m_TabContent = m_StoryContent[m_nCurStep];
        if (null == m_TabContent)
        {
            return;
        }

        //显示内容
        //支持换行符#r
        string szContent = m_TabContent.Content.Replace("#r", "\n");
        m_szContentLabel.text = szContent;

        m_ContentTE.ResetToBeginning();

        // 是否可跳过
        m_BtnSkip.SetActive(m_TabContent.IsCanSkip == 0);
        m_bForceAutoPlay = (m_TabContent.IsCanSkip == 2);

        //显示说话人
        //首先当SpeakerName不为空的时候显示SpeakerName
        //然后看大于0读取RoleBase表格，否则显示自己的名字
        m_szNameLabelL.text = "";
        if (m_TabContent.SpeakerNameL.Length > 0)
        {
            m_szNameLabelL.text = m_TabContent.SpeakerNameL;
        }

        m_LeftFakeTop.SetActive(m_TabContent.LeftSpeakerID != GlobeVar.INVALID_ID && !string.IsNullOrEmpty(m_TabContent.SpeakerNameL));

        m_szNameLabelR.text = "";
        if (m_TabContent.SpeakerNameR.Length > 0)
        {
            m_szNameLabelR.text = m_TabContent.SpeakerNameR;
        }
        m_RightFakeTop.SetActive(m_TabContent.RightSpeakerID != GlobeVar.INVALID_ID && !string.IsNullOrEmpty(m_TabContent.SpeakerNameR));

        //播放声音
        if (m_TabContent.SoundID > GlobeVar.INVALID_ID)
        {
            GameManager.SoundManager.PlayRealSound(m_TabContent.SoundID);
            m_RealSoundId = m_TabContent.SoundID;
        }

        // 刷新半身像
        //RefreshFakeObj(m_TabContent);
        RefreshExInfo();

        //播放动作
        if (GlobeVar.INVALID_ID != m_TabContent.LeftAnim)
        {
            if (m_TabContent.LeftSpeakerID == GlobeVar.MAIN_PLAYER_SP_ID || GlobeVar.INVALID_ID != Utils.GetSpecialHeroID(m_TabContent.LeftSpeakerID))
            {
                if (ObjManager.MainPlayer != null)
                {
                    ObjManager.MainPlayer.PlayAnim(m_TabContent.LeftAnim);
                }
            }
            else
            {
                Obj_NPC _npcLeft = ObjManager.GetObjNPCBySceneNPCID(m_TabContent.LeftSpeakerID);
                if (null != _npcLeft)
                {
                    _npcLeft.PlayAnim(m_TabContent.LeftAnim);
                }
            }
        }
        if (GlobeVar.INVALID_ID != m_TabContent.RightAnim)
        {
            if (m_TabContent.RightSpeakerID == GlobeVar.MAIN_PLAYER_SP_ID || GlobeVar.INVALID_ID != Utils.GetSpecialHeroID(m_TabContent.RightSpeakerID))
            {
                if (ObjManager.MainPlayer != null)
                {
                    ObjManager.MainPlayer.PlayAnim(m_TabContent.RightAnim);
                }
            }
            else
            {
                Obj_NPC _npc = ObjManager.GetObjNPCBySceneNPCID(m_TabContent.RightSpeakerID);
                if (null != _npc)
                {
                    _npc.PlayAnim(m_TabContent.RightAnim);
                }
            }
        }

        //记录时间
        ResetDialogTime();
        if (m_TabContent.LifeTime > 0)
        {
            m_fStoryShowTime = Time.time;
            m_fStoryLifeTime = m_TabContent.LifeTime;
        }
    }
    public static string m_recordData = "";
    public bool m_bIsUserSkip = true;

    private void RefreshExInfo()
    {
        if(m_TabContent==null)
        {
            return;
        }
        if(m_TabContent.ExInfoID==GlobeVar.INVALID_ID )
        {
            RefreshFakeObj(m_TabContent);
            return;
        }
        //检查是否需要roleshow
        Tab_StoryRoleShow tabRoleShow = TableManager.GetStoryRoleShowByID(m_TabContent.ExInfoID, 0);
        if(tabRoleShow==null)
        {
            return;
        }
        //0 左边 1 右边。如果都不是按正常处理
        if(tabRoleShow.Area!=0 && tabRoleShow.Area!=1)
        {
            RefreshFakeObj(m_TabContent);
            return;
        }
        FakeRtInstance ins;
        switch (tabRoleShow.Area)
        {
            case 0://左
                //左边做特殊显示
                ins = m_FakeList[0];
                ins.Refresh(GlobeVar.INVALID_ID, null);
                RefreshFakeObj(m_TabContent.RightSpeakerID, m_TabContent.RightHead, m_TabContent.RightHeadAnim, 1);//右边按照表格显示
                break;
            case 1://右
                RefreshFakeObj(m_TabContent.LeftSpeakerID, m_TabContent.LeftHead, m_TabContent.LeftHeadAnim, 0);//左边安表格显示
                //右边做特殊显示
                ins = m_FakeList[1];
                ins.Refresh(GlobeVar.INVALID_ID, null);
                break;
        }
        StartCoroutine(ShowRoleExInfo(tabRoleShow));
    }

    /// <summary>
    /// 走ExInfo逻辑
    /// </summary>
    /// <param name="flag">0 更新左边信息  1更新右边信息</param>
    IEnumerator ShowRoleExInfo(Tab_StoryRoleShow tabRoleShow)
    {
        if(tabRoleShow==null)
        {
            yield break;
        }
        yield return new WaitForSeconds(m_TweenWait);
        if (tabRoleShow.Area==0)
        {
            if(m_TabContent!=null)
            {
                if(tabRoleShow!=null)
                {
                    m_LeftExInfoRoot.SetActive(true);

                    m_LeftExInfoBGTween.PlayForward();
                    if(tabRoleShow.getDescCount()>=3)
                    {
                        m_LeftExInfoDesc1.text = tabRoleShow.GetDescbyIndex(0);
                        m_LeftExInfoDesc2.text = tabRoleShow.GetDescbyIndex(1);
                        m_LeftExInfoDesc3.text = tabRoleShow.GetDescbyIndex(2);
                    }
                    m_LeftExInfoCV.text = "声优 "+tabRoleShow.CV;
                    m_LeftExInfoName.text = tabRoleShow.Name;
                }
                else
                {
                    m_LeftExInfoRoot.SetActive(false);
                }

            }

        }
        else if (tabRoleShow.Area == 1)
        {
            if (m_TabContent != null)
            {
                if (tabRoleShow != null)
                {
                    m_RightExInfoRoot.SetActive(true);

                    m_RightExInfoBGTween.PlayForward();
                    if(tabRoleShow.getDescCount()>=3)
                    {
                        m_RightExInfoDesc1.text = tabRoleShow.GetDescbyIndex(0);
                        m_RightExInfoDesc2.text = tabRoleShow.GetDescbyIndex(1);
                        m_RightExInfoDesc3.text = tabRoleShow.GetDescbyIndex(2);
                        m_RightExInfoCV.text = "声优 " + tabRoleShow.CV;
                        m_RightExInfoName.text = tabRoleShow.Name;
                    }
                }
                else
                {
                    m_RightExInfoRoot.SetActive(false);
                }
            }
        }
    }


    /// <summary>
    /// 初始化当前ExInfo
    /// </summary>
    void ResetExInfo()
    {
        m_IsTweenPlayFinish = false;
        m_bIsExInfoFakeShow = false;
        m_LeftExInfoBGTween.ResetToBeginning();
        m_LeftExInfoDescTween.ResetToBeginning();
        m_LeftExInfoNameTween.ResetToBeginning();
        m_LeftExInfoCVTween.ResetToBeginning();
        m_RightExInfoBGTween.ResetToBeginning();
        m_RightExInfoDescTween.ResetToBeginning();
        m_RightExInfoNameTween.ResetToBeginning();
        m_RightExInfoCVTween.ResetToBeginning();
        m_LeftExInfoRoot.SetActive(false);
        m_RightExInfoRoot.SetActive(false);
        m_FakeList[0].SetScale(m_LeftFakeObjScale);
        m_FakeList[1].SetScale(m_RightFakeObjScale);
        m_LeftFakeObjScale = Vector3.one;
        m_RightFakeObjScale = Vector3.one;
        StopCoroutine(ShowRoleExInfo(null));
    }

    /// <summary>
    /// 把当前ExInfo的所有拖到结束状态
    /// </summary>
    void SetExInfoEnd()
    {
        if (!m_bIsExInfoFakeShow)
        {
            RefreshFakeObj(m_TabContent);
            m_bIsExInfoFakeShow = true;
            if (m_LeftExInfoRoot.activeSelf)
            {
                m_LeftFakeObjScale = m_FakeList[0].GetScale();
                m_FakeList[0].SetScale(m_LeftFakeObjScale * 1.5f);
            }
            if (m_RightExInfoRoot.activeSelf)
            {
                m_RightFakeObjScale = m_FakeList[1].GetScale();
                m_FakeList[1].SetScale(m_RightFakeObjScale * 1.5f);
            }
        }
        if(!m_IsTweenPlayFinish)
        {
            if (m_LeftExInfoRoot.activeSelf)
            {
                m_LeftExInfoBGTween.ResetToBeginning(false);
                m_LeftExInfoDescTween.ResetToBeginning(false);
                m_LeftExInfoCVTween.ResetToBeginning(false);
                m_LeftExInfoNameTween.ResetToBeginning(false);
            }
            if (m_RightExInfoRoot.activeSelf)
            {
                m_RightExInfoBGTween.ResetToBeginning(false);
                m_RightExInfoDescTween.ResetToBeginning(false);
                m_RightExInfoCVTween.ResetToBeginning(false);
                m_RightExInfoNameTween.ResetToBeginning(false);
            }
        }
    }

    /// <summary>
    /// BG动画播放结束的回调
    /// </summary>
    void OnBGTweenPlayFinish()
    {
        RefreshFakeObj(m_TabContent);
        if (m_bIsExInfoFakeShow)
        {
            return;
        }
        m_bIsExInfoFakeShow = true;
        if (m_LeftExInfoRoot.activeSelf)
        {
            m_LeftFakeObjScale = m_FakeList[0].GetScale();
            m_FakeList[0].SetScale(m_LeftFakeObjScale * 1.5f);
            m_LeftExInfoNameTween.PlayForward();
        }
        if (m_RightExInfoRoot.activeSelf)
        {
            m_RightFakeObjScale = m_FakeList[1].GetScale();
            m_FakeList[1].SetScale(m_RightFakeObjScale * 1.5f);
            m_RightExInfoNameTween.PlayForward();
        }
    }

    /// <summary>
    /// Name动画播放结束的回调
    /// </summary>
    void OnNameTweenPlayFinish()
    {
        if (m_LeftExInfoRoot.activeSelf)
        {
            m_LeftExInfoDescTween.PlayForward();
        }
        if (m_RightExInfoRoot.activeSelf)
        {
            m_RightExInfoDescTween.PlayForward();
        }
    }

    /// <summary>
    /// DESC动画播放结束的回调
    /// </summary>
    void OnDescTweenPlayFinish()
    {
        if (m_LeftExInfoRoot.activeSelf)
        {
            m_LeftExInfoCVTween.PlayForward();
        }
        if (m_RightExInfoRoot.activeSelf)
        {
            m_RightExInfoCVTween.PlayForward();
        }
    }

    void OnCVTweenPlayFinish()
    {
        m_IsTweenPlayFinish = true;
    }


    /// <summary>
    /// ExInfo的tween动画是否还在播放
    /// </summary>
    private bool IsExInfoTweenPlayingNow()
    {
        if (m_RightExInfoRoot.activeSelf)
        {
            if (m_RightExInfoBGTween.isActiveAndEnabled || m_RightExInfoDescTween.isActiveAndEnabled)
            {
                return true;
            }
        }
        if (m_LeftExInfoRoot.activeSelf)
        {
            if (m_LeftExInfoBGTween.isActiveAndEnabled || m_LeftExInfoDescTween.isActiveAndEnabled)
            {
                return true;
            }
        }
        return false;
    }

    //播放下一段
    public void PlayNext()
    {        
        if (null == m_StoryContent)
        {
            LogModule.ErrorLog("ShowNextDialog StoryContent is null");
            return;
        }

        MsdkReportData.RecordStoryData(m_nStoryID, m_bIsUserSkip);

        m_bIsUserSkip = true;
        SetExInfoEnd();
        //如果打字机效果还没有结束，则点击之后先结束效果，将文字全部显示
        //否则跳过这句话
        if (m_ContentTE.isActive)
        {
            //把所有UI展开    
            m_ContentTE.Finish();
        }
        else
        {
            m_nCurStep++;
            if(m_TabContent!=null)
            {
                int ret = m_TabContent.ExInfoID;
                if (ret !=GlobeVar.INVALID_ID)
                {
                    Tab_StoryRoleShow tabRoleShow = TableManager.GetStoryRoleShowByID(ret, 0);
                    if (tabRoleShow != null && tabRoleShow.Area == 2)
                    {

                        StoryRoleShow.tabRoleShow = tabRoleShow;
                        UIManager.ShowUI(UIInfo.StoryRoleShow);
                        //通知story模块暂停
                        if (GameManager.storyManager != null)
                        {
                            GameManager.storyManager.IsPause = true;
                        }
                    }   
                }
            }
            if (m_nCurStep <= 0 || m_nCurStep >= m_StoryContent.Count)
            {
                //结束
                Finish();
            }
            else
            {
                PlayStory();
            }          
        }
       
    }

	public void Finish()
	{
        if (m_Finished)
            return;

        //如果有事件，则事件结束
        if (null != m_Event)
		{
			m_Event.Leave();
		}

        m_nLastStoryID = m_nStoryID;
        StartCoroutine(DelayCloseUI());

        m_Finished = true;
	}

    // 剧情大跳
    public void Skip()
    {
        MsdkReportData.RecordStoryData(m_nStoryID, m_bIsUserSkip);

        if (m_Event == null)
        {
            Finish();
            return;
        }

        if (GameManager.CDManager.GetCoolDown(COOLDOWN_TYPE.STORY_SKIP))
            return;

        var storyMgr = GameManager.storyManager;
        if (null != storyMgr)
        {
            int bc = storyMgr.GetCurBranchCnt();
            // 如果是剧情最后一段, 或者有分支, 则只跳过当前步骤
            if (bc == 0 || bc > 1)
            {
                Finish();
                GameManager.CDManager.SetCoolDown(COOLDOWN_TYPE.STORY_SKIP, 1);
            }
            else
            {
                // 如果成功进入跳步流程, 立个flag
                storyMgr.RequestSkip();
                if (storyMgr.SkipMode)
                {
                    m_BtnSkip.SetActive(false);

                    UIManager.ShowUI(UIInfo.StorySkipRoot);

                    GameManager.CDManager.SetCoolDown(COOLDOWN_TYPE.STORY_SKIP, 1);
                }
            }
        }
    }
    
    IEnumerator DelayCloseUI()
    {
        yield return new WaitForSeconds(0.2f);

        if (m_nLastStoryID == m_nStoryID)
        {
            //上次对话剧情ID和本次相同，表示还没有更换剧情，直接关闭
            UIManager.CloseUI(UIInfo.StoryPlayer);
        }
    }

    public void RefreshFakeObj(Tab_StoryContent tab)
    {
        if (tab == null)
            return;

        RefreshFakeObj(tab.LeftSpeakerID, tab.LeftHead, tab.LeftHeadAnim, 0);
        RefreshFakeObj(tab.RightSpeakerID, tab.RightHead, tab.RightHeadAnim, 1);
    }

    int GetFake(int speakerId, ref AvatarLoadInfo outLoadInfo)
    {
        if (speakerId == GlobeVar.INVALID_ID)
            return speakerId;

        //只有指定Hero的時候才需要outLoadInfo
        //所以一开始统一改为null
        outLoadInfo = null;

        //StoryContent表中SpeakerID是-9999的对应Story表中的StoryModel, 对话界面播放动画时会操作MainPlayer
        //StoryContent表中SpeakerID是SceneNpcId时, 对应Story表中StroyNPC, 对话界面播放动画时, 会操作场景中相应的npc
        //SceneNpc支持-9000~-9003的特殊主角id
        if (speakerId == GlobeVar.MAIN_PLAYER_SP_ID && GameManager.storyManager != null && GameManager.storyManager.CurStoryTable != null)
        {
            int storychar = GameManager.storyManager.CurStoryTable.StoryModel;
            if (storychar == GlobeVar.MAIN_PLAYER_SP_ID)
            {
                // story表中主角为游戏主角
                if (GameManager.PlayerDataPool == null || GameManager.PlayerDataPool.PlayerHeroData == null)
                    return GlobeVar.INVALID_ID;

                Hero hero = GameManager.PlayerDataPool.PlayerHeroData.GetCurHero();
                if (hero == null)
                    return GlobeVar.INVALID_ID;

                int charIdAlt = hero.GetCharModelId();
                if (charIdAlt == GlobeVar.INVALID_ID)
                    return GlobeVar.INVALID_ID;

                Tab_CharModel _charModel = TableManager.GetCharModelByID(charIdAlt, 0);
                if (null == _charModel)
                    return GlobeVar.INVALID_ID;

                outLoadInfo = ObjManager.MainPlayer.OrgAvatarInfo;
                return _charModel.StoryFakeObjID;
            }
        }
        else
        {
            Tab_SceneNpc tabSceneNpc = TableManager.GetSceneNpcByID(speakerId, 0);
            if (tabSceneNpc != null)
            {
                int realChar = Utils.GetRealCharModel(tabSceneNpc.DataID);
                
                // CharModel表用的模型资源和Fake表用的模型资源可能不一样, ccb2.5临时这样处理一下, 策划说后续会跟进, 改成一样的
                bool isSpecialChar = (tabSceneNpc.DataID == GlobeVar.MAIN_PLAYER_SP_ID ||
                                      Utils.GetSpecialHeroID(tabSceneNpc.DataID) != GlobeVar.INVALID_ID);
                if (isSpecialChar)
                {
                    outLoadInfo = new AvatarLoadInfo();
                    outLoadInfo.m_BodyId = realChar;

                    int realColor = Utils.GetRealDyeColorId(tabSceneNpc.DataID);
                    Tab_DyeColor tabColor = TableManager.GetDyeColorByID(realColor, 0);
                    if (tabColor != null)
                    {
                        outLoadInfo.LoadDyeColor(realColor);
                    }    
                }
                else
                {
                    outLoadInfo = null;
                }

                Tab_CharModel tabModel = TableManager.GetCharModelByID(realChar, 0);
                if (tabModel != null)
                {
                    return tabModel.StoryFakeObjID;
                }
            }
        }

        return GlobeVar.INVALID_ID;
    }

    //showFlag: 0暗，1亮，-1不显示
    void RefreshFakeObj(int speakerID, int showFlag, int anim, int fakeIdx)
    {
        if (fakeIdx >= m_FakeList.Length)
            return;

        FakeRtInstance ins = m_FakeList[fakeIdx];
        ins.anim = anim;
        ins.effect = GlobeVar.INVALID_ID;
        ins.hightlight = (1 == showFlag);// highlight;

        if (GlobeVar.INVALID_ID == showFlag)
        {
            //不显示FakeObj
            ins.Refresh(GlobeVar.INVALID_ID, null);
        }
        else
        {
            //显示FakeObj
            AvatarLoadInfo info = null;
            int nFakeObjID = (-1 == showFlag ? GlobeVar.INVALID_ID : GetFake(speakerID, ref info));

            //根据info是否为空来确认是否需要加载染色信息
            ins.Refresh(nFakeObjID, info);
        }
    }
}
