
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--筝2技能：濒死复活

local i_fuhuo  = i_mk{
    Duration = -1,
    ImpactClass= 0,
    CooldownId= 1115,
    Cooldown = 5,
    IsPassiveImpact=1,
    ImpactLogic = 50,              --死亡后释放技能

    Param_1 = 111601,                  -- 技能参数

}



local sk_main = sk_mk{

    

 
    H_1 = h_mk{                         --伤害
        TargetType = 2,
        I_1 = {
            Impact = i_fuhuo,
        },
    },
    IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放

   
}

return sk_main