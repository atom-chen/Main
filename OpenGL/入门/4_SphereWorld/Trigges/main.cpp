#include "main.h"
#define NUM_SPHERE 50
GLFrame spheres[NUM_SPHERE];

GLMatrixStack modelViewMatrix;
GLMatrixStack projectionMatrix;
GLGeometryTransform transformPipeline;//������������
GLFrustum           viewFrustum;
GLShaderManager     shaderManager;

GLTriangleBatch		torusBatch;
GLBatch				floorBatch;
GLTriangleBatch		sphereBatch;

GLFrame cameraFrame;
M3DMatrix44f  mCamera;//view Matrix

GLfloat vRed[] = { 1.0f, 0.0f, 0.0f, 1.0f };
GLfloat vGreen[] = { 0.0f, 1.0f, 0.0f, 1.0f };
GLfloat vBlue[] = { 0.0f, 0.0f, 1.0f, 1.0f };

M3DVector4f vLightPos = { 0.0f, 10.0f, 5.0f, 1.0f };//��λ��
M3DVector4f vLightEyePos;//�۾���λ�������������
void SetupRC()
{
	shaderManager.InitializeStockShaders();
	glEnable(GL_DEPTH_TEST);
	//glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);

	glClearColor(0.0f, 0.0f, 0.0f, 1.0f);

	//����
	gltMakeTorus(torusBatch, 0.4f, 0.15f, 30, 30);

	//��s
	gltMakeSphere(sphereBatch, 0.1f, 26, 13);

	//�����Ķ�������
	floorBatch.Begin(GL_LINES, 324);
	for (GLfloat x = -20.0; x <= 20.0f; x += 0.5) {
		floorBatch.Vertex3f(x, -0.55f, 20.0f);
		floorBatch.Vertex3f(x, -0.55f, -20.0f);

		floorBatch.Vertex3f(20.0f, -0.55f, x);
		floorBatch.Vertex3f(-20.0f, -0.55f, x);
	}
	floorBatch.End();

	for (int i = 0; i < NUM_SPHERE; i++)
	{
		GLfloat x = ((rand() % 400) - 200.0f)*0.1f;
		GLfloat z = ((rand() % 400) - 200.0f)*0.1f;
		spheres[i].SetOrigin(x, 0.1f, z);
	}
}
void ChangeSize(int w, int h)
{
	if (h == 0)
	{
		h = 1;
	}
	glViewport(0, 0, w, h);
	//�����Ӿ����Լ�ͶӰ����
	viewFrustum.SetPerspective(35.0f, float(w) / float(h), 1.0f, 1000.0f);
	projectionMatrix.LoadMatrix(viewFrustum.GetProjectionMatrix());
	transformPipeline.SetMatrixStacks(modelViewMatrix, projectionMatrix);//���ñ任����
}
void OnDrawBegin()
{
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

	//���������
	cameraFrame.GetCameraMatrix(mCamera);//��ȡviewMatrix
	modelViewMatrix.PushMatrix(mCamera);//V 1

	m3dTransformVector4(vLightEyePos, vLightPos, mCamera);//�����۾���λ��
}
void Draw()
{
	static CStopWatch rotTimer;
	float yRot = rotTimer.GetElapsedSeconds() * 60.0f;

	//���Ʊ���
	shaderManager.UseStockShader(GLT_SHADER_FLAT, transformPipeline.GetModelViewProjectionMatrix(), vRed);
	floorBatch.Draw();

	//С��
	for (int i = 0; i < NUM_SPHERE; i++)
	{
		modelViewMatrix.PushMatrix();//v 2
		modelViewMatrix.MultMatrix(spheres[i]);//view*model
		shaderManager.UseStockShader(GLT_SHADER_POINT_LIGHT_DIFF, transformPipeline.GetModelViewMatrix(), 
			transformPipeline.GetProjectionMatrix(), vLightEyePos, vBlue);
		modelViewMatrix.PopMatrix();//v 1
		sphereBatch.Draw();
	}

	modelViewMatrix.PushMatrix();//v 2
	//����
	modelViewMatrix.Translate(0.0f, 0.0f, -2.5f);//vT
	modelViewMatrix.Rotate(yRot, 0.0, 1.0, 0.0);//v*t*R
	shaderManager.UseStockShader(GLT_SHADER_POINT_LIGHT_DIFF, transformPipeline.GetModelViewMatrix(),
		transformPipeline.GetProjectionMatrix(), vLightEyePos, vGreen);
	torusBatch.Draw();

	modelViewMatrix.PopMatrix();//v 1
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
	float angular = float(m3dDegToRad(5.0f));//ÿ֡��ת5��

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