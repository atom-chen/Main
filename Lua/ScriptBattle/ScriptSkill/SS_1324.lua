local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

local i_damage = i_mk{
    ImpactLogic = 0,                 --普通伤害
    ImpactClass= 2,
    Param_1 = "a1",
    
}

local i_Frozen = i_mk(sc.CommonBuffs.Frozen)     --引用通用冰冻buff
      i_Frozen.Duration = 1                              --修改持续回合 


--普攻+冰冻901001
local sk_main = sk_mk{                      
    H_1 = h_mk{                          --普通攻击
    IsAnimHit = 1,
        I_1 = {
            Impact = i_damage,
        },
        I_2 = {
            Impact = i_Frozen,
            IsChanceRefix = 1,
        }    
}
}
return sk_main