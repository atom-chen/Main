--声明参与的演员
local Actors = {
    hero = 3,

    --怪物，我方指定站位角色，使用MonsterID
    monster_1 = 7,

    --卡牌，指定卡牌ID
    myCard_1 = {CardId = 10},
    myCard_2 = {CardId = 11},
}
-----------------------------------------------------

--剧情开始
local Sections = {

    
    {
        Name = "Over",
        Trigger = function(S)
            return S:IsRoundBegin(1,1)
        end,

        Events = {
            {
                EventType = "PlayCutscene",
                BC_Id = 1,
            },
        }
    }

}

return Sections