#pragma  once
#include <GLTools.h>
#include <GLShaderManager.h>
#include <GLMatrixStack.h>
#include <GLFrame.h>
#include <GLFrustum.h>
#include <GLGeometryTransform.h>
#include <gl/glew.h>

#ifdef __APPLE
#include <freeglut/GL/glut.h>
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

