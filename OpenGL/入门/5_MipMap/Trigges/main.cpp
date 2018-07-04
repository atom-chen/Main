#include "main.h"
#include "Tools.h"
#define TGA_PATH "stone.tga"
GLShaderManager		shaderManager;
GLMatrixStack		modelViewMatrix;
GLMatrixStack		projectionMatrix;
GLFrustum			viewFrustum;//�Ӿ���
GLGeometryTransform	transformPipeline;//�仯����

GLBatch             floorBatch;
GLBatch             ceilingBatch;
GLBatch             leftWallBatch;
GLBatch             rightWallBatch;

GLfloat             viewZ = -65.0f;

#define TEXTURE_BRICK  0
#define TEXTURE_FLOOR 1
#define TEXTURE_CEILING 2
#define TEXTURE_COUNT 3


GLuint texture[TEXTURE_COUNT];
const char* szTextureFiles[TEXTURE_COUNT] = { "brick.tga", "floor.tga", "ceiling.tga" };


M3DMatrix44f		shadowMatrix;

//Ϊÿ������ �ı���map
//0 GL_NEAREST 1 GL_LINEAR
void ProcessMenu(int flag)
{
	for (GLuint i = 0; i < TEXTURE_COUNT; i++)
	{
		glBindTexture(GL_TEXTURE_2D, texture[i]);
		switch (flag)
		{
		case 0:
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
			break;
		case 1:
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
			break;
		case 2:
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST_MIPMAP_NEAREST);//ѡ�ĸ��Լ���ѡ�е�������죬ʹ�����ڽ�ԭ��
			break;
		case 3:
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST_MIPMAP_LINEAR);//ѡ�ĸ�ʹ�����Բ�ֵ,��ѡ�е��������ʹ�����ڽ�ԭ��
			break;
		case 4:
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_NEAREST);//ѡ�ĸ�ʹ�����ڽ�ԭ��,��ѡ�е��������ʹ�����Բ�ֵ
			break;
		case 5:
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR);//ѡ�ĸ��Լ���ѡ�е�������죬ʹ�����Բ�ֵ
			break;
		}
	}
	glutPostRedisplay();
}



void SetupRC()
{
	glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
	shaderManager.InitializeStockShaders();
	glEnable(GL_DEPTH_TEST);
	GLbyte* pByte;
	GLint iWidth, iHeight, iComponents;
	GLenum eFormat;

	//׼������ begin
	glGenTextures(TEXTURE_COUNT, texture);
	for (GLuint i = 0; i < TEXTURE_COUNT; i++)
	{
		//����һ���������
		glBindTexture(GL_TEXTURE_2D, texture[i]);

		//�����������ù������ͻ���ģʽ
		pByte = gltReadTGABits(szTextureFiles[i], &iWidth, &iHeight, &iComponents, &eFormat);

		//���ó������
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);

		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_R, GL_CLAMP_TO_EDGE);

		glGenerateMipmap(GL_TEXTURE_2D);
		free(pByte);
	}
	//׼������ end

	//׼���������� begin

}

void ShutdownRC(void)
{

}
void OnDrawBegin()
{
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT | GL_STENCIL_BUFFER_BIT);
	modelViewMatrix.PushMatrix();
}
void OnDrawEnd()
{
	modelViewMatrix.PopMatrix();

	// Flush drawing commands
	glutSwapBuffers();
}

void RenderScene(void)
{
	static GLfloat vLightPos[] = { 1.0f, 1.0f, 0.0f };
	static GLfloat vWhite[] = { 1.0f, 1.0f, 1.0f, 1.0f };




}


void SpecialKeys(int key, int x, int y)
{


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