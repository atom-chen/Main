local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

                                      




local i_recover  = i_mk{
    ImpactLogic = 1,              --扫晴娘回血
    Param_1 = 2,   
    Param_2 ="a1",
}






local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType = 3,
        I_1 = {
            Impact = i_recover,
        },
        I_2 = {
            Impact =  i_mk{        
                Duration = 0,
                ImpactLogic = 5,          --逻辑说明：驱散buff/debuff
                Param_1 = 4,              --参数1：被驱散的impact class
                Param_4 = 3,             --参数4：驱散的数量
            }
        },
    },




}
return sk_main


