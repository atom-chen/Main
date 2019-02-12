--群体加血-全队平均血量低于70%或任意人血量低于50%或我方减益效果数量大于3个
--扫晴娘3技能觉醒
--
local SkillAI = {
	--使用条件
	ShouldUse = function(R)
		return R:AI_ShouldUse_OurAverageHPLe(7000)
        or R:AI_ShouldUse_AnyHPLe(5000)
        or R:AI_IsOurAllBuffCountByImpactClass(4,2,0) --impactClass, count, oper:0-大于 1-小于
	end,
}
return SkillAI