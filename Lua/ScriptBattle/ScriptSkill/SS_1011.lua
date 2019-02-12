local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--【被动】【妖界效果】笔神昌化生命低于50%时，伤害减免增加20%。

local i_nothing  = i_mk{         --空imapct
    ImpactLogic = -1,
    Duration = -1,
    MutexID  = 1011,
    MutexPriority = 1,
}

local i_defense  = i_mk{         --免伤
    ImpactLogic = 4,
    Param_1 = 7,
    Param_2 ="a1",
    Param_3 =0,
    Duration = -1,
    MutexID  = 1012,
    IsShow = 0,
    MutexPriority = 1,
    IsPassiveImpact = 1,
}

local i_lowhpunhurt1  = i_mk{
    ImpactLogic = 28 ,     --低血额外效果

    Param_1 = 0,           
    Param_2 = 5000,         
    Param_3 = 1,  
    Param_4 = i_defense,       

    Duration = -1,
    MutexID  = 1011,
    MutexPriority = 1,
    IsPassiveImpact = 1,
}

local i_lowhpunhurt2  = i_mk{
    ImpactLogic = 28 ,     --低血额外效果,立即触发

    Param_1 = 2,           
    Param_2 = 5000,         
    Param_3 = 1,  
    Param_4 = i_defense,       

    Duration = -1,
    MutexID  = 1011,
    MutexPriority = 1,
    IsPassiveImpact = 1,
}


local sk_assist = sk_mk{
    H_1 = h_mk{
        TargetType = 2,
        IsAnimHit = 0,
        I_1 ={
            Impact =  i_nothing,               --人界无效果
        },
    },
    EnvLimit = 0,

    OtherID = sk_mk{
        H_1 = h_mk{
            TargetType = 2,
            IsAnimHit = 0,
            I_1 ={
                Impact =  i_lowhpunhurt1,          --妖界生命低于50%时，伤害减免增加20%。
            },
            I_2 ={
                Impact =  i_lowhpunhurt2,          --妖界生命低于50%时，伤害减免增加20%。
            },
        },
        EnvLimit = 1,   
        IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放
    },
    IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放
}

local  i_UseSkillEnvChange  = i_mk{  -- 切换环境使用技能
    ImpactLogic = 24,                --定期触发使用技能

    Param_1 = sk_assist,             -- 技能id
    Param_2 = 12,                    -- 周期类型（见24号逻辑）
    Param_3 = -1,                    -- 条件类型（见24号逻辑）
    Param_4 = -1,                    -- 条件参数
    Param_5 = -1,                    -- 条件参数
    Param_6 = -1,                    -- 是否立即生效（回合开始时用的技能，必须时立即释放，这个参数无用，回合结束使用技能，必须时顺序释放，这个参数无用）
    Param_7 = -1,                    -- 技能类型（-1，skillExId；1，skillIndex）
    Param_8 = -1,                    -- 周期参数
    Param_9 = -1,                    -- 周期参数

    Duration = -1
}

local  i_UseSkillRoundBegin = i_mk{  -- 波次开始使用技能
    ImpactLogic = 24,                --定期触发使用技能

    Param_1 = sk_assist,               -- 技能id
    Param_2 = 2,                    -- 周期类型（见24号逻辑）
    Param_3 = -1,                   -- 条件类型（见24号逻辑）
    Param_4 = -1,                   -- 条件参数
    Param_5 = -1,                   -- 条件参数
    Param_6 = -1,                   -- 是否立即生效（回合开始时用的技能，必须时立即释放，这个参数无用，回合结束使用技能，必须时顺序释放，这个参数无用）
    Param_7 = -1,                   -- 技能类型（-1，skillExId；1，skillIndex）
    Param_8 = -1,                   -- 周期参数
    Param_9 = -1,                   -- 周期参数

    Duration = -1
    
}

local  i_UseSkillEnemyRoundBegin = i_mk{  -- 任意敌人回合开始
    ImpactLogic = 24,                --定期触发使用技能

    Param_1 = sk_assist,               -- 技能id
    Param_2 = 5,                    -- 周期类型（见24号逻辑）
    Param_3 = -1,                   -- 条件类型（见24号逻辑）
    Param_4 = -1,                   -- 条件参数
    Param_5 = -1,                   -- 条件参数
    Param_6 = -1,                   -- 是否立即生效（回合开始时用的技能，必须时立即释放，这个参数无用，回合结束使用技能，必须时顺序释放，这个参数无用）
    Param_7 = -1,                   -- 技能类型（-1，skillExId；1，skillIndex）
    Param_8 = -1,                   -- 周期参数
    Param_9 = -1,                   -- 周期参数

    Duration = -1
    
}









local sk_main = sk_mk{

    H_1 = h_mk{                 --切换环境使用技能，回合开始使用技能
        TargetType = 2,
        I_1 = {
            Impact = i_UseSkillEnvChange,
        },
        I_2 = {
            Impact = i_UseSkillRoundBegin,
        },
        I_3 = {
            Impact = i_UseSkillEnemyRoundBegin,
        }
    },
    IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放
}

return sk_main