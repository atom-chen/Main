--复合impact，激活时，同时创建多个子impact

--废弃

local Impact = require("BattleCore/Impact/Impact")

require("class")
local common = require("common")
local BuffContainer = require("BattleCore/BuffContainer")

local Impact_2 = class("Impact_2",Impact)

-- function Impact_2:OnImpactFadeIn()
--     Impact_2.__base.OnImpactFadeIn(self)

--     local sender = self.sender
--     local recver = self.recver

--     self.buffContainer = new(BuffContainer)
--     self.buffContainer:Init(recver)
--     self.buffContainer:SetNeverNotify()

--     if self.tab.param == nil then
--         return
--     end

--     for _,child in ipairs(self.tab.param) do
--         if child ~= -1 then
--             Impact.SendImpactToTarget(child,recver,sender,self.buffContainer)
--         end
--     end
-- end

-- function Impact_2:RefixImpactSend(other)
--     self.buffContainer:RefixImpactSend(other)
-- end

-- function Impact_2:RefixImpactRecv(other)
--     self.buffContainer:RefixImpactRecv(other)
-- end

-- function Impact_2:Update()
--     if not self.isAlive then
--         return false
--     end
    
--     local ret = Impact_2.__base.Update(self)
--     self.buffContainer:Update()
--     return ret
-- end

-- function Impact_2:SendMessage(...)
--     if not self.isAlive then
--         return
--     end

--     if self.buffContainer == nil then
--         return
--     end

--     self.buffContainer:SendMessage(...)
-- end

return Impact_2