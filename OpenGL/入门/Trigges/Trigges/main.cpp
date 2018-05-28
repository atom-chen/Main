#include "main.h"



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
	SetupRC();
	glutMainLoop();
	return 0; 
}

void ChangeSize(int width, int height)
{
	glViewport(0, 0, width, height);
}
void RenderScene()
{

}
void SetupRC()
{

}