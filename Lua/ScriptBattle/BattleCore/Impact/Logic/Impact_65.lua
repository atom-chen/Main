--封禁被动
--参数：

local Impact = require("BattleCore/Impact/Impact")

require("class")
local common = require("common")
local warn = common.mk_warn('Impact_65')

local Impact_65 = class("Impact_65",Impact)

--原则上应该是只需要一个标记，然后每个buff的触发、获取时判断激活
--这样会有循环嵌套
--因此还是再role上加一个计数标记，这个65逻辑，增减这个标记
--这样buff里可以直接通过owner查询这个标记
--代码上有交叉，不过可以提升查找速度

function Impact_65:OnImpactFadeIn()
    self.recver:IncPassiveDisableCount()
end

function Impact_65:OnImpactFadeOut(autoFadeOut)
    self.recver:DescPassiveDisableCount()
end

return Impact_65