#include "Vector2.h"

Vector2::Vector2() :x(0), y(0){}
Vector2::Vector2(const Vector2& other) : x(other.x), y(other.y){}
Vector2::Vector2(const float &x, const float &y) : x(x), y(y){}

Vector2& Vector2::operator=(const Vector2& other)
{
	this->x = other.x;
	this->y = other.y;
	return *this;
}

Vector2 Vector2::operator+(const Vector2& other) const
{
	return Vector2(this->x + other.x, this->y + other.y);
}

Vector2& Vector2::operator+=(const Vector2& other)
{
	this->x += other.x;
	this->y += other.y;
	return *this;
}
Vector2 Vector2::operator-(const Vector2& other) const
{
	return Vector2(this->x - other.x, this->y - other.y);
}
Vector2& Vector2::operator-=(const Vector2& other)
{
	this->x -= other.x;
	this->y -= other.y;
	return *this;
}
bool Vector2::operator==(const Vector2& other) const
{
	return (x == other.x && y == other.y);
}

float Vector2::Dot(const Vector2& other) const
{
	return x*other.x + y*other.y ;
}

Vector2 Vector2::Cross(const Vector2& other) const
{
	Vector2 ret;
	ret.x = y- other.y;
	ret.y = other.x - x;
	return ret;
}
void Vector2::Norlize()
{
	float len = Length();
	this->x /= len;
	this->y /= len;
}
Vector2 Vector2::Norlize() const
{
	float len = Length();
	return Vector2(x / len, y / len);
}
float Vector2::Length() const
{
	return sqrt(Dot(*this));
}