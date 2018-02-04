#include <Windows.h>
#include <tchar.h>

void myyyymain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
	MSG msg = {0};
	while (msg.message != WM_QUIT)
	{
		//如果有消息
		if (PeekMessage(&msg, 0, 0, 0, PM_REMOVE))
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
		else{
			
		}
	}
}
