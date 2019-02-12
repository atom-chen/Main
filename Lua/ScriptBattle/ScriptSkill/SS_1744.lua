local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")




-- -- --没有桃子的时候回合开始
-- local i_begin0 = i_mk{
--     Duration = 0,
--     ImpactLogic = 10, 
--     IsShow = 0, 
--     Param_1= 1,
--     Param_2= i_change1,
--     Param_3= "a1",
--     Param_4= i_change2,
--     Param_5= "a2",
-- }

-- local i_begin1 = i_mk{
--     Duration = 0,
--     ImpactLogic = 10, 
--     IsShow = 0, 
--     Param_1= 1,
--     Param_2= i_change1,
--     Param_3= "a3",
--     Param_4= i_change2,
--     Param_5= "a4",
-- }

-- local i_begin2 = i_mk{
--     Duration = 0,
--     ImpactLogic = 10, 
--     IsShow = 0, 
--     Param_1= 1,
--     Param_2= i_change1,
--     Param_3= "a5",
--     Param_4= i_change2,
--     Param_5= "a6",
-- }

-- local i_begin3 = i_mk{
--     Duration = 0,
--     ImpactLogic = 10,
--     IsShow = 0, 
--     Param_1= 1,
--     Param_2= i_change1,
--     Param_3= "a7",
--     Param_4= i_change2,
--     Param_5= "a8",
-- }


----换技能-----------------------------------------------换技能------------------------------------------------------

local i_change0 = i_mk{
    ImpactLogic = 36,                        
    ImpactClass= 0,                           
    Param_1 = "a2",                            
    Param_2 = -1,
    Param_3 = -1,
    Param_4 = -1,
    Param_5 = -1,
    Duration =-1,
    MutexID = 1735,
    MutexPriority = 1,
    IsShow = 1,
}

local i_change1 = i_mk{
    ImpactLogic = 36,                        
    ImpactClass= 0,                           
    Param_1 = "a3",                            
    Param_2 = -1,
    Param_3 = -1,
    Param_4 = -1,
    Param_5 = -1,
    Duration =-1,
    MutexID = 1735,
    MutexPriority = 1,
    IsShow = 1,
}

local i_change2 = i_mk{
    ImpactLogic = 36,                        
    ImpactClass= 0,                           
    Param_1 = "a4",                            
    Param_2 = -1,
    Param_3 = -1,
    Param_4 = -1,
    Param_5 = -1,
    Duration =-1,
    MutexID = 1735,
    MutexPriority = 1,
    IsShow = 1,
}

local i_change3 = i_mk{
    ImpactLogic = 36,                        
    ImpactClass= 0,                           
    Param_1 = "a4",                            
    Param_2 = -1,
    Param_3 = -1,
    Param_4 = -1,
    Param_5 = -1,
    Duration =-1,
    MutexID = 1735,
    MutexPriority = 1,
    IsShow = 1,
}
--检测数量走技能-------------------------------------------根据桃子数量挂上面的“回合开始时释放”-----------------------------------------------
local sk_check = sk_mk{                
    H_1 = h_mk{
        TargetType=2,
        I_1 = {
            Impact = i_mk{
                Duration = 0,
                ImpactLogic = 29, 
                Param_1= 0,
                Param_2= 1731011,
                Param_3= 0,
                Param_4= i_change0,
            },
        },
        I_2 = {
            Impact = i_mk{
                Duration = 0,
                ImpactLogic = 29, 
                Param_1= 0,
                Param_2= 1731011,
                Param_3= 1,
                Param_4= i_change1,
            },
        },
        I_3 = {
            Impact = i_mk{
                Duration = 0,
                ImpactLogic = 29, 
                Param_1= 0,
                Param_2= 1731011,
                Param_3= 2,
                Param_4= i_change2,
            },
        },
        I_4 = {
            Impact = i_mk{
                Duration = 0,
                ImpactLogic = 29, 
                Param_1= 0,
                Param_2= 1731011,
                Param_3= 3,
                Param_4= i_change3,
            },
        },
    }
}



-- --回合开始根据桃子数量走

local sk_main = sk_mk{            
        H_1 = h_mk{
            TargetType=1,
            I_1 = {                      
            Impact =  i_mk{
                ImpactLogic = 24,                        
                ImpactClass= 0,                           
                Param_1 = sk_check,                     --走根据不同桃子数量用不同概率的换技能ID
                Param_2 = 1, 
                Duration =-1,
            }
        }
    }
}

return sk_main