
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--猫将军2技能 攻击单体敌人，【人界】攻击带有燃烧状态的敌人时，获得吸血效果

local i_damage  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}


--吸血
local i_xixue = i_mk{
    Duration=1,
    AliveCheckType=2,
    AutoFadeOutTag=8,
    MutexID=1841,
    MutexPriority=1,
    ImpactLogic =4,                      

    Param_1= 101, 
    Param_2= 4000,               
    Param_3= 0,
}



local sk_main = sk_mk{

    H_1 = h_mk{
        IsAnimHit=0,
        TargetType = 2,
        I_1 ={
            Impact = i_xixue,               
        },   
    },
        
    H_2 = h_mk{
        TargetType=1,
       
        I_1 = {
            Impact =  i_damage,
        }
     
    },


   
}

return sk_main





