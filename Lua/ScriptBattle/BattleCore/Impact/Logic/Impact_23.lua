--根据生命上限百分比直接造成伤害
--无视防御
--参数:
--1,百分比


local Impact = require("BattleCore/Impact/Impact")

require("class")
local AttrType = require("BattleCore/Common/AttrType")
local common = require("common")
local CalcParams = require("BattleCore/CalcParams")
local ImpactUtils = require('BattleCore/Impact/ImpactUtils')
local HitType = require('BattleCore/Common/HitType')
local Impact_23 = class("Impact_23",Impact)

function Impact_23:OnActive()
    Impact_23.__base.OnActive(self)
    self:DoCalc()
end

function Impact_23:DoCalc()
    local tab = self.tab
    local sender = self.sender
    local recver = self.recver

    if sender == nil or recver == nil then
        return
    end

    if not sender.isValid or not recver:IsAlive() then
        return
    end

    local p = tab.Param[1]
    local percent = p / 10000

    local final = math.ceil(sender:GetMaxHP() * percent)
    final = math.max(0,final)

    local ret = {
        impact = self,
        value = final,
        isCrit = false,
        senderId = sender.id,
        hitType = HitType.Damage,
		notDrainLife = true, --不吸血
    }

    self.ret = ret
end

function Impact_23:OnImpact()
    Impact_23.__base.OnImpact(self)

    if not self:CanEffected() then
        return
    end
    
    if self.ret == nil then
        return
    end

    local recver = self.recver

    if recver == nil or not recver:IsAlive() or not recver.isValid then
        return
    end

    recver:RecvDamage(self.ret)
    self:ImpactEffected()    
    
end

return Impact_23