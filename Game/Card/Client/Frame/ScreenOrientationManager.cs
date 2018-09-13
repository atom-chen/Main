using System.Collections;
using System.Collections.Generic;
using Games.GlobeDefine;
using Games.LogicObj;
using UnityEngine;

public class ScreenOrientationManager : MonoBehaviour
{
    public enum ScreenState
    {
        Horizontal,
        Vertical,
    }

    private float m_ChangeTime = 0;
    private bool m_IsHorizontalGoing = false;
    private bool m_IsVerticalGoing = false;
    private ScreenState m_CurScreenState = ScreenState.Horizontal;
    private Coroutine m_EnterRoleTouchCor = null;
    private Coroutine m_LeaveRoleTouchCor = null;

    private const float ChangeCD = 3.0f;

    private static bool m_IsOpen = false;
    public static void SetIsOpen(bool bOpen)
    {
        m_IsOpen = false;
    }

    public bool IsVerticalGoing()
    {
        return m_IsVerticalGoing;
    }

    void Start()
    {
        //if (Screen.orientation != ScreenOrientation.Landscape)
        //{
        //    Screen.orientation = ScreenOrientation.Landscape;
        //}

        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToPortrait = false;
    }

    void Update()
    {
        if (false == m_IsOpen)
        {
            return;
        }

        if (IsCanVertical())
        {
            ChangeVertical();
        }
        if (IsCanHorizontal())
        {
            ChangeHorizontal();
        }

        if (DebugInfo.Instance() != null && DebugInfo.Instance().PrintGyro)
        {
            //LogModule.DebugLog("--------------acceleration " + Input.acceleration + "--------------");
        }
    }

    bool IsCanVertical()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        if (false == Input.GetKey(KeyCode.W))
        {
            return false;
        }

#elif UNITY_IPHONE || UNITY_ANDROID
            if (Mathf.Abs(Input.acceleration.x) < 0.8f)
            {
                return false;
            }
#else
            return false;
#endif

        if (m_IsHorizontalGoing || m_IsVerticalGoing || m_CurScreenState == ScreenState.Vertical)
        {
            return false;
        }

        if (m_ChangeTime > 0 && Time.time - m_ChangeTime < ChangeCD)
        {
            return false;
        }

        if (GameManager.CurScene == null)
        {
            return false;
        }

        if (GameManager.LoadingScene || GameManager.ReqTeleporting)
        {
            return false;
        }

        if (GameManager.CurScene.LeaveSceneTime > 0 && Time.time - GameManager.CurScene.LeaveSceneTime < ChangeCD)
        {
            return false;
        }

        if (GameManager.CurScene.IsAsyncPVPScene())
        {
            return false;
        }

        if (GameManager.storyManager != null && GameManager.storyManager.StoryMode)
        {
            return false;
        }

        if (UIManager.loadingList != null && UIManager.loadingList.Count > 0)
        {
            return false;
        }

        if (UIManager.IsPopUIShow() || UIManager.IsStoryUIShow() ||
            UIManager.IsMessageUIShowExcept("CenterNoticeEx", "RollNoticeRoot") ||
            UIManager.IsTipUIShow() || UIManager.IsMessageBoxUIShow())
        {
            return false;
        }

        if (FunctionOpenController.Ins != null && FunctionOpenController.Ins.IsPlayingUnlockEffect)
        {
            return false;
        }

        if (MainUI.Ins != null && false == MainUI.Ins.TweenDone)
        {
            return false;
        }

        return true;
    }

    bool IsCanHorizontal()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        if (false == Input.GetKey(KeyCode.S))
        {
            return false;
        }
#elif UNITY_IPHONE || UNITY_ANDROID
            if (Mathf.Abs(Input.acceleration.x) < 0.8f)
            {
                return false;
            }
#else
            return false;
