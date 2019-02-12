--SkillAI范例
local SkillAI = {
	--使用条件
	ShouldUse = function(R)
		return R:AI_ShouldUse_HPMore(5000) --自己血量万分比大于等于
		--and R:AI_ShouldUse_HPLe(5000) --自己血量万分比小于等于
		--and R:AI_ShouldUse_AnyHPLe(5000) --任意一员，血量小于等于（合法技能目标中）
		--and R:AI_ShouldUse_IsEnv(1)	--是否处于某个环境下
		--and R:AI_ShouldUse_OurAllBetterEnv(1) --是否处于我方优势环境下，根据我方优势符灵数量，对比对方优势符灵数量
		--and R:AI_ShouldUse_OurRoleDead(0) --我方角色死亡数量大于等于
		--and R:AI_ShouldUse_OurAverageHPLe(7000) --全员平均血量，小于等于
		--and R:AI_IsOurAllBuffCountByImpactClass(4,0,0) --我方特定buff数量和是否大于/小于指定数量 AI_IsOurAllBuffCountByImpactClass(ImpactClass, count, oper, enemy)oper:0-大于 1-小于 enemy: true敌方 false 我方
		--and R:AI_IsOurAllBuffCountByImpactSubClass(256,0,0)--我方特定buff数量和是否大于/小于指定数量 AI_IsOurAllBuffCountByImpactSubClass(ImpactSubClass, count, oper, enemy)oper:0-大于 1-小于 enemy: true敌方 false 我方
		--and R:AI_IsOurAllBuffCountByImpactImpactId(11,0,0)--我方特定buff数量和是否大于/小于指定数量 AI_IsOurAllBuffCountByImpactImpactId(ImpactImpactId, count, oper, enemy)oper:0-大于 1-小于 enemy: true敌方 false 我方
	end,
	--优先目标
	IsPriorityTarget = function(R)
		return R:AI_ShouldUse_HPMore(5000) --血量万分比大于等于
		--and R:AI_ShouldUse_HPLe(10000) --血量万分比小于等于
		--and R:AI_IsBuffCountByImpactSubClass(1,1,0) --指定buff数小于/大于指定数量（不包括等于）  AI_IsBuffCountByImpactSubClass(ImpactSubClass, count, oper)oper:0-大于 1-小于
		--and R:AI_IsBuffCountByImpactImpactId(1001,1,1)--指定buff数小于/大于指定数量（不包括等于） AI_IsBuffCountByImpactImpactId(ImpactImpactId, count, oper)oper:0-大于 1-小于
		--and R:AI_IsBuffCountByImpactClass(4,1,2)--指定buff数小于/大于指定数量（不包括等于） AI_IsBuffCountByImpactClass(ImpactClass, count, oper)oper:0-大于 1-小于
	end,
}
return SkillAI