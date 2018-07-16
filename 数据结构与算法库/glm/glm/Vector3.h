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
	///����������
	Vector3 operator+(const Vector3& other) const;
	Vector3& operator+=(const Vector3& other);
	Vector3 operator-(const Vector3& other) const;
	Vector3& operator-=(const Vector3& other);
	bool operator==(const Vector3& other) const;
public:
	float Dot(const Vector3& other) const;            //���
	Vector3 Cross(const Vector3& other) const;        //���
	void Norlize();                             //������λ��
	Vector3 Norlize() const;                         //���ص�λ�����ֵ
	float Length() const;
public:
	///ȫ�ֹ��ܺ���
protected:
private:

};