local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


local i_Weak = i_mk(sc.CommonBuffs.Weak)               --引用通用虚弱buff
      i_Weak.Duration = 2                              --修改持续回合 

local i_damage = i_mk{
    ImpactLogic = 0,
    ImpactClass= 2,
    Param_1 = "a1",

}

local sk_main = sk_mk{

    H_1 = h_mk{

        IsAnimHit = 1,
        I_1 = {
            Impact = i_damage,
        }, 
        I_2 = {
            Impact = i_Weak,
            IsChanceRefix = 1,
            Chance = "a2",
        }, 

    },
}

return sk_main