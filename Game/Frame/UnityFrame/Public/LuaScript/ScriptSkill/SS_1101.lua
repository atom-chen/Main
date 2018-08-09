
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--白泽2技能-回合开始，触发伤害免疫1次的效果。


--免疫伤害效果1次
-- local i_mianyishanghai = i_mk{
--     ImpactLogic = 7,                        --免疫、抵消效果
--     Param_1 = 2,                            --ImpactClass（0时不考虑，否则必须完全满足）
--     Param_2 = 0,                            --ImpactSubClass（0时不考虑，否则和ImpactClass同时匹配时，抵消，满足任意一个SubClass即可）
--     Param_3 = 1,                            --次数
--     Duration = -1,
--     AliveCheckType = 3,
--     AutoFadeOutTag = 0,
--     ImpactClass = 1,
--     ImpactSubClass = 0, 
--     MutexID = 1101,
--     MutexPriority =1,

-- }


local i_mianyishanghai = i_mk{
    ImpactLogic = 8,                        --无视伤害
    Param_1 = 1,                            --次数（-1表示无限）

    Duration = -1,
    AliveCheckType = 3,
    AutoFadeOutTag = 0,
    ImpactClass = 1,
    ImpactSubClass = 0, 
    MutexID = 1101,
    MutexPriority =1,

}



local sk_mianyishanghai = sk_mk{
    H_1 = h_mk{
        TargetType = 2,
        I_1 ={
            Impact =  i_mianyishanghai,               --免疫伤害1回合
        }
    
    }

}
    







local  i_UseSkillRoundBegin = i_mk{  -- 波次开始使用技能
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
            Impact = i_UseSkillRoundBegin,
        
        }
    },
}

return sk_main