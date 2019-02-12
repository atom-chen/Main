local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")







-- local i_dispel  = i_mk{
--     ImpactLogic = 5,              --素馨驱散我方减益
--     Param_1 =4,   
--     Param_4 =99,
-- }

local i_recover  = i_mk{
    ImpactLogic = 1,              --素馨回血
    Param_1 =2,   
    Param_2 ="a1",
}






local sk_main = sk_mk{



    H_1 = h_mk{
        TargetType = 3,
        I_1 = {
            Impact = i_recover,
        },
    },




}
return sk_main


