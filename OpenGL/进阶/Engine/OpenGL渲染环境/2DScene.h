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

enum Scene2DType
{
	TINVALID = -1,
	BLEND = 0,
	LIGHTER = 1,
	DARKER = 2,
	ZPDD = 3,
	MOTE_DARKER = 4,
	MOTE_LIGHTER = 5,
	RG = 6,
	ADD = 7,
	DEL = 8,
	DJ = 9,           //����
	QG = 10,          //ǿ��
	CZ = 11,         //��ֵ
	FCZ = 12,       //����ֵ
	PC = 13,        //�ų�
	MAX
};
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
	FullScreenQuad m_FSQS[Scene2DType::MAX];
	int state = 0;

};