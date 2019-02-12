local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")



local i_damage = i_mk{
    ImpactLogic = 0,
    ImpactClass= 2,
    Param_1 = "a1",
}

local i_BuffRoundDown = i_mk{
    ImpactLogic = 47,
    ImpactClass= 0,
    Param_1 = 1,
    Param_2 = -1,
    Param_3 = 99,
}

local i_DebuffRoundUp = i_mk{
    ImpactLogic = 47,
    ImpactClass= 0,
    Param_1 = 4,
    Param_2 = 1,
    Param_3 = 99,
}

--落叶分三次，扫向敌方全体，并降低对面buff持续时间一回合
local sk_main = sk_mk{

    H_1 = h_mk{

        I_1 = {
            Impact = i_damage,
        },
        I_2 = {
            Impact = i_BuffRoundDown,
            IsChanceRefix = 1,
            Chance = 5000,
        },    
        I_3 = {
            Impact = i_DebuffRoundUp,
            IsChanceRefix = 1,
            Chance = 5000,
        }   

    },
    H_2 = h_mk{

        I_1 = {
            Impact = i_damage,
        },
        I_2 = {
            Impact = i_BuffRoundDown,
            IsChanceRefix = 1,
            Chance = 5000,
        },    
        I_3 = {
            Impact = i_DebuffRoundUp,
            IsChanceRefix = 1,
            Chance = 5000,
        }   

    },
}

return sk_main