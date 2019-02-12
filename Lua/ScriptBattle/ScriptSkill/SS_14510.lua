local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

local i_SpeedEnhance = i_mk(sc.CommonBuffs.SpeedEnhance)                --急速 
      i_SpeedEnhance.Duration = 2  
local i_AttackEnhance = i_mk(sc.CommonBuffs.AttackEnhance)              --攻击强化
      i_AttackEnhance.Duration = 2                                    
local i_Cure = i_mk(sc.CommonBuffs.Cure)                              --恢复
      i_Cure.Duration = 2      
local i_Immune = i_mk(sc.CommonBuffs.Immune)                            --免疫
      i_Immune.Duration = 2                                 
local i_DefenceEnhance = i_mk(sc.CommonBuffs.DefenceEnhance)            --防御强化              
      i_DefenceEnhance.Duration = 2                                                                               
local i_CriticalEnhance = i_mk(sc.CommonBuffs.CriticalEnhance)          --狂暴              
      i_CriticalEnhance.Duration = 2        
local i_Invincible = i_mk(sc.CommonBuffs.Invincible)                    --无敌    
      i_Invincible.Duration = 2                                         




local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 3,

        I_1 = {
            Impact = i_mk{
                ImpactLogic = 10,                   --逻辑说明：随机触发子效果 
                            
                Param_1 =1,                         --参数1：随机类型，（0第一次激活随机，之后每次固定生效,1每次效果生效时随机）
                Param_2 =i_SpeedEnhance,
                Param_3 =3,
                Param_4 =i_AttackEnhance,
                Param_5 =3,
                Param_6 =i_Cure,
                Param_7 =3,
                Param_8 =i_Immune,
                Param_9 =3,
                Param_10 =i_DefenceEnhance,
                Param_11 =3,
                Param_12 =i_CriticalEnhance,
                Param_13 =3,
                Param_14 =i_Invincible,
                Param_15 =1,

                ImpactClass = 1,
            },
        },
        I_2 = {
            Impact = i_mk{
                ImpactLogic = 10,                   --逻辑说明：随机触发子效果 
                            
                Param_1 =1,                         --参数1：随机类型，（0第一次激活随机，之后每次固定生效,1每次效果生效时随机）
                Param_2 =i_SpeedEnhance,
                Param_3 =3,
                Param_4 =i_AttackEnhance,
                Param_5 =3,
                Param_6 =i_Cure,
                Param_7 =3,
                Param_8 =i_Immune,
                Param_9 =3,
                Param_10 =i_DefenceEnhance,
                Param_11 =3,
                Param_12 =i_CriticalEnhance,
                Param_13 =3,
                Param_14 =i_Invincible,
                Param_15 =1,

                ImpactClass = 1,
            },
        },

    },
}
return sk_main


