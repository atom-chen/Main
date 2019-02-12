--特定符灵受到指向性技能时，自身生命值大于特定符灵时，会有几率对其进行分摊伤害
--参数
--1,cardId（特定符灵,-1表示任意队友）
--2,概率
--3,分摊比例（10000）

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')
local HitType = require('BattleCore/Common/HitType')
local BattleEventType = require('BattleCore/Common/BattleEventType')

require("class")

local Impact_21 = class("Impact_21",Impact)

function Impact_21:OnActive()
    Impact_21.__base.OnActive(self)
    self.specCardId = self.tab.Param[1]
end

function Impact_21:bcRefixDamage(role,ret)

    if not self:CanEffected() then
        return
    end

    local recver = self.recver

    if role.side ~= recver.side then
        return
    end

    if role == recver then
        return
    end

    if self.specCardId ~= -1 and not self:CheckIsSpecCard(role) then
        return
    end

    if ret.impact == nil then
        return
    end

    if not ret.impact:IsDirective() then
        return
    end

    if recver:GetHP() < role:GetHP() then
        return
    end

    --自己打的，不守护了
    if recver.id == ret.senderId then
        return
    end

    --概率
    local chance = self.tab.Param[2]
    if not recver.battle:IsRandLt(chance) then
        return
    end

    --伤害分配
    local percent = self.tab.Param[3] / 10000
    local takeDamage = math.ceil(ret.value * percent)
    if takeDamage <= 0 then
        return
    end
    ret.value = ret.value - takeDamage
    if ret.value < 0 then ret.value = 0 end

    recver.battle:AddEvent({
        type = BattleEventType.Hit,
        hitEvent = {
            senderID = recver.id,
            targetID = role.id,
            val = takeDamage,
            hitType = HitType.Guard,
        }
    })

    --自己受到伤害
    recver:RecvDamage({
        impact = self,
        value = takeDamage,
        senderId = ret.senderId,
        hitType = HitType.Damage,
    })

    self:ImpactEffected()    
    
end

return Impact_21