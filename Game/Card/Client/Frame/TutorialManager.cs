/********************************************************************************
 *	文件名：	PlayerData.cs
 *	全路径：	\Script\GlobalSystem\Manager\TutorialManager.cs
 *	创建人：	刘子鹏
 *	创建时间：2017-08-28
 *
 *	功能说明：新手指引相关逻辑
 *	修改记录：
*********************************************************************************/
using UnityEngine;
using System.Collections;
using Games.GlobeDefine;
using Games.Table;
using System.Collections.Generic;
using Games;
using Games.LogicObj;
using ProtobufPacket;

public class TutorialManager {

    // 已完成的新手引导数据
    private static List<int> m_CompleteData = new List<int>();
    private static List<int> m_FinishGroup = new List<int>();
    private static bool m_IsOpenTutorial = true;
    public static bool IsOpenTutorial
    {
        get
        {
#if UNITY_EDITOR
            m_IsOpenTutorial = PlayerPreferenceData.ReadIsOpenTutorial();
#endif
            return m_IsOpenTutorial;
        }
        set
        {
            m_IsOpenTutorial = value;
#if UNITY_EDITOR
            PlayerPreferenceData.WriteIsOpenTutorial(m_IsOpenTutorial);
#endif
        }
    }

    public static bool TryRepeatTutorial(TutorialGroup group, int step)
    {
        if (false == GlobeVar._GameConfig.m_IsTutorialRepeatOpen)
        {
            return false;
        }

        if (false == IsTutorialComplete(group, step))
        {
            return true;
        }

        if (IsGroupCanRepeat(group) && false == m_FinishGroup.Contains((int)group))
        {
            return true;
        }

        return false;
    }

    public static bool IsGroupCanRepeat(TutorialGroup group)
    {
        //包含所有任务触发的引导 
        return group == TutorialGroup.Augur || group == TutorialGroup.DrawCard_1 || group == TutorialGroup.QuartzEquip ||
               group == TutorialGroup.StoryCopyScene || group == TutorialGroup.AwakenCopyScene ||
               group == TutorialGroup.StarCopyScene || group == TutorialGroup.EnterStory ||
               group == TutorialGroup.HeroSwitch || group == TutorialGroup.DrawCard_2 ||
               group == TutorialGroup.CardAwaken_1 || group == TutorialGroup.CardAwaken_2 ||
               group == TutorialGroup.CardAwaken_3 || group == TutorialGroup.TutorialBattle1 ||
               group == TutorialGroup.TutorialBattle2 || group == TutorialGroup.TutorialBattle3 ||group == TutorialGroup.TutorialBattle4 ||
               group == TutorialGroup.TutorialBattle5 || group == TutorialGroup.CardLevelUp ||
               group == TutorialGroup.CardIntimacy || group == TutorialGroup.HeLuoRift ||
               group == TutorialGroup.Arena || group == TutorialGroup.AsyncPVP || group == TutorialGroup.TutorialBattle6||
               group == TutorialGroup.DrawCard_3 || group == TutorialGroup.QuartzSuit || group == TutorialGroup.Guild ||
               group == TutorialGroup.House;
    }

    public static bool IsGroupFinish(TutorialGroup group)
    {
        if (false == GlobeVar._GameConfig.m_IsTutorialRepeatOpen)
        {
            return true;
        }

        return m_FinishGroup.Contains((int)group);
    }

    public static void HandleSyncData(GC_SYNC_TUTORIAL_DATA packet)
    {
        if (packet == null)
        {
            return;
        }

        m_CompleteData.Clear();
        m_CompleteData.AddRange(packet.CompleteData);

        m_FinishGroup.Clear();
        m_FinishGroup.AddRange(packet.FinishGroup);
    }

    public static void TutorialComplete(TutorialGroup group, int nStep)
    {
        foreach (var pair in TableManager.GetTutorialGroup())
        {
            if (pair.Value == null || pair.Value.Count < 1)
            {
                continue;
            }

            Tab_TutorialGroup tTutorialGroup = pair.Value[0];
            if (tTutorialGroup == null)
            {
                continue;
            }

            if (tTutorialGroup.GroupId == (int) group && tTutorialGroup.GroupStep == nStep)
            {
                TutorialComplete(tTutorialGroup.Id);
                break;
            }
        }
    }

