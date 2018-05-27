#include "MidDrawLine.h"
#include "Tools.h"


void Mid(int x0, int y0, int x1, int y1)
{
	//ax+by+c=0
	int a = y0 - y1;
	int b = x1 - x0;
	int c = x0*y1 - x1*y0;
	int d = 2*a + b;//F(M)ֵ
	int d1 = 2 * a;
	int d2 = 2*(a + b);

	int x = x0, y = y0;

	while (x <= x1)
	{
		DrawPoint(x, y);
		//׼����һ��������
		//mid��ֱ���·���ȡP2
		if (d < 0)
		{
			x++;
			y++;
			d += d2;
		}
		//mid��ֱ���Ϸ���ȡP1
		else
		{
			x++;
			d += d1;
		}
	}
}
