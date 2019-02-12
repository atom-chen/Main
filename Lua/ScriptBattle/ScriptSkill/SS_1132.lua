local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")




local i_guard  = i_mk{
    ImpactLogic = 21,              --护卫
    Param_1 = 24,                  -- 
    Param_2 = "a1",
    Param_3 = 5000,
    Duration = -1,
    IsPassiveImpact = 1,
}

local i_save  = i_mk{
    ImpactLogic = 20,              --分血救基
    Param_1 = 24,                  -- 
    Param_2 = 3000,
    Param_3 = 5000,
    Param_4 = 2002,
    Duration = -1,
    IsPassiveImpact = 1,
}



local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 2,
        I_1 = {
            Impact = i_guard,
        },
        I_2 = {
            Impact = i_save,
        },
    },
}
return sk_main


