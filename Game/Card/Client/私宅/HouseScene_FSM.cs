using System;
using System.Collections;
using System.Collections.Generic;
using Games;
using Games.LogicObj;
using ProtobufPacket;
using UnityEngine;
using Games.GlobeDefine;
using Games.Table;

public partial class HouseScene
{
    public enum HouseMode
    {
        NONE,
        NORMAL,     // 普通
        EDIT,       // 摆放
        INTIMACY,   // 亲密度
    }

    private HouseMode mMode;
    public bool InMode(HouseMode m)
    {
        return m == mMode;
    }

    public void SwitchMode(HouseMode mode)
    {
        if (InMode(mode))
            return;

        // azuki todo: 一个切换流程还没有完成时开始另一个, 逻辑上是否允许打断?
        StopCoroutine("SwitchModeCo");
        StartCoroutine(SwitchModeCo(mode));
    }

    private IEnumerator SwitchModeCo(HouseMode mode)
    {
        yield return LeaveState(mode);
        mMode = mode;
        yield return EnterState(mMode);
    }

    private IEnumerator LeaveState(HouseMode to)
    {
        Tab_SceneClass tab = TableManager.GetSceneClassByID(GameManager.RunningScene, 0);
        switch (mMode)
        {
            case HouseMode.NONE:
                break;
            case HouseMode.NORMAL:     // 普通
                CleanTarget();
                if (ObjManager.MainPlayer != null)
                {
                    ObjManager.MainPlayer.EventMove = false;
                }
                break;
            case HouseMode.EDIT:       // 摆放
                UIManager.CloseUI(UIInfo.HouseCardShowRoot);
                StoryFadeInOutController.ShowCommon("");
                yield return new WaitForSeconds(0.5f);
                if (tab != null && ObjManager.MainPlayer != null)
                {
                    GameManager.CameraManager.CameraChangeParam(tab.CameraCfgID, 0f, () =>
                    {
                        GameManager.CameraManager.FollowTarget = ObjManager.MainPlayer.ObjTransform;
                    });
                }
                break;
            case HouseMode.INTIMACY:   // 亲密度
                StoryFadeInOutController.ShowCommon("");
                StopCoroutine("IntimacyCardShowBubble");
                yield return new WaitForSeconds(0.5f);

                if (mIntimacyObjCard == null)
                {
                    yield break;
                }
                var nav = mNavManager.GetNavigator(mIntimacyObjCard);
                if (nav != null)
                {
                    nav.Run(mNavManager.Speed);
                }
                //ObjManager.MainPlayer.transform.position = mIntimacyPlayerPos;         //还原玩家位置
                mIntimacyObjCard.transform.position = mIntimacyCardPos;                //还原Card位置
                mIntimacyObjCard.NavAgent.enabled = true;
                if (TargetFrameController.Instance!=null)
                {
                    TargetFrameController.Instance.RefreshTarget();
                }

                if (tab != null && ObjManager.MainPlayer != null)
                {
                    GameManager.CameraManager.CameraChangeParam(tab.CameraCfgID, 0f, () =>
                    {
                        GameManager.CameraManager.FollowTarget = ObjManager.MainPlayer.ObjTransform;
                    });
                }
                mIntimacyObjCard = null;
                mIntimacyCardPos = Vector3.zero;
                mIntimacyPlayerPos = Vector3.zero;
                IsMainPlayerLookAtCamera = true;
                break;
        }

        yield return null;
    }

    private IEnumerator EnterState(HouseMode from)
    {
        Tab_HouseSkin tabHouse = TableManager.GetHouseSkinByID(mYard.SkinId,0);
        switch (mMode)
        {
            case HouseMode.NONE:
                break;
            case HouseMode.NORMAL:     // 普通
                if (ObjManager.MainPlayer != null)
                {
                    ObjManager.MainPlayer.EventMove = true;
                }
                break;
            case HouseMode.EDIT:       // 摆放
                StoryFadeInOutController.ShowCommon("");
                yield return new WaitForSeconds(0.7f);

                GameManager.CameraManager.FollowTarget = mCamStartTrans;
                if(tabHouse!=null)
                {
                    bool camFinished = false;
                    GameManager.CameraManager.CameraChangeParam(tabHouse.EditCamCfg, 2f, () => { camFinished = true; });
                    if (!camFinished)
                        yield return null;
                }

                HouseCardShowController.Open();
                break;
            case HouseMode.INTIMACY:   // 亲密度
                if (ObjManager.MainPlayer == null)
                {
                    yield break;
                }
                mIntimacyObjCard = ObjManager.MainPlayer.m_selectTarget as Obj_Card;  //目标卡牌
                if (mIntimacyObjCard == null)
                {
                    yield break;
                }
                HouseScene.SceneCardInfo sceneCard = GetSceneCardInfo(mIntimacyObjCard);   //sceneCard数据
                if(sceneCard == null)
                {
                    yield break;
                }

                //不允许Card移动
                var nav = mNavManager.GetNavigator(mIntimacyObjCard);
                if (nav != null)
                {
                    nav.Stop();
                }
                mIntimacyObjCard.NavAgent.enabled = false;
                ObjManager.MainPlayer.StopMove();
                IsMainPlayerLookAtCamera = false;
                mIntimacyPlayerPos = mIntimacyObjCard.transform.position;       //缓存玩家之前的位置
                mIntimacyCardPos = mIntimacyObjCard.transform.position;         //缓存Card之前的位置
                StoryFadeInOutController.ShowCommon("");
                yield return new WaitForSeconds(0.5f);

                //摄像机效果
                GameManager.CameraManager.FollowTarget = mIntimacyTarget;
                if (tabHouse != null)
                {
                    bool camFinished = false;
                    GameManager.CameraManager.CameraChangeParam(tabHouse.IntimacyCamCfg, 2f, () => { camFinished = true; });
                    if (!camFinished)
                        yield return null;
                }
                //设置新位置
                mIntimacyObjCard.transform.position = mIntimacyCard.position;
                ObjManager.MainPlayer.transform.position = mIntimacyPlayer.position;
                mIntimacyObjCard.FaceTo(mIntimacyPlayer.position);
                ObjManager.MainPlayer.FaceTo(mIntimacyCard.position);
                StartCoroutine(IntimacyCardShowBubble());
                ObjManager.MainPlayer.m_selectTarget = null;
                HouseIntimacyController.Show(sceneCard.card);
                break;
        }
        yield return null;
    }
}
