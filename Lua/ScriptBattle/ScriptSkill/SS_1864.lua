
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--孟婆召唤壶技能-敌人行动回合开始释放技能，每次耗血不超过10%


-- local i_haoxue = i_mk{
--     Duration= -1,
--     AliveCheckType= 1,

--     ImpactLogic = 53,              

--     Param_1 = 0,                  
--     Param_2 = 1000,     
--   }

-- local i_jinliao = i_mk{
   

--     ImpactLogic = 4,
--     Param_1 = 103,
--     Param_2 = 1,
--     Param_3 = 0,
    
--     Duration = -1,
    
--     AutoFadeOutTag = 0,

--     MutexID = 907,
--     MutexPriority =1,  
--   }


--免疫任何buff以及debuff

local i_mianyidebuff = i_mk{
   

    ImpactLogic = 7,
    Param_1 = 4,
    Param_2 = 0,
    Param_3 = -1,
    
    Duration = -1,
    AliveCheckType = 3,
    AutoFadeOutTag = 0,

    MutexID = 916,
    MutexPriority =2,
    
  }

local i_mianyibuff = i_mk{
   
    ImpactLogic = 7,
    Param_1 = 1,
    Param_2 = 0,
    Param_3 = -1,

    Duration = -1,
    AliveCheckType = 2,
    AutoFadeOutTag = 0,
   
    MutexID = 909,
    MutexPriority =2,
    
  }



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
    Param_10 = 1, 
    Duration = -1
}


local sk_main = sk_mk{

    H_1 = h_mk{                 --回合开始使用技能
        TargetType = 2,
    
        I_1 = {
            Impact = i_UseSkillRoundBegin,
        
        },
        I_2 = {
            Impact = i_mianyidebuff,
        
        },
        I_3 = {
            Impact = i_mianyibuff,
        
        },
    },
  

}

return sk_main