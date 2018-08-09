--根据受击者生命上限百分比直接造成伤害
--无视防御，不会被修正
--参数:
--1,百分比
--2,伤害类型（-1普通伤害，11血祭伤害（扣血不播受击）
--3,扣血百分比类型（-1生命上限 0当前血量）

local Impact = require("BattleCore/Impact/Impact")

require("class")
local AttrType = require("BattleCore/Common/AttrType")
local common = require("common")
local CalcParams = require("BattleCore/CalcParams")
local ImpactUtils = require('BattleCore/Impact/ImpactUtils')
local HitType = require('BattleCore/Common/HitType')
local RoleType = require('BattleCore/Common/RoleType')


local Impact_34 = class("Impact_34",Impact)

function Impact_34:OnActive()
    Impact_34.__base.OnActive(self)
    self:DoCalc()
end

function Impact_34:DoCalc()
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
    local hitType = tab.Param[2]
    local damageType = tab.Param[3]
    local percent = p / 10000

    local final = 0
    if (0 == damageType) then
        final = math.ceil(recver:GetHP() * percent)        
    else
        final = math.ceil(recver:GetMaxHP() * percent)
    end

    final = math.max(0,final)

    --针对boss，效果减半
    if recver.roleBaseTab ~= nil and recver.roleBaseTab.Type == RoleType.Boss then
        final = math.ceil( final * 0.5 )
    end

    local ret = {
        impact = self,
        value = final,
        isCrit = false,
        senderId = sender.id,
    }

    if hitType ~= -1 then
        ret.hitType = hitType
    end

    self.ret = ret
end

function Impact_34:OnImpact()
    Impact_34.__base.OnImpact(self)
    
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

return Impact_34