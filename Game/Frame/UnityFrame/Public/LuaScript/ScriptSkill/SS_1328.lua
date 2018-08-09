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
        TargetType = 14,                      --14，随机敌方有特定buff（1，数量，2，impactClass，3，是否反转（不包含buff））
        TargetParam_1 = 6,                    --人数
        TargetParam_2 = 4,                    --impactclass
        TargetParam_3 = 1,                    --反转


        I_1 = {
            Impact = i_damage,
        },

        I_2 = {
            Impact=i_mk{
                ImpactLogic = 6,               --行动条
                Param_1 = -300,                --击退
            }
        }    
    },




}

return sk_main