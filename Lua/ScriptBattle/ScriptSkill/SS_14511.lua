local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


local i_DefenceReduce = i_mk(sc.CommonBuffs.DefenceReduce)           
      i_DefenceReduce.Duration = 2                                    
local i_CureProhibit = i_mk(sc.CommonBuffs.CureProhibit)                
      i_CureProhibit.Duration = 2                                       
local i_Weak = i_mk(sc.CommonBuffs.Weak)                                
      i_Weak.Duration = 2                                            
local i_SpeedReduce = i_mk(sc.CommonBuffs.SpeedReduce)                
      i_SpeedReduce.Duration = 2                                   
local i_AttackReduce = i_mk(sc.CommonBuffs.AttackReduce)           
      i_AttackReduce.Duration = 2                                  
local i_Poison = i_mk(sc.CommonBuffs.Poison)                       
      i_Poison.Duration = 2                                       
local i_Stun = i_mk(sc.CommonBuffs.Stun)                        
      i_Stun.Duration = 2                                       
      






local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 4,

        I_1 = {
            IsChanceRefix = 1,
            Impact = i_mk{
                ImpactLogic = 10,                   --逻辑说明：随机触发子效果 
                            
                Param_1 =1,                         --参数1：随机类型，（0第一次激活随机，之后每次固定生效,1每次效果生效时随机）
                Param_2 =i_DefenceReduce,
                Param_3 =3,
                Param_4 =i_CureProhibit,
                Param_5 =3,
                Param_6 =i_Weak,
                Param_7 =3,
                Param_8 =i_SpeedReduce,
                Param_9 =3,
                Param_10 =i_AttackReduce,
                Param_11 =3,
                Param_12 =i_Poison,
                Param_13 =3,
                Param_14 =i_Stun,
                Param_15 =1,

                ImpactClass = 4,
            },
        },
        I_2 = {
            IsChanceRefix = 1,
            Impact = i_mk{
                ImpactLogic = 10,                   --逻辑说明：随机触发子效果 
                            
                Param_1 =1,                         --参数1：随机类型，（0第一次激活随机，之后每次固定生效,1每次效果生效时随机）
                Param_2 =i_DefenceReduce,
                Param_3 =3,
                Param_4 =i_CureProhibit,
                Param_5 =3,
                Param_6 =i_Weak,
                Param_7 =3,
                Param_8 =i_SpeedReduce,
                Param_9 =3,
                Param_10 =i_AttackReduce,
                Param_11 =3,
                Param_12 =i_Poison,
                Param_13 =3,
                Param_14 =i_Stun,
                Param_15 =1,

                ImpactClass = 4,
            },
        },

    },
}
return sk_main


