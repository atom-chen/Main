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

	void Pitch(float angle);//���Լ���X����ת
	void Yaw(float angle);//����������ϵy��ת
	void RotateView(float angle, float x, float y, float z);//����������ת��x,y,zΪ��ķ���������

	void SwitchTo2D();
	void SwitchTo3D();
public:
	const Vector3& GetPosition();
private:
	Vector3 m_Position;//�����λ��
	Vector3 m_ViewCenter;//Ŀ���ӵ�
	Vector3 m_Direction;//�����ȥ�ķ�������
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