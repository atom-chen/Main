local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--拉条塔-为首领增加50%的行动条。


--驱散
local i_latiao  = i_mk{
    ImpactLogic = 6,              --行动条增加减少

    Param_1 ="a1",                 -- 增减数（负数表示减少）
  
}                                 

local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType=1,
        I_1 = {
            Impact = i_latiao,
        },  
    },
}
return sk_main

