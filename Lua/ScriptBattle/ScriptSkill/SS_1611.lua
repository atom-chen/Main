local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")




--单体普通伤害
local i_damage = i_mk{
    ImpactLogic = 0, 
    ImpactClass= 2,
    Param_1 = "a1",               --参数1：攻击力百分比(无则填0   10000表示造成的100%攻击力)
}






--单体普攻---
local sk_main = sk_mk{

    H_1 = h_mk{                 --单体伤害
    TargetType = 1,
    I_1 = {
        Impact = i_damage,
    },

},

}

return sk_main