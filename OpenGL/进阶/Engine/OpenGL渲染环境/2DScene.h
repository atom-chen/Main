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
	DJ = 9,           //叠加
	QG = 10,          //强光
	CZ = 11,         //插值
	FCZ = 12,       //反插值
	PC = 13,        //排除
	SMOOTH = 14,    //平滑
	SHARPEN =15,   //锐化
	EDGE = 16,     //边缘检测
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

	virtual void OnKeyDown(char KeyCode);//按下键盘时调用
	virtual void OnKeyUp(char KeyCode);//松开键盘时被调用
	virtual void OnMouseMove(float deltaX, float deltaY);//鼠标移动导致旋转时被调用
	virtual void OnMouseWheel(int direction);
protected:
private:
	FullScreenQuad m_FSQS[Scene2DType::MAX];
	int state = 0;

};