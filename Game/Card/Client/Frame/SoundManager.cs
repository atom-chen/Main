/********************************************************************************
 *	文件名：	SoundManager.cs
 *	全路径：	\Script\Scene\SoundManager.cs
 *	创建人：	王华
 *	创建时间：2015-11-04
 *
 *	功能说明：声音管理器
 *	修改记录：
 *	2013-11-22 先去掉协程，把声音缓冲池分成2个，一个场景相关（比如背景音乐），一个非场景相关的（比如人物技能）
 *	场景相关的3D音源绑在人物身上，随人物移动
 *	2013-12-12 把声音表格第一列改为名称，背景音乐挂在摄像机上
 *	2013-12-18 非场景音乐（比如技能音乐）的中心点挂在主角身上,设置PanLevel和Spread
 *	2013-12-24 PanLevel和Spread配表设置
 *	2014-03-13 背景音乐和场景各种音效使用同一个池子，池子大小固定，采用最长时间未时间即替换算法更新池子内容，
 *	初始时，立即创建m_SFXChannelsCount个AudioSource，用于播放背景音乐和音效，同时能够播放的最大声音数也是m_SFXChannelsCount个，
 *	另外，AudioSource挂接在SoundManager这个物体上
*********************************************************************************/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Games.GlobeDefine;
using Games.LogicObj;
using Games.Table;
using Random = UnityEngine.Random;


[Serializable]
public class SoundClip
{
    private AudioClip m_Audioclip;
    public AudioClip Audioclip
    {
        get { return m_Audioclip; }
        set { m_Audioclip = value; }
    }

    //表格数据
    public short m_priority = 128;
    public string m_name = string.Empty;
    public string m_path = string.Empty;
    public float m_minDistance = 10;
    public float m_volume = 1.0f;
    public float m_delay = 0.0f;
    public float m_panLevel = 0.0f;
    public float m_spread = 0.0f;
    public bool m_isLoop = false;
    public short m_curMaxPlayingCount = 1;
    public int m_uID = -1;

    //运行时数据
    public float m_LastActiveTime = 0.0f;  //上次活跃时间,上次播放时间
}

public class SoundClipPools
{
    public class SoundClipParam
    {
        public delegate void OnSoundEffectPlay(int nSoundId);

        public SoundClipParam(float volumeFactor, OnSoundEffectPlay delOnSoundEffectPlay = null)
        {
            m_volumeFactor = volumeFactor;
            m_fadeInTime = 0;
            m_fadeOutTime = 0;
            m_delOnSoundEffectPlay = delOnSoundEffectPlay;
        }

        public SoundClipParam(int clipId, float fadeOutTime, float fadeInTime)
        {
            m_volumeFactor = 1;
            m_fadeInTime = fadeInTime;
            m_fadeOutTime = fadeOutTime;
            m_clipID = clipId;
        }

        public SoundClipParam(int clipId)
        {
            m_volumeFactor = 1;
            m_fadeInTime = 0;
            m_fadeOutTime = 0;
            m_clipID = clipId;
        }

        public float m_volumeFactor;
        public float m_fadeOutTime;
        public float m_fadeInTime;
        public int m_clipID;
        public OnSoundEffectPlay m_delOnSoundEffectPlay;
    }
    public delegate void GetSoundClipDelegate(SoundClip soundClip, SoundClipParam param);

    private Dictionary<int, SoundClip> m_SoundClipMap = new Dictionary<int,SoundClip>();    //音效列表，限制最大数量MAX

    const int PARAM_COUNT_OF_LOAD = 4; // 执行加载资源时的参数格式一定是4个,不多也不少
    const string TAG_OF_LOAD = "sd_load"; // 执行声音加载时的tag标记

    /// <summary>
    /// 根据声音名称得到SoundClip，不存在会自动添加
    /// </summary>
    /// <param name="name">名称</param>
    /// <returns></returns>
    public void GetSoundClip(int nSoundId, GetSoundClipDelegate delFun, SoundClipParam param)
    {
        if (nSoundId >= 0)
        {
            if (m_SoundClipMap.ContainsKey(nSoundId))
            {
                //更新上次播放时间
                m_SoundClipMap[nSoundId].m_LastActiveTime = Time.realtimeSinceStartup;
                if (null != delFun) delFun(m_SoundClipMap[nSoundId], param);
                return;
            }
            else
            {
                if (m_SoundClipMap.Count > SoundManager.m_SFXChannelsCount) //超过最大值，删除最长时间未使用的
                {
                    //LogModule.DebugLog("Warnning m_SoundClipList.Count > " + SoundManager.m_SFXChannelsCount);
                    RemoveLastUnUsedClip();
                }

                Tab_Sounds soundsTab = TableManager.GetSoundsByID(nSoundId,0);
                if (soundsTab == null)
                {
                    //LogModule.DebugLog("sound id " + nSoundId.ToString() + " is null");
                    if (null != delFun) delFun(null, param);
                    return;
                }

                string fullsoundName = soundsTab.FullPathName;
                if (string.IsNullOrEmpty(fullsoundName))
                {
                    if (null != delFun) delFun(null, param);
                    return;
                }

                //if (GameManager.CurActiveScene == null)
                //{
                //    if (null != delFun) delFun(null, param);
                //    return;
                //}

                BundleTask task = new BundleTask(OnLoadSound);
                task.AddParam(soundsTab);
                task.AddParam(delFun);
                task.AddParam(param);
                task.AddParam(fullsoundName);
                task.Add(BundleTask.BundleType.SOUND, fullsoundName, TAG_OF_LOAD);
                AssetLoader.Instance.AddBundleTask(task);
            }
        }
    }

    void OnLoadSound( BundleTask task )
    {
        if (task.ParamCount() < PARAM_COUNT_OF_LOAD)
        {
            return; // 加载参数不足
        }
            
        object param1 = task.GetParamByIndex(0);
        object param2 = task.GetParamByIndex(1);
        object param3 = task.GetParamByIndex(2);
        string soundPath = task.GetParamByIndex(3) as string;
        AudioClip curAudioClip = task.GetFinishObjByTag(TAG_OF_LOAD) as AudioClip; // 这里不判空,因为下面的函数判空后会有提示,并且还有相应的处理

        OnLoadSound(soundPath, curAudioClip, param1, param2, param3);
    }

