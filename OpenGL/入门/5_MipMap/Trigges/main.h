#pragma  once
#include <GLTools.h>
#include <GLShaderManager.h>
#include <GLFrustum.h>
#include <GLBatch.h>
#include <GLMatrixStack.h>
#include <GLGeometryTransform.h>
#include <StopWatch.h>

#include <math.h>
#include <stdio.h>

#ifdef __APPLE__
#include <glut/glut.h>
#else
#define FREEGLUT_STATIC
#include <GL/glut.h>
#endif

#pragma  comment(lib,"freeglut_static.lib")
#pragma  comment(lib,"gltools.lib")

void ChangeSize(int width, int height);
void RenderScene();
void OnDrawBegin();
void Draw();
void OnDrawEnd();
void SetupRC();
void OnSpecialKeys(int key, int x, int y);

//GL_REPEAT,����Ƚϼ򵥣������ظ��߽������
//GL_CLAMP��������ģʽ�ǣ�opengl����һ��2X2�ļ�Ȩ����Ԫ������ʹ��ȡ�Ա߿������Ԫ����ʱ��ı߿����û�����õĻ���Ӧ�þ���ԭ����ı߽������ֵ��
//GL_CLAMP_TO_EDGE��������ģʽ�£��߿�ʼ�ձ����ԡ�λ�������Ե���߿��������Ե������Ԫ������������㣬����ʹ������߿��ϵ�����Ԫ��
//GL_CLAMP_TO_BORDER�����������λ�ڷ�Χ[0,1]֮�⣬��ôֻ�ñ߿�����Ԫ�����û�б߿���ʹ�ó����߿���ɫ�����볣���߿���ɫ���Ǻ�ɫ����������