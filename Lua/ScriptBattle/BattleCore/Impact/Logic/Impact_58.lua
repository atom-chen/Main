--根据当前血量（损失血量）计算属性修正
--参数：
--1，类型（1，当前血量；2，损失血量）
--2，属性类型
--3，固定值修正
--4,百分比修正

local Impact = require("BattleCore/Impact/Impact")

require("class")
local AttrType = require("BattleCore/Common/AttrType")
local common = require("common")

local Impact_58 = class("Impact_58",Impact)

local CalcType = {
    CurHP = 1,
    HPDamaged = 2,
}

function Impact_58:OnImpactFadeIn()
    Impact_58.__base.OnImpactFadeIn(self)

    local tab = self.tab
    local param = tab.Param
    local recver = self.recver
    
    self.refixBase = {
        addition = param[3],
        percent = param[4],
    }

    self.refix = {
        attrType = param[2],
        addition = 0,
        percent = 0,

        impact = self,
    }

    self.type = param[1]

    self:ReCalc()

    self.recver:AddAttrRefix(self.refix)
end

function Impact_58:msgHPChange(hp,maxHP)
    self:ReCalc()
end

function Impact_58:ReCalc()

    local hp = self.recver:GetHP()
    local maxHP = self.recver:GetMaxHP()

    local percent = 0

    if self.type == CalcType.CurHP then
        percent = hp / maxHP
    elseif self.type == CalcType.HPDamaged then
        percent = (maxHP - hp) / maxHP
    end

    if percent < 0 then
        percent = 0
    end

    self.refix.addition = math.ceil(percent * self.refixBase.addition)
    self.refix.percent = math.ceil( percent * self.refixBase.percent )

    self.recver:SetAttrsDirty()

end

function Impact_58:OnImpactFadeOut()
    Impact_58.__base.OnImpactFadeOut(self)
    if self.refix == nil then
        return
    end
    self.recver:RemoveAttrRefix(self.refix)
end

return Impact_58