--伤害无视
--参数
--次数,-1表示无限


local Impact = require("BattleCore/Impact/Impact")

require("class")
local common = require("common")

local Impact_8 = class("Impact_8",Impact)

function Impact_8:RefixDamage(ret)
    local count = self.tab.Param[1]

    --直接改成免疫
    ret.isImmue = true
    self.recver:NotifyImmue()
    self:ImpactEffected()    
    
    if count < 0 then
        return
    end

    if not self:CanEffected() then
        return
    end

    self.count = self.count or 0
    self.count = self.count + 1
    
    if self.count >= count then
        self.recver:RemoveBuff(self)
    end
end

return Impact_8