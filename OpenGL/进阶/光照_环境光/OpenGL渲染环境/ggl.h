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
#include <map>
#include <functional>
#include <tchar.h>
#include <functional>
#include "Glm/glm.hpp"
#include "Glm/ext.hpp"

using int32_t = int;
using namespace std;

#define WINDOW_WIDTH	800							//Ϊ���ڿ�ȶ���ĺ꣬�Է����ڴ˴��޸Ĵ��ڿ��
#define WINDOW_HEIGHT	600							//Ϊ���ڸ߶ȶ���ĺ꣬�Է����ڴ˴��޸Ĵ��ڸ߶�
#define WINDOW_TITLE	L"�λ�����"	//Ϊ���ڱ��ⶨ��ĺ�

#define INVALID (-1)
#define _INVALID_ID_ (0)
#define _INVALID_LOCATION_ (-1)

#define ASSERT_PTR_BOOL(p) if(p==nullptr){return 0;}

#define ASSERT_INT_BOOL(i) if(i==_INVALID_ID_){return 0;}

#define ASSERT_INT_VOID(i) if(i==_INVALID_ID_){return ;}

#define ARRLEN(arr) sizeof(arr)/sizeof(arr[0])


#define BEGIN 	m_VertexBuf.Begin();{m_Shader.Begin();{m_Shader.Bind(glm::value_ptr(m_ModelMatrix), glm::value_ptr(viewMatrix), glm::value_ptr(ProjectionMatrix));
#define END 		}m_Shader.End();}m_VertexBuf.End();

#define DRAW_MODEL BEGIN { glDrawArrays(GL_TRIANGLES,0,m_VertexBuf.GetLenth());}END