#include "GameState.h"



void Content::Requeat(int value)
{
	if (m_CurState != nullptr)
	{
		m_CurState->Handler(value);
	}
}

void Content::SetState(GameStateBase* state)
{
	if (m_CurState != nullptr)
	{
		delete m_CurState;
		m_CurState = nullptr;
	}
	m_CurState = state;
}