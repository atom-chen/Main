
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--仓颉3：给队友一个2回合的神佑效果（神佑：持续时间内，受到致死伤害时立即复活）

-- local i_fuhuo  = i_mk{
--     Duration = 2,
--     ImpactClass= 1,
 
--     ImpactLogic = 13,              --死亡后复活释放技能

--     Param_1 = 99,                  -- 激活次数
--     Param_2 = 3000,                 -- 回复血量
--     Param_3 = -1                    --释放技能

-- }

--通用神佑
local i_Guard = i_mk(sc.CommonBuffs.Guard)   
      i_Guard.Duration = 2   


local sk_main = sk_mk{

    H_1 = h_mk{                        
        TargetType = 1,
        I_1 = {
            Impact = i_Guard,
        },
    }

   
}

return sk_main