
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--螭吻3技能：aoe+减速2回合+增加cd

local i_damage  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}


--增加技能冷却时间

local i_colddownup = i_mk{
    Duration=0,
    ImpactClass=4,

    ImpactLogic =35,                 --增减CD，有CD的技能，才会受这个效果影响         

    Param_1= -1,                  --技能索引（-1表示全部技能(1~3)）
    Param_2=  1,                  --增减值(大于0增加CD，小于0减少CD)


}


--缓速效果
local i_SpeedReduce = i_mk(sc.CommonBuffs.SpeedReduce)     --引用缓速效果
      i_SpeedReduce.Duration = 2                              --修改持续回合  


local sk_main = sk_mk{

    H_1 = h_mk{                         --伤害
        TargetType = 4,

        I_1 = {
            Impact = i_damage,
        },
        
        I_2 = {     
            Chance = "a2",
            IsChanceRefix = 1,         --增加技能冷却时间效果
            Impact =  i_colddownup,
    },

        I_3 = {     
        
            IsChanceRefix = 1,         --缓速效果
            Impact =  i_SpeedReduce,
    }
    },

   
}

return sk_main