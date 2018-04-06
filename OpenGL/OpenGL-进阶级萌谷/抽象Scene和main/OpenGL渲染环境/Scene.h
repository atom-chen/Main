#pragma once
#include "ggl.h"
#include "Ground.h"
#include "GameObject.h"
#include "SkyBox.h"
#include "ParticleSystem.h"
#include "Camera.h"
#include "Camera_2D.h"
#include "Texture.h"
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

	virtual void SetViewPortSize(float width, float height);

	virtual void OnKeyDown(char KeyCode);//按下键盘时调用
	virtual void OnKeyUp(char KeyCode);//松开键盘时被调用
	virtual void OnMouseMove(float deltaX, float deltaY);//鼠标移动导致旋转时被调用
	virtual void OnMouseWheel(int32_t direction);
protected:
	Ground m_Ground;
	GameObject m_box;
	GameObject m_Niu;
	SkyBox m_Skybox;
	ParticleSystem m_ParticleSystem;
	Camera_1st m_MainCamera;
	Camera_2D m_2DCamera;
	FrameBuffer* m_FrameBuf;
	UITexture m_Texture;
};