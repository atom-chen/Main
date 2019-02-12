--任意人血量低于80%
local SkillAI = {
	--使用条件
	ShouldUse = function(R)
		return R:AI_ShouldUse_AnyHPLe(8000)
	end,
}
return SkillAI