local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--海神-火 3技能

--
local i_kuangbao = i_mk{                      --攻击力暴击率增加
    Id = 2033000,
    ImpactLogic = 4,
    Param_1 = 4,
    Param_2 = 5000,
    Param_3 = 0,
    Param_4 = 2,
    Param_5 = 0,
    Param_6 = 5000,
    Param_7 = -1,
    Param_8 = -1,
    Param_9 = -1,

    Duration = -1,
}



--施加狂暴状态
local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType=2,
        IsAnimHit = 1,
        I_1 = {
            Impact = i_kuangbao,
        }
    },

}
return sk_main


