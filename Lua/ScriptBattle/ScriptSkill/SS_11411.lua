local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

local i_AttackEnhance = i_mk(sc.CommonBuffs.AttackEnhance)   --引用通用攻击强化buff
      i_AttackEnhance.Duration = 2                            --修改持续回合 

local i_CriticalEnhance = i_mk(sc.CommonBuffs.CriticalEnhance)          --引用通用狂暴
      i_CriticalEnhance.Duration = 2                                   --修改持续回合

local i_atonce  = i_mk{
    ImpactLogic = 15,              --
    Param_1 =1,                 -- 
    Param_2 =7560,
    ImpactClass = 0,    
}


local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 2,
        IsAnimHit = 0,
        I_1 = {
            Impact = i_AttackEnhance,
        },
        I_2 = {
            Impact = i_atonce,
        },
    },
}
return sk_main


