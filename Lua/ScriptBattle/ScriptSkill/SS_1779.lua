local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

local i_Stun = i_mk(sc.CommonBuffs.Stun)           --引用通用晕眩
i_Stun.Duration = 1                            --修改持续回合



local i_damage1  = i_mk{
    ImpactLogic = 48,              --狮爷防御伤害
    Param_1 ="a1",                 --防御力百分比
    Param_2 = 0,
    Param_4= 3,
    Param_5= 10000,
    ImpactClass = 2,
}

local i_damage2  = i_mk{
    ImpactLogic = 48,              --狮爷防御伤害
    Param_1 ="a2",                 --防御力百分比
    Param_2 = 0,
    Param_4= 3,
    Param_5= 10000,
    ImpactClass = 2,
}



local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 1,
        I_1 = {
            Impact = i_damage1,
        },
        I_2 = {
            Chance = 5000,
            IsChanceRefix = 1,
            Impact = i_Stun,
        }  
    },
    H_2 = h_mk{
        TargetType = 15,
        I_1 = {
            Impact = i_damage2,
        },
    },
}
return sk_main


