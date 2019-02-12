local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")
--玄武3技能被动
local i_Invincible = i_mk(sc.CommonBuffs.Invincible)   --引用通用无敌buff
      i_Invincible.Duration = 1                        --修改持续回合    


local i_chufa = i_mk{                    --血量低于30%时无敌一回合        
    ImpactLogic = 28,                    -- 血量满足特定条件时，触发子效果，不满足时，根据配置是否移除子效果，该效果被移除时，自动移除子效果

    Param_1 = 0,                         --血量操作符（0，小于，1大于，2小于（上buff立即触发），3大于（上buff立即触发））
    Param_2 = 3000,                      --血量百分比(10000)
    Param_3 = 0,                         --不满足时是否移除（0，否；1，是）
    Param_4 = i_Invincible,              --子效果impact id--无敌1回合
    Param_5 = 1974010,                   --子效果impact id--表现辅助1
    Param_6 = 1972010,                   --子效果impact id--表现辅助2
    IsPassiveImpact = 1, 
    Duration =-1,
}

local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType = 2,
        I_1 = {
        Impact = i_chufa,
        },
    },
    IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放
}

return sk_main

