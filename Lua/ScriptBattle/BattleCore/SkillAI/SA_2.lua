--自己血量低于50%
local SkillAI = {
	--使用条件
	ShouldUse = function(R)
		return R:AI_ShouldUse_HPLe(5000)
	end,
}
return SkillAI