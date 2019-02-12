
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--四鸾衔绶镜2技能【被动】四鸾护体，四鸾衔绶纹镜受到的每次伤害，不能超过四鸾衔绶纹镜生命上限的1/4。

local i_shangxian  = i_mk{
    Duration= -1,
    AliveCheckType=1,
    IsPassiveImpact = 1, 

    ImpactLogic = 53,             

    Param_1 = 0,               
    Param_2 = 2500,                  
}


local sk_main = sk_mk{


 
    H_1 = h_mk{                         --伤害
        TargetType = 2,
        I_1 = {
            Impact = i_shangxian,
        },

    },

   
}

return sk_main