#pragma once
#include "ggl.h"
#include "Scene.h"

class SceneManager
{
private:
	SceneManager();
public:
	static void AddScene(string name,Scene* scene);
	static void LoadScene(string name);
public:
	static void Start();
	static void Update();
	static int Event(UINT message, WPARAM wParam, LPARAM lParam, HWND hwnd);
private:
	static std::map<string, Scene*> m_mScene;
	static Scene* m_CurScene;

	static POINT m_OriginalPos;//记录按下鼠标时的位置
	static bool m_IsRotate;//是否正在旋转
};