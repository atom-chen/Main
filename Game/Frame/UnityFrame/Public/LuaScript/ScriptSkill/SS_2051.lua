
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--黑狼2技能，击杀后获得回合

local i_GetRound  = i_mk{                       --获得回合      
    ImpactLogic = 15,
    Param_1 = 1,
    Param_2 = 7560,

    Duration = 0,
    AliveCheckType = 1,
    MutexID  = 2051,
    MutexPriority = 1,
}

local i_AddAtkOnKill = i_mk{                    --加20%攻击力
    ImpactLogic = 4,  
    Param_1 = 2,
    Param_2 = 0,
    Param_3 = 2000,


    Duration = -1,
    AliveCheckType = 1,
    LayerID  = 2051,
    LayerMax = 6,
}
local i_kuangbao = i_mk{                      --速度，暴击，攻击增加
    Id = 2051002,
    ImpactLogic = 4,
    Param_1 = 1,
    Param_2 = 15,
    Param_3 = 0,
    Param_4 = 5,
    Param_5 = 3000,
    Param_6 = 0,
    Param_7 = 2,
    Param_8 = 0,
    Param_9 = 5000,

    Duration = -1,
    MutexID  = 20512,
    MutexPriority = 1,
}
local  i_UseSkillRoundBegin  = i_mk{   --回合开始使用技能
    Id = 2051003,
    ImpactLogic = 24,                 --定期触发使用技能

    Param_1 = 205500,                 -- 技能id
    Param_2 = 1,                     -- 周期类型（见24号逻辑）
    Param_3 = -1,                    -- 条件类型（见24号逻辑）
    Param_4 = -1,                    -- 条件参数
    Param_5 = -1,                    -- 条件参数
    Param_6 = -1,                    -- 是否立即生效（回合开始时用的技能，必须时立即释放，这个参数无用，回合结束使用技能，必须时顺序释放，这个参数无用）
    Param_7 = -1,                    -- 技能类型（-1，skillExId；1，skillIndex）
    Param_8 = -1,                    -- 周期参数
    Param_9 = -1,                    -- 周期参数
    
    Duration = -1,
    MutexID  = 20513,
    MutexPriority = 1,
}
local i_HpUnder30 = i_mk{
    ImpactLogic = 28,                --血量满足特定条件时，触发子效果，不满足时，根据配置是否移除子效果，该效果被移除时，自动移除子效果
    
    Param_1 = 0,                     --血量操作符（0，小于，1大于，2小于（上buff立即触发），3大于（上buff立即触发））
    Param_2 = 3000,                  --血量百分比(10000)
    Param_3 = 1,                     --不满足时是否移除（0，否；1，是）
    Param_4 = i_UseSkillRoundBegin,  --子效果impact id 
    Param_5 = i_kuangbao,            --子效果impact id 
    Param_6 = -1,                    --子效果impact id     
    
    Duration = -1,
}

local sk_assist2 = sk_mk{                        --辅助技能，给自己加buff
    H_1 = h_mk{
        TargetType = 2,
        I_1 ={
            Impact =  i_GetRound,                --给自己加加攻击力的buff
        },
      
    },
}



local  i_UseSkillRoundEnd  = i_mk{   --回合结束使用技能
    ImpactLogic = 24,                --定期触发使用技能

    Param_1 = sk_assist2,            -- 技能id
    Param_2 = 3,                     -- 周期类型（见24号逻辑）
    Param_3 = -1,                    -- 条件类型（见24号逻辑）
    Param_4 = -1,                    -- 条件参数
    Param_5 = -1,                    -- 条件参数
    Param_6 = -1,                    -- 是否立即生效（回合开始时用的技能，必须时立即释放，这个参数无用，回合结束使用技能，必须时顺序释放，这个参数无用）
    Param_7 = -1,                    -- 技能类型（-1，skillExId；1，skillIndex）
    Param_8 = -1,                    -- 周期参数
    Param_9 = -1,                    -- 周期参数

    Duration = 1,
    AliveCheckType = 2
}


local sk_assist = sk_mk{                        --辅助技能，给自己加buff
    H_1 = h_mk{
        TargetType = 2,
        I_1 ={
            Impact =  i_UseSkillRoundEnd,               --给自己加获得回合的buff
        },
      
    },
}

local sk_assist1 = sk_mk{                        --辅助技能，给自己加buff
    H_1 = h_mk{
        TargetType = 2,
        I_1 ={
            Impact =  i_AddAtkOnKill,               --给自己加加攻击力的buff
        },
      
    },
}



local  i_UseSkillOnKill  = i_mk{     --击杀后使用技能
    ImpactLogic = 24,                --定期触发使用技能

    Param_1 = sk_assist,             -- 技能id
    Param_2 = 8,                     -- 周期类型（见24号逻辑）
    Param_3 = -1,                    -- 条件类型（见24号逻辑）
    Param_4 = -1,                    -- 条件参数
    Param_5 = -1,                    -- 条件参数
    Param_6 = -1,                    -- 是否立即生效（回合开始时用的技能，必须时立即释放，这个参数无用，回合结束使用技能，必须时顺序释放，这个参数无用）
    Param_7 = -1,                    -- 技能类型（-1，skillExId；1，skillIndex）
    Param_8 = -1,                    -- 周期参数
    Param_9 = -1,                    -- 周期参数

    Duration = -1
}

local  i_UseSkillOnDie = i_mk{  -- 敌人死亡后使用技能
    ImpactLogic = 24,                --定期触发使用技能

    Param_1 = sk_assist1,            -- 技能id
    Param_2 = 7,                    -- 周期类型（见24号逻辑）
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

    H_1 = h_mk{                 
        TargetType = 2,
        I_1 = {
            Impact = i_UseSkillOnKill,      --击杀后使用技能
        },
        I_2 = {
            Impact = i_UseSkillOnDie,  --敌人死亡后使用技能 
        },
        I_3 = {
            Impact = i_HpUnder30,        --血量低于30%进入狂暴状态
        }
    },
}

return sk_main 