#include <windows.h>
#include <GL/glut.h>

void init()
{
	glClearColor(1, 1, 1, 0);
	glMatrixMode(GL_PROJECTION);
	gluOrtho2D(0, 200, 0, 150);
}
void lineSegment()
{
	glClear(GL_COLOR_BUFFER_BIT);   //清理输出窗口
	glColor3f(0, 0.4, 0.2);
	glBegin(GL_LINES);
	glVertex2i(180, 15);
	glVertex2i(10, 145);
	glEnd();

	glFlush();
}
void main(int argc, char** args)
{
	glutInit(&argc, args);
	glutInitDisplayMode(GLUT_SINGLE | GLUT_RGB);
	glutInitWindowPosition(50, 100);
	glutInitWindowSize(400, 300);
	glutCreateWindow("An Example OpenGL Program");
	init();
	glutDisplayFunc(lineSegment);
	glutMainLoop();
}