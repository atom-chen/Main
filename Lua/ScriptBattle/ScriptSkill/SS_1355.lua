
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")



--狂暴效果
local i_CriticalEnhance = i_mk(sc.CommonBuffs.CriticalEnhance)     --引用狂暴效果
      i_CriticalEnhance.Duration = 2                               --修改持续回合          


local i_nextattack1 =i_mk{
    Duration=1,
    AliveCheckType=2,
    IsShow=0,
    MutexID=1351,
    MutexPriority=1,

    ImpactLogic = 14,        --使用技能时，条件额外使用技能

    Param_1 = 4,             --skilltype，1，普攻，2，任意主动，3指定class，4指定id
    Param_2 = "a1",          --技能参数
    Param_3 = 10000,         --概率
    Param_4 = -1,            --额外使用的技能索引（该角色的第几个技能）
    Param_5 = "a2",          --额外使用的技能（skillEx表id，参数4必须填-1才生效）
    Param_6 = 2,             --条件检查时机（1，使用技能前，2，使用技能后）
    Param_7 = 0,             --不能切换目标（0，否（目标死亡后会随机选择目标），1是（目标死亡后追加不能触发））     
    Param_8 = -1,            --条件类型（见17号逻辑）
    Param_9 = -1,            --条件参数1
    Param_10 = -1,           --条件参数2

}

local i_nextattack2 =i_mk{
    Duration=1,
    AliveCheckType=2,
    IsShow=0,
    MutexID=1351,
    MutexPriority=1,

    ImpactLogic = 14,        --使用技能时，条件额外使用技能

    Param_1 = 4,             --skilltype，1，普攻，2，任意主动，3指定class，4指定id
    Param_2 = "a3",          --技能参数
    Param_3 = 10000,         --概率
    Param_4 = -1,            --额外使用的技能索引（该角色的第几个技能）
    Param_5 = "a4",          --额外使用的技能（skillEx表id，参数4必须填-1才生效）
    Param_6 = 2,             --条件检查时机（1，使用技能前，2，使用技能后）
    Param_7 = 0,             --不能切换目标（0，否（目标死亡后会随机选择目标），1是（目标死亡后追加不能触发））     
    Param_8 = -1,            --条件类型（见17号逻辑）
    Param_9 = -1,            --条件参数1
    Param_10 = -1,           --条件参数2

}
local sk_nextskill = sk_mk{
    H_1 = h_mk{
        TargetType = 2,
        I_1 ={
            Impact =  i_nextattack1,              
        },
      
    },
    EnvLimit = 0,
    OtherID = sk_mk{
        H_1 = h_mk{
            TargetType = 2,
            I_1 ={
                Impact =  i_nextattack2,          
            },
        
        },
        EnvLimit = 1,   
    }
   
}




--检查暴击buff,若有则触发技能：使用技能释放技能
local i_jiachabaoji = i_mk{
    Duration= -1,
    MutexID=1355,
    MutexPriority=1,
    ImpactLogic =46,                --回合开始时，若指定impact存在，则触发技能，技能目标自己                   

    Param_1= 917001,                --impact的id
    Param_2= sk_nextskill,          --技能id
    Param_3=  1,                    --立即释放(1,立即；其他顺序)
}


local sk_assist = sk_mk{

    H_1 = h_mk{        
        IsAnimHit=0,                 
        TargetType = 2,
        I_1 = {
            Impact = i_jiachabaoji,
        }
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
}


return sk_main




