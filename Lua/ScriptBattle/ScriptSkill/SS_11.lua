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


local i_bianhuli  = i_mk{
    Id = 11000,
    Duration = 3,
    IsShow =  1 ,
    MutexID =10,
    MutexPriority =1,
    ImpactLogic = 2,              

    ImpactClass = 4,
}

local i_yaojiexiaoguo  = i_mk{

    Duration = 1,
    AliveCheckType = 2,
    IsShow =  1 ,
    MutexID =11,
    MutexPriority =1,
    Tag = 10003,

}





local sk_main = sk_mk{
    H_1 = h_mk{
        IsAnimHit=0,
        TargetType = 2,
        I_1 = {
            Impact =  i_qusan,
        },
    },
    H_2 = h_mk{
        TargetType = 2,
        I_1 = {
            Impact =  i_bianhuli,
        },
    },
    H_3 = h_mk{
        TargetType = 3,
        I_1 = {
            Impact = i_yaojiexiaoguo,
        },
    },
}
return sk_main


