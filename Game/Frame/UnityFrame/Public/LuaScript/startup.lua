--启动文件

--这里做全局的初始化

--package.path = package.path .. ";./Battle/?.lua"

local tabMgr = require("TabManager")
tabMgr.Clear()

local r = CS.LuaTabReader

gClientBattle = true

--注册函数
tabMgr.RegistTabReader({
    ["SkillEx"] = r.C_GetSkillEx,
    ["SkillBase"] = r.C_GetSkillBase,
    ["Battle"] = r.C_GetBattle,
    ["BattleWave"] = r.C_GetBattleWave,
    ["SkillHit"] = r.C_GetSkillHit,
    ["Impact"] = r.C_GetImpact,
    ['RoleAttrEx'] = r.C_GetRoleAttrEx,
    ['RoleBaseAttr'] = r.C_GetRoleBaseAttr,
    ['Monster'] = r.C_GetMonster,
    ['BattleCutscene'] = r.C_GetBattleCutscene,
    ['AI'] = r.C_GetAI,
    ['SkillAI'] = r.C_GetSkillAI,    
    ['SummonMonster'] = r.C_GetSummonMonster,    
    ['SkillLevels'] = r.C_GetSkillLevels,    
    ['BattleBubbleCard'] = r.C_GetBattleBubbleCard,    
    ['SkillParams'] = r.C_GetSkillParams,    
})

--tabMgr.SetCache(false)

--注册pb
local pb = require ('protobuf')
local loader = CS.LuaGlobalFunc.CSharp_ReadFromStreamingAsset
local pbBuf = loader(lua_root .. 'BattleMessage.pb')
pb.register(pbBuf)

require('BattleCore')
require("BattleSimulator")

local hotfix = require("hotfix")
hotfix.log_error = function( msg )
    print(msg)
end

hotfix.log_info = function( msg )
    --print(msg)
end

hotfix.log_debug = function( msg )
    --print(msg)
end

hotfix.loader = function(name)
    -- local file_path = assert(package.searchpath(module_name, package.path))
    -- local fp = assert(io.open(file_path))
    -- local chunk = fp:read("*all")
    -- fp:close()
    return loader(lua_root .. name .. ".lua")
end

function relua(path)
    hotfix.hotfix_module(path) 
end