#include "Scene.h"
#include "Utils.h"

GLuint vbo;                  //vbo�൱��ָ���Դ��ָ��
GLint program;
bool Init()
{
	glewInit();
	float data[] = { -0.2f, -0.2f, -0.6f, 1.0f, 0.2f, -0.2f, -0.6f, 1.0f, 0, 0.2f, -0.6f, 1 };
	
	glGenBuffers(1, &vbo);//����һ��vbo,Ȼ��vbo��ʶ�����ĵ�ַ��OpenGL��ȥ�޸�����ڴ棬ʹָ֮��ĳ���Դ�
	glBindBuffer(GL_ARRAY_BUFFER, vbo);//����Ϊ��ǰvbo
	glBufferData(GL_ARRAY_BUFFER, sizeof(float) * 12, data, GL_STATIC_DRAW);//�ڵ�ǰvbo����12��float����ڴ棬�����ֵΪdataָ��ָ�������ڴ棬�����Դ��Ͳ����޸�����
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
	return 1;
}
void SetViewPortSize(float width, float height)
{

}
void Update()
{

}
void Draw()
{

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

