local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")




local i_renjiexiaoguo  = i_mk{

    Duration = 1,
    AliveCheckType = 2,
    IsShow =  1 ,
    MutexID =11,
    MutexPriority =1,
    Tag = 10002,

}



local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType = 3,
        I_1 = {
            Impact = i_renjiexiaoguo,
        },
    },
}
return sk_main


