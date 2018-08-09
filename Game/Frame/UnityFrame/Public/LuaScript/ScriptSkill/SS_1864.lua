
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--孟婆召唤壶技能-敌人行动回合开始释放技能

-- --加cd
-- local i_jiaCD = i_mk{
--     Duration = 0,
--     IsShow=1,
--     ImpactClass=4,
--     ImpactLogic =35,                         --增减CD，有CD的技能，才会受这个效果影响

--     Param_1 = -1,                            --技能索引（-1表示全部技能(1~3)）
--     Param_2 = 1,                         --增减值(大于0增加CD，小于0减少CD)


-- }


-- --攻击降低效果
-- local i_AttackReduce = i_mk(sc.CommonBuffs.AttackReduce)   --引用通用攻击降低
--       i_AttackReduce.Duration = 1                     --修改持续回合          



-- local sk_zhaohuan = sk_mk{
--     H_1 = h_mk{
--         IsChanceRefix = 1,
--         TargetType = 2,
--         I_1 ={
--             Impact =  i_AttackReduce,               --攻击降低
--         },
--         I_2 ={
--             Impact =  i_jiaCD,               --加CD
--         }
--     }

-- }
    

local  i_UseSkillRoundBegin = i_mk{  -- 敌人行动开始使用技能
    ImpactLogic = 24,                --定期触发使用技能

    Param_1 = "a1",          -- 技能id
    Param_2 =  5,                   -- 周期类型（见24号逻辑）
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