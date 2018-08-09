--激活时检测，如果接受者指定buff存在超过指定数量时，触发一个额外的效果
--参数：
--类型（0，id；1，clas；2，subClass）
--参数（根据类型区分具体意义）
--数量
--额外的效果


local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')

require("class")

local Impact_29 = class("Impact_29",Impact)

local CounterType = {
    byId = 0,
    byClass = 1,
    bySubClass = 2,
}

--只在激活时生效一次
function Impact_29:OnActive()
    Impact_29.__base.OnActive(self)
    local recver = self.recver
    local param = self.tab.Param

    if recver == nil then
        return
    end

    if not self:CanEffected() then
        return
    end

    local counterType = param[1]
    local dataId = param[2]
    local countNeed = param[3]
    local extraImpactId = param[4]

    local count = 0

    if counterType == CounterType.byId then
        local ret = recver:GetBuffsByImpactId(dataId)
        if ret ~= nil then
            count = #ret
        end
    elseif counterType == CounterType.byClass then
        local ret = recver:GetBuffsByImpactClass(dataId)
        if ret ~= nil then
            count = #ret
        end
    elseif counterType == CounterType.bySubClass then
        local ret = recver:GetBuffsByImpactSubClass(dataId)
        if ret ~= nil then
            count = #ret
        end
    end

    if count >= countNeed then
        --发起者，额外再发一个效果
        Impact.SendImpactToTarget(extraImpactId,recver,self.sender)
        self:ImpactEffected() 
    end

end

return Impact_29