#pragma  once
#include "RenderAble.h"


class GameObject:public RenderAble
{
public:
	GameObject();
	bool Init(const char* path, const char* vertexShader ="res/obj.vert" , const char* fragmentShader = "res/obj.frag");
	void Update(const vec3& cameraPos);
	void Draw();
	void Destory();
public:
	void MoveToLeft(bool isMove);
	void MoveToRight(bool isMove);
	void MoveToTop(bool isMove);
	void MoveToBottom(bool isMove);
	void SetMoveSpeed(float speed);
protected:
private:
	//ģ�͵Ĳ���
	bool m_IsMoveToLeft, m_IsMoveToRight, m_IsMoveToTop, m_IsMoveToBottom;
	float m_MoveSpeed = 5;
	string m_ModelName;
};