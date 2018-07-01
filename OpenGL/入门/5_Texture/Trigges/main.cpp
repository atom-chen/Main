#include "main.h"
#include "Tools.h"
#define TGA_PATH "stone.tga"
GLShaderManager		shaderManager;
GLMatrixStack		modelViewMatrix;
GLMatrixStack		projectionMatrix;
GLFrame				cameraFrame;
GLFrame             objectFrame;
GLFrustum			viewFrustum;

GLBatch             pyramidBatch;

GLuint              textureID;

GLGeometryTransform	transformPipeline;
M3DMatrix44f		shadowMatrix;





void SetupRC()
{
	// Black background
	glClearColor(0.7f, 0.7f, 0.7f, 1.0f);

	shaderManager.InitializeStockShaders();

	glEnable(GL_DEPTH_TEST);

	glGenTextures(1, &textureID);
	glBindTexture(GL_TEXTURE_2D, textureID);
	LoadTGATexture(TGA_PATH, GL_LINEAR, GL_LINEAR, GL_CLAMP_TO_EDGE);

	MakePyramid(pyramidBatch);

	cameraFrame.MoveForward(-7.0f);
}

void ShutdownRC(void)
{
	glDeleteTextures(1, &textureID);
}


void RenderScene(void)
{
	static GLfloat vLightPos[] = { 1.0f, 1.0f, 0.0f };
	static GLfloat vWhite[] = { 1.0f, 1.0f, 1.0f, 1.0f };

	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT | GL_STENCIL_BUFFER_BIT);

	modelViewMatrix.PushMatrix();
	M3DMatrix44f mCamera;
	cameraFrame.GetCameraMatrix(mCamera);
	modelViewMatrix.MultMatrix(mCamera);

	M3DMatrix44f mObjectFrame;
	objectFrame.GetMatrix(mObjectFrame);
	modelViewMatrix.MultMatrix(mObjectFrame);

	glBindTexture(GL_TEXTURE_2D, textureID);
	shaderManager.UseStockShader(GLT_SHADER_TEXTURE_POINT_LIGHT_DIFF,transformPipeline.GetModelViewMatrix(),transformPipeline.GetProjectionMatrix(),
		vLightPos, vWhite, 0);

	pyramidBatch.Draw();


	modelViewMatrix.PopMatrix();

	// Flush drawing commands
	glutSwapBuffers();
}


void SpecialKeys(int key, int x, int y)
{
	if (key == GLUT_KEY_UP)
		objectFrame.RotateWorld(m3dDegToRad(-5.0f), 1.0f, 0.0f, 0.0f);

	if (key == GLUT_KEY_DOWN)
		objectFrame.RotateWorld(m3dDegToRad(5.0f), 1.0f, 0.0f, 0.0f);

	if (key == GLUT_KEY_LEFT)
		objectFrame.RotateWorld(m3dDegToRad(-5.0f), 0.0f, 1.0f, 0.0f);

	if (key == GLUT_KEY_RIGHT)
		objectFrame.RotateWorld(m3dDegToRad(5.0f), 0.0f, 1.0f, 0.0f);

	glutPostRedisplay();
}




void ChangeSize(int w, int h)
{
	glViewport(0, 0, w, h);
	viewFrustum.SetPerspective(35.0f, float(w) / float(h), 1.0f, 500.0f);
	projectionMatrix.LoadMatrix(viewFrustum.GetProjectionMatrix());
	transformPipeline.SetMatrixStacks(modelViewMatrix, projectionMatrix);
}

int main(int argc, char* argv[])
{
	gltSetWorkingDirectory(argv[0]);

	glutInit(&argc, argv);
	glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGBA | GLUT_DEPTH | GLUT_STENCIL);
	glutInitWindowSize(800, 600);
	glutCreateWindow("Pyramid");
	glutReshapeFunc(ChangeSize);
	glutSpecialFunc(SpecialKeys);
	glutDisplayFunc(RenderScene);

	GLenum err = glewInit();
	if (GLEW_OK != err) {
		fprintf(stderr, "GLEW Error: %s\n", glewGetErrorString(err));
		return 1;
	}


	SetupRC();

	glutMainLoop();

	ShutdownRC();

	return 0;
}