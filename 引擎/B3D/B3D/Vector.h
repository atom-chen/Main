#pragma once
#include "B3DClass.h"
/************************************************************************/
/* ���ܣ�������������ϵ
   ���ߣ�
                                                                   */
/************************************************************************/
namespace B3D
{
#pragma region Vector2
#define NULLVECTOR2 Vector2(-999,-999);
	class Vector2
	{
	public:    //���쿽�����
		Vector2() :x(0), y(0)
		{

		}
		Vector2(float x, float y) :x(x), y(y)
		{

		}
		Vector2(const Vector2& vector2)
		{
			this->x = vector2.x;
			this->y = vector2.y;
		}
		Vector2& operator= (const Vector2& vector2);
	public:     //����������㺯��
		Vector2 operator+(const Vector2& vector2);
		Vector2 operator-(const Vector2& vector2);
		Vector2 operator*(const float& num);//����
		float Dot(const Vector2& vector2) const;   //���
		Vector2 Cross(const Vector2& vector2) const; //���
		float Angle(const Vector2& vector2) const;    //���������ļн�
		Vector2 Normalize();                       //�淶��
		Vector2 SetReverse();                      //ȡ��

	public:       //�������
		bool operator==(const Vector2& vector2) const;
		bool IsZero();
		float GetLength() const;   //�������ĳ���


		
		
	private:
		//��ĳ���
		
	public:    
		static const Vector2 Up;
		static const Vector2 Down;
		static const Vector2 Right;
		static const Vector2 Left;
	private:
		float x;
		float y;
	};
	const Vector2 Vector2::Up = Vector2(0, 1);
	const Vector2 Vector2::Down = Vector2(0, -1);
	const Vector2 Vector2::Right = Vector2(1, 0);
	const Vector2 Vector2::Left = Vector2(-1, 0);
#pragma endregion


#pragma region Vector3
	class Vector3
	{
	public:
	protected:
	private:
	};
#pragma endregion



#pragma region Vecror4
	class Vector4
	{
	public:
		Vector4(float w, float x, float y, float z):w(w), x(x), y(y), z(z)
		{

		}
	protected:
	private:
		float w;
		float x;
		float y;
		float z;
	};
#pragma endregion
}
