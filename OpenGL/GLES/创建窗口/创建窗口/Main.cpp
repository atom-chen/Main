#include "glfw3.h"
#include "glfw3native.h"
#pragma comment(lib,"glfw3_x32.lib")
#pragma comment(lib,"opengl32.lib")

int main()
{
	glfwInit();
	GLFWwindow *window = glfwCreateWindow(640, 480, "GLFWµÄ", NULL, NULL);
	glfwMakeContextCurrent(window);
	while (!glfwWindowShouldClose(window))
	{
		glfwSwapBuffers(window);
	}
	return 0;
}