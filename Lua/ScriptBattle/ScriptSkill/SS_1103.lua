
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--白泽觉醒技能：队友每次攻击均有概率释放技能1+白泽2技能-回合开始，触发伤害免疫1次的效果。

--协同
local i_xietong  = i_mk{

    IsPassiveImpact=1,
    Duration= -1,
    AliveCheckType=1,
    MutexID=1103,
    MutexPriority=1,
    
    ImpactLogic = 19,              --特定使用技能时，协同一起使用技能

    Param_1 = -1,                  -- cardId（特定符灵，-1时，任意队友攻击都可协同攻击,-2表示发送者使用技能时，协同攻击）
    Param_2 = 5,                   -- skillClass（技能类型）
    Param_3 = "a1",              -- 概率
    Param_4 =  1,                  --额外使用的技能索引（该角色的第几个技能）
    Param_5 = -1,                  --额外使用的技能额外使用的技能（skillEx表id，参数4必须填-1才生效）
    Param_6 = 1,                   --一回合内激活的次数
}




--开局释放协同
local sk_xietong= sk_mk{
    H_1 = h_mk{
        TargetType = 2,
        I_1 ={
            Impact =  i_xietong,               --免疫伤害1回合
        }
    
    }

}

local i_mianyishanghai = i_mk{
    IsPassiveImpact=1,
    ImpactLogic = 8,                        --无视伤害
    Param_1 = 1,                            --次数（-1表示无限）

    Duration = -1,
    AliveCheckType = 3,
    AutoFadeOutTag = 0,
    ImpactClass = 0,
    ImpactSubClass = 0, 
    MutexID = 1104,
    MutexPriority =1,

}


local sk_mianyishanghai = sk_mk{
    H_1 = h_mk{
        TargetType = 2,
        I_1 ={
            Impact =  i_mianyishanghai,               --免疫伤害1回合
        }
    
    },
    IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放

}


local  i_UseSkillkaiju = i_mk{      -- 开局开始使用技能

  

    ImpactLogic = 24,                --定期触发使用技能

    Param_1 = sk_xietong,           -- 技能id
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



local  i_UseSkillRoundBegin = i_mk{  -- 回合开始使用技能

    ImpactLogic = 24,                --定期触发使用技能

    Param_1 = sk_mianyishanghai,    -- 技能id
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

    H_1 = h_mk{                 --回合开始使用技能
        TargetType = 2,
    
        I_1 = {
            Impact = i_UseSkillkaiju,
        
        },
        I_2 = {
            Impact = i_UseSkillRoundBegin,
        },
    },
    IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放
}

return sk_main

