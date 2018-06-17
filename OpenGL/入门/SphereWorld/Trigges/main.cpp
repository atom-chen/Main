#include "main.h"

GLMatrixStack modelViewMatrix;
GLMatrixStack projectionMatrix;
GLGeometryTransform transformPipeline;//������������
GLFrustum           viewFrustum;
GLShaderManager     shaderManager;

GLTriangleBatch		torusBatch;
GLBatch				floorBatch;
GLTriangleBatch		sphereBatch;
GLfloat vRed[] = { 1.0f, 0.0f, 0.0f, 1.0f };
GLfloat vGreen[] = { 0.0f, 1.0f, 0.0f, 1.0f };
GLfloat vBlue[] = { 0.0f, 0.0f, 1.0f, 1.0f };

void SetupRC()
{
	shaderManager.InitializeStockShaders();
	glEnable(GL_DEPTH_TEST);
	glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);

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

void RenderScene(void)
{
	static CStopWatch rotTimer;
	float yRot = rotTimer.GetElapsedSeconds() * 60.0f;
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	modelViewMatrix.PushMatrix();

	//���Ʊ���
	shaderManager.UseStockShader(GLT_SHADER_FLAT, transformPipeline.GetModelViewProjectionMatrix(), vRed);
	floorBatch.Draw();

	modelViewMatrix.Translate(0.0f, 0.0f, -2.5f);//T
	modelViewMatrix.PushMatrix();//I
	modelViewMatrix.Rotate(yRot, 0.0, 1.0, 0.0);//R
	shaderManager.UseStockShader(GLT_SHADER_FLAT, transformPipeline.GetModelViewProjectionMatrix(), vGreen);
	torusBatch.Draw();

	modelViewMatrix.PopMatrix();//������ת ->T

	modelViewMatrix.Rotate(yRot*-2.0f, 0.0f, 1.0f, 0.0f);//TR
	modelViewMatrix.Translate(0.8f, 0.0f, 0.0f);//TRT
	shaderManager.UseStockShader(GLT_SHADER_FLAT, transformPipeline.GetModelViewProjectionMatrix(), vBlue);
	sphereBatch.Draw();
	modelViewMatrix.PopMatrix();
	
	glutSwapBuffers();
	glutPostRedisplay();
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


	GLenum err = glewInit();
	if (GLEW_OK != err) {
		fprintf(stderr, "GLEW Error: %s\n", glewGetErrorString(err));
		return 1;
	}

	SetupRC();

	glutMainLoop();
	return 0;
}