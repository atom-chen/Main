#include "GamaMain.h"
#include "SceneManager.h"
#include "Zhucheng.h"
#include "Yewai.h"
#include "Fuben.h"
#include "Yewai2.h"
#include "SceneSea.h"



void GameLogic::Start()
{
	Scene *zhucheng = new Zhucheng;
	Scene *yewai = new Yewai;
	Scene *yewai2 = new Yewai2;
	Scene *fuben = new Fuben;
	Scene* sea = new SceneSea;
	SceneManager::AddScene("main", zhucheng);
	SceneManager::AddScene("yewai", yewai);
	SceneManager::AddScene("yewai2", yewai2);
	SceneManager::AddScene("fuben", fuben);
	SceneManager::AddScene("sea", sea);
	SceneManager::LoadScene("main");
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
			SceneManager::LoadScene("yewai2");
			break;
		case '4':
			SceneManager::LoadScene("fuben");
			break;
		case '5':
			SceneManager::LoadScene("sea");
			break;
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