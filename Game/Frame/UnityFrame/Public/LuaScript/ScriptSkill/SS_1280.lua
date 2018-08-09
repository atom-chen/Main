
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--祸斗1技能：祸斗攻击敌人，造成祸斗攻击480%的普通伤害，如果敌人身上有增益效果，则随机抢夺一个添加到自身上。

local i_damage  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}

--偷取增益buff
local i_touzengyi  = i_mk{
    Duration=0,
    ImpactLogic = 30,              --全体随机转移impact

    Param_1 = 1,               -- ImpactClass
    Param_2 = 1,                  -- 偷取数量(全局)
    Param_3 = 3,                 -- 目标类型（1敌方，2我方，3发送者）

}



local sk_main = sk_mk{

 
    H_1 = h_mk{                         --伤害
        TargetType = 1,
        I_1 = {
            Impact = i_damage,
        }
    },
    H_2 = h_mk{ 
        IsAnimHit=0,                        --偷buff
        TargetType = 1,
        I_1 = {
            IsChanceRefix = 1,
            Impact = i_touzengyi,
        }
    },

   
}

return sk_main