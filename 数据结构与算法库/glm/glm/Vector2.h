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
	///����������
	Vector2 operator+(const Vector2& other) const;
	Vector2& operator+=(const Vector2& other);
	Vector2 operator-(const Vector2& other) const;
	Vector2& operator-=(const Vector2& other);
	bool operator==(const Vector2& other) const;
public:
	float Dot(const Vector2& other) const;            //���
	Vector2 Cross(const Vector2& other) const;        //���
	void Norlize();                             //������λ��
	Vector2 Norlize() const;                         //���ص�λ�����ֵ
	float Length() const;
public:
	///ȫ�ֹ��ܺ���
protected:
private:

};