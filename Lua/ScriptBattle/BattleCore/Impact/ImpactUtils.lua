
local Utils = {}

local CalcParams = require("BattleCore/CalcParams")
local AttrType = require("BattleCore/Common/AttrType")
local common = require("common")
local tabMgr = require("TabManager")
local Attr = require("BattleCore/RoleAttrs")

function Utils.CalcCrit( sender,recver)

    if sender == nil or recver == nil then
        return false,0
    end

    local critEffect = 10000
    local isCrit = sender.battle:IsRandLt(sender:GetAttrValue(AttrType.CritChance) - recver:GetAttrValue(Attr.HidenAttrType.CritChanceResist) )
    if isCrit then
        critEffect = common.clamp((CalcParams.P3 + sender:GetAttrValue(AttrType.CritEffect)),10000,CalcParams.MaxCritEffect)
    end

    return isCrit,critEffect
end

function Utils.IsClass(tab,impactClass)
    if type(tab) == 'number' then
        if tab == -1 then
            return false
        end
        tab = tabMgr.GetByID('Impact',tab)
    end

    if tab == nil then
        return false
    end

    if impactClass < 0 then return false end
    return tab.ImpactClass & impactClass > 0
end

function Utils.IsSubClass(tab,subClass)
    if type(tab) == 'number' then
        if tab == -1 then
            return false
        end
        tab = tabMgr.GetByID('Impact',tab)
    end

    if tab == nil then
        return false
    end
    
    if subClass < 0 then return false end
    return tab.ImpactSubClass & subClass > 0
end

--是否有环境影响
function Utils.CalcEnvEffect(sender,target)
    if sender == nil or target == nil then
        return 0
    end
    if sender.battle == nil then
        return 0
    end

    --如果sender含有优势buff，则始终处于优势
    if sender:IsAlwaysEnvEnhance() then
        return 1
    end

    if sender.envType == 2 or target.envType == 2 then
        return 0
    end

    if sender.envType == target.envType then
        return 0
    end
    
    if sender.battle.curEnvType == sender.envType then
        --优势
        return 1
    else
        --劣势
        return -1
    end

    return 0
end

return Utils