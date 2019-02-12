
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--攻击敌方单体，造成扶鸾攻击330%的普通伤害。【妖界效果】目标每有1个减益效果，伤害增加10%。


local i_damage1  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
    
}



--根据buff数量增加属性修正
local i_yishang  = i_mk{

    Duration=1,
    AliveCheckType=2,
    AutoFadeOutTag = 16,

    ImpactLogic = 57,              --根据buff数量增加属性修正

    Param_1 = 2,                  -- buff计数类型（1，impactId；2，class；3，subClass）
    Param_2 = 4,                  -- buff参数
    Param_3 = 7,                  -- 属性类型
    Param_4 = -1000,              -- 固定值修正
    Param_5 = 0,                  -- 百分比修正

}



local sk_main = sk_mk{

    H_1 = h_mk{                         --伤害
        TargetType = 1,
        I_1 = {
            Impact = i_yishang,
        },
        I_2 = {
            Impact = i_damage1,
        },
    },
    
}



return sk_main