    void OnLoadSound(string soundPath, AudioClip curAudioClip, object param1, object param2, object param3 = null)
    {
        SoundClip clip = new SoundClip();
        clip.Audioclip = curAudioClip;
        GetSoundClipDelegate delFun = param2 as GetSoundClipDelegate;
        SoundClipParam soundClipParam = param3 as SoundClipParam;
        Tab_Sounds soundsTab = param1 as Tab_Sounds;
        if (null == clip.Audioclip)
        {
            //LogModule.DebugLog("sound clip " + soundPath + " is null");
            if (null != delFun) delFun(null, soundClipParam);
            return;
        }

        //if (clip.Audioclip.loadState != AudioDataLoadState.Loaded)
        //{
        //    LogModule.DebugLog("Cann't decompress the sound resource " + soundPath);
        //    if (null != delFun) delFun(null, soundClipParam);
        //    return;
        //}
        clip.m_LastActiveTime = Time.realtimeSinceStartup;
        clip.m_delay = soundsTab.Delay;
        clip.m_minDistance = soundsTab.MinDistance;
        clip.m_panLevel = soundsTab.PanLevel;
        clip.m_spread = soundsTab.Spread;
        clip.m_volume = soundsTab.Volume;
        clip.m_isLoop = soundsTab.IsLoop;
        clip.m_path = soundPath;
        clip.m_name = soundsTab.Name;
        clip.m_uID = soundsTab.Id;
        clip.m_curMaxPlayingCount = soundsTab.CurMaxPlayingCount;

        if (!m_SoundClipMap.ContainsKey(soundsTab.Id))
        {
            m_SoundClipMap.Add(soundsTab.Id, clip);
        }


        if (null != delFun) delFun(clip, soundClipParam);
    }
    /// <summary>
    /// 删除最长时间未使用的条目
    /// </summary>
    private void RemoveLastUnUsedClip()
    {
        float fSmallestTime = 99999999.0f;
        int smallestId = -1;
        foreach (SoundClip clip in m_SoundClipMap.Values)
        {
            if (fSmallestTime > clip.m_LastActiveTime)
            {
                smallestId = clip.m_uID;
                fSmallestTime = clip.m_LastActiveTime;
            }
        }

        //LogModule.DebugLog("RemoveLastUnUsedClip( " + smallestId.ToString() + " )");

        m_SoundClipMap.Remove(smallestId);
    }

    //add by sunyu 2014-07-31
    //force Remove clip from pool, special for bgmusic
    public void ForceRemoveClipByID(int uid)
    {
        if(uid != -1)
        {
            int nNeeddelId = -1;
            foreach (SoundClip clip in m_SoundClipMap.Values) //dictionary 的 foreach去不掉
            {
                if(clip.m_uID == uid)
                {
                    nNeeddelId = clip.m_uID;
                    break;
                }
            }

            m_SoundClipMap.Remove(nNeeddelId);
        }
    }
}

public class SoundManager : MonoBehaviour
{
    //单独的某个类型的音源类
    public class MyAudioSource
    {
        public MyAudioSource()
        {
            m_uID = -1;
            m_AudioSource = null;
        }
        public int m_uID;         //声音表格配的唯一标示符
        public AudioSource m_AudioSource;
    }

    public SoundClipPools m_SoundClipPools;     //声音数据池 
    private MyAudioSource m_BGAudioSource = new MyAudioSource();            //背景音乐源
    private MyAudioSource m_RealSoundAudioSource = new MyAudioSource();     //真人语音，永远只有一个
    private MyAudioSource m_NpcHelloSoundAudioSource = new MyAudioSource(); //Npc靠近语音，永远只有一个（避免多人触发）
    private float       m_CurBGVolume = 0;      //当前背景音乐音量
	private float		m_FadeInDstVolume = 1.0f;	//淡入效果最终音量
    private float       m_FadeOutSrcVolume = 1.0f;  //淡出效果起始音量
    public static int m_SFXChannelsCount = 24;   //最大声道数量
    private MyAudioSource[] m_SFXChannel = new MyAudioSource[m_SFXChannelsCount]; //Sound FX，声音特效。
    private SoundClip m_NextSoundClip = null; 

    public static bool m_EnableBGM = true;      //是否启用背景音乐
    public static bool m_EnableSFX = true;      //是否启用环境音效
    public static bool m_EnableChatVoice = true;//是否启用玩家语音

    private int m_lastMusicID = -1;           //上次播放的场景背景音乐Id，用于中断后重播

    public float m_sfxVolume = 1.0f;                //音效音量系数
    public float m_bgmVolume = 1.0f;                //场景背景音乐音量系数
    public float m_chatvoiceVolume = 1.0f;          //语音聊天音量

	public AudioSource m_UISoundSource = null;
    
    private enum FadeMode //播放模式
    {
        None,
        FadeIn, //淡入
        FadeOut //淡出
    }
    private FadeMode m_fadeMode;
    private float m_fadeOutTime;
    private float m_fadeOutTimer;
    private float m_fadeInTime;
    private float m_fadeInTimer;

    public bool EnableSFX 
	{
		get{ return m_EnableSFX; }
		set{ m_EnableSFX = value; }
	}

    public bool EnableChatVoice
    {
        get { return m_EnableChatVoice; }
        set { m_EnableChatVoice = value; }
    }
    	
	public bool EnableBGM
	{
		get{ return m_EnableBGM; }
		set
		{
			if( m_EnableBGM && !value )
			{
				if(m_BGAudioSource!=null)
				{
					if(m_BGAudioSource.m_AudioSource.isPlaying)
					{
						m_BGAudioSource.m_AudioSource.Stop();
					}
				}
                m_NextSoundClip = null;

                if (GameManager.CurScene != null)
                    GameManager.CurScene.SetSceneSoundBG(false);
            }
			else if(!m_EnableBGM && value)
			{
				if(m_BGAudioSource!=null)
				{
                    PlayBGMWithFade(m_lastMusicID, 0.1f, 0);
				}

                if (GameManager.CurScene != null)
                    GameManager.CurScene.SetSceneSoundBG(true);
            }
            m_EnableBGM = value;
		}
	}

