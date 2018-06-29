#include "main.h"

GLShaderManager shaderManager;//存储着色器

GLfloat vRed[] = { 1.0, 0.0, 0.0, 1.0 };

#define XBOUND_MIN -1.0f
#define XBOUND_MAX 1.0f-2*blockSize
#define YBOUND_MAX 1.0f
#define YBOUND_MIN -1.0f+2*blockSize

int main(int argc, char **argv)
{
	gltSetWorkingDirectory(argv[0]);

	glutInit(&argc, argv);
	glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGBA | GLUT_DEPTH | GLUT_STENCIL); //渲染模式
	glutInitWindowSize(800, 600);
	glutCreateWindow("Triangles");

	glutReshapeFunc(ChangeSize);
	glutDisplayFunc(RenderScene);
	glutSpecialFunc(OnSpecialKeys);

	GLenum err = glewInit();//初始化glew
	if (err != GLEW_OK)
	{
		fprintf(stderr, "GLEW Error%s\n", glewGetErrorString(err));
		exit(1);
	}
	shaderManager.InitializeStockShaders();
	SetupRC();
	glutMainLoop();
	return 0; 
}

void ChangeSize(int width, int height)
{
	glViewport(0, 0, width, height);
}
void SetupRC()
{
	glClearColor(255, 255, 255, 255);
}
void RenderScene()
{
	// Clear blue window
	glClearColor(0.0f, 0.0f, 1.0f, 0.0f);
	glClear(GL_COLOR_BUFFER_BIT);

	// Now set scissor to smaller red sub region
	glClearColor(1.0f, 0.0f, 0.0f, 0.0f);
	glScissor(100, 100, 600, 400);
	glEnable(GL_SCISSOR_TEST);
	glClear(GL_COLOR_BUFFER_BIT);

	// Finally, an even smaller green rectangle
	//glClearColor(0.0f, 1.0f, 0.0f, 0.0f);
	//glScissor(200, 200, 400, 200);
	//glClear(GL_COLOR_BUFFER_BIT);

	// Turn scissor back off for next render
	glDisable(GL_SCISSOR_TEST);

	glutSwapBuffers();
}
void OnSpecialKeys(int key, int x, int y)
{

}