
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--【被动】句芒回合结束时，会生成一个相当于自身生命最大值10%的护盾，在句芒下回合行动时，如果护盾没有被打破，
--【人界效果】永久提升自身20%防御，[A78C84]【妖界效果】永久提升自身20%攻击。[-]属性最大叠加5次。

--护盾
local i_hudun = i_mk{
    Id= 1484010,

    Duration = 1,
    IsShow = 1,
    ImpactClass = 1,
    ImpactSubClass = 2048, 
    
    ImpactLogic = 9,                        --护盾（受到伤害时，优先扣护盾的数值，扣完后，buff移除）
    Param_1 = "a1",                         --被加护盾符灵的血量上限百分比

}

--提升属性
local i_tisheng1 = i_mk{
    Duration= -1,
    IsShow= 1,
    LayerID=1483,
    LayerMax=3,
    
    ChildImpact= i_mk{
        Id =1483011,
        MutexID = 1480,
        MutexPriority=1,
    },
    
    ImpactLogic =4,                      

    Param_1= 3, 
    Param_2= 0,               
    Param_3= 3500,
}

local i_tisheng2 = i_mk{
    Duration= -1,
    IsShow= 1,
    LayerID=1484,
    LayerMax=3,
    
    ChildImpact= i_mk{
        Id =1483111,
        MutexID = 1481,
        MutexPriority=1,
    },
    ImpactLogic =4,                      

    Param_1= 2, 
    Param_2= 0,               
    Param_3= 3500,
}


local sk_hudun = sk_mk{
    H_1 = h_mk{
        TargetType = 2,
        I_1 ={
            
            Impact = i_hudun,               --释放护盾
        },   
    },
    IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放
   
}

local sk_tisheng = sk_mk{
    H_1 = h_mk{
        TargetType = 2,
        I_1 ={
            
            Impact = i_tisheng1,               --提升属性
        },   
    },
    EnvLimit = 0,
    IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放
    OtherID = sk_mk{
    H_1=h_mk{
        TargetType=2,
        I_1={                       
            Impact =i_tisheng2,
        }
    },
    EnvLimit = 1,
    IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放
    },   
}

local  i_UseSkillRoundOver = i_mk{       -- 回合结束后释放技能
    Duration= -1,
    IsShow= 1,
    AliveCheckType=2,
    MutexID=120,
    MutexPriority=1,
    IsPassiveImpact=1,

    ImpactLogic = 24,                --定期触发使用技能

    Param_1 = sk_hudun,             -- 技能id
    Param_2 = 3,                    -- 周期类型（见24号逻辑）
    Param_3 = -1,                   -- 条件类型（见24号逻辑）
    Param_4 = -1,                   -- 条件参数
    Param_5 = -1,                   -- 条件参数
    Param_6 = -1,                   -- 是否立即生效（回合开始时用的技能，必须时立即释放，这个参数无用，回合结束使用技能，必须时顺序释放，这个参数无用）
    Param_7 = -1,                   -- 技能类型（-1，skillExId；1，skillIndex）
    Param_8 = -1,                   -- 周期参数
    Param_9 = -1,                   -- 周期参数

}

--回合开始检查护盾
local i_jiachahudun = i_mk{
    Duration= -1,
    IsPassiveImpact=1,

    ImpactLogic =46,                --回合开始时，若指定impact存在，则触发技能，技能目标自己                   

    Param_1= 1484010,               --impact的id
    Param_2= sk_tisheng,            --技能id
    Param_3=  1,                    --立即释放(1,立即；其他顺序)
}


local sk_main = sk_mk{

    H_1 = h_mk{                 
        TargetType = 2,
   
        I_1 = {
            Impact = i_jiachahudun,
        },

        I_2 = {
            Impact = i_UseSkillRoundOver,
        },
    },

    IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放


   
}

return sk_main