local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")






 ---------------------------------------------------------波次开始、阴阳界切换，主动走1次反击被动--------------------------------------------

local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 2,
        IsAnimHit = 0,
        I_1 = {
             Impact =i_mk{
            ImpactLogic = 24,                     
            Param_1 = "a1", 
            Param_2 = 12, 
            Duration =-1,
            IsShow = -1,
            AliveCheckType = 0,
        }
    },
        I_2 = {
        Impact =i_mk{
       ImpactLogic = 24,                     
       Param_1 = "a1", 
       Param_2 = 2, 
       Duration =-1,
       IsShow = -1,
       AliveCheckType = 0,
   }
}
}
}
return sk_main





-- --被动反击
-- local sk_main = sk_mk{
--     H_1 = h_mk{
--         TargetType = 2,
--         IsAnimHit = 0,
--         I_1 = {
--              Impact =i_mk{
--             ImpactLogic = 17,                     
--             Param_1 = 3000, 
--             Param_2 = "a1", 
--             Param_3 = 1, 
--             Param_4 = 0,
--             Param_5 = 1,  
--             Duration =-1,
--             AliveCheckType = 0,
--             AutoFadeOutTag = 0 ,
--             MutexID = 1643,
--             MutexPriority = 1,
--             RoundMaxEffectedCount = 1,
--         }
--     }
-- },



-- }
-- return sk_main


