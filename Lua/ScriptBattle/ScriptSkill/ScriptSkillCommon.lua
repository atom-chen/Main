require("BattleCore/SkillProcess/ScriptSkillParser")

local t = {
    
    --冷却Id全局维护
    CoolDownId = {
    },

    --互斥Id，全局维护
    MutexId = {

    },

    --互斥优先级，全局维护
    MutexPriority = {

    },

    --常用Class
    Class = {
        --Debuff = 4,
    },

    --常用SubClass
    SubClass = {
        
    },

    --常用Impact Id，表现用
    CommonImpactId = {
        -- Stun = 1000,
        -- Ice = 1001,
        -- Fire = 1002,
    },

    --常用Buff类Impact，使用时记得加i_mk()
    CommonBuffs = {

        Stun = {                  --通用眩晕
            Id = 900001,
            ImpactLogic = -1,

            Duration = 1,
            AliveCheckType = 3,
            AutoFadeOutTag = 0,
            ImpactClass = 4,
            ImpactSubClass = 1, 
            MutexID = 1,
            MutexPriority = 5,
        },
        Frozen={                  --通用冰冻
            Id = 901001,
            ImpactLogic = -1,

            Duration = 1,
            AliveCheckType = 3,
            AutoFadeOutTag = 0,
            ImpactClass = 4,
            ImpactSubClass = 256, 
            MutexID = 1,
            MutexPriority = 4,
        },

        DefenceReduce ={          --通用防御降低 
            Id = 902001,
            ImpactLogic = 4,
            Param_1 = 3,
            Param_2 = 0,
            Param_3 = -7000,

            Duration = 1,
            AliveCheckType = 3,
            AutoFadeOutTag = 0,
            ImpactClass = 4,
            ImpactSubClass = 0, 
            MutexID = 902,
            MutexPriority = 1,
        },

        AttackReduce ={          --通用攻击降低 
            Id = 903001,
            ImpactLogic = 4,
            Param_1 = 2,
            Param_2 = 0,
            Param_3 = -5000,

            Duration = 1,
            AliveCheckType = 3,
            AutoFadeOutTag = 0,
            ImpactClass = 4,
            ImpactSubClass = 0, 
            MutexID = 903,
            MutexPriority = 1,
        },

        Poison = {               --通用持续伤害
            Id = 904001,
            ImpactLogic = 34,
            Param_1 = 500,
           
            Duration = 1,
            AliveCheckType = 1,
            AutoFadeOutTag = 0,
            ImpactClass = 4,
            ImpactSubClass = 64, 
           
        },

        SpeedReduce = {          --通用减速
            Id = 905001,
            ImpactLogic = 4,
            Param_1 = 1,
            Param_2 = 0,
            Param_3 = -3000,

            Duration = 1,
            AliveCheckType = 3,
            AutoFadeOutTag = 0,
            ImpactClass = 4,
            ImpactSubClass = 1024, 
            MutexID = 905,
            MutexPriority = 1,
        },

        Sleep = {               --通用沉睡
            Id = 906001,
            ImpactLogic = -1,
            
            Duration = 1,
            AliveCheckType = 3,
            AutoFadeOutTag = 0,
            ImpactClass = 4,
            ImpactSubClass = 512, 
            MutexID = 1,
            MutexPriority =3,
        },


        CureProhibit = {           --通用禁疗
            Id = 907001,
            ImpactLogic = 4,
            Param_1 = 103,
            Param_2 = 1,
            Param_3 = 0,
            
            Duration = 1,
            AliveCheckType = 3,
            AutoFadeOutTag = 0,
            ImpactClass = 4,
            ImpactSubClass = 0, 
            MutexID = 907,
            MutexPriority =1,
        },


        Defiance = {              --通用嘲讽
            Id = 908001,
            ImpactLogic = -1,

            Duration = 1,
            AliveCheckType = 3,
            AutoFadeOutTag = 2,
            ImpactClass = 4,
            ImpactSubClass = 4, 
            MutexID = 1,
            MutexPriority =2,
        },

        Weak = {                   --通用虚弱
            Id = 909001,
            ImpactLogic = 7,
            Param_1 = 1,
            Param_2 = 0,
               
            Duration = 1,
            AliveCheckType = 3,
            AutoFadeOutTag = 0,
            ImpactClass = 4,
            ImpactSubClass = 0, 
            MutexID = 909,
            MutexPriority =1,
        },
        
        Silent = {                   --通用沉默
            Id = 910001,
            ImpactLogic = -1,
         
            Duration = 1,
            AliveCheckType = 3,
            AutoFadeOutTag = 0,
            ImpactClass = 4,
            ImpactSubClass = 2, 
            MutexID = 2,
            MutexPriority =1,
        },

        Burning = {                  --通用灼伤
            Id = 911001,
            ImpactLogic = 4,
            Param_1 = 7,
            Param_2 = -2500,
            Param_3 = 0,
            
            Duration = 1,
            AliveCheckType = 3,
            AutoFadeOutTag = 0,
            ImpactClass = 4,
            ImpactSubClass = 128, 
            MutexID = 911,
            MutexPriority =1,
        },

        Disablepassive = {                  --通用封印
            Id = 912001,
            ImpactLogic = 65,
         
        
            Duration = 1,
            AliveCheckType = 3,
            AutoFadeOutTag = 0,
            ImpactClass = 4,
            ImpactSubClass = 0, 
            MutexID = 912,
            MutexPriority =1,
        },



        AttackEnhance = {                   --通用攻击强化
            Id = 913001,
            ImpactLogic = 4,
            Param_1 = 2,
            Param_2 = 0,
            Param_3 = 5000,
            
            Duration = 1,
            AliveCheckType = 3,
            AutoFadeOutTag = 0,
            ImpactClass = 1,
            ImpactSubClass = 0, 
            MutexID = 913,
            MutexPriority =1,
        },
        SpeedEnhance = {                   --通用速度强化
            Id = 914001,
            ImpactLogic = 4,
            Param_1 = 1,
            Param_2 = 0,
            Param_3 = 3000,
            
            Duration = 1,
            AliveCheckType = 3,
            AutoFadeOutTag = 0,
            ImpactClass = 1,
            ImpactSubClass = 0, 
            MutexID = 914,
            MutexPriority =1,
        },
        DefenceEnhance = {                   --通用防御强化
            Id = 915001,
            ImpactLogic = 4,
            Param_1 = 3,
            Param_2 = 0,
            Param_3 = 7000,
            
            Duration = 1,
            AliveCheckType = 3,
            AutoFadeOutTag = 0,
            ImpactClass = 1,
            ImpactSubClass = 0, 
            MutexID = 915,
            MutexPriority =1,
        },

        Immune = {                             --通用免疫
            Id = 916001,
            ImpactLogic = 7,
            Param_1 = 4,
            Param_2 = 0,
    
            Duration = 1,
            AliveCheckType = 3,
            AutoFadeOutTag = 0,
            ImpactClass = 1,
            ImpactSubClass = 0, 
            MutexID = 916,
            MutexPriority =1,
        },

        CriticalEnhance = {                     --通用狂暴
            Id = 917001,
            ImpactLogic = 4,
            Param_1 = 4,
            Param_2 = 3000,
            Param_3 = 0,

            Duration = 1,
            AliveCheckType = 3,
            AutoFadeOutTag = 0,
            ImpactClass = 1,
            ImpactSubClass = 8192, 
            MutexID = 917,
            MutexPriority =1,
        },

        Cure = {                                --通用恢复
            Id = 918001,
            ImpactLogic = 1,
            Param_1 = 3,
            Param_2 = 1500,
            

            Duration = 1,
            AliveCheckType = 3,
            AutoFadeOutTag = 0,
            ImpactClass = 1,
            ImpactSubClass = 0, 
            MutexID = 918,
            MutexPriority =1,
        },
        Invincible = {                            --通用无敌
            Id = 919001,
            ImpactLogic = 8,
            Param_1 = -1,
        
            Duration = 1,
            AliveCheckType = 3,
            AutoFadeOutTag = 0,
            ImpactClass = 1,
            ImpactSubClass = 0, 
            MutexID = 919,
            MutexPriority =1,
        },
        CritChanceResist = {                            --通用强韧
            Id = 920001,
            ImpactLogic = 4,
            Param_1 = 106,
            Param_2 = 3000,
        
            Duration = 1,
            AliveCheckType = 3,
            AutoFadeOutTag = 0,
            ImpactClass = 1,
            ImpactSubClass = 0, 
            MutexID = 920,
            MutexPriority =1,
        },
        Guard = {                            --通用神佑
            Id = 921001,
            ImpactLogic = 13,
            Param_1 = 1,
            Param_2 = 3000,
        
            Duration = 1,
            AliveCheckType = 3,
            AutoFadeOutTag = 0,
            ImpactClass = 1,
            ImpactSubClass = 0, 
            MutexID = 921,
            MutexPriority =1,
        },
    
        Bramble = {                            --通用荆棘
            Id = 923001,
            ImpactLogic = 4,
            Param_1 = 100,
            Param_2 = 3000,
            Param_3 = 0,
        
            Duration = 1,
            AliveCheckType = 3,
            AutoFadeOutTag = 0,
            ImpactClass = 1,
            ImpactSubClass = 0, 
            MutexID = 923,
            MutexPriority =1,
        },

    },

    --常用Skill，使用时记得加sk_mk()
    CommonSkills = {

    },
}

return t