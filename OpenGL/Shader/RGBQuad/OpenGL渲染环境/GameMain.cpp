#include "GamaMain.h"
#include "SceneManager.h"
#include "Zhucheng.h"
#include "Yewai.h"



void GameLogic::Start()
{
	Scene *zhucheng = new Zhucheng;
	Scene *yewai = new Yewai;
	SceneManager::AddScene("main", zhucheng);
	SceneManager::AddScene("yewai", yewai);
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