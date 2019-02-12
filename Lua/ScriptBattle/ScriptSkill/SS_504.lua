
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--星魂套装技能；吸血12%

local i_xixue  = i_mk{

    ImpactLogic = 4,              --吸血24%
    Param_1 = 101,        
    Param_2 = 2400,
    Param_3 = 0,                         
    
    Duration = -1 ,
    LayerID = 502,
    LayerMax = 3,
}

local sk_main = sk_mk{

    H_1 = h_mk{                         --
        TargetType = 2,
        I_1 = {
            Impact = i_xixue ,
        },
    },

   
}

return sk_main