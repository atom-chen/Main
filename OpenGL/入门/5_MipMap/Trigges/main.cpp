#include "main.h"
#include "Tools.h"
#define TGA_PATH "stone.tga"
GLShaderManager		shaderManager;
GLMatrixStack		modelViewMatrix;
GLMatrixStack		projectionMatrix;
GLFrustum			viewFrustum;//视景体
GLGeometryTransform	transformPipeline;//变化管线

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
const char* szTextureFiles[TEXTURE_COUNT] = { "res//brick.tga", "res//floor.tga", "res//ceiling.tga" };


M3DMatrix44f		shadowMatrix;

//为每个纹理 改变其map
//0 GL_NEAREST 1 GL_LINEAR
void ProcessMenu(int flag)
{
	GLfloat fLargest;
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
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST_MIPMAP_NEAREST);//选哪个以及对选中的如何拉伸，使用最邻近原则
			break;
		case 3:
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST_MIPMAP_LINEAR);//选哪个使用线性插值,对选中的如何拉伸使用最邻近原则
			break;
		case 4:
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_NEAREST);//选哪个使用最邻近原则,对选中的如何拉伸使用线性插值
			break;
		case 5:
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR);//选哪个以及对选中的如何拉伸，使用线性插值
			break;
		case 6:
			glGetFloatv(GL_MAX_TEXTURE_MAX_ANISOTROPY_EXT, &fLargest);
			glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_MAX_ANISOTROPY_EXT, fLargest);//打开最大化各向异性过滤，远景的细节清晰许多
			break;

		case 7:
			glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_MAX_ANISOTROPY_EXT, 1.0f);//关闭各向异性过滤
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

	//准备纹理 begin
	glGenTextures(TEXTURE_COUNT, texture);
	for (GLuint i = 0; i < TEXTURE_COUNT; i++)
	{
		//绑定下一个纹理对象
		glBindTexture(GL_TEXTURE_2D, texture[i]);

		//加载纹理，设置过滤器和环绕模式
		pByte = gltReadTGABits(szTextureFiles[i], &iWidth, &iHeight, &iComponents, &eFormat);

		//设置常规参数
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);

		//设置当这些UV超出纹理边界时应该怎样处理
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_R, GL_CLAMP_TO_EDGE);

		glTexImage2D(GL_TEXTURE_2D, 0, iComponents, iWidth, iHeight, 0, eFormat, GL_UNSIGNED_BYTE, pByte);
		glGenerateMipmap(GL_TEXTURE_2D);
		free(pByte);
	}
	//准备纹理 end

	//准备顶点数据 begin
	GLfloat z;
	floorBatch.Begin(GL_TRIANGLE_STRIP, 28, 1);
	for (z = 60.0f; z >= 0.0f; z -= 10.0f)
	{
		floorBatch.MultiTexCoord2f(0, 0.0f, 0.0f);
		floorBatch.Vertex3f(-10.0f, -10.0f, z);

		floorBatch.MultiTexCoord2f(0, 1.0f, 0.0f);
		floorBatch.Vertex3f(10.0f, -10.0f, z);

		floorBatch.MultiTexCoord2f(0, 0.0f, 1.0f);
		floorBatch.Vertex3f(-10.0f, -10.0f, z - 10.0f);

		floorBatch.MultiTexCoord2f(0, 1.0f, 1.0f);
		floorBatch.Vertex3f(10.0f, -10.0f, z - 10.0f);
	}
	floorBatch.End();

	ceilingBatch.Begin(GL_TRIANGLE_STRIP, 28, 1);
	for (z = 60.0f; z >= 0.0f; z -= 10.0f)
	{
		ceilingBatch.MultiTexCoord2f(0, 0.0f, 1.0f);
		ceilingBatch.Vertex3f(-10.0f, 10.0f, z - 10.0f);

		ceilingBatch.MultiTexCoord2f(0, 1.0f, 1.0f);
		ceilingBatch.Vertex3f(10.0f, 10.0f, z - 10.0f);

		ceilingBatch.MultiTexCoord2f(0, 0.0f, 0.0f);
		ceilingBatch.Vertex3f(-10.0f, 10.0f, z);

		ceilingBatch.MultiTexCoord2f(0, 1.0f, 0.0f);
		ceilingBatch.Vertex3f(10.0f, 10.0f, z);
	}
	ceilingBatch.End();

	leftWallBatch.Begin(GL_TRIANGLE_STRIP, 28, 1);
	for (z = 60.0f; z >= 0.0f; z -= 10.0f)
	{
		leftWallBatch.MultiTexCoord2f(0, 0.0f, 0.0f);
		leftWallBatch.Vertex3f(-10.0f, -10.0f, z);

		leftWallBatch.MultiTexCoord2f(0, 0.0f, 1.0f);
		leftWallBatch.Vertex3f(-10.0f, 10.0f, z);

		leftWallBatch.MultiTexCoord2f(0, 1.0f, 0.0f);
		leftWallBatch.Vertex3f(-10.0f, -10.0f, z - 10.0f);

		leftWallBatch.MultiTexCoord2f(0, 1.0f, 1.0f);
		leftWallBatch.Vertex3f(-10.0f, 10.0f, z - 10.0f);
	}
	leftWallBatch.End();


	rightWallBatch.Begin(GL_TRIANGLE_STRIP, 28, 1);
	for (z = 60.0f; z >= 0.0f; z -= 10.0f)
	{
		rightWallBatch.MultiTexCoord2f(0, 0.0f, 0.0f);
		rightWallBatch.Vertex3f(10.0f, -10.0f, z);

		rightWallBatch.MultiTexCoord2f(0, 0.0f, 1.0f);
		rightWallBatch.Vertex3f(10.0f, 10.0f, z);

		rightWallBatch.MultiTexCoord2f(0, 1.0f, 0.0f);
		rightWallBatch.Vertex3f(10.0f, -10.0f, z - 10.0f);

		rightWallBatch.MultiTexCoord2f(0, 1.0f, 1.0f);
		rightWallBatch.Vertex3f(10.0f, 10.0f, z - 10.0f);
	}
	rightWallBatch.End();
	//准备顶点数据 end
}

