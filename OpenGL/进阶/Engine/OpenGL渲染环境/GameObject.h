#pragma  once
#include "RenderAble_Light.h"


class GameObject :public RenderAble_Light
{
public:
	GameObject();
	virtual bool Init(const char* path, const char* vertexShader = SHADER_ROOT"FragObj.vert", const char* fragmentShader = SHADER_ROOT"FragObj.frag");
	virtual void Update(const vec3& cameraPos);

	void Destroy();
public:
	void MoveToLeft(bool isMove);
	void MoveToRight(bool isMove);
	void MoveToTop(bool isMove);
	void MoveToBottom(bool isMove);
	void SetMoveSpeed(float speed);
protected:
	bool m_IsMoveToLeft, m_IsMoveToRight, m_IsMoveToTop, m_IsMoveToBottom;
	float m_MoveSpeed = 5;
	string m_ModelName;
};