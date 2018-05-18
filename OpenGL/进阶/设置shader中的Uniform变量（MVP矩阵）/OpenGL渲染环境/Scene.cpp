#include "Scene.h"
#include "Utils.h"
/*
插槽起始位置从0开始
不同标志符的插槽不共享
*/

GLuint vbo;                  //vbo相当于指向显存的指针
GLint program;
GLint positionLocation, modelMatrixLocation, viewMatrixLocation, projectionMatrixLocation;//申请几个插槽，存储几个shader里面变量的位置
glm::mat4 modelMatrix, viewMatrix, projectionMatrix;
bool Init()
{
	glewInit();
	float data[] = { -0.2f, -0.2f, -0.6f, 1.0f, 0.2f, -0.2f, -0.6f, 1.0f, 0, 0.2f, -0.6f, 1 };
	
	glGenBuffers(1, &vbo);//申请一个vbo,然后传vbo标识变量的地址。OpenGL会去修改这块内存，使之指向某个显存
	glBindBuffer(GL_ARRAY_BUFFER, vbo);//设置为当前vbo
	glBufferData(GL_ARRAY_BUFFER, sizeof(float) * 12, data, GL_STATIC_DRAW);//在当前vbo分配12个float大的内存，存入的值为data指针指向的这块内存，存入显存后就不会修改它了
	glBindBuffer(GL_ARRAY_BUFFER, 0);

	int32_t fileSize = 0;
	unsigned char* vertexShaderCode = LoadFileContent("res/test.vert", fileSize);
	ASSERT(vertexShaderCode);
	GLuint vertexShader = CompileShader(GL_VERTEX_SHADER, (char*)vertexShaderCode);
	delete vertexShaderCode;
	if (vertexShader == INVALID)
	{
		return 0;
	}
	unsigned char* fragmentShaderCode = LoadFileContent("res/text.frag", fileSize);
	ASSERT(fragmentShaderCode);
	GLuint fragmentShader = CompileShader(GL_FRAGMENT_SHADER, (char*)fragmentShaderCode);
	delete fragmentShaderCode;
	if (fragmentShader == INVALID)
	{
		return 0;
	}
	program = CreateProgram(vertexShader, fragmentShader);
	glDeleteShader(vertexShader);
	glDeleteShader(fragmentShader);
	if (program == 0)
	{
		return 0;
	}
	//获取插槽位置
	positionLocation = glGetAttribLocation(program, "position");
	positionLocation = glGetAttribLocation(program, "ModelMatrix");
	positionLocation = glGetAttribLocation(program, "ViewMatrix");
	positionLocation = glGetAttribLocation(program, "ProjectionMatrix");
	return 1;
}
void SetViewPortSize(float width, float height)
{
	projectionMatrix = glm::perspective(60.0f, width / height, 0.1f, 1000.0f);//设置投影矩阵
}
void Update()
{

}
void Draw()
{
	glUseProgram(program);      //使用program程序
	//为程序设置uniform变量
	glUniformMatrix4fv(modelMatrixLocation, 1, GL_FALSE, glm::value_ptr(modelMatrix));//插槽位置，设置几个这样的矩阵（插槽上可能是矩阵数组）,是否转置，数据的起始地址
	glUniformMatrix4fv(viewMatrixLocation, 1, GL_FALSE, glm::value_ptr(viewMatrix));
	glUniformMatrix4fv(projectionMatrixLocation, 1, GL_FALSE, glm::value_ptr(projectionMatrix));
	glUseProgram(0);     //好习惯：将当前程序设置为0号程序（状态机的概念）
}
void OnKeyDown(char KeyCode)
{

}

void OnKeyUp(char KeyCode)
{
}

void OnMouseMove(float deltaX, float deltaY)
{


}