void ShutdownRC(void)
{
	glDeleteTextures(TEXTURE_COUNT, texture);
}
void OnDrawBegin()
{
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT | GL_STENCIL_BUFFER_BIT);
	modelViewMatrix.PushMatrix();

}
void Draw()
{
	modelViewMatrix.Translate(0, 0, viewZ);//摄像机移动

	shaderManager.UseStockShader(GLT_SHADER_TEXTURE_REPLACE, transformPipeline.GetModelViewProjectionMatrix(), 0);
	glBindTexture(GL_TEXTURE_2D, texture[TEXTURE_FLOOR]);
	floorBatch.Draw();

	glBindTexture(GL_TEXTURE_2D, texture[TEXTURE_CEILING]);
	ceilingBatch.Draw();

	glBindTexture(GL_TEXTURE_2D, texture[TEXTURE_BRICK]);
	leftWallBatch.Draw();
	rightWallBatch.Draw();
}
void OnDrawEnd()
{
	modelViewMatrix.PopMatrix();

	// Flush drawing commands
	glutSwapBuffers();
}

void RenderScene(void)
{
	OnDrawBegin();

	Draw();

	OnDrawEnd();
}


void SpecialKeys(int key, int x, int y)
{
	if (key == GLUT_KEY_DOWN)
	{
		viewZ -= 0.5f;
	}
	else if (key == GLUT_KEY_UP)
	{
		viewZ += 0.5f;
	}

	glutPostRedisplay();
}




void ChangeSize(int w, int h)
{
	if (h == 0)
	{
		h = 1;
	}
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
	glutCreateWindow("MipMap");
	glutReshapeFunc(ChangeSize);
	glutSpecialFunc(SpecialKeys);
	glutDisplayFunc(RenderScene);

	//添加菜单入口以改变过滤器
	glutCreateMenu(ProcessMenu);
	glutAddMenuEntry("GL_NEAREAT", 0);
	glutAddMenuEntry("GL_LINEAR", 1);
	glutAddMenuEntry("GL_NEAREST_MIPMAP_NEAREST", 2);
	glutAddMenuEntry("GL_NEAREST_MIPMAP_LINEAR", 3);
	glutAddMenuEntry("GL_LINEAR_MIPMAP_NEAREST", 4);
	glutAddMenuEntry("GL_LINEAR_MIPMAP_LINEAR", 5);
	if (1 || gltIsExtSupported("GL_EXT_texture_filter_anisotropic"))
	{
		glutAddMenuEntry("Anisotropic Filter", 6);
		glutAddMenuEntry("Anisotropic Off", 7);
	}
	glutAttachMenu(GLUT_RIGHT_BUTTON);


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