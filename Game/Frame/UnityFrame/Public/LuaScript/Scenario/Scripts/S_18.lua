--第一章第三回穷奇祸斗boss战
--声明参与的演员
local Actors = {
    hero = 3,
    monster_1 = 10060, --石狮子1
    monster_2 = 10061, --石狮子2
    monster_3 = 10063, --牛魔
    card_1 = {CardId = 93},--山鬼
    card_2 = {CardId = 45},--鬼兵
    
}
-----------------------------------------------------

--剧情开始
local Sections = {
    {
        Name = "第一回合开战对话",
        Trigger = function(S)
            return S:IsRoundBegin(1,1)
        end,

        Events = {

            {
                EventType = "Spawn",
                Side = 1,
                BattlePos = 1,
                MonsterId = 10063,
                IsPlayer = true,
            },
            {
                EventType = "Idle",
                Time = 0.5,
            },
            {
                EventType = "PlayStoryContent",
                SC_Id = 220,
            },
            {
                EventType = "PauseEx",--暂停战斗进行引导
                PE_Id = 1,
            },
            {
                EventType = "UseSkill",
                Actor = Actors.monster_1,
                Target = Actors.card_1,
                SkillId = 177401,--石狮子普攻
            },
            {
                EventType = "SkipRound",
            },
        },
     },
     {
        Name = "第二回合开战对话",
        Trigger = function(S)
            return S:IsRoundBegin(1,2)
        end,

        Events = {
            {
                EventType = "Idle",
                Time = 0.5,
            },
            {
                EventType = "PlayStoryContent",
                SC_Id = 221,
            },
            {
                EventType = "PauseEx",--暂停战斗进行引导
                PE_Id = 1,
            },            
            {
                EventType = "UseSkill",
                Actor = Actors.monster_2,
                Target = Actors.card_2,
                SkillId = 177401,--石狮子普攻
            },
            {
                EventType = "SkipRound",
            },
        },
     },

     {
        Name = "第4回合开战对话",
        Trigger = function(S)
            return S:IsRoundBegin(1,4)
        end,

        Events = {
            {
                EventType = "Idle",
                Time = 0.5,
            },
            {
                EventType = "PlayStoryContent",
                SC_Id = 222,
            },

        },
     },
     {
        Name = "第5回合开战对话",
        Trigger = function(S)
            return S:IsRoundBegin(1,5)
        end,

        Events = {
            {
                EventType = "Idle",
                Time = 0.5,
            },
            {
                EventType = "PlayStoryContent",
                SC_Id = 223,
            },

        },
     },

    {
        Name = "第6回合开战对话",
        Trigger = function(S)
            return S:IsRoundBegin(1,6)
        end,

        Events = {

            {
                EventType = "UseSkill",
                Actor = Actors.monster_2,
                Target = Actors.card_1,
                SkillId = 177401,--石狮子普攻
            },
            
            {
                EventType = "SkipRound",
            },
        },
     },   

     {
        Name = "第6回合结束对话",
        Trigger = function(S)
            return S:IsRoundEnd(1,6)
        end,

        Events = {

            {
                EventType = "Idle",
                Time = 0.5,
            },
            {
                EventType = "PlayStoryContent",
                SC_Id = 224,
            },
        },
     },   
}

return Sections