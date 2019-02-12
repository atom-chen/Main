local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--【被动】生命低于50%时，自动触发石化
local i_DefenceEnhance = i_mk(sc.CommonBuffs.DefenceEnhance)   --引用通用防御强化buff
      i_DefenceEnhance.Duration = 2                            --修改持续回合 

local i_Immune = i_mk(sc.CommonBuffs.Immune)           --引用通用免疫
      i_Immune.Duration = 2                            --修改持续回合 


local i_hudun  = i_mk{
    Id = 1771010,
    EffectID = 573,
    ImpactLogic = 9,              -- 狮爷护盾
    Param_1 =1500,                -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    ImpactClass = 1,
    Duration = 2,
    IsShow =1,
    MutexID = 1771,
    MutexPriority = 1,
}

local i_renjie  = i_mk{         --
    ImpactLogic = 28,
    Duration = -1,
    IsShow = 1,
    MutexID  = 1773,
    MutexPriority = 1,
    Param_1 =0,
    Param_2 =5000,
    Param_3 =0,
    Param_4 = i_hudun,
    Param_5 = i_Immune,
    Param_6 = i_DefenceEnhance,
    IsPassiveImpact = 1, 
}

local i_yaojie  = i_mk{         --
    ImpactLogic = 28,
    Duration = -1,
    IsShow = 1,
    MutexID  = 1773,
    MutexPriority = 1,
    Param_1 =0,
    Param_2 =5000,
    Param_3 =0,
    Param_4 = i_hudun,
    Param_5 = i_DefenceEnhance,
    IsPassiveImpact = 1, 
}



local sk_assist = sk_mk{
    H_1 = h_mk{
        IsAnimHit = 0,
        TargetType = 2,
        I_1 ={
            Impact =  i_renjie,               --人界
        },
    },
    EnvLimit = 0,
    OtherID = sk_mk{
        H_1 = h_mk{
            TargetType = 2,
            IsAnimHit = 0,
            I_1 ={
                Impact =  i_yaojie,          --妖界
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

local sk_main = sk_mk{

    H_1 = h_mk{                 --切换环境使用技能，回合开始使用技能
        TargetType = 2,
        I_1 = {
            Impact = i_UseSkillEnvChange,
        },
        I_2 = {
            Impact = i_UseSkillRoundBegin,
        }
    },
    IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放
}

return sk_main