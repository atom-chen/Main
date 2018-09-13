/********************************************************************************
 *	文件名：	CutsceneManager.cs
 *	全路径：	\Script\GlobalSystem\Manager\CutsceneManager.cs
 *	创建人：	李嘉
 *	创建时间：2017-01-10
 *
 *	功能说明：剧情TV管理器,负责播放TV和结束回调
 *	修改记录：
*********************************************************************************/
using UnityEngine;
using Games.Table;
using Games.GlobeDefine;
using Games.LogicObj;
using System.Collections.Generic;
using Games;

public class CutsceneManager : MonoBehaviour
{
    public enum DialogState
    {
        Show,
        Wait,
    }

    private int m_CurCutsceneId = GlobeVar.INVALID_ID;
    public int CurCutsceneId
    {
        get { return m_CurCutsceneId; }
    }

    private int m_DialogId = GlobeVar.INVALID_ID;
    private int m_CurDialogIndex = GlobeVar.INVALID_ID;
    private float m_DialogTimer = GlobeVar.INVALID_ID;
    private int m_CurDialogVoice = GlobeVar.INVALID_ID;
    private bool m_bSpeedDialog = false;
    private bool m_bLoadingSoundEffect = false;
    private bool m_isFadeOuting = false;

    public bool LoadingSoundEffect
    {
        get { return m_bLoadingSoundEffect; }
    }
    private bool m_bWaitActiveScene = true;
    public bool WaitActiveScene
    {
        get { return m_bWaitActiveScene; }
    }

    private bool m_CutsencePlay = false;
    private float m_CutsceneStart = 0;
    private float m_CutsceneDuration = 0;
    private DialogState m_DialogState = DialogState.Wait;

    private List<float> m_SpeedTimePoint = new List<float>();
    private float m_SpeedTimeDuration = 0;
    private GameObject m_CurCutSceneObject = null;

    //public GameObject CurCutSceneObject
    //{
    //    get { return m_CurCutSceneObject; }
    //}

    public delegate void OnPlayOver(object oParam);
    private OnPlayOver m_delOnPlayOver = null;
    private object m_Param = GlobeVar.INVALID_ID;

    ////当前播放涉及的Event
    //private StoryEvent_TV m_Event = null;
    //public StoryEvent_TV Event
    //{
    //    get { return m_Event; }
    //    set { m_Event = value; }
    //}
    void Start()
    {
        if (GameManager.CurScene != null)
        {
            GameManager.CurScene.ResetNameBoardCamera();
        }
    }

    void Update()
    {
        if (false == m_CutsencePlay)
        {
            return;
        }

        if (m_CutsceneStart <= 0)
        {
            return;
        }

        if (m_SpeedTimePoint.Count > 0)
        {
            if (Time.time - m_CutsceneStart >= m_SpeedTimePoint[0])
            {
                m_SpeedTimePoint.RemoveAt(0);
            }
        }

        if (Time.time - m_CutsceneStart + m_SpeedTimeDuration >= m_CutsceneDuration)
        {
            m_CutsencePlay = false;
            m_CutsceneStart = 0;
            OnCutsceneOver();
        }
    }

    public bool IsCurCutsceneHideNameBoard()
    {
        if (m_CurCutsceneId == GlobeVar.INVALID_ID)
        {
            return false;
        }

        Tab_Cutscene tabCutscene = TableManager.GetCutsceneByID(m_CurCutsceneId, 0);
        if (tabCutscene == null)
        {
            return false;
        }

        return tabCutscene.HideNameBoard == 1;
    }

    //已有的m_CutsencePlay是UI加载完，这个不够用，这个标记是从play时已经置上了
    private bool m_IsFinish = true;
    public bool IsFinish { get { return m_IsFinish; } private set { m_IsFinish = value; } }

    public void PlayCutscene(int id, OnPlayOver delOnPlayOver = null, object param = null,bool hasUI = true)
    {
        //wtm，没有主角时，也有播放tv的情况

        //if (ObjManager.MainPlayer == null)
        //{
        //    return;
        //}

        Tab_Cutscene tabCutscene = TableManager.GetCutsceneByID(id, 0);
        if (tabCutscene == null)
        {
            return;
        }

        if (tabCutscene.HideNameBoard == 1)
        {
            if (null != GameManager.CurScene && null != GameManager.CurScene.NameBoardRoot)
            {
                GameManager.CurScene.NameBoardRoot.gameObject.SetActive(false);
            }
            if (null != GameManager.CurScene && null != GameManager.CurScene.BattleNameBoardRoot)
            {
                GameManager.CurScene.BattleNameBoardRoot.gameObject.SetActive(false);
            }
        }

        if (m_CurCutsceneId != GlobeVar.INVALID_ID)
        {
            OnCutsceneOver();
        }

        m_CurCutsceneId = id;

        InitDelegate(delOnPlayOver, param);

        IsFinish = false;
        if (hasUI)
        {
            UIManager.ShowUI(UIInfo.CutsceneDialog, OnCutsceneDialogShow);
        }
        else
        {
            OnCutsceneDialogShow(true,null);
        }
    }
      
