#pragma  once
#include "Scene.h"
#include "Light.h"
#include "GameObject.h"
#include "ParticleSystem.h"
#include "Texture.h"
#include "Ground.h"


class Yewai:public Scene
{
public:
	virtual bool Awake();
	virtual void Start();

	virtual void Update();
	virtual void OnDrawBegin();
	virtual void Draw3D();
	virtual void Draw2D();

	virtual void OnKeyDown(char KeyCode);//���¼���ʱ����
	virtual void OnKeyUp(char KeyCode);//�ɿ�����ʱ������
	virtual void OnMouseMove(float deltaX, float deltaY);//����ƶ�������תʱ������
	virtual void OnMouseWheel(int32_t direction);
protected:
private:
	Ground m_Ground;
	GameObject m_box;
	GameObject m_Niu;
	ParticleSystem m_ParticleSystem;
	DirectionLight m_DirectionLight;

	int m_BoxRotateY = 0;
};