    private bool m_BGMPause = false;
    private bool m_BGMGoesOn = false;
    private float m_BGMPauseTimer = 0;
    private float m_BGMPauseTime = 0.5f;
    private float m_BGMPauseVolume = 1;//GlobeVar.INVALID_ID;

    //private float m_TVVolume;

    private bool m_NeedRecoverBGM = false;
    private bool m_NeedRecoverSFX = false;

    ////////////////////////////////////方法实现//////////////////////////////////////////
	
	void Awake()
	{
        DontDestroyOnLoad(this.gameObject);
        GameObject soundPool = new GameObject("soundPool");
        soundPool.transform.SetParent(gameObject.transform);


        m_sfxVolume = PlayerPreferenceData.SFXVol;
        m_bgmVolume = PlayerPreferenceData.BGMVol;
        m_chatvoiceVolume = PlayerPreferenceData.ChatVoiceVol;

        m_BGAudioSource.m_AudioSource = soundPool.AddComponent<AudioSource>() as AudioSource;
        m_BGAudioSource.m_uID = -1;

        for (int i = 0; i < m_SFXChannelsCount; ++i)
        {
            m_SFXChannel[i] = new MyAudioSource();
            m_SFXChannel[i].m_uID = -1;
            m_SFXChannel[i].m_AudioSource = soundPool.AddComponent<AudioSource>() as AudioSource;
        }
        
        m_RealSoundAudioSource.m_AudioSource = soundPool.AddComponent<AudioSource>() as AudioSource;
        m_RealSoundAudioSource.m_uID = -1;

        m_UISoundSource = soundPool.AddComponent<AudioSource>() as AudioSource;

        m_NpcHelloSoundAudioSource.m_AudioSource = soundPool.AddComponent<AudioSource>() as AudioSource;
        m_NpcHelloSoundAudioSource.m_uID = GlobeVar.INVALID_ID;

        m_SoundClipPools = new SoundClipPools();
    }
	

	void Start()
	{
        if (PlayerPreferenceData.BGM == 0)
        {
            EnableBGM = false;
        }
        if (PlayerPreferenceData.SFX == 0)
        {
            EnableSFX = false;
        }
    }
	
    void FixedUpdate()
    {
		UpdateSound();
        UpdateBGMPauseGoesOn();
    }

    #region General
    public void OnBGMVolumnChange(float fVal)
    {
        fVal = Mathf.Min(1.0f, Mathf.Max(fVal, 0.0f));
        m_bgmVolume = fVal;

        //更新用到该音量的音源
        if (null != m_BGAudioSource && null != m_BGAudioSource.m_AudioSource)
        {
            float soundFactor = 1f;
            int soundid = m_BGAudioSource.m_uID;
            Tab_Sounds tabSound = TableManager.GetSoundsByID(soundid, 0);
            if (tabSound != null)
            {
                soundFactor = tabSound.Volume;
            }
            m_BGAudioSource.m_AudioSource.volume = m_bgmVolume * soundFactor;
        }
    }

    public void OnSFXVolumnChange(float fVal)
    {
        fVal = Mathf.Min(1.0f, Mathf.Max(fVal, 0.0f));
        m_sfxVolume = fVal;

        //更新用到该音量的音源
        if (null != m_SFXChannel)
        {
            for (int i=0; i < m_SFXChannel.Length; ++i)
            {
                if (null != m_SFXChannel[i] && null != m_SFXChannel[i].m_AudioSource)
                {
                    m_SFXChannel[i].m_AudioSource.volume = m_sfxVolume;
                }
            }
        }
        if (null != m_RealSoundAudioSource && null != m_RealSoundAudioSource.m_AudioSource)
        {
            m_RealSoundAudioSource.m_AudioSource.volume = m_sfxVolume;
        }

        if (null != m_NpcHelloSoundAudioSource && null != m_NpcHelloSoundAudioSource.m_AudioSource)
        {
            m_NpcHelloSoundAudioSource.m_AudioSource.volume = m_sfxVolume;
        }
    }

    public void OnChatVoiceVolumnChange(float fVal)
    {
        fVal = Mathf.Min(1.0f, Mathf.Max(fVal, 0.0f));
        m_chatvoiceVolume = fVal;
    }

    public void OnVolumnChange(float fBGMVol, float fSFXVol, float fChatVol)
	{
        OnBGMVolumnChange(fBGMVol);
        OnSFXVolumnChange(fSFXVol);
        OnChatVoiceVolumnChange(fChatVol);
	}