    public void OnCutsceneOver(bool callback = true)
    {
       
        if (IsFinish)
        {
            return;
        }

        IsFinish = true;

        Tab_Cutscene tabCutscene = TableManager.GetCutsceneByID(m_CurCutsceneId, 0);
        if (tabCutscene == null)
        {
            return;
        }
        
        //跳过不播放渐变效果了
        CancelInvoke("FadeOutEffectAutoPlay");

        //if (tabCutscene.FadeOutEffect != GlobeVar.INVALID_ID && !m_isFadeOuting)
        //{
        //    //有渐变效果，但是没播出来，立即执行
        //    FadeOutEffectAutoPlay();
        //}

        RecoverState();

        if (callback)
        {
            OnDelegate();
        }

        UIManager.CloseUI(UIInfo.CutsceneDialog);

        //去掉显式调用, 通过注册的回调来通知调用者
        //if (null != m_Event)
        //{
        //    m_Event.Leave();
        //}
    }

    void RecoverState()
    {
        //if (ObjManager.MainPlayer == null)
        //{
        //    return;
        //}

        Tab_Cutscene tabCutscene = TableManager.GetCutsceneByID(m_CurCutsceneId, 0);
        if (tabCutscene == null)
        {
            return;
        }

        if (m_CurCutSceneObject != null)
        {
            AssetManager.DestroyObj(m_CurCutSceneObject);
            m_CurCutSceneObject = null;
        }

        m_CurCutsceneId = GlobeVar.INVALID_ID;
        m_CurCutSceneObject = null;
        m_DialogId = GlobeVar.INVALID_ID;
        m_CurDialogIndex = GlobeVar.INVALID_ID;
        m_DialogTimer = GlobeVar.INVALID_ID;
        m_bSpeedDialog = false;
        m_isFadeOuting = false;

        m_SpeedTimePoint.Clear();
        m_SpeedTimeDuration = 0;


        if (!m_bWaitActiveScene)
        {
            if (GameManager.CurScene != null)
            {
                GameManager.CurScene.OnCutsceneOver();
            }
        }
        else
        {
            CancelInvoke("ActiveSceneCutscenPlay");
            m_bWaitActiveScene = false;
        }
        
        if (tabCutscene.SoundId > GlobeVar.INVALID_ID)
        {
            if (GameManager.CurScene != null)
            {
                GameManager.CurScene.PlaySceneMusic();
            }
        }

        if (tabCutscene.SoundEffectId != GlobeVar.INVALID_ID)
        {
            GameManager.SoundManager.StopSoundEffect(tabCutscene.SoundEffectId);
            //GameManager.SoundManager.OnTVSoundEffectRecover();
        }

        //if (m_CurDialogVoice > 0)
        //{
        //    GameManager.SoundManager.StopRealSound();
         //   m_CurDialogVoice = GlobeVar.INVALID_ID;
        //}
    }

    public void SkipCurCutscene()
    {
        Tab_Cutscene tabCutscene = TableManager.GetCutsceneByID(m_CurCutsceneId, 0);
        if (tabCutscene == null)
        {
            return;
        }

        if (tabCutscene.CanSkip != 1)
        {
            return;
        }

        OnCutsceneOver(true);
    }

    public void SpeedCurCutscene()
    {
        Tab_Cutscene tabCutscene = TableManager.GetCutsceneByID(m_CurCutsceneId, 0);
        if (tabCutscene == null)
        {
            return;
        }

        if (tabCutscene.CanSkip != 1)
        {
            return;
        }

        bool bSkipClip = false;
        if (m_SpeedTimePoint.Count > 0)
        {
            if (m_CurCutSceneObject != null)
            {
                Animation cutsceneAnim = m_CurCutSceneObject.GetComponent<Animation>();
                if (cutsceneAnim != null)
                {
                    if (cutsceneAnim.GetClip(tabCutscene.MainCilpName) != null)
                    {
                        bSkipClip = true;

                        cutsceneAnim[tabCutscene.MainCilpName].normalizedTime = m_SpeedTimePoint[0] / m_CutsceneDuration;
                        m_SpeedTimeDuration = m_SpeedTimePoint[0] - (Time.time - m_CutsceneStart);

                        UpdateDialogSpeed(m_SpeedTimePoint[0]);

                        m_SpeedTimePoint.RemoveAt(0);
                    }
                }
            }
        }

        if (false == bSkipClip)
        {
            OnCutsceneOver();
        }
    }

