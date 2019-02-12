--处于妖界
local SkillAI = {
	--使用条件
	ShouldUse = function(R)
		return R:AI_ShouldUse_IsEnv(1) --是否处于某个环境（0人1妖-1不考虑）
	end,
}
return SkillAI