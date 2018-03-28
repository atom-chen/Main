#include "Shader.h"
#include "Vertex.h"


class GameObject
{
public:
	GameObject();
	void Init();
	void Draw(glm::mat4& viewMatrix, glm::mat4 &ProjectionMatrix);
protected:
private:
	Shader m_Shader;
	VertexBuffer m_VB;
	glm::mat4 m_ModelMatrix;
};




/*
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
if (!textShader.Init("res/test.vert", "res/text.frag"))
{
return 0;
}

texture = CreateTexture2DFromBMP("res/test.bmp");
ASSERT_INT_BOOL(texture);



textShader.Begin();
{
//Ϊ��������uniform����
textShader.SetMatrix(modelMatrix, viewMatrix, projectionMatrix);
textShader.SetTexture_2D(texture);
_
glBindBuffer(GLARRAY_BUFFER, vbo);
//���øò��
glEnableVertexAttribArray(textShader.GetPositionLocation());
glEnableVertexAttribArray(textShader.GetColorLocation());
glEnableVertexAttribArray(textShader.GetTexcoordLocation());

//����GPU,��ô��ȥ��ȡvbo���ڴ��
glVertexAttribPointer(textShader.GetPositionLocation(), 4, GL_FLOAT, GL_FALSE, sizeof(float) * 10, 0);//��۵�λ�ã�����������м�������(x,y,z,w)��ÿ��������ʲô���ͣ��Ƿ��һ����������֮��ľ��룬���õ���Ϣ��vbo��ɶ�ط���ʼȡֵ
//�����м����һ��position����ӦΪ�Լ���color+position�ľ��룻��ʼλ����position��߹�ӦΪ4��float�ľ���
glVertexAttribPointer(textShader.GetColorLocation(), 4, GL_FLOAT, GL_FALSE, sizeof(float) * 10, (void*)(sizeof(float) * 4));
glVertexAttribPointer(textShader.GetTexcoordLocation(), 2, GL_FLOAT, GL_FALSE, sizeof(float) * 10, (void*)(sizeof(float) * 8));

//����������-������������ GPU�������3�����ݣ�Ȼ�󴫵�3��shader,��3��shader�õ��˲�ͬ�ĵ㣬�������ǵ�����������һģһ����

//д��2����Ժ��
glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, ebo);
glDrawElements(GL_TRIANGLES, 3, GL_UNSIGNED_SHORT, 0);//��ʲô�������ٸ��������ݣ��������ͣ����ݵ���ʼλ��

//д��1������
//unsigned short arIndex[] ={0, 1, 2, 3, 4,5,6,7,8,9,10,11};
//glDrawElements(GL_TRIANGLES, 3, GL_UNSIGNED_SHORT, arIndex);//����ģʽ��ȡ���Σ��������ͣ��ֱ�ȡpos�����е���Щ�±�
}
textShader.End();

*/