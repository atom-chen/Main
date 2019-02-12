local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")





local i_huixue  = i_mk{

    
    ImpactLogic = 1,
    Param_1 = 2,              
    Param_2 = 4000,                  
}




local sk_main = sk_mk{
    TargetType = 3,
    H_1 = h_mk{
        I_1 = {
            Impact = i_huixue,
        },
    },
}
return sk_main


