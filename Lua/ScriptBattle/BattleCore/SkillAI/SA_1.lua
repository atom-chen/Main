
--全队平均血量低于50%
local SkillAI = {
	--使用条件
	ShouldUse = function(R)
		return R:AI_ShouldUse_OurAverageHPLe(5000)
	end,
}
return SkillAI