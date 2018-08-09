--声明参与的演员
local Actors = 
{
    hero = -9999,

    --怪物，我方指定站位角色，使用MonsterID
    monster_1 = 1,

    --卡牌，指定卡牌ID
    myCard_1 = {CardId = 10},
    myCard_2 = {CardId = 11},
}
-----------------------------------------------------

--剧情开始
local Sections = 
{
    {
        Name = "AAA",
        Trigger = function(S)
            return S:IsRoundBegin(1,1)
        end,

        Events = 
        {
            {
                EventType = "PlayCutscene",
                BC_Id = 3,
            },
            {
                EventType = "Idle",
                Time = 2,
            },
            {
                EventType = "Spawn",
                Side = 1,               --我方
                BattlePos = 1,          --1号位
                MonsterId = 9,          --id为9的怪
                IsPlayer = true,       --玩家能否操作
            },
            {
                EventType = "Idle",
                Time = 1,
            },
             {
                EventType = "PlayCutscene",
                BC_Id = 1,
            },
        },
    },
    {
        Name = "BBB",
        Trigger = function(S)
            return S:IsRoundEnd(1,1)
        end,
    },

    {
        Name = "CCC",
        Trigger = function(S)
            return S:IsSectionDone("BBB") and S:IsHPLt(Actors.monster_1,10000)
        end,

        Events = 
        {
            {
                EventType = "Spawn",
                Side = 1,               --我方
                BattlePos = 2,          --2号位
                MonsterId = 10,          --id为10的怪
                IsPlayer = true,       --玩家能否操作
            },
            {
                EventType = "PlayCutscene",
                BC_Id = 1,
            },
        },
    },


}

return Sections