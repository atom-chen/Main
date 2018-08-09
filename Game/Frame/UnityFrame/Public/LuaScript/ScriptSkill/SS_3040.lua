local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

local i_Silent = i_mk(sc.CommonBuffs.Silent)   --引用通用沉默
      i_Silent.Silent = 2                      --修改持续回合 

--普通伤害
local i_damage1 = i_mk{
    ImpactLogic = 0,                       --逻辑说明：普通伤害
    ImpactClass= 2,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    Param_1 = "a1",                        --攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Duration =0,
}



--普攻+30%概率引爆炸弹
local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType=1,
        I_1 = {
            Impact = i_damage1,
        },
        I_2 = {
            Chance = "a2",
            IsChanceRefix = 1,
            Impact = i_Silent,
        },
    },    
}
return sk_main

