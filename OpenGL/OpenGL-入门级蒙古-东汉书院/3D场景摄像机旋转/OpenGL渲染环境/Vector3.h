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
	Vector3& operator+=(const Vector3& other);
	Vector3 operator-(const Vector3& other) const;
	Vector3& operator-=(const Vector3& other);
	float operator*(const Vector3& other) const;//内积
	Vector3 operator*(float num) const;//数乘
	float Len() const;//长度
	void Normalize();//标准化
	Vector3 Cross(const Vector3& other) const;//叉乘
protected:
private:
};