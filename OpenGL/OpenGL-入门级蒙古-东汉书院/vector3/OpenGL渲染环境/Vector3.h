#pragma  once
#include "ggl.h"

class Vector3
{
public:
	Vector3(float x = 0, float y = 0, float z = 0);
	void CopyFrom(const Vector3& other);
public:
	Vector3 operator+(const Vector3& other);
	Vector3 operator-(const Vector3& other);
	Vector3 operator*(const Vector3& other);//�ڻ�
	Vector3 operator*(float num);//����
	float Len();//����
	void Normalize();//��׼��
	Vector3& Cross(const Vector3& other);//���
protected:
private:
	float x, y, z;
};