
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--白骨2技能-白骨妖女拔出骨剑攻击敌人，造成白骨妖女攻击520%的普通伤害。敌人的血量越低，伤害越高。击杀敌人后将重置【傲骨】的冷却时间。
--[A78C84]【妖界效果】回复所造成伤害量20%的生命。[-]冷却时间4回合。


local i_damage  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}



--敌人血量越低伤害越高
local i_xueliangshanghai = i_mk{
    Duration=1,
    AliveCheckType=2,
    AutoFadeOutTag=8,
    MutexID=1541,
    MutexPriority=1,
    ImpactLogic =58,                    -- 根据当前血量（损失血量）计算属性修正 

    Param_1= 2,                         --类型（1，当前血量；2，损失血量）
    Param_2= 7,                         --属性类型
    Param_3= -10000,                     --固定值修正
    Param_4= 0,                          --百分比修正
}


local sk_main = sk_mk{

   
    H_1 = h_mk{                         --伤害
    TargetType = 1,
    I_1 = {
        Impact = i_xueliangshanghai,
    },
    I_2 = {
        Impact = i_damage,
    },
},
}

return sk_main