
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--黑狼2技能辅助，清buff回血
local i_core = i_mk{                 --回复5%最大生命值的血量

    ImpactLogic = 1,
    Param_1 = 3,
    Param_2 = 500,
}
local i_qusan = i_mk{                 --驱散buff
    ImpactLogic = 5,                 
    Param_1 =  4,                     --被驱散的impact class
    Param_2 = -1 ,                    --subCLass 
    Param_3 = -1,                     --tag
    Param_4 = 99,                     --驱散数量
    Param_5 = 1 ,                     --是否提示
}


local sk_main = sk_mk{

    H_1 = h_mk{                 
        TargetType = 2,
        I_1 = {
            Impact = i_core,      --回复血量
        },
        I_2 = {
            Impact = i_qusan,    --驱散 
        },
    },
}

return sk_main