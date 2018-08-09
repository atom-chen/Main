--伤害分摊，发送者收到伤害，会分摊给接受者
--参数
--参数1，分摊比例

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')

require("class")

local Impact_54 = class("Impact_54",Impact)

function Impact_54:OnActive()
    self:InitFromTab()
end

function Impact_54:InitFromTab()
    self.percent = self.tab.Param[1]
end

function Impact_54:bcRefixDamage(role,ret)

    if not self:CanEffected() then
        return
    end

    if role ~= self.sender then
        return
    end

    --自己分摊自己的伤害会死循环
    if self.sender == self.recver then
        return
    end
    
    local dmg = math.ceil( ret.value * (self.percent / 10000) )
    ret.value = ret.value - dmg

    if ret.value < 0 then ret.value = 0 end

    if dmg > 0 then
        --收到伤害
        self.recver:RecvDamage({
            impact = self,
            value = dmg,
            senderId = self.sender.id,
        })
        self:ImpactEffected()
    end
end

return Impact_54