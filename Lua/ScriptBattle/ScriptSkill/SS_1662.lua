local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

local i_damage1  = i_mk{
    ImpactLogic = 48,              --宫商防御伤害
    Param_1 ="a1",                 --防御力百分比
    Param_2 = 0,
    Param_4= 3,
    Param_5= 10000,
    ImpactClass = 2,
}

local i_damage2  = i_mk{
    ImpactLogic = 48,              --宫商防御伤害
    Param_1 ="a2",                 --防御力百分比
    Param_2 = 0,
    Param_4= 3,
    Param_5= 10000,
    ImpactClass = 2,
}

local i_damage3  = i_mk{
    ImpactLogic = 48,              --宫商防御伤害
    Param_1 ="a3",                 --防御力百分比
    Param_2 = 0,
    Param_4= 3,
    Param_5= 10000,
    ImpactClass = 2,
}

local i_qusan= i_mk{
    ImpactLogic = 5,                --驱散buff
    
    Param_1 = 1,                    --被驱散的impact class
    Param_2 = -1,                   --subclasss
    Param_3 = -1,                   --tag
    Param_4 = 1,                    --驱散数量
    Param_5 = 1                     --是否提示
}   

local i_tuitiao = i_mk{
    ImpactClass=4,
    ImpactLogic = 6,                --行动条增减  
    Param_1 = -200,                 --参数：增减数（负数表示减少）
}

--攻击敌方单体3次，分别造成宫商防御120%，150%，200%的普通伤害。前两段攻击各以50%的概率驱散目标的一个增益效果，第三段攻击将击退目标20%的行动条。冷却时间5回合。
local sk_main = sk_mk{
    
    H_1 = h_mk{
        TargetType = 1,
        I_1 = {
            Impact = i_damage1,
        },
        I_2 = {
            Impact = i_qusan,
            Chance = 5000,
            IsChanceRefix = 1,
        },
    },
    H_2 = h_mk{
        TargetType = 1,
        I_1 = {
            Impact = i_damage1,
        },
        I_2 = {
            Impact = i_qusan,
            Chance = 5000,
            IsChanceRefix = 1,
        },
    },
    H_3 = h_mk{
        TargetType = 1,
        I_1 = {
            Impact = i_damage1,
        },
        I_2 = {
            Impact = i_tuitiao,
            IsChanceRefix = 1,
        },
    },


    
}

return sk_main