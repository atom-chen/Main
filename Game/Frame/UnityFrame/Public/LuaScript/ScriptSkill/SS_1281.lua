
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--祸斗2技能：祸斗攻击敌人，造成祸斗攻击480%的普通伤害，如果自身带有减益效果，则随机转移一个减益效果给敌人。


local i_damage  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}

--嫁祸减益buff
local i_rengjianyi  = i_mk{
    Duration=0,
    ImpactLogic = 11,              --随机转移impact

    Param_1 = 4,               -- impact class
    Param_2 = 1,                  -- 偷取数量
    Param_3 = 4,                 -- 目标类型（1敌方，2我方，3发送者，4技能目标）技能目标是敌方，hit目标是施法者即可

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
        TargetType = 2,
        I_1 = {
            IsChanceRefix = 1,
            Impact = i_rengjianyi,
        }
    },

   
}

return sk_main