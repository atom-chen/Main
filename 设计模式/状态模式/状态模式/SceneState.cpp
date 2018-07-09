#include "SceneState.h"



// 开始
void StartState::StateBegin()
{
	if (IsInit)
	{
		return;
	}
	IsInit = false;

	/*初始化*/
	printf("%s", "StartState Loading...");

	IsInit = true;
}

// 结束
void StartState::StateEnd()
{
	printf("%s", "StartState Destroy...");
	IsInit = false;
}

// 更新
void StartState::StateUpdate()
{
	if (IsInit)
	{
		//Update逻辑
	}
}