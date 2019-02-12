
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--仓颉1：攻击敌方单体，概率使目标的攻击力降低

local i_damage  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}

--攻击力降低效果
local i_AttackReduce = i_mk(sc.CommonBuffs.AttackReduce)   
      i_AttackReduce.Duration = 2                      --修改持续回合          



local sk_main = sk_mk{

    

 
    H_1 = h_mk{                         --伤害
        TargetType = 1,
        I_1 = {
            Impact = i_damage,
        },
        I_2 = {                       
            Chance = "a2",
            IsChanceRefix = 1,
            Impact = i_AttackReduce,
    }
    },

   
}

return sk_main