local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")




local i_damage  = i_mk{
    ImpactLogic = 0,              --英招普通伤害
    Param_1 ="a1",                -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    ImpactClass = 2,
}

local i_ifcrit  = i_mk{
    Duration = 1,
    AliveCheckType = 2,
    AutoFadeOutTag = 16,
    RoundMaxEffectedCount = 1,
    ImpactLogic = 44,
    Param_1 = "a2",
}


local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 2,
        IsAnimHit = 0,
        I_1 = {
            Impact = i_ifcrit,
        },
    },
    H_2 = h_mk{
        TargetType = 1,
        I_1 = {
            Impact = i_damage,
        },
    },
}
return sk_main


