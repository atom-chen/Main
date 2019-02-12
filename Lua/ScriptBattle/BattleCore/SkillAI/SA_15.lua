--是否有buff
local SkillAI = {
	--使用条件
	ShouldUse = function(R)
		return R:AI_IsOurAllBuffCountByImpactClass(1,0,0) --impactClass, count, oper:0-大于 1-小于
	end,
}
return SkillAI