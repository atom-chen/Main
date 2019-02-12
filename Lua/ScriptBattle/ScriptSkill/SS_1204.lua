local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")




--【被动】英招受到的群体伤害降低15%。
local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType=2,
        IsAnimHit = 0 ,
        I_1 = {
            Impact = i_mk{
                ImpactClass = 1,
                ImpactLogic  =35,
                Param_1 = 3,
                Param_2 = -1,
                IsShow = 0,              
            },
        },
    },
}
return sk_main

