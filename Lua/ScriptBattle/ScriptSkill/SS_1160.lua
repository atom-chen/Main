local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


--1.睚眦挥剑斩击敌人，有几率嘲讽攻击目标


local i_damage  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}



--嘲讽效果
local i_Defiance = i_mk(sc.CommonBuffs.Defiance)   --引用通用嘲讽
      i_Defiance.Duration = 1                      --修改持续回合          


local i_qingchubiaoji  = i_mk{
    Duration=0,
    AliveCheckType=1,

    ImpactLogic = 25,              --根据id驱散buff

    Param_1 = 1162210,               -- 被驱散的impact id
}


local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 1,
        I_1 = {
            Impact = i_damage,
        },
        I_2 = {                        --嘲讽效果
            Chance = "a2",
            IsChanceRefix = 1,
            Impact = i_Defiance,
        },
        I_3 = {                        
    
            Impact = i_qingchubiaoji,
    },

    },
}
return sk_main


