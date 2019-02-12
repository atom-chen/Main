--声明参与的演员
local Actors = {
    monster_1 = 10135, --劫兽
    monster_2 = 10136,--筝儿
    card_1 = {CardId = 93},--山鬼

}
-----------------------------------------------------

--剧情开始
local Sections = {

     {
        Name = "第3回合创建筝儿",
        Trigger = function(S)
            return S:IsRoundEnd(1,2)
        end,

        Events = {
            {
                EventType = "Spawn",
                Side = 1,
                BattlePos = 4,
                MonsterId = 10136,
                IsPlayer = true,
                IsPlaySpawnAnim = true,
            },
            {
                EventType = "Idle",
                Time = 0.5,
            },
            {
                EventType = "UseSkill",
                Actor = Actors.monster_2,
                Target = Actors.monster_2,
                SkillId = 2121,
            },
          
            {
                EventType = "Idle",
                Time = 0.5,
            },
            {
                EventType = "PlayStoryContent",
                SC_Id = 225,
            },
        
            {
                EventType = "SendImpact",
                ImpactId = 123,
                Target = Actors.monster_2,
            },
        },
     },
 

}

return Sections 