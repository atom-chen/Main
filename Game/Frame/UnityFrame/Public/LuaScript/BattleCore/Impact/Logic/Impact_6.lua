--行动条增加减少
--参数
--增减数

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')

require("class")

local Impact_6 = class("Impact_6",Impact)

function Impact_6:OnImpact()
    Impact_6.__base.OnImpact(self)
    local recver = self.recver
    local tab = self.tab

    if not self:CanEffected() then
        return
    end

    local val = tab.Param[1]

    local ret = {
        value = val,
        senderId = self.sender.id,
    }

    if val > 0 then
        recver:RecvApInc(ret)
    else
        ret.value = -ret.value
        recver:RecvApDesc(ret)
    end
    self:ImpactEffected()    
end

return Impact_6