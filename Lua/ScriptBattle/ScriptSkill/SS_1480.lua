
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--句芒1技能：普攻+2回合持续伤害

local i_damage  = i_mk{
    ImpactClass=2,
    ImpactLogic = 48,              --特殊伤害，根据指定的属性，计算攻击力

    Param_1 = "a1",                -- 技能系数
    Param_2 = 0,                   -- 技能固定值系数
    Param_3 = -1,                  -- 同一技能相同impact多次命中衰减系数
    Param_4 = 2,                   -- 属性1
    Param_5 = 5000,                -- 属性修正值1
    Param_6 = 3,                   -- 属性修正2
    Param_7 = 6000,                -- 属性修正2

}

--持续伤害效果
local i_Poison = i_mk(sc.CommonBuffs.Poison)     --引用通用持续伤害
      i_Poison.Duration = 2                      --修改持续回合          



local sk_main = sk_mk{

    H_1 = h_mk{                         --伤害
        TargetType = 1,
        I_1 = {
            Impact = i_damage,
        },
        I_2 = {                        --持续伤害效果
            Chance = "a2",
            IsChanceRefix = 1,
            Impact = i_Poison,
    }
    },

   
}

return sk_main