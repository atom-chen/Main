--敌方有灼伤debuff时释放，优先攻击有灼伤buff的目标
--罗刹女2技能觉醒
--祝融女3技能
--祝融女3技能觉醒
local SkillAI = {
	--使用条件
	ShouldUse = function(R)
		return R:AI_IsOurAllBuffCountByImpactImpactId(911001,0,0,true)--我方特定buff数量和是否大于/小于指定数量 AI_IsOurAllBuffCountByImpactImpactId(ImpactImpactId, count, oper, enemy)oper:0-大于 1-小于 enemy: true敌方 false 我方
	end,
	--优先目标
	IsPriorityTarget = function(R)
		return  R:AI_IsBuffCountByImpactImpactId(911001,0,0)--指定buff数小于/大于指定数量（不包括等于） AI_IsBuffCountByImpactImpactId(ImpactImpactId, count, oper)oper:0-大于 1-小于
		
	end,
}
return SkillAI