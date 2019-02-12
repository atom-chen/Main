
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--筝2技能：使用一次无CD的3技能

local i_fuhuo1  = i_mk{
    Duration = -1,
    ImpactClass= 0,
    CooldownId= 1118,
    Cooldown = 6,
    IsPassiveImpact=1,
    ImpactLogic = 13,              --死亡后复活释放技能

    Param_1 = 99,                  -- 激活次数
    Param_2 = 0,                   -- 回复血量
    Param_3 = "a1"                 --释放技能

}
local sk_main = sk_mk{

    

 
    H_1 = h_mk{                         --伤害
        TargetType = 2,
        I_1 = {
            Impact = i_fuhuo1,
        },
    },
    IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放
   
}

return sk_main