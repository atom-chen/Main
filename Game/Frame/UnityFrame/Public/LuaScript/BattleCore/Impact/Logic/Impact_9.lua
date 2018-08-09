--护盾
--收到伤害时，优先扣护盾的数值，扣完后，buff移除
--相同的护盾会冲突，数值高的优先级高
--参数：
--护盾比例
-- -1取接受者血量上限，0取施法者血量上限

local Impact = require("BattleCore/Impact/Impact")

require("class")
local common = require("common")

local Impact_9 = class("Impact_9",Impact)

function Impact_9:OnActive()
    Impact_9.__base.OnActive(self)
    --计算护盾值
    local percent = self.tab.Param[1]

    if (0 == self.tab.Param[2]) then
        self.absorbLeft = math.ceil(self.sender:GetMaxHP() * (percent / 10000))
    else
        self.absorbLeft = math.ceil(self.recver:GetMaxHP() * (percent / 10000))
    end
end

function Impact_9:OnImpactFadeIn()
    self.recver:IncShiled(self.absorbLeft)
end

function Impact_9:OnImpactFadeOut()
    if self.absorbLeft > 0 then
        self.recver:IncShiled(-self.absorbLeft)
    end
end

function Impact_9:RefixDamage(ret)

    if self.absorbLeft <= 0 then
        self.recver:RemoveBuff(self)
        return
    end

    if not self:CanEffected() then
        return
    end

    local absorbVal = math.min(ret.value,self.absorbLeft)
    ret.value = ret.value - absorbVal
    self.absorbLeft = self.absorbLeft - absorbVal

    --通知客户端吸收事件
    self.recver:IncShiled(-absorbVal)
    self.recver:NotifyAbsorb(absorbVal)
    self:ImpactEffected()        
    
    if self.absorbLeft <= 0 then
        self.recver:RemoveBuff(self)
    end
end

function Impact_9:CheckMutex(tab)
    --已经冲突了，不处理
    local mutex,ret = Impact_9.__base.CheckMutex(self,tab)
    if mutex then
        return mutex,ret
    end

    --根据数值检查
    if tab.ImpactLogic ~= 9 then
        return false,0
    end

    return true , self.tab.Param[1] - tab.Param[1]
end

return Impact_9