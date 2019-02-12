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


local i_xingdonghouchufa  = i_mk{

    Id = 12000,
    Duration = 1,
    AliveCheckType = 2,
    IsShow = 1,
    MutexID = 120,
    MutexPriority = 1,

    ImpactLogic = 24, 
    Param_1 = 1800,
    Param_2 = 3,
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
            Impact = i_xingdonghouchufa,
        },
    },
}
return sk_main


