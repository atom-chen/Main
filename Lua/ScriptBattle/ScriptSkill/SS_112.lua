local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--云梦章3技能

local i_jingu  = i_mk{
    Id = 112010,
    ImpactLogic = -1,

    Duration = 1,
    AliveCheckType = 2,
    AutoFadeOutTag = 0,
    ImpactClass = 4,
    ImpactSubClass = 16384, 
    MutexID = 1,
    MutexPriority = 5,
}


local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType=1,
        I_1 = {
            Impact = i_jingu,
        }, 
    },

}
return sk_main


