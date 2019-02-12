local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

local i_DefenceEnhance = i_mk(sc.CommonBuffs.DefenceEnhance)   --引用通用防御强化buff
      i_DefenceEnhance.Duration = 2                            --修改持续回合 

local i_Immune = i_mk(sc.CommonBuffs.Immune)                  --引用通用免疫BUFF
      i_Immune.Duration = 2                                   --修改持续回合

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
            Impact = i_DefenceEnhance,
        },
        I_2 = {
            Impact = i_atonce,
        },
    },
}
return sk_main


