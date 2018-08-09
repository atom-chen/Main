local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


local sk_peach1 = sk_mk{
    H_1 = h_mk{
        TargetType=3,
        I_1 = {                      --1个桃子回全体10%的桃瑶最大血量
            Impact =  i_mk{
                Duration = 0,
                ImpactLogic = 1,
                Param_1 = 2,
                Param_2 = 1000,    
            }
        },
    },
}
--二个桃子辅助
local sk_peach2 = sk_mk{
    H_1 = h_mk{
        TargetType=3,
        I_1 = {                      --2个桃子回全体20%的桃瑶最大血量
            Impact =  i_mk{
                Duration = 0,
                ImpactLogic = 1,
                Param_1 = 2,
                Param_2 = 2000,    
            }
        },
    },
}
--三个桃子辅助
local sk_peach3 = sk_mk{
    H_1 = h_mk{
        TargetType=3,
        I_1 = {                      --3个桃子回全体30%的桃瑶最大血量
            Impact =  i_mk{
                Duration = 0,
                ImpactLogic = 1,
                Param_1 = 2,
                Param_2 = 3000,    
            }
        },
    },
}

local i_peach1 = i_mk{
    Id = 1731015,
    EffectID = 603,
    IsShow =1 ,
    ImpactLogic = 24,                       --逻辑说明：普通伤害
    ImpactClass= 0,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    MutexID = 1734,
    MutexPriority = 1,
    Param_1 = sk_peach1,                        --攻击力百分比(无则填0   10000表示造成的100%攻击力)
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
    Param_1 = sk_peach2,                        --攻击力百分比(无则填0   10000表示造成的100%攻击力)
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
    Param_1 = sk_peach3,                        --攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 1, 
    Duration =-1,
}

local sk_check = sk_mk{                  
    H_1 = h_mk{
        TargetType=1,
        I_1 = {
            Impact =  i_mk{                     ---加桃子
                Id = 1731011,
                Duration = -1,
                ImpactLogic = -1,   
            }
        },
        I_2 = {
            Impact =  i_mk{                    --根据桃子数量加表现和效果
                Duration = 0,
                ImpactLogic = 29, 
                Param_1= 0,
                Param_2= 1731011,
                Param_3= 1,
                Param_4= i_peach1,
            },
        },
        I_3 = {
            Impact =  i_mk{                    ----根据桃子数量加表现和效果
                Duration = 0,
                ImpactLogic = 29, 
                Param_1= 0,
                Param_2= 1731011,
                Param_3= 2,
                Param_4= i_peach2,
            },
        },
        I_4 = {
            Impact =  i_mk{                   --根据桃子数量加表现和效果
                Duration = 0,
                ImpactLogic = 29, 
                Param_1= 0,
                Param_2= 1731011,
                Param_3= 3,
                Param_4= i_peach3,
            },
        },
    },
}
local sk_main = sk_mk{
    TargetType = 1,
    H_1 = h_mk{
        I_1 = {
            Impact =  i_mk{
                ImpactLogic = 24,                        
                ImpactClass= 0,                           
                Param_1 = sk_check,                     --回合开始加桃子
                Param_2 = 1, 
                Duration =-1,
        },
    },
  }
}
return sk_main


