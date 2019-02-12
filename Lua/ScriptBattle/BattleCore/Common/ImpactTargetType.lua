-- Generate Code,do not edit

--impact作用目标
local t = {
    SkillTarget = 1, -- 技能目标
    SkillCaster = 2, -- 技能释放者
    OurAll = 3, -- 我放全体
    EnemyAll = 4, -- 对方全体
    OurRandom = 5, -- 我方随机
    EnemyRandom = 6, -- 敌方随机
    OurHpMin = 9, -- 我方血量最小
    EnemyHpMin = 10, -- 敌方血量最小
    MarkedEnemy = 11, -- 特定buff标记过的
    Splash = 12, -- 弹射伤害
    OurRandomHasBuff = 13, -- 随机我方有特定buff
    EnemyRandomHasBuff = 14, -- 随机敌方有特定buff
    EnemyHpMax = 15, -- 敌方血量最多
    OurRandomByCardId = 16, -- 我方随机某个特定卡牌
    OurHpPercentMin = 17, -- 我方血量百分比最小
    OurApMin = 18, -- 我方AP最小
    
}
return t