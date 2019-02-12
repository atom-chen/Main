local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--驱散塔-为首领驱散所有的减益效果。

--驱散
local i_qusan  = i_mk{
    ImpactLogic = 5,              --驱散buff/debuff

    Param_1 =4,                 -- 被驱散的impact class
    Param_2 = -1,                  -- subCLass
    Param_3 = -1,                  -- tag
    Param_4 = 99,                  --驱散的数量
    Param_5 = 1,                   --是否提示(0不提示，其他提示)
}                                 

--
local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType=1,
        I_1 = {
            Impact = i_qusan,
        },  
    },
}
return sk_main

