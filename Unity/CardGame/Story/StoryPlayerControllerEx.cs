using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Games.GlobeDefine;

using Games.Table;
using Games.LogicObj;
using Games;

[System.Serializable]
class FakeRtInstance
{
    public FakeObjRt fakeRt;

    [HideInInspector]
    Obj_Fake fakeObj;
    [HideInInspector]
    public int effect = GlobeVar.INVALID_ID;
    [HideInInspector]
    public int anim = GlobeVar.INVALID_ID;
    [HideInInspector]
    public bool hightlight = true;

    int lastEffect = GlobeVar.INVALID_ID;

    public void Reset()
    {
        if (fakeRt != null && fakeRt.rtCamera != null)
        {
            fakeRt.rtCamera.cullingMask = GlobeVar.STORY_FAKE_RT_CAMMASK;
        }
        fakeObj = null;
        effect = GlobeVar.INVALID_ID;
        anim = GlobeVar.INVALID_ID;
        lastEffect = GlobeVar.INVALID_ID;
    }

    public void Refresh(int fakeObjId, AvatarLoadInfo loadinfo = null)
    {
        if (fakeObjId == GlobeVar.INVALID_ID)
        {
            ObjManager.DestroyFakeObj(fakeObj);
            fakeRt.gameObject.SetActive(false);
            fakeRt.fakeObjId = GlobeVar.INVALID_ID;
            lastEffect = GlobeVar.INVALID_ID;
        }
        else
        {
            if (!fakeRt.gameObject.activeSelf)
                fakeRt.gameObject.SetActive(true);

            if (fakeObjId != fakeRt.fakeObjId)
            {
                if (null == loadinfo)
                {
                    fakeObj = fakeRt.Refresh(fakeObjId, OnLoadFinished);
                }
                else
                {
                    fakeObj = fakeRt.Refresh(fakeObjId, loadinfo, OnLoadFinished);
                }
            }
            else
            {
                OnLoadFinished();
            }
        }
    }

    public void OnLoadFinished()
    {
        if (fakeObj == null)
            return;

        if (fakeObj.UseRightCoord())
            fakeRt.texture.flip = UIBasicSprite.Flip.Nothing;
        else if (fakeRt.rightFake)
            fakeRt.texture.flip = UIBasicSprite.Flip.Horizontally;
        else
            fakeRt.texture.flip = UIBasicSprite.Flip.Nothing;

        // 动作
        if (anim != GlobeVar.INVALID_ID)
        {
            fakeObj.PlayAnimation(anim);
        }
        // 特效
        if (lastEffect != effect)
        {
            if (lastEffect != GlobeVar.INVALID_ID)
            {
                fakeObj.StopEffect(lastEffect);
            }
            if (effect != GlobeVar.INVALID_ID)
            {
                fakeObj.PlayEffect(effect);
                lastEffect = effect;
            }
        }
        // 灰化
        if (fakeRt.texture != null)
        {
            Color color = fakeRt.texture.color;
            if (hightlight)
            {
                color.r = 1f;
                color.g = 1f;
                color.b = 1f;
            }
            else
            {
                color.r = 0.5f;
                color.g = 0.5f;
                color.b = 0.5f;
            }
            fakeRt.texture.color = color;
        }
    }
}

public class StoryPlayerControllerEx : UIControllerBase<StoryPlayerControllerEx>
{
    [SerializeField]
    FakeRtInstance[] m_FakeList;

    [SerializeField]
    UILabel m_Scenario;
    [SerializeField]
    UILabel m_CharName;
    [SerializeField]
    GameObject m_BtnSkip;
    [SerializeField]
    GameObject m_Next;

    [SerializeField]
    UILabel m_szContentLabel;       	//文字内容
    [SerializeField]
    TypewriterEffect m_ContentTE;    //文字打字机效果

    //缓存的相关数据
    int m_nStoryID = GlobeVar.INVALID_ID;   //缓存当前的故事ID
    int m_RealSoundId = GlobeVar.INVALID_ID;
    int m_nLastStoryID = GlobeVar.INVALID_ID;   //上一次对话时候的故事ID
    
    List<Tab_StoryContentEx> m_StoryContentEx = null;
    StoryEvent_DialogEx m_Event = null;

    private int m_nCurStep;     //记录在多步的时候的当前步骤
    private float m_fStoryShowTime = -1.0f;    //本次剧情的播放时间
    private float m_fStoryLifeTime = 0.0f;      //本次剧情的持续时间
    private bool m_LastTEState = false; //上一帧打字机是否在运行
    private bool m_bForceAutoPlay = false;//是否强制自动播放

