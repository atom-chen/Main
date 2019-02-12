
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


local i_colddowndown = i_mk{
    Duration=0,

    ImpactLogic =35,                 --增减CD，有CD的技能，才会受这个效果影响         

    Param_1= 2,                      --技能索引（-1表示全部技能(1~3)）
    Param_2= -5,                   --增减值(大于0增加CD，小于0减少CD)

}


local sk_assist = sk_mk{

    H_1 = h_mk{
        IsAnimHit=0,
        TargetType = 2,
        I_1 ={
            Impact = i_colddowndown,               --减少技能冷却
        },   
    }


   
}

local i_nextattack =i_mk{
    MutexID=1842,
    MutexPriority=1,
    ImpactLogic = 14,        --使用技能时，条件额外使用技能

    Param_1 = 4,             --skilltype，1，普攻，2，任意主动，3指定class，4指定id
    Param_2 = "a2",          --技能参数
    Param_3 = 10000,         --概率
    Param_4 = -1,            --额外使用的技能索引（该角色的第几个技能）
    Param_5 = sk_assist,     --额外使用的技能（skillEx表id，参数4必须填-1才生效）
    Param_6 = 2,             --条件检查时机（1，使用技能前，2，使用技能后）
    Param_7 = -1,            --不能切换目标（0，否（目标死亡后会随机选择目标），1是（目标死亡后追加不能触发））     
    Param_8 = 2,             --条件类型（见17号逻辑）
    Param_9 = 1,             --条件参数1
    Param_10 = 911001,       --条件参数2
    RoundMaxEffectedCount=1,
    Duration = -1,
    AliveCheckType = 2,
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

    H_3 = h_mk{

        TargetType=2,
       
        I_1 = {
            Impact =  i_nextattack,
        }
     
    },
 


   
}

return sk_main





