local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")





local i_jiasu  = i_mk{
    Id = 1027010,
    EffectID =607,
    ImpactLogic = 4,
    Param_1 = 1,              
    Param_2 = 18, 
    Param_3 = 0,      
    MutexID =1581,
    MutexPriority =1,           
    Duration = 2,
    ImpactClass = 1,
    IsShow = 1,
}


local sk_main = sk_mk{
    TargetType = 1,
    H_1 = h_mk{
        I_1 = {
            Impact = i_jiasu,
        },
    },
    LogicParam_1 = 8,
}
return sk_main


