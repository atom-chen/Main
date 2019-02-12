--直接修改阴阳界
--参数：
--环境


local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')

require("class")

local Impact_27 = class("Impact_27",Impact)

--只在激活时生效一次
function Impact_27:OnActive()
    Impact_27.__base.OnActive(self)
    local recver = self.recver
    local tab = self.tab

    if not self:CanEffected() then
        return
    end

    local env = tab.Param[1]

    if recver == nil then
        return
    end

    if recver.battle ~= nil then
        recver.battle:ChangeEnvType(env)
    end

    self:ImpactEffected()    
end

return Impact_27