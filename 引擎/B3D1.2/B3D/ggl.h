#pragma once

#include <windows.h>
//#include "glew.h"
#include <GL/GL.h>
//#include <GL/GLU.h>
//#include <gl/glext.h>
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
#include "EGL/egl.h"
#include "gles2/gl2.h"
#pragma comment(lib,"opengl32.lib")
#pragma comment(lib,"glu32.lib")
#pragma comment(lib,"winmm.lib")
#pragma comment(lib,"SOIL.lib")
#pragma comment(lib,"glew32.lib")
#pragma comment(lib,"libEGL.lib")
#pragma comment(lib,"libGLESv2.lib")
#pragma comment(lib,"FreeImage.lib")


using int32_t = int;
using namespace std;

#define WINDOW_WIDTH	800							//为窗口宽度定义的宏，以方便在此处修改窗口宽度
#define WINDOW_HEIGHT	600							//为窗口高度定义的宏，以方便在此处修改窗口高度

#define INVALID (0)
#define _INVALID_ID_ (-1)

#define ASSERT(p) if(p==nullptr || (*p)==_INVALID_ID_){return 0;}

