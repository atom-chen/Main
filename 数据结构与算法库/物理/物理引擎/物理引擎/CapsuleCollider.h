#include "Extern.h"

enum DIRECTION3D
{
	X=0,
	Y=1,
	Z=2,
};
enum  DIRECTION2D
{
	Vertical=0,
	Horizontal=1,
};

//������ײ��
class CapsuleCollider2D
{
public:
	Vector2 CenterPoint;         //�м�������ĵ�����
	float Radius;                //����Բ�İ뾶
	float height;               //�м䲿�ֵĿ��
	DIRECTION2D direction;     //���Ż�������
protected:
private:
};

class CapsuleCollider3D
{
public:
	DIRECTION3D direction;
	Vector3 Center;
	float Height;        //����ĸ߶�
	float Radius;
protected:
private:
};