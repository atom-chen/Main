
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--钟馗1技能：普攻+层数叠加伤害变高+溅射

local i_damage  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --钟馗普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
    
}

--溅射
local i_jianshe  = i_mk{
    ImpactClass=2,
    ImpactLogic = 59,              --伤害溅射，产生伤害后，按照比例溅射给敌方2其他人

    Param_1 = "a2",         -- 溅射伤害比例
    Param_2 = -1,                  -- 暴击才触发
    Duration = 1,
    AliveCheckType=2,
    AutoFadeOutTag=16,
}

--清标记
local i_qingbiaoji  = i_mk{
    ImpactLogic = 25,              --根据id驱散buff

    Param_1 = 1013011 ,          -- 根据id驱散buff

    Duration = 0,

}


----------------------------------------------------1层---------------------------------------------------------------
--1层攻击力提升
local i_attackup1 = i_mk{
    ImpactLogic = 4,               
    MutexID=1424,
    MutexPriority=1,
    Param_1 = 6,                    
    Param_2 = 2000,                    
    Param_3 = 0,
    Duration = -1,
    AliveCheckType=1,
    AutoFadeOutTag=8,
    
}




--根据层数修正伤害(1层)
local i_attackmodify1 = i_mk{
    ImpactLogic = 29,               --激活时检测，如果接受者指定buff存在超过指定数量时，触发一个额外的效果
    MutexID=1425,
    MutexPriority=1,
    Param_1 = 0,                    --类型（0，id；1，class；2，subClass）
    Param_2 = 1013011,                    --参数(根据类型区分具体意义)
    Param_3 = 1,                    --数量
    Param_4 = i_attackup1 ,          --额外的效果impact id
    Duration = 0,
   
}
----------------------------------------------------2层---------------------------------------------------------------

--2层攻击力提升
local i_attackup2 = i_mk{
    ImpactLogic = 4,               
    MutexID=1424,
    MutexPriority=1,
    Param_1 = 6,                    
    Param_2 = 4000,                    
    Param_3 = 0,
    Duration = -1,
    AliveCheckType=1,
    AutoFadeOutTag=8,
}



--根据层数修正伤害(2层)
local i_attackmodify2 = i_mk{
    ImpactLogic = 29,               --激活时检测，如果接受者指定buff存在超过指定数量时，触发一个额外的效果
    MutexID=1425,
    MutexPriority=1,
    Param_1 = 0,                    --类型（0，id；1，class；2，subClass）
    Param_2 = 1013011,                    --参数(根据类型区分具体意义)
    Param_3 = 2,                    --数量
    Param_4 = i_attackup2 ,          --额外的效果impact id
    Duration = 0,

}

----------------------------------------------------3层---------------------------------------------------------------
--3层攻击力提升
local i_attackup3 = i_mk{
    ImpactLogic = 4,               
    MutexID=1424,
    MutexPriority=1,
    Param_1 = 6,                    
    Param_2 = 6000,                    
    Param_3 = 0,
    Duration = -1,
    AliveCheckType=1,
    AutoFadeOutTag=8,
}



--根据层数修正伤害(3层)
local i_attackmodify3 = i_mk{
    ImpactLogic = 29,               --激活时检测，如果接受者指定buff存在超过指定数量时，触发一个额外的效果
    MutexID=1425,
    MutexPriority=1,
    Param_1 = 0,                    --类型（0，id；1，class；2，subClass）
    Param_2 = 1013011,                    --参数(根据类型区分具体意义)
    Param_3 = 3,                    --数量
    Param_4 = i_attackup3 ,          --额外的效果impact id
    Duration = 0,
}

----------------------------------------------------4层---------------------------------------------------------------



--4层攻击力提升
local i_attackup4 = i_mk{
    ImpactLogic = 4,               
    MutexID=1424,
    MutexPriority=1,
    Param_1 = 6,                    
    Param_2 = 8000,                    
    Param_3 = 0,
    Duration = -1,
    AliveCheckType=1,
    AutoFadeOutTag=8,
}



--根据层数修正伤害(4层)
local i_attackmodify4 = i_mk{
    ImpactLogic = 29,               --激活时检测，如果接受者指定buff存在超过指定数量时，触发一个额外的效果
    MutexID=1425,
    MutexPriority=1,
    Param_1 = 0,                    --类型（0，id；1，class；2，subClass）
    Param_2 = 1013011,                    --参数(根据类型区分具体意义)
    Param_3 = 4,                    --数量
    Param_4 = i_attackup4 ,          --额外的效果impact id
    Duration = 0,
}



----------------------------------------------------5层---------------------------------------------------------------

--5层攻击力提升
local i_attackup5 = i_mk{
    ImpactLogic = 4,               
    MutexID=1424,
    MutexPriority=1,
    Param_1 = 6,                    
    Param_2 = 10000,                    
    Param_3 = 0,
    Duration = -1,
    AliveCheckType=1,
    AutoFadeOutTag=8,
}




--根据层数修正伤害(5层)
local i_attackmodify5 = i_mk{
    ImpactLogic = 29,               --激活时检测，如果接受者指定buff存在超过指定数量时，触发一个额外的效果
    MutexID=1425,
    MutexPriority=1,
    Param_1 = 0,                    --类型（0，id；1，class；2，subClass）
    Param_2 = 1013011,                    --参数(根据类型区分具体意义)
    Param_3 = 5,                    --数量
    Param_4 = i_attackup5 ,          --额外的效果impact id
    Duration = 0,
}




local sk_main = sk_mk{
    H_1 = h_mk{
        IsAnimHit=0,
        TargetType = 2,
        
        I_1 = {
            Impact = i_jianshe,
   
        },
    },

    H_2 = h_mk{
        IsAnimHit=0,
        TargetType = 2,
        
        I_1 = {
            Impact = i_attackmodify1,
        },
        I_2 = {
            Impact = i_attackmodify2,
        },
        I_3 = {
            Impact = i_attackmodify3,
        },
        I_4 = {
            Impact = i_attackmodify4,
        },
        I_5 = {
            Impact = i_attackmodify5,
        },
    } ,



    H_3 = h_mk{                         --伤害
        TargetType = 1,
        I_1 = {
            Impact = i_damage,
   
        }
    } ,
    H_4 = h_mk{
        IsAnimHit=0,
        TargetType = 2,
    
        I_1 = {
            Impact = i_qingbiaoji,
   
        },
     
    },
  
    
}



return sk_main