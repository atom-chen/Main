--第一章第三回穷奇祸斗boss战
--声明参与的演员
local Actors = {
    hero = 3,

    --怪物，我方指定站位角色，使用MonsterID
    monster_1 = 10215,

    --卡牌，指定卡牌ID
    myCard_1 = {CardId = 19},
    myCard_2 = {CardId = 11},
}
-----------------------------------------------------

--剧情开始
local Sections = {
    {
        Name = "开战对话",
        Trigger = function(S)
            return S:IsRoundBegin(1,1)
        end,

        Events = {
            {
                EventType = "Idle",
                Time = 1,
            },
            {
                EventType = "PlayStoryContent",
                SC_Id = 101,
            },
        },
     },
  --{
        --Name = "第四回合玄武登场",
        --Trigger = function(S)
            --return S:IsRoundEnd(1,4)
        --end,

        --Events = {
          -- {
                --EventType = "Spawn",
               -- Side = 1,               --我方
               -- BattlePos = 1,          --1号位
               -- MonsterId = 10215,          --id为4的怪
               -- IsPlayer = true,       --玩家能否操作
                --IsPlaySpawnAnim = true,
           -- },
            --{
                --EventType = "Idle",
                --Time = 2,
            --},
            --{
                --EventType = "Idle",
               -- Time = 1,
            --},
            --{
                --EventType = "SendImpact",
--ImpactId = 1131011,
               -- Target = Actors.monster_1,
            --},
        --},
    --},
}

return Sections