#include "main.h"

GLBatch	triangleBatch;
GLShaderManager	shaderManager;

GLint	myIdentityShader;

namespace Normal
{
	void ChangeSize(int w, int h)
	{
		glViewport(0, 0, w, h);
	}


	void SetupRC()
	{
		// Blue background
		glClearColor(0.0f, 0.0f, 0.0f, 1.0f);

		shaderManager.InitializeStockShaders();

		// Load up a triangle
		GLfloat vVerts[] = { -0.5f, 0.0f, 0.0f,
			0.5f, 0.0f, 0.0f,
			0.0f, 0.5f, 0.0f };

		GLfloat vColors[] = { 1.0f, 0.0f, 0.0f, 1.0f,
			0.0f, 1.0f, 0.0f, 1.0f,
			0.0f, 0.0f, 1.0f, 1.0f };

		triangleBatch.Begin(GL_TRIANGLES, 3);
		triangleBatch.CopyVertexData3f(vVerts);
		triangleBatch.CopyColorData4f(vColors);
		triangleBatch.End();

		myIdentityShader = gltLoadShaderPairWithAttributes("ShaderSource\\book\\ShadedIdentity.vp", "ShaderSource\\book\\ShadedIdentity.fp", 2,
			GLT_ATTRIBUTE_VERTEX, "vVertex", GLT_ATTRIBUTE_COLOR, "vColor");
	}


	void ShutdownRC()
	{
		glDeleteProgram(myIdentityShader);

	}


	void RenderScene(void)
	{
		// Clear the window with current clearing color
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT | GL_STENCIL_BUFFER_BIT);

		glUseProgram(myIdentityShader);
		triangleBatch.Draw();

		// Perform the buffer swap to display back buffer
		glutSwapBuffers();
	}
}
namespace provoking
{

	void ChangeSize(int w, int h)
	{
		glViewport(0, 0, w, h);
	}


	void SetupRC()
	{
		// Blue background
		glClearColor(0.0f, 0.0f, 0.0f, 1.0f);

		shaderManager.InitializeStockShaders();

		// Load up a triangle
		GLfloat vVerts[] = { -0.5f, 0.0f, 0.0f,
			0.5f, 0.0f, 0.0f,
			0.0f, 0.5f, 0.0f };

		GLfloat vColors[] = { 1.0f, 0.0f, 0.0f, 1.0f,
			0.0f, 1.0f, 0.0f, 1.0f,
			0.0f, 0.0f, 1.0f, 1.0f };

		triangleBatch.Begin(GL_TRIANGLES, 3);
		triangleBatch.CopyVertexData3f(vVerts);
		triangleBatch.CopyColorData4f(vColors);
		triangleBatch.End();

		myIdentityShader = gltLoadShaderPairWithAttributes("ProvokingVertex.vp", "ProvokingVertex.fp", 2,
			GLT_ATTRIBUTE_VERTEX, "vVertex", GLT_ATTRIBUTE_COLOR, "vColor");

		glProvokingVertex(GL_FIRST_VERTEX_CONVENTION);
	}


	void ShutdownRC()
	{
		glDeleteProgram(myIdentityShader);

	}

	void RenderScene(void)
	{
		// Clear the window with current clearing color
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT | GL_STENCIL_BUFFER_BIT);

		glUseProgram(myIdentityShader);
		triangleBatch.Draw();

		// Perform the buffer swap to display back buffer
		glutSwapBuffers();
	}

	int nToggle = 1;
	void KeyPressFunc(unsigned char key, int x, int y)
	{
		if (key == 32)
		{
			nToggle++;

			if (nToggle % 2 == 0) 
			{
				glProvokingVertex(GL_LAST_VERTEX_CONVENTION);                                 //flat取最后一个顶点的值
				glutSetWindowTitle("Provoking Vertex - Last Vertex - Press Space Bars");
			}
			else 
			{
				glProvokingVertex(GL_FIRST_VERTEX_CONVENTION);                                //flat取第一个顶点的值
				glutSetWindowTitle("Provoking Vertex - First Vertex - Press Space Bars");
			}

			glutPostRedisplay();
		}
	}



	///////////////////////////////////////////////////////////////////////////////
	// Main entry point for GLUT based programs
	int main(int argc, char* argv[])
	{
		gltSetWorkingDirectory(argv[0]);

		glutInit(&argc, argv);
		glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGBA | GLUT_DEPTH | GLUT_STENCIL);
		glutInitWindowSize(800, 600);
		glutCreateWindow("Provoking Vertex - First Vertex - Press Space Bars");
		glutReshapeFunc(ChangeSize);
		glutDisplayFunc(RenderScene);
		glutKeyboardFunc(KeyPressFunc);


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

}
using namespace provoking;
int main(int argc, char* argv[])
{
	gltSetWorkingDirectory(argv[0]);

	glutInit(&argc, argv);
	glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGBA | GLUT_DEPTH | GLUT_STENCIL);
	glutInitWindowSize(800, 600);
	glutCreateWindow("Shaded Triangle");
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