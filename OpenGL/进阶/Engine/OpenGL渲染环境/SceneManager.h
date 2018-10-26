#pragma once
#include "ggl.h"
#include "Scene.h"
#include "RenderAble.h"
#include "GlobalState.h"

class SceneManager
{
private:
	SceneManager();
public:
	static void AddScene(string name,Scene* scene);
	static void LoadScene(string name);
	static void LoadScene(int index);
	static void LoadScene(Scene* scene);
	static void ReSetScene();
private:

public:
	static void Update();
	static int Event(UINT message, WPARAM wParam, LPARAM lParam, HWND hwnd);
public:
	static void SceneManager::DrawGameObject(RenderAble* render);
	static void SceneManager::DrawGameObject(const RenderDomain& render);

public:
	static void InitGlew();

	static float GetViewWidth();
	static float GetViewHidth();
	static void SetViewPort(float width,float height);

	static void DrawCommit();
public:
	static void SetBlendState(AlphaBlendInfo info);
	inline static AlphaBlendInfo GetBlendState()
	{
		return m_Blend;
	}

	static void SetDepthTestState(bool isEnable);
	inline static bool GetDepthTestState()
	{
		return m_IsDepthTest;
	}

	static void SetScissorState(const ScissorState& scissor);
	inline static ScissorState GetScissorState()
	{
		return m_ScissorState;
	}

	static void SetProgramPointSizeState(bool isEnable);
	inline static bool GetProgramPointSizeState()
	{
		return m_IsProgramPointSize.isProgramPointSize;
	}

	static void SetClearColor(const vec4& color);
private:
	static std::map<string, Scene*> m_mScene;
	static Scene* m_CurScene;

	static POINT m_OriginalPos;//记录按下鼠标时的位置
	static bool m_IsRotate;//是否正在旋转
	static bool m_IsGlewInit;

	static AlphaBlendInfo m_Blend;//alpha混合状态
	static bool m_IsDepthTest;//是否开启深度测试
	static ProgramPointSize m_IsProgramPointSize;//是否由程序控制点的大小
	static ScissorState m_ScissorState;//裁剪开启状态

	static float m_ViewWidth;
	static float m_ViewHeight;
};