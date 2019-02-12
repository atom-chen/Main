
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--朽扉2技能：普攻+禁疗2回合

local i_damage  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}

--禁疗效果
local i_CureProhibit = i_mk(sc.CommonBuffs.CureProhibit)       --引用通用禁疗
      i_CureProhibit.Duration = 2                              --修改持续回合          



local sk_main = sk_mk{

    

 
    H_1 = h_mk{                         --伤害
        TargetType = 1,
        I_1 = {
            Impact = i_damage,
        },
        I_2 = {                        --禁疗效果
            Chance = "a2",
            IsChanceRefix = 1,
            Impact = i_CureProhibit,
    }
    },

   
}

return sk_main