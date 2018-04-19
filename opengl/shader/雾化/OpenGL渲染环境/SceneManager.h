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
	static void ReSetScene();
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

	static void SetClearColor(const vec4& color);
private:
	static std::map<string, Scene*> m_mScene;
	static Scene* m_CurScene;

	static POINT m_OriginalPos;//��¼�������ʱ��λ��
	static bool m_IsRotate;//�Ƿ�������ת
	static bool m_IsGlewInit;

	static AlphaBlendInfo m_Blend;//alpha���״̬
	static bool m_IsDepthTest;//�Ƿ�����Ȳ���
	static bool m_IsProgramPointSize;//�Ƿ��ɳ�����Ƶ�Ĵ�С
	
};