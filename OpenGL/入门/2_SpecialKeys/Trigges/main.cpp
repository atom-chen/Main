#include "main.h"

GLShaderManager shaderManager;//存储着色器
GLBatch cubeBatch;
GLfloat blockSize = 0.1f;//绘制一个矩形
GLfloat vVerts[] = 
{ -blockSize, -blockSize, 0.0f,
blockSize, -blockSize, 0.0f,
blockSize, blockSize, 0.0f,
-blockSize, blockSize, 0.0f };

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
	cubeBatch.Begin(GL_TRIANGLE_FAN, 4);
	cubeBatch.CopyVertexData3f(vVerts);
	cubeBatch.End();
}
void RenderScene()
{
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT | GL_STENCIL_BUFFER_BIT);
	shaderManager.UseStockShader(GLT_SHADER_IDENTITY, vRed);//传递数据到默认着色器
	cubeBatch.Draw();
	glutSwapBuffers();
}
void OnSpecialKeys(int key, int x, int y)
{
	GLfloat stepSize = 0.025f;

	GLfloat blockX = vVerts[0];
	GLfloat blockY = vVerts[7];

	switch (key)
	{
	case GLUT_KEY_UP:
		blockY += stepSize;
		break;

	case GLUT_KEY_DOWN:
		blockY -= stepSize;
		break;

	case GLUT_KEY_LEFT:
		blockX -= stepSize;
		break;

	case GLUT_KEY_RIGHT:
		blockX += stepSize;
		break;
	}

	if (blockX < XBOUND_MIN)
	{
		blockX = XBOUND_MIN;
	}
	if (blockX > XBOUND_MAX)
	{
		blockX = XBOUND_MAX;
	}
	if (blockY < YBOUND_MIN)
	{
		blockY = YBOUND_MIN;
	}
	if (blockY > YBOUND_MAX)
	{
		blockY = YBOUND_MAX;
	}
	//if (blockX < -1.0f) blockX = -1.0f;
	//if (blockX > (1.0f - blockSize * 2)) blockX = 1.0f - blockSize * 2;;
	//if (blockY < -1.0f + blockSize * 2)  blockY = -1.0f + blockSize * 2;
	//if (blockY > 1.0f) blockY = 1.0f;

	//vertex 1  左上
	vVerts[0] = blockX;
	vVerts[1] = blockY-blockSize*2;

	//vertex 2 右上
	vVerts[3] = blockX + blockSize * 2;
	vVerts[4] = blockY - blockSize * 2;

	//vertex 3 右下
	vVerts[6] = blockX + blockSize * 2;
	vVerts[7] = blockY;

	//vertex 4 左下
	vVerts[9] = blockX;
	vVerts[10] = blockY;

	cubeBatch.CopyVertexData3f(vVerts);
	glutPostRedisplay();
}
