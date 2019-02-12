local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")



--防御强化+强韧效果

local i_DefenceEnhance = i_mk(sc.CommonBuffs.DefenceEnhance)   --引用通用防御强化
      i_DefenceEnhance.Duration = 2                          --修改持续回合       


local i_CritChanceResist = i_mk(sc.CommonBuffs.CritChanceResist)   --引用强韧
      i_CritChanceResist.Duration = 2                        --修改持续回合       



-- local i_Poison = i_mk{                                      --持续伤害
--     Id = 904002,
    
    
--     Duration=2,   
--     AliveCheckType=3,


-- }




local sk_main = sk_mk{
    
    H_1= h_mk{                      
       TargetType=3,
       I_1={                                --防御强化
        Impact =i_DefenceEnhance,
       },

       I_2={
           Impact =i_CritChanceResist,           --强韧
           },

    --    I_3={
    --        Impact =i_Poison,
               
    --        }
       }
   }  


return sk_main