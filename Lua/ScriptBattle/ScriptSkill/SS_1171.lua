
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--毕方2技能-回合开始，毕方会携带火焰护甲，护甲可抵挡自身最大生命15%的伤害，并以20%的概率使攻击者受到持续2回合的灼伤效果。

-- --灼烧
-- local i_zhuoshao = i_mk{
--     Id=1177010,

--     Duration =2,
--     AliveCheckType=3,
--     IsShow=1,
--     MutexID = 911,
--     MutexPriority =1,
--     ImpactClass=4,
--     ImpactSubClass=128,

--     ImpactLogic =4,                        
--     Param_1 = 7,                            
--     Param_1 = -2500,                      
--     Param_1 = 0,                             
                   
-- }


-- local sk_zhuoshao = sk_mk{
--     H_1 = h_mk{
--         TargetType = 1,
--         I_1 ={
--             Impact =  i_zhuoshao,               --释放灼烧
--         }
    
--     }
-- }




-- --受击释放技能
-- local i_shouji = i_mk{
  
--     Duration = -1,
--     MutexID = 1172,
--     MutexPriority =1,
    

--     ImpactLogic = 17,                        --护盾（受到伤害时，优先扣护盾的数值，扣完后，buff移除）
--     Param_1 = 10000,--"a1",                            --概率
--     Param_1 = sk_zhuoshao,                      --技能id
--     Param_1 =1,                             --1自己，2队友，3任意我方
--     Param_1 =1,                             --立即使用（0等正在行动的人结束后使用，1立即使用）（一般有动画表现的，最好0，没动画表现的1）
--     Param_1 =1,                             -- 技能目标（1攻击者，2自己）(最好是hit里直接使用特殊目标，技能目标只是辅助用)
-- }

-- --护盾
-- local i_hudun = i_mk{
--     Id=1176010,
--     Duration = -1,
--     MutexID = 1171,
--     MutexPriority =1,
--     ImpactClass = 1,
--     ImpactSubClass = 2048, 
--     ChildImpact= i_shouji,

--     ImpactLogic = 9,                        --护盾（受到伤害时，优先扣护盾的数值，扣完后，buff移除）
--     Param_1 = 1500,                            --被加护盾符灵的血量上限百分比



-- }


-- local sk_hudun = sk_mk{
--     H_1 = h_mk{
--         TargetType = 2,
--         I_1 ={
--             Impact =  i_hudun,               --护盾
--         }
    
--     }

-- }
    

local  i_UseSkillRoundBegin = i_mk{  -- 回合开始使用技能
    IsPassiveImpact=1,
    ImpactLogic = 24,                --定期触发使用技能

    Param_1 = "a1",                  -- 技能id
    Param_2 =  1,                    -- 周期类型（见24号逻辑）
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
    IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放
}

return sk_main