    void UpdateSound()
    {
		//更新背景音乐
        if (m_fadeMode == FadeMode.FadeOut)
        {
            //保护代码
            if (Math.Abs(m_fadeOutTime) < 0.001f)
            {
                return;
            }

			m_fadeOutTimer += Time.fixedDeltaTime;
			m_CurBGVolume -= (Time.fixedDeltaTime / m_fadeOutTime) * m_FadeOutSrcVolume;
			m_BGAudioSource.m_AudioSource.volume = m_CurBGVolume;
            if (m_BGAudioSource.m_AudioSource.volume < 0.0f)
            {
                m_BGAudioSource.m_AudioSource.volume = 0.0f;
            }

            if (m_fadeOutTimer >= m_fadeOutTime)
            {
                //add by sunyu 2014-07-31
                //force Remove clip from pool, special for bgmusic
				int deluid = m_BGAudioSource.m_uID;
				m_SoundClipPools.ForceRemoveClipByID(deluid);
                m_FadeOutSrcVolume = 1f;

                if (m_NextSoundClip != null)
                {
                    float fNextBGMVol = m_bgmVolume * m_NextSoundClip.m_volume;
                    //加载新的bgm
                    SetAudioSource(ref m_BGAudioSource, m_NextSoundClip, fNextBGMVol);
                    if (m_fadeInTime > 0)
                    {
                        m_fadeMode = FadeMode.FadeIn;
                        m_fadeOutTimer = 0;
                        m_BGAudioSource.m_AudioSource.volume = 0;
                        m_FadeInDstVolume = fNextBGMVol;
                    }
                    else
                    {
                        m_fadeMode = FadeMode.None;
                        m_BGAudioSource.m_AudioSource.volume = fNextBGMVol;
                    }
                    m_BGAudioSource.m_AudioSource.Play();
                }
                else
                {
                    m_BGAudioSource.m_AudioSource.Stop();
                }
            }
        }
        else if (m_fadeMode == FadeMode.FadeIn)
        {
            //保护代码
            if (Math.Abs(m_fadeInTime) < 0.001f)
            {
                return;
            }

			m_fadeInTimer += Time.fixedDeltaTime;
			m_CurBGVolume += (Time.fixedDeltaTime / m_fadeInTime) * m_FadeInDstVolume;
			m_BGAudioSource.m_AudioSource.volume = m_CurBGVolume;
			if (m_BGAudioSource.m_AudioSource.volume > m_FadeInDstVolume)
            {
				m_BGAudioSource.m_AudioSource.volume = m_FadeInDstVolume;
				m_fadeMode = FadeMode.None;
				m_fadeInTimer = 0;
				m_FadeInDstVolume = 0.0f;
				return;
            }

            if (m_fadeInTimer >= m_fadeInTime)
            {
                m_fadeMode = FadeMode.None;
                m_fadeInTimer = 0;
				m_BGAudioSource.m_AudioSource.volume = m_FadeInDstVolume;
				m_FadeInDstVolume = 0.0f;
            }
        }
/*        else if (m_fadeMode == FadeMode.None)
        {

            if (m_EnableBGM == false)
            {
                if (GameManager.CurScene != null)
                    GameManager.CurScene.SetSceneSoundBG(false);
            }
            else
            {
                if (GameManager.CurScene != null)
                    GameManager.CurScene.SetSceneSoundBG(true);
            }
        }

		//更新音效
		if (m_EnableSFX == false)
		{
			if (GameManager.CurScene != null)
				GameManager.CurScene.SetSceneSoundEffect(false);
		}
		else
		{
			if (GameManager.CurScene != null)
				GameManager.CurScene.SetSceneSoundEffect(true);
		}

		//更新真人语音
		if (m_EnableRS)
		{
			float fRsv = m_rsVolume;
			if (fRsv < 0) fRsv = 0;
			if (fRsv > 1) fRsv = 1;
			m_RealSoundAudioSource.m_AudioSource.volume = fRsv;
		}
		else
		{
			m_RealSoundAudioSource.m_AudioSource.volume = 0;
		}*/
    }

    public void MusicRecover()
    {
        if (m_NeedRecoverBGM)
        {
            m_BGAudioSource.m_AudioSource.Play();
        }
        if (m_NeedRecoverSFX)
        {
            EnableSFX = true;
            if (GameManager.CurScene != null)
            {
                GameManager.CurScene.SetSceneSoundEffect(true);
            }
        }
        m_NeedRecoverBGM = false;
        m_NeedRecoverSFX = false;
    }

    public void MusicDown()
    {
        if (EnableBGM)
        {
            m_BGAudioSource.m_AudioSource.Pause();
            m_NeedRecoverBGM = true;
        }
        if (EnableSFX)
        {
            EnableSFX = false;
            if (GameManager.CurScene != null)
            {
                GameManager.CurScene.SetSceneSoundEffect(false);
            }
            m_NeedRecoverSFX = true;
        }
    }

    //赋值
    private void SetAudioSource(ref MyAudioSource audioSource, SoundClip clip, float volume)
    {
        if (audioSource == null) return;
        if (clip == null)
        {
            LogModule.ErrorLog("Error Clip null, please resolve");
            return;
        }
        audioSource.m_AudioSource.clip = clip.Audioclip;
		audioSource.m_AudioSource.volume = volume;
        audioSource.m_AudioSource.spread = clip.m_spread;
        audioSource.m_AudioSource.priority = clip.m_priority;
        audioSource.m_AudioSource.spatialBlend = clip.m_panLevel;
        audioSource.m_AudioSource.minDistance = clip.m_minDistance;
        audioSource.m_AudioSource.loop = clip.m_isLoop;
        audioSource.m_AudioSource.pitch = 1f;
        audioSource.m_uID = clip.m_uID;
    }
    
    //有衰减的音效播放，目前其他玩家和NPC的技能音效上.
    public static void PlaySoundAtPosForSkill(int nSoundID, Vector3 playingPos)
    {
        if (GameManager.SoundManager == null)
        {
            return;
        }

        GameManager.SoundManager.PlaySoundEffectAtPos(nSoundID, playingPos, ObjManager.MainPlayer != null ? ObjManager.MainPlayer.Position : playingPos);
    }

    //有衰减的音效播放 用于受击、死亡等
    public static void PlaySoundAtPosForOther(int nSoundID, Vector3 playingPos)
    {
        if (GameManager.SoundManager == null)
        {
            return;
        }

        GameManager.SoundManager.PlaySoundEffectAtPos(nSoundID, playingPos, ObjManager.MainPlayer != null ? ObjManager.MainPlayer.Position : playingPos);
    }

    // 删除原本代码里的减弱, 通过配表实现
    //public void OnTVSoundEffectPlay()
    //{
    //    float tvBgVol = PlayerPreferenceData.BGMVol;//m_BGAudioSource.m_AudioSource.volume;

    //    // 不能控制当前背景音是否在暂停或fade, 所以从表中取
    //    var tab = TableManager.GetSoundsByID(m_BGAudioSource.m_uID, 0);
    //    if (tab != null)
    //    {
    //        tvBgVol *= tab.Volume;
    //    }
    //    m_TVVolume = tvBgVol;
    //    m_BGAudioSource.m_AudioSource.volume = m_TVVolume * 0.7f;
    //    Debug.LogErrorFormat("tv bgm vol {0}, id {1} @ OnTVSoundEffectPlay", m_BGAudioSource.m_AudioSource.volume, m_BGAudioSource.m_uID);
    //}

