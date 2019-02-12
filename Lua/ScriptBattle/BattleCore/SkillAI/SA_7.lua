--任意人血量低于50%
--句芒3技能
--狄仁杰3技能
--叶执明3技能
--素馨2技能
local SkillAI = {
	--使用条件
	ShouldUse = function(R)
		return R:AI_ShouldUse_AnyHPLe(5000)
	end,
	IsPriorityTarget = function(R)
		return R:AI_ShouldUse_HPLe(5000) --血量万分比小于等于
	
	end,
}
return SkillAI