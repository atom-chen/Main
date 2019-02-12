local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


--白泽3技能：发动一次单体攻击，概率将敌方变成小羊1回合且清空行动条，小羊会分摊白泽受到的一定伤害
local i_damage = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,                --普通伤害

    Param_1 ="a1",                 -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                    -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = 0,                    -- 同一技能相同impact多次命中衰减系数
}


--分摊
local i_fentan = i_mk{

    Duration=1,
    MutexID=1102,
    MutexPriority=1,
    
    ImpactLogic = 54,                --伤害分摊，发送者收到伤害，会分摊给接受者
    Param_1 ="a2",                 -- 分摊比例

}

--变羊效果
local i_Sheep = i_mk{
    Id=1102011,

    Duration=1,
    AliveCheckType=2,
    MutexID=1,
    MutexPriority=6,
    ImpactClass=4,
    ImpactSubClass=1,

    ChildImpact=i_fentan,

    ImpactLogic=-1,
}


--清空行动条
local i_xindongtiao = i_mk{
    
    Duration=0,
    IsShow=1,
    AliveCheckType=1,
    ImpactClass=4,
    ImpactLogic=6,                      --行动条增加减少
    Param_1 =-1000,                     --增减数（负数表示减少） 
}

local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType = 1 ,
        I_1 = {                         --伤害
            Impact = i_damage,
        },
 
        I_2 = {                        --变羊
            Chance ="a3",
            IsChanceRefix = 1,
            Impact = i_Sheep,
        },
        I_3 = {                        --清空行动条
            IsChanceRefix = 1,
            Impact = i_xindongtiao,
    }
    },
    
}

return sk_main