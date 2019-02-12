
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--攻击敌方单体，造成小籍攻击力330%的普通伤害。【觉醒效果】小籍攻击时以50%的概率减少自身所有技能的冷却时间1回合。

local i_damage  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}

--减少技能冷却时间

local i_colddowndown = i_mk{
    Duration=0,

    ImpactLogic =35,                 --增减CD，有CD的技能，才会受这个效果影响         

    Param_1= -1,                  --技能索引（-1表示全部技能(1~3)）
    Param_2=  -1,                  --增减值(大于0增加CD，小于0减少CD)


}



local sk_main = sk_mk{


    H_1 = h_mk{                         --伤害
        TargetType = 1,
        I_1 = {
            Impact = i_damage,
        },
    },

    H_2 = h_mk{                     
        TargetType = 2,
        I_1 = {
            Chance = "a2",
            Impact = i_colddowndown,
        },
    }

   
}

return sk_main