    public static void TutorialComplete(int nTutorialId)
    {
        if (m_CompleteData.Contains(nTutorialId))
        {
            return;
        }

        CG_TUTORIAL_COMPLETE_PAK pak = new CG_TUTORIAL_COMPLETE_PAK();
        pak.data.ID = nTutorialId;
        pak.SendPacket();

        if (false == m_CompleteData.Contains(nTutorialId))
        {
            m_CompleteData.Add(nTutorialId);
        }
    }

    public static void TutorialCompleteGroup(TutorialGroup group)
    {
        foreach (var pair in TableManager.GetTutorialGroup())
        {
            if (pair.Value == null || pair.Value.Count < 1)
            {
                continue;
            }

            Tab_TutorialGroup tTutorialGroup = pair.Value[0];
            if (tTutorialGroup == null)
            {
                continue;
            }

            if (tTutorialGroup.GroupId == (int)group)
            {
                TutorialComplete(tTutorialGroup.Id);
            }
        }
    }

    public static bool IsTutorialComplete(TutorialGroup group, int nStep)
    {
        foreach (var pair in TableManager.GetTutorialGroup())
        {
            if (pair.Value == null || pair.Value.Count < 1)
            {
                continue;
            }

            Tab_TutorialGroup tTutorialGroup = pair.Value[0];
            if (tTutorialGroup == null)
            {
                continue;
            }

            if (tTutorialGroup.GroupId == (int) group && tTutorialGroup.GroupStep == nStep)
            {
                return m_CompleteData.Contains(tTutorialGroup.Id);
            }
        }

        return false;
    }

    public static void HandleFinishGroup(int group)
    {
        m_FinishGroup.Add(group);
    }

