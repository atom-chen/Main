
--角色属性
--为了加快速度，很多方法调用不走元表查询，因此继承RoleAttr或者RoleAttrTb会出现重载失败的情况
--最好不要继承这俩个类

local AttrType = require("BattleCore/Common/AttrType")

--隐藏属性，从100开始，不能喝AttrType冲突了
local HidenAttrType = {
    DamageReflect = 100, --伤害反弹
    DrainLife = 101, --吸血
    DamageReflectChance = 102, --伤害反弹概率

    NeverHeal = 103, --不可治疗
    SilentAtk = 104, --攻击时不会打醒睡眠角色

    NormalAtkDamageReduce = 105, --普攻伤害减免
	CritChanceResist = 106, --暴击抵抗
}

local AttrLimit = {
    [AttrType.MaxHP] = {
        min = 1,
        minPercent = 5000,
    },
    [AttrType.Speed] = {
        min = 1,
        max = 999,
    },
    [AttrType.Attack] = {
        min = 0,
    },
    [AttrType.Defense] = {
        min = 0,
    },
    [AttrType.CritChance] = {
        min = 0,
    },
    [AttrType.CritEffect] = {
        min = 0,
    },
    [HidenAttrType.DrainLife] = {
        min = 0,
        max = 10000,
    },
	[HidenAttrType.CritChanceResist] = {
        min = 0,
        max = 10000,
    },
}

local mkRefix = function()
    return {
        attrType = 0,
        addition = 0,
        percent = 0,
    }
end

--单一一条属性
local RoleAttr = {}
RoleAttr.__mt = {__index = RoleAttr}

--属性集合
local RoleAttrTb = {}
RoleAttrTb.__mt = {__index = RoleAttrTb}

function RoleAttr.Create(attrType,baseValue)
    local t = {}
    setmetatable(t, RoleAttr.__mt)
    t.attrType = attrType
    t.baseValue = baseValue
    t.dirty = false
    t.value = t.baseValue
    return t
end

function RoleAttr:SetDirty()
    self.dirty = true
end

function RoleAttr:SetBaseValue(baseValue)
    self.baseValue = baseValue
    self.dirty = true
end

function RoleAttr:GetValue()
    if self.dirty then
        RoleAttr.ReCalcValue(self)
    end
    return self.value
end

local isValidRefix = function(refix)

    if refix.impact ~= nil and not refix.impact:IsActiveValid() then
        return false
    end

    return true
end

function RoleAttr:ReCalcValue()
    self.dirty = false
    local refixList = self.refixList
    local baseValue = self.baseValue
    if refixList == nil then
        self.value = baseValue
        return baseValue
    end

    local percent = 10000
    local add = 0

    for _,refix in ipairs(refixList) do
        if isValidRefix(refix) then
            percent = percent + refix.percent
            add = add + refix.addition
        end
    end

    self.value = math.ceil(baseValue * (percent / 10000)) + add
    local limit = AttrLimit[self.attrType]
    if limit ~= nil then
        if limit.min ~= nil then
            self.value = math.max(self.value,limit.min)
        end
        if limit.minPercent ~= nil then
            self.value = math.max(self.value, math.ceil( (limit.minPercent  / 10000) * baseValue) )
        end
        if limit.max ~= nil then
            self.value = math.min(self.value,limit.max)
        end
    end
end

function RoleAttr:AddRefix(refix)
    if self.refixList == nil then
        self.refixList = {}
    end
    table.insert( self.refixList,refix)
    self.dirty = true
end

function RoleAttr:RemoveRefix(refix)
    local refixList = self.refixList
    if refixList == nil then
        return
    end
    for i,r in ipairs(refixList) do
        if r == refix then
            table.remove( refixList,i)
            break
        end
    end
    self.dirty = true
end



function RoleAttrTb.Create()
    local t = {}
    setmetatable(t,RoleAttrTb.__mt)
    t.attrs = {}
    return t
end

function RoleAttrTb:Init(msg)
    for _,v in pairs(AttrType) do
        local attr = RoleAttr.Create(v,0)
        self.attrs[v] = attr
    end

    for _,v in pairs(HidenAttrType) do
        local attr = RoleAttr.Create(v,0)
        self.attrs[v] = attr
    end

    if msg == nil then
        return
    end

    for _,item in ipairs(msg.attrs) do
        local attr = self.attrs[item.type]
        if attr ~= nil and item.value ~= nil then
            RoleAttr.SetBaseValue(attr,item.value)
            --attr:SetBaseValue(item.value)
        else
            print('no such attr:',item.type)
        end
    end
end

function RoleAttrTb:SetBaseValue(attrType,val)
    local attr = self.attrs[attrType]
    if attr ~= nil then
        RoleAttr.SetBaseValue(attrType,val)
        --attr:SetBaseValue(val)
    end
end

function RoleAttrTb:GetValue(attrType)
    local attr = self.attrs[attrType]
    --return attr:GetValue()
    return RoleAttr.GetValue(attr)
end

function RoleAttrTb:AddRefix(refix)
    local attr = self.attrs[refix.attrType]
    if attr ~= nil then
        --attr:AddRefix(refix)
        RoleAttr.AddRefix(attr,refix)
    end
end

function RoleAttrTb:RemoveRefix(refix)
    local attr = self.attrs[refix.attrType]
    if attr ~= nil then
        --attr:RemoveRefix(refix)
        RoleAttr.RemoveRefix(attr,refix)
    end
end

function RoleAttrTb:SetDirty(attrType)
    if attrType == nil then
        self:SetDirtyAll()
    else
        local attr = self.attrs[attrType]
        if attr ~= nil then
            --attr:AddRefix(refix)
            RoleAttr.SetDirty(attr)
        end
    end
end

function RoleAttrTb:SetDirtyAll()
    for _,v in pairs(self.attrs) do
        v:SetDirty()
    end
end

--test

-- local r = mkRefix()
-- r.addition = 100

-- local a0 = RoleAttr.Create(1,100)
-- print(a0:GetValue())
-- a0:AddRefix(r)
-- print(a0:GetValue())

-- local r2 = mkRefix()
-- r2.percent = 5000
-- a0:AddRefix(r2)
-- print(a0:GetValue())

-- a0:RemoveRefix(r)
-- print(a0:GetValue())

-- local c1 = RoleAttrTb.Create()
-- c1:Init()
-- print(c1:GetValue(AttrType.MaxHP))

-- c1:AddRefix({attrType = AttrType.MaxHP,addition = 1000,percent = 0})
-- print(c1:GetValue(AttrType.MaxHP))
-- c1:AddRefix({attrType = AttrType.MaxHP,addition = 0,percent = 1000})
-- print(c1:GetValue(AttrType.MaxHP))
-- c1:SetBaseValue(AttrType.MaxHP,100)
-- print(c1:GetValue(AttrType.MaxHP))
-- c1:AddRefix({attrType = AttrType.MaxHP,addition = 0,percent = 3333})
-- print(c1:GetValue(AttrType.MaxHP))

return {
    CreateRefix = mkRefix,
    RoleAttr = RoleAttr,
    RoleAttrTb = RoleAttrTb,
    HidenAttrType = HidenAttrType,
}