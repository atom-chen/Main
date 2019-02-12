local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")




local i_qusan  = i_mk{
    ImpactLogic = 5,             --驱散所有debuff
    Param_1 = 4,
    Param_4 = 99,
    ImpactClass =0,
    IsShow =1,
}


local i_huixue  = i_mk{
    Id = 102011,
    ImpactLogic = 1,              --回复100%血量
    Param_1 = 3,
    Param_2 = 10000,
    ImpactClass = 0,
}



local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType = 1,
        I_1 = {
            Impact = i_qusan,
        },
        I_2 = {
            Impact = i_huixue,
        },
    },
}
return sk_main


