--新版脚本技能创建器
--因为要兼容之前的表格体系的技能，通过Creator，把脚本里的Table，翻译成实际用的兼容版的Table
--本质上是做适配

local tabMgr = require("TabManager")

local array_parser = function(t,fmt,fixedLength)
    local ret = {}
    local maxIndex = 0
    local tmp = {}
    if fixedLength ~= nil then
        maxIndex = fixedLength
    end
    for k,v in pairs(t) do
        local kk = string.match(k,fmt)
        if kk ~= nil then
            local paramIndex = tonumber(kk)
            maxParam = math.max(paramIndex,maxIndex)
            tmp[paramIndex] = v
        end
    end

    for i=1,maxIndex do
        table.insert(ret,-1)
    end
    for k,v in pairs(tmp) do
        ret[k] = v
    end
    return ret
end

_G.class_mk = function(t)
    local ret = 0
    for _,v in ipairs(t) do
        ret = ret ~ v
    end
    return ret
end

local ImpactMt = {
    Duration = 0,
    AliveCheckType = 1,	
    AutoFadeOutTag	= 0,
    IsShow	= 0,
    MutexID	 = -1,
    MutexPriority = 0,	
    LayerID	= -1,
    LayerMax = -1,
    ImpactClass	= 0,
    ImpactSubClass	= 0,
    Tag	= -1,
    CooldownId = -1,	
    Cooldown = -1,
    RoundMaxEffectedCount = 10,
    ChildImpact = -1,
    ImpactLogic = -1,
}

local Impact_mk = function(t)
    local ret = {}
    ret.Param = array_parser(t,"Param_(%d+)",10)
    for k,v in pairs(t) do
        if not string.match(k,"(.+)_(%d+)") then
            ret[k] = v
        end
    end
    ret.__script_type = "impact"
    return ret
end


local HitMt = {
    IsAnimHit = 1,
    TargetType = 1,
}

local Hit_mk = function(t)
    local ret = {}
    ret.TargetParam = array_parser(t,"TargetParam_(%d+)",5)

    local impactSends = array_parser(t,"I_(%d+)")
    ret.TmpImpacts = array_parser(t,"Tmp_(%d+)")

    for k,v in pairs(t) do
        if not string.match(k,"(.-)_(%d+)") then
            ret[k] = v
        end
    end

    ret.Chance = {}
    ret.Impact = {}
    ret.IsChanceRefix = {}

    for i,item in ipairs(impactSends) do
        if type(item) ~= 'table' then
            error("parse hit failed." .. type(item))
        end
        ret.Impact[i] = item.Impact or -1
        ret.Chance[i] = item.Chance or 10000
        ret.IsChanceRefix[i] = item.IsChanceRefix or 0
    end

    ret.__script_type = "hit"

    return ret
end

local SkillBaseMt = {
    TargetType = 1,
    Range = 1,
    SkillClass = 16,
    MaxLevel = 999,
    PassiveEffectId = 0,
    IsShowPassiveName = 0,
}

local SkillBase_mk = function(t)
    local ret = {}
    for k,v in pairs(t) do
        if not string.match(k,"(.-)_(%d+)") then
            ret[k] = v
        end
    end
    ret.__script_type = "skillBase"
    return ret
end

--公用的SkillBase，隐藏技能的SkillBase都是一样的
local internalSkillBase = SkillBase_mk(SkillBaseMt)

local SkillMt = {
    Level = 1,
    SkillAI	= -1,
    Cooldown = -1,
    CooldownId	= -1,
    SPCost	= 0,
    EnvLimit = -1,
    BetterEnv = -1,
    OtherID	= -1,
    Time = 0,
    ChildPassive = -1,

    BaseID = internalSkillBase,
}

--主技能不绑定元表，加载时绑定
local Skill_mk = function(t)
    local ret = {}
    ret.Hit = array_parser(t,"H_(%d+)")
    ret.TmpImpacts = array_parser(t,"Tmp_(%d+)")
    ret.LogicParam = array_parser(t,"LogicParam_(%d+)")

    for k,v in pairs(t) do
        if not string.match(k,"(.-)_(%d+)") then
            ret[k] = v
        end
    end
    ret.__script_type = "skill"
    return ret
end

local function ReadParamTab(paramId,key)
    local tab = tabMgr.GetByID("SkillParams",paramId)
    if tab == nil then
        error("SkillParams not found." .. paramId)
    end
    local index = string.match(key,"a(%d+)")
    return tab.A[tonumber(index)]
end

local tabTb = {
    hit = "SkillHit",
    impact = "Impact",
    skill = "SkillEx",
    skillBase = "SkillBase",
}

local mtTb = {
    hit = HitMt,
    impact = ImpactMt,
    skill = SkillMt,
    skillBase = SkillBaseMt,
}

