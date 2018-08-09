--修正伤害，每次伤害不能超过特定属性的百分比
--参数
--参数1，属性类型
--参数2，比例

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')

require("class")

local Impact_53 = class("Impact_53",Impact)

function Impact_53:RefixDamage(ret)
    local attrType = self.tab.Param[1]
    local percent = self.tab.Param[2]

    if not self:CanEffected() then
        return
    end
    
    local attrValue = self.recver:GetAttrValue(attrType)
    local maxDmg = math.ceil( attrValue * (percent / 10000) )

    if ret.value > maxDmg then
        ret.value = maxDmg
        self:ImpactEffected()        
    end
end

return Impact_53