    public void OnCutsceneDialogShow(bool bSuccess, object param)
    {
        //wtm修改，没有主角时也有播放tv的需求

        //if (ObjManager.MainPlayer == null)
        //{
        //    return;
        //}

        //if (GameManager.CurScene == null)
        //{
        //    return;
        //}

        Tab_Cutscene tabCutscene = TableManager.GetCutsceneByID(m_CurCutsceneId, 0);
        if (tabCutscene == null)
        {
            return;
        }

        Vector3 localpos;
        Quaternion localrot;

        m_CurCutSceneObject = AssetManager.LoadObjWithTransform("Prefab/Cutscene/" + tabCutscene.Name, out localpos, out localrot);
        if (m_CurCutSceneObject == null)
        {
            m_CurCutsceneId = GlobeVar.INVALID_ID;
            ClearDelegate();
            //UIManager.CloseUI(UIInfo.CutsceneDialog);
            return;
        }

        Camera[] cameras = m_CurCutSceneObject.GetComponentsInChildren<Camera>(true);
        Camera mainCam = Camera.main;
        foreach (var cam in cameras)
        {
            Utils.CopyCameraPostEffect(mainCam,cam);
        }

        SkinnedMeshRenderer[] renderers = m_CurCutSceneObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        foreach (var skinnedMeshRenderer in renderers)
        {
            Games.LogicObj.Avatar.ModifyOutline(skinnedMeshRenderer,Display.IsShowOutLine());
        }

        Transform trans = m_CurCutSceneObject.transform;
        if (GameManager.CurScene != null && GameManager.CurScene.TVRoot != null)
        {
            trans.SetParent(GameManager.CurScene.TVRoot);
        }

        trans.localPosition = localpos;
        trans.localRotation = localrot;

        //AddCutSceneCameraCullShort(m_CurCutSceneObject);

        m_CutsencePlay = true;
        m_CutsceneStart = Time.time;
        m_CutsceneDuration = tabCutscene.Duration;
        m_SpeedTimeDuration = 0;

        if (tabCutscene.CanSkip == 1)
        {
            string[] skipinfo = tabCutscene.SkipTimePoint.Split('-');

            m_SpeedTimePoint.Clear();
            for (int i = 0; i < skipinfo.Length; i++)
            {
                float skiptime;
                if (float.TryParse(skipinfo[i], out skiptime))
                {
                    m_SpeedTimePoint.Add(skiptime);
                }
            }
        }

        m_bWaitActiveScene = true;
        Invoke("ActiveSceneCutscenPlay", 0.03f);
        
        m_DialogId = tabCutscene.DialogId;

        m_CurDialogIndex = GlobeVar.INVALID_ID;
        m_DialogTimer = GlobeVar.INVALID_ID;

        CurDialogIndexOver();
        
        Tab_Sounds tsound = TableManager.GetSoundsByID(tabCutscene.SoundId, 0);
        if (tsound != null)
        {
            GameManager.SoundManager.PlayBGMusic(tabCutscene.SoundId, tsound.FadeOutTime, tsound.FadeInTime);
        }

        if (tabCutscene.SoundEffectId != GlobeVar.INVALID_ID)
        {
            //GameManager.SoundManager.OnTVSoundEffectPlay();
            m_bLoadingSoundEffect = true;
            GameManager.SoundManager.PlaySoundEffect(tabCutscene.SoundEffectId, 1, OnSoundEffectPlay);
            Invoke("SoundEffectPlayAutoOver", 0.2f);
        }

        if (CutsceneDialogController.Instance() != null)
        {
            CutsceneDialogController.Instance().InitUI(tabCutscene.DialogId, Time.time, tabCutscene.CanSkip == 1);
        }

        //这里特殊处理，有些剧情可能会保持Loading界面不关，这里将Loading界面关闭
        if (GameManager.m_bStillShowLoading)
        {
            GameManager.HideLoadingWindow();
        }

        if (tabCutscene.FadeOutEffect != GlobeVar.INVALID_ID)
        {
            Invoke("FadeOutEffectAutoPlay", tabCutscene.FadeOutDelay);
        }
    }

    void OnSoundEffectPlay(int nSoundId)
    {
        m_bLoadingSoundEffect = false;
        CancelInvoke("SoundEffectPlayAutoOver");
    }

    // OnSoundEffectPlay函数有可能调不到 用invoke保证能置标记
    void SoundEffectPlayAutoOver()
    {
        m_bLoadingSoundEffect = false;
    }

    void FadeOutEffectAutoPlay()
    {
        CancelInvoke("FadeOutEffectAutoPlay");
        Tab_Cutscene tab = TableManager.GetCutsceneByID(m_CurCutsceneId, 0);
        if (GameManager.CurScene != null && tab != null && tab.FadeOutEffect != GlobeVar.INVALID_ID)
        {
            GameManager.CurScene.PlayEffect(tab.FadeOutEffect, Vector3.zero, false);
        }
        m_isFadeOuting = true;
    }

