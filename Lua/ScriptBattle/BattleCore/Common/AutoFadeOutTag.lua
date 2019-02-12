-- Generate Code,do not edit

--
local t = {
    RecvDamage = 1, -- 受到攻击
    SenderDead = 2, -- 发送者死亡
    UseSkill = 4, -- 使用技能后（必须是自己的回合）
    UseSkillAny = 8, -- 使用任意技能（非自己回合也算）
    SendSkillFin = 16, -- 发送这个Impact的技能结束
    SenderActEnd = 32, -- 发送者行动结束
    SenderRoundEnd = 64, -- 发送者回合结束
    
}
return t