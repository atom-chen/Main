
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--蜃珧1技能：普攻+概率沉睡1回合，【妖界】：如果攻击处于沉睡状态的敌人，增加40%攻击条

local i_damage  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}

--沉睡效果
local i_Sleep = i_mk(sc.CommonBuffs.Sleep)       --引用通用沉睡
      i_Sleep.Duration = 1                       --修改持续回合          



local biaoji  = i_mk{
    Id=1090012,
    Duration=-1,
    AliveCheckType=1,
    AutoFadeOutTag=8,

    ImpactLogic = -1,

}

--如果目标为沉睡状态，则上标记
local i_shangbiaoji  = i_mk{
    Duration= 0,

    ImpactLogic = 29,              -- 激活时检测，如果接受者指定buff存在超过指定数量时，触发一个额外的效果

    Param_1 = 2,                   -- 类型（0，id；1，class；2，subClass）
    Param_2 = 512,                 -- 参数(根据类型区分具体意义)
    Param_3 = 1,                   -- 数量
    Param_4 = 1090012,             -- 额外的效果impact id

}

local sk_main = sk_mk{
 
    H_1 = h_mk{                         --伤害
        TargetType = 1,
        I_1 = {
            Impact = i_damage,
        },
        I_2 = {
            Impact = i_shangbiaoji,
        },
        I_3 = {                        --沉睡效果
            Chance = "a2",
            IsChanceRefix = 1,
            Impact = i_Sleep,
    }
    },

   
}

return sk_main