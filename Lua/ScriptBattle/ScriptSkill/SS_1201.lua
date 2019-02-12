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
        I_1 = {
            Impact = i_mk{
                ImpactLogic  =31,
                Param_1 = -1,
                Param_2 = "a1",
                IsShow = 0,
                Duration = -1,  
                IsPassiveImpact = 1,             
            },
        },
    },
    IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放
}
return sk_main

