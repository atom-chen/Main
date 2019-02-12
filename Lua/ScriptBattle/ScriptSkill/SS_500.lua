
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--星魂套装技能；每场战斗第一次出手，伤害翻倍

local i_AddDamage  = i_mk{

    ImpactLogic = 32,              --战斗开始（包括新波次）时，第一个行动的符灵伤害翻倍，若第一个行动的符灵的技能不是直接攻击，则这个效果被浪费
    Param_1 = 10000,                                 
    
    Duration = -1 ,
    RoundMaxEffectedCount = 5
}

local sk_main = sk_mk{

    H_1 = h_mk{                         --
        TargetType = 2,
        I_1 = {
            Impact = i_AddDamage ,
        },
    },

   
}

return sk_main