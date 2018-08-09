--根据buff数量增加属性修正，属性修正
--参数：
--1，buff计数类型（1，impactId；2，class；3，subClass
--2，buff参数

--3，属性类型
--4，固定值修正
--5,百分比修正

local Impact = require("BattleCore/Impact/Impact")

require("class")
local AttrType = require("BattleCore/Common/AttrType")
local common = require("common")

local Impact_57 = class("Impact_57",Impact)

local BuffCountType = {
    ById = 1,
    ByClass = 2,
    BySubClass = 3,
}

function Impact_57:OnImpactFadeIn()
    Impact_57.__base.OnImpactFadeIn(self)

    local tab = self.tab
    local param = tab.Param
    local recver = self.recver

    self.countType = param[1]
    self.countParam = param[2]

    local attrType = param[3]

    self.refixBase = {
        addition = param[4],
        percent = param[5],
    }

    self.refix = {
        attrType = attrType,
        addition = 0,
        percent = 0,
    }
    self:ReCalc()

    self.recver:AddAttrRefix(self.refix)
end

function Impact_57:OnRoleImpactFadeIn(impact)
    if self == impact then
        return
    end

    self:ReCalc()
end

function Impact_57:OnRoleImpactFadeOut(impact,autoFadeOut)
    if self == impact then
        return
    end
    
    self:ReCalc()
end

function Impact_57:ReCalc()

    local buffs = nil 

    if self.countType == BuffCountType.ById then
        buffs = self.recver:GetBuffsByImpactId(self.countParam)
    elseif self.countType == BuffCountType.ByClass then
        buffs = self.recver:GetBuffsByImpactClass(self.countParam)
    elseif self.countType == BuffCountType.BySubClass then
        buffs = self.recver:GetBuffsByImpactSubClass(self.countParam)
    end

    local buffCount = 0

    if buffs ~= nil then buffCount = #buffs end

    self.refix.addition = buffCount * self.refixBase.addition
    self.refix.percent = buffCount * self.refixBase.percent

    self.recver:SetAttrsDirty(self.refix.attrType)

end

function Impact_57:OnImpactFadeOut()
    Impact_57.__base.OnImpactFadeOut(self)
    if self.refix == nil then
        return
    end
    self.recver:RemoveAttrRefix(self.refix)
end

return Impact_57