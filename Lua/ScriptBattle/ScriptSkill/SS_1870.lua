local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--黄泉1技能

--普通伤害
local i_damage = i_mk{
    ImpactLogic = 48,               --根据指定的属性，计算普通伤害
    ImpactClass= 2,               

    Param_1 ="a1",                 --技能系数
    param_2 =0,                    --技能固定值系数
    Param_3 =-1,                   --同一技能相同impact多次命中衰减系数
    Param_4 =3,                    --属性1
    Param_5 =10000,                --属性修正值1
    Param_6 =-1,                   --属性2
    Param_7 =-1,                   --属性2（-1表示不参与)
    param_8 =-1,                   --属性3
    Param_9 = -1                   --属性3（-1表示不参与)
}
local i_Defiance = i_mk(sc.CommonBuffs.Defiance)   --引用通用嘲讽buff
      i_Defiance.Duration = 1                      --修改持续回合             

--普攻
local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType=1,
        I_1 = {
            Impact = i_damage,
        },

        I_2 = {
            Chance = "a2",
            IsChanceRefix = 1,
            Impact = i_Defiance,
        }    
    },
}
return sk_main

