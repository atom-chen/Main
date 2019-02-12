--行动条抽取
--参数
--抽取量

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')

require("class")

local Impact_55 = class("Impact_55",Impact)

function Impact_55:OnImpact()
    Impact_55.__base.OnImpact(self)
    local recver = self.recver
    local tab = self.tab

    if not self:CanEffected() then
        return
    end

    local val = tab.Param[1]

    val = common.clamp(val,0,recver.ap)

    if (val <= 0 ) then
        return
    end

    recver:RecvApDesc({
        value = val,
        senderId = self.sender.id,
    })
    
    self.sender:RecvApInc({
        value = val,
        senderId = self.sender.id,
    })

    self:ImpactEffected()    
end

return Impact_55