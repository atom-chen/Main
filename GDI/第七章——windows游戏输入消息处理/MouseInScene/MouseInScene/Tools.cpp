
#include "Tools.h"
void LitmitCursor(const HWND &hwnd)
{
	RECT rect;
	POINT lt, rb;
	//拿到窗口内部的矩形
	GetClientRect(hwnd, &rect);

	lt.x = rect.left;
	lt.y = rect.top;

	rb.x = rect.right;
	rb.y = rect.bottom;
	//转化两个点为屏幕坐标
	ClientToScreen(hwnd, &lt);
	ClientToScreen(hwnd, &rb);
	rect.left = lt.x;
	rect.top = lt.y;
	rect.right = rb.x;
	rect.bottom = rb.y;
	ClipCursor(&rect);
}