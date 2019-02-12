local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


--酒童2技能：在接下来的3回合内受到的伤害降低30%，





local i_tihuan = i_mk{
    Duration=3,
    AliveCheckType=3,
    MutexID=1941,
    MutexPriority=1,
    ImpactLogic = 36,           --技能替换,激活时,替换指定的技能为新技能          
    Param_1 = "a1",            --技能3替换id,不替换配-1
}

local i_reducedam  = i_mk{
    ImpactLogic = 4,               
    Param_1 = 7,                    
    Param_2 = "a2",                    
    Param_3 = 0,

    Duration = 3,
    MutexID  = 1942,
    MutexPriority = 1,
    
}



local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 2,
        I_1 = {
            Impact = i_tihuan,
        },
        I_2 = {                        
            
            Impact = i_reducedam,
    }
    },
}
return sk_main


