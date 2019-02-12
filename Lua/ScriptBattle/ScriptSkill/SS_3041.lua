local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")





local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType=2,
        I_1 = {
            Impact = i_mk{
                Duration = -1, 
                MutexID = 3041,
                MutexPriority = 1,
                ImpactLogic = 4,
                Param_1 = 105,
                Param_2 = 3000,
                Param_3  =0, 
                IsShow = 0,
                IsPassiveImpact = 1, 
            },
        },

    },    
}
return sk_main

