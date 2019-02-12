--声明参与的演员
local Actors = 
{
    --怪物，我方指定站位角色，使用MonsterID
	shenyao = 10272,
}
-----------------------------------------------------

--剧情开始
local Sections = 
{
    {
        Name = "蜃珧加入战斗",
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
			    MonsterId = Actors.shenyao,
                IsPlayer = true,       --玩家能否操作
            },
            {
                EventType = "PlayStoryContent",
                SC_Id = 9130,
            },
        },
    },
	
	{
        Name = "蜃珧加BUFF-5回合",
        Trigger = function(S)
            return S:IsRoundBegin(1,5)
        end,

        Events = 
		{
            {
                EventType = "Idle",
                Time = 1,
            },
			{
                EventType = "PlayStoryContent",
                SC_Id = 9153,
            },
            {
                EventType = "SendImpact",
                ImpactId = 11,
				Target = Actors.shenyao,
            },
			{
                EventType = "Idle",
                Time = 2.2,
            },
        },
    },

}

return Sections