    //public void OnTVSoundEffectRecover()
    //{
    //    m_BGAudioSource.m_AudioSource.volume = m_TVVolume;
    //    Debug.LogErrorFormat("tv bgm vol {0}, id {1} @ OnTVSoundEffectRecover", m_BGAudioSource.m_AudioSource.volume, m_BGAudioSource.m_uID);
    //}

    //根绝某个音乐配置的概率，确认是否播放
    private bool IsSoundPlayByRate(int nSoundID)
	{
        Tab_Sounds _tabSound = TableManager.GetSoundsByID(nSoundID, 0);
		if (null == _tabSound)
		{
			return false;
		}

        if (_tabSound.Rate <= 0)
		{
            return false;
		}
		else if (_tabSound.Rate >= 100)
		{
            return true;
		}
		else
		{
			//随机一个1-100的数字
			int nRand = UnityEngine.Random.Range(1, 101);
			if (nRand <=  _tabSound.Rate)
			{
                return true;
			}
		}

        return false;
	}
    #endregion

    #region BGM
    void UpdateBGMPauseGoesOn()
    {
        if (m_BGMPauseVolume == GlobeVar.INVALID_ID)
        {
            return;
        }

        if (m_BGMPause && !m_BGMGoesOn)
        {
            // 暂停 淡出
            m_BGMPauseTimer += Time.deltaTime;
            m_BGAudioSource.m_AudioSource.volume = (1 - m_BGMPauseTimer / m_BGMPauseTime) * m_BGMPauseVolume;
            if (m_BGMPauseTimer >= m_BGMPauseTime)
            {
                m_BGAudioSource.m_AudioSource.Pause();

                m_BGMPause = false;
                m_BGMGoesOn = false;
                m_BGMPauseTimer = 0;
            }
        }
        else if (!m_BGMPause && m_BGMGoesOn)
        {
            // 继续播放 淡入
            m_BGMPauseTimer += Time.deltaTime;
            m_BGAudioSource.m_AudioSource.volume = (m_BGMPauseTimer / m_BGMPauseTime) * m_BGMPauseVolume;
            if (m_BGMPauseTimer >= m_BGMPauseTime)
            {
                m_BGMPause = false;
                m_BGMGoesOn = false;
                m_BGMPauseTimer = 0;
                m_BGMPauseVolume = GlobeVar.INVALID_ID;
            }
        }
    }

    /// <summary>
    /// 淡入淡出播放背景音乐
    /// </summary>
    /// <param name="clipName"></param>
    /// <param name="fadeInTime">淡入时间</param>
    /// <param name="fadeOutTime">淡出时间</param>
    private void PlayBGMWithFade(int nSoundclipId, float fadeOutTime, float fadeInTime)
    {
        m_SoundClipPools.GetSoundClip(nSoundclipId, OnPlayBGMWithFade, new SoundClipPools.SoundClipParam(nSoundclipId, fadeOutTime, fadeInTime));
    }

    void OnPlayBGMWithFade(SoundClip bgSoundClip, SoundClipPools.SoundClipParam param)
    {
        if (param == null)
        {
            LogModule.WarningLog("::OnPlayBGMWithFade:: param is null !!");
            return;
        }

        if (m_BGAudioSource != null && m_BGAudioSource.m_AudioSource != null && bgSoundClip != null)
        {
            // 更新上次播放记录
            m_lastMusicID = param.m_clipID;
            if (m_BGAudioSource.m_AudioSource.isPlaying) //正常播放上一首背景音乐
            {
                if (m_BGAudioSource.m_uID == m_lastMusicID)
                {
                    return;
                }

                if ((m_NextSoundClip != null && m_NextSoundClip.m_uID == bgSoundClip.m_uID))
                {
                    return;
                }


                m_fadeOutTime = param.m_fadeOutTime;
                m_fadeInTime = param.m_fadeInTime;
                m_fadeOutTimer = 0;
                m_fadeInTimer = 0;
                if (m_fadeOutTime <= 0)
                {
                    //force Remove clip from pool, special for bgmusic
                    int deluid = m_BGAudioSource.m_uID;
					SetAudioSource(ref m_BGAudioSource, bgSoundClip, m_bgmVolume * bgSoundClip.m_volume);
                    m_SoundClipPools.ForceRemoveClipByID(deluid);

					m_CurBGVolume = m_bgmVolume * bgSoundClip.m_volume;
                    if (m_fadeInTime <= 0)
                    {
                        m_BGAudioSource.m_AudioSource.Play();
                        m_fadeMode = FadeMode.None;
                    }
                    else
                    {
                        m_BGAudioSource.m_AudioSource.volume = 0;
                        m_BGAudioSource.m_AudioSource.Play();
                        m_fadeMode = FadeMode.FadeIn;
                    }
                }
                else
                {
                    m_NextSoundClip = bgSoundClip;
                    m_fadeMode = FadeMode.FadeOut;
                    m_FadeOutSrcVolume = 0f;
                    m_FadeOutSrcVolume = m_BGAudioSource.m_AudioSource.volume;
                    m_CurBGVolume = m_FadeOutSrcVolume;
                }
            }
            else //没在播放，直接播放
            {
                m_fadeInTime = param.m_fadeInTime;
                m_fadeInTimer = 0;

                //add by sunyu 2014-07-31
                //force Remove clip from pool, special for bgmusic
                int deluid = m_BGAudioSource.m_uID;
				SetAudioSource(ref m_BGAudioSource, bgSoundClip, m_bgmVolume *bgSoundClip.m_volume);
                m_SoundClipPools.ForceRemoveClipByID(deluid);

				m_CurBGVolume = m_bgmVolume * bgSoundClip.m_volume;
                if (m_fadeInTime <= 0)
                {
                    m_BGAudioSource.m_AudioSource.Play();
                    m_fadeMode = FadeMode.None;
                }
                else
                {
                    m_BGAudioSource.m_AudioSource.volume = 0;
                    m_BGAudioSource.m_AudioSource.Play();
					m_FadeInDstVolume = m_CurBGVolume;
                    m_fadeMode = FadeMode.FadeIn;
                }
            }
        }
    }

