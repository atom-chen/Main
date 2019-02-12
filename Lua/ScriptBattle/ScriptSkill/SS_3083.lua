
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--四鸾衔绶镜2技能觉醒【被动】四鸾护体，四鸾衔绶纹镜受到的每次伤害，不能超过四鸾衔绶纹镜生命上限的1/4。【觉醒效果】每损失1/4血量，伤害提升15%

local i_shangxian  = i_mk{
    Duration= -1,
    AliveCheckType=1,
    IsPassiveImpact = 1, 

    ImpactLogic = 53,             

    Param_1 = 0,               
    Param_2 = 2500,                  
}
---------------------------------------------------------------------------------
local i_attackup = i_mk{

    Duration = -1,
    AliveCheckType=2,
    AutoFadeOutTag=0,

    ImpactLogic = 4,               
    Param_1 = 6,                    
    Param_2 = 1500,                    
    Param_3 = 0,

}

local i_sunshiHP1  = i_mk{         
    Duration = -1,
    IsPassiveImpact = 1, 
  
    ImpactLogic = 28,
  
    Param_1 =0,
    Param_2 =7501,
    Param_3 = 1,
    Param_4 = i_attackup,
    Param_5 = -1,
    Param_6 = -1,
    
}

local i_sunshiHP2  = i_mk{         
    Duration = -1,
    IsPassiveImpact = 1, 

    ImpactLogic = 28,
  
    Param_1 =0,
    Param_2 =5001,
    Param_3 = 1,
    Param_4 = i_attackup,
    Param_5 = -1,
    Param_6 = -1,
    
}

local i_sunshiHP3  = i_mk{         
    Duration = -1,
    IsPassiveImpact = 1, 
 
    ImpactLogic = 28,
  
    Param_1 =0,
    Param_2 =2501,
    Param_3 = 1,
    Param_4 = i_attackup,
    Param_5 = -1,
    Param_6 = -1,
    
}


local sk_main = sk_mk{


 
    H_1 = h_mk{                         --伤害
        TargetType = 2,
        I_1 = {
            Impact = i_shangxian,
        },
        I_2 = {
            Impact = i_sunshiHP1,
        },
        I_3 = {
            Impact = i_sunshiHP2,
        },
        I_4 = {
            Impact = i_sunshiHP3,
        },

    },

   
}

return sk_main