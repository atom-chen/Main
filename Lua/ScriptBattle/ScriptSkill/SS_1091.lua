local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")



--【被动】蜃瑶用幻术保护自己，以30%的概率将敌人的指向性攻击折射给1名随机敌人。

local i_zheshe = i_mk{
    Duration=-1,
    AliveCheckType=1,
    IsShow=0,
    MutexID=1092,
    MutexPriority=1,
    IsPassiveImpact=1,

    ImpactLogic = 18,                 --受到伤害后，会把伤害折射给一个非攻击者玩家

    Param_1=  2001,                   --表现用的技能id
    Param_2=  3000,                   --概率

}


local sk_main = sk_mk{
    
    H_1=h_mk{
        TargetType=2,
        I_1={                       --折射
            Impact = i_zheshe,
               
        }
    }
 
} 
                      

return sk_main