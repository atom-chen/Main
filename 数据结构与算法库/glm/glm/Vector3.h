#pragma once
#include <math.h>
class Vector3
{
public:
	float x, y, z;
public:
	Vector3();
	Vector3(const Vector3& other);
	Vector3(const float &x, const float &y, const float& z);
	Vector3& operator=(const Vector3& other);
public:
	///操作符函数
	Vector3 operator+(const Vector3& other) const;
	Vector3& operator+=(const Vector3& other);
	Vector3 operator-(const Vector3& other) const;
	Vector3& operator-=(const Vector3& other);
	bool operator==(const Vector3& other) const;
public:
	float Dot(const Vector3& other) const;            //点乘
	Vector3 Cross(const Vector3& other) const;        //叉乘
	void Norlize();                             //将自身单位化
	Vector3 Norlize() const;                         //返回单位化后的值
	float Length() const;
public:
	///全局功能函数
protected:
private:

};