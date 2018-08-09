local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")





local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType=2,
        I_1 = {
            Impact = i_mk{
                Duration = 0, 
                ImpactClass = 1,
                ImpactLogic = 6,
                Param_1 = 100,
            },
        },
    },    
}
return sk_main

