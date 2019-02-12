--自爆-自己血量低于33%
local SkillAI = {
	--使用条件
	ShouldUse = function(R)
		return R:AI_ShouldUse_HPLe(3333)
	end,
}
return SkillAI