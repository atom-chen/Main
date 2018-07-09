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
	// ������
	ISceneState(StateController *Controller) 
	{
		m_Controller = Controller;
	}

	// ��ʼ
	virtual void StateBegin() = 0;

	// ����
	virtual void StateEnd() = 0;

	// ����
	virtual void StateUpdate() = 0;

	bool IsLoadOK(){ return IsInit; }

protected:
	StateController *m_Controller;
	bool IsInit = false;
};