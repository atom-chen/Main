--队友受到伤害后，会概率使用技能
--参数
--1,概率
--2,技能id
--3,触发类型(1自己，2队友，3任意我方)
--4,立即使用（0等正在行动的人结束后使用，1立即使用）（一般有动画表现的，最好0，没动画表现的1）
--5,技能目标（1攻击者，2自己）(最好是hit里直接使用特殊目标，技能目标只是辅助用)

local TargetType = {
    Attacker = 1,
    Self = 2,
}

local TriggerType = {
    Self = 1,
    AlliesOnly = 2,
    AnyAllies = 3,
}

local Impact = require("BattleCore/Impact/Impact")

require("class")
local common = require("common")

local Impact_17 = class("Impact_17",Impact)

function Impact_17:bcRoleDamage(roleId,ret)

    if not self:CanEffected() then
        return
    end

    local tab = self.tab
    local chance = tab.Param[1]
    local skillExId = tab.Param[2]
    local recvType = tab.Param[3]
    local isUseImm = tab.Param[4] == 1
    local targetType = tab.Param[5]

    local recver = self.recver
    if recver.battle == nil then
        return
    end

    local role = recver.battle:GetRoleById(roleId)
    if role == nil then
        return
    end

    if recvType == TriggerType.Self then
        if role ~= recver then
            return
        end
    elseif recvType == TriggerType.AlliesOnly then
        if role == recver or role.side ~= recver.side then
            return
        end
    elseif recvType == TriggerType.AnyAllies then
        if role.side ~= recver.side then
            return
        end
    else
        return
    end

    if not recver.battle:IsRandLt(chance) then
        return
    end

    local targetId = -1
    
    if targetType == TargetType.Attacker then
        targetId = ret.senderId
    elseif targetType == TargetType.Self then
        targetId = recver.id
    end

    if targetId == -1 then
        return
    end

    local ret = false
    if isUseImm then
        ret = recver:CastSkill(skillExId,targetId)
    else
        ret = recver:ActUseSkillByID(skillExId,targetId)
    end

    if ret then
        self:ImpactEffected()    
    end
end

return Impact_17