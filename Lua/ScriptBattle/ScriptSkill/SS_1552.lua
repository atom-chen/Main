
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--地动3技能：全体+打乱行动条

local i_damage  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
    
}


local i_xitiao  = i_mk{
    Duration = 0,
    MutexID=1552,
    MutexPriority=1,
    ImpactLogic = 52,              --行动条洗条，所有行动条上的人，互换位置

    Param_1 = -1,                

}



local sk_main = sk_mk{
    H_1 = h_mk{

        TargetType = 4,
        I_1 = {
            Impact = i_damage,
        },
        I_2 = {
            Impact = i_xitiao,
        }
    },

}



return sk_main