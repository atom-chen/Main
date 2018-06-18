#include "Oval.h"
bool first = 0;

inline int Oval_GetD1(int a, int b, int x, int y)
{
	int a2 = a*a;
	int b2 = b*b;
	return b2*x*x + a2*y*y - a2*b2;
}
inline int Oval_GetD2(int a, int b, int x, int y)
{

}

//aÎªºábÎªÊú
void OvalDraw1(int a, int b)
{
	int x = 0;
	int y = b;
	float d1 = b*b + a*a*(-b + 0.25f);
	if (!first)
	{
		printf("d1=%f\n", d1);
		d1 = Oval_GetD1(a, b, x, y);
		printf("d1=%f\n", d1);
	}


	//part 1
	while (b*b*(x + 1) < a*a*(y - 0.5f))
	{
		DrawPoint(x, y);
		if (d1 < 0)
		{
			d1 += b*b*(2 * x + 3);
			x++;
		}
		else
		{
			d1 += b*b*(2 * x + 3) + a*a*(-2 * y + 2);
			x++;
			y--;
		}
	}
	//part 2
	float d2 = sqrt(b*(x + 0.5f)) + sqrt((float)a*(y - 1)) - sqrt((float)a*b);
	if (!first)
	{
		printf("d2=%f\n", d2);
		d2 = Oval_GetD1(a, b, x, y);
		printf("d2=%f\n", d2);
		first = 1;
	}

	while (y > 0)
	{
		if (d2 < 0)
		{
			d2+=b*b*(2 * x + 2) + a*a*(-2 * y + 3);
			x++;
			y--;
		}
		else
		{
			d2 += a*a*(-2 * y + 3);
			y--;
		}
		DrawPoint(x, y);
	}
}



void DrawOval(int a, int b)
{
	OvalDraw1(a, b);
}