#include "MidDrawLine.h"
#include "Tools.h"


void Mid(int x0, int y0, int x1, int y1)
{
	//ax+by+c=0
	int a = y0 - y1;
	int b = x1 - x0;
	int c = x0*y1 - x1*y0;
	int d = 2*a + b;//F(M)值
	int d1 = 2 * a;
	int d2 = 2*(a + b);

	int x = x0, y = y0;

	while (x <= x1)
	{
		DrawPoint(x, y);
		//准备下一个点数据
		//mid在直线下方，取P2
		if (d < 0)
		{
			x++;
			y++;
			d += d2;
		}
		//mid在直线上方，取P1
		else
		{
			x++;
			d += d1;
		}
	}
}
