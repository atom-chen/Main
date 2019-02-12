local Actors = {

}

local Sections = {

    {
        Name = "战斗开始前说话",
        Trigger = function(S)
            return S:IsRoundBegin(1,1)
        end,

        Events = {
            {
                EventType = "PlayStoryContent",
                SC_Id = 15807,
            },
        },
    },

    {
        Name = "战斗结束说话",
        Trigger = function(S)
            return S:IsBattleFinish()
        end,

        Events = {
            {
                EventType = "PlayStoryContent",
                SC_Id = 15815,
            },
        },
    },

}

return Sections