--自己血量低于60%（或者任意单体血量低于40%或者全体血量低于50%）
local SkillAI = {
	--使用条件
	ShouldUse = function(R)
		return R:AI_ShouldUse_HPLe(6000)
		or R:AI_ShouldUse_AnyHPLe(4000)
		or R:AI_ShouldUse_OurAverageHPLe(5000)
	end,
}
return SkillAI