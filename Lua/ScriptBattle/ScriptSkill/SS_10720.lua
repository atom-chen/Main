local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

local i_damage1  = i_mk{
    ImpactLogic = 0,              --玉兔普通伤害
    Param_1 ="a1",                -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    ImpactClass = 2,
}

local i_tuitiao = i_mk{
    ImpactClass= 4,
    ImpactLogic = 6,                --行动条增减  
    Param_1 = -200,                 --参数：增减数（负数表示减少）
}



local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 1,
        I_1 = {
            IsChanceRefix = 1,
            Impact = i_tuitiao,
            Chance = "a2",
        },
        I_2 = {
            Impact = i_damage1,
        },
    },
    H_2 = h_mk{
        TargetType = 1,
        I_1 = {
            IsChanceRefix = 1,
            Impact = i_tuitiao,
            Chance = "a2",
        },
        I_2 = {
            Impact = i_damage1,
        },
    },
    H_3 = h_mk{
        TargetType = 1,
        I_1 = {
            IsChanceRefix = 1,
            Impact = i_tuitiao,
            Chance = "a2",
        },
        I_2 = {
            Impact = i_damage1,
        },
    },
    H_4 = h_mk{
        TargetType = 1,
        I_1 = {
            IsChanceRefix = 1,
            Impact = i_tuitiao,
            Chance = "a2",
        },
        I_2 = {
            Impact = i_damage1,
        },
    },
}
return sk_main


