local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


local i_CriticalEnhance = i_mk(sc.CommonBuffs.CriticalEnhance)   --引用狂暴
      i_CriticalEnhance.Duration = 2                      --修改持续回合  



local sk_check1 = sk_mk{
        H_1 = h_mk{
            TargetType = 2,
            I_1 = {                           --换复活技能
                Impact = i_mk{
                    Duration = 0,
                    ImpactLogic = 29, 
                    Param_1= 0,
                    Param_2= 1731011,
                    Param_3= 3,
                    Param_4= i_mk{
                        ImpactLogic = 36,                        
                        Param_1 = -1,                            
                        Param_2 = -1,
                        Param_3 = "a1",
                        Param_4 = -1,--“a1”
                        Param_5 = -1,--“a1”
                        Duration =1,
                    }
                }
        },

    },
 }

 local sk_check2 = sk_mk{   
    H_1 = h_mk{
        TargetType = 2,
            I_1 = {                           --换2技能成被动，主动技能禁止
                Impact = i_mk{
                Duration = 0,
                ImpactLogic = 29, 
                Param_1= 0,
                Param_2= 1743001,
                Param_3= 1,
                Param_4= i_mk{
                            ImpactLogic = 36,                        
                            Param_1 = -1,                            
                            Param_2 ="a3",
                            Param_3 = -1,
                            Param_4 = -1,--“a1”
                            Param_5 = -1,--“a1”
                            Duration =1,
                        },
                },
            },
    }
 }

 local sk_clean = sk_mk{

    H_1 = h_mk{   
        TargetType = 2,
        I_1 = {
            Impact = i_mk{
                Duration = 0,
                ImpactLogic = 25, 
                Param_1= 1731017,
            },
        }
    },
    H_2 = h_mk{                            --驱散桃子标签
        TargetType = 2,
        I_1 = {
            Impact = i_mk{
                Duration = 0,
                ImpactLogic = 25, 
                Param_1= 1731011,
            }        
        },
    },
}


local sk_trytry = sk_mk{
    H_1 = h_mk{                                          --清标签和表现
      TargetType = 2,
      IsAnimHit = 0,
        I_1 = {
            Impact =  i_mk {                   --使用3技能人界时额外用技能
                ImpactLogic = 14,                        
                Param_1 = 4,                            
                Param_2 = "a1",
                Param_3 = 10000,    
                Param_4 = -1,--“a1”
                Param_5 = sk_clean,--“a1”
                Param_6 = 2,--“a1”
                Param_7 = 1,--“a1”
                Duration =1,
            }
        },
        I_2 = {
            Impact =  i_mk {                   --使用3技能妖界时额外用技能
                ImpactLogic = 14,                        
                Param_1 = 4,                            
                Param_2 = "a2",
                Param_3 = 10000,    
                Param_4 = -1,--“a1”
                Param_5 = sk_clean,--“a1”
                Param_6 = 2,--“a1”
                Param_7 = 1,--“a1”
                Duration =1,
            }
        },
    },
}




local i_lowhpchange = i_mk{                    --血量低于20%时换技能2成被动        
    ImpactLogic = 28,                          --血量满足特定条件时，触发子效果，不满足时，根据配置是否移除子效果，该效果被移除时，自动移除子效果
    Param_1 = 0,                               --血量操作符（0，小于，1大于，2小于（上buff立即触发），3大于（上buff立即触发））
    Param_2 = 2000,                            --血量百分比(10000)
    Param_3 = 1,                               --不满足时是否移除（0，否；1，是）
    Param_4 = 1743001,                         --子效果impact id--低血标签
    -- Param_4 = i_mk{
    --     ImpactLogic = 36,                        
    --     Param_1 = -1,                            
    --     Param_2 ="a3",
    --     Param_3 = -1,                        
    --     Param_4 = -1,--“a1”
    --     Param_5 = -1,--“a1”
    --     Duration =1,
    -- },
    Param_5 = -1,                              
    Param_6 = -1,                            
    Duration =-1,   
}


local sk_lowhp = sk_mk{
    H_1 = h_mk{                                          
      TargetType = 2,
      IsAnimHit = 0,
        I_1 = {
            Impact =  i_mk{
            Id= 1743002,                           --人界标签，作用是用来互斥   
            ImpactLogic = -1,    
            MutexID = 1733,
            MutexPriority = 1,
        },
    },
},
        EnvLimit = 0,
        OtherID = sk_mk{
            H_1 = h_mk{
                TargetType = 2,
                I_1 ={  
                    Impact =  i_lowhpchange,      
                },
            EnvLimit = 1,   
        },
    }
}


local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 2,
        I_1 = {
            Impact =  i_mk{
                ImpactLogic = 24,                        
                ImpactClass= 0,                           
                Param_1 = sk_lowhp,                     --低血换成被动不能释放   自己回合开始
                Param_2 = 1, 
                Duration =-1,
            },
        },
    },
    H_2 = h_mk{
        TargetType = 2,
        I_1 = {
            Impact = i_mk{
                ImpactLogic = 24,                        
                ImpactClass= 0,                           
                Param_1 = sk_check1,                     --满足3个桃子就换复活
                Param_2 = 1, 
                Duration =-1,
            },
        },
    },
    H_3 = h_mk{
    TargetType = 2,
        I_1 = {
            Impact =  i_mk{
                ImpactLogic = 24,                        
                ImpactClass= 0,                           
                Param_1 = sk_trytry,                     --清标签和表现
                Param_2 = 1, 
                Duration =-1,
                MutexID = 1732,
                MutexPriority = 1,
            },
        },
    },
    H_4 = h_mk{
        TargetType = 2,
        I_1 = {
            Impact = i_mk{
                ImpactLogic = 24,                        
                ImpactClass= 0,                           
                Param_1 = sk_check2,                     --行动前检测是否有低血标签，然后换技能
                Param_2 = 1, 
                Duration =-1,
            },
        },
    },
    H_5 = h_mk{
        TargetType = 2,
        I_1 = {
            Impact = i_mk{
                ImpactLogic = 24,                        
                ImpactClass= 0,                           
                Param_1 = sk_lowhp,                     --阴阳界切换后，在阴界把检测先加上，这是一个辅助，不然第一次切换阴界时因优先级原因，不会检测
                Param_2 = 12, 
                Duration =-1,
            },
        },
    }
}
return sk_main