local script_mt = {}

function script_mt.__tostring(t)
    setmetatable(t,nil)
    local str = string.format("ss[%d][%s][%s].",t.Id,t.__script_type,tostring(t))
    setmetatable(t,script_mt)
    return str
end

function script_mt.__index(t,k)
    local rawTab = rawget(t,"__tab")
    if rawTab ~= nil then
        return rawTab[k]
    end

    local internal = rawget(t,"__internal_tab")
    return internal[k]
end

local function SetMeta(r,t,tabId)
    
    if tabId ~= nil then
        local tabName = tabTb[t.__script_type]
        local tab = tabMgr.GetByID(tabName,tabId)
        r.__tab = tab
    end
    r.__internal_tab = mtTb[rawget(t,"__script_type")]

    setmetatable(r,script_mt)
end

local function IsScriptTab(t)
    return t.__script_type ~= nil
end

--递归解析技能脚本，生成一个新的table
--新的table把需要动态替换的数据替换
--生成临时id
local function deepParse(t,data,tabId)
    if t == nil then
        return nil
    end

    if type(t) ~= 'table' then
        return nil
    end

    local r = {}
    local paramId = data.paramId
    if data.root == nil then
        data.root = r
    end

    r.root = data.root

    --是脚本化的配置，还是普通数组
    if IsScriptTab(t) then

        if tabId == nil then     
            if t.Id == nil then
                --id生成
                local baseId = paramId
                if baseId < 0 then baseId = 1 end
                r.Id = -1 * (baseId * 100 + data.iter)
            else
                --根据对应的表格，设置元表
                local ttype = type(t.Id)
                tabId = t.Id
                if ttype == "string" then
                    tabId = ReadParamTab(paramId,tabId)
                end
            end
        end

        SetMeta(r,t,tabId)

        data.iter = data.iter + 1
    elseif t.__dynamic then
        data.iter = data.iter + 1

        setmetatable(r,t)

        r.root.dynamic = true
    end

    for k,v in pairs(t) do
        --参数替换
        local ttype = type(v)
        if ttype == 'string' and not string.match(k,"__(.+)") then
            r[k] = ReadParamTab(paramId,v)
        elseif ttype == 'table' then
            if data.parsed[v] ~= nil then
                r[k] = data.parsed[v]
            else
                --print(k)
                r[k] = deepParse(v,data)
            end
        else
            r[k] = v
        end
    end

    if IsScriptTab(t) then
        data.parsed[t] = r
    end

    return r
end


local selector = function(t)
    local selected = rawget(t,"__selected")
    if selected == nil then
        selected = t.__select_func(t,t.__items)
        rawset(t,"__selected",selected)
    end
    return selected
end

local function selecotr_index(t,k)
    local selected = selector(t)
    --print(selected,k)
    return selected[k]
end

local function selecotr_tostring(t)
    local selected = selector(t)
    return tostring(selected)
end

local select = function(t,select_func)
    local ret = {}
    ret.__items = t
    ret.__dynamic = true
    ret.__index = selecotr_index
    ret.__tostring = selecotr_tostring

    ret.__select_func = select_func

    return ret
end

local static_cache = {}

local ParseSkill = function(tabEx,skillProcess)
    if static_cache[tabEx.Id] ~= nil then
        --print("static")
        return static_cache[tabEx.Id]
    end

    local scriptSkill = require("ScriptSkill/SS_" .. tabEx.ScriptID)
    local paramId = tabEx.ScriptParamID
    local sk = deepParse(scriptSkill,{
        paramId = paramId,
        iter = 1,
        parsed = {}
    },tabEx.Id)

    if not sk.dynamic then
        --静态技能缓存，动态技能每次需要重构
        static_cache[tabEx.Id] = sk
        --print("static cached")
    else
        sk.root.process = skillProcess
        --print("dynamic")
    end

    return sk
    
end

_G.i_mk = Impact_mk
_G.h_mk = Hit_mk
_G.sk_mk = Skill_mk
_G.ib_mk = function(t)
    local ret = Impact_mk(t)
    ret.Duration = ret.Duration or 1
    return ret
end

_G.select = select

_G.chain_mk = function(list)
    local last = nil
    for _,t in ipairs(list) do
        if last ~= nil then
            last.ChildImpact = t
        end
        last = t
    end
end


local selectByChance = function(t,items)
    if items == nil then
        return nil
    end
    local root = t.root
    local process = root.process
    if process == nil then
        error("can not get skillProcess.")
        return nil
    end
    return process.battle:RandomSelectOne_Chnace(items)
end

return {
    Parse = ParseSkill,
    deepParse = deepParse,

    select_by_chance = function(t)
        return select(t,selectByChance)
    end,
}