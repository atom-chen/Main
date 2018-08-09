
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--海神3技能，体力增加400%，每回合结束为自己增加10%的


local i_AddHP = i_mk{                    --增加体力
    ImpactLogic = 4,  
    Param_1 = 0,
    Param_2 = 0,
    Param_3 = "a1",


    Duration = -1,
    AliveCheckType = 1,
    MutexID  = 20420,
    MutexPriority = 1,
}


local  i_UseSkillRoundEnd  = i_mk{   --回合结束使用技能
    ImpactLogic = 24,                --定期触发使用技能

    Param_1 = 204300,            -- 技能id
    Param_2 = 3,                     -- 周期类型（见24号逻辑）
    Param_3 = -1,                    -- 条件类型（见24号逻辑）
    Param_4 = -1,                    -- 条件参数
    Param_5 = -1,                    -- 条件参数
    Param_6 = -1,                    -- 是否立即生效（回合开始时用的技能，必须时立即释放，这个参数无用，回合结束使用技能，必须时顺序释放，这个参数无用）
    Param_7 = -1,                    -- 技能类型（-1，skillExId；1，skillIndex）
    Param_8 = -1,                    -- 周期参数
    Param_9 = -1,                    -- 周期参数

    Duration = -1,
    AliveCheckType = 1,
    MutexID  = 20421,
    MutexPriority = 1,
}

local sk_main = sk_mk{

    H_1 = h_mk{                 
        TargetType = 2,
        I_1 = {
            Impact = i_AddHP,      --增加体力
        },
        I_2 = {
            Impact = i_UseSkillRoundEnd,       --回合结束后使用技能 
        },
    
    },
}

return sk_main 