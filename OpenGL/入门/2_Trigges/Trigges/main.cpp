#include "main.h"

GLShaderManager shaderManager;//存储着色器
GLBatch triangleBatch;


int main(int argc, char **argv)
{
	gltSetWorkingDirectory(argv[0]);

	glutInit(&argc, argv);
	glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGBA | GLUT_DEPTH | GLUT_STENCIL); //渲染模式
	glutInitWindowSize(800, 600);
	glutCreateWindow("Triangles");

	glutReshapeFunc(ChangeSize);
	glutDisplayFunc(RenderScene);
	
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
	GLfloat vVertexs[] =
	{ -0.5f, 0.0f, 0.0f,
	0.5f, 0.0f, 0.0f,
	0.0f, 0.5f, 0.0f };
	triangleBatch.Begin(GL_TRIANGLES, 3);
	triangleBatch.CopyVertexData3f(vVertexs);
	triangleBatch.End();
}
void RenderScene()
{
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT | GL_STENCIL_BUFFER_BIT);
	GLfloat vRed[] = { 1.0, 0.0, 0.0, 1.0 };
	shaderManager.UseStockShader(GLT_SHADER_IDENTITY, vRed);//传递数据到默认着色器
	triangleBatch.Draw();
	glutSwapBuffers();
}
void SpecialKeys(int key, int x, int y)
{

}
