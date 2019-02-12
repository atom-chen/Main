
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--选择一个我方队友，使其立刻获得攻击力提升，生命恢复，人界状态下，技能2使用后使目标立刻获得回合


--攻击力增强效果
local i_AttackEnhance = i_mk(sc.CommonBuffs.AttackEnhance)   
      i_AttackEnhance.Duration = 2                      --修改持续回合          

--通用恢复
local i_Cure = i_mk(sc.CommonBuffs.Cure)   
      i_Cure.Duration = 2                      --修改持续回合       

-- --获得回合
local i_getaround = i_mk{
    ImpactLogic = 15,                   --下回合立即行动 

    Param_1 = 1,                         --是否无视主角（1无视,0不无视）
    Param_2 = 7560,
    Duration = 0,

}


local sk_main = sk_mk{

    H_1 = h_mk{                         
        TargetType = 1,
     
        I_1 = {
            Impact = i_AttackEnhance,
        },
        I_2 = {                       
            Impact = i_Cure,
        },
        I_3 = {
            Impact = i_getaround,
        },
    },

   
}

return sk_main