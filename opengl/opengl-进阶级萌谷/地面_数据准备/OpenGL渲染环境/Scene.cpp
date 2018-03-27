#include "Scene.h"
#include "Utils.h"
/*
�����ʼλ�ô�0��ʼ
��ͬ��־���Ĳ�۲�����
*/

GLuint vbo,ebo;                  //vbo�൱��ָ���Դ��ָ�룬eboָ�������֯�Դ�������ݻ���ͼԪ
GLint program;
GLint positionLocation, modelMatrixLocation, viewMatrixLocation, projectionMatrixLocation,colorLocation;//���뼸����ۣ��洢����shader���������λ��
GLint texcoordLocation, textureLocation;//uv����ͼ��λ��
GLuint texture;
glm::mat4 modelMatrix, viewMatrix, projectionMatrix;
bool Init()
{
	glewInit();
	//////////////////////////////////////////////////////////////////////////vbo��position,color,uv
	float data[] = { -0.2f, -0.2f, -0.6f, 1.0f, 1,1,1,1,0,0 ,
		0.2f, -0.2f, -0.6f, 1.0f,0,1,0,1,1,0,
		0, 0.2f, -0.6f, 1 ,1,0,0,1,0.5f,1};
	vbo = CreateBufferObject(GL_ARRAY_BUFFER, sizeof(float) * 30, GL_STATIC_DRAW,data);
	ASSERT_INT_BOOL(vbo);

	//////////////////////////////////////////////////////////////////////////ebo
	unsigned short indexes[] = { 0, 1, 2 };
	ebo = CreateBufferObject(GL_ELEMENT_ARRAY_BUFFER, sizeof(float) * 3, GL_STATIC_DRAW, indexes);
	ASSERT_INT_BOOL(ebo);

	//////////////////////////////////////////////////////////////////////////����shader
	int32_t fileSize = 0;
	unsigned char* vertexShaderCode = LoadFileContent("res/test.vert", fileSize);
	ASSERT_PTR_BOOL(vertexShaderCode);
	GLuint vertexShader = CompileShader(GL_VERTEX_SHADER, (char*)vertexShaderCode);
	delete vertexShaderCode;
	if (vertexShader == _INVALID_ID_)
	{
		return 0;
	}
	unsigned char* fragmentShaderCode = LoadFileContent("res/text.frag", fileSize);
	ASSERT_PTR_BOOL(fragmentShaderCode);
	GLuint fragmentShader = CompileShader(GL_FRAGMENT_SHADER, (char*)fragmentShaderCode);
	delete fragmentShaderCode;
	if (fragmentShader == _INVALID_ID_)
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
	colorLocation = glGetAttribLocation(program, "color");
	texcoordLocation = glGetAttribLocation(program, "texcoord");

	modelMatrixLocation = glGetUniformLocation(program, "ModelMatrix");
	viewMatrixLocation = glGetUniformLocation(program, "ViewMatrix");
	projectionMatrixLocation = glGetUniformLocation(program, "ProjectionMatrix");
	textureLocation = glGetUniformLocation(program, "U_Texture");
	

	texture = CreateTexture2DFromBMP("res/test.bmp");
	ASSERT_INT_BOOL(texture);
	return 1;
}
void SetViewPortSize(float width, float height)
{
	projectionMatrix = glm::perspective(60.0f, width / height, 0.1f, 1000.0f);//����ͶӰ����
}
void Update()
{
	float deltime = GetFrameTime();
}
void Draw()
{
	glClearColor(0.1f, 0.4f, 0.6f, 1);
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	

	glUseProgram(program);      //ʹ��program����
	//Ϊ��������uniform����
	glUniformMatrix4fv(modelMatrixLocation, 1, GL_FALSE, glm::value_ptr(modelMatrix));//���λ�ã����ü��������ľ��󣨲���Ͽ����Ǿ������飩,�Ƿ�ת�ã����ݵ���ʼ��ַ
	glUniformMatrix4fv(viewMatrixLocation, 1, GL_FALSE, glm::value_ptr(viewMatrix));
	glUniformMatrix4fv(projectionMatrixLocation, 1, GL_FALSE, glm::value_ptr(projectionMatrix));
	glBindTexture(GL_TEXTURE_2D, texture);
	glUniform1i(textureLocation, 0);

	//���øò��
	glBindBuffer(GL_ARRAY_BUFFER, vbo);
	glEnableVertexAttribArray(positionLocation);
	glEnableVertexAttribArray(colorLocation);
	glEnableVertexAttribArray(texcoordLocation);

	//����GPU,��ô��ȥ��ȡvbo���ڴ��
	glVertexAttribPointer(positionLocation, 4, GL_FLOAT, GL_FALSE, sizeof(float) * 10, 0);//��۵�λ�ã�����������м�������(x,y,z,w)��ÿ��������ʲô���ͣ��Ƿ��һ����������֮��ľ��룬���õ���Ϣ��vbo��ɶ�ط���ʼȡֵ
	//�����м����һ��position����ӦΪ�Լ���color+position�ľ��룻��ʼλ����position��߹�ӦΪ4��float�ľ���
	glVertexAttribPointer(colorLocation, 4, GL_FLOAT, GL_FALSE, sizeof(float) * 10, (void*)(sizeof(float)*4));
	glVertexAttribPointer(texcoordLocation, 2, GL_FLOAT, GL_FALSE, sizeof(float) * 10, (void*)(sizeof(float) * 8));

	//����������-������������ GPU�������3�����ݣ�Ȼ�󴫵�3��shader,��3��shader�õ��˲�ͬ�ĵ㣬�������ǵ�����������һģһ����
	
	//д��2����Ժ��
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, ebo);
	glDrawElements(GL_TRIANGLES, 3, GL_UNSIGNED_SHORT, 0);//��ʲô�������ٸ��������ݣ��������ͣ����ݵ���ʼλ��
	
	//д��1������
	//unsigned short arIndex[] ={0, 1, 2, 3, 4,5,6,7,8,9,10,11};
	//glDrawElements(GL_TRIANGLES, 3, GL_UNSIGNED_SHORT, arIndex);//����ģʽ��ȡ���Σ��������ͣ��ֱ�ȡpos�����е���Щ�±�

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

