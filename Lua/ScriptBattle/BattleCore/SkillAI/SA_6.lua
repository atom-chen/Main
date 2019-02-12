--群体加血-全队平均血量低于70%或任意人血量低于50%
--素馨3技能
--扫晴娘3技能未觉醒
--鲛2技能觉醒
local SkillAI = {
	--使用条件
	ShouldUse = function(R)
		return R:AI_ShouldUse_OurAverageHPLe(7000)
		or R:AI_ShouldUse_AnyHPLe(5000)
	end,
}
return SkillAI