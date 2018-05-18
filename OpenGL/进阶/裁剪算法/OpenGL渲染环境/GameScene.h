#pragma  once
#include "Scene.h"

class Zhucheng:public Scene
{
public:
	virtual bool Awake();
	virtual void Start();

	virtual void Update();
	virtual void Draw3D();
	virtual void Draw2D();

	virtual void OnKeyDown(char KeyCode);//���¼���ʱ����
	virtual void OnKeyUp(char KeyCode);//�ɿ�����ʱ������
	virtual void OnMouseMove(float deltaX, float deltaY);//����ƶ�������תʱ������
	virtual void OnMouseWheel(int32_t direction);
protected:
private:
	GameObject m_box;
	GameObject m_Niu;
	UITexture m_Texture;
	ParticleSystem m_ParticleSystem;
};