    /// <summary>
    /// 淡入淡出播放背景音乐
    /// </summary>
    /// <param name="clipName"></param>
    /// <param name="fadeOutTime">淡出时间</param>
    /// <param name="fadeInTime">淡入时间</param>
    public void PlayBGMusic(int nClipID, float fadeOutTime, float fadeInTime)
    {
		if ( AudioListener.volume == 0 )
			return;

        if (nClipID < 0)
        {
            LogModule.ErrorLog("PlayBGM id < 0");
            return;
        }

        m_lastMusicID = nClipID;

        if (m_EnableBGM)
        {
            PlayBGMWithFade(nClipID, fadeOutTime, fadeInTime);
        }
        else
		{
			 //m_BGAudioSource.clip = null;
		}
    }

    

    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public void StopBGM(float fadeOutTime)
    {
        if (m_EnableBGM)
        {
            m_fadeOutTime = fadeOutTime;
            m_fadeMode = FadeMode.FadeOut;
            m_NextSoundClip = null;
        }
    }

    private IEnumerator StopBGMWithFade()
    {
        if (m_BGAudioSource != null)
        {
            if (m_BGAudioSource.m_AudioSource.isPlaying)
            {
                float time = m_fadeOutTime;
                while (time > 0)
                {
                    m_BGAudioSource.m_AudioSource.volume = (time / m_fadeOutTime) * m_bgmVolume;
                    time -= Time.deltaTime;

                    yield return null;
                }

                m_BGAudioSource.m_AudioSource.Stop();
            }
        }
    }

    public void BGMGoesOn()
    {
        m_BGAudioSource.m_AudioSource.Play();

        m_BGMPause = false;
        m_BGMGoesOn = true;
    }

    public void BGMPause()
    {
        m_BGMPauseVolume = m_BGAudioSource.m_AudioSource.volume;

        m_BGMPause = true;
        m_BGMGoesOn = false;
    }

    public bool IsBGMPlaying()
    {
        if (null != m_BGAudioSource && null != m_BGAudioSource.m_AudioSource)
        {
            return m_BGAudioSource.m_AudioSource.isPlaying;
        }

        return false;
    }
    #endregion

    #region RealSound
    public void PlayRealSound(int nSoundID)
    {
        if (AudioListener.volume == 0)
            return;

        if (nSoundID < 0)
        {
            return;
        }

        //真人语音共用游戏音效开关控制
        if (m_EnableSFX)
        {
			if (false == IsSoundPlayByRate(nSoundID))
			{
				return;
			}

            Tab_Sounds _realSound = TableManager.GetSoundsByID(nSoundID, 0);
			if (null == _realSound)
			{
				return;
			}

            SoundClipPools.SoundClipParam _param = new SoundClipPools.SoundClipParam(nSoundID);
			_param.m_volumeFactor = _realSound.Volume;

			m_SoundClipPools.GetSoundClip(nSoundID, OnPlayRealSound, _param);
        }
    }

	private void OnPlayRealSound(SoundClip rsSoundClip, SoundClipPools.SoundClipParam param)
    {
        if (null == m_RealSoundAudioSource || null == rsSoundClip || null == param)
        {
            return;
        }

        if (IsRealSoundPlaying())
        {
            StopRealSound();
        }

        int deluid = m_RealSoundAudioSource.m_uID;


		SetAudioSource(ref m_RealSoundAudioSource, rsSoundClip, m_sfxVolume * param.m_volumeFactor);
        m_SoundClipPools.ForceRemoveClipByID(deluid);

        if (null != m_RealSoundAudioSource.m_AudioSource)
        {
            m_RealSoundAudioSource.m_AudioSource.Play();
        }
    }

    public void StopRealSound()
    {
        if (null != m_RealSoundAudioSource && null != m_RealSoundAudioSource.m_AudioSource && m_RealSoundAudioSource.m_AudioSource.isPlaying)
        {
            m_RealSoundAudioSource.m_AudioSource.Stop();
        }
    }

    public bool IsRealSoundPlaying()
    {
        if (null != m_RealSoundAudioSource && null != m_RealSoundAudioSource.m_AudioSource)
        {
            return m_RealSoundAudioSource.m_AudioSource.isPlaying;
        }

        return false;
    }

    public int GetCurRealSoundId()
    {
        if (null != m_RealSoundAudioSource && null != m_RealSoundAudioSource.m_AudioSource)
        {
            return m_RealSoundAudioSource.m_uID;
        }

        return GlobeVar.INVALID_ID;
    }

    #endregion

    #region HelloNPCSound
    public void PlayNpcHelloSound(int nSoundID, Vector3 playingPos)
    {
        if (AudioListener.volume == 0)
            return;

        if (nSoundID < 0)
        {
            LogModule.ErrorLog("PlayNpcHelloSound id < 0");
            return;
        }

        //npc语音共用游戏音效开关控制
        if (m_EnableSFX)
        {
            if (false == IsSoundPlayByRate(nSoundID))
            {
                return;
            }
            
            Vector3 listenerPos = (ObjManager.MainPlayer != null ? ObjManager.MainPlayer.Position : playingPos);
            float volume = 1.0f;
            listenerPos.y = 0;
            playingPos.y = 0;

            float distance = Vector3.Distance(listenerPos, playingPos);
            volume = (1.2f - distance / 10.0f) * (1 / 1.2f);
            if (volume < 0.01f)
            {
                volume = 0.01f;
                return;     //声音过低就返回
            }
            else if (volume > 1.0f)
            {
                volume = 1.0f;
            }

            Tab_Sounds _tabSound = TableManager.GetSoundsByID(nSoundID, 0);
            if (null == _tabSound)
            {
                return;
            }

            volume *= _tabSound.Volume;
            SoundClipPools.SoundClipParam _param = new SoundClipPools.SoundClipParam(nSoundID);
            _param.m_volumeFactor = volume;
            m_SoundClipPools.GetSoundClip(nSoundID, OnPlayNpcHelloSound, _param);
        }
    }

