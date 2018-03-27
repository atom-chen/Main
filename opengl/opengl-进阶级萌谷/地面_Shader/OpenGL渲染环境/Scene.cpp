#include "Scene.h"
#include "Utils.h"
#include "Shader.h"
/*
插槽起始位置从0开始
不同标志符的插槽不共享
*/
Shader textShader;
GLuint vbo,ebo;                  //vbo相当于指向显存的指针，ebo指导如何组织显存里的数据绘制图元
GLuint texture;
glm::mat4 modelMatrix, viewMatrix, projectionMatrix;
bool Awake()
{
	glewInit();
	//////////////////////////////////////////////////////////////////////////vbo：position,color,uv
	float data[] = { -0.2f, -0.2f, -0.6f, 1.0f, 1,1,1,1,0,0 ,
		0.2f, -0.2f, -0.6f, 1.0f,0,1,0,1,1,0,
		0, 0.2f, -0.6f, 1 ,1,0,0,1,0.5f,1};
	vbo = CreateBufferObject(GL_ARRAY_BUFFER, sizeof(float) * 30, GL_STATIC_DRAW,data);
	ASSERT_INT_BOOL(vbo);

	//////////////////////////////////////////////////////////////////////////ebo
	unsigned short indexes[] = { 0, 1, 2 };
	ebo = CreateBufferObject(GL_ELEMENT_ARRAY_BUFFER, sizeof(float) * 3, GL_STATIC_DRAW, indexes);
	ASSERT_INT_BOOL(ebo);

	//////////////////////////////////////////////////////////////////////////加载shader
	if (!textShader.Init("res/test.vert", "res/text.frag"))
	{
		return 0;
	}

	texture = CreateTexture2DFromBMP("res/test.bmp");
	ASSERT_INT_BOOL(texture);
	return 1;
}

void SetViewPortSize(float width, float height)
{
	projectionMatrix = glm::perspective(60.0f, width / height, 0.1f, 1000.0f);//设置投影矩阵
}
void Update()
{
	float deltime = GetFrameTime();

}
void OnDrawBegin()
{
	glClearColor(0.1f, 0.4f, 0.6f, 1);
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
}
void Draw()
{
	//glUseProgram(textShader.GetProgram());
	textShader.UseThis();
	//为程序设置uniform变量
	textShader.SetMatrix(modelMatrix, viewMatrix, projectionMatrix);
	textShader.SetTexture_2D(texture);

	glBindBuffer(GL_ARRAY_BUFFER, vbo);
	//启用该插槽
	glEnableVertexAttribArray(textShader.GetPositionLocation());
	glEnableVertexAttribArray(textShader.GetColorLocation());
	glEnableVertexAttribArray(textShader.GetTexcoordLocation());

	//告诉GPU,怎么样去读取vbo的内存块
	glVertexAttribPointer(textShader.GetPositionLocation(), 4, GL_FLOAT, GL_FALSE, sizeof(float) * 10, 0);//插槽的位置，插槽中数据有几个分量(x,y,z,w)，每个分量是什么类型，是否归一化，两个点之间的距离，设置的信息从vbo的啥地方开始取值
	//由于中间夹了一组position，故应为自己的color+position的距离；起始位置在position后边故应为4个float的距离
	glVertexAttribPointer(textShader.GetColorLocation(), 4, GL_FLOAT, GL_FALSE, sizeof(float) * 10, (void*)(sizeof(float) * 4));
	glVertexAttribPointer(textShader.GetTexcoordLocation(), 2, GL_FLOAT, GL_FALSE, sizeof(float) * 10, (void*)(sizeof(float) * 8));

	//绘制三角形-》发生的事情 GPU会遍历这3个数据，然后传到3个shader,这3个shader拿到了不同的点，但是他们的三个矩阵是一模一样的
	
	//写法2：书院版
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, ebo);
	glDrawElements(GL_TRIANGLES, 3, GL_UNSIGNED_SHORT, 0);//画什么，画多少个索引数据，数据类型，数据的起始位置
	
	//写法1：引擎
	//unsigned short arIndex[] ={0, 1, 2, 3, 4,5,6,7,8,9,10,11};
	//glDrawElements(GL_TRIANGLES, 3, GL_UNSIGNED_SHORT, arIndex);//绘制模式，取几次，数据类型，分别都取pos数组中的哪些下标
}
void OnDrawOver()
{
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
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

