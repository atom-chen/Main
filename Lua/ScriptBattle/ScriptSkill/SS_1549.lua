
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--白骨2技能辅助，标记3技能等级
local sk_assist = sk_mk{

   
    H_1 = h_mk{                         --标记
        TargetType = 2,
        I_1 = {
            Impact = i_mk{
                Id = "a1" ,
                Duration=-1,
                MutexID=1549,
                MutexPriority=1, 
            }
        },
    },

}
local  i_UseSkillRoundBegin = i_mk{  -- 回合开始使用技能
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

   
        H_1 = h_mk{                         --标记
            TargetType = 2,
            I_1 = {
                Impact = i_UseSkillRoundBegin     
            },
        },

}

return sk_main