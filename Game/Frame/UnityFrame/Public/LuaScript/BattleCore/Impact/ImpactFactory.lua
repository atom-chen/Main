--辅助创建Impact的工厂

local Impact = require("BattleCore/Impact/ImpactBase")
local tabMgr = require('TabManager')
local common = require("common")
local warn = common.mk_warn('ImpactFactory')


local function CreateImpact(tab,target,sender)
    if type(tab) == 'number' then
        tab = tabMgr.GetByID('Impact',tab)
    end

    if tab == nil then
        return
    end

    local clazz = nil
    if tab.ImpactLogic ~= -1 then
        clazz = require("BattleCore/Impact/Logic/Impact_" .. tab.ImpactLogic)  --tab.ImpactLogic
        if clazz == nil then
            warn("Not support logicID:",tab.ImpactLogic)
            return nil
        end

        if type(clazz) == 'boolean' then
            warn('you forget return Impact',"BattleCore/Impact/Logic/Impact_" .. tab.ImpactLogic)
            return nil
        end
    else
        clazz = require("BattleCore/Impact/ImpactBase")
    end


    local impact = new(clazz)
    impact.logicID = tab.ImpactLogic
    impact:Init(tab,target,sender)
    return impact
end

--创建一个impact，并激活
local function SendImpactToTarget(tab,target,sender,buffContainer,skillInfo,recursionCount,silent)
    --目标是否不能选中
    if target:IsNothingness() then
        return nil
    end

    if recursionCount and recursionCount > 5 then
        warn("too many child impact.")
        return nil
    end

    if type(tab) == 'number' then
        local impactId = tab
        tab = tabMgr.GetByID('Impact',impactId)
        if tab == nil then
            warn('impact not found',impactId)
        end
    end

    if tab == nil then
        return nil
    end

    --检查是否免疫
    if target:IsImmue(tab) then
        target:NotifyImmue()
        return nil
    end

    if buffContainer == nil then
        buffContainer = target.buffContainer
    end

    --检查是否互斥
    if buffContainer:CheckMutex(tab) then
        return nil
    end

    --检查叠加上限
    if buffContainer:CheckLayerFull(tab) then
        return nil
    end

    local impact = CreateImpact(tab,target,sender)

    if impact == nil then
        return nil
    end

    impact.skillInfo = skillInfo

    --暂时不启用修正
    -- if sender ~= nil then
    --     sender:RefixImpactSend(impact)
    -- end
    -- target:RefixImpactRecv(impact)

    --激活
    local isFinish = impact:Active()
    --print("impact active.",impact.impactId)
    target:SendMessage("msgRecvImpact",impact)

    if isFinish then
        return impact
    end
    
    --如果是特殊容器管理这个buff，则由特殊容器去管理
    if buffContainer ~= nil then
        buffContainer:AddBuff(impact,silent)
    else
        --让接收方管理
        if target ~= nil then
            target:AddBuff(impact,silent)
        end
    end

    --子buff机制
    if tab.ChildImpact ~= -1 then
        
        local childImpact = SendImpactToTarget(tab.ChildImpact
            ,target,sender
            ,buffContainer
            ,skillInfo
            ,(recursionCount or 0) + 1
        )

        if childImpact ~= nil then
            childImpact.parent = impact
            impact.child = childImpact
        end
    end

    return impact
end

Impact.SendImpactToTarget = SendImpactToTarget
Impact.CreateImpact = CreateImpact