#pragma once
#include <math.h>
class Vector2
{
public:
	float x, y;
public:
	Vector2();
	Vector2(const Vector2& other);
	Vector2(const float &x, const float &y);
	Vector2& operator=(const Vector2& other);
public:
	///操作符函数
	Vector2 operator+(const Vector2& other) const;
	Vector2& operator+=(const Vector2& other);
	Vector2 operator-(const Vector2& other) const;
	Vector2& operator-=(const Vector2& other);
	bool operator==(const Vector2& other) const;
public:
	float Dot(const Vector2& other) const;            //点乘
	Vector2 Cross(const Vector2& other) const;        //叉乘
	void Norlize();                             //将自身单位化
	Vector2 Norlize() const;                         //返回单位化后的值
	float Length() const;
public:
	///全局功能函数
protected:
private:

};