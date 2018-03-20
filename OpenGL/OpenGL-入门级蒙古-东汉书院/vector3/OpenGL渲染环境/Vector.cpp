#include "Vector3.h"


Vector3::Vector3(float x = 0, float y = 0, float z = 0)
{
	this->x = x;
	this->y = y;
	this->z = z;
}

void Vector3::CopyFrom(const Vector3& other)
{
	this->x = other.x;
	this->y = other.y;
	this->z = other.z;
}

Vector3 Vector3::operator + (const Vector3& other)
{
	return Vector3(x+other.x,y+other.y,z+other.z);
}

Vector3 Vector3::operator-(const Vector3& other)
{
	return Vector3(x - other.x, y - other.y, z - other.z);
}

Vector3 Vector3::operator*(const Vector3& other)//内积
{
	return Vector3(x*other.x, y*other.y, z*other.z);
}

Vector3 Vector3::operator*(float num)//数乘
{
	return Vector3(x*num, y*num, z*num);
}

float Vector3::Len()//长度
{
	return sqrtf(x*x + y*y + z*z);
}

void Vector3::Normalize()//标准化
{
	float len = this->Len();
	this->x /= len;
	this->y /= len;
	this->z /= len;
}

Vector3& Vector3::Cross(const Vector3& other)//叉乘
{
	return Vector3(this->y*other.z - this->z*other.y, this->z*other.x - this->x*other.z, this->x*other.y - this->y*other.x);
}