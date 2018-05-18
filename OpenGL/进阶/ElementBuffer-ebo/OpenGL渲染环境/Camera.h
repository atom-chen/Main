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
	void MoveToFront(bool isMove);
	void MoveToBack(bool isMove);
	void SetMoveSpeed(float speed);

	void Pitch(float angle);//绕自己的X轴旋转
	void Yaw(float angle);//绕世界坐标系y旋转
	void RotateView(float angle, float x, float y, float z);//绕任意轴旋转（x,y,z为轴的方向向量）

	void SwitchTo2D();
	void SwitchTo3D();
public:
	const Vector3& GetPosition();
private:
	Vector3 m_Position;//摄像机位置
	Vector3 m_ViewCenter;//目标视点
	Vector3 m_Direction;//发射出去的方向向量
	bool m_IsMoveToLeft, m_IsMoveToRight, m_IsMoveToTop, m_IsMoveToBottom, m_IsMoveToFront, m_IsMoveToBack;
	float m_XMoveSpeed = 10.0f;
	float m_YMoveSpeed = 10.0f;
	float m_ZMoveSpeed = 5.0f;

	float m_Angle = 50.0f;
	float m_ViewportWidget = 800;
	float m_ViewportHeight = 600;
	float m_Near = 0.1f;
	float m_Far = 1000;
};