local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")



--拉条


local i_actionbar = i_mk{
   
    ImpactLogic =6,                 --行动条增加减少           

    Param_1= "a1",                  --增减数（负数表示减少）
    Duration = 0,

}

local i_SpeedEnhance = i_mk(sc.CommonBuffs.SpeedEnhance)   --引用通用急速
      i_SpeedEnhance.Duration = 2                          --修改持续回合      

local sk_main = sk_mk{
    

    
    H_1=h_mk{
           TargetType=3,
            I_1={                       --拉条
                Impact =i_actionbar,
               
                },
            I_2={                       --两回合急速
                Impact =i_SpeedEnhance,
               
                }
            }
 
} 
                      

return sk_main