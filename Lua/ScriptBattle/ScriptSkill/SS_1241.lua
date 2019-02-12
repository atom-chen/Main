local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")




--普攻+30%概率引爆炸弹
local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType=2,
        I_1 = {
            Impact = i_mk{
                ImpactLogic = 24,                                             
                Param_1 = "a1",                           
                Param_2 = 3, 
                Param_3 = 1,
                Param_4 = 1,        
                Duration = -1,               
            },
        },

        I_2 = {
            Impact = i_mk{
                ImpactLogic = 39,                                             
                Param_1 = 1,                           
                Param_2 = 3, 
                Param_3 = "a2",
                Param_4 = 0,        
                Duration = -1, 
                IsPassiveImpact = 1,               
            },

        
        }    
    },
    IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放
}
return sk_main

