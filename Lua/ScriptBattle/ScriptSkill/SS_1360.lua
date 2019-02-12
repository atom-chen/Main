local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


local i_Frozen = i_mk(sc.CommonBuffs.Frozen)           --引用通用冰冻
      i_Frozen.Duration = 1                            --修改持续回合 


local i_damage  = i_mk{
    ImpactLogic = 0,              --氐普通伤害
    Param_1 ="a1",                -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    ImpactClass = 2,
}



local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 1,
        I_1 = {
            Impact = i_damage,
        },
        I_2 = {
            IsChanceRefix = 1,
            Impact = i_Frozen,
            Chance = "a2",
        },

    },
}
return sk_main


