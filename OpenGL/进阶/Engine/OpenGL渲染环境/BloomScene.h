#pragma  once
#include "Scene.h"
#include "DirectionLight.h"
#include "GameObject.h"
#include "ParticleSystem.h"
#include "Texture.h"
#include "Ground.h"
#include "SkyBox.h"
#include "SkyBoxC.h"
#include "FullScreenQuad.h"


class BloomScene :public Scene
{
public:
	virtual bool Awake();
	virtual void Start();

	virtual void Update();
	virtual void OnDrawBegin();
	virtual void Draw3D();
	virtual void Draw2D();
	virtual void OnDesrory();

	virtual void OnKeyDown(char KeyCode);//按下键盘时调用
	virtual void OnKeyUp(char KeyCode);//松开键盘时被调用
	virtual void OnMouseMove(float deltaX, float deltaY);//鼠标移动导致旋转时被调用
	virtual void OnMouseWheel(int direction);
protected:
private:
	SkyBox m_Skybox;
	GameObject m_Light;
	FrameBuffer m_Fbo;
	FullScreenQuad m_FSQ;

	FrameBuffer blurFBOH, blurFBOV;
	FullScreenQuad blurFSQH, blurFSQV;

	FullScreenQuad combineFSQ;
};