--有标记buff时释放，优先攻击血量少的目标

local SkillAI = {
	--使用条件
	ShouldUse = function(R)
		return R:AI_IsOurAllBuffCountByImpactImpactId(1274010,0,0)--我方特定buff数量和是否大于/小于指定数量 AI_IsOurAllBuffCountByImpactImpactId(ImpactImpactId, count, oper, enemy)oper:0-大于 1-小于 enemy: true敌方 false 我方
	end,
	--优先目标
	IsPriorityTarget = function(R)
		return  R:AI_ShouldUse_HPLe(5000) --血量万分比小于等于
		
	end,
}
return SkillAI