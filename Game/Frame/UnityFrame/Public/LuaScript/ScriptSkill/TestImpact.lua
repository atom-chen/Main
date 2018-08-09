local ssp = require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local testImpact = i_mk{
    ImpactLogic = 0,       --普通伤害

    Param_1 = 15000,       -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,           -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,          -- 同一技能相同impact多次命中衰减系数
}

local impact = ssp.deepParse(testImpact,{
    paramId = 1,
    iter = 1,
    parsed = {}
})

return impact