#pragma once
#include "ggl.h"
#include "Ground.h"
#include "SkyBox.h"
#include "Camera.h"
#include "Camera_2D.h"
#include "Vertex.h"


class Scene
{
public:
	virtual bool Awake();
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


protected:
	Ground m_Ground;
	SkyBox m_Skybox;
	Camera_1st m_MainCamera;
	Camera_2D m_2DCamera;
	FrameBuffer* m_FrameBuf;
};