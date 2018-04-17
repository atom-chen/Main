#include "GamaMain.h"
#include "SceneManager.h"
#include "Zhucheng.h"
#include "Yewai.h"
#include "Fuben.h"



void GameLogic::Start()
{
	Scene *zhucheng = new Zhucheng;
	Scene *yewai = new Yewai;
	Scene *fuben = new Fuben;
	SceneManager::AddScene("main", zhucheng);
	SceneManager::AddScene("yewai", yewai);
	SceneManager::AddScene("fuben", fuben);
	SceneManager::LoadScene("yewai");
	EngineBehavior ::Start();
}
void GameLogic::Update()
{

}
void GameLogic::Event(UINT message, WPARAM wParam, LPARAM lParam)
{
	switch (message)
	{
	case WM_KEYDOWN:
		switch (wParam)
		{
		case '1':
			SceneManager::LoadScene("main");
			break;
		case '2':
			SceneManager::LoadScene("yewai");
			break;
		case '3':
			SceneManager::LoadScene("fuben");
			
		default:
			break;
		}
	default:
		break;
	}
}
void GameLogic::OnDestory()
{

}