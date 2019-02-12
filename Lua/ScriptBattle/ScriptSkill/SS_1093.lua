local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")



--不会唤醒沉睡的攻击


local i_nothuanxing = i_mk{
    Duration=-1,
    MutexID=1090,
    MutexPriority=1,

    ImpactLogic =4,                         

    Param_1= 104,           
    Param_2= 1,      
    Param_3= 0,      
}


local sk_main = sk_mk{
    

    
    H_1=h_mk{
        TargetType=2,
        I_1={                       
            Impact = i_nothuanxing,
               
        }
    }
 
} 
                      

return sk_main