#include <Windows.h>
#pragma  comment(lib,"winmm.lib")
void myymain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
	PlaySound(L"FirstBlood.wav", NULL, SND_FILENAME || SND_ASYNC);
	MessageBox(NULL, L"First blood!��ã���Ϸ���磡�����������ˣ�", L"First blood!��Ϣ����", 0);
}
int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
	myymain(hInstance, hPrevInstance, lpCmdLine, nCmdShow);
	return 0;
}


