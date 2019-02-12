local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--一个桃子辅助
local sk_main = sk_mk{            

    H_1 = h_mk{
        TargetType=3,
        I_1 = {                      --1个桃子回全体3%的桃瑶最大血量
            Impact =  i_mk{
                Duration = 0,
                ImpactLogic = 1,
                Param_1 = 2,
                Param_2 = "a1", 
                IsPassiveImpact = 1,    
            }
            },
    },
IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放
}
return sk_main