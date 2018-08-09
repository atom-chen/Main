local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


--玉圭2技能 回血+驱散
local i_Core = i_mk{
    ImpactLogic = 1,               --治疗
    
    Param_1 = 3,
    Param_2 = "a1",
}


local i_qusan= i_mk{
    ImpactLogic = 5,                --驱散debuff
    
    Param_1 = 4,                    --被驱散的impact class
    Param_2 = -1,                   --subclasss
    Param_3 = -1,                   --tag
    Param_4 = 2,                    --驱散数量
    Param_5 = 1                     --是否提示
}   


local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType = 9 ,
        I_1 = {                         --治疗
            Impact = i_Core,
        },
 
        I_2 = {                        
            Impact =i_qusan,            --驱散
        }
    
    },
    
}

return sk_main