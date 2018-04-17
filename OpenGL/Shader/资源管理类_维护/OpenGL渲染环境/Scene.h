#pragma once
#include "ggl.h"
#include "Ground.h"
#include "SkyBox.h"
#include "Camera.h"
#include "Camera_2D.h"
#include "RenderList.h"

class Camera
{
public:
	Camera(Camera_1st *camera)
	{
		m_3DRendList.SetCamera(camera);
	}
protected:
private:
	Camera_1st *m_MainCamera;
	RenderList m_3DRendList;
};

class Scene
{
friend class SceneManager;
public:
	virtual bool Awake();
protected:

	virtual void Start();


	virtual void Update();
	virtual void OnDrawBegin();

	virtual void Draw3D();
	virtual void Draw2D();

	virtual void OnDrawOver();
	virtual void OnDesrory();

	virtual void SetViewPortSize(float width, float height);

	virtual void OnKeyDown(char KeyCode);//���¼���ʱ����
	virtual void OnKeyUp(char KeyCode);//�ɿ�����ʱ������
	virtual void OnMouseMove(float deltaX, float deltaY);//����ƶ�������תʱ������
	virtual void OnMouseWheel(int32_t direction);
private:
	void SystemAwake();
	void SystemStart();
	void SystemUpdate();
	void SystemOnDrawBegin();
	void SystemDraw3D();
	void SystemDraw2D();
	void SystemOnDraw2DOver();
	void SystemOnDrawOver();
	void SystemDesrory();


protected:
	SkyBox m_Skybox;
	Camera_2D m_2DCamera;
	Camera_1st *m_MainCamera;
private:
	FrameBuffer* m_FrameBuf;
	RenderList m_3DRendList;
};