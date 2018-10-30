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


class Fuben :public Scene
{
public:
	virtual bool Awake();
	virtual void Start();

	virtual void Update();
	virtual void OnDrawBegin();
	virtual void Draw3D();
	virtual void Draw2D();
	virtual void OnDesrory();

	virtual void OnKeyDown(char KeyCode);//���¼���ʱ����
	virtual void OnKeyUp(char KeyCode);//�ɿ�����ʱ������
	virtual void OnMouseMove(float deltaX, float deltaY);//����ƶ�������תʱ������
	virtual void OnMouseWheel(int direction);
protected:
private:
	SkyBox m_Skybox;
	GameObject m_Sphere;
	DirectionLight m_DR;
	FrameBuffer m_Fbo;
	FullScreenQuad m_FSQ;

	//ͨ��
	GameObject m_Sphere2;
	FrameBuffer hdrFbo,ldrFbo;
	Shader ldrShader;
};