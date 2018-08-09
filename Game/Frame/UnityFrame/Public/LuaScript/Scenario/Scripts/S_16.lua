--声明参与的演员
local Actors = {
    hero = 10,
    monster_1 = 10055, --牛魔王
   -- monster_2 = 10054, --洛神
    monster_3 = 10057, --筝
    monster_4 = 10053, --桃瑶
    monster_5 = 10056, --山鬼
    monster_6 = 10051, --穷奇
    monster_7 = 10050, --黄泉
    monster_8 = 10052, --白骨妖女
    relivemonster_5 = {ReliveMonsterId = 10056}, --复活山鬼
}
-----------------------------------------------------

--剧情开始
local Sections = {
     {
        Name = "第一回合黄泉aoe",
        Trigger = function(S)
            return S:IsRoundBegin(1,1)
        end,

        Events = {

            {
                EventType = "PauseEx",--暂停战斗进行引导
                PE_Id = 1,
            },

            {
                EventType = "UseSkill",
                Actor = Actors.monster_7,
                Target = Actors.monster_1,
                SkillId = 187601,--黄泉aoe
            },
            {
                EventType = "SkipRound",
            },
        },
     },
    
     {
        Name = "第2回合白骨妖女大招",
        Trigger = function (S)
           return S:IsRoundBegin(1,2)
        end,

        Events = {


           {
               EventType = "UseSkill",
               Actor = Actors.monster_8,
               Target = Actors.monster_3,
               SkillId = 154701,
           },
           {
               EventType = "SkipRound",
           },
        },
    },
     {
        Name = "第3回合穷奇单体攻击杀山鬼",
        Trigger = function(S)
            return S:IsRoundBegin(1,3)
        end,

        Events = {
            {
                EventType = "PlayStoryContent",
                SC_Id = 215,
            },
            {
                EventType = "UseSkill",
                Actor = Actors.monster_6,
                Target = Actors.monster_5,
                SkillId = 106001,--穷奇普攻
            },
            {
                EventType = "SkipRound",
            },
        },
     },
 

     {
        Name = "第四回合桃瑶群体回血复活山鬼",
        Trigger = function(S)
            return S:IsRoundBegin(1,4)
        end,

        Events = {
            {
                EventType = "PlayStoryContent",
                SC_Id = 216,
            },
            {
                EventType = "UseSkill",
                Actor = Actors.monster_4,
                Target = Actors.relivemonster_5,
                SkillId = 102701,
            },  

            {
                EventType = "SkipRound",
            },
        },
     },



    
     {
        Name = "第5回合筝一击",
        Trigger = function (S)
            return S:IsRoundBegin(1,5)
        end,

        Events = {

            {
                EventType = "UseSkill",
                Actor = Actors.monster_3,
                Target = Actors.monster_6,
                SkillId = 111401,
            },
           
            {
                EventType = "SkipRound",
            },
        },
     },


     {
         Name = "第6回合牛魔王",
         Trigger = function(S)
            return S:IsRoundBegin(1,6)
         end,

         Events = {
            {
                EventType = "PlayStoryContent",
                SC_Id = 217,
            },
             {
                 EventType = "UseSkill",
                 Actor = Actors.monster_1,
                 Target = Actors.monster_6,
                 SkillId = 162401,
             },

             {
                 EventType = "SkipRound",
             },
         },
     },

     {
        Name = "6回合结束主角技能",
        Trigger = function (S)
            return S:IsRoundEnd(1,6)
        end,

        Events = {

            {
                EventType = "UseSkill",
                Actor = Actors.hero,
                Target = Actors.monster_6,
                SkillId = 1000,
            },
        },
     },

     {
         Name = "第7回合结束",
         Trigger = function(S)
            return S:IsRoundEnd(1,7)
         end,

         Events = {

            {
                 EventType = "ForceFinish",
                 WinSide = 1,
             },
         },
     },
}

return Sections