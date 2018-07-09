#pragma once
#include <stdio.h>

class GameStateBase;

class Content
{
public:
	void Requeat(int value);
	void SetState(GameStateBase* state);
protected:
private:
	GameStateBase* m_CurState = nullptr;
};

class GameStateBase
{
public:
	GameStateBase(Content *content);
	virtual void Handler(int value) = 0;
protected:
	Content* content;
};


class GameState1:public GameStateBase
{
public:
	GameState1(Content *content) :GameStateBase(content){}
	virtual void Handler(int value);
protected:
private:
};

class GameState2 :public GameStateBase
{
public:
	GameState2(Content *content) :GameStateBase(content){}
	virtual void Handler(int value);
protected:
private:
};

class GameState3 :public GameStateBase
{
public:
	GameState3(Content *content) :GameStateBase(content){}
	virtual void Handler(int value);
protected:
private:
};