local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


--攻击敌方单体，造成宫商防御350%的普通伤害，并为我方随机目标驱散一个减益效果。

local i_damage1  = i_mk{
    ImpactLogic = 48,              --狮爷防御伤害
    Param_1 ="a1",                 --防御力百分比
    Param_2 = 0,
    Param_4= 3,
    Param_5= 10000,
    ImpactClass = 2,
}

local i_qusan= i_mk{

    ImpactLogic = 5,                --驱散debuff

    Param_1 = 4,                    --被驱散的impact class
    Param_2 = -1,                   --subclasss
    Param_3 = -1,                   --tag
    Param_4 = 1,                    --驱散数量
    Param_5 = 1                     --是否提示
    
}   

local sk_main = sk_mk{
    
    H_1 = h_mk{
        TargetType = 1,
        I_1 = {
            Impact = i_damage1,
        },
    },

    H_2 = h_mk{
        TargetType = 5,
        TargetParam_1 = 1,
        TargetParam_2 = 0,
        I_1 = {                        
            Impact =i_qusan,            --驱散
        }
    },
    
}

return sk_main