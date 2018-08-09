--local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


local i_clear = i_mk{
    ImpactLogic = 5,

    Param_1 = 4,
    Param_2 = -1,
    Param_3 = -1,
    Param_4 = 99,
    Param_5 = -1,
    Duration = 0,
}

local i_getaround = i_mk{
    ImpactLogic = 15,

    Param_1 = 1,
    Param_2 = 7560,
    Duration = 0,

}







local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType = 1,
        I_1 = {
            Impact = i_clear,
            
        }
    },

    H_2 = h_mk{
        TargetType = 1,
        I_1 = {
            Impact = i_getaround,
        },

        
      
    },
}

return sk_main