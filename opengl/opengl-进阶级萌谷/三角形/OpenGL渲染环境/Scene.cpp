#include "Scene.h"
#include "Utils.h"
/*
�����ʼλ�ô�0��ʼ
��ͬ��־���Ĳ�۲�����
*/

GLuint vbo;                  //vbo�൱��ָ���Դ��ָ�룬eboָ�������֯�Դ�������ݻ���ͼԪ
GLint program;
GLint positionLocation, modelMatrixLocation, viewMatrixLocation, projectionMatrixLocation;//���뼸����ۣ��洢����shader���������λ��
glm::mat4 modelMatrix, viewMatrix, projectionMatrix;
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
	//��ʼ���������ڲ��λ��
	positionLocation = glGetAttribLocation(program, "position");
	modelMatrixLocation = glGetUniformLocation(program, "ModelMatrix");
	viewMatrixLocation = glGetUniformLocation(program, "ViewMatrix");
	projectionMatrixLocation = glGetUniformLocation(program, "ProjectionMatrix");
	return 1;
}
void SetViewPortSize(float width, float height)
{
	projectionMatrix = glm::perspective(60.0f, width / height, 0.1f, 1000.0f);//����ͶӰ����
}
void Update()
{

}
void Draw()
{
	glClearColor(0, 0, 0, 1);
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	

	glUseProgram(program);      //ʹ��program����
	//Ϊ��������uniform����
	glUniformMatrix4fv(modelMatrixLocation, 1, GL_FALSE, glm::value_ptr(modelMatrix));//���λ�ã����ü��������ľ��󣨲���Ͽ����Ǿ������飩,�Ƿ�ת�ã����ݵ���ʼ��ַ
	glUniformMatrix4fv(viewMatrixLocation, 1, GL_FALSE, glm::value_ptr(viewMatrix));
	glUniformMatrix4fv(projectionMatrixLocation, 1, GL_FALSE, glm::value_ptr(projectionMatrix));
	
	
	glBindBuffer(GL_ARRAY_BUFFER, vbo);
	//���øò��
	glEnableVertexAttribArray(positionLocation);
	//����GPU,��ô��ȥ��ȡvbo���ڴ��
	glVertexAttribPointer(positionLocation, 4, GL_FLOAT, GL_FALSE, sizeof(float) * 4, 0);//��۵�λ�ã�����������м�������(x,y,z,w)��ÿ��������ʲô���ͣ��Ƿ��һ����������֮��ľ��룬���õ���Ϣ��vbo��ɶ�ط���ʼȡֵ
	
	//����������-������������ GPU�������3�����ݣ�Ȼ�󴫵�3��shader,��3��shader�õ��˲�ͬ�ĵ㣬�������ǵ�����������һģһ����
	glDrawArrays(GL_TRIANGLES, 0, 3);//����ʲô���ӵڼ����㿪ʼ���ƣ���Ҫ���ƶ��ٸ���
	glBindBuffer(GL_ARRAY_BUFFER, 0);
	glUseProgram(0);     //��ϰ�ߣ�����ǰ��������Ϊ0�ų���״̬���ĸ��
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

