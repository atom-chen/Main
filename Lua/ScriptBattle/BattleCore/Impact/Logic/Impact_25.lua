--根据id驱散buff
--参数：
--被驱散的impact id
--驱散数量


local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')

require("class")

local Impact_25 = class("Impact_25",Impact)

--只在激活时生效一次
function Impact_25:OnActive()
    Impact_25.__base.OnActive(self)
    local recver = self.recver
    local tab = self.tab

    if not self:CanEffected() then
        return
    end

    local impactId = tab.Param[1]
    local count = tab.Param[2]

    if impactId < 0 then
        return
    end

    local buffs = recver:GetBuffsByImpactId(impactId)
    if buffs == nil then
        return
    end
    local n = 0
    for _,buff in ipairs(buffs) do
        if count < 0 or n < count then
            if recver:RemoveBuff(buff,true) then
                n = n + 1
            end
        end
    end
    if n > 0 then
        --暂时不做表现，这个效果一般是用来清标记buff
        --recver:NotifyDispell(impactClass)        
    end
    --一次性通知
    recver:NotifyBuffChange()
    self:ImpactEffected()    
end
return Impact_25