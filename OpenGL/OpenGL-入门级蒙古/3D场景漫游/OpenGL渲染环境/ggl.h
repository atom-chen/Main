#pragma once

#include <windows.h>
#include <GL/GL.h>
#include <GL/GLU.h>
#include <gl/glext.h>
#include <stdio.h>
#include <math.h>
#include <string.h>
#include <string>
#include <sstream>
#include <iostream>
#include <vector>
#include <functional>
#include <tchar.h>
#include <functional>
using namespace std;

#define WINDOW_WIDTH	800							//为窗口宽度定义的宏，以方便在此处修改窗口宽度
#define WINDOW_HEIGHT	600							//为窗口高度定义的宏，以方便在此处修改窗口高度
#define WINDOW_TITLE	L"梦幻西游"	//为窗口标题定义的宏

#define INVALID -1
#define _INVALID_ID_ (-1)

static unsigned g_frame = 900000;                   //每帧间隔时间
static DWORD g_tPre = 0, g_tNow = 0;             //记录当前时间和上一次绘图时间