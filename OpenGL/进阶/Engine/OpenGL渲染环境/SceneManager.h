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

	static POINT m_OriginalPos;//��¼�������ʱ��λ��
	static bool m_IsRotate;//�Ƿ�������ת
	static bool m_IsGlewInit;

	static AlphaBlendInfo m_Blend;//alpha���״̬
	static bool m_IsDepthTest;//�Ƿ�����Ȳ���
	static ProgramPointSize m_IsProgramPointSize;//�Ƿ��ɳ�����Ƶ�Ĵ�С
	static ScissorState m_ScissorState;//�ü�����״̬

	static float m_ViewWidth;
	static float m_ViewHeight;
};