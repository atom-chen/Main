local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

local i_damage = i_mk{
    ImpactLogic = 0,
    ImpactClass= 2,
    Param_1 = "a1",

}



local i_SpeedReduce = i_mk(sc.CommonBuffs.SpeedReduce)     --引用通用缓速buff
      i_SpeedReduce.Duration = 1                              --修改持续回合  



--普攻+缓速905001
local sk_main = sk_mk{

    H_1 = h_mk{

        IsAnimHit = 1,
        I_1 = {
            Impact = i_damage,
        },
        I_2 = {
            Impact = i_SpeedReduce,
            IsChanceRefix = 1,
        }    
    },
}

return sk_main