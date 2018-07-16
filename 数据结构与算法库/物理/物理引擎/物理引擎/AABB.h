#include "Extern.h"


class AABBCollider2D
{
public:
	Vector2 LeftTopPoint;     //左上角
	Vector2 RightDownPoint;   //右下角
protected:
private:
};


class AABBCollider3D
{
public:
	Vector3 LeftTopPoint;     //左上角
	Vector3 RightDownPoint;   //右下角
	float AABBLong;          //立方体长度
protected:
private:
};

