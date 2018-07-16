#include "Extern.h"

enum DIRECTION3D
{
	X=0,
	Y=1,
	Z=2,
};
enum  DIRECTION2D
{
	Vertical=0,
	Horizontal=1,
};

//胶囊碰撞体
class CapsuleCollider2D
{
public:
	Vector2 CenterPoint;         //中间矩形中心点坐标
	float Radius;                //两端圆的半径
	float height;               //中间部分的宽度
	DIRECTION2D direction;     //横着或是竖着
protected:
private:
};

class CapsuleCollider3D
{
public:
	DIRECTION3D direction;
	Vector3 Center;
	float Height;        //沿轴的高度
	float Radius;
protected:
private:
};