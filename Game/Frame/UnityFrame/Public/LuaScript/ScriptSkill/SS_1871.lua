
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--黄泉2技能，被黄泉杀死的敌人无法复活【妖界】黄泉受到的伤害降低30%

local i_nothing  = i_mk{         --空imapct
    ImpactLogic = -1,

    Duration = -1,
    MutexID  = 1871,
    MutexPriority = 1,
}

local i_reducedam  = i_mk{
    ImpactLogic = 4,               
    Param_1 = 7,                    
    Param_2 = "a1",                    
    Param_3 = 0,

    Duration = -1,
    MutexID  = 1871,
    MutexPriority = 1,
}

local sk_assist = sk_mk{
    H_1 = h_mk{
        TargetType = 2,
        I_1 ={
            Impact =  i_nothing,               --人界替换掉减伤buff
        },
      
    },
    EnvLimit = 0,
    OtherID = sk_mk{
        H_1 = h_mk{
            TargetType = 2,
            I_1 ={
                Impact =  i_reducedam,          --妖界受伤降低
            },
          
        },
        EnvLimit = 1,   
    }
   
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


local i_addbuffwhenkill  = i_mk{         --击杀后给击杀对象加buff
    ImpactLogic = 61,                    --击杀敌人后，触发标记buff 
    Param_1 = 1871010,
    Duration = -1,
}



local sk_main = sk_mk{

    H_1 = h_mk{                 
        TargetType = 2,
        I_1 = {
            Impact = i_UseSkillEnvChange,   --切换环境使用技能
        },
        I_2 = {
            Impact = i_UseSkillRoundBegin,  --回合开始使用技能 
        },
        I_3 = {
            Impact = i_addbuffwhenkill,     --击杀后对击杀对象使用技能
        }
    },
}

return sk_main