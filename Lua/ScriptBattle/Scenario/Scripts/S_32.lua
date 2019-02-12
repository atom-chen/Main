--声明参与的演员
local Actors = 
{
    --怪物，我方指定站位角色，使用MonsterID
	baihu = 10326,
}
-----------------------------------------------------

--剧情开始
local Sections = 
{
    {
        Name = "白虎加入战斗",
        Trigger = function(S)
            return S:IsRoundBegin(1,1)
        end,

        Events = 
		{
            {
                EventType = "Idle",
                Time = 1,
            },			
            {
                EventType = "Spawn",
                Side = 1,               --我方
                BattlePos = 4,          --0号位        
			    MonsterId = Actors.baihu,
                IsPlayer = true,       --玩家能否操作
            },
            -- {
            --     EventType = "PlayStoryContent",
            --     SC_Id = 9130,
            -- },
        },
    },
	

}

return Sections