local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")




local i_damage  = i_mk{
    ImpactLogic = 0,              --虹普通伤害
    Param_1 ="a1",                -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    ImpactClass = 2,
}


local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 1,
        I_1 = {
            Impact = i_damage,
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


