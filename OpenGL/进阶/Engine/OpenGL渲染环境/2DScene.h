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


class Scene2D :public Scene
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
	FullScreenQuad m_AlphaBlendFSQ,m_LighterFSQ,m_DarkerFSQ;
	FullScreenQuad m_zpddFSQ, m_moteDarkerFSQ,m_moteLighterFSQ,m_rgFSQ;
	FullScreenQuad m_AddFSQ, m_DelFSQ;
	int state = 0;

};