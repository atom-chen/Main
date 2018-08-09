
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--黄泉2技能觉醒辅助，受到伤害时使用技能反击
i_fanji = i_mk{
    ImpactLogic = 17,               
    Param_1 = 3000,                    
    Param_2 = "a1",                    
    Param_3 = 1,
    Param_4 = 0,
    Param_5 = 1,

    Duration = -1,
    MutexID  = 1873,
    MutexPriority = 1,
    RoundMaxEffectedCount =1,
} 



local sk_main = sk_mk{

    H_1 = h_mk{                 
        TargetType = 2,
        I_1 = {
            Impact = i_fanji,   --切换环境使用技能
        },
    },
}

return sk_main