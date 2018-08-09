local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


--牛魔王 2技能觉醒

local  i_UseSkillRoundEnd  = i_mk{   --回合结束使用技能
    ImpactLogic = 24,                --定期触发使用技能

    Param_1 = 162501,            -- 技能id
    Param_2 = 12,                     -- 周期类型（见24号逻辑）
    Param_3 = -1,                    -- 条件类型（见24号逻辑）
    Param_4 = -1,                    -- 条件参数
    Param_5 = -1,                    -- 条件参数
    Param_6 = -1,                    -- 是否立即生效（回合开始时用的技能，必须时立即释放，这个参数无用，回合结束使用技能，必须时顺序释放，这个参数无用）
    Param_7 = -1,                    -- 技能类型（-1，skillExId；1，skillIndex）
    Param_8 = -1,                    -- 周期参数
    Param_9 = -1,                    -- 周期参数

    Duration = -1,
}


local sk_main = sk_mk{

    H_1 = h_mk{

        TargetType = 2 ,
        I_1 = {                         --回合结束使用技能
            Impact = i_UseSkillRoundEnd,
        },
    },
    
}

return sk_main