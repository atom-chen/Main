#include "StateController.h"

void StateController::StateUpdate()
{
	if (m_CurState != nullptr)
	{
		m_CurState->StateUpdate();
	}
}
void StateController::SetState(ISceneState* state)
{
	if (m_CurState != state && m_CurState != nullptr)
	{
		m_CurState->StateEnd();
		m_CurState = state;
		state->StateBegin();
	}
}