require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--瓶灵2技能
--【被动】瓶灵的攻击不会唤醒陷入沉睡效果的敌人，并且自身免疫沉睡效果。[A78C84]【妖界效果】当蜃珧攻击时，瓶灵会以25%的概率使用普通攻击协同攻击。[-]

--攻击不会唤醒睡眠
local i_gongjisleep  = i_mk{         
    Duration = -1,
    MutexID  = 1150,
    MutexPriority = 1,

    ImpactLogic = 4,  
    Param_1 = 104,             
    Param_2 = 1, 
    Param_3 = 0,                   
}

--免疫睡眠
local i_mianyisleep  = i_mk{         
    Duration = -1,
    MutexID  = 1152,
    MutexPriority = 1,
    IsPassiveImpact=1,
    
    ImpactLogic = 7,                   --免疫、抵消效果
    Param_1 = 0,                     --ImpactClass（0时不考虑，否则必须完全满足）
    Param_2 = 512,                       --ImpactSubClass（0时不考虑，否则和ImpactClass同时匹配时，抵消，满足任意一个SubClass即可）
    Param_3 = -1,                       --次数（-1时无限次数）
}

--协战
local i_xiezhan  = i_mk{
    Duration= -1,
    AliveCheckType=2,

    MutexID=1151,
    MutexPriority=1,
    IsPassiveImpact=1,
    
    ImpactLogic = 19,               --特定使用技能时，协同一起使用技能

    Param_1 = 19,                   -- cardId（特定符灵，-1时，任意队友攻击都可协同攻击,-2表示发送者使用技能时，协同攻击）
    Param_2 = 5,                    -- skillClass（技能类型）
    Param_3 = "a1",                 -- 概率
    Param_4 = 1,                    --额外使用的技能索引（该角色的第几个技能）
    Param_5 = -1,                   --额外使用的技能额外使用的技能（skillEx表id，参数4必须填-1才生效）
    Param_6 = 1,                    --一回合内激活的次数
}
--空
local i_kong  = i_mk{
    Duration= -1,
    AliveCheckType=2,

    MutexID=1151,
    MutexPriority=1,

    ImpactLogic = -1,               --空
}

local sk_assist = sk_mk{
    H_1 = h_mk{
        TargetType = 2,
        I_1 ={
            Impact =  i_gongjisleep,            
        },
        I_2 ={
            Impact =   i_mianyisleep,            
        },
        I_3 ={
            Impact =   i_kong,            
        },
      
    },
    EnvLimit = 0,
    OtherID = sk_mk{
        H_1 = h_mk{
            TargetType = 2,
            I_1 ={
                Impact =  i_gongjisleep,            
            },
            I_2 ={
                Impact =   i_mianyisleep,            
            },
            I_3 ={
                Impact =    i_xiezhan,            
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

local  i_zijihuihejieshu = i_mk{  -- 回合开始使用技能
  
    ImpactLogic = 24,                --定期触发使用技能

    Param_1 = sk_assist,                  -- 技能id
    Param_2 =  4,                    -- 周期类型（见24号逻辑）
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
            Impact = i_UseSkillEnvChange,   --切换环境使用技能
        },
        I_2 = {
            Impact = i_UseSkillRoundBegin,  --回合开始使用技能 
        },
        I_3 = {
            Impact = i_zijihuihejieshu,  --回合开始使用技能 
        },
    
    }
}

return sk_main