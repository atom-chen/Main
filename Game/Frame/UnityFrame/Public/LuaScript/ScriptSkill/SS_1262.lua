local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")







local i_recovery = i_mk{
    ImpactClass=1,
    ImpactLogic =1,                         --治疗

    Param_1 = 3,                            --治疗类型(1攻击力,2治疗者血上限,3被治疗者血上限)
    Param_2 = "a1",                         --技能系数(10000表示造成的100%的治疗)
    Duration = 0,

}





local sk_main = sk_mk{
    

    
    H_1=h_mk{
           TargetType=3,
            I_1={                           -- 治疗
                Impact =i_recovery,
               
         }
    }
 
} 
                      

return sk_main