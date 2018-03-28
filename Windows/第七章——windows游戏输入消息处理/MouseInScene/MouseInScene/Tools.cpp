
#include "Tools.h"
void LitmitCursor(const HWND &hwnd)
{
	RECT rect;
	POINT lt, rb;
	//�õ������ڲ��ľ���
	GetClientRect(hwnd, &rect);

	lt.x = rect.left;
	lt.y = rect.top;

	rb.x = rect.right;
	rb.y = rect.bottom;
	//ת��������Ϊ��Ļ����
	ClientToScreen(hwnd, &lt);
	ClientToScreen(hwnd, &rb);
	rect.left = lt.x;
	rect.top = lt.y;
	rect.right = rb.x;
	rect.bottom = rb.y;
	ClipCursor(&rect);
}