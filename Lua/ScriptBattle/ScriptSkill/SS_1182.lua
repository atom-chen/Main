
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--鲛3技能：【被动】当我方其他队友（除自身）受到攻击时，以25%的概率为自身增加15%的行动条（此效果在一个行动回合内只能获得一次）。
--【妖界效果】为我方血量最少的单位增加相当于自身生命值最大值15%的护盾。


--拉条
local i_actionbar = i_mk{
    MutexID=1183,
    MutexPriority=1,

    RoundMaxEffectedCount=1,

    Duration = 0,

    ImpactLogic =6,                 --行动条增加减少           

    Param_1= "a1",                  --增减数（负数表示减少）
   

}

--护盾
local i_hudun = i_mk{
    Id=1183111 ,
    Duration = -1,
    IsShow=1,
    MutexID = 1184,
    MutexPriority =1,
    ImpactClass = 1,
    ImpactSubClass = 2048, 

    
    ImpactLogic = 9,                        --护盾（受到伤害时，优先扣护盾的数值，扣完后，buff移除）
    Param_1 = "a2",                            --被加护盾符灵的血量上限百分比
    Param_2 = -1,                            -- -1取接受者血量上限，0取施法者血量上限
}

local sk_actionbar = sk_mk{

    H_1=h_mk{
           TargetType=2,
            I_1={                       --拉条
                Impact =i_actionbar,
            }
    },
    EnvLimit = 0,
    IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放
    OtherID = sk_mk{
        H_1=h_mk{
            TargetType=2,
             I_1={                       --拉条
                 Impact =i_actionbar,
             }
         },
         
     H_2 = h_mk{
             IsAnimHit=0,
             TargetType = 9,
             I_1 ={
             Impact =  i_hudun,               --护盾
             }
     },
        EnvLimit = 1, 
        IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放  
    },
        
} 
   

--受击释放技能
local i_shouji = i_mk{
  
    Duration = -1,
    MutexID = 1182,
    MutexPriority = 1,
    RoundMaxEffectedCount=1,
    IsPassiveImpact=1,
    ImpactLogic = 17,                      --受到伤害后，会概率使用技能
    Param_1 = "a3",                        --概率
    Param_2 = sk_actionbar,                  --技能id
    Param_3 =2,                             --1自己，2队友，3任意我方
    Param_4 =0,                             --立即使用（0等正在行动的人结束后使用，1立即使用）（一般有动画表现的，最好0，没动画表现的1）
    Param_5 =2,                             -- 技能目标（1攻击者，2自己）(最好是hit里直接使用特殊目标，技能目标只是辅助用)
}



local sk_main = sk_mk{

    

 
    H_1 = h_mk{
        TargetType = 2,
        I_1 ={
            Impact =  i_shouji,               --受击释放技能
        },
    },
    IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放

}

   
return sk_main