local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")



local i_damage1  = i_mk{
    ImpactLogic = 0,              --九尾灵狐普通伤害

    Param_1 ="a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
    ImpactClass = 2,
}

local i_boomdot  = i_mk{
    ImpactLogic = 63,              --爆毒

    Param_1 =64,                   -- 参数1：SubClass
    Param_2 = -1,                  -- 参数2：增加buff回合（0无，-1减1回合，1加一回合，-2减2回合。。以此类推）
    ImpactClass = 4,
}



local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType=1,
        I_1 = {
            Impact = i_damage1,
        },
        I_2 = {
            IsChanceRefix = 1,
            Impact = i_boomdot,
        }  
    },
  
}
return sk_main


