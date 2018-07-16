#include "Vector3.h"

Vector3::Vector3() :x(0), y(0), z(0){}
Vector3::Vector3(const Vector3& other) : x(other.x), y(other.y), z(other.z){}
Vector3::Vector3(const float &x, const float &y, const float& z) : x(x), y(y), z(z){}

Vector3& Vector3::operator=(const Vector3& other)
{
	this->x = other.x;
	this->y = other.y;
	this->z = other.z;
	return *this;
}

Vector3 Vector3::operator+(const Vector3& other) const
{
	return Vector3(this->x + other.x, this->y + other.y, this->z + other.z);
}

Vector3& Vector3::operator+=(const Vector3& other)
{
	this->x += other.x;
	this->y += other.y;
	this->z += other.z;
	return *this;
}
Vector3 Vector3::operator-(const Vector3& other) const
{
	return Vector3(this->x - other.x, this->y - other.y, this->z - other.z);
}
Vector3& Vector3::operator-=(const Vector3& other)
{
	this->x -= other.x;
	this->y -= other.y;
	this->z -= other.z;
	return *this;
}
bool Vector3::operator==(const Vector3& other) const
{
	return (x == other.x && y==other.y && z==other.z);
}

float Vector3::Dot(const Vector3& other) const
{
	return x*other.x + y*other.y + z*other.z;
}

Vector3 Vector3::Cross(const Vector3& other) const
{
	Vector3 ret;
	ret.x = y*other.z - z*other.y;
	ret.y = z*other.x - x*other.z;
	ret.z = x*other.y - y*other.x;
	return ret;
}
void Vector3::Norlize()
{
	float len = Length();
	this->x /= len;
	this->y /= len;
	this->z /= len;
}
Vector3 Vector3::Norlize() const
{
	float len = Length();
	return Vector3(x / len, y / len, z / len);
}
float Vector3::Length() const
{
	return sqrt(Dot(*this));
}