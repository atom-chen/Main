#include "vector3f.h"

Vector3f::Vector3f(float x, float y, float z)
{
	this->x = x;
	this->y = y;
	this->z = z;
}
Vector3f::Vector3f()
{
	this->x = 0;
	this->y = 0;
	this->z = 0;
}

Vector3f Vector3f::operator*(float scaler)
{
	return Vector3f(x*scaler,y*scaler,z*scaler);
}

Vector3f Vector3f::operator^(Vector3f&r)
{
	return Vector3f(y*r.z-z*r.y,x*r.z-z*r.z,x*r.y-y*r.x);
}

float Vector3f::operator*(Vector3f&r)
{
	return x*r.x+y*r.y+z*r.z;
}

Vector3f Vector3f::operator+(Vector3f&r)
{
	return Vector3f(x+r.x,y+r.y,z+r.z);
}

Vector3f Vector3f::operator-(Vector3f&r)
{
	return Vector3f(x - r.x, y - r.y, z - r.z);
}

void Vector3f::operator=(Vector3f&r)
{
	x = r.x;
	y = r.y;
	z = r.z;
}

void Vector3f::Normalize()
{
	float len = Magnitude();
	x /= len;
	y /= len;
	z /= len;
}

float Vector3f::Magnitude()
{
	return sqrtf(x*x+y*y+z*z);
}