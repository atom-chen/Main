local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--海神-火 2技能

--普通伤害
local i_damage = i_mk{
    ImpactLogic = 0,                       --逻辑说明：普通伤害
    ImpactClass= 2,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    Param_1 = "a1",                        --攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Duration =0,
}
local i_Burning = i_mk(sc.CommonBuffs.Burning)   --引用通用灼伤buff
      i_Burning.Duration = 3                  --修改持续回合             

local i_CoreOnCrit = i_mk{
    ImpactLogic = 49,                         --触发暴击时，根据伤害量吸血
    
    Param_1 = 3000,                           --吸血比例 
    Param_2 = -1,                             --包装技能--施法者效果接受者，技能目标受伤害者，一段hit，hit目标效果接受者（被治疗者）  

    Duration = 1,
    AliveCheckType = 2,
    AutoFadeOutTag =16,
}



--普攻
local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType=2,
        IsAnimHit = 0,
        I_1 = {
            Impact = i_CoreOnCrit,
        }
    },


    H_2 = h_mk{
        TargetType=4,
        I_1 = {
            Impact = i_damage,
        },

        I_2 = {
            Chance = "a2",
            IsChanceRefix = 1,
            Impact = i_Burning,
        }    
    },
}
return sk_main


