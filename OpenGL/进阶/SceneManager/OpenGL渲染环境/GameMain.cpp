#include "GamaMain.h"
#include "SceneManager.h"
#include "GameScene.h"



void GameLogic::Start()
{
	Scene *scene = new Zhucheng;
	SceneManager::AddScene("main", scene);
	SceneManager::LoadScene("main");
	EngineBehavior ::Start();
}
void GameLogic::Update()
{
	EngineBehavior ::Update();
}
void GameLogic::OnDestory()
{
	EngineBehavior ::OnDestory();
}