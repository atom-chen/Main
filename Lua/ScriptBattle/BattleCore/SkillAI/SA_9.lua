--是否换界
local SkillAI = {
	--使用条件
	ShouldUse = function(R)
		return R:AI_ShouldUse_OurAllBetterEnv(0) --处于我方优势环境(我方优势符灵数大于敌方优势符灵数）（1是0否-1不关心）
	end,
}
return SkillAI