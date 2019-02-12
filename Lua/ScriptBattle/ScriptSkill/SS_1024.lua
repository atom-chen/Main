local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")





local i_RoundBegin  = i_mk{
        
    Id = 1024010,
    EffectID = 605,

    MutexID = 1025 ,
    MutexPriority = 1,
    Duration =1,
    AliveCheckType =2 ,
    AutoFadeOutTag = 0,
    
    ImpactLogic = 24,
    Param_1 =102501,              
    Param_2 = 1,                  
}




local sk_main = sk_mk{
    TargetType = 1,
    H_1 = h_mk{
        I_1 = {
            Impact = i_RoundBegin,
        },
    },
}
return sk_main


