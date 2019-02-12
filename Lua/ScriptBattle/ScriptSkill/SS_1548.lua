
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--白骨3技能：白骨妖女召唤刺骨寒阵攻击敌方全体，造成白骨妖女攻击320%的普通伤害， 
--并以30%的的概率使其受到持续2回合的禁疗效果，持续2回合的攻击降低效果和持续两回合的缓速效果。[A78C84]【妖界效果】回复所造成伤害量20%的生命。[-]冷却时间5回合。

local i_damage  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}


--禁疗效果
local i_CureProhibit = i_mk(sc.CommonBuffs.CureProhibit)                   --引用通用禁疗效果
i_CureProhibit.Duration = 2                                                --修改持续回合     
--攻击降低效果
local i_AttackReduce = i_mk(sc.CommonBuffs.AttackReduce)                   --引用通用攻击降低效果
i_AttackReduce.Duration = 2                                                --修改持续回合     
--虚弱效果
local i_Weak = i_mk(sc.CommonBuffs.Weak )                   --引用通用缓速效果
i_Weak .Duration = 2                                                --修改持续回合     


local sk_main = sk_mk{

   
        H_1 = h_mk{                         --伤害
            TargetType = 4,
            I_1 = {
                Impact = i_damage,
            },
            I_2 = {                        --禁疗效果
                Chance = "a2",
                IsChanceRefix = 1,
                Impact = i_CureProhibit,
            },
            I_3 = {                        --攻击降低效果
                Chance = "a3",
                IsChanceRefix = 1,
                Impact = i_AttackReduce,
            },
            I_4 = {                        --虚弱效果
                Chance = "a4",
                IsChanceRefix = 1,
                Impact = i_Weak,
            }
        },

}

return sk_main