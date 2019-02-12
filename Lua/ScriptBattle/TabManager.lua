
local common = require("common")
local warn = common.mk_warn('TabManager')

local loaded = {}

local funcTb = {}

local cached = true

local scriptSkillTab = {
    SkillEx = true,
    SkillBase = true,
    Impact = true,
    SkillHit = true,
}

local function GetByID(tabName,id)

    if type(id) ~= 'number' then

        --脚本化的技能，底层直接把id当tab返回
        if scriptSkillTab[tabName] and type(id) == 'table' then
            return id
        end

        warn("Call tabMgr.GetByID failed,id type is " .. type(id))
        error("Call tabMgr.GetByID failed,id type is " .. type(id))
    end

    if cached then
        local tab = loaded[tabName]
        if tab == nil then
            tab = {}
            loaded[tabName] = tab
        end
        local item = tab[id]
        if item ~= nil then
            return item
        end
    end

    --调用加载
    local func = funcTb[tabName]
    if func == nil then
        error("load tab failed,not support yet." .. tabName)
        return nil
    end

    local item = func(id)
    if item == nil or item.failed then
        --error("load tab failed,no such id." .. tabName .. "," .. tostring(id))
        return nil
    end
    if cached then
        loaded[tabName][id] = item
    end
    return item
end

local function Clear()
    loaded = {}
end

return {
    RegistTabReader = function(tb) funcTb = tb end,
    GetByID = GetByID,
    Clear = Clear,
    SetCache = function(val) cached = val end,
}