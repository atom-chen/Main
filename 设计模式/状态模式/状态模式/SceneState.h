#pragma once
#include "StateController.h"

//开始状态
class StartState:public ISceneState
{
public:
	StartState(StateController *controller) :ISceneState(controller){};

	// 开始
	virtual void StateBegin();

	// 结束
	virtual void StateEnd();

	// 更新
	virtual void StateUpdate();
};

//普通状态
class MainState :public ISceneState
{
public:
	MainState(StateController *controller) :ISceneState(controller){};

	// 开始
	virtual void StateBegin();

	// 结束
	virtual void StateEnd();

	// 更新
	virtual void StateUpdate();
};

//战斗状态
class BattleState :public ISceneState
{
public:
	BattleState(StateController *controller) :BattleState(controller){};

	// 开始
	virtual void StateBegin();

	// 结束
	virtual void StateEnd();

	// 更新
	virtual void StateUpdate();
};