--声明参与的演员
local Actors = {
    monster_1 = 10115, --祸斗
    card_1 = {CardId = 93},--山鬼

}
-----------------------------------------------------

--剧情开始
local Sections = {

     {
        Name = "第3回合祸斗攻击山鬼",
        Trigger = function(S)
            return S:IsRoundBegin(1,3)
        end,

        Events = {

            {
                EventType = "UseSkill",
                Actor = Actors.monster_1,
                Target = Actors.card_1,
                SkillId = 128001,--祸斗普攻
            },
            {
                EventType = "SkipRound",
            },
        },
     },
 
     {
        Name = "第6回合祸斗",
        Trigger = function(S)
            return S:IsRoundBegin(1,6)
        end,

        Events = {
            {
                EventType = "PauseEx",--暂停战斗进行引导
                PE_Id = 1,
            },
        },
     },

}

return Sections