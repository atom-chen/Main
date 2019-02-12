--反击，受到伤害后，对伤害来源使用指定技能反击，最终结算伤害有系数
--参数：
--概率
--伤害系数（10000表示百分百伤害，小于10000表示伤害衰减，大于伤害加强）
--无用参数已经删除
--技能index

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')

require("class")

local Impact_33 = class("Impact_33",Impact)

function Impact_33:OnActive()
    
    self.chance = self.tab.Param[1]
    self.damagePercent = self.tab.Param[2]
    self.skillIndex = self.tab.Param[4]

end

function Impact_33:msgRecvDamage( ret )
    if ret.senderId == nil then
        return
    end

    if not self:CanEffected() then
        return
    end

    --不能被反击
    if ret.notReverge then
        return
    end

    local battle = self.recver.battle

    local target = battle:GetRoleById(ret.senderId)
    if target == nil or not target.isValid or not target:IsAlive() then
        return
    end
    --队友伤害不触发
    if target.side == self.recver.side then
        return
    end

    --概率是否通过
    if not battle:IsRandLt(self.chance) then
        return
    end

    --强制使用技能
    local ret = self.recver:ActUseSkillByIndex(self.skillIndex,target.id,{isHitBack = true})
    if ret then
        self:ImpactEffected()
    end
end

function Impact_33:RefixSendDamage(ret)

    if ret.impact == nil then
        return
    end

    if ret.impact.skillInfo == nil then
        return
    end

    local skillProcess = ret.impact.skillInfo.skillProcess
    if skillProcess == nil then
        return
    end

    --有过标记
    if skillProcess.data == nil then
        return
    end

    if not skillProcess.data.isHitBack then
        return
    end

    local val = math.ceil( ret.value * (self.damagePercent / 10000) )
    val = math.max(0,val)
    ret.value = val
end

return Impact_33