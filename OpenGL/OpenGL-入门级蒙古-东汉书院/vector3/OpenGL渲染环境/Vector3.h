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
	Vector3 operator*(const Vector3& other);//内积
	Vector3 operator*(float num);//数乘
	float Len();//长度
	void Normalize();//标准化
	Vector3& Cross(const Vector3& other);//叉乘
protected:
private:
	float x, y, z;
};