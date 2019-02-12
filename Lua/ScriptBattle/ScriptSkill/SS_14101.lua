local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

local ss = require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--祝融男1技能-妖界-概率使用2技能

--普通伤害--高系数
local i_damage = i_mk{
    ImpactLogic = 0,                       --逻辑说明：普通伤害
    ImpactClass= 2,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    Param_1 = "a1",                        --攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Duration =0,
}
    
      
local i_Burning = i_mk(sc.CommonBuffs.Burning)               --引用通用灼伤buff
      i_Burning.Duration = 2                                 --修改持续回合  

local i_fourteen = i_mk{
        ImpactLogic = 14,         
        Param_1 = 4,             --参数1：skilltype
        Param_2 ="a2",           --参数2：技能参数
        Param_3 =10000,          --参数3：概率上炸弹的概率
        Param_4 =-1,             --参数4：额外使用的技能索引（该角色的第几个技能）     
        Param_5 ="a3",           --参数5：额外使用的技能（skillEx表id，参数4必须填-1才生效）
        Param_6 =2,              --参数6：条件检查时机（1，使用技能前，2，使用技能后）
        Param_7 =1,              --参数7：不能切换目标（0，否（目标死亡后会随机选择目标），1是（目标死亡后追加不能触发））   
        Duration = 1,
        AliveCheckType=2,
    }



local sk_main = sk_mk{                      --妖界技能
    H_1 = h_mk{
        TargetType=1,
        I_1 = {
            Impact = i_damage,
        },

        I_2 = {
            Chance = "a4",                 --概率灼伤
            IsChanceRefix = 1,
            Impact = i_Burning,
        },
    },
    H_2 = h_mk{
        TargetType=2,
        I_1= {
            Chance = "a5",                 --妖界  30%概率使用技能2
            Impact = i_fourteen,
        }  
    }
}


return sk_main

