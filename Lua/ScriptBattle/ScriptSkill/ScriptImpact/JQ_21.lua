local ssp = require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local testImpact = i_mk{

    ImpactLogic = 4,
    Param_1 = 4,
    Param_2 = 10000,
    Param_3 = 0,

    Duration = -1,
    AliveCheckType = 3,
    AutoFadeOutTag = 0,
    ImpactClass = 0,
    ImpactSubClass = 0, 
}

local impact = ssp.deepParse(testImpact,{
    paramId = 1,
    iter = 1,
    parsed = {}
})

return impact