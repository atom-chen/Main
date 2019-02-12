
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--钟馗2技能-轮到钟馗行动时，小鬼会钟馗身上的一个减益效果，每吃掉一个，小鬼成长一次，小鬼最多成长5次
--[妖界]会根据小鬼吃掉buff的数量回复钟馗的体力

local i_recovery = i_mk{  
    
    IsPassiveImpact=1,

    MutexID = 1422,
    MutexPriority=1,
    
    ImpactLogic= 56,                       --被动监听,驱散了指定的buff后，根据驱散量，获得治疗
    Param_1= 4,                            --impact class
    Param_2= "a1",                          --治疗基数
    Duration = -1,
    AutoFadeOutTag=16,

}


local i_clear = i_mk{

    IsPassiveImpact=1,
    
    ImpactLogic = 5,                --驱散buff/debuff
    Tag = 3,

    Param_1 = 4,                    --被驱散的impact class
    Param_2 = -1,                   --subCLass
    Param_3 = -1,                    --tag
    Param_4 =  1,                   --驱散的数量
    Param_5 = -1,                   --是否提示(0不提示,其他提示)
    Param_6 = 64257,                  
    Duration = 0,

}

local i_jianshi = i_mk{
    ImpactLogic = 29,               --激活时检测，如果接受者指定buff存在超过指定数量时，触发一个额外的效果

    Param_1 = 1,                    --类型（0，id；1，class；2，subClass）
    Param_2 = 4,                    --参数(根据类型区分具体意义)
    Param_3 = 1,                    --数量
    Param_4 = i_clear ,             --额外的效果impact id                               
    Duration = 0,
    
}




local i_diejiacengshu = i_mk{
    ImpactLogic = 39,               --发送过指定数量的impact后，触发使用技能，技能目标自己，通过hit做差异

    Param_1 = 3,                    --impact的tag（-1表示任意）
    Param_2 = 1,                    --数量
    Param_3 = sk_mk{
            H_1=h_mk{
                TargetType= 2 ,
                I_1={
                     Impact=i_mk{
                        Id =1013011,
                        ChildImpact = 1013012,
                     }
                    }
            
            }  
    } ,                             --技能id
    Param_4 = 1 ,                   --立即释放(1,立即；其他顺序)
    Duration = -1,
}






local sk_clear = sk_mk{
    H_1 = h_mk{
        TargetType = 2,
        I_1 ={
            Impact =  i_jianshi,               --人界：检测如果debuff超过1个则触发驱散
        },
        -- I_2 ={
        --     Impact =  i_diejiacengshu,               --层数检测
        -- },
      
    },
    EnvLimit = 0,
    IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放
    OtherID = sk_mk{
        H_1 = h_mk{
            TargetType = 2,
            I_1 ={
                Impact =  i_recovery,          --妖界：同时进行回血
            },
            I_2 ={
                Impact =  i_jianshi,          --妖界：检测+驱散
            },
          
            -- I_3 ={
            --     Impact =  i_diejiacengshu,               --层数检测
            -- },
        },
        EnvLimit = 1,  
        IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放 
    },

}
    



local  i_UseSkillRoundBegin = i_mk{  -- 波次开始使用技能
    ImpactLogic = 24,                --定期触发使用技能

    Param_1 = sk_clear,               -- 技能id
    Param_2 = 1,                    -- 周期类型（见24号逻辑）
    Param_3 = -1,                   -- 条件类型（见24号逻辑）
    Param_4 = -1,                   -- 条件参数
    Param_5 = -1,                   -- 条件参数
    Param_6 = -1,                   -- 是否立即生效（回合开始时用的技能，必须时立即释放，这个参数无用，回合结束使用技能，必须时顺序释放，这个参数无用）
    Param_7 = -1,                   -- 技能类型（-1，skillExId；1，skillIndex）
    Param_8 = -1,                   -- 周期参数
    Param_9 = -1,                   -- 周期参数

    Duration = -1
}





local sk_main = sk_mk{

    H_1 = h_mk{                 --回合开始使用技能
        TargetType = 2,
    
        I_1 = {
            Impact = i_UseSkillRoundBegin,
        },
        I_2= {
            Impact = i_diejiacengshu,
        }
    },
    IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放
}

return sk_main