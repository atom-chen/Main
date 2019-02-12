--声明参与的演员
local Actors = {
    hero = 3,

    --怪物，我方指定站位角色，使用MonsterID
    monster_1 = 1,

    --卡牌，指定卡牌ID
    myCard_1 = {CardId = 10},
    myCard_2 = {CardId = 11},
}
-----------------------------------------------------

--剧情开始
local Sections = {
    {
        Name = "AAA",
        Trigger = function(S)
            return S:IsRoundBegin(1,1)
        end,

        Events = {
            {
                EventType = "Idle",
                Time = 2,
            },
            {
                EventType = "Spawn",
                Side = 2,               --敌方
                BattlePos = 4,          --0号位
                MonsterId = 3,          --id为3的怪
                IsPlayer = false,       --玩家能否操作
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