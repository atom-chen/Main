--声明参与的演员
local Actors = {
    hero = 3,

    --怪物，我方指定站位角色，使用MonsterID
    monster_1 = 10215,

    --卡牌，指定卡牌ID
    myCard_1 = {CardId = 19},
    myCard_2 = {CardId = 11},
}
-----------------------------------------------------

--第二章第一场战斗玄武
--剧情开始
local Sections = {
    {
        Name = "第四回合玄武登场",
        Trigger = function(S)
            return S:IsRoundEnd(1,4)
        end,

        Events = {
           {
                EventType = "Spawn",
                Side = 1,               --我方
                BattlePos = 4,          --0号位
                MonsterId = 10215,          --id为4的怪
                IsPlayer = true,       --玩家能否操作
                IsPlaySpawnAnim = true,
            },
            {
                EventType = "Idle",
                Time = 2,
            },
            {
                EventType = "PlayStoryContent",
                SC_Id = 9023,
            },
            {
                EventType = "Idle",
                Time = 1,
            },
            {
                EventType = "SendImpact",
                ImpactId = require("ScriptSkill/ScriptImpact/JQ_15"),
                Target = Actors.monster_1,
            },
        },
    },
--   {
--         Name = "第七回合结束战斗",
--         Trigger = function(S)
--             return S:IsRoundEnd(1,7)
--         end,

--         Events = {
--             {
--                 EventType = "Idle",
--                 Time = 2,
--             },
--             {
--                 EventType = "PlayStoryContent",
--                 SC_Id = 9024,
--             },
--             {
--                 EventType = "ForceFinish",
--                 WinSide = 1,
--             },
--         },
--     },

}

return Sections