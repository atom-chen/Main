#include "GamaMain.h"
#include "SceneManager.h"
#include "Zhucheng.h"
#include "Yewai.h"
#include "HDR.h"
#include "Yewai2.h"
#include "SceneSea.h"
#include "Fuben2.h"
#include "Gauss.h"
#include "BloomScene.h"
#include "2DScene.h"



void GameLogic::Start()
{
	Scene *zhucheng = new Zhucheng;
	Scene *yewai = new Yewai;
	Scene *yewai2 = new Yewai2;
	Scene* fuben2 = new Fuben2;
	Scene* gau = new GaussScene;
	Scene *hdr = new HDRScene;
	Scene* bloom = new BloomScene;
	Scene* scene2D = new Scene2D;
	SceneManager::AddScene("main", zhucheng);
	SceneManager::AddScene("yewai", yewai);
	SceneManager::AddScene("yewai2", yewai2);
	SceneManager::AddScene("fuben", fuben2);
	SceneManager::AddScene("hdr", hdr);
	SceneManager::AddScene("gauss", gau);
	SceneManager::AddScene("bloom", bloom);
	SceneManager::AddScene("2d", scene2D);
	SceneManager::LoadScene("2d");
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
		case '6':
			SceneManager::LoadScene("hdr");
			break;
		case '4':
			SceneManager::LoadScene("fuben");
			break;
		case '5':
			SceneManager::LoadScene("gauss");
			break;
		case '7':
			SceneManager::LoadScene("bloom");
			break;
		case '8':
			SceneManager::LoadScene("2d"); break;
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