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


local sk_main = sk_mk{
    

    
    H_1=h_mk{
           TargetType=3,
            I_1={                       --拉条
                Impact =i_actionbar,
               
                }
            }
 
} 
                      

return sk_main