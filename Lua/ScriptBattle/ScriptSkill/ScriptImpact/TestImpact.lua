local ssp = require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local testImpact = i_mk{
    Id = 911001,

    ImpactLogic = 4,
    Param_1 = 7,
    Param_2 = -2500,
    Param_3 = 0,
    
    Duration = 1,
    AliveCheckType = 2,
    AutoFadeOutTag = 0,
    ImpactClass = 4,
    ImpactSubClass = 128, 
    MutexID = -1,
    MutexPriority =1,
}

local impact = ssp.deepParse(testImpact,{
    paramId = 1,
    iter = 1,
    parsed = {}
})

return impact