local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


local i_qusan  = i_mk{
    ImpactLogic = 5,              
    Param_1 = 4,
    Param_4 = 1,
    Param_5 = 0,
    IsShow =1 ,
}


local i_damage1  = i_mk{
    ImpactLogic = 34,              --最大生命值伤害
    Param_1 = 1000,
    ImpactClass = 2,
    ImpactSubClass = 32768 ,
}

local i_qingtiao  = i_mk{
    ImpactLogic = 6,              --清空行动条
    Param_1 = -1000,
    ImpactClass = 4,
}

local i_damage2  = i_mk{
    ImpactLogic = 0,              --领导力的普通伤害
    Param_1 = 200000,
    ImpactClass = 2,
}



local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 2,
        I_1 = {
            Impact = i_qusan,
        },
    },
    H_2 = h_mk{
        TargetType = 1,
        I_1 = {
            Impact = i_damage1,
        },
        I_2 = {
           
            Impact = i_qingtiao,
        },
        I_3 = {
            Impact = i_damage2,
        },
    },
}
return sk_main


