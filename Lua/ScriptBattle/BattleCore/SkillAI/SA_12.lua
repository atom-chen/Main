--任意人血量低于50%或者debuff数量大于1
--药童2技能
local SkillAI = {
	--使用条件
	ShouldUse = function(R)
		return R:AI_ShouldUse_AnyHPLe(5000)
		or R:AI_IsOurAllBuffCountByImpactClass(4,1,0) --impactClass, count, oper:0-大于 1-小于
	end,
		--优先目标
	IsPriorityTarget = function(R)
		return  R:AI_ShouldUse_HPLe(5000) --血量万分比小于等于
		or R:AI_IsBuffCountByImpactClass(4,1,0) --指定buff数小于/大于指定数量（不包括等于）  AI_IsBuffCountByImpactSubClass(ImpactSubClass, count, oper)oper:0-大于 1-小于
		
	end,
}
return SkillAI