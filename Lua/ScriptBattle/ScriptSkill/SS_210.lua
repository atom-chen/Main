local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--判定山鬼的协战指定人选专用技能

local i_xiezhanzhiding  = i_mk{
    Duration = -1,
    AliveCheckType = 2,
    AutoFadeOutTag=0,
    IsShow=1,
    ImpactClass=4,
    ImpactSubClass=0,


    ImpactLogic = 4,
    Param_1 = 3,
    Param_2 = -1,  
    Param_3 = 0,  

}

local sk_main = sk_mk{
    H_1 = h_mk{
        IsAnimHit=0,
        TargetType=2,
        I_1 = {
            Impact = i_xiezhanzhiding,
        }, 
    },

}
return sk_main


