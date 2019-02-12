
Òh
BattleMessage.protoProtobufPacket"å
BattleEvent
type (1

skillEvent (2.ProtobufPacket.UseSkillEvent*
hitEvent (2.ProtobufPacket.HitEvent2
roundAPEvent (2.ProtobufPacket.RoundAPEvent>
impactsChangeEvent (2".ProtobufPacket.ImpactsChangeEvent4
killRoleEvent (2.ProtobufPacket.KillRoleEvent<
changeSpiritEvent (2!.ProtobufPacket.ChangeSpiritEvent6
changeEnvEvent (2.ProtobufPacket.ChangeEnvEvent8
deleteRoleEvent	 (2.ProtobufPacket.DeleteRoleEvent2
addRoleEvent
 (2.ProtobufPacket.AddRoleEvent2
	syncEvent (2.ProtobufPacket.SyncBattleState8
roundBeginEvent (2.ProtobufPacket.RoundBeginEvent.

roundEvent (2.ProtobufPacket.RoundEvent6
waveStartEvent (2.ProtobufPacket.WaveStartEvent2
waveEndEvent (2.ProtobufPacket.WaveEndEvent2
prepareEvent (2.ProtobufPacket.PrepareEvent4
dropItemEvent (2.ProtobufPacket.DropItemEvent<
playCutsceneEvent (2!.ProtobufPacket.PlayCutsceneEvent,
	idleEvent (2.ProtobufPacket.IdleEvent4
scenarioEvent (2.ProtobufPacket.ScenarioEvent8
roleActionEvent (2.ProtobufPacket.RoleActionEventD
playStoryContentEvent (2%.ProtobufPacket.PlayStoryContentEventD
skillCooldownEvent (2(.ProtobufPacket.SkillCooldownChangeEvent0
noticeEvent (2.ProtobufPacket.NoticeEvent<
changeSkillsEvent (2!.ProtobufPacket.ChangeSkillsEvent4
parallelEvent (2.ProtobufPacket.ParallelEvent0
bubbleEvent (2.ProtobufPacket.BubbleEvent:
battleStartEvent (2 .ProtobufPacket.BattleStartEvent5
setAITargetEvent (2.ProtobufPacket.SetAITarget4
roundEndEvent  (2.ProtobufPacket.RoundEndEvent"J
	HitResult
targetID (+
events (2.ProtobufPacket.BattleEvent"<
ParallelEvent+
events (2.ProtobufPacket.BattleEvent"G
Hit-

hitResults (2.ProtobufPacket.HitResult
	isAnimHit ("k
UseSkillEvent
casterID (
targetID (
usedSkillID (!
hits (2.ProtobufPacket.Hit"l
HitEvent
senderID (
targetID (
hitType (
val (
param0 (
param1 ("T
APChange
roleID (
newAP (
newActIndex (
isWaitingAct ("J
RoundAPEvent
count (+
	apChanges (2.ProtobufPacket.APChange"ƒ

ImpactInfo
impactID (

roundCount (

layerCount (
senderID (

id (
duration (
tag ("c
ImpactsChangeEvent
ownerID (
seq (/
infos (2 .ProtobufPacket.ImpactUpdateInfo"\
ImpactUpdateInfo

updateType (

id ((
info (2.ProtobufPacket.ImpactInfo"1
KillRoleEvent
killerID (
roleID ("7
DeleteRoleEvent
roleID (
isForReplace ("_
AddRoleEvent
roleID (
isPlaySpawnAnim (&
sync (2.ProtobufPacket.RoleSync"-
ChangeSpiritEvent
cur (
max ("
ChangeEnvEvent
envID ("Œ
PrepareEvent'
blues (2.ProtobufPacket.RoleSync&
reds (2.ProtobufPacket.RoleSync+
events (2.ProtobufPacket.BattleEvent"X
BattleStartEvent
envID (
hasDefaultEnv (
count0 (
count1 ("U
DropItemEvent4
dropList (2".ProtobufPacket.BattleDropItemList
roleId ("
PlayCutsceneEvent

id ("
	IdleEvent
time ("<
ScenarioEvent+
events (2.ProtobufPacket.BattleEvent"g
RoleActionEvent
roleId (
animId (
effectId (
paopao (

paopaoTime ("#
PlayStoryContentEvent

id ("q
BubbleAction
roleId (
type (0

nextBubble (2.ProtobufPacket.BubbleAction
	userObjId ("<
BubbleEvent-
bubbles (2.ProtobufPacket.BubbleAction"=
SkillCooldownInfo

cooldownId (
cooldownLeft ("i
SkillCooldownChangeEvent
roleId (=
skillCooldownInfos (2!.ProtobufPacket.SkillCooldownInfo"
NoticeEvent
notice (	"5
ChangeSkillsEvent
roleId (
skillIds ("#
Attr
type (
value (",
Attrs#
attrs (2.ProtobufPacket.Attr"+
	SkillInfo
skillID (
level ("ÿ
RoleVisualInfo
modelId (:-1
talismanVisualId (
soulWareModelId (

dyeColorId (:-1
ornamentEffect (:-16
cardGuid (2$.ProtobufPacket.RoleVisualInfo._Guid
hangPieceList ("
_Guid
low (
high ("ú
RoleInfo

roleBaseID (-

skillInfos (2.ProtobufPacket.SkillInfo$
attrs (2.ProtobufPacket.Attrs

isHeroCard (
cardId (:-1
	monsterId (:-12

visualInfo (2.ProtobufPacket.RoleVisualInfo
heroId	 (:-1"å
RoleInitData*
roleInfo (2.ProtobufPacket.RoleInfo
side (
	battlePos (
battlePosArea (*
status (2.ProtobufPacket.RoleStatus
	spawnRule (
	userObjId (:-1

ai (
queryId	 ("Œ

RoleStatus

hp (

ap (9
skillCooldowns (2!.ProtobufPacket.SkillCooldownInfo+
impacts (2.ProtobufPacket.ImpactInfo"e

BattleSync
envID (
spirit (
	spiritMax (
	waveIndex (

roundCount ("Ì
RoleSync

roleBaseID (
roleID (

hp (
maxHP (

ap (
actIndex (
side (
	battlePos (
battlePosArea	 (
skills
 (9
skillCooldowns (2!.ProtobufPacket.SkillCooldownInfo+
impacts (2.ProtobufPacket.ImpactInfo

isHeroCard (
cardId (:-1
	monsterId (:-1(
	fullAttrs (2.ProtobufPacket.Attrs
	userObjId (:-1

sp (
isWaitingAct (2

visualInfo (2.ProtobufPacket.RoleVisualInfo
summonOnwerId (
heroId (:-1
maxHpReduce (
shiled (
speed ("X
AITargetSync
	userObjId (
aiOurPriorityTarget (
aiPriorityTarget ("¢
SyncBattleState.

battleSync (2.ProtobufPacket.BattleSync+
	roleSyncs (2.ProtobufPacket.RoleSync2
aiTargetSync (2.ProtobufPacket.AITargetSync"#
WaveStartEvent
	waveIndex ("l
RoundBeginEvent
roundRoleID (
isNeedPlayerInput (

roundCount (
	userObjId (:-1"N

RoundEvent
roundRoleID (

roundCount (
totalRoundCount ("!
WaveEndEvent
	waveIndex ("X
BattleStatus
isWaveClear (
winSide (
	waveIndex (
waveMax ("J
	BattleMsg+
events (2.ProtobufPacket.BattleEvent
hasPause ("(

BattleData
key (
value ("Ä
BattleResultQueryRoleOption
skillDamage (:false
impactDamge (:false
hitTypeDamage (:false
	skillHeal (:false

impactHeal (:false
hitTypeHeal (:false"…
BattleResultQueryOption

redStatist (:false

roleBasics (:falseA
blueRoleOpts (2+.ProtobufPacket.BattleResultQueryRoleOption@
redRoleOpts (2+.ProtobufPacket.BattleResultQueryRoleOption
blueKill (:false
redKill (:false"Š
BattleStatist_RoleBasic
queryId (

roleBaseId (
cardId (
	monsterId (
side (

hp (
maxHp ("-
BattleStatist_KV

id (
value ("ò
BattleStatist_Role
queryId (
actCount (
totalDamage (
	totalHeal (
	maxDamage (6
skillDamages (2 .ProtobufPacket.BattleStatist_KV4

skillHeals (2 .ProtobufPacket.BattleStatist_KV7
impactDamages (2 .ProtobufPacket.BattleStatist_KV5
impactHeals	 (2 .ProtobufPacket.BattleStatist_KV8
hitTypeDamages
 (2 .ProtobufPacket.BattleStatist_KV6
hitTypeHeals (2 .ProtobufPacket.BattleStatist_KV
	deadRound (
totalDamageLastWave ("š
BattleStatist_Total
totalDamage (
	totalHeal (
	maxDamage (
maxDamageUnclamp (
	deadCount (
totalDamageLastWave ("·
BattleResult
winSide (
collectedPreviewDropIds (
seed (
retHash ()
datas (2.ProtobufPacket.BattleData

roundCount (8
blueStatist (2#.ProtobufPacket.BattleStatist_Total7

redStatist (2#.ProtobufPacket.BattleStatist_Total6
roles	 (2'.ProtobufPacket.BattleStatist_RoleBasic;
blueRoleStatics (2".ProtobufPacket.BattleStatist_Role:
redRoleStatics (2".ProtobufPacket.BattleStatist_Role9
blueKillStatics (2 .ProtobufPacket.BattleStatist_KV8
redKillStatics (2 .ProtobufPacket.BattleStatist_KV"
	BattleCmd
type (
	userObjId (6
useSkillCmd (2!.ProtobufPacket.PlayerUseSkillCmd(
skipCmd (2.ProtobufPacket.SkipCmd.

heroActCmd (2.ProtobufPacket.HeroActCmd0
setAITarget (2.ProtobufPacket.SetAITarget+
setAuto (2.ProtobufPacket.SetAutoCmd"7
BattleCmdMsg'
cmds (2.ProtobufPacket.BattleCmd"R
PlayerUseSkillCmd
roleID (
skillSelected (
targetSelected ("

HeroActCmd
side ("/
SkipCmd
roleID (
autoUseSkill ("
SetAITarget
targetId ("#
RoundEndEvent

roundCount ("

SetAutoCmd
auto ("¡
VersionInfo
gameVersion (
programVersion (
clientUpdateVersion (
privateResourceVersion (
tableVersion (

luaVersion ("N
BattleMsgWithTimestamp&
msg (2.ProtobufPacket.BattleMsg
time ("»
BattleRecord0
versionInfo (2.ProtobufPacket.VersionInfo,
result (2.ProtobufPacket.BattleResult-

cmdRecords (2.ProtobufPacket.CmdRecord0
initData (2.ProtobufPacket.BattleInitData
seed (
battleId (6
events (2&.ProtobufPacket.BattleMsgWithTimestamp

battleType ("P
	CmdRecord
tick (
state (&
cmd (2.ProtobufPacket.BattleCmd";
CmdRecordList*
records (2.ProtobufPacket.CmdRecord"•
BattleSimulateData0
initData (2.ProtobufPacket.BattleInitData
battleId (

battleSeed ('
cmds (2.ProtobufPacket.CmdRecord"v
BattleDebug
nospiritChange (
noskillLimit (
startEnv (
startSpirit (

nohpChange ("N
WaveRoleData
	waveIndex (+
roles (2.ProtobufPacket.RoleInitData"T
WaveDropData
	waveIndex (1
drops (2".ProtobufPacket.BattleDropItemList"1

BattleCost
costType (
	costValue (">
BattleCosts/
battleCosts (2.ProtobufPacket.BattleCost"¶
BattleInitData
waveMax (+
blues (2.ProtobufPacket.WaveRoleData*
reds (2.ProtobufPacket.WaveRoleData+
drops (2.ProtobufPacket.WaveDropData
blueImpacts (

redImpacts (:
	queryOpts (2'.ProtobufPacket.BattleResultQueryOption
enableBubble (

arenaLevel	 ("Q
CG_TEST_ASK_ENTER_BATTLE
battleId (

isAtServer (
isStory ("(
GC_TEST_BATTLE_FINISH
winSide ("Ñ
GC_ENTER_BATTLE
battleId (

battleSeed (

id (>

battleType (2*.ProtobufPacket.GC_ENTER_BATTLE.BattleType0
initData (2.ProtobufPacket.BattleInitData
	cardGuids (
heroId (
side (
	userObjId	 (
validPosSet
 (
battleRetType (
waveMax (
	isReEnter (
isHeroValid (
	quitDelay (
	forceHero (:-1"G

BattleType

Client
ServerSingle
MultiPlayer

Replay"P
CG_CLT_BATTLE_FINISH

id (,
record (2.ProtobufPacket.BattleRecord" 
CG_ASK_QUIT_BATTLE

id ("Q
CG_CHANGE_CARD

id (
	battlePos (
cardGuid (
heroId ("8
CG_BATTLE_READY

id (
autoAITargetParts ("J
CG_SEND_BATTLE_INPUT

id (&
cmd (2.ProtobufPacket.BattleCmd"
CG_REQ_HERO_ACT

id ("0
GC_HERO_ACT_EVENT
side (
index (":
GC_BATTLE_EVENTS&
msg (2.ProtobufPacket.BattleMsg"w
ChangeCardInfo.
initData (2.ProtobufPacket.RoleInitData
cardGuid (
oldCardGuid (
heroId ("C
GC_CHANGE_CARD_RET-
infos (2.ProtobufPacket.ChangeCardInfo"1
BattleDropItem

dropItemId (
num ("„
BattleDropItemList
previewDropId (
	battlePos (
battlePosArea (-
drops (2.ProtobufPacket.BattleDropItem"6
GC_COMMON_BATTLE_FINISH
winSide (

id ("
CG_BATTLE_RESUME

id ("A
GC_BATTLE_RESYNC-
sync (2.ProtobufPacket.SyncBattleState"
GC_BATTLE_GM
gmstr (	"J
GC_BATTLE_VERIFY_RET2
serverRecord (2.ProtobufPacket.BattleRecord"í
_ASYNC_PVP_PLAYER_FOR_CLIENT

lv (
modelId (
grade (
name (	
isWin (:false
guid (
skinId (:-1

deyColorId (:-1
ornamentEffectId	 (:-1
hangPieceList
 (
heroid ("ž
_DB_ASYNC_PVP_PLAYER_FOR_SERVER&
role (2.ProtobufPacket.RoleInfo
	battlePos (@

clientData (2,.ProtobufPacket._ASYNC_PVP_PLAYER_FOR_CLIENT"u
_DB_ASYNC_PVP_OTHER_INFO
awardedAward (
buffs (
tokens (
grade (:0

weekTokens ("›
_ASYNC_PVP_DATA_FOR_SERVER@
players (2/.ProtobufPacket._DB_ASYNC_PVP_PLAYER_FOR_SERVER;
	otherInfo (2(.ProtobufPacket._DB_ASYNC_PVP_OTHER_INFO"
_DBASYNC_PVP_ROUNTINE_DATA@
players (2/.ProtobufPacket._DB_ASYNC_PVP_PLAYER_FOR_SERVER
curIndex (
totle ("˜
_ASYNC_PVP_DATA_FOR_CLIENT=
players (2,.ProtobufPacket._ASYNC_PVP_PLAYER_FOR_CLIENT;
	otherInfo (2(.ProtobufPacket._DB_ASYNC_PVP_OTHER_INFO"h
GC_ASYNC_PVP_SYNC8
data (2*.ProtobufPacket._ASYNC_PVP_DATA_FOR_CLIENT

showEffect (:false"
CG_REQ_SURRENDER"
CG_ENTER_BATTLE_SCENE"_
GC_BATTLE_ROON_BATTLE_TIMES
battleTimes (
curBattleTimes (
need2Punish ("D
GC_BATTLE_PULL_SYNC-
sync (2.ProtobufPacket.SyncBattleState"
CG_REQ_BATTLE_SYNC