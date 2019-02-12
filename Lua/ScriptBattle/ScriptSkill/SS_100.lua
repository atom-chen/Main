local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")




local i_damage1  = i_mk{
    ImpactLogic = 34,              --最大生命值伤害
    Param_1 = 500,
    ImpactClass = 2,
    ImpactSubClass = 32768 ,
}

local i_jiangdishanghai  = i_mk{
    ImpactLogic = 4,              --降低伤害
    Param_1 = 6,
    Param_2 = -2000,
    Param_3 = 0,
    ImpactClass = 4,
    Duration =1,
    AliveCheckType =2,
    MutexID =100 ,
    MutexPriority =1,

}

local i_damage2  = i_mk{
    ImpactLogic = 0,              --领导力的普通伤害
    Param_1 = 100000,
    ImpactClass = 2,
}



local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType = 4,
        I_1 = {
            Impact = i_damage1,
        },
        I_2 = {
            Impact = i_jiangdishanghai,
        },
        I_3 = {
            Impact = i_damage2,
        },
    },
}
return sk_main


