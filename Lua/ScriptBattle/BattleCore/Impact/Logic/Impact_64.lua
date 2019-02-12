--吸血溢出转换为护盾
--参数：
--1.护盾值不超过最大生命百分比
--2.护盾buff

local Impact = require("BattleCore/Impact/Impact")

require("class")
local common = require("common")
local warn = common.mk_warn('Impact_64')

local Impact_64 = class("Impact_64",Impact)

function Impact_64:OnActive()
    self.maxPercent = self.tab.Param[1]
    self.shiledImpactId = self.tab.Param[2]
end

function Impact_64:msgDrainLife(drainVal)
    local recver = self.recver
    local maxHP = recver:GetMaxHP()
    local validValue = recver:GetHP() + drainVal - maxHP
    if validValue <= 0 then
        return
    end

    local maxShiled = math.ceil(maxHP * (self.maxPercent / 10000))

    local currShiled = recver:GetShiled()
    local newShiled = validValue + currShiled

    newShiled = common.clamp(newShiled,0,maxShiled)

    if self.shiledBuff == nil or not self.shiledBuff:IsActiveValid() then
        self.shiledBuff = Impact.SendImpactToTarget(self.shiledImpactId,recver,recver,nil,self.skillInfo)
    end

    if self.shiledBuff == nil then
        return
    end

    self.shiledBuff:ChangeShiled(newShiled)

    self:ImpactEffected()        
end

return Impact_64