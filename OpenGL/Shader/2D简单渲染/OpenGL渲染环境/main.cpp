#include "ggl.h"
#include "WinApp.h"
#include "GamaMain.h"
int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
	EngineBehavior  *main=new GameLogic;
	main->Awake(hInstance);
}
