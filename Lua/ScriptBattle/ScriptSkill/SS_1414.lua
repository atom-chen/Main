local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

local i_CureProhibit = i_mk(sc.CommonBuffs.CureProhibit)               --引用通用禁疗
      i_CureProhibit.Duration = 2                                      --修改持续回合 



local i_damage = i_mk{
        ImpactLogic = 0,                       --逻辑说明：普通伤害
        ImpactClass= 2,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
        Param_1 = "a1",                        --攻击力百分比(无则填0   10000表示造成的100%攻击力)
        Duration =0,
}

local i_clearflag = i_mk{                       --
    ImpactLogic = 25,                                             
    Param_1 = 1414001,  
    Duration =0,
}

--最大生命值上线削减
local i_damage2 = i_mk{
    ImpactLogic = 4,                       --逻辑说明：属性修正
    ImpactClass= 2,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    ImpactSubClass = 4096,                 --子效果分类 削减生命上限
    Param_1 = 0,                           --//MaxHP = 0,//气血上限
    Param_3 = -1000,                        --类型1修正的值，百分比修正(无则填 0 正数增加 负数减小)
    Duration =-1,
}

local i_ifburnlatiao = i_mk{                       --
    ImpactLogic = 29,                                             
    Param_1 = 2,  
    Param_2 = 128,
    Param_3 = 1,
    Param_4 = i_clearflag,
    Duration =0,
}


local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 1,
        I_1 = {
            Impact = i_damage,
        },   
        I_2 = {
            Impact = i_CureProhibit,                    --禁疗
        }, 
        I_3 = {
            Impact = i_damage2,                         --降低血上限
        },   
        I_4 = {            
            Impact = i_mk{
                Id= 1414001,                           --拉条标签
                ImpactLogic = -1,    
                MutexID = 1414,
                MutexPriority = 1,
                Duration = 1,
                AliveCheckType = 1,
                AutoFadeOutTag = 0,
            },
        },
    },
    H_2 = h_mk{
        TargetType = 1,
        I_1 = {
            Impact = i_ifburnlatiao,
        },   
    }
}
return sk_main


