local Actors = {
    card_1 = {CardId = 93},--山鬼
    card_2 = {CardId = 45},--鬼兵
    monster_1 = 10120,--官兵1
    monster_2 = 10121,--官兵2 一速
}

local Sections = {

    {
        Name = "引导 外加给鬼兵和山鬼100%暴击buff",
        Trigger = function(S)
            return S:IsRoundBegin(1,1)
        end,

        Events = {
            {
                EventType = "PauseEx",--暂停战斗进行引导
                PE_Id = 1,
            },
            {
                EventType = "SendImpact",
                ImpactId = require("ScriptSkill/ScriptImpact/JQ_21"),
                Target = Actors.card_2,
            },
            {
                EventType = "SendImpact",
                ImpactId = require("ScriptSkill/ScriptImpact/JQ_21"),
                Target = Actors.card_1,
            },
            --{
              --  EventType = "UseSkill",
              --  Actor = Actors.monster_2,
              --  Target = Actors.card_1,
              --  SkillId = 189001,
            --},
            --{
              --  EventType = "SkipRound",
            --},
        },
    },



    {
        Name = "怒气增加",
        Trigger = function(S)
            return S:IsRoundEnd(1,3)
        end,

        Events = {
            {
                EventType = "GainSp",
                Side = 1,
                SP = 500,
            },
            
        },
    },

}

return Sections