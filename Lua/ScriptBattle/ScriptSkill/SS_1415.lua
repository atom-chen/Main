local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

local sk_latiao =sk_mk{
    H_1 = h_mk{
        TargetType = 3,
        I_1 = {
            Impact = i_mk{
                ImpactLogic = 6,                                                  
                Param_1 = 300,                     --群体拉条
            },
        },
    },
}



local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 2,
        I_1 = {
            Impact = i_mk{
                ImpactLogic = 24,                                               
                Param_1 = sk_latiao,                     
                Param_2 = 11,
                Param_8 = 1414001,
                Param_9 = 2,
                
                Duration =-1,
            },
        },
    },
}
return sk_main



