local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


--奔波儿霸2技能：单体攻击，造成自身防御X%的伤害并以80%的概率嘲讽目标，冷却时间4回合


local i_damage1  = i_mk{

    ImpactLogic = 48,              --防御伤害
    Param_1 ="a1",                 --防御力百分比
    Param_2 = 0,
    Param_4= 3,
    Param_5= 10000,
    
}


--嘲讽效果
local i_Defiance = i_mk(sc.CommonBuffs.Defiance)   --引用通用嘲讽
      i_Defiance.Duration = 1                      --修改持续回合          



local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 1,
        I_1 = {
            Impact = i_damage1,
        },
        I_2 = {                        --眩晕效果
            Chance = "a2",
            IsChanceRefix = 1,
            Impact = i_Defiance,
    }
    },
}
return sk_main


