local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


--伤害药童1技能普攻+持续伤害
local i_damage = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,                --普通伤害

    Param_1 = "a1",                 -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                    -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = 0,                    -- 同一技能相同impact多次命中衰减系数
}

--持续伤害
-- local i_poison = i_mk{
--     Id = 904002,
--     Duration = 2,

-- }

local i_Poison = i_mk(sc.CommonBuffs.Poison)   --引用通用持续伤害
      i_Poison.Duration = 2                      --修改持续回合       



local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType = 1 ,
        I_1 = {                         --伤害
            Impact = i_damage,
        },
 
        I_2 = {                        --持续伤害
            Chance = "a2",
            IsChanceRefix = 1,
            Impact =i_Poison,
        }
    
    },
    
}

return sk_main