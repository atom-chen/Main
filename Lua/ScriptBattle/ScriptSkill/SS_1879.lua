local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--黄泉3技能，攻击敌方全体，造成四段伤害

--一段伤害
local i_damage1 = i_mk{
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
--二段伤害
local i_damage2 = i_mk{
    ImpactLogic = 48,               --根据指定的属性，计算普通伤害
    ImpactClass= 2,               

    Param_1 ="a2",                 --技能系数
    param_2 =0,                    --技能固定值系数
    Param_3 =-1,                   --同一技能相同impact多次命中衰减系数
    Param_4 =3,                    --属性1
    Param_5 =10000,                --属性修正值1
    Param_6 =-1,                   --属性2
    Param_7 =-1,                   --属性2（-1表示不参与)
    param_8 =-1,                   --属性3
    Param_9 = -1                   --属性3（-1表示不参与)
}
--三段伤害
local i_damage3 = i_mk{
    ImpactLogic = 48,               --根据指定的属性，计算普通伤害
    ImpactClass= 2,               

    Param_1 ="a3",                 --技能系数
    param_2 =0,                    --技能固定值系数
    Param_3 =-1,                   --同一技能相同impact多次命中衰减系数
    Param_4 =3,                    --属性1
    Param_5 =10000,                --属性修正值1
    Param_6 =-1,                   --属性2
    Param_7 =-1,                   --属性2（-1表示不参与)
    param_8 =-1,                   --属性3
    Param_9 = -1                   --属性3（-1表示不参与)
}
--四段伤害
local i_damage4 = i_mk{
    ImpactLogic = 48,               --根据指定的属性，计算普通伤害
    ImpactClass= 2,               

    Param_1 ="a4",                 --技能系数
    param_2 =0,                    --技能固定值系数
    Param_3 =-1,                   --同一技能相同impact多次命中衰减系数
    Param_4 =3,                    --属性1
    Param_5 =10000,                --属性修正值1
    Param_6 =-1,                   --属性2
    Param_7 =-1,                   --属性2（-1表示不参与)
    param_8 =-1,                   --属性3
    Param_9 = -1                   --属性3（-1表示不参与)
}
local i_Immune = i_mk(sc.CommonBuffs.Immune)     --引用通用免疫buff
      i_Immune.Duration = 2                              --修改持续回合             

--攻击
local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType=2,
        IsAnimHit = 0,
        I_1 = {
            Impact = i_Immune,
        },
    },
    
    
    H_2 = h_mk{
        TargetType=1,
        I_1 = {
            Impact = i_damage1,
        },
    },

    H_3 = h_mk{
        TargetType=1,
        I_1 = {
            Impact = i_damage2,
        },
    },

    H_4 = h_mk{
        TargetType=1,
        I_1 = {
            Impact = i_damage3,

        },
    },
    
    H_5 = h_mk{
        TargetType=1,
        I_1 = {
            Impact = i_damage4,
        },
    },

}
return sk_main



