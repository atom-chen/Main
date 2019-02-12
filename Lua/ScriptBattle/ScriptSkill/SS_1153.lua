
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--瓶灵3觉醒技能：瓶灵幻化出一个瓶子从天而降攻击敌人，造成自身生命最大值28%的普通伤害，同时使其受到持续3回合的持续伤害效果，并以40%的概率使其受到持续1回合的沉睡效果。冷却时间4回合。


local i_damage  = i_mk{
    ImpactClass=2,
    ImpactLogic = 48,              --特殊伤害，根据指定的属性，计算攻击力

    Param_1 = "a1",                -- 技能系数
    Param_2 = 0,                   -- 技能固定值系数
    Param_3 = -1,                   -- 同一技能相同impact多次命中衰减系数
    Param_4 = 0,                    -- 属性1
    Param_5 = 10000,                -- 属性修正值1
    Param_6 = -1,                   -- 属性修正2
    Param_7 = -1,                   -- 属性修正2

}


--沉睡效果
local i_Sleep = i_mk(sc.CommonBuffs.Sleep)             --引用通用沉睡效果
      i_Sleep.Duration = 1                             --修改持续回合          

--持续伤害效果
local i_Poison = i_mk(sc.CommonBuffs.Poison)             --引用通用持续伤害效果
      i_Poison.Duration = 3                             --修改持续回合          


local sk_main = sk_mk{

    H_1 = h_mk{                         --伤害
        TargetType = 1,
        I_1 = {
            Impact = i_damage,
        },
        I_2 = {                        --沉睡效果
            Chance = "a2",
            IsChanceRefix = 1,
            Impact = i_Sleep,
        },
        I_3 = {                        --持续伤害效果
        IsChanceRefix = 1,
        Impact = i_Poison,
        },
    },

   
}

return sk_main