local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

local i_damage = i_mk{
    ImpactLogic = 48,               --根据指定的属性，计算普通伤害
    ImpactClass= 2,               

    Param_1 ="a1",                 --技能系数
    param_2 =0,                    --技能固定值系数
    Param_3 =-1,                   --同一技能相同impact多次命中衰减系数
    Param_4 =0,                    --属性1
    Param_5 =10000,                --属性修正值1
    Param_6 =-1,                   --属性2
    Param_7 =-1,                   --属性2（-1表示不参与)
    param_8 =-1,                   --属性3
    Param_9 = -1                   --属性3（-1表示不参与)
}

--普通攻击
local sk_main = sk_mk{

    H_1 = h_mk{

        I_1 = {
            Impact = i_damage,
        },
  
    },

}

return sk_main