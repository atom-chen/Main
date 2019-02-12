
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--蜃珧3技能觉醒：以不会唤醒沉睡效果的攻击，攻击敌人，造成蜃珧攻击640%的普通伤害，敌人每有一个减益效果，本次攻击伤害就增加25%。冷却时间4回合。

local i_damage  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}

--根据buff数量增加属性修正
local i_yishang  = i_mk{
    Id=1095012,
    Duration=1,
    AliveCheckType=2,

    ImpactLogic = 57,              --根据buff数量增加属性修正

    Param_1 = 2,                  -- buff计数类型（1，impactId；2，class；3，subClass）
    Param_2 = 4,                  -- buff参数
    Param_3 = 7,                 -- 属性类型
    Param_4 = -2500,                 -- 固定值修正
    Param_5 = 0,                 -- 百分比修正

}

local i_qusanbuff  = i_mk{
    Duration=0,
    IsShow=1,

    ImpactLogic = 25,              --根据id驱散buff

    Param_1 = 1095012,             -- 被驱散的impact id

}


local sk_main = sk_mk{
 
    H_1 = h_mk{ 
        IsAnimHit=0,                        
        TargetType = 1,
        I_1 = {
            Impact = i_yishang,
        },
    },
    H_2 = h_mk{                         
        TargetType = 1,
    
        I_1 = {
            Impact = i_damage,
        },
        I_2 = {
            Impact = i_qusanbuff,
        },
    },

   
}

return sk_main