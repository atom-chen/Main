local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

local i_AttackReduce = i_mk(sc.CommonBuffs.AttackReduce)   --引用通用攻击降低buff
      i_AttackReduce.AttackReduce = 1                      --修改持续回合    

--普通伤害
local i_damage1 = i_mk{
    ImpactLogic = 0,                       --逻辑说明：普通伤害
    ImpactClass= 2,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    Param_1 = "a1",                        --攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Duration =0,
}

local i_Stun = i_mk(sc.CommonBuffs.Stun)   --引用通用眩晕
      i_Stun.Duration = 1                      --修改持续回合     

--3段普攻+持续攻击降低
local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType=4,
        I_1 = {
            Impact = i_damage1,
        },
    },   
    H_2 = h_mk{
        TargetType=6,
        TargetParam_1 = 1,
        TargetParam_2 = 0,

        I_1 = {
            Impact = i_mk{
                Id = 1243010,
                ImpactLogic =37,
                Param_1 = 1,
                Param_2= 334,
                Param_3= "a2",
                Param_4 = -1,
                Param_5 = 10000,
                Param_6 = 1243011,
                Param_7 = 10000, 
                Param_8 = i_Stun,
                Param_9 = 10000, 
            },
        },
    },     
}
return sk_main

