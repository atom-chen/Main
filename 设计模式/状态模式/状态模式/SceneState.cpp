#include "SceneState.h"



// ��ʼ
void StartState::StateBegin()
{
	if (IsInit)
	{
		return;
	}
	IsInit = false;

	/*��ʼ��*/
	printf("%s", "StartState Loading...");

	IsInit = true;
}

// ����
void StartState::StateEnd()
{
	printf("%s", "StartState Destroy...");
	IsInit = false;
}

// ����
void StartState::StateUpdate()
{
	if (IsInit)
	{
		//Update�߼�
	}
}