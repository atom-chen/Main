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
void SetupRC();
void OnSpecialKeys(int key, int x, int y);

