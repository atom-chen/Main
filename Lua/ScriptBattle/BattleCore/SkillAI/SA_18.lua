--敌方有灼伤debuff时释放，优先攻击有灼伤buff的目标
--罗刹女2技能

local SkillAI = {
    	--使用条件
	ShouldUse = function(R)
		return R:AI_ShouldUse_HPLe(10000)

	end,
	--优先目标
	IsPriorityTarget = function(R)
		return  R:AI_IsBuffCountByImpactImpactId(911001,0,0)--指定buff数小于/大于指定数量（不包括等于） AI_IsBuffCountByImpactImpactId(ImpactImpactId, count, oper)oper:0-大于 1-小于
		
	end,
}
return SkillAI