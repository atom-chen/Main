-- Generate Code,do not edit

--战斗事件
local t = {
    None = 0, -- 
    UseSkill = 1, -- 使用技能
    Hit = 2, -- 
    RoundAP = 3, -- 
    ImpactsChange = 4, -- 
    Kill = 5, -- 
    ChangeSpirit = 6, -- 
    ChangeEnv = 7, -- 
    DeleteRole = 8, -- 
    AddRole = 9, -- 
    SyncState = 10, -- 
    RoundBegin = 11, -- 
    Round = 12, -- 
    RoundEnd = 13, -- 
    WaveStart = 14, -- 
    WaveEnd = 15, -- 
    BattleStart = 16, -- 战斗开始
    Prepare = 17, -- 战斗准备
    DropItem = 18, -- 战斗内掉落
    Pause = 19, -- 暂停
    PlayCutscene = 20, -- 动画
    Idle = 21, -- 空闲
    Scenario = 22, -- 剧情
    RoleActionEvent = 23, -- 角色表现
    PlayStoryContentEvent = 24, -- 播放半身像对话
    SkillCooldownChangeEvent = 25, -- CD变化
    NoticeEvent = 26, -- 提示
    ChangeSkills = 27, -- 技能改变
    ParallelEvent = 28, -- 平行事件
    PauseEx = 29, -- 暂停特殊
    Bubble = 30, -- 喊话
    SetAITarget = 31, -- 集火目标
    
}
return t