
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--【被动】每回合结束时，干戚为自己生成一个相当于自身生命最大值10%的护盾。

--护盾
local i_hudun = i_mk{
    Id= 1401010,

    Duration = 1,
    IsShow = 1,
    ImpactClass = 1,
    ImpactSubClass = 2048, 
    
    ImpactLogic = 9,                        --护盾（受到伤害时，优先扣护盾的数值，扣完后，buff移除）
    Param_1 = "a1",                         --被加护盾符灵的血量上限百分比

}


local sk_hudun = sk_mk{
    H_1 = h_mk{
        TargetType = 2,
        I_1 ={
            Impact = i_hudun,               --释放护盾
        },   
    },
    IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放
}


local  i_UseSkillRoundOver = i_mk{       -- 回合结束后释放技能
    Duration= -1,
    IsShow= 1,
    AliveCheckType=2,
    MutexID=120,
    MutexPriority=1,
    IsPassiveImpact=1,

    ImpactLogic = 24,                --定期触发使用技能

    Param_1 = sk_hudun,             -- 技能id
    Param_2 = 3,                    -- 周期类型（见24号逻辑）
    Param_3 = -1,                   -- 条件类型（见24号逻辑）
    Param_4 = -1,                   -- 条件参数
    Param_5 = -1,                   -- 条件参数
    Param_6 = -1,                   -- 是否立即生效（回合开始时用的技能，必须时立即释放，这个参数无用，回合结束使用技能，必须时顺序释放，这个参数无用）
    Param_7 = -1,                   -- 技能类型（-1，skillExId；1，skillIndex）
    Param_8 = -1,                   -- 周期参数
    Param_9 = -1,                   -- 周期参数

}

--回合开始检查护盾
-- local i_jiachahudun = i_mk{
--     Duration= -1,
--     IsPassiveImpact=1,

--     ImpactLogic =46,                --回合开始时，若指定impact存在，则触发技能，技能目标自己                   

--     Param_1= 1484010,               --impact的id
--     Param_2= sk_tisheng,            --技能id
--     Param_3=  1,                    --立即释放(1,立即；其他顺序)
-- }


local sk_main = sk_mk{

    H_1 = h_mk{                 
        TargetType = 2,
        -- I_1 = {
        --     Impact = i_jiachahudun,
        -- },
        I_1 = {
            Impact = i_UseSkillRoundOver,
        },
    },
    IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放
}

return sk_main