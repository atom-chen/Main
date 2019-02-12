local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


local i_DefenceReduce = i_mk(sc.CommonBuffs.DefenceReduce)           --引用通用持续伤害
      i_DefenceReduce.Duration = 1                                   --修改持续回合 
local i_CureProhibit = i_mk(sc.CommonBuffs.CureProhibit)                --引用通用持续伤害
      i_CureProhibit.Duration = 1                                       --修改持续回合 
local i_Weak = i_mk(sc.CommonBuffs.Weak)                                --引用通用持续伤害
      i_Weak.Duration = 1                                           --修改持续回合 
local i_SpeedReduce = i_mk(sc.CommonBuffs.SpeedReduce)                --引用通用持续伤害
      i_SpeedReduce.Duration = 1                                    --修改持续回合
local i_AttackReduce = i_mk(sc.CommonBuffs.AttackReduce)           --引用通用持续伤害
      i_AttackReduce.Duration = 1                                  --修改持续回合
local i_Poison = i_mk(sc.CommonBuffs.Poison)                        --引用通用持续伤害
      i_Poison.Duration = 1                                         --修改持续回合      
local i_damage1  = i_mk{
    ImpactLogic = 0,              -- 夜莺普通伤害
    Param_1 ="a1",                -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    ImpactClass = 2,
}




local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 1,

        I_1 = {
            Impact = i_damage1,
        },
        I_2 = {
            IsChanceRefix = 1,
            Impact = i_mk{
                ImpactLogic = 10,                   --逻辑说明：随机触发子效果 
                            
                Param_1 =1,                         --参数1：随机类型，（0第一次激活随机，之后每次固定生效,1每次效果生效时随机）
                Param_2 =i_DefenceReduce,
                Param_3 =1,
                Param_4 =i_CureProhibit,
                Param_5 =1,
                Param_6 =i_Weak,
                Param_7 =1,
                Param_8 =i_SpeedReduce,
                Param_9 =1,
                Param_10 =i_AttackReduce,
                Param_11 =1,
                Param_12 =i_Poison,
                Param_13 =1,

                ImpactClass = 4,
            },
            Chance = "a2",
        },

    },
}
return sk_main


