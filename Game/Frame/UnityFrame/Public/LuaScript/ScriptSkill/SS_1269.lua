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
    ImpactLogic = 36,             --技能替换,激活时,替换指定的技能为新技能          
    Param_3 = "a1",             --技能3替换id,不替换配-1
}


--换治疗+攻爆毒
local i_recovery = i_mk{
    Duration=1,
    AliveCheckType = 2,
    ImpactLogic = 36,           --技能替换,激活时,替换指定的技能为新技能          
    Param_3 = "a2",            --技能3替换id,不替换配-1
    
             
}

--换拉条+攻爆毒
local i_buff = i_mk{
    Duration=1,
    AliveCheckType=2,
    ImpactLogic = 36,           --技能替换,激活时,替换指定的技能为新技能          
    Param_3 = "a3",            --技能3替换id,不替换配-1
}


local sk_main = sk_mk{
    
    H_1 = h_mk{
        TargetType = 2,
        I_1 = {
            Impact = i_mk{
                Duration = -1,
                ImpactLogic = 24,               --定期触发使用技能  
                Param_1 = sk_mk{                --技能id
                    H_1 = h_mk{
                        TargetType=2,
                        I_1 = {
                            Chance = 10000,
                            Impact =i_actionbar,
                        },

            
                        I_2 = {
                            Chance = 5000,
                            Impact = i_buff,
                        },  

                        I_3 = {
                            Chance = 3333,
                            Impact = i_recovery,
                        }
                    
                    }
                
                },
                Param_2= 1,                         --周期类型--1,自己回合开始；
            }  
        }
    }   
}

return sk_main