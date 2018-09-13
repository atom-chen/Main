using System;
using System.Collections;
using ProtobufPacket;

public class TestBattleManager
{
    public static void EnterBattle(int battleID, bool isStoryBattle = false)
    {
        if (GameManager.m_bOffLineMode)
        {
            if (BeforeLoginSceneLogic.IsStoryBeforeLogin)
            {
                GameManager.offLineModeBattleFinish = BattleBeforeLoginFinish;
                GC_ENTER_BATTLE_Handler.ReceivePacket(new GC_ENTER_BATTLE()
                {
                    battleId = battleID,
                    battleSeed = DateTime.Now.Millisecond,
                    id = 1,
                    battleType = GC_ENTER_BATTLE.BattleType.Client,
                    userObjId = 1,
                   // initData = BeforeLoginSceneLogic.GetBattleData,
                });
                MsdkReportData.RecordData(MsdkReportData.REPORTSTEP.EnterBattle);
            }
            else
            {
                GameManager.offLineModeBattleFinish = BattleFinish;

                GC_ENTER_BATTLE_Handler.ReceivePacket(new GC_ENTER_BATTLE()
                {
                    battleId = battleID,
                    battleSeed = DateTime.Now.Millisecond,
                    id = 1,
                    battleType = GC_ENTER_BATTLE.BattleType.Client,
                    userObjId = 1,
                });
            }
            
        }
        else
        {
            CG_TEST_ASK_ENTER_BATTLE_PAK packet = new CG_TEST_ASK_ENTER_BATTLE_PAK();
            packet.data.battleId = battleID;
            packet.data.isAtServer = false;
            packet.data.isStory = isStoryBattle;
            packet.SendPacket();
        }
    }

    public static void HandleTestBattleFinish(GC_TEST_BATTLE_FINISH packet)
    {
        BattleFinish((BattleSide)packet.winSide);
    }

    private static void BattleFinish(BattleSide winSide)
    {
        BattleController ctrl = BattleController.CurBattleController;
        if (ctrl != null)
        {
            ctrl.PlayFinish(winSide, ctrl.BattleNormalOver(winSide));
        }
    }

    private static void BattleBeforeLoginFinish(BattleSide winSide)
    {
        BattleController ctrl = BattleController.CurBattleController;
        if (ctrl != null)
        {
            ctrl.PlayFinish(winSide, ctrl.TutorialBattleOver(winSide));
        }

    }
}