local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


--普通伤害
local i_damage = i_mk{
    ImpactLogic = 0,
    ImpactClass= 2,
    Param_1 = "a1",
}


--额外普攻+击退
local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType = 11,                      --11，特定buff标记过的敌人
        TargetParam_1 = 1322010,              --标记
    
        I_1 = {
            Impact = i_damage,
        },
    
        I_2 = {
            IsChanceRefix = 1,
            Impact=i_mk{
                ImpactClass=4,
                ImpactLogic = 6,               --行动条
                Param_1 = -300,                --击退
            }
        }    
    },



}

return sk_main