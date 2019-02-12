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
                SC_Id = 8806,
            },
        },
     },
  {
        Name = "敌方第4回合",
        Trigger = function(S)
            return S:IsRoundEnd(1,4)
        end,

        Events = {
            {
                EventType = "Idle",
                Time = 0.5,
            },
            {
                EventType = "PlayStoryContent",
                SC_Id = 213,
            },
        },
    },
  {
        Name = "我方第5回合",
        Trigger = function(S)
            return S:IsRoundEnd(1,5)
        end,

        Events = {
            {
                EventType = "Idle",
                Time = 0.5,
            },
            {
                EventType = "PlayStoryContent",
                SC_Id = 214,
            },
        },
    },
}

return Sections