--免疫前几次群攻(非指向性伤害)
--参数：
--次数,-1表示无限
--吸收比例（10000表示免疫，7500表示75%吸收）


local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')

require("class")

local Impact_31 = class("Impact_31",Impact)

--只在激活时生效一次

function Impact_31:OnActive()
    self.hasImm = true
    self.count = 0
end

function Impact_31:bcWaveStart( waveIndex )
    self.hasImm = true
    self.count = 0
end

function Impact_31:RefixDamage(ret)

    if not self:CanEffected() then
        return
    end
    
    if not self.hasImm then
        return
    end

    --伤害值0，不处理
    if ret.value <= 0 then
        return
    end

    --已经免疫了，不处理
    if ret.isImmue then
        return
    end

    if ret.impact == nil then
        return
    end

    --指向性的伤害，不处理
    if ret.impact:IsDirective() then
        return
    end

    --dot不处理
    if ret.impact:IsDOT() then
        return
    end

    local count = self.tab.Param[1]
    local percent = self.tab.Param[2]

    if percent >= 10000 then
        ret.isImmue = true
        self.recver:NotifyImmue()
    else
        local val = math.ceil( ret.value * ((10000 - percent) / 10000) )
        val = math.max(0,val)
        ret.value = val
    end

    self:ImpactEffected()   
    
    if count < 0 then
        return
    end

    self.count = self.count + 1
    
    if self.count >= count then
        --移除自己
        self.recver:RemoveBuff(self)        
    end
end

return Impact_31