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
    public GameObject m_BGObj;

    public GameObject m_BtnSkip;

    public StoryExInfoController m_ExInfo;
    private static Vector3 m_LeftFakeObjScale = Vector3.one;
    private static Vector3 m_RightFakeObjScale = Vector3.one;

    private bool m_bIsPause = false;
    public bool IsPause
    {
        get
        {
            return m_bIsPause;
        }
        set
        {
            m_bIsPause = value;
            //通知story模块暂停
            if (GameManager.storyManager != null)
            {
                GameManager.storyManager.IsPause = value;

            }
        }
    }
    private bool m_bIsInStoryRoleShow = false;

    [SerializeField]
    FakeRtInstance[] m_FakeList;            //半身像rt

    //缓存的相关数据
    private int m_nStoryID = GlobeVar.INVALID_ID;   //缓存当前的故事ID
    private int m_RealSoundId = GlobeVar.INVALID_ID;
    private int m_nLastStoryID = GlobeVar.INVALID_ID;   //上一次对话时候的故事ID
    private List<Tab_StoryContent> m_StoryContent = null;
    private Tab_StoryContent m_TabContent = null;    //当前Tab_StoryContent
    private StoryEvent_Dialog m_Event = null;
    private int m_nCurStep;     //记录在多部的时候的当前步骤
    private float m_fStoryShowTime = -1.0f;    //本次剧情的播放时间
    private float m_fStoryLifeTime = 0.0f;      //本次剧情的持续时间
    private bool m_bForceAutoPlay = false;//是否强制自动播放
    public bool IsAutoPlay
    {
        get { return m_bForceAutoPlay; }
    }

    bool m_Finished = false;

    private static StoryPlayerController m_Instance = null;
    public static StoryPlayerController Instance()
    {
        return m_Instance;
    }

    void Awake()
    {
        m_Instance = this;
        m_ExInfo.gameObject.SetActive(false);
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
        if (m_bIsPause)
        {
            return;
        }
        if (m_fStoryShowTime > 0.0f && m_fStoryLifeTime > 0.0f)
        {
            if ((m_bForceAutoPlay || PlayerPreferenceData.AutoPlayDialog ) && Time.time - m_fStoryShowTime +  StoryRoleShow.KeepTime >= m_fStoryLifeTime)
            {
                PlayNext(true);
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

    private float mEnterStorySpeed = 1f;

    private float EnterStorySpeed
    {
        get { return mEnterStorySpeed; }
        set
        {
            mEnterStorySpeed = value;
            //Debug.LogFormat("enter speed {0}", mEnterStorySpeed);
        }
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

        EnterStorySpeed = Time.timeScale;
        Time.timeScale = 1;

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
        ResetExInfoState();
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
        m_BtnSkip.SetActive(m_TabContent.IsCanSkip == (int)STORY_SKIP_MODE.NORM_SHOW_BTN || 
            m_TabContent.IsCanSkip == (int)STORY_SKIP_MODE.FORCE_NEXT_SHOW_SKIP);
        m_bForceAutoPlay = (m_TabContent.IsCanSkip == (int)STORY_SKIP_MODE.FORCE_NEXT_NOT_SHOW_SKIP || 
            m_TabContent.IsCanSkip == (int)STORY_SKIP_MODE.FORCE_NEXT_SHOW_SKIP);

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

    private void RefreshExInfo()
    {
        if(m_TabContent==null)
        {
            return;
        }
        Tab_StoryRoleShow tabRoleShow = TableManager.GetStoryRoleShowByID(m_TabContent.ExInfoID, 0);
        //检查是否需要roleshow
        //0 左边 1 右边。如果都不是则按正常处理
        if (tabRoleShow == null || tabRoleShow.Area != 0 && tabRoleShow.Area != 1 || m_ExInfo.IsHideing)
        {
            RefreshFakeObj();
            return;
        }
        //如果左右对不上，取消增强展示
        switch(tabRoleShow.Area)
        {
            case 0:
                if (m_TabContent.LeftSpeakerID == GlobeVar.INVALID_ID)
                {
                    RefreshFakeObj();
                    return;
                }
                break;
            case 1:
                if (m_TabContent.RightSpeakerID == GlobeVar.INVALID_ID)
                {
                    RefreshFakeObj();
                    return;
                }
                break;
        }
        FakeRtInstance ins = null;
        //检查完毕 增强展示
        switch (tabRoleShow.Area)
        {
            case 0://左
                //左边做特殊显示，由ExInfo脚本触发显示
                ins = m_FakeList[0];
                ins.Refresh(GlobeVar.INVALID_ID, null);
                RefreshFakeObj(m_TabContent.RightSpeakerID, m_TabContent.RightCustomFake, m_TabContent.RightHead, m_TabContent.RightHeadAnim, 1);//右边按照表格显示
                m_ExInfo.ShowRoleExInfo(tabRoleShow);
                break;
            case 1://右
                RefreshFakeObj(m_TabContent.LeftSpeakerID, m_TabContent.LeftCustomFake, m_TabContent.LeftHead, m_TabContent.LeftHeadAnim, 0);//左边按照表格显示
                //右边做特殊显示，由ExInfo脚本触发显示
                ins = m_FakeList[1];
                ins.Refresh(GlobeVar.INVALID_ID, null);
                m_ExInfo.ShowRoleExInfo(tabRoleShow);
                break;
        }

    }

    /// <summary>
    /// 每次对话初始化fake大小
    /// </summary>
    void ResetExInfoState()
    {
        if (m_FakeList.Length < 2)
        {
            return;
        }

        m_FakeList[0].SetScale(m_LeftFakeObjScale);
        m_FakeList[1].SetScale(m_RightFakeObjScale);
        m_LeftFakeObjScale = Vector3.one;
        m_RightFakeObjScale = Vector3.one;
        if(m_ExInfo.gameObject.activeSelf)
        {
            m_ExInfo.ExInfoFadeOut();
        }
        SetContentTEActive(true);
    }



    //播放下一段
    public void PlayNext(bool isAuto=false)
    {   //已经受过一次影响，将其置0
        if (!isAuto)
        {
            PlatformHelper.RecordEvent("剧情小跳");
        }
        StoryRoleShow.KeepTime = 0;     
        if (null == m_StoryContent)
        {
            LogModule.ErrorLog("ShowNextDialog StoryContent is null");
            return;
        }

        //如果打字机效果还没有结束，则点击之后先结束效果，将文字全部显示
        //否则跳过这句话
        if (m_ContentTE.isActive || m_ExInfo.Active)
        {  
            m_ContentTE.Finish();
            if(m_ExInfo.Active)
            {
                m_ExInfo.SetExInfoEnd();
            }
        }
        else 
        {
            //如果本次没有触发过RoleShow
            if (m_TabContent != null && !m_bIsInStoryRoleShow)
            {
                int ret = m_TabContent.ExInfoID;
                if (ret !=GlobeVar.INVALID_ID)
                {
                    Tab_StoryRoleShow tabRoleShow = TableManager.GetStoryRoleShowByID(ret, 0);
                    if (tabRoleShow != null && tabRoleShow.Area == 2)
                    {
                        m_bIsInStoryRoleShow = true;
                        SetContentTEActive(false);
                        StoryRoleShow.Show(tabRoleShow);
                        IsPause = true;
                        return;
                    }   
                }
            }
            m_nCurStep++;
            m_bIsInStoryRoleShow = false;
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

        Time.timeScale = EnterStorySpeed;
        EnterStorySpeed = 1f;

        StartCoroutine(DelayCloseUI());

        m_Finished = true;
	}

    // 剧情大跳
    public void Skip()
    {
        string eventName = string.Format("[{0},{1}]", GameManager.PlayerDataPool.m_nStoryLine, GameManager.PlayerDataPool.m_nStroyID);
        PlatformHelper.RecordEvent("剧情大跳" + eventName);
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

    /// <summary>
    /// 根据m_TabContent的值，将Fake缩放后显示，每次对话开始前会重置左右fake的大小
    /// </summary>
    /// <param name="fakeIndex">0 = 左  1 = 右</param>
    /// <param name="scale">vec3的缩放值</param>
    public void RefreshFakeObj(int fakeIndex,Vector3 scale)
    {
        if(m_TabContent!=null)
        {
            if(fakeIndex==0)
            {
                RefreshFakeObj(m_TabContent.LeftSpeakerID, m_TabContent.LeftCustomFake, m_TabContent.LeftHead, m_TabContent.LeftHeadAnim, 0);
                if(0<m_FakeList.Length)
                {
                    m_LeftFakeObjScale = m_FakeList[0].GetScale();
                    m_FakeList[0].SetScale(scale);


                }
            }
            else if(fakeIndex==1)
            {
                RefreshFakeObj(m_TabContent.RightSpeakerID, m_TabContent.RightCustomFake, m_TabContent.RightHead, m_TabContent.RightHeadAnim, 1);
                if (1 < m_FakeList.Length)
                {
                    m_RightFakeObjScale = m_FakeList[1].GetScale();
                    m_FakeList[1].SetScale(scale);
                }
            }
        }
    }

    public void RefreshFakeObj()
    {
        if (m_TabContent == null)
            return;

        RefreshFakeObj(m_TabContent.LeftSpeakerID, m_TabContent.LeftCustomFake, m_TabContent.LeftHead, m_TabContent.LeftHeadAnim, 0);
        RefreshFakeObj(m_TabContent.RightSpeakerID, m_TabContent.RightCustomFake, m_TabContent.RightHead, m_TabContent.RightHeadAnim, 1);
    }

    int GetFake(int speakerId, int customFake, ref AvatarLoadInfo outLoadInfo)
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
            // 如果Story表中是-9999
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
            // 如果Story表中是-9000~-9002
            int heroid = Utils.GetSpecialHeroID(storychar);
            if (heroid != GlobeVar.INVALID_ID)
            {
                outLoadInfo = new AvatarLoadInfo();
                outLoadInfo.m_BodyId = Utils.GetModelByHeroId(heroid);
                outLoadInfo.LoadDyeColor(Utils.GetRealDyeColorId(storychar));
                if (customFake != GlobeVar.INVALID_ID)
                {
                    return customFake;
                }
                Tab_CharModel _charModel = TableManager.GetCharModelByID(outLoadInfo.m_BodyId, 0);
                if (null == _charModel)
                    return GlobeVar.INVALID_ID;
                return _charModel.StoryFakeObjID;
            }
            // 如果Story表中是普通charmodelid
            if (customFake != GlobeVar.INVALID_ID)
                return customFake;
            Tab_CharModel storyModel = TableManager.GetCharModelByID(storychar, 0);
            if (null == storyModel)
                return GlobeVar.INVALID_ID;
            return storyModel.StoryFakeObjID;
        }
        else
        {
            Tab_StoryNpc tabStoryNpc = TableManager.GetStoryNpcByID(speakerId, 0);
            if (tabStoryNpc != null)
            {
                int realChar = Utils.GetRealCharModel(tabStoryNpc.CharModelId);
                
                // CharModel表用的模型资源和Fake表用的模型资源可能不一样, ccb2.5临时这样处理一下, 策划说后续会跟进, 改成一样的
                bool isSpecialChar = (tabStoryNpc.CharModelId == GlobeVar.MAIN_PLAYER_SP_ID ||
                                      Utils.GetSpecialHeroID(tabStoryNpc.CharModelId) != GlobeVar.INVALID_ID);
                if (isSpecialChar)
                {
                    outLoadInfo = new AvatarLoadInfo();
                    outLoadInfo.m_BodyId = realChar;

                    int realColor = Utils.GetRealDyeColorId(tabStoryNpc.DyeColorID);
                    Tab_DyeColor tabColor = TableManager.GetDyeColorByID(realColor, 0);
                    if (tabColor != null)
                    {
                        outLoadInfo.LoadDyeColor(realColor);
                    }
                    if (customFake != GlobeVar.INVALID_ID)
                    {
                        return customFake;
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
    void RefreshFakeObj(int speakerID, int customFake, int showFlag, int anim, int fakeIdx)
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
            int nFakeObjID = (-1 == showFlag ? GlobeVar.INVALID_ID : GetFake(speakerID, customFake, ref info));

            //根据info是否为空来确认是否需要加载染色信息
            ins.Refresh(nFakeObjID, info);
        }
    }

    public void SetContentTEActive(bool active)
    {
        m_ContentTE.gameObject.SetActive(active);
        m_LeftFakeTop.SetActive(active);
        m_RightFakeTop.SetActive(active);
        m_BGObj.SetActive(active);
        if(!active && m_FakeList.Length >= 2)
        {
            m_BtnSkip.SetActive(false);
            m_FakeList[0].Refresh(GlobeVar.INVALID_ID, null);
            m_FakeList[1].Refresh(GlobeVar.INVALID_ID, null);
        }
    }

    #region share
    public void OnClickShare()
    {
        ShareWindowControllor.Show(OnShareBegin, OnShareEnd, ShareWindowControllor.SHARECAMERA.SCREEN, ShareWindowControllor.ENUM_SHARETYPE.ENUM_SHARETYPE_STORY);
    }

    public GameObject[] m_shareHideList;
    private bool[] m_isVisibleShareObjs;
    private bool[] m_isVisibleShareObjs_Controller;
    private void OnShareBegin()
    {
        if (null != m_shareHideList && m_shareHideList.Length > 0)
        {
            if (null == m_isVisibleShareObjs)
            {
                m_isVisibleShareObjs = new bool[m_shareHideList.Length];
            }

            for (int i = 0; i < m_shareHideList.Length; ++i)
            {
                if (m_shareHideList[i] != null)
                {
                    m_isVisibleShareObjs[i] = m_shareHideList[i].activeSelf;
                    if (m_isVisibleShareObjs[i])
                    {
                        m_shareHideList[i].SetActive(false);
                    }
                }
                else
                {
                    m_isVisibleShareObjs[i] = false;
                }
            }
        }
    }

    private void OnShareEnd()
    {
        if (null != m_shareHideList && m_shareHideList.Length > 0)
        {
            for (int i = 0; i < m_isVisibleShareObjs.Length; ++i)
            {
                if (m_isVisibleShareObjs[i] && null != m_shareHideList[i])
                {
                    m_shareHideList[i].SetActive(true);
                }
            }
        }
    }
    #endregion
}
