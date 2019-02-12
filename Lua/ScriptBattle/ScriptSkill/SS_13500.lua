
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--鬼兵1技能：普攻+【被动】[A78C84]【妖界效果】日巡游在妖界攻击敌人时伤害提升20%，并将直接斩破目标的护盾效果。[-]


local i_damage  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
    
}

-- --驱散护盾
-- local i_clear = i_mk{
--     ImpactLogic = 5,                --驱散buff/debuff

--     Param_1 = 1,                    --被驱散的impact class
--     Param_2 = 2048,                   --subCLass
--     Param_3 = -1,                   --tag
--     Param_4 =  1,                   --驱散的数量
--     Param_5 =  0,                   --是否提示(0不提示，其他提示)
--     Duration = 0,
-- }

local sk_main = sk_mk{

    H_1 = h_mk{                         --伤害
        TargetType = 1,
        I_1 = {
            Impact = i_damage,
   
        }
    }
    
}



return sk_main