#pragma once
#include "WinApp.h"

class GameLogic:public EngineBehavior 
{
public:
	virtual void Start();
	virtual void Update();
	virtual void OnDestory();
protected:
private:
};

