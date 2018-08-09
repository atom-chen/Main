local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")



--攻击强化+狂暴+持续伤害

local i_AttackEnhance = i_mk(sc.CommonBuffs.AttackEnhance)   --引用通用攻击强化
      i_AttackEnhance.Duration = 2                      --修改持续回合       


local i_CriticalEnhance = i_mk(sc.CommonBuffs.CriticalEnhance)   --引用狂暴
      i_CriticalEnhance.Duration = 2                      --修改持续回合       



-- local i_Poison = i_mk{                                      --持续伤害
--     Id = 904002,
    
    
--     Duration=2,   
--     AliveCheckType=3,


-- }




local sk_main = sk_mk{
    
    H_1= h_mk{                      
       TargetType=3,
       I_1={                                --攻击强化
        Impact =i_AttackEnhance,
       },

       I_2={
           Impact =i_CriticalEnhance,           --狂暴
           },

    --    I_3={
    --        Impact =i_Poison,
               
    --        }
       }
   }  


return sk_main