    private void OnPlayNpcHelloSound(SoundClip rsSoundClip, SoundClipPools.SoundClipParam param)
    {
        if (null == m_NpcHelloSoundAudioSource || null == rsSoundClip || null == param)
        {
            return;
        }

        if (IsNpcHelloSoundPlaying())
        {
            StopNpcHelloSound();
        }

        int deluid = m_NpcHelloSoundAudioSource.m_uID;

        SetAudioSource(ref m_NpcHelloSoundAudioSource, rsSoundClip, m_sfxVolume * param.m_volumeFactor);
        m_SoundClipPools.ForceRemoveClipByID(deluid);

        if (null != m_NpcHelloSoundAudioSource.m_AudioSource)
        {
            m_NpcHelloSoundAudioSource.m_AudioSource.Play();
        }
    }

    public void StopNpcHelloSound()
    {
        if (null != m_NpcHelloSoundAudioSource && null != m_NpcHelloSoundAudioSource.m_AudioSource && m_NpcHelloSoundAudioSource.m_AudioSource.isPlaying)
        {
            m_NpcHelloSoundAudioSource.m_AudioSource.Stop();
        }
    }

    public bool IsNpcHelloSoundPlaying()
    {
        if (null != m_NpcHelloSoundAudioSource && null != m_NpcHelloSoundAudioSource.m_AudioSource)
        {
            return m_NpcHelloSoundAudioSource.m_AudioSource.isPlaying;
        }

        return false;
    }

    public int GetCurNpcHelloSoundId()
    {
        if (null != m_NpcHelloSoundAudioSource
            && null != m_NpcHelloSoundAudioSource.m_AudioSource
            && m_NpcHelloSoundAudioSource.m_AudioSource.isPlaying)
        {
            return m_NpcHelloSoundAudioSource.m_uID;
        }

        return GlobeVar.INVALID_ID;
    }
    #endregion

    #region SFX
    /// <summary>
    /// 停止音效
    /// </summary>
    /// <param name="name">名称,声音表格第一列</param>
    public void StopSoundEffect(int nSoundID)
    {
        if (m_SFXChannel == null)
        {
            return;
        }

        for (int nIndex = 0; nIndex < m_SFXChannelsCount; nIndex++)
        {
            if (m_SFXChannel[nIndex] == null)
                continue;
            if (m_SFXChannel[nIndex].m_AudioSource == null)
            {
                continue;
            }

            if (m_SFXChannel[nIndex].m_uID == nSoundID)
            {
                m_SFXChannel[nIndex].m_AudioSource.Stop();
            }
        }
    }

    public void StopAllSoundEffect()
    {
        if (m_SFXChannel != null)
        {
            for (int nIndex = 0; nIndex < m_SFXChannelsCount; ++nIndex)
            {
                if (m_SFXChannel[nIndex] != null)
                {
                    m_SFXChannel[nIndex].m_AudioSource.Stop();
                }
            }
        }
    }
    
    //////////////////////////////////播放音效////////////////////////////////////
    /// <summary>
    /// 根据目标的坐标和接收者的坐标listenerPos的距离来确定音量,用于技能音效
    /// </summary>
    /// <param name="nSoundID"></param>
    /// <param name="playSoundPos"></param>
    /// <param name="listenerPos"></param>
    public void PlaySoundEffectAtPos(int nSoundID, Vector3 playSoundPos, Vector3 listenerPos)
    {
        if (nSoundID < 0)
        {
            return;
        }

        float volume = 1.0f;
        listenerPos.y = 0;
        playSoundPos.y = 0;

        float distance = Vector3.Distance(listenerPos, playSoundPos);

        volume = (1.2f - distance / 10.0f)*(1/1.2f);
        if (volume < 0.01f)
        {
            volume = 0.01f;
            return;     //声音过低就返回
        }
        else if (volume > 1.0f)
        {
            volume = 1.0f;
        }
        
        PlaySoundEffect(nSoundID, volume);
    }

    //////////////////////////////////播放音效////////////////////////////////////
    /// <summary>
    /// 根据目标的坐标和接收者的坐标listenerPos的距离来确定音量，用于受击、死亡
    /// </summary>
    /// <param name="nSoundID"></param>
    /// <param name="playSoundPos"></param>
    /// <param name="listenerPos"></param>
    public void PlaySoundEffectAtPos2(int nSoundID, Vector3 playSoundPos, Vector3 listenerPos)
    {
        if (nSoundID < 0)
        {
            return;
        }

        float volume = 1.0f;
        listenerPos.y = 0;
        playSoundPos.y = 0;

        float distance = Vector3.Distance(listenerPos, playSoundPos);
         
        volume = (0.6f - distance / 10.0f)*1.67f;
        if (volume < 0.05f)
        {
            volume = 0.05f;
            return;     //声音过低就返回
        }
        else if (volume > 1.0f)
        {
            volume = 1.0f;
        }

        PlaySoundEffect(nSoundID, volume);
    }


    /// <summary>
    /// 播放音效，默认音量缩放系数可以不填，配表值
    /// </summary>
    /// <param name="name"></param>
    /// <param name="volumeFactor">音量缩放系数</param>
    /// <returns></returns>
    public void PlaySoundEffect(int nSoundID, float volumeFactor = 1.0f, SoundClipPools.SoundClipParam.OnSoundEffectPlay delOnSoundEffectPlay = null)
    {
        if (nSoundID < 0)
            return;

        if (AudioListener.volume == 0)
            return;

        if (!m_EnableSFX)
        {
            return;
        }

		if (false == IsSoundPlayByRate(nSoundID))
		{
			return;
		}

        if (name == null)
        {
            LogModule.ErrorLog("PlaySoundEffect name is null");
            return;
        }

        name = name.Trim();

		Tab_Sounds _tabSound = TableManager.GetSoundsByID(nSoundID, 0);
		if (null == _tabSound)
		{
			return;
		}

		volumeFactor *= _tabSound.Volume;

        m_SoundClipPools.GetSoundClip(nSoundID, OnPlaySoundEffect, new SoundClipPools.SoundClipParam(volumeFactor, delOnSoundEffectPlay));           
    }

