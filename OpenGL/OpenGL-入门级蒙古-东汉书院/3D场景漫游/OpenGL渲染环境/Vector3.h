#pragma  once
#include "ggl.h"

class Vector3
{
public:
	float x, y, z;
	Vector3(float x = 0, float y = 0, float z = 0);
	void CopyFrom(const Vector3& other);
public:
	Vector3 operator+(const Vector3& other) const;
	Vector3 operator-(const Vector3& other) const;
	Vector3 operator*(const Vector3& other) const;//�ڻ�
	Vector3 operator*(float num) const;//����
	float Len() const;//����
	void Normalize();//��׼��
	Vector3 Cross(const Vector3& other) const;//���
protected:
private:
};