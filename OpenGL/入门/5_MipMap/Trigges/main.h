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

//GL_REPEAT,这个比较简单，就是重复边界的纹理。
//GL_CLAMP采用这种模式是，opengl就在一个2X2的加权纹理单元数组中使用取自边框的纹理单元。这时候的边框如果没有设置的话，应该就是原纹理的边界的像素值。
//GL_CLAMP_TO_EDGE，在这种模式下，边框始终被忽略。位于纹理边缘或者靠近纹理边缘的纹理单元将用于纹理计算，但不使用纹理边框上的纹理单元。
//GL_CLAMP_TO_BORDER如果纹理坐标位于范围[0,1]之外，那么只用边框纹理单元（如果没有边框，则使用常量边框颜色，我想常量边框颜色就是黑色）用于纹理。