    void OnPlaySoundEffect(SoundClip soundClip, SoundClipPools.SoundClipParam param)
    {
        if (soundClip == null)
        {
            //LogModule.ErrorLog("soundClip is null");
            return;
        }
        if (param == null)
        {
            return;
        }

		soundClip.m_volume = param.m_volumeFactor;

        PlaySoundEffect(soundClip);

        if (param.m_delOnSoundEffectPlay != null)
        {
            param.m_delOnSoundEffectPlay(soundClip.m_uID);
        }
    }

    /// <summary>
    /// 播放soundClip中的音效
    /// </summary>
    /// <param name="soundClip"></param>
    /// <param name="volume"></param>
    /// <returns></returns>
    private void PlaySoundEffect(SoundClip soundClip)
    {
        if (soundClip == null)
            return;
        if (m_EnableSFX && !string.IsNullOrEmpty(soundClip.m_path))
        {
            if (soundClip.Audioclip == null)
            {
                return;
            }

            int leastImportantIndex = 0;
            int nCurMaxPlayingCount = 0; //最大播放次数
            int nFirstEmptyIndex = -1; //第一个空位
            int nFirstSameClipValidIndex = -1; //第一个已经停止的上次播放过的位置

            for (int nIndex = 0; nIndex < m_SFXChannelsCount; ++nIndex)   //先找已经在播放或者播放过的
            {
                if (m_SFXChannel[nIndex] == null) return; //error
                if (leastImportantIndex < 0 || 
                    leastImportantIndex >= m_SFXChannelsCount ||
                    m_SFXChannel[leastImportantIndex] == null) return;

                if (nFirstEmptyIndex == -1 && !m_SFXChannel[nIndex].m_AudioSource.isPlaying)  //记录第一个可用的空位置
                {
                    nFirstEmptyIndex = nIndex;
                }

                if (m_SFXChannel[nIndex].m_AudioSource.clip == null) continue;

                if (m_SFXChannel[nIndex].m_uID == soundClip.m_uID) //有播放过的内容
                {
                    if (nFirstSameClipValidIndex == -1 && !m_SFXChannel[nIndex].m_AudioSource.isPlaying)  //记录第一个已经停止的上次播放过的位置
                    {
                        nFirstSameClipValidIndex = nIndex;
                    }

                    if (m_SFXChannel[nIndex].m_AudioSource.isPlaying) ++nCurMaxPlayingCount;  //正在播放的计数
                    if (nCurMaxPlayingCount >= soundClip.m_curMaxPlayingCount) //已经超过了，不播放了
                    {
                        break;
                    }
                }

                if (m_SFXChannel[leastImportantIndex].m_AudioSource.priority < m_SFXChannel[nIndex].m_AudioSource.priority)  //记录最低优先级
                {
                    leastImportantIndex = nIndex;
                }
            }

            if (nCurMaxPlayingCount < soundClip.m_curMaxPlayingCount)    //没到播放数量限制，直接播放
            {
                int nValidIndex = -1;        //先选择已经停止播放的，如果没有，选第一个空的，如果也没有空的，替换优先数字最高的
                if (nFirstSameClipValidIndex != -1)
                {
                    nValidIndex = nFirstSameClipValidIndex;
                }
                else
                {
                    if (nFirstEmptyIndex != -1)
                    {
                        nValidIndex = nFirstEmptyIndex;
                    }
                    else
                    {
                        nValidIndex = leastImportantIndex;
                    }
                }

                if (nValidIndex >= 0 && nValidIndex < m_SFXChannelsCount)
                {
                    if ( m_SFXChannel[nValidIndex] == null ) return; // error

                    m_SFXChannel[nValidIndex].m_AudioSource.Stop();
					SetAudioSource(ref m_SFXChannel[nValidIndex], soundClip, m_sfxVolume * soundClip.m_volume);
                    m_SFXChannel[nValidIndex].m_AudioSource.PlayDelayed(soundClip.m_delay);

                    return;// m_SFXChannel[nValidIndex]; 
                }
            }
            else
            {
                //达到播放上限，不播放
                //LogModule.DebugLog("Warning PlaySoundEffect " + soundClip.m_name + " PlayingCount = " + nCurMaxPlayingCount);
            }

        }
    }

    public bool IsSoundEffectPlaying(int nSoundId)
    {
        for (int i = 0; i < m_SFXChannel.Length; i++)
        {
            if (m_SFXChannel[i] == null || m_SFXChannel[i].m_AudioSource == null)
            {
                continue;
            }

            if (m_SFXChannel[i].m_uID == nSoundId && m_SFXChannel[i].m_AudioSource.isPlaying)
            {
                return true;
            }
        }

        return false;
    }
    #endregion

    #region ChatVoice
    public void EnterChatVoice()
    {
        AudioListener.volume = 0.05f;
    }

    public void LeaveChatVoice()
    {
        AudioListener.volume = 1.0f;
    }
    #endregion

    public void PlayRandomSound(int randomSoundID,SOUND_TYPE type)
    {
        if (randomSoundID <= -1)
        {
            return;
        }
        

        List<Tab_RandomSounds> tabs = TableManager.GetRandomSoundsByID(randomSoundID);
        if (tabs == null)
        {
            return;
        }

        int count = 0;
        foreach (var tab in tabs)
        {
            count += tab.Chance;
        }

        int lastID = -1;
        int chance = 0;
        int random = Random.Range(0, count);
        foreach (var tab in tabs)
        {
            chance += tab.Chance;
            lastID = tab.SoundID;
            if (random <= chance)
            {
                break;
            }
        }

        if (lastID == -1)
        {
            return;
        }

        if (type == SOUND_TYPE.SFX)
        {
            PlaySoundEffect(lastID);
        }
        else if (type == SOUND_TYPE.RS)
        {
            PlaySoundEffect(lastID);
        }
        else
        {
            LogModule.WarningLog("RandomSound Not Support Type:" + type);
        }
    }
}
