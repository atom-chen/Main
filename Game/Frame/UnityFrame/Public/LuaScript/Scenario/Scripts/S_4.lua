--声明参与的演员
local Actors = {
    hero = -9999,

    --怪物，我方指定站位角色，使用MonsterID
    monster_1 = 9,

    --卡牌，指定卡牌ID
    myCard_1 = {CardId = 9},
    myCard_2 = {CardId = 11},
}
-----------------------------------------------------

--剧情开始，第一回合泡泡喊话
local Sections = {
    {
        Name = "AAA",
        Trigger = function(S)
            return S:IsRoundBegin(1,1)
        end,

        Events = {
            {
                EventType = "PlayCutscene",
                BC_Id = 6,
            },
            {
                EventType = "Idle",
                Time = 1,
            },
            {
                EventType = "PlayStoryContent",
                SC_Id = 100
            },
        },
    },
  {
        Name = "BBB",
        Trigger = function(S)
            return S:IsRoundBegin(1,2)
        end,

        Events = {
           {
                EventType = "Spawn",
                Side = 1,               --我方
                BattlePos = 0,          --0号位
                MonsterId = 9,          --id为9的怪
                IsPlayer = true,       --玩家能否操作
            },
            {
                EventType = "Idle",
                Time = 1,
            },
            {
                EventType = "RoleAction",
                Actor = Actors.monster_1,
                AnimId = -1,
                EffectId = -1,
                PaoPao = 109,
                PaoPaoTime = 2,
            },
        },
    },
}

return Sections