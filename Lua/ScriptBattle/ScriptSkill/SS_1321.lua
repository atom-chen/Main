local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

-- --水盾
-- local i_watershield  = i_mk{
--     Id = "a1",
--     Duration = 1, 
--     MutexID = 1326,  
--     MutexPriority = 1,
--     ImpactClass = 1,
--     ImpactLogic = 9,
--     Param_1 = "a2", 
    
-- }


-- --冰盾
-- local i_iceshield  = i_mk{
--     Id = "a3",
--     Duration = 1, 
--     MutexID = 1326,  
--     MutexPriority = 1,
--     ImpactClass = 1,
--     ImpactLogic = 8,
--     Param_1 = "a4", 
-- }


-- local sk_shield = sk_mk{
--     H_1 = h_mk{
--         TargetType = 2,
--         I_1 ={
--                 Impact =  i_watershield, 
--                              --人界水盾
--         },
--     },
--         EnvLimit = 0,
--     OtherID = sk_mk{
--         H_1 = h_mk{
--             TargetType = 2,
--             I_1 ={
--                 Impact =  i_iceshield,   
--                            --妖界冰盾
--             },
--         },
--         EnvLimit = 1,   
--     },

-- }





local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType = 2,
        I_1 = {
            Impact = i_mk{
                Duration=-1,
                IsShow = 0,
                ImpactLogic = 24,
                Param_1 = "a1",
                Param_2 =3,         
                IsPassiveImpact = 1,       
            }
        },
        --周期触发 人界护盾 妖界冰盾

    },
    IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放
}

return sk_main