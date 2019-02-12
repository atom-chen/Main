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
                and S:CanFindActor(Actors.card_2)
        end,

        Events = {

            {
                EventType = "Spawn",
                Side = 1,
                BattlePos = 2,
                MonsterId = 10063,
                IsPlayer = true,
                IsPlaySpawnAnim = true,
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
                EventType = "SendImpact",
                ImpactId = require("ScriptSkill/ScriptImpact/JQ_18_1"),
                Target = Actors.monster_1,-- 给石狮子上加攻buff
            },
            {
                EventType = "UseSkill",
                Actor = Actors.monster_1,
                Target = Actors.card_2,--山鬼
                SkillId = 177401,--石狮子普攻
            },
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
               PE_Id = 2,
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
                and S:CanFindActor(Actors.card_2)
        end,

        Events = {
      
            {
                EventType = "UseSkill",
                Actor = Actors.monster_2,
                Target = Actors.monster_3,--傲因
                SkillId = 177401,--石狮子普攻
            },
            {
                EventType = "SendImpact",
                ImpactId = require("ScriptSkill/ScriptImpact/JQ_18_2"),
                Target = Actors.card_2,-- 给鬼兵上减攻buff
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
                and S:CanFindActor(Actors.card_2)
        end,

        Events = {
            {
                EventType = "PauseEx",--暂停战斗进行引导
                PE_Id = 3,
            },
            {
                EventType = "SendImpact",
                ImpactId = require("ScriptSkill/ScriptImpact/JQ_18_4"),
                Target = Actors.monster_1,-- 给石狮子1上减防buff
            },     
            {
                EventType = "SendImpact",
                ImpactId = require("ScriptSkill/ScriptImpact/JQ_18_3"),
                Target = Actors.card_2,-- 给鬼兵上加攻buff
            },    
            {
                EventType = "UseSkill",
                Actor = Actors.monster_1,
                Target = Actors.card_2,--鬼兵
                SkillId = 177401,--石狮子普攻
            },
            {
                EventType = "SkipRound",
            },
        },
     },
     {
        Name = "第5回合开战对话",
        Trigger = function(S)
            return S:IsRoundBegin(1,5)
                and S:CanFindActor(Actors.monster_3)
        end,

        Events = {
            {
                EventType = "UseSkill",
                Actor = Actors.monster_2,
                Target = Actors.monster_3,--傲因
                SkillId = 177401,--石狮子普攻
            },
            {
                EventType = "SkipRound",
            },
        },
     },
     {
        Name = "第6回合开战对话",
        Trigger = function(S)
            return S:IsRoundBegin(1,6)
                and S:CanFindActor(Actors.card_2)
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


}

return Sections