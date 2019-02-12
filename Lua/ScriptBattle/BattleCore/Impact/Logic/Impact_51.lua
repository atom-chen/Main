--吸取目标血量治疗自己
--参数
--1,抽取比例
--2,目标最低血量比例(低于这个比例不抽取，-1表示无视)

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')
local HitType = require('BattleCore/Common/HitType')

require("class")

local Impact_51 = class("Impact_51",Impact)

function Impact_51:OnActive()
    Impact_51.__base.OnActive(self)

    self.drainPercent = self.tab.Param[1]
    self.minHPPercent = self.tab.Param[2]
end

function Impact_51:OnImpact()
    Impact_51.__base.OnImpact(self)

    if not self:CanEffected() then
        return
    end

    local recver = self.recver
    if not recver:IsAlive() then
        return
    end

    local sender = self.sender

    if self.minHPPercent > 0 and recver:GetHPPercent10000() < self.minHPPercent then
        return
    end

    local val = math.ceil( recver:GetHP() * (self.drainPercent / 10000) )
    if val <= 0 then
        return
    end

    --扣血
    recver:RecvDamage({
        impact = self,
        value = val,
        senderId = sender.id,
        hitType = HitType.Damage,
    })

    --治疗
    sender:RecvHeal({
        impact = self,
        value = val,
        senderId = sender.id,
    })
    
    self:ImpactEffected()        

end

return Impact_51