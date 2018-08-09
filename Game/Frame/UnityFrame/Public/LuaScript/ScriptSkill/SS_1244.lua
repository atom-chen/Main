local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")






local sk_main = sk_mk{

    H_1 = h_mk{                                          
      TargetType = 14,
      IsAnimHit = 1,
      TargetParam_1 = 1,
      TargetParam_2 = 1,

        I_1 = {
            Impact =  i_mk{  
                Tag = 1,
                ImpactLogic = 5,  
                Param_1 = 1,   
                Param_4 = 1,   
        },
    },
},
}


return sk_main

