#include "Scene.h"
#include "Utils.h"
/*
插槽起始位置从0开始
不同标志符的插槽不共享
*/

GLuint vbo;                  //vbo相当于指向显存的指针，ebo指导如何组织显存里的数据绘制图元
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
	//初始化变量所在插槽位置
	positionLocation = glGetAttribLocation(program, "position");
	modelMatrixLocation = glGetUniformLocation(program, "ModelMatrix");
	viewMatrixLocation = glGetUniformLocation(program, "ViewMatrix");
	projectionMatrixLocation = glGetUniformLocation(program, "ProjectionMatrix");
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
	glClearColor(0, 0, 0, 1);
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	

	glUseProgram(program);      //使用program程序
	//为程序设置uniform变量
	glUniformMatrix4fv(modelMatrixLocation, 1, GL_FALSE, glm::value_ptr(modelMatrix));//插槽位置，设置几个这样的矩阵（插槽上可能是矩阵数组）,是否转置，数据的起始地址
	glUniformMatrix4fv(viewMatrixLocation, 1, GL_FALSE, glm::value_ptr(viewMatrix));
	glUniformMatrix4fv(projectionMatrixLocation, 1, GL_FALSE, glm::value_ptr(projectionMatrix));
	
	
	glBindBuffer(GL_ARRAY_BUFFER, vbo);
	//启用该插槽
	glEnableVertexAttribArray(positionLocation);
	//告诉GPU,怎么样去读取vbo的内存块
	glVertexAttribPointer(positionLocation, 4, GL_FLOAT, GL_FALSE, sizeof(float) * 4, 0);//插槽的位置，插槽中数据有几个分量(x,y,z,w)，每个分量是什么类型，是否归一化，两个点之间的距离，设置的信息从vbo的啥地方开始取值
	
	//绘制三角形-》发生的事情 GPU会遍历这3个数据，然后传到3个shader,这3个shader拿到了不同的点，但是他们的三个矩阵是一模一样的
	glDrawArrays(GL_TRIANGLES, 0, 3);//绘制什么，从第几个点开始绘制，我要绘制多少个点
	glBindBuffer(GL_ARRAY_BUFFER, 0);
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

