#pragma once
#include "ggl.h"
#include "Scene.h"
#include "RenderAble.h"
#include "RenderOptions.h"

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
public:
	static void DrawGameObject(RenderAble* render);
public:
	static void InitGlew();

	static void SetBlendState(AlphaBlendInfo info);
	inline static AlphaBlendInfo GetBlendState();

	static void SetDepthTestState(bool isEnable);
	inline static bool GetDepthTestState();

	static void SetProgramPointSizeState(bool isEnable);
	inline static bool GetProgramPointSizeState();

private:
	static std::map<string, Scene*> m_mScene;
	static Scene* m_CurScene;

	static POINT m_OriginalPos;//记录按下鼠标时的位置
	static bool m_IsRotate;//是否正在旋转
	static bool m_IsGlewInit;

	static AlphaBlendInfo m_Blend;//alpha混合状态
	static bool m_IsDepthTest;//是否开启深度测试
	static bool m_IsProgramPointSize;//是否由程序控制点的大小
	
};