    public static bool IsFunctionUnlock(int nFunctionId, int nCount = 0)
    {
        if (null == GameManager.PlayerDataPool)
        {
            return false;
        }

        Tab_FunctionUnlock tFunction = TableManager.GetFunctionUnlockByID(nFunctionId, 0);
        if (tFunction == null)
        {
            return false;
        }

        if (tFunction.ReqLevel != GlobeVar.INVALID_ID)
        {
            if (GameManager.PlayerDataPool.PlayerHeroData.Level < tFunction.ReqLevel)
            {
                return false;
            }
        }
        if (tFunction.ReqFunction != GlobeVar.INVALID_ID)
        {
            if (nCount >= 5)
            {
                return false;
            }

            if (false == IsFunctionUnlock(tFunction.ReqFunction, nCount + 1))
            {
                return false;
            }
        }
        if (tFunction.ReqStory != GlobeVar.INVALID_ID)
        {
            if (false == GameManager.PlayerDataPool.IsStoryFin(tFunction.ReqStory))
            {
                return false;
            }
        }

        if (tFunction.ReqTutorialID != GlobeVar.INVALID_ID)
        {
            if (false == GameManager.PlayerDataPool.IsTutorialQuestFinish(tFunction.ReqTutorialID, TutorialQuestType.Main))
            {
                return false;
            }
        }

        if (tFunction.Id == (int)FunctionUnlockId.FirstChargeReward)
        {            
            if (!GlobeVar._GameConfig.m_IsFirstChargeOpen)
            {
                return false;
            }
        }

        if (tFunction.Id == (int)FunctionUnlockId.SevenDayActivity)
        {
            if (!GlobeVar._GameConfig.m_IsSevenDayActivityOpen)
            {
                return false;
            }

            if (null != GameManager.PlayerDataPool && null != GameManager.PlayerDataPool.SevenDayActivity)
            {
                //活动类型除了要满足表中开启条件还要满足当前活动状态
                if (GameManager.PlayerDataPool.SevenDayActivity.CurrActivityStatus() != SevenDayActivityStatus.Award &&
                    GameManager.PlayerDataPool.SevenDayActivity.CurrActivityStatus() != SevenDayActivityStatus.Going)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    //是否处于预开启状态
    public static bool IsFunctionPreUnlock(int nFunctionId)
    {
        Tab_FunctionUnlock tFunction = TableManager.GetFunctionUnlockByID(nFunctionId, 0);
        if (tFunction == null)
        {
            return false;
        }
        if(tFunction.IsPreUnLock !=1)
        {
            return false;
        }
        if (IsFunctionUnlock(nFunctionId))
        {
            //已经开启了
            return false;
        }
        if (tFunction.PreReqLevel != GlobeVar.INVALID_ID)
        {
            if (GameManager.PlayerDataPool.PlayerHeroData.Level < tFunction.PreReqLevel)
            {
                return false;
            }
        }
        if (tFunction.PreReqFunction != GlobeVar.INVALID_ID)
        {
            if (false == IsFunctionUnlock(tFunction.PreReqFunction))
            {
                return false;
            }
        }
        if (tFunction.PreReqStory != GlobeVar.INVALID_ID)
        {
            if (false == GameManager.PlayerDataPool.IsStoryFin(tFunction.PreReqStory))
            {
                return false;
            }
        }
        return true;
    }
    public static void SendFunctionLockNotice(int nFunctionId)
    {
        Tab_FunctionUnlock tFunction = TableManager.GetFunctionUnlockByID(nFunctionId, 0);
        if (tFunction == null)
        {
            return;
        }

        // 提示只显示一条
        if (tFunction.ReqLevel != GlobeVar.INVALID_ID && GameManager.PlayerDataPool.PlayerHeroData.Level < tFunction.ReqLevel)
        {
            string szNotice = StrDictionary.GetDicByID(5754, tFunction.ReqLevel);
            Utils.CenterNotice(szNotice);
        }
        else if (tFunction.ReqStory != GlobeVar.INVALID_ID && GameManager.PlayerDataPool.GetStoryFin(tFunction.ReqStory) == 0)
        {
            Tab_StoryLine tStoryLine = TableManager.GetStoryLineByID(tFunction.ReqStory, 0);
            if (tStoryLine != null)
            {
                string szNotice = StrDictionary.GetDicByID(5753, tStoryLine.LineNum + tStoryLine.LineName);
                Utils.CenterNotice(szNotice);
            }
        }
        else if (tFunction.ReqFunction != GlobeVar.INVALID_ID && false == IsFunctionUnlock(tFunction.ReqFunction))
        {
            Utils.CenterNotice(5747);
        }
        else if (tFunction.ReqTutorialID != GlobeVar.INVALID_ID && GameManager.PlayerDataPool.IsTutorialQuestFinish(tFunction.ReqTutorialID, TutorialQuestType.Main) == false)
        {
            Utils.CenterNotice(5747);
        }
    }

    public static int GetFunctionEntryType(int nFunctionId)
    {
        Tab_FunctionUnlock tFunction = TableManager.GetFunctionUnlockByID(nFunctionId, 0);
        if (tFunction == null)
        {
            return GlobeVar.INVALID_ID;
        }

        return tFunction.EntryType;
    }

    public static bool IsFunctionEntryMainUI(int nFunctionId)
    {
        return GetFunctionEntryType(nFunctionId) == (int)FunctionEntryType.MainUI;
    }

    public static bool IsFunctionEntryBattleUI(int nFunctionId)
    {
        return GetFunctionEntryType(nFunctionId) == (int)FunctionEntryType.BattleUI;
    }

    public static bool IsFunctionEntrySceneNpc(int nFunctionId)
    {
        return GetFunctionEntryType(nFunctionId) == (int)FunctionEntryType.SceneNpc;
    }

    public static bool IsFunctionBtnMainUIShow(int nFunctionId)
    {
        return IsFunctionEntryMainUI(nFunctionId) && IsFunctionUnlock(nFunctionId);
    }

    public static bool IsFunctionBtnBattleUIShow(int nFunctionId)
    {
        return IsFunctionEntryBattleUI(nFunctionId) && IsFunctionUnlock(nFunctionId);
    }

    public static bool IsCanStartTutorialAutoBattle()
    {
        if (IsTutorialComplete(TutorialGroup.AutoBattle, 1))
        {
            return false;
        }

        if (false == IsFunctionUnlock((int)FunctionUnlockId.BattleAuto))
        {
            return false;
        }

        if (false == IsFunctionUnlock((int)FunctionUnlockId.BattleGroupLock))
        {
            return false;
        }

        if (false == IsFunctionUnlock((int)FunctionUnlockId.BattleSpeed))
        {
            return false;
        }

        return true;
    }
}
