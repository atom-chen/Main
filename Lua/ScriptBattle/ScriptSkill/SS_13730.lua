local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


--玉圭2技能 人界回血
local i_Core = i_mk{
    ImpactLogic = 1,               --治疗
    
    Param_1 = 2,                          --治疗类型(1攻击力，2治疗者血上限，3被治疗者血上限)
    Param_2 = "a1",

}



local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType = 17 ,
        I_1 = {                         --治疗
            Impact = i_Core,
        },
    },
    IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放
}

return sk_main