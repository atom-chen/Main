
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--选择一个我方队友，使其立刻获得攻击力提升，生命恢复


--攻击力增强效果
local i_AttackEnhance = i_mk(sc.CommonBuffs.AttackEnhance)   
      i_AttackEnhance.Duration = 2                      --修改持续回合          

--通用恢复
local i_Cure = i_mk(sc.CommonBuffs.Cure)   
      i_Cure.Duration = 2                      --修改持续回合       



local sk_main = sk_mk{

    H_1 = h_mk{                         
        TargetType = 1,
        I_1 = {
            Impact = i_AttackEnhance,
        },
        I_2 = {                       
            Impact = i_Cure,
    }
    },

   
}

return sk_main