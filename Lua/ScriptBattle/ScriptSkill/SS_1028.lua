local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


local i_DefenceReduce = i_mk(sc.CommonBuffs.DefenceReduce)     --引用通用防御降低
      i_DefenceReduce.Duration = 2                             --修改持续回合 


local i_damage  = i_mk{
    ImpactLogic = 0,              --张震元普通伤害
    Param_1 ="a1",                -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    ImpactClass = 2,
}



local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 1,
        I_1 = {
            IsChanceRefix = 1,
            Impact = i_DefenceReduce,
            Chance = 5000,
        },
        I_2 = {
            Impact = i_damage,
        },

    },
}
return sk_main


