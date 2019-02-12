local ssp = require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local testImpact = i_mk{

    ImpactLogic = 15,
    Param_1 = 1,
    Param_2 = 7560,
    Param_3 = 0,
    
}

local impact = ssp.deepParse(testImpact,{
    paramId = 1,
    iter = 1,
    parsed = {}
})

return impact