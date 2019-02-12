local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")




local i_damage  = i_mk{
    ImpactLogic = 0,              --霓普通伤害
    Param_1 ="a1",                -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    ImpactClass = 2,
}

local i_latiao  = i_mk{
    ImpactLogic = 6,              --霓拉条
    Param_1 =100,                
    ImpactClass = 1,
}

local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 1,
        I_1 = {
            Impact = i_damage,
        },
    },
    H_2 = h_mk{
        TargetType = 5,
        IsAnimHit =0,
        TargetParam_1 = 1,
        TargetParam_3 = 1,
        I_1 = {
            Impact = i_latiao,
        },
    },
    H_3 = h_mk{
        TargetType = 1,
        I_1 = {
            Impact = i_damage,
        },
    },
    H_4 = h_mk{
        TargetType = 5,
        IsAnimHit =0,
        TargetParam_1 = 1,
        TargetParam_3 = 1,
        I_1 = {
            Impact = i_latiao,
        },
    },
}
return sk_main


