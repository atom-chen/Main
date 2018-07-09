#pragma once
#include "GameState.h"


class Content
{
public:
	void Requeat(int value);
	void SetState(GameStateBase* state);
protected:
private:
	GameStateBase* m_CurState = nullptr;
};
