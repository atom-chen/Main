
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--盗宝小妖普攻

local i_damage  = i_mk{
    ImpactLogic = 34,       --普通伤害
    ImpactClass= 2,
    Param_1 = 500,          -- 参数1：百分比
    Param_2 = -1,           -- 参数2：伤害类型（-1普通伤害，11血祭伤害（扣血不播受击）
    Param_3 = -1,           -- 参数3：扣血百分比类型（-1生命上限 0当前血量）
}


local sk_main = sk_mk{

 
    H_1 = h_mk{                 --伤害
        TargetType = 1,
        I_1 = {
            Impact = i_damage,
        }
    },
}

return sk_main