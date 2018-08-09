--声明参与的演员
local Actors = {
    hero = 3,

    --怪物，我方指定站位角色，使用MonsterID
    monster_1 = 33,

    --卡牌，指定卡牌ID
    myCard_1 = {CardId = 19},
    myCard_2 = {CardId = 11},
}
-----------------------------------------------------

--剧情开始
local Sections = {
    {
        Name = "开战对话",
        Trigger = function(S)
            return S:IsRoundBegin(1,1)
        end,

        Events = {
            {
                EventType = "Idle",
                Time = 2,
            },
            {
                EventType = "PlayStoryContent",
                SC_Id = 186,
            },
        },
     },
  {
        Name = "第四回合毕方登场",
        Trigger = function(S)
            return S:IsRoundEnd(1,3)
        end,

        Events = {
           {
                EventType = "Spawn",
                Side = 1,               --我方
                BattlePos = 1,          --1号位
                MonsterId = 33,          --id为4的怪
                IsPlayer = true,       --玩家能否操作
                IsPlaySpawnAnim = true,
            },
            {
                EventType = "Idle",
                Time = 2,
            },
            {
                EventType = "PlayStoryContent",
                SC_Id = 187,
            },
            {
                EventType = "Idle",
                Time = 1,
            },
            {
                EventType = "SendImpact",
                ImpactId = 1131011,
                Target = Actors.monster_1,
            },
        },
    },
  {
        Name = "第七回合毕方放大招",
        Trigger = function(S)
            return S:IsRoundEnd(1,7)
        end,

        Events = {
            {
                EventType = "Idle",
                Time = 2,
            },
            {
                EventType = "PlayStoryContent",
                SC_Id = 188,
            },
            {
                EventType = "Idle",
                Time = 1,
            },
            {
                EventType = "SendImpact",
                ImpactId = 1131011,
                Target = Actors.monster_1,
            },
        },
    },
  {
        Name = "第七回合结束",
        Trigger = function(S)
            return S:IsBattleFinish()
        end,

        Events = {
            {
                EventType = "Idle",
                Time = 1,
            },
            {
                EventType = "PlayStoryContent",
                SC_Id = 189,
            },
        },
    },

}

return Sections