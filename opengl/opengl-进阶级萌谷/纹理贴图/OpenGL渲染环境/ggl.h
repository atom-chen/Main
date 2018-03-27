#pragma once

#include <windows.h>
#include "glew.h"
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
#include "Glm/glm.hpp"
#include "Glm/ext.hpp"

using int32_t = int;
using namespace std;

#define WINDOW_WIDTH	800							//为窗口宽度定义的宏，以方便在此处修改窗口宽度
#define WINDOW_HEIGHT	600							//为窗口高度定义的宏，以方便在此处修改窗口高度
#define WINDOW_TITLE	L"梦幻西游"	//为窗口标题定义的宏

#define INVALID (0)
#define _INVALID_ID_ (-1)

#define ASSERT_PTR_BOOL(p) if(p==nullptr){return 0;}

#define ASSERT_INT_BOOL(i) if(i==0){return 0;}