    void ActiveSceneCutscenPlay()
    {
        m_bWaitActiveScene = false;
        if (GameManager.CurScene != null)
        {
            GameManager.CurScene.OnCutscenePlay();
        }
    }

    void AddCutSceneCameraCullShort(GameObject curSceneObjRoot)
    {
        if (null == curSceneObjRoot)
        {
            return;
        }

        //遍历所有的节点，找到所有摄像机
        Transform trans = curSceneObjRoot.transform;
        if (trans == null)
        {
            return;
        }

        Camera camera = curSceneObjRoot.GetComponent<Camera>();
        if (null != camera)
        {
            Utils.TryAddComponent<CullShortOtherCamera>(curSceneObjRoot);
        }

        int nChildCount = trans.childCount;
        for (int i = 0; i < nChildCount; i++)
        {
            if (null != trans.GetChild(i) && null != trans.GetChild(i).gameObject)
            {
                AddCutSceneCameraCullShort(trans.GetChild(i).gameObject);
            }
        }
    }

    // 显示对白 开始等待清空对白
    void ShowNextDialog()
    {
        Tab_CutsceneDialog tabDialog = TableManager.GetCutsceneDialogByID(m_DialogId, m_CurDialogIndex);
        if (tabDialog == null)
        {
            return;
        }

        //if (CutsceneDialogController.Instance() != null)
        //{
        //    CutsceneDialogController.Instance().ShowDialog(tabDialog.Speaker, tabDialog.Content);
        //}

        if (m_CurDialogVoice > 0 && m_bSpeedDialog)
        {
            //GameManager.SoundManager.StopRealSound();
            m_CurDialogVoice = GlobeVar.INVALID_ID;
            m_bSpeedDialog = false;
        }

		m_CurDialogVoice = tabDialog.VoiceSound;

        m_DialogState = DialogState.Show;

        Invoke("CurDialogIndexOver", tabDialog.Duration);
    }

    // 清空对白 并等待下一句对白
    void CurDialogIndexOver()
    {
        if (m_CurDialogIndex == GlobeVar.INVALID_ID)
        {
            m_DialogTimer = Time.time;
        }

        //if (CutsceneDialogController.Instance() != null)
        //{
        //    CutsceneDialogController.Instance().ClearDialog();
        //}

        m_CurDialogIndex += 1;

        Tab_CutsceneDialog tabDialog = TableManager.GetCutsceneDialogByID(m_DialogId, m_CurDialogIndex);
        if (tabDialog == null)
        {
            return;
        }

        m_DialogState = DialogState.Wait;

        Invoke("ShowNextDialog", tabDialog.StartTime - (m_SpeedTimeDuration + Time.time - m_DialogTimer));
    }

    void UpdateDialogSpeed(float toTime)
    {
        m_bSpeedDialog = true;

        int toDialogIndex = m_CurDialogIndex;

        Tab_CutsceneDialog tToDialog = TableManager.GetCutsceneDialogByID(m_DialogId, toDialogIndex);
        while (tToDialog != null && tToDialog.StartTime < toTime)
        {
            toDialogIndex += 1;
            tToDialog = TableManager.GetCutsceneDialogByID(m_DialogId, toDialogIndex);
        }

        if (tToDialog == null)
        {
            // 后面没有对话了
            CancelInvoke("CurDialogIndexOver");
            CancelInvoke("ShowNextDialog");
        }
        else
        {
            if (m_DialogState == DialogState.Wait)
            {
                m_CurDialogIndex = toDialogIndex;

                CancelInvoke("ShowNextDialog");
                Invoke("ShowNextDialog", tToDialog.StartTime - toTime);
            }
            if (m_DialogState == DialogState.Show)
            {
                toDialogIndex -= 1;
                m_CurDialogIndex = toDialogIndex;

                tToDialog = TableManager.GetCutsceneDialogByID(m_DialogId, m_CurDialogIndex);
                if (tToDialog == null)
                {
                    return;
                }

                CancelInvoke("CurDialogIndexOver");
                Invoke("CurDialogIndexOver", tToDialog.StartTime + tToDialog.Duration - toTime);
            }
        }
    }
        
    public void OnConnectLost()
    {
        OnCutsceneOver(false);
    }

    void InitDelegate(OnPlayOver delOnPlayOver, object param)
    {
        m_delOnPlayOver = delOnPlayOver;
        m_Param = param;
    }

    void OnDelegate()
    {
        if (m_delOnPlayOver != null)
        {
            m_delOnPlayOver(m_Param);
            ClearDelegate();
        }
    }

    void ClearDelegate()
    {
        m_delOnPlayOver = null;
        m_Param = null;
    }

}