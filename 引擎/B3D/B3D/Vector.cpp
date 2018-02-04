#include "Vector.h"

namespace B3D
{
#pragma region Vector2
	Vector2& Vector2::operator=(const Vector2& vector2)
	{
		this->x = vector2.x;
		this->y = vector2.y;
		return *this;
	}
	Vector2 Vector2::operator+(const Vector2& vector2)
	{
		Vector2 v2;
		v2.x = this->x + vector2.x;
		v2.y = this->y + vector2.y;
		return v2;
	}
	Vector2 Vector2::operator-(const Vector2& vector2)
	{
		Vector2 v2;
		v2.x = this->x - vector2.x;
		v2.y = this->y - vector2.y;
		return v2;
	}
	Vector2 Vector2::operator*(const float& num)//数乘
	{
		Vector2 v2;
		v2.x = this->x*num;
		v2.y = this->y*num;
		return v2;
	}
	float Vector2::Dot(const Vector2& vector2) const  //点乘（内积）
	{
		return this->x*vector2.x + this->y*vector2.y;
	}
	Vector2 Vector2::Cross(const Vector2& vector2) const //叉乘
	{
		Vector2 temp;
		temp.x = this->x*vector2.y;
		temp.y = this->y*vector2.x;
		return temp;
	}
	float Vector2::Angle(const Vector2& vector2) const    //两个向量的夹角
	{
		float cos = this->Dot(vector2) / (this->GetLength()*vector2.GetLength());
		double temp = acos(cos);
		return (float)temp;
	}
	Vector2 Vector2::Normalize()
	{
		float len = GetLength();
		if (len <= 0)
		{
			return NULLVECTOR2;
		}
		return Vector2(this->x / len, this->y / len);
	}

	bool Vector2::operator==(const Vector2& vector2) const
	{
		return this->x == vector2.x && this->y == vector2.y;
	}
	bool Vector2::IsZero()
	{
		return *this == Vector2(0, 0);
	}
	float Vector2::GetLength() const
	{
		return sqrt(x*x + y*y);
	}
	Vector2 Vector2::SetReverse()                      //取反
	{
		return Vector2(-(this->x), -(this->y));
	}
#pragma endregion

#pragma region Vector3
	
#pragma endregion


#pragma region Vector4

#pragma endregion



	
}
