local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


--酒童1技能：酒童坐着酒坛撞向敌方目标，对其造成酒童生命最大值14%的普通伤害，并以40%的概率使其受到持续1回合的嘲讽效果。


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



--嘲讽效果
local i_Defiance = i_mk(sc.CommonBuffs.Defiance)   --引用通用嘲讽
      i_Defiance.Duration = 1                      --修改持续回合          



local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 1,
        I_1 = {
            Impact = i_damage,
        },
        I_2 = {                        --眩晕效果
            Chance = "a2",
            IsChanceRefix = 1,
            Impact = i_Defiance,
    }
    },
}
return sk_main


