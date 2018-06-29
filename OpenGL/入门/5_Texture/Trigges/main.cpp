#include "main.h"
#include "Tools.h"
#define NUM_SPHERE 50
#define TGA_PATH "stone.tga"
GLFrame spheres[NUM_SPHERE];

GLMatrixStack modelViewMatrix;
GLMatrixStack projectionMatrix;
GLGeometryTransform transformPipeline;//矩阵对阵管理器
GLFrustum           viewFrustum;
GLShaderManager     shaderManager;

GLuint texture;
GLBatch pyramidBatch;

GLFrame cameraFrame;
M3DMatrix44f  mCamera;//view Matrix

GLfloat vRed[] = { 1.0f, 0.0f, 0.0f, 1.0f };
GLfloat vGreen[] = { 0.0f, 1.0f, 0.0f, 1.0f };
GLfloat vBlue[] = { 0.0f, 0.0f, 1.0f, 1.0f };

M3DVector4f vLightPos = { 0.0f, 10.0f, 5.0f, 1.0f };//光位置
M3DVector4f vLightEyePos;//眼睛的位置由摄像机决定
void MakePyramid(GLBatch& pyramidBatch)
{

}

void SetupRC()
{
	shaderManager.InitializeStockShaders();
	glEnable(GL_DEPTH_TEST);
	//glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);

	glClearColor(0.0f, 0.0f, 0.0f, 1.0f);

	texture=LoadTGATexture(TGA_PATH, GL_LINEAR, GL_LINEAR, GL_CLAMP_TO_EDGE);
	MakePyramid(pyramidBatch);
}
void ChangeSize(int w, int h)
{
	if (h == 0)
	{
		h = 1;
	}
	glViewport(0, 0, w, h);
	//设置视景体以及投影矩阵
	viewFrustum.SetPerspective(35.0f, float(w) / float(h), 1.0f, 1000.0f);
	projectionMatrix.LoadMatrix(viewFrustum.GetProjectionMatrix());
	transformPipeline.SetMatrixStacks(modelViewMatrix, projectionMatrix);//设置变换管线
}
void OnDrawBegin()
{
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

	//摄像机处理
	cameraFrame.GetCameraMatrix(mCamera);//获取viewMatrix
	modelViewMatrix.PushMatrix(mCamera);//V 1

	m3dTransformVector4(vLightEyePos, vLightPos, mCamera);//更新眼睛的位置
}
void Draw()
{

}
void OnDrawEnd()
{
	modelViewMatrix.PopMatrix();
	glutSwapBuffers();
	glutPostRedisplay();
}
void RenderScene(void)
{
	OnDrawBegin();
	Draw();
	OnDrawEnd();
}

void OnSpecialKeys(int key,int x,int y)
{
	float linear = 0.1f;
	float angular = float(m3dDegToRad(5.0f));//每帧旋转5度

	if (key == GLUT_KEY_UP)
	{
		cameraFrame.MoveForward(linear);
	}
	if (key == GLUT_KEY_DOWN)
	{
		cameraFrame.MoveForward(-linear);
	}
	if (key == GLUT_KEY_RIGHT)
	{
		cameraFrame.RotateWorld(-angular, 0.0f, 1.0f, 0.0f);
	}
	if (key == GLUT_KEY_LEFT)
	{
		cameraFrame.RotateWorld(angular, 0.0f, 1.0f, 0.0f);
	}
}




int main(int argc, char* argv[])
{
	gltSetWorkingDirectory(argv[0]);

	glutInit(&argc, argv);
	glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGBA | GLUT_DEPTH | GLUT_STENCIL);
	glutInitWindowSize(800, 600);
	glutCreateWindow("Sphere World Example");
	glutReshapeFunc(ChangeSize);
	glutDisplayFunc(RenderScene);
	glutSpecialFunc(OnSpecialKeys);

	GLenum err = glewInit();
	if (GLEW_OK != err) {
		fprintf(stderr, "GLEW Error: %s\n", glewGetErrorString(err));
		return 1;
	}

	SetupRC();

	glutMainLoop();
	return 0;
}