local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

local ss = require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--祝融男3技能-人界-附带灼伤

--普通伤害--黑火
local i_damage = i_mk{
    ImpactLogic = 0,                       --逻辑说明：普通伤害
    ImpactClass= 2,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    Param_1 = "a1",                        --攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Duration =0,
}

local i_Stun = i_mk(sc.CommonBuffs.Stun)                   --引用通用眩晕buff
      i_Stun.Duration = 1                                    --修改持续回合     
      
local i_Burning = i_mk(sc.CommonBuffs.Burning)               --引用通用灼伤buff
      i_Burning.Duration = 2                                 --修改持续回合  


local i_ifburn = i_mk{                       --
    ImpactLogic = 29,                                             
    Param_1 = 2,  
    Param_2 = 128,
    Param_3 = 1,
    Param_4 = i_Stun,
    Duration =0,
}

local i_clearflag = i_mk{                       --
    ImpactLogic = 25,                                             
    Param_1 = 1414001,  
    Duration =0,
}

local i_ifburnCD = i_mk{                       --
    ImpactLogic = 29,                                             
    Param_1 = 2,  
    Param_2 = 128,
    Param_3 = 1,
    Param_4 = i_clearflag,
    Duration =0,
}

local i_boomdown = i_mk{                       --云中君炸弹回合-1
    ImpactLogic = 47,                                             
    Param_1 = 4,  
    Param_2 = -1,
    Param_3 = 5,
    Param_4 = 2,
    Duration =0,
}

local sk_main = sk_mk{                      --人界技能
    H_1 = h_mk{
        TargetType=1,
        I_1 = {
            Impact = i_damage,
        },
        I_2 = {
            Impact = i_boomdown,            --人界  加速炸弹 -1CD

        },  
        I_3 = {
            Chance = 10000,                 --人界  附带灼伤
            IsChanceRefix = 1,
            Impact = i_Burning,
        },
        I_4 = {
            Chance = 3000,                 --人界  如果灼伤概率晕眩
            IsChanceRefix = 1,
            Impact = i_ifburn,
        },
        I_5 = {            
            Impact = i_mk{
                Id= 1743002,                           --人界标签
                ImpactLogic = -1,    
                MutexID = 1733,
                MutexPriority = 1,
            },
        },
    },
    H_2 = h_mk{
        TargetType=1,
        I_1 = {
             --人界  如果灼伤减CD
            Impact = i_ifburnCD,
        } 
    }
}


return sk_main

