--声明参与的演员
local Actors = {
    monster_1 = 10115, --祸斗
    card_1 = {CardId = 93},--山鬼
    card_2 = {CardId = 45},--鬼兵

}
-----------------------------------------------------

--剧情开始
local Sections = {

    {
        Name = "第1回合结束给鬼兵buff",
        Trigger = function(S)
            return S:IsRoundBegin(1,1)
        end,

        Events = {
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
        },
     }, 
 
     {
        Name = "第3回合祖奶奶",
        Trigger = function(S)
            return S:IsRoundEnd(1,3)
        end,

        Events = {
            {
                EventType = "Idle",
                Time = 0.5,
            },
            {
                EventType = "PlayStoryContent",
                SC_Id = 226,
            },
        },
     },

}

return Sections