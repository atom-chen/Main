local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--【被动】穷奇免疫无法行动的减益效果（眩晕，冰冻，睡眠）

local i_nothing  = i_mk{         --空imapct
    ImpactLogic = -1,
    Duration = -1,
    MutexID  = 1061,
    MutexPriority = 1,
}

local i_immune  = i_mk{
    ImpactLogic = 7 ,     --免疫,抵消效果

    Param_1 = 4,           --ImpactClass（0时不考虑，否则必须完全满足）
    Param_2 = 769,         --ImpactSubClass（0时不考虑，否则和ImpactClass同时匹配时，抵消，满足任意一个SubClass即可）
    Param_3 = -1,          --次数（-1时无限次数）

    Duration = -1,
    MutexID  = 1061,
    MutexPriority = 1,
    IsPassiveImpact = 1, 
}


local i_xixuehudun  = i_mk{

    Duration= -1,
    AliveCheckType=1,
    IsPassiveImpact = 1, 
    MutexID=1061,
    MutexPriority=1,
    ImpactLogic =64,              --吸血溢出转化为护盾
        Param_1 =5000,            --护盾值不超过最大生命百分比
        Param_2 =i_mk{
            Id=1062010,
            Duration= -1,
            IsShow=1,
            AliveCheckType=1,
            ImpactClass=1,
            ImpactSubClass=2048,
            MutexID=1062,
            MutexPriority=1,

            ImpactLogic =9,              
                Param_1 =0, 
                Param_2 =1, 

            }        
}



local sk_assist = sk_mk{
    H_1 = h_mk{
        TargetType = 2,
        I_1 ={
            Impact =  i_nothing,               --人界无效果
        },
     
    },
    EnvLimit = 0,
    OtherID = sk_mk{
        H_1 = h_mk{
            TargetType = 2,
            I_1 ={
                Impact =  i_immune,          --妖界免疫buff
            },
        
        },
        EnvLimit = 1,  
        IgnoreSelfState = 1 
    },
    IgnoreSelfState = 1
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
        },
        I_3 = {
            Impact = i_xixuehudun
        },
    },
    IgnoreSelfState = 1
}

return sk_main