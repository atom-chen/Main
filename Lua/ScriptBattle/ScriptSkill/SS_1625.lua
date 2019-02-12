local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


--牛魔王 2技能觉醒辅助

local  i_Core  = i_mk{   --回合结束使用技能
    IsPassiveImpact=1,
    
    ImpactLogic = 1,        --治疗

    Param_1 = 3,            -- 治疗类型
    Param_2 = 3000,         -- 周期类型（见24号逻辑）
}


local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType = 2 ,
        I_1 = {                         --回合结束使用技能
            Impact = i_Core,
        },
    },
    IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放
    
}

return sk_main