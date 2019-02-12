local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")



local i_AttackEnhance = i_mk(sc.CommonBuffs.AttackEnhance)   --引用通用攻击加强buff
      i_AttackEnhance.Duration = 2                            --修改持续回合    


local i_latiao = i_mk{
        Duration=0,
        ImpactClass=1,
        ImpactLogic =6,                         
        Param_1= 300,                  
}

local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 3,
        IsAnimHit = 0,
        I_1 = {
            Impact = i_latiao,
        },
    },
    H_2 = h_mk{
        TargetType = 3,
        IsAnimHit = 1,
        I_1 = {
            Impact = i_AttackEnhance,
        },
    },

}
return sk_main


