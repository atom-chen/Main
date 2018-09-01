#include "main.h"

GLFrame             viewFrame;
GLFrustum           viewFrustum;
GLTriangleBatch     torusBatch;
GLMatrixStack       modelViewMatrix;
GLMatrixStack       projectionMatrix;
GLGeometryTransform transformPipeline;

GLuint	flatShader;			

GLint	locMVP;				
GLint   locColor;			



void SetupRC(void)
{
	glClearColor(0.0f, 0.0f, 0.0f, 1.0f);

	glEnable(GL_DEPTH_TEST);
	glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);

	viewFrame.MoveForward(4.0f);

	gltMakeTorus(torusBatch, .80f, 0.25f, 52, 26);

	flatShader = gltLoadShaderPairWithAttributes("ShaderSource\\book\\FlatShader.vp", "ShaderSource\\book\\FlatShader.fp", 1, GLT_ATTRIBUTE_VERTEX, "vVertex");

	locMVP = glGetUniformLocation(flatShader, "mvpMatrix");
	locColor = glGetUniformLocation(flatShader, "vColorValue");
}

void ShutdownRC(void)
{

}


void RenderScene(void)
{
	static CStopWatch rotTimer;

	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

	modelViewMatrix.PushMatrix(viewFrame);
	modelViewMatrix.Rotate(rotTimer.GetElapsedSeconds() * 10.0f, 0.0f, 1.0f, 0.0f);

	GLfloat vColor[] = { 0.1f, 0.1f, 1.f, 1.0f };

	glUseProgram(flatShader);
	glUniform4fv(locColor, 1, vColor);
	glUniformMatrix4fv(locMVP, 1, GL_FALSE, transformPipeline.GetModelViewProjectionMatrix());
	torusBatch.Draw();

	modelViewMatrix.PopMatrix();


	glutSwapBuffers();
	glutPostRedisplay();
}



void ChangeSize(int w, int h)
{
	if (h == 0)
		h = 1;

	glViewport(0, 0, w, h);

	viewFrustum.SetPerspective(35.0f, float(w) / float(h), 1.0f, 100.0f);

	projectionMatrix.LoadMatrix(viewFrustum.GetProjectionMatrix());
	transformPipeline.SetMatrixStacks(modelViewMatrix, projectionMatrix);
}

int main(int argc, char* argv[])
{
	gltSetWorkingDirectory(argv[0]);

	glutInit(&argc, argv);
	glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGBA | GLUT_DEPTH | GLUT_STENCIL);
	glutInitWindowSize(800, 600);
	glutCreateWindow("Simple Transformed Geometry");
	glutReshapeFunc(ChangeSize);
	glutDisplayFunc(RenderScene);

	GLenum err = glewInit();
	if (GLEW_OK != err) 
	{
		fprintf(stderr, "GLEW Error: %s\n", glewGetErrorString(err));
		return 1;
	}

	SetupRC();
	glutMainLoop();
	ShutdownRC();
	return 0;
}
