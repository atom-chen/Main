--启动文件

--这里做全局的初始化

local lua_root = './ScriptBattle/'

if not _G.__add_path then
    package.path = package.path .. ";".. lua_root .. "?.lua"
    _G.__add_path = true
end

local tabMgr = require("TabManager")
tabMgr.Clear()

gServer = true

--如果是多人战斗，会通过init赋值这个bool值
--gMultiPlayer = false

--注册函数
tabMgr.RegistTabReader({
    ["SkillEx"] = C_GetSkillEx,
    ["SkillBase"] = C_GetSkillBase,
    ["Battle"] = C_GetBattle,
    ["BattleWave"] = C_GetBattleWave,
    ["SkillHit"] = C_GetSkillHit,
    ["Impact"] = C_GetImpact,
    ['RoleAttrEx'] = C_GetRoleAttrEx,
    ['RoleBaseAttr'] = C_GetRoleBaseAttr,
    ['Monster'] = C_GetMonster,
    ['BattleCutscene'] = C_GetBattleCutscene,
    ['AI'] = C_GetAI,
    ['SkillAI'] = C_GetSkillAI,    
    ['SummonMonster'] = C_GetSummonMonster,    
    ['SkillLevels'] = C_GetSkillLevels,    
    ['BattleBubbleCard'] = C_GetBattleBubbleCard,    
    ['SkillParams'] = C_GetSkillParams, 
	['ArenaLevel'] = C_GetArenaLevel,
	['AvatarBinding'] = C_GetAvatarBinding,	
})

--不要缓存
tabMgr.SetCache(false)

--注册pb
local pb = require ('protobuf')
pb.register_file(lua_root .. 'BattleMessage.pb')

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
    local file_path = assert(package.searchpath(name, package.path))
    local fp = assert(io.open(file_path))
    local chunk = fp:read("*all")
    fp:close()
    return chunk
end


function x0001_relua(path)
    local count = C_GetScriptCount()
    if count == nil then
        return
    end
    
    for i=0,count-1 do
        local path = C_GetScriptPath(i)
        if path ~= nil then
            hotfix.hotfix_module(path)
        end
    end

end

--多人战斗，额外的初始化
function x0001_InitMultiPlayer()
    gMultiPlayer = true
end