    void Awake()
    {
        SetInstance(this);
    }
    void OnDestroy()
    {
        base.Release();
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
        if (m_fStoryShowTime > 0.0f && m_fStoryLifeTime > 0.0f)
        {
            if ((m_bForceAutoPlay || PlayerPreferenceData.AutoPlayDialog )&& Time.time - m_fStoryShowTime >= m_fStoryLifeTime)
            {
                PlayNext();
            }
        }
        // 更新下一步状态
        bool curTEState = m_ContentTE.isActive;
        if (m_LastTEState && !curTEState)
        {
            m_Next.SetActive(true);
        }
        m_LastTEState = curTEState;
    }

    void ResetDialogTime()
    {
        m_fStoryLifeTime = 0.0f;
        m_fStoryShowTime = -0.1f;
    }

    void CleanUp()
    {
        m_nCurStep = 0;
        m_StoryContentEx = null;
        m_Event = null;

        ResetDialogTime();
    }

    public bool InitUI(int nStoryID, StoryEvent_DialogEx sEvent)
    {
        //Debug.Log("参数storyID=" + nStoryID);
        CleanUp();

        m_nStoryID = nStoryID;
        m_nCurStep = 0;
        m_StoryContentEx = TableManager.GetStoryContentExByID(nStoryID);
        m_Event = sEvent;
        if (null == m_StoryContentEx || m_StoryContentEx.Count <= 0)
        {
            UIManager.CloseUI(UIInfo.StoryPlayerEx);
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
        //Debug.Log(m_nCurStep);
        //获取当前的列表项
        if (m_nCurStep < 0 || m_nCurStep >= m_StoryContentEx.Count)
        {
            return;
        }

        Tab_StoryContentEx _tabContent = m_StoryContentEx[m_nCurStep];
        if (null == _tabContent)
        {
            return;
        }

        // 刷新半身像
        RefreshFakeObj(_tabContent);

        if (_tabContent.CamShake != GlobeVar.INVALID_ID)
        {
            GameManager.CameraManager.PlayCameraRock(_tabContent.CamShake);
        }

        // 情景名
        if (!string.IsNullOrEmpty(_tabContent.ScenarioName))
            m_Scenario.text = _tabContent.ScenarioName;

        // 角色名
        string realName = _tabContent.SpeakerName;
        if (realName == GlobeVar.MAIN_PLAYER_SP_ID.ToString())
        {
            if (GameManager.PlayerDataPool != null && GameManager.PlayerDataPool.PlayerHeroData != null)
            {
                Hero hero = GameManager.PlayerDataPool.PlayerHeroData.GetCurHero();
                if (hero != null)
                    realName = hero.GetName();
            }
        }
        m_CharName.text = realName;

        // 是否可跳过
        m_BtnSkip.SetActive(_tabContent.IsCanSkip == 1);
        m_bForceAutoPlay = ( _tabContent.IsCanSkip == 2 );

        // 显示内容
        // 支持换行符#r
        string szContent = _tabContent.Content.Replace("#r", "\n");
        m_szContentLabel.text = szContent;

        m_ContentTE.ResetToBeginning();
        m_LastTEState = true;

        // 隐藏下一步
        m_Next.SetActive(!m_LastTEState);

        //播放声音
        if (_tabContent.SoundID > GlobeVar.INVALID_ID)
        {
            GameManager.SoundManager.PlayRealSound(_tabContent.SoundID);
            m_RealSoundId = _tabContent.SoundID;
        }

        //记录时间
        ResetDialogTime();
        if (_tabContent.LifeTime > 0)
        {
            m_fStoryShowTime = Time.time;
            m_fStoryLifeTime = _tabContent.LifeTime;
        }
    }
    /// <summary>
    /// 当点击跳过的时候触发
    /// </summary>
    public void PlayNextStoryContent()
    {
        if (null == m_StoryContentEx)
        {
            LogModule.ErrorLog("ShowNextDialog StoryContent is null");
            return;
        }

        //如果打字机效果还没有结束，则点击之后先结束效果，将文字全部显示
        //否则跳过这句话
        if (m_ContentTE.isActive)
        {
            m_ContentTE.Finish();
        } 
        else
        {
            m_nCurStep++;
            //拿到下一阶段节点
            for (; m_nCurStep < m_StoryContentEx.Count; m_nCurStep++)
            {
                //拿到下一可跳节点
                if (m_StoryContentEx[m_nCurStep].IsJumpNode == 1)                 //找到了
                {
                    break;
                }
            }
            if (m_nCurStep <= 0 || m_nCurStep >= m_StoryContentEx.Count)
            {
                 //结束
                 Finish();
             }else
             {
                 PlayStory();
             }
        }
    }

    //播放下一段
    public void PlayNext()
    {
        if (null == m_StoryContentEx)
        {
            LogModule.ErrorLog("ShowNextDialog StoryContent is null");
            return;
        }

        //如果打字机效果还没有结束，则点击之后先结束效果，将文字全部显示
        //否则跳过这句话
        if (m_ContentTE.isActive)
        {
            m_ContentTE.Finish();
        }
        else
        {
            m_nCurStep++;

            if (m_nCurStep <= 0 || m_nCurStep >= m_StoryContentEx.Count)
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
        //如果有事件，则事件结束
        if (null != m_Event)
        {
            m_Event.Leave();
        }

        m_nLastStoryID = m_nStoryID;
        StartCoroutine(DelayCloseUI());
    }
    
    IEnumerator DelayCloseUI()
    {
        yield return new WaitForSeconds(0.2f);

        if (m_nLastStoryID == m_nStoryID)
        {
            //上次对话剧情ID和本次相同，表示还没有更换剧情，直接关闭
            UIManager.CloseUI(UIInfo.StoryPlayerEx);
        }
    }

    public void LeaveStory()
    {
        if (GameManager.CDManager.GetCoolDown(COOLDOWN_TYPE.STORY))
            return;

        //向服务器发消息申请离开剧情模式
        StoryHandler.ReqLeaveStory();
        GameManager.CDManager.SetCoolDown(COOLDOWN_TYPE.STORY, 2f);
        //MessageBoxController.OpenOKCancel(111, 112, LeaveStoryModeOK);
    }

    void LeaveStoryModeOK()
    {
        if (GameManager.LoadingScene)
        {
            //LogModule.DebugLog("reqest leaving story while loading scene, abort leaving");
            return;
        }

        if (null != GameManager.storyManager && LoginData.user != null)
        {
            if (GameManager.m_bOffLineMode)
            {
                GameManager.storyManager.LeaveStoryMode(LoginData.user.scene);
            }
            else
            {
                if (GameManager.CDManager.GetCoolDown(COOLDOWN_TYPE.STORY))
                    return;

                //向服务器发消息申请离开剧情模式
                StoryHandler.ReqLeaveStory();
                GameManager.CDManager.SetCoolDown(COOLDOWN_TYPE.STORY, 2f);
            }
        }
    }

    public void RefreshFakeObj(Tab_StoryContentEx tab)
    {
        if (tab == null)
            return;
        
        for (int i = 0; i < tab.getSpeakerCount(); ++i)
        {
            int realChar = tab.GetSpeakerbyIndex(i);
            if (realChar == GlobeVar.MAIN_PLAYER_SP_ID)
            {
                if (GameManager.PlayerDataPool == null || GameManager.PlayerDataPool.PlayerHeroData == null)
                    continue;
                
                Hero hero = GameManager.PlayerDataPool.PlayerHeroData.GetCurHero();
                if (hero == null)
                    continue;

                int playModelId = hero.GetCharModelId();
                if (playModelId == GlobeVar.INVALID_ID)
                    continue;

                Tab_CharModel _charModel = TableManager.GetCharModelByID(playModelId, 0);
                if (null == _charModel)
                    continue;

                realChar = _charModel.StoryFakeObjID;
            }
            else if (GlobeVar.INVALID_ID != Utils.GetSpecialHeroID(realChar))
            {
                int nHeroID = Utils.GetSpecialHeroID(realChar);
                if (null != GameManager.PlayerDataPool.PlayerHeroData)
                {
                    Hero hero = GameManager.PlayerDataPool.PlayerHeroData.GetHero(nHeroID);
                    if (hero == null)
                        continue;

                    int playModelId = hero.GetCharModelId();
                    if (playModelId == GlobeVar.INVALID_ID)
                        continue;

                    Tab_CharModel _charModel = TableManager.GetCharModelByID(playModelId, 0);
                    if (null == _charModel)
                        continue;

                    realChar = _charModel.StoryFakeObjID;

                    FakeRtInstance _spIns = m_FakeList[i];
                    _spIns.anim = tab.GetAnimebyIndex(i);
                    _spIns.effect = tab.GetEffectbyIndex(i);
                    _spIns.hightlight = tab.GetHighlightedbyIndex(i) == 1;
                    //加载一个loadinfo，里面包含染色信息
                    AvatarLoadInfo _info = new AvatarLoadInfo();
                    if (null != _info)
                    {
                        //获取当前该英雄的染色信息
                        if (null != GameManager.PlayerDataPool.PlayerHeroData)
                        {
                            _info.LoadDyeColor(GameManager.PlayerDataPool.PlayerHeroData.GetHeroDyeColor(nHeroID));
                        }
                    }

                    //特殊处理过染色，直接返回
                    _spIns.Refresh(realChar, _info);
                    return;
                }
            }

            FakeRtInstance ins = m_FakeList[i];
            ins.anim = tab.GetAnimebyIndex(i);
            ins.effect = tab.GetEffectbyIndex(i);
            ins.hightlight = tab.GetHighlightedbyIndex(i) == 1;
            ins.Refresh(realChar, null);
        }
    }
}
