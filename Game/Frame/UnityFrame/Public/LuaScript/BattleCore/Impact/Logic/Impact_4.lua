
--属性修正
--参数：
--1，属性类型1
--2，固定值修正
--3,百分比修正

--4，属性类型2
--5，固定值修正
--6,百分比修正
--...

local Impact = require("BattleCore/Impact/Impact")

require("class")
local AttrType = require("BattleCore/Common/AttrType")
local common = require("common")

local MaxAttrCount = 5

local Impact_4 = class("Impact_4",Impact)

function Impact_4:OnImpactFadeIn()
    Impact_4.__base.OnImpactFadeIn(self)
    self.RefixList = {}
    local tab = self.tab
    local param = tab.Param
    local recver = self.recver
    for i=1,MaxAttrCount*3,3 do
        local attrType = param[i]
        if attrType ~= -1 then
            local refix = {
                attrType = attrType,
                addition = param[i+1],
                percent = param[i+2],
            }
            table.insert( self.RefixList, refix )
            recver:AddAttrRefix(refix)
        end
    end
end

function Impact_4:OnImpactFadeOut()
    Impact_4.__base.OnImpactFadeOut(self)
    if self.RefixList == nil then
        return
    end
    local recver = self.recver
    for _,refix in ipairs(self.RefixList) do
        recver:RemoveAttrRefix(refix)
    end
    self.RefixList = nil
end

return Impact_4