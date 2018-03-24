#include "Vector3.h"

class Camera
{
public:
	Camera();
public:
	void Update(float frameTime);
	void MoveToLeft(bool isMove);
	void MoveToRight(bool isMove);
	void MoveToTop(bool isMove);
	void MoveToBottom(bool isMove);
	void SetMoveSpeed(float speed);
private:
	Vector3 m_Position;//摄像机位置
	Vector3 m_ViewCenter;//目标视点
	Vector3 m_Direction;//发射出去的方向向量
	bool m_IsMoveToLeft, m_IsMoveToRight,m_IsMoveToTop, m_IsMoveToBottom;
	float m_MoveSpeed = 10.0f;
};