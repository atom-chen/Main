local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


local i_Poison = i_mk(sc.CommonBuffs.Poison)           --引用通用持续伤害
      i_Poison.Duration = 2                            --修改持续回合 


local i_damage1  = i_mk{
    ImpactLogic = 0,              -- 夜莺普通伤害
    Param_1 ="a1",                -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    ImpactClass = 2,
}




local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 1,
        I_1 = {
            IsChanceRefix = 1,
            Impact = i_Poison,
            Chance = "a2",
        },
        I_2 = {
            Impact = i_damage1,
        },

    },
}
return sk_main


