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
public:
	const Vector3& GetPosition();
private:
	Vector3 m_Position;//�����λ��
	Vector3 m_ViewCenter;//Ŀ���ӵ�
	Vector3 m_Direction;//�����ȥ�ķ�������
	bool m_IsMoveToLeft, m_IsMoveToRight, m_IsMoveToTop, m_IsMoveToBottom, m_IsMoveToFront, m_IsMoveToBack;
	float m_XMoveSpeed = 0.5f;
	float m_YMoveSpeed = 0.5f;
	float m_ZMoveSpeed = 0.2f;
};