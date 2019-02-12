
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--【被动】鬼兵妖界增加20%伤害

local i_nothing  = i_mk{     --空imapct

    ImpactLogic = -1,
    Duration = -1,
    MutexID  = 1352,
    MutexPriority = 1,
}

local i_attackup = i_mk{

    IsPassiveImpact=1,

    ImpactLogic = 4,               
    MutexID=1352,
    MutexPriority=1,
    Param_1 = 6,                    
    Param_2 = 2000,                    
    Param_3 = 0,
    Duration = -1,
    AliveCheckType=1,
    --AutoFadeOutTag=8,
}


local i_kongimpact = i_mk{
    Id=1352010,
    Duration = 1,
    AliveCheckType=3,
    MutexID=13521,
    MutexPriority=1,

    ImpactLogic = -1,      

}


local sk_assist1 = sk_mk{
    H_1 = h_mk{
        TargetType = 2,
  
        I_1 = {
            Impact = i_kongimpact,
        }
    },
   
}



local i_texiao = i_mk{
    Duration = -1,

    ImpactLogic = 45,      

    Param_1 = 8192,                    
    Param_2 = -1,                    
    Param_3 = sk_assist1,
    Param_4 = -1,
    Param_5 = 10000,
    Param_6 = 1,
}

-- local i_jiance = i_mk{
--     Duration = 0,

--     ImpactLogic = 29,      

--     Param_1 = 0,                    
--     Param_2 = 917001,                    
--     Param_3 = 1,
--     Param_4 = i_kongimpact,
 
-- }

-- local i_kongimpact11 = i_mk{
--     Duration = 1,
--     AliveCheckType=2,
--     MutexID=13521,
--     MutexPriority=2,

--     ImpactLogic = -1,      

-- }





local sk_assist2 = sk_mk{
    H_1 = h_mk{
        TargetType = 2,
        I_1 = {
            Impact = i_kongimpact11 ,
        },
        I_2 = {
            Impact = i_jiance,
        }
    },
   
}




local sk_assist = sk_mk{
    H_1 = h_mk{
        TargetType = 2,
        I_1 ={
            Impact =  i_nothing,              
        },
   
    },
    EnvLimit = 0,
    OtherID = sk_mk{
        H_1 = h_mk{
            TargetType = 2,
            I_1 ={
                Impact =  i_attackup,          
            },
           
        },
        EnvLimit = 1,   
    }
   
}

-- local  i_UseSkillRoundOver = i_mk{       -- 回合结束后释放技能
--     Duration= -1,
--     IsShow=1,
--     AliveCheckType=2,
--     MutexID=13522,
--     MutexPriority=1,

--     ImpactLogic = 24,                --定期触发使用技能

--     Param_1 = sk_assist2,            -- 技能id
--     Param_2 = 3,                    -- 周期类型（见24号逻辑）
--     Param_3 = -1,                   -- 条件类型（见24号逻辑）
--     Param_4 = -1,                   -- 条件参数
--     Param_5 = -1,                   -- 条件参数
--     Param_6 = -1,                   -- 是否立即生效（回合开始时用的技能，必须时立即释放，这个参数无用，回合结束使用技能，必须时顺序释放，这个参数无用）
--     Param_7 = -1,                   -- 技能类型（-1，skillExId；1，skillIndex）
--     Param_8 = -1,                   -- 周期参数
--     Param_9 = -1,                   -- 周期参数

-- }

    
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


local  i_zijihuihe = i_mk{  -- 波次开始使用技能
    
    ImpactLogic = 24,                --定期触发使用技能

    Param_1 = sk_assist,               -- 技能id
    Param_2 = 1,                    -- 周期类型（见24号逻辑）
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
        -- I_3 = {
        --     Impact = i_UseSkillRoundOver,
        -- },
        I_3 = {
            Impact = i_texiao ,
        },
        I_4 = {
            Impact = i_zijihuihe,
        },
    },
    
}

return sk_main