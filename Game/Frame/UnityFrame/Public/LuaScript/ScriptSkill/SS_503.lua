
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--星魂套装技能；30%概率反击

local i_fanji  = i_mk{

    ImpactLogic = 33,              --以30%的概率使用1技能反击
    Param_1 = 3000,        
    Param_2 = 10000,
    Param_3 = 0,                         
    Param_4 = 1,

    Duration = -1 ,
    RoundMaxEffectedCount=1,
}

local sk_main = sk_mk{

    H_1 = h_mk{                         --
        TargetType = 2,
        I_1 = {
            Impact = i_fanji ,
        },
    },

   
}

return sk_main