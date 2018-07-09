#pragma once
#include "StateController.h"

//��ʼ״̬
class StartState:public ISceneState
{
public:
	StartState(StateController *controller) :ISceneState(controller){};

	// ��ʼ
	virtual void StateBegin();

	// ����
	virtual void StateEnd();

	// ����
	virtual void StateUpdate();
};

//��ͨ״̬
class MainState :public ISceneState
{
public:
	MainState(StateController *controller) :ISceneState(controller){};

	// ��ʼ
	virtual void StateBegin();

	// ����
	virtual void StateEnd();

	// ����
	virtual void StateUpdate();
};

//ս��״̬
class BattleState :public ISceneState
{
public:
	BattleState(StateController *controller) :BattleState(controller){};

	// ��ʼ
	virtual void StateBegin();

	// ����
	virtual void StateEnd();

	// ����
	virtual void StateUpdate();
};