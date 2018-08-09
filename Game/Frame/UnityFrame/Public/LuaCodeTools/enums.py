# -*- coding: utf-8 -*-
enums = [
    {
        'name' : 'AttrType',
        'comment' : u"属性类型",
        'cases' : [
            {'name':'MaxHP','value':0,'comment':u'血上限'},
            {'name':'Speed','value':1,'comment':u'速度'},
            {'name':'Attack','value':2,'comment':u'攻击力'},
            {'name':'Defense','value':3,'comment':u'防御力'},
            {'name':'CritChance','value':4,'comment':u'暴击率'},
            {'name':'CritEffect','value':5,'comment':u'暴击效果'},
            {'name':'DmgEnhance','value':6,'comment':u'伤害加成'},
            {'name':'DmgReduce','value':7,'comment':u'伤害减免'},
            {'name':'HealEnhance','value':8,'comment':u'治疗加成'},
            {'name':'HealRecvEnhace','value':9,'comment':u'受到的治疗效果加成'},
            {'name':'ImpactChance','value':10,'comment':u'效果命中'},
            {'name':'ImpactResist','value':11,'comment':u'效果抵抗'},
        ]
    },
    {
        'name' : 'AttrRefixType',
        'comment' : u"属性修正类型",
        'cases' : [
            {'name':"MaxHPAdd","value":1,"comment":u"血上限加法"},
            {'name':"MaxHPPercent","value":2,"comment":u"血上限百分比"},
            {'name':"MaxHPFinal","value":3,"comment":u"血上限最终百分比"},

            {'name':"SpeedAdd","value":4,"comment":u"速度加法"},
            {'name':'SpeedPercent','value':5,'comment':u'速度百分比'},
            {'name':'SpeedFinal','value':6,"comment":u"速度最终百分比"},

            {'name':'AttackAdd','value':7,'comment':u'攻击力加法'},
            {'name':'AttackPercent','value':8,'comment':u'攻击力百分比'},
            {'name':'AttackFinal','value':9,'comment':u"攻击力最终百分比"},

            {'name':'DefenseAdd','value':10,'comment':u'防御力加法'},
            {'name':'DefensePercent','value':11,'comment':u'防御力百分比'},
            {'name':'DefenseFinal','value':12,'comment':u'防御力最终百分比'},

            {'name':'CritChanceAdd','value':13,'comment':u'暴击率加法'},
            {'name':'CritEffectAdd','value':14,'comment':u'暴击效果加法'},
            {'name':'DmgEnhanceAdd','value':15,'comment':u'伤害加成加法'},
            {'name':'DmgReduceAdd','value':16,'comment':u'伤害减免加法'},

            {'name':'ImpactChanceAdd','value':17,'comment':u'效果命中'},
            {'name':'ImpactResistAdd','value':18,'comment':u'效果抵抗'},
        ]
    },
    {
        'name' : "BattleCommonData",
        "comment" : u"战斗里用的常量",
        'cases' : [
            {'name':'AP_FULL','value':1000,'comment':u'行动力满'},
            {'name':'Max_BattlePos','value':6,'comment':u'最多出战角色'},
            {'name':'SP_FULL','value':3000,'comment':u'怒气满'},
            {'name':'SUNMMON_POS_NUM','value':2,'comment':u'召唤位'}
        ]
    },
    {
        'name' : "BattleEventType",
        'comment' : u'战斗事件',
        'cases': [
            {'name':'None','value':0,'comment':u''},
            {'name':'UseSkill','value':1,'comment':u'使用技能'},
            {'name':'Hit','value':2,'comment':u''},
            {'name':'RoundAP','value':3,'comment':u''},
            {'name':'ImpactsChange','value':4,'comment':u''},
            {'name':'Kill','value':5,'comment':u''},
            {'name':'ChangeSpirit','value':6,'comment':u''},
            {'name':'ChangeEnv','value':7,'comment':u''},
            {'name':'DeleteRole','value':8,'comment':u''},
            {'name':'AddRole','value':9,'comment':u''},
            {'name':'SyncState','value':10,'comment':u''},
            {'name':'RoundBegin','value':11,'comment':u''},
            {'name':'Round','value':12,'comment':u''},
            {'name':'RoundEnd','value':13,'comment':u''},
            {'name':'WaveStart','value':14,'comment':u''},
            {'name':'WaveEnd','value':15,'comment':u''},
            {'name':"BattleStart",'value':16,'comment':u"战斗开始"},
            {'name':"Prepare",'value':17,'comment':u"战斗准备"},
            {'name':"DropItem",'value':18,'comment':u"战斗内掉落"},
            {'name':"Pause",'value':19,'comment':u'暂停'},
            {'name':'PlayCutscene','value':20,'comment':u'动画'},
            {'name':"Idle","value":21,'comment':u'空闲'},
            {'name':"Scenario","value":22,'comment':u'剧情'},
            {'name':"RoleActionEvent","value":23,'comment':u'角色表现'},
            {'name':"PlayStoryContentEvent","value":24,'comment':u'播放半身像对话'},
            {'name':"SkillCooldownChangeEvent","value":25,'comment':u'CD变化'},
            {'name':"NoticeEvent","value":26,'comment':u'提示'},
            {'name':"ChangeSkills","value":27,'comment':u'技能改变'},
            {'name':"ParallelEvent","value":28,'comment':u'平行事件'},
            {'name':"PauseEx","value":29,'comment':u'暂停特殊'},
            {'name':"Bubble",'value':30,'comment':u'喊话'}
        ]
    },
    {
        'name' : "BattleSide",
        'comment' : u'阵营',
        'cases' : [
            {'name':'bs_Invalid','value':-1,'comment':u'无效'},
            {'name':'bs_Blue','value':1,'comment':u'红方'},
            {'name':'bs_Red','value':2,'comment':u'蓝方'},
            {'name':'bs_Neutrality','value':3,'comment':u'中立'},
            {'name':'bs_None','value':4,'comment':u'无'},
        ]
    },
    {
        'name' : "ImpactTargetType",
        'comment' : u'impact作用目标',
        'cases' : [
            {'name':'SkillTarget','value':1,'comment':u'技能目标'},
            {'name':'SkillCaster','value':2,'comment':u'技能释放者'},
            {'name':'OurAll','value':3,'comment':u'我放全体'},
            {'name':'EnemyAll','value':4,'comment':u'对方全体'},
            {'name':'OurRandom','value':5,'comment':u'我方随机'},
            {'name':'EnemyRandom','value':6,'comment':u'敌方随机'},
            # {'name':'OurRandomWithTarget','value':7,'comment':u'敌方随机必包含目标'},
            # {'name':'EnemyRandomWithTarget','value':8,'comment':u'敌方随机必包含目标'},
            {'name':'OurHpMin','value':9,'comment':u'我方血量最小'},
            {'name':'EnemyHpMin','value':10,'comment':u'敌方血量最小'},
            {'name':'MarkedEnemy','value':11,'comment':u'特定buff标记过的'},
            {'name':'Splash','value':12,'comment':u'弹射伤害'},
            {'name':'OurRandomHasBuff','value':13,'comment':u'随机我方有特定buff'},
            {'name':'EnemyRandomHasBuff','value':14,'comment':u'随机敌方有特定buff'},
            {'name':'EnemyHpMax','value':15,'comment':u'敌方血量最多'},
            {'name':'OurRandomByCardId','value':16,'comment':u'我方随机某个特定卡牌'},
        ]
    },
    {
        'name' : 'SkillTargetType',
        'comment' : u"技能目标",
        'cases' : [
            {'name':'Enemy','value':1,'comment':u'敌方'},
            {'name':'Our','value':2,'comment':u'我方'},
            {'name':'Self','value':3,'comment':u'自己'},
            {'name':'OurDead','value':4,'comment':u'无目标'},
        ]
    },
    {
        'name' : "BattleAreaType",
        'comment' : u'战场区域类型',
        'no_gen' : ['csharp'],              #不生成C#版本，C#版本是来源
        'cases' : [
            {'name':'Blue','value':0},
            {'name':'BlueSummon','value':1},
            {'name':'Red','value':2},
            {'name':'RedSummon','value':3},
            {'name':'Neutrality','value':4},
            {'name':'BlueHero','value':5},
            {'name':'RedHero','value':6},
        ]
    },
    {
        'name' : "HitType",
        'cases' : [
            {'name':'Damage','value':1},
            {'name':'Heal','value':2},
            {'name':'CritDamage','value':3},
            {'name':'Miss','value':4},
            {'name':'Immue','value':5},
            {'name':'IncAp','value':6},
            {'name':'DescAp','value':7},
            {'name':'Interrupt','value':8,'comment':u'打断'},
            {'name':'GainSp','value':9,'comment':u'怒气积攒'},
            {'name':'CostSp','value':10,'comment':u'怒气消耗'},
            {'name':'Sacrifice','value':11,'comment':u'奉献(分血)'},
            {'name':'Guard','value':12,'comment':u'守护（分摊伤害）'},
            {'name':'Absorb','value':13,'comment':u'吸收'},
            {'name':'Dispell','value':14,'comment':u'驱散'},
            {'name':'Tips','value':15,'comment':u'通用字典提示'},
            {'name':'PassiveEffected','value':16,'comment':u'被动技能生效'},
            {'name':'BombBlast','value':17,'comment':u'炸弹爆炸伤害'},
            {'name':'EnvEnhance','value':18,'comment':u'环境加强'},
            {'name':'EnvResist','value':19,'comment':u'环境抵抗'},
            {'name':'MaxHPChange','value':20,'comment':u'血上限变化'},
            {'name':'EnvEnhanceCrit','value':21,'comment':u'环境加强暴击'},
            {'name':'EnvResistCrit','value':22,'comment':u'环境抵抗暴击'},
            {'name':'ActGain','value':23,'comment':u'下回合立即行动'},
            {'name':'ShiledChange','value':24,'comment':u'护盾变化'},
        ]
    },
    {
        'name' : "BattleState",
        'cases' : [
            {'name':'Invalid','value':-1},
            {'name':'Prepare','value':1},
            {'name':'BattleStart','value':2},
            {'name':'BattleEnd','value':3},
            {'name':'RoundBegin','value':4},
            {'name':'Round','value':5},
            {'name':'RoundEnd','value':6},
            {'name':'RoundStart','value':7},
            {'name':'Round0','value':8,'comment':u'特殊状态，“回合中”开始前插入特定输入'},
            {'name':'WaveStart','value':9}
        ]
    },
    # {
    #     'name' : "BattleMsgType",
    #     'cases' : [
    #         {'name':'AddRole','value':1},
    #         {'name':'WaveStart','value':2},
    #         {'name':'RoundBegin','value':3},
    #         {'name':'Round','value':4},
    #         {'name':'RoundEnd','value':5},
    #     ]
    # },
    {
        'name' : "CmdType",
        'cases' : [
            {'name':'UseSkill','value':1},
            {'name':'Skip','value':2},
            {'name':'HeroAct','value':3},
            {'name':'HeroUseSkill','value':4},
            {'name':'SetAITarget','value':5},
            {'name':'SetAuto','value':6},
        ]
    },
    {
        'name' : "AISkillStrategyType",
        'cases' : [
            {'name':'Normal','value':0},
            {'name':'NoSkill','value':1},
        ]
    },
    {
        'name' : "ImpactSubClass",
        'cases' : [
            {'name':'Stun','value':1,"comment":u'无法行动'},
            {'name':'Silence','value':2,'comment':u'沉默'},
            {'name':'Taunt','value':4,'comment':u'嘲讽'},
            {'name':'Chaos','value':8,'comment':u'混乱'},
            {'name':'Nothingness','value':16,'comment':u'无法被选中'},
            {'name':'Cursed','value':32,'comment':u'诅咒'},
            {'name':"DOT",'value':64,'comment':u'DOT'},
            {'name':"Fire",'value':128,'comment':u'灼烧'},
            {'name':'Ice','value':256,'comment':u'冰冻'},
            {'name':'Sleep','value':512,'comment':u'睡眠'},
            {'name':'Slow','value':1024,'comment':u'减速'},
            {'name':'Shiled','value':2048,'comment':u'护盾'},
        ]
    },
    {
        'name' : "ImpactTags",
        'cases' : [
            {'name':'NeverRelive','value':10001,'comment':u'不可复活'},
            {'name':'AlwaysDay','value':10002,'comment':u'始终处于阳界'},
            {'name':'AlwaysNight','value':10003,'comment':u'始终处于阴界'},
            {'name':'AlwaysEnvEnhance','value':10004,'comment':u'环境压制'},
        ]
    },
    {
        'name' : "ImpactClass",
        'cases' : [
            {'name':'Positive','value':1,"comment":u'增益'},
            {'name':'NormalDamage','value':2,'comment':u'普通伤害'},
            {'name':'Negative','value':4,'comment':u'减益'},
        ]
    },
    {
        'name' : "SkillClass",
        'cases' : [
            {'name':'Active','value':1,"comment":u'主动'},
            {'name':'Passive','value':2,'comment':u'被动'},
            {'name':'Attack','value':4,'comment':u'普攻'},
            {'name':'CanSilence','value':8,'comment':u'可被沉默'},
            {'name':'SpecailSkill','value':16,'comment':u'特殊技能'},
            {'name':'TalismanSkill','value':32,'comment':u'法宝被动'},
            {'name':'HeroPassive','value':64,'comment':u'主角被动'},
            {'name':'KarmaPassive','value':128,'comment':u'情缘被动'},
        ]
    },
    {
        'name' : "AutoFadeOutTag",
        'no_gen' : ['csharp','cplus'],
        'cases' : [
            {'name':'RecvDamage','value':1,"comment":u'受到攻击'},
            {'name':'SenderDead','value':2,'comment':u'发送者死亡'},
            {'name':'UseSkill','value':4,'comment':u'使用技能后（必须是自己的回合）'},
            {'name':"UseSkillAny",'value':8,'comment':u'使用任意技能（非自己回合也算）'},
            {'name':"SendSkillFin",'value':16,'comment':u'发送这个Impact的技能结束'},
            {'name':"SenderActEnd",'value':32,'comment':u'发送者行动结束'},
            {'name':"SenderRoundEnd",'value':64,'comment':u'发送者回合结束'},
        ]
    },
    {
        'name' : "ImpactDirtyType",
        'no_gen' : ['cplus'],
        'cases' : [
            {'name':'Add','value':1,"comment":u'添加'},
            {'name':'Remove','value':2,'comment':u'删除'},
            {'name':'Update','value':3,'comment':u'更新'},
        ]
    },
    {
        'name' : "RoleType",
        'no_gen' : ['cplus'],
        'cases' : [
            {'name':'Normal','value':0,"comment":u'普通'},
            {'name':'Boss','value':1,'comment':u'Boss'},
            {'name':'Protege','value':2,'comment':u'被保护者'},
        ]
    },
    {
        'name' : "SpawnRule",
        'cases' : [
            {'name':'FailedIfHold','value':0,'comment':u'被占了则创建失败'},
            {'name':'Replace','value':1,"comment":u'替换之前创建的'},
            {'name':'Overlay','value':2,'comment':u'继续创建在改位置重叠'},
        ]
    },
    {
        'name' : "SummonType",
        'cases' : [
            {'name':'SummonPos','value':1,'comment':u'召唤位召唤'},
            {'name':'RolePos','value':2,"comment":u'角色位召唤'},
            {'name':'AnyPos','value':3,'comment':u'任意指定位置随时可以使用召唤'},
        ]
    },
    {
        'name' : "SkillLogicType",
        'cases' : [
            {'name':'Normal','value':1,'comment':u'普通技能'},
            {'name':'Summon','value':2,"comment":u'召唤技能'},
            {'name':'Relive','value':3,"comment":u'复活技能'},
        ]
    },
    {
        'name' : "BubbleType",
        'cases' : [
            {'name':'BattleStart','value':1,'comment':u'战斗开始时'},
            {'name':'UseSkill','value':2,"comment":u'使用技能时'},
            {'name':'Die','value':3,"comment":u'死亡时'},
            {'name':'Win','value':4,"comment":u'胜利时'},
            {'name':'Lose','value':5,"comment":u'失败时'},
            {'name':'Talk','value':6,"comment":u'情缘喊话'},
            {'name':'Chat','value':7,"comment":u'搭讪'},
            {'name':'Response','value':8,"comment":u'回话'},
        ]
    }
]
