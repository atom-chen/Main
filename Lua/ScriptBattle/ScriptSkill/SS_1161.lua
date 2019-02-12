local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


--眦每次受到伤害时，攻击力上升10%，最多积累5次，且积累的额外攻击力会在睚眦下一次造成伤害后归0重新计算；睚眦在死亡会对给予他致命一击的伤害来源造成相当于自身生命值30%的固定伤害


local i_attackup = i_mk{
    Duration=-1,
    AliveCheckType=1,
    AutoFadeOutTag=4,
    LayerID=1161,
    LayerMax=5,
    IsPassiveImpact = 1,
    ImpactLogic = 4,               
    Param_1 = 2,                    
    Param_2 = 0,                    
    Param_3 = 1000,

}

local sk_assist1 = sk_mk{
    H_1 = h_mk{

        TargetType = 2,
        I_1 ={
            
            Impact = i_attackup,             
        },   
    }
   
}

local i_hurtedatkup  = i_mk{
    Duration= -1,
    AliveCheckType=1,
   

    ImpactLogic = 17,              --受到伤害后，会概率使用技能

    Param_1 = 10000,               -- 概率
    Param_2 = sk_assist1,                  -- 技能id
    Param_3 = 1,                 -- 1自己，2队友，3任意我方
    Param_4 = 1,                 -- 立即使用（0等正在行动的人结束后使用，1立即使用）（一般有动画表现的，最好0，没动画表现的1）
    Param_5 = 2,                 -- 技能目标（1攻击者，2自己）(最好是hit里直接使用特殊目标，技能目标只是辅助用)
}
---------------------------------------------------------------
local i_siwangda = i_mk{
    Duration=0,
    AliveCheckType=1,
    
    ImpactLogic = 23,               
    Param_1 = 3000,                    


}

local sk_assist2 = sk_mk{
    H_1 = h_mk{

        TargetType = 1,
        I_1 ={
            
            Impact = i_siwangda,             
        },   
    }
   
}

local i_siwangfanji = i_mk{
    Duration=-1,
    AliveCheckType=1,
  
    ImpactLogic = 22,               
    Param_1 = sk_assist2,                    
   

}
-------------------------------------------------------------------------------

local i_biaoji  = i_mk{
    Id=  1162210,
    Duration= 1,
    AliveCheckType=1,
    AutoFadeOutTag=2,
    MutexID=1162,
    MutexPriority=1,

}

local sk_assist3 = sk_mk{
    H_1 = h_mk{

        TargetType = 1,
        I_1 ={
            
            Impact = i_biaoji,             
        },   
    }
   
}



local i_guaskill3beidong = i_mk{
    Duration= -1,
    AliveCheckType=1,
  
   
    ImpactLogic = 17,              --受到伤害后，会概率使用技能

    Param_1 = 10000,               -- 概率
    Param_2 = sk_assist3,                  -- 技能id
    Param_3 = 1,                 -- 1自己，2队友，3任意我方
    Param_4 = 1,                 -- 立即使用（0等正在行动的人结束后使用，1立即使用）（一般有动画表现的，最好0，没动画表现的1）
    Param_5 = 1,                 -- 技能目标（1攻击者，2自己）(最好是hit里直接使用特殊目标，技能目标只是辅助用)

}





local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 2,
        I_1 = {
            Impact = i_hurtedatkup,
        },
        I_2 = {                       
          
            Impact = i_siwangfanji,
        },
    },
    H_2 = h_mk{
        TargetType = 1,
        I_1 = {
            Impact = i_guaskill3beidong,
        },
    },
}
return sk_main