#endif

        if (m_IsHorizontalGoing || m_IsVerticalGoing || m_CurScreenState == ScreenState.Horizontal)
        {
            return false;
        }

        if (m_ChangeTime > 0 && Time.time - m_ChangeTime < ChangeCD)
        {
            return false;
        }

        if (GameManager.CurScene == null)
        {
            return false;
        }

        if (GameManager.CurScene.LeaveSceneTime > 0 && Time.time - GameManager.CurScene.LeaveSceneTime < ChangeCD)
        {
            return false;
        }

        return true;
    }

    void ChangeVertical()
    {
        m_EnterRoleTouchCor = StartCoroutine(EnterRoleTouch());
    }

    void ChangeHorizontal()
    {
        m_LeaveRoleTouchCor = StartCoroutine(LeaveRoleTouch());
    }

    IEnumerator EnterRoleTouch()
    {
        m_IsVerticalGoing = true;
        m_CurScreenState = ScreenState.Vertical;

        if (UIManager.Instance() != null)
        {
            UIManager.Instance().SetVerticalMode(true);
            UIManager.Instance().CloseAllStoryUI();
        }
        if (QuickChatController.Ins!=null)
        {
            QuickChatController.Ins.CloseQuickRoot();
        }

        if (ObjManager.MainPlayer != null && ObjManager.MainPlayer.IsMoving)
        {
            ObjManager.MainPlayer.StopMove();
        }
        yield return null;

        // 隐藏NPC
        foreach (var pair in ObjManager.ObjPools)
        {
            if (pair.Value == null)
            {
                continue;
            }

            Obj_Char objChar = pair.Value as Obj_Char;
            if (objChar == null)
            {
                continue;
            }

            if (ObjManager.MainPlayer != null)
            {
                if (ObjManager.MainPlayer.Talisman == objChar)
                {
                    continue;
                }
            }

            objChar.SetVisible(ObjVisibleLayer.RoleTouch, false);
        }

        if (null != GameManager.CurScene && null != GameManager.CurScene.NameBoardRoot)
        {
            GameManager.CurScene.NameBoardRoot.gameObject.SetActive(false);
        }

        //取消变身
        GameManager.PlayerDataPool.CheckAndStopRoleMask();

        yield return new WaitForSeconds(0.5f);

        Screen.orientation = ScreenOrientation.Portrait;

#if !UNITY_EDITOR
            while (Screen.width > Screen.height)
            {
                yield return null;
            }
#endif

        GameManager.IsHorizontalScreen = false;

        // 播放进入特效
        GameObject uiRoot = GameManager.GetUIRoot();
        if (uiRoot != null && uiRoot.GetComponent<UIRootAdapter>() != null)
        {
            uiRoot.GetComponent<UIRootAdapter>().ChangeVerticalScreen();
        }
        yield return null;

        if (ObjManager.MainPlayer != null)
        {
            ObjManager.MainPlayer.PlayEffect(GlobeVar.ROLETOUCH_EFFECT_ENTER);
            //ObjManager.MainPlayer.CameraFaceTo();
        }

        if (RoleTouchController.Instance == null)
        {
            RoleTouchController.EnterRoleTouch();
        }

        while (RoleTouchController.Instance == null)
        {
            yield return null;
        }

        m_ChangeTime = Time.time;
        m_IsVerticalGoing = false;

        yield return null;
    }

    IEnumerator LeaveRoleTouch()
    {
        m_IsHorizontalGoing = true;
        m_CurScreenState = ScreenState.Horizontal;

        if (RoleTouchController.Instance != null)
        {
            RoleTouchController.Instance.PreLeaveRoleTouch();
        }

        if (UIManager.Instance() != null)
        {
            UIManager.Instance().SetVerticalMode(false);
        }

        Screen.orientation = ScreenOrientation.Landscape;

        while (Screen.width < Screen.height)
        {
            yield return null;
        }

        GameManager.IsHorizontalScreen = true;

        // 隐藏UI
        GameObject uiRoot = GameManager.GetUIRoot();
        if (uiRoot != null && uiRoot.GetComponent<UIRootAdapter>() != null)
        {
            uiRoot.GetComponent<UIRootAdapter>().ChangeHorizontalScreen();
        }
        yield return null;

        foreach (var pair in ObjManager.ObjPools)
        {
            if (pair.Value == null)
            {
                continue;
            }

            Obj_Char objChar = pair.Value as Obj_Char;
            if (objChar == null)
            {
                continue;
            }

            objChar.SetVisible(ObjVisibleLayer.RoleTouch,true);
        }

        if (ObjManager.MainPlayer != null)
        {
            ObjManager.MainPlayer.FaceToWithTween(Camera.main.transform.position, 1.0f);
            ObjManager.MainPlayer.PlayEffect(GlobeVar.ROLETOUCH_EFFECT_ENTER);
        }

        if (null != GameManager.CurScene && null != GameManager.CurScene.NameBoardRoot)
        {
            GameManager.CurScene.NameBoardRoot.gameObject.SetActive(true);
        }

        if (RoleTouchController.Instance != null && false == RoleTouchController.Instance.WaitClose)
        {
            RoleTouchController.Instance.OnLeaveRoleTouch();
        }

        while (RoleTouchController.Instance != null)
        {
            yield return null;
        }

        m_ChangeTime = Time.time;
        m_IsHorizontalGoing = false;

        yield return new WaitForSeconds(3.0f);
        yield return null;
    }

    public void ForceLeaveRoleTouch()
    {
        StopCoroutine(m_EnterRoleTouchCor);
        ChangeHorizontal();
    }
}
