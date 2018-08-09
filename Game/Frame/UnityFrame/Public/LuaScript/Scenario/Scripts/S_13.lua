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
        Name = "第七回合开始播放对白",
        Trigger = function(S)
            return S:IsRoundBegin(1,4)
        end,

        Events = {
            {
                EventType = "Idle",
                Time = 2,
            },
            {
                EventType = "PlayStoryContent",
                SC_Id = 212,
            },
        },
    },
  

}

return Sections