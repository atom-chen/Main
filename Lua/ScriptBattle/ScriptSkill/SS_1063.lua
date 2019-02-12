local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")




local i_xixue  = i_mk{
    ImpactLogic = 51,              --穷奇死前吸血
    Param_1 =3000,         
    Param_2 =3000,
}



local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 3,
        I_1 = {
            Impact = i_xixue,
        },
    },
    IgnoreSelfState = 1,
    IgnoreTargetDead = 1,

}
return sk_main


