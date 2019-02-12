--行动条洗条，所有行动条上的人，互换位置
--本质上是ap互换
--参数

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')

require("class")

local Impact_52 = class("Impact_52",Impact)

function Impact_52:OnImpact()
    Impact_52.__base.OnImpact(self)
    local battle = self.recver.battle
    if battle == nil then
        return
    end
    
    local q = battle.actQ

    local t = {}

    for _,r in ipairs(q.actQ) do
        table.insert(t,r.ap)
    end

    --洗牌
    for i=1,3 do
        battle:Shuffle(t)
    end

    for i,r in ipairs(q.actQ) do
        r:SetAP(t[i])
    end

    q:ReSort()
    battle:SyncAP()

    self:ImpactEffected()
end

return Impact_52