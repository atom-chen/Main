--召唤物消散机制，buff消失时，销毁掉buff持有者
--参数

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')

require("class")

local Impact_41 = class("Impact_41",Impact)

function Impact_41:OnImpactFadeOut(autoFadeOut)
    if self.recver == nil then
        return
    end
    if self.recver.battle == nil then
        return
    end
    if not self.recver.isValid or not self.recver:IsAlive() then
        return
    end
    --直接删除掉
    self.recver.battle:DelRole(self.recver)
end

return Impact_41