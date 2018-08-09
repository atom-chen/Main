--声明参与的演员，-9999为主角
local Actors = {
    hero = 3,

    --怪物，我方指定站位角色，使用MonsterID
    monster_1 = 1,

    --卡牌，指定卡牌ID，通常用于我方角色
    myCard_1 = {CardId = 10},
    myCard_2 = {CardId = 11},
}
-----------------------------------------------------

--剧情开始
local Sections = {
    {
        Name = "WaitRound",
        Trigger = function(S)
            return S:IsRoundEnd(1,1)
        end,
    },
    {
        Name = "AAA",
        Trigger = function(S)
            return S:IsSectionDone("WaitRound")
        end,

        Events = {
            {
                EventType = "Idle",
                Time = 2,
            },
            {
                EventType = "Spawn",
                Side = 2,               --1我方，2敌方
                BattlePos = 4,          --站位（0~6）号位
                MonsterId = 3,          --Monster表ID
                IsPlayer = false,       --玩家能否操作false否true能
            },
            {
                EventType = "PlayCutscene",
                BC_Id = 1,
            },
            {
                EventType = "UseSkill",
                Actor = Actors.hero,
                SkillId = 111,
                Target = Actors.hero,
            },
            {
                EventType = "PlayCutscene",
                BC_Id = 2,
            },
        },
    },
    
    {
        Name = "Over",
        Trigger = function(S)
            return S:IsHPLt(Actors.monster_1,8000)
        end,

        Events = {
            {
                EventType = "PlayCutscene",
                BC_Id = 2,
            },
            {
                EventType = 'DelRole',
                Actor = Actors.monster_1,
            },
            {
                EventType = "Idle",
                Time = 1,
            },
            {
                EventType = "ForceFinish",
                WinSide = 1,
            }
        }
    }

}

return Sections