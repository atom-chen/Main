local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


--玉圭3技能 拉条20%
local i_latiao = i_mk{
    ImpactLogic = 6,                --行动条增加减少

    Param_1 = "a1",                 -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
  
}


local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType = 3 ,
        I_1 = {                         --拉条
            Impact = i_latiao,
        },
    },
    
}

return sk_main