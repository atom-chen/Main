#pragma once
#include <stdio.h>
#include <stdlib.h>
class ISceneState;

class StateController
{
public:
	void StateUpdate();
	void SetState(ISceneState* state);
private:
	ISceneState *m_CurState;
};


class ISceneState
{
public:
	// 建造者
	ISceneState(StateController *Controller) 
	{
		m_Controller = Controller;
	}

	// 开始
	virtual void StateBegin() = 0;

	// 结束
	virtual void StateEnd() = 0;

	// 更新
	virtual void StateUpdate() = 0;

	bool IsLoadOK(){ return IsInit; }

protected:
	StateController *m_Controller;
	bool IsInit = false;
};