local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--一个桃子辅助
local sk_peach1 = sk_mk{
    H_1 = h_mk{
        TargetType=3,
        I_1 = {                      --1个桃子回全体3%的桃瑶最大血量
            Impact =  i_mk{
                Duration = 0,
                ImpactLogic = 1,
                Param_1 = 2,
                Param_2 = "a1", 
                IsPassiveImpact = 1,    
            }
        },
    },
    IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放
}
--二个桃子辅助
local sk_peach2 = sk_mk{
    H_1 = h_mk{
        TargetType=3,
        I_1 = {                      --2个桃子回全体6%的桃瑶最大血量
            Impact =  i_mk{
                Duration = 0,
                ImpactLogic = 1,
                Param_1 = 2,
                Param_2 = "a2",  
                IsPassiveImpact = 1,   
            }
        },
    },
    IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放
}
--三个桃子辅助
local sk_peach3 = sk_mk{
    H_1 = h_mk{
        TargetType=3,
        I_1 = {                      --3个桃子回全体9%的桃瑶最大血量
            Impact =  i_mk{
                Duration = 0,
                ImpactLogic = 1,
                Param_1 = 2,
                Param_2 = "a3",  
                IsPassiveImpact = 1,   
            }
        },
    },
    IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放
}
--回合开始释放1个桃子技能
local i_peach1 = i_mk{
    Id = 1731015,
    EffectID = 603,
    IsShow =1 ,
    ImpactLogic = 24,                       --逻辑说明：普通伤害
    ImpactClass= 0,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    MutexID = 1734,
    MutexPriority = 1,
    Param_1 = "a1",                        --攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 1, 
    Duration =-1,
 
}
--回合开始释放2个桃子技能
local i_peach2 = i_mk{
    Id = 1731016,
    EffectID = 604,
    IsShow =1 ,
    ImpactLogic = 24,                       --逻辑说明：普通伤害
    ImpactClass= 0,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    MutexID = 1734,
    MutexPriority = 2,
    Param_1 = "a2",                        --攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 1, 
    Duration =-1,

}
--回合开始释放3个桃子技能
local i_peach3 = i_mk{
    Id = 1731017,
    EffectID = 605,
    IsShow =1 ,
    ImpactLogic = 24,                       --逻辑说明：普通伤害
    ImpactClass= 0,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    MutexID = 1734,
    MutexPriority = 3,
    Param_1 = "a3",                        --攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 1, 
    Duration =-1,

}

local sk_main = sk_mk{            --妖界2技能

        H_1 = h_mk{
            TargetType=2,
            I_1 = {                      --消耗自己当前血量的20%
                Impact =  i_mk{
                    Duration = 0,
                    ImpactLogic = 34,
                    Param_1= 2000, 
                    Param_2= 11, 
                    Param_3= 0, 
                }
            },
            I_2= {                      --加桃子标记
                Impact =  i_mk{
                    Id = 1731011,
                    Duration = -1,
                    ImpactLogic = -1,   
                }
            },
        },
        H_2 = h_mk{
            TargetType=2,
            I_1 = {
                Impact =  i_mk{
                    Duration = 0,
                    ImpactLogic = 29, 
                    Param_1= 0,
                    Param_2= 1731011,
                    Param_3= 1,
                    Param_4= i_peach1,
                },
            },
            I_2 = {
                Impact =  i_mk{
                    Duration = 0,
                    ImpactLogic = 29, 
                    Param_1= 0,
                    Param_2= 1731011,
                    Param_3= 2,
                    Param_4= i_peach2,
                },
            
            },
            I_3 = {
                Impact =  i_mk{
                    Duration = 0,
                    ImpactLogic = 29, 
                    Param_1= 0,
                    Param_2= 1731011,
                    Param_3= 3,
                    Param_4= i_peach3,
                },
            
            },
        },
        IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放
}
return sk_main