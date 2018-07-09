#include "GameState.h"

GameStateBase::GameStateBase(Content *content)
{
	this->content = content;
}

void GameState1::Handler(int value)
{
	if (value <= 10 && content!=nullptr)
	{
		printf("%u <=10,change state to state2\n", value);
		content->SetState(new GameState2(content));
	}
}

void GameState2::Handler(int value)
{
	if (value > 10 && value <= 20 && content != nullptr)
	{
		printf("%u >10 <=20 ,change state to state3\n", value);
		content->SetState(new GameState3(content));
	}
}

void GameState3::Handler(int value)
{
	if (value > 20 && content != nullptr)
	{
		printf("%u >20,change state to state1\n", value);
		content->SetState(new GameState1(content));
	}
}