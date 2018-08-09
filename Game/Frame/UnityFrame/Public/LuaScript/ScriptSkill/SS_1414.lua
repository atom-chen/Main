local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")



local sk_reduceCD =sk_mk{
    H_1 = h_mk{
        TargetType = 2,
        I_1 = {
            Impact = i_mk{
                ImpactLogic = 35,                                                  
                Param_1 = 3,                     --减少3技能CD
                Param_2 = -1,
                Duration =0,
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
                Param_1 = sk_reduceCD,                     
                Param_2 = 11,
                Param_3 = -1,
                Param_4 = 1414001,
                Param_5 = -1,
                Param_6 = 1,
                Duration =-1,
            },
        },
    },
}
return sk_main


