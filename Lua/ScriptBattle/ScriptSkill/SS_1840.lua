
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--猫将军1技能：普攻

local i_damage  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}
     

--灼伤效果
local i_Burning = i_mk(sc.CommonBuffs.Burning)             --引用通用灼伤
      i_Burning.Duration = 2                               --修改持续回合          
                   

local sk_main = sk_mk{

    H_1 = h_mk{                         --伤害
        TargetType = 1,
        I_1 = {
            Impact = i_damage,
        },
        I_2 = {                        --灼烧效果
        Chance = "a2",
        IsChanceRefix = 1,
        Impact = i_Burning,
    },
},
    H_2 = h_mk{                         --伤害
        TargetType = 1,
        I_1 = {
        Impact = i_damage,
        },
        I_2 = {                        --灼烧效果
        Chance = "a2",
        IsChanceRefix = 1,
        Impact = i_Burning,
    },
},
    H_3 = h_mk{                         --伤害
    TargetType = 1,
    I_1 = {
        Impact = i_damage,
    },
    I_2 = {                        --灼烧效果
        Chance = "a2",
        IsChanceRefix = 1,
        Impact = i_Burning,
    },
}
    

   
}

return sk_main