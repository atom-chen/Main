local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")





--换治疗+拉条
local i_actionbar = i_mk{                   
    Duration= 1,
    AliveCheckType= 2,
    MutexID=1269,
    MutexPriority=1,

    ImpactLogic = 36,             --技能替换,激活时,替换指定的技能为新技能          
    Param_3 = "a1",             --技能3替换id,不替换配-1
}


--换治疗+攻爆毒
local i_recovery = i_mk{
    Duration=1,
    AliveCheckType = 2,
    MutexID=1269,
    MutexPriority=1,

    ImpactLogic = 36,           --技能替换,激活时,替换指定的技能为新技能          
    Param_3 = "a2",            --技能3替换id,不替换配-1
    
             
}

--换拉条+攻爆毒
local i_buff = i_mk{
    Duration=1,
    AliveCheckType=2,
    MutexID=1269,
    MutexPriority=1,
    ImpactLogic = 36,           --技能替换,激活时,替换指定的技能为新技能          
    Param_3 = "a3",            --技能3替换id,不替换配-1
}

--换三合一
local i_sange = i_mk{
    Duration=1,
    AliveCheckType=2,
    MutexID=1269,
    MutexPriority=1,
    ImpactLogic = 36,           --技能替换,激活时,替换指定的技能为新技能          
    Param_3 = "a4",            --技能3替换id,不替换配-1
}

local i_suiji1 = i_mk{
    Duration=1,
    MutexID=1268,
    MutexPriority=1,

    ImpactLogic = 10,            
    Param_1 = 1,
    Param_2 = i_actionbar, 
    Param_3 = 10000, 
    Param_4 = i_recovery, 
    Param_5 = 10000, 
    Param_6 = i_buff, 
    Param_7 = 10000,  
          
}


local i_suiji2 = i_mk{
    Duration=1,
    MutexID=1268,
    MutexPriority=1,

    ImpactLogic = 10,            
    Param_1 = 1,
    Param_2 = i_suiji1, 
    Param_3 = "a5", 
    Param_4 = i_sange, 
    Param_5 = "a6", 
           
}

local sk_main = sk_mk{
    

    H_1=h_mk{

        TargetType=2,
        I_1 = {
            Impact = i_mk{
                Duration = -1,
                ImpactLogic = 24,               --定期触发使用技能  
                Param_1 = sk_mk{                --技能id
                    H_1 = h_mk{
                        TargetType=2,
                        I_1 = {
                            Impact = i_suiji2,
                        },
                                    
                    }
                },
                Param_2= 2,                         --周期类型--2,波次开始；
            } 
        }, 
        I_2 = {
            Impact = i_mk{
                Duration = -1,
                ImpactLogic = 24,               --定期触发使用技能  
                Param_1 = sk_mk{                --技能id
                    H_1 = h_mk{
                        TargetType=2,
                        I_1 = {
                            Impact = i_suiji2,
                        },
                                    
                    }
                },
                Param_2= 1,                         --周期类型--1,自己回合开始；
            } 
        }, 
    },